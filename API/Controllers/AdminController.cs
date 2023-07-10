using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .Include(r => r.UserRoles!)
                .ThenInclude(r => r.Role)
                .IgnoreQueryFilters()
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    UserName = u.UserName,
                    Roles = u.UserRoles!.Select(r => r.Role!.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }
        
        [HttpPost("edit-roles/{userName}")]
        public async Task<ActionResult> EditRoles(string userName, [FromQuery]string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(userName);

            if(user == null) return NotFound("Could not find user");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            
            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            return Ok(await unitOfWork.PhotoRepository.GetUnapprovedPhotosAsync());
        }

        [HttpPut("approve-photo/{id}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoByIdAsync(id);
            var user = await unitOfWork.UserRepository.GetUserByPhotoAsync(photo);
            photo.IsApproved = "approved";
            if(user.Photos!.Select(p => p.IsMain == true).ToList().Count == 0)
                photo.IsMain = true;
            return Ok(await unitOfWork.Complete());
        }

        [HttpPut("reject-photo/{id}")]
        public async Task<ActionResult> RejectPhoto(int id)
        {
            var photo = await unitOfWork.PhotoRepository.GetPhotoByIdAsync(id);
            photo.IsApproved = "rejected";
            return Ok(await unitOfWork.Complete());
        }
    }
}