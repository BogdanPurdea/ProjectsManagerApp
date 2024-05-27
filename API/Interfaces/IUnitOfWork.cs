using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IMessageRepository MessageRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IFileRepository FileRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}