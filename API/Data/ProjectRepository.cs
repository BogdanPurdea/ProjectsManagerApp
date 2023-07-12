using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public ProjectRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void Add(Project project)
        {
            context.Projects.Add(project);
        }

        public void Delete(Project project)
        {
            context.Projects.Remove(project);
        }

        public async Task<ProjectDto> GetProjectAsync(int id)
        {
            return await context.Projects
                .Where(p => p.Id == id)
                .ProjectTo<ProjectDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await context.Projects.FindAsync(id);
        }

        public async Task<Project> GetProjectByProjectNameAsync(string projectName)
        {
            return await context.Projects
                .Where(p => p.ProjectName == projectName)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await context.Projects.ToListAsync();
        }

        public async Task<PagedList<ProjectDto>> GetProjectsAsync(ProjectParams projectParams)
        {
            var query = context.Projects.AsQueryable();

            query = projectParams.OrderBy switch
            {
                "alphabetically" => query.OrderBy(p => p.ProjectName),
                _ => query.OrderByDescending(p => p.Id)
            };

            return await PagedList<ProjectDto>.CreateAsync(query.ProjectTo<ProjectDto>(mapper
            .ConfigurationProvider).AsNoTracking(), projectParams.PageNumber, projectParams.PageSize);
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int id)
        {
            return await context.Projects
                .Where(p => p.Creator.Id == id)
                .ToListAsync();
        }

        public void Update(Project project)
        {
            context.Entry(project).State = EntityState.Modified;
        }
    }
}