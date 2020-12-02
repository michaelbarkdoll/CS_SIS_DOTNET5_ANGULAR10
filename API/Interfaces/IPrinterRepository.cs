using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPrinterRepository
    {
        Task<AppUser> GetUserByIdPrintJobAsync(int id);
        Task<AppUser> GetUserByUsernamePrintJobAsync(string username);
        Task<IEnumerable<PrinterDto>> GetPrintersAsync();
        Task<MemberPrintJobDto> GetMemberPrintJobsAsync(string username);
        Task<MemberPrintQuotaDto> GetMemberPrintQuotaAsync(int id);
        Task<PagedList<PrintJobDto>> GetMemberPrintJobsAsync(UserParams userParams);
        Task<PagedList<PrintJobDto>> GetMembersPrintJobsAsync(UserParams userParams);
        Task<PagedList<PrinterDto>> GetPagedPrintersAsync(UserParams userParams);
        
    }
}