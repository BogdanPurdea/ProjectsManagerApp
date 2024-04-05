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
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public PhotoRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await context.Photos.FindAsync(id);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotosAsync()
        {
            return await context.Photos
                .Include(p => p.AppUser)
                .Where(p => p.IsApproved == "unapproved")
                .ProjectTo<PhotoForApprovalDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            context.Photos.Remove(photo);
        }
    }
}