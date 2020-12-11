using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper mapper;
        private readonly DataContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public UnitOfWork(DataContext context, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(context, mapper);
        public IPrinterRepository PrinterRepository => new PrinterRepository(context, mapper);

        public IMessageRepository MessageRepository => new MessageRepository(context, mapper);

        public ILikesRespository LikesRespository => new LikesRepository(context);

        public IAdminRepository AdminRepository => new AdminRepository(context, mapper, signInManager, userManager);
        public ICoursesRepository CoursesRepository => new CoursesRepository(context, mapper);

        public async Task<bool> Complete()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return this.context.ChangeTracker.HasChanges();
        }
    }
}