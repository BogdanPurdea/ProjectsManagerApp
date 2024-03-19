using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IProjectRepository
    {
        void Update(Project project);
        void Add(Project project);
        void Delete(Project project);
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> GetProjectByProjectNameAsync(string projectName);
        Task<PagedList<ProjectDto>> GetProjectsAsync(ProjectParams projectParams, int userId);
        Task<ProjectDto> GetProjectAsync(int id);
    }
}