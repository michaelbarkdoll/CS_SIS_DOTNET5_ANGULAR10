using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPrinterRepository PrinterRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRespository LikesRespository { get; }
        // Method to save all of changes
        Task<bool> Complete();
        // Used to see if EF has any changes
        bool HasChanges();
    }
}