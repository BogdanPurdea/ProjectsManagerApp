using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize]
    public class ProjectsController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        public ProjectsController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromQuery]ProjectParams projectParams)
        {
            var userId = User.GetUserId();

            var projects = await unitOfWork.ProjectRepository.GetProjectsAsync(projectParams, userId);

            Response.AddPaginationHeader(projects.CurrentPage, projects.PageSize, projects.TotalCount, projects.TotalPages);

            return Ok(projects);
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
        {
            return await unitOfWork.ProjectRepository.GetProjectAsync(id);
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

        [HttpPost("add-file/{projectId}")]
        public async Task<ActionResult<ProjectFileDto>> AddFile(int projectId, IFormFile file)
        {
            var project = await unitOfWork.ProjectRepository.GetProjectByIdAsync(projectId);

            var result = await fileService.AddFileAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var projectFile = new ProjectFile
            {
                Name = file.FileName,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                Size = file.Length,
                Type = file.GetType().ToString(),
                LastModified = DateTime.Now,
            };
            if(project.Files.IsNullOrEmpty())
                project.Files = new List<ProjectFile>();
            project.Files?.Add(projectFile);

            if(await unitOfWork.Complete())
            {
                return CreatedAtRoute("GetProject", new{id = project.Id}, mapper.Map<ProjectFileDto>(projectFile));
            }

            return BadRequest("Problem adding file");
        }

        [HttpDelete("delete-file/{fileId}")]
        public async Task<ActionResult> DeletePhoto(int fileId)
        {
            var projectFile = await unitOfWork.FileRepository.GetFileByIdAsync(fileId);

            if(projectFile == null) return NotFound();

            if(projectFile.PublicId != null)
            {
                var result = await fileService.DeleteFileAsync(projectFile.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            unitOfWork.FileRepository.RemoveFile(projectFile);

            if(await unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the file");
        }
    }
}