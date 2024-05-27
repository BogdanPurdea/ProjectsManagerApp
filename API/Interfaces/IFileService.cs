using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IFileService
    {
        Task<RawUploadResult> AddFileAsync(IFormFile file);
        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}