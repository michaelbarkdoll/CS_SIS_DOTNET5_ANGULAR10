using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPrinterRepository PrinterRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRespository LikesRespository { get; }
        IAdminRepository AdminRepository { get; }
        ICoursesRepository CoursesRepository { get; }
        IDockerRepository DockerRepository { get; }
        // Method to save all of changes
        Task<bool> Complete();
        // Used to see if EF has any changes
        bool HasChanges();
    }
}