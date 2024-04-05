using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ProjectsController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProjectsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromQuery]ProjectParams projectParams)
        {
            var userId = User.GetUserId();

            var projects = await unitOfWork.ProjectRepository.GetProjectsAsync(projectParams, userId);

            Response.AddPaginationHeader(projects.CurrentPage, projects.PageSize, projects.TotalCount, projects.TotalPages);

            return Ok(projects);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            var creatorName = User.GetUserName();
            var project = await unitOfWork.ProjectRepository.GetProjectAsync(id);
            project.CreatorName = creatorName;
            return project;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectDto projectDto)
        {
            var userName = User.GetUserName();

            var creator = await unitOfWork.UserRepository.GetUserByUserNameAsync(userName);

            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                ProjectDescription = projectDto.ProjectDescription,
                Creator = creator,
                IsFinished = false,
                IsApproved = false,
                Contributors = new List<AppUser> {creator}
            };

            unitOfWork.ProjectRepository.Add(project);

            var result = mapper.Map<ProjectDto>(project);
            result.CreatorName = creator.UserName;

            if(await unitOfWork.Complete()) return Ok(result);

            return BadRequest("Failed to create project");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProject(int id, ProjectUpdateDto projectUpdateDto)
        {
            var project = await unitOfWork.ProjectRepository.GetProjectByIdAsync(id);

            mapper.Map(projectUpdateDto, project);

            unitOfWork.ProjectRepository.Update(project);

            if(await unitOfWork.Complete()) return NoContent();
            
            return BadRequest("Failed to update project");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var project = await unitOfWork.ProjectRepository.GetProjectByIdAsync(id);

            if(project==null)
                return NotFound();

            unitOfWork.ProjectRepository.Delete(project);

            if(await unitOfWork.Complete()) return Ok();

            return BadRequest("Could not delete project");
        }

    }
}