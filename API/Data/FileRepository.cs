using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public FileRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ProjectFile> GetFileByIdAsync(int id)
        {
            return await context.Files.FindAsync(id);
        }

        public void RemoveFile(ProjectFile File)
        {
            context.Files.Remove(File);
        }
    }
}