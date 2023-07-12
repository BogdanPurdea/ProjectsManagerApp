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
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
            var projects = await unitOfWork.ProjectRepository.GetProjectsAsync(projectParams);

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
            };

            unitOfWork.ProjectRepository.Add(project);

            var result = mapper.Map<ProjectDto>(project);
            result.CreatorName = creator.UserName;

            if(await unitOfWork.Complete()) return Ok(result);

            return BadRequest("Failed to create project");
        }
    }
}