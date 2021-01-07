using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;

namespace API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly sshSettings sshServer;

        public AdminRepository(DataContext context, IMapper mapper, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, sshSettings sshServer)
        {
            this.sshServer = sshServer;
            this.userManager = userManager;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<PagedList<MemberAdminViewDto>> GetUsersPaginatedAccessList(UserParams userParams)
        {
            var query = context.Users.AsQueryable();

            // // Filter first
            if (!string.IsNullOrEmpty(userParams.SearchUser))
            {
                query = query.Where(u => u.UserName.Contains(userParams.SearchUser.ToString()));
            }

            // query = userParams.PrintStatus switch // New C# 8 switch expressions, no need for breaks 
            // {
            //     "Held" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Queued" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Cancelled" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Completed" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     _ => query.OrderBy(u => u.Id)               // Default case (show everything)
            // };

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap AppUser to MemberAdminViewDto
            return await PagedList<MemberAdminViewDto>.CreateAsync(query.ProjectTo<MemberAdminViewDto>(mapper
                .ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<UserFile> GetUserFileAsync(AppUser user, string publicId)
        {
            // this.context.Printers
            var query = this.context.UserFiles.AsQueryable();

            var userFile = await query.FirstOrDefaultAsync(u => u.PublicId.Contains(publicId));

            return userFile;
        }

        public async Task<bool> BatchCreateUsersFromCSV(UserFile userFile, SemesterDto semester)
        {
            using (var reader = new StreamReader(userFile.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                var records = csv.GetRecords<ClassListDto>();

                foreach (var record in records.ToList())
                {
                    var props = record.GetType().GetProperties();
                    // var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        //System.Console.WriteLine(p.Name + ": " + p.GetValue(record, null));
                        // sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));

                    }

                    // System.Console.WriteLine("!!!" + record.MAJOR);
                    // return sb.ToString();

                    // Check if it is a new user
                    if (!string.IsNullOrEmpty(record.STUDENT_ID)
                        && !await this.userManager.Users.AnyAsync(x => x.UserName == "siu" + record.STUDENT_ID))
                    {

                        // System.Console.WriteLine("siu" + record.STUDENT_ID + " doesn't exist.");

                        if (Int32.TryParse(record.STUDENT_ID, out int siuDawgTag))
                        {
                            var user = new AppUser
                            {
                                UserName = "siu" + record.STUDENT_ID.ToLower(),
                                KnownAs = record.STUDENT_NAME,
                                EmailSIU = record.INTERNET_ADDRESS,
                                PrimaryMajor = record.MAJOR,
                                PrimaryMajorProgram = record.PROGRAM,
                                DawgTag = siuDawgTag,
                                CLASS_LEVEL_BOAP = record.CLASS_LEVEL_BOAP,
                                EnrollmentStartYear = semester.Year,
                                EnrollmentStartTerm = semester.Term,
                                AccessPermitted = true
                            };

                            // Assign public ssh key
                            var keygen = new SshKeyGenerator.SshKeyGenerator(2048);
                            var privateKey = keygen.ToPrivateKey();
                            var publicSshKey = keygen.ToRfcPublicKey();

                            user.PrivateKeySSH1 = privateKey;
                            user.PublicKeySSH1 = publicSshKey;
                            user.PrivateKeySSH2 = privateKey;
                            user.PublicKeySSH2 = publicSshKey;

                            // Create web user personal url
                            System.Console.WriteLine(sshServer.Pc00host);
                            System.Console.WriteLine(sshServer.Pc00port);
                            System.Console.WriteLine(sshServer.Pc00user);
                            System.Console.WriteLine(sshServer.Pc00passwd);
                            var client = this.setupConnection(sshServer.Pc00host, sshServer.Pc00port, sshServer.Pc00user, sshServer.Pc00passwd);
                            string command = $"sudo -H -u {user.UserName} bash -c 'mkdir -p ~/.ssh; echo \"{user.PublicKeySSH1}\" > ~/.ssh/authorized_keys; chmod 644 ~/.ssh/authorized_keys; echo \"{user.PublicKeySSH1}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{user.PrivateKeySSH1}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
                            var retCode = await this.RunClientCommandAsync(client, command);

                            client.Disconnect();
                            
                            if (retCode != 0)
                                return false;

                            client = this.setupConnection(sshServer.Pc01host, sshServer.Pc01port, sshServer.Pc01user, sshServer.Pc01passwd);
                            command = $"sudo -H -u {user.UserName} bash -c 'mkdir -p ~/.ssh; echo \"{user.PublicKeySSH1}\" > ~/.ssh/authorized_keys; chmod 644 ~/.ssh/authorized_keys; echo \"{user.PublicKeySSH1}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{user.PrivateKeySSH1}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
                            retCode = await this.RunClientCommandAsync(client, command);

                            client.Disconnect();
                            
                            if (retCode != 0)
                                return false;

                            // Transfer keys to server for their Personal URL User
                            // if( ! await this.accountService.IssueUserWebKeysAsync(user.PersonalURL, publicSshKey, privateKey)) {
                            //     return false;
                            // }
                            // End assign public ssh key

                            var tempPassword = "Pa$$w0rd";
                            var result = await userManager.CreateAsync(user, tempPassword);

                            if (!result.Succeeded)
                                return false;

                            var roleResult = await userManager.AddToRoleAsync(user, "Member");

                            if (!roleResult.Succeeded)
                                return false;

                            System.Console.WriteLine("siu" + record.STUDENT_ID + " added.");
                        }
                    }
                }
                return true;
            }
        }




        public async Task<bool> BatchAllowLoginFromCSV(UserFile userFile)
        {
            using (var reader = new StreamReader(userFile.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                var records = csv.GetRecords<ClassListDto>();

                foreach (var record in records.ToList())
                {
                    var props = record.GetType().GetProperties();
                    // var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        //System.Console.WriteLine(p.Name + ": " + p.GetValue(record, null));
                        // sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
                    }
                    // System.Console.WriteLine("!!!" + record.MAJOR);
                    // return sb.ToString();

                    // Confirm user exists
                    if (!string.IsNullOrEmpty(record.STUDENT_ID)
                        && await this.userManager.Users.AnyAsync(x => x.UserName == "siu" + record.STUDENT_ID))
                    {

                        // var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
                        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == "siu" + record.STUDENT_ID);

                        if (user.Equals(null))
                            return false;

                        if (!user.AccessPermitted)
                        {
                            user.AccessPermitted = true;
                            Update(user);
                            System.Console.WriteLine("siu" + record.STUDENT_ID + " access permitted.");
                        }

                        ///
                        // Transfer keys to server
                        // if( ! await this.accountService.IssueUserKeysAsync(user.UserName, user.PublicKeySSH1, user.PrivateKeySSH1)) {
                        //     return false;
                        // }

                        // Transfer keys to server for their Personal URL User
                        // if( ! await this.accountService.IssueUserWebKeysAsync(user.PersonalURL, user.PublicKeySSH1, user.PrivateKeySSH1)) {
                        //     return false;
                        // }
                        ///
                    }
                }
                return true;
            }
        }

        public async Task<bool> BatchDisableLoginFromCSV(UserFile userFile)
        {
            using (var reader = new StreamReader(userFile.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                var records = csv.GetRecords<ClassListDto>();

                foreach (var record in records.ToList())
                {
                    var props = record.GetType().GetProperties();
                    // var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        //System.Console.WriteLine(p.Name + ": " + p.GetValue(record, null));
                        // sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
                    }
                    // System.Console.WriteLine("!!!" + record.MAJOR);
                    // return sb.ToString();

                    // Confirm user exists
                    if (!string.IsNullOrEmpty(record.STUDENT_ID)
                        && await this.userManager.Users.AnyAsync(x => x.UserName.Equals("siu" + record.STUDENT_ID)))
                    {

                        // var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
                        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals("siu" + record.STUDENT_ID));

                        if (user.Equals(null))
                            return false;

                        if (user.AccessPermitted)
                        {
                            user.AccessPermitted = false;

                            Update(user);
                            // System.Console.WriteLine("siu" + record.STUDENT_ID + " access restricted.");
                        }
                    }
                }
                return true;
            }
        }



        public async Task<bool> BatchUpdateMajorFromCSV(UserFile userFile)
        {
            using (var reader = new StreamReader(userFile.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                var records = csv.GetRecords<ClassListDto>();

                foreach (var record in records.ToList())
                {
                    var props = record.GetType().GetProperties();
                    // var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        //System.Console.WriteLine(p.Name + ": " + p.GetValue(record, null));
                        // sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
                    }
                    // System.Console.WriteLine("!!!" + record.MAJOR);
                    // return sb.ToString();

                    // Confirm user exists
                    if (!string.IsNullOrEmpty(record.STUDENT_ID)
                        && await this.userManager.Users.AnyAsync(x => x.UserName == "siu" + record.STUDENT_ID))
                    {

                        // var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
                        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == "siu" + record.STUDENT_ID);

                        if (user.Equals(null))
                            return false;

                        if (!user.PrimaryMajor.Equals(record.MAJOR)
                            || !user.PrimaryMajorProgram.Equals(record.PROGRAM))
                        {
                            user.PrimaryMajor = record.MAJOR;
                            user.PrimaryMajorProgram = record.PROGRAM;
                            Update(user);
                            System.Console.WriteLine("siu" + record.STUDENT_ID + " major updated.");
                        }
                    }
                }
                return true;
            }
        }

        public void Update(AppUser user)
        {
            // Lets EF update and add a flag to the entity to let it know that yep thats been modified
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<int> RunClientCommandAsync(SshClient client, string command) {
            int retCode = -1;
            try {
                using (var cmd = client.CreateCommand(command))
                {
                    var cmdResult = await Task.FromResult<string>(cmd.Execute());
                    
                    // Console.WriteLine("Command>" + cmd.CommandText);
                    // Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                    // Console.WriteLine("Result Value = {0}", cmd.Result);
                    // if (cmd.ExitStatus != 0)
                    // {
                    //     Console.WriteLine("Command failed!!!!");
                    // }

                    retCode = cmd.ExitStatus;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception");
                Console.WriteLine(ex);
            }
            finally {
            }

            return retCode;
            // return Task.FromResult<int>(retCode);
        }

        public SshClient setupConnection(string host, int port, string username, string password)
        {
            ConnectionInfo ConnNfo = new ConnectionInfo(host, port, username,
                   new AuthenticationMethod[]{
                        // Pasword based Authentication
                        new PasswordAuthenticationMethod(username, password),

                        /*
                        // Key Based Authentication (using keys in OpenSSH Format)
                        new PrivateKeyAuthenticationMethod("username",new PrivateKeyFile[]{
                            new PrivateKeyFile(@"..\openssh.key","passphrase")
                        }), 
                        */
                    }
                );

            var sshclient = new SshClient(ConnNfo);

            try
            {
                sshclient.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception");
                Console.WriteLine(ex);
            }
            
            // return Task.FromResult<SshClient>(sshclient);
            return sshclient;

        }


    }
}