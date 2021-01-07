using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper mapper;
        private readonly DataContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly sshSettings sshServer;
        
        
        public UnitOfWork(DataContext context, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<sshSettings> config)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
            this.sshServer = new sshSettings(config.Value.Hostname, config.Value.Port, 
                                    config.Value.Www2host, config.Value.Www2port,
                                    config.Value.Www2user, config.Value.Www2passwd,
                                    config.Value.Pc00host, config.Value.Pc00port,
                                    config.Value.Pc00user, config.Value.Pc00passwd,
                                    config.Value.Pc01host, config.Value.Pc01port,
                                    config.Value.Pc01user, config.Value.Pc01passwd);
        }

        public IUserRepository UserRepository => new UserRepository(context, mapper);
        public IPrinterRepository PrinterRepository => new PrinterRepository(context, mapper);

        public IMessageRepository MessageRepository => new MessageRepository(context, mapper);

        public ILikesRespository LikesRespository => new LikesRepository(context);

        public IAdminRepository AdminRepository => new AdminRepository(context, mapper, signInManager, userManager, sshServer);
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