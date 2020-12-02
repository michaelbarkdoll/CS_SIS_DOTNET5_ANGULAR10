using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PrinterController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IFileRepoService fileRepoService;
        private readonly DataContext context;
        public PrinterController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, IFileRepoService fileRepoService, DataContext context)
        {
            this.context = context;
            this.fileRepoService = fileRepoService;
            this.photoService = photoService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        //MemberPrintQuotaDto

        [HttpGet("get-user-printquota", Name = "GetUserPrintQuota")]
        // public async Task<ActionResult<MemberFileDto>> GetUserPrintJobs(string username) 
        public async Task<ActionResult<MemberPrintQuotaDto>> GetUserPrintQuota() 
        {
            var sourceUserId = User.GetUserId();
            //var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);
            // return Ok(sourceUser);

            var sourceUser = await this.unitOfWork.PrinterRepository.GetMemberPrintQuotaAsync(sourceUserId);
            return Ok(sourceUser);
        }


        [HttpGet("get-user-printjobs/{username}", Name = "GetUserPrintJobs")]
        // public async Task<ActionResult<MemberFileDto>> GetUserPrintJobs(string username) 
        public async Task<ActionResult<MemberPrintJobDto>> GetUserPrintJobs(string username) 
        {

            var sourceUserId = User.GetUserId();

            var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);
            
            // var sourceUser = await this.unitOfWork.UserRepository.GetUserByIdPrintJobAsync(sourceUserId);
            // var sourceUser = await this.unitOfWork.UserRepository.GetUserByUsernamePrintJobAsync(username);
            

            /******************************************************
            // Add a printer
            var printer = new Printer{ 
                Id = 1,
                PrinterName = "Test",
                URL = "None",
                port = 631,
                SshUsername = "root",
                SshPassword = "root",
                SshHostname = "localhost",
                SshPublicKey = "none"
            };
            // A printjob must be assigned to a printer?

            
            this._context.Printers.Add(printer); // .Printers.Add(printer);       // Just tracking in EF
            // await this._context.SaveChangesAsync();  // Saves changes to Entity Framework
            ******************************************************/
            
            /******************************************************
            // Add a print job
            *******************************************************
            var pp = await this._context.Printers.FindAsync(1);
            
            var thing = new PrintJob();
            thing.JobNumber = 7;
            thing.JobOwner = "Bob";
            thing.JobName = "thing7";
            thing.JobStatus = "Held";
            thing.PrinterName = "test";
            thing.NumberOfPages = 1;
            thing.PagesPrinted = 0;
            // thing.Printer = printer;
            thing.Printer = pp;
            thing.AppUserId = sourceUserId;

            sourceUser.PrintJobs.Add(thing);
            this.unitOfWork.UserRepository.Update(sourceUser);
            ******************************************************/

            // sourceUser.PrintJobs = new PrintJob {
                
            // };

            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            //return await this.unitOfWork.UserRepository.GetMemberAsync(username);
            // return await this.unitOfWork.UserRepository.GetMemberFilesAsync(username);
            
            return await this.unitOfWork.PrinterRepository.GetMemberPrintJobsAsync(username);
            // return await this.unitOfWork.UserRepository.GetMemberPrintJobsAsync(username);

            //GetUserByUsernamePrintJobAsync
            // return await this.unitOfWork.UserRepository.GetUserByUsernamePrintJobAsync(username);
            


            //return await _context.Users.FindAsync(id);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-user-by-name-paged-printjobs/{username}", Name = "GetUserByNamePagedPrintJobs")]
        // public async Task<ActionResult<IEnumerable<MemberPrintJobDto>>> GetUserPagniatedPrintJobs([FromQuery]UserParams userParams) 
        public async Task<ActionResult<MemberPrintJobDto>> GetUserByNamePagniatedPrintJobs([FromQuery]UserParams userParams) 
        {
            // var users = await this.unitOfWork.UserRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //var users = await this.unitOfWork.UserRepository.GetMembersAsync();

            // var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // var gender = await this.unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUserName = User.GetUsername();    // From Token
            // userParams.CurrentUserName = user.UserName;

            // if (string.IsNullOrEmpty(userParams.Gender))
            //     userParams.Gender = gender == "male" ? "female" : "male";

            // Todo:
            // Change GetMemberPrintJobsAsync to GetMemberByNamePrintJobsAsync(userParams, username) must pass in inquirying user
            var printJobs = await this.unitOfWork.PrinterRepository.GetMemberPrintJobsAsync(userParams);
            // var users = await this.unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(printJobs.CurrentPage, printJobs.PageSize, printJobs.TotalCount, printJobs.TotalPages);

            return Ok(printJobs);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        [HttpGet("get-user-paged-printjobs", Name = "GetUserPagedPrintJobs")]
        // public async Task<ActionResult<IEnumerable<MemberPrintJobDto>>> GetUserPagniatedPrintJobs([FromQuery]UserParams userParams) 
        public async Task<ActionResult<MemberPrintJobDto>> GetUserPagniatedPrintJobs([FromQuery]UserParams userParams) 
        {
            // var users = await this.unitOfWork.UserRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //var users = await this.unitOfWork.UserRepository.GetMembersAsync();

            // var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // var gender = await this.unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUserName = User.GetUsername();    // From Token
            // userParams.CurrentUserName = user.UserName;

            // if (string.IsNullOrEmpty(userParams.Gender))
            //     userParams.Gender = gender == "male" ? "female" : "male";

            var printJobs = await this.unitOfWork.PrinterRepository.GetMemberPrintJobsAsync(userParams);
            // var users = await this.unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(printJobs.CurrentPage, printJobs.PageSize, printJobs.TotalCount, printJobs.TotalPages);

            return Ok(printJobs);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-users-paged-printjobs", Name = "GetUsersPagedPrintJobs")]
        // public async Task<ActionResult<IEnumerable<MemberPrintJobDto>>> GetUserPagniatedPrintJobs([FromQuery]UserParams userParams) 
        public async Task<ActionResult<MemberPrintJobDto>> GetUsersPagniatedPrintJobs([FromQuery]UserParams userParams) 
        {
            // var users = await this.unitOfWork.UserRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //var users = await this.unitOfWork.UserRepository.GetMembersAsync();

            // var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // var gender = await this.unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUserName = User.GetUsername();    // From Token
            // userParams.CurrentUserName = user.UserName;

            // if (string.IsNullOrEmpty(userParams.Gender))
            //     userParams.Gender = gender == "male" ? "female" : "male";

            var printJobs = await this.unitOfWork.PrinterRepository.GetMembersPrintJobsAsync(userParams);
            // var users = await this.unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(printJobs.CurrentPage, printJobs.PageSize, printJobs.TotalCount, printJobs.TotalPages);

            return Ok(printJobs);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-print-job/{printJobId}")]
        public async Task<ActionResult> DeletePrintJob(int printJobId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printJob = await this.context.PrintJobs.FindAsync(printJobId);

            if (printJob == null)
                return NotFound();

            this.context.PrintJobs.Remove(printJob);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to delete the print job");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("pause-print-job/{printJobId}")]
        public async Task<ActionResult> PausePrintJob(int printJobId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printJob = await this.context.PrintJobs.FindAsync(printJobId);

            if (printJob == null)
                return NotFound();

            if(printJob.JobStatus == null || !printJob.JobStatus.Equals("Held") ) {
                printJob.JobStatus = "Held";
            } else {
                // return Ok();
                return BadRequest("Print Job is already paused");
            }

            this.context.PrintJobs.Update(printJob);
            

            // this.context.PrintJobs.Remove(printJob);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to pause the print job");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("queue-print-job/{printJobId}")]
        public async Task<ActionResult> QueuePrintJob(int printJobId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printJob = await this.context.PrintJobs.FindAsync(printJobId);

            if (printJob == null)
                return NotFound();

            if(printJob.JobStatus == null || !printJob.JobStatus.Equals("Queued")) {
                printJob.JobStatus = "Queued";
            } else {
                // return Ok();
                return BadRequest("Print Job is already in queue");
            }

            this.context.PrintJobs.Update(printJob);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to queue the print job");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("complete-print-job/{printJobId}")]
        public async Task<ActionResult> CompletePrintJob(int printJobId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printJob = await this.context.PrintJobs.FindAsync(printJobId);

            if (printJob == null)
                return NotFound();

            if(printJob.JobStatus == null || !printJob.JobStatus.Equals("Completed")) {
                printJob.JobStatus = "Completed";
            } else {
                // return Ok();
                return BadRequest("Print Job has already completed");
            }

            this.context.PrintJobs.Update(printJob);
            

            // this.context.PrintJobs.Remove(printJob);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to complete the print job");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("cancel-print-job/{printJobId}")]
        public async Task<ActionResult> CancelPrintJob(int printJobId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printJob = await this.context.PrintJobs.FindAsync(printJobId);

            if (printJob == null)
                return NotFound();

            if(printJob.JobStatus == null || !printJob.JobStatus.Equals("Cancelled")) {
                printJob.JobStatus = "Cancelled";
            } else {
                // return Ok();
                return BadRequest("Print Job has already been cancelled");
            }

            this.context.PrintJobs.Update(printJob);
            

            // this.context.PrintJobs.Remove(printJob);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to cancel the print job");
        }

        // [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-user-printers/{username}", Name = "GetUserPrinters")]
        public async Task<ActionResult<IEnumerable<PrinterDto>>> GetUserPrinters(string username) 
        {
            // var sourceUserId = User.GetUserId();
            
            // var sourceUser = await this.unitOfWork.UserRepository.GetUserByIdPrintJobAsync(sourceUserId);

            // Add a print job
            // var pp = await this._context.Printers.FindAsync(1);
            // var printerList = await this.unitOfWork.UserRepository.GetPrintersAsync(); //GetPrintersAsync
            var printerList = await this.unitOfWork.PrinterRepository.GetPrintersAsync(); //GetPrintersAsync
            // System.Console.WriteLine(pp.PrinterName + " " + pp.Id + " " + pp.port + " " + pp.SshHostname + " " + pp.SshPassword + " " + pp.SshPublicKey + " " + pp.SshUsername + " " + pp.URL);
            return Ok(printerList); 

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-printers", Name = "GetPrinters")]
        public async Task<ActionResult<IEnumerable<PrinterDto>>> GetPrinters() 
        {
            // var printer1 = await this._context.Printers.FindAsync(1);
            
            var printerList = await this.unitOfWork.PrinterRepository.GetPrintersAsync(); //GetPrintersAsync
            // System.Console.WriteLine(printer1.PrinterName + " " + printer1.Id + " " + printer1.port + " " + printer1.SshHostname + " " + printer1.SshPassword + " " + printer1.SshPublicKey + " " + printer1.SshUsername + " " + printer1.URL);
            return Ok(printerList); 

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-paged-printers", Name = "GetPagedPrinters")]
        // public async Task<ActionResult<IEnumerable<PrinterDto>>> GetPagedPrinters() 
        public async Task<ActionResult<PrinterDto>> GetPagedPrinters([FromQuery]UserParams userParams) 
        {
            // var printer1 = await this._context.Printers.FindAsync(1);
            userParams.CurrentUserName = User.GetUsername();    // From Token
            var printersList = await this.unitOfWork.PrinterRepository.GetPagedPrintersAsync(userParams);
            
            Response.AddPaginationHeader(printersList.CurrentPage, printersList.PageSize, printersList.TotalCount, printersList.TotalPages);

            return Ok(printersList);
            // System.Console.WriteLine(printer1.PrinterName + " " + printer1.Id + " " + printer1.port + " " + printer1.SshHostname + " " + printer1.SshPassword + " " + printer1.SshPublicKey + " " + printer1.SshUsername + " " + printer1.URL);
           

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add-printer", Name = "AddPrinter")]
        // public async Task<ActionResult<MemberFileDto>> GetUserPrintJobs(string username) 
        public async Task<ActionResult<PrinterDto>> AddPrinter(PrinterDto printer) 
        {

            // var sourceUserId = User.GetUserId();

            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);
            
            // var sourceUser = await this.unitOfWork.UserRepository.GetUserByIdPrintJobAsync(sourceUserId);
            // var sourceUser = await this.unitOfWork.UserRepository.GetUserByUsernamePrintJobAsync(username);
            
            var addPrinter = new Printer{ 
                Id = printer.Id,
                PrinterName = printer.PrinterName,
                URL = printer.URL,
                Port = printer.port,
                SshUsername = printer.SshUsername,
                SshPassword = printer.SshPassword,
                SshHostname = printer.SshHostname,
                SshPublicKey = printer.SshPublicKey
            };
            
            // Add a printer
            // var addPrinter2 = new Printer{ 
            //     Id = 1,
            //     PrinterName = "Test",
            //     URL = "None",
            //     port = 631,
            //     SshUsername = "root",
            //     SshPassword = "root",
            //     SshHostname = "localhost",
            //     SshPublicKey = "none"
            // };

            
            this.context.Printers.Add(addPrinter); // .Printers.Add(printer);       // Just tracking in EF
            // await this.context.SaveChangesAsync();  // Saves changes to Entity Framework

            if (await unitOfWork.Complete()) 
            {
                return (mapper.Map<PrinterDto>(addPrinter));
                // return CreatedAtRoute("GetPrinters", mapper.Map<PrinterDto>(addPrinter));

                // Now we're return the route to get the photos and the photo object
                //return CreatedAtRoute("GetUser", mapper.Map<PhotoDto>(photo));
                //return CreatedAtRoute("GetUser", new {username = user.UserName}, mapper.Map<PhotoDto>(photo));
                // return CreatedAtRoute("GetPrinters", new {}, mapper.Map<PrinterDto>(addPrinter));
                // return CreatedAtRoute("GetPrinters", mapper.Map<PrinterDto>(addPrinter));
            }

            return BadRequest("Problem adding printer");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-printer/{printerId}")]
        public async Task<ActionResult> DeletePrinter(int printerId) {
            // var sourceUserId = User.GetUserId();
            // var sourceUser = await this.unitOfWork.PrinterRepository.GetUserByIdPrintJobAsync(sourceUserId);

            var printer = await this.context.Printers.FindAsync(printerId);

            if (printer == null)
                return NotFound();

            this.context.Printers.Remove(printer);

            if (await unitOfWork.Complete()) {
                return Ok();
                // return RedirectToRoute("GetPrinters");   // Returns printers array
            }
            
            return BadRequest("Failed to delete the printer");
        }
    }
}