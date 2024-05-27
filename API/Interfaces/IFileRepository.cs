using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IFileRepository
    {
        Task<ProjectFile> GetFileByIdAsync(int id);
        void RemoveFile(ProjectFile file);
    }
}