using System;
using System.IO;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class FileService : IFileRepoService
    {
        private readonly FileRepoSettings fileRepo;
        public FileService(IOptions<FileRepoSettings> config)
        {
            this.fileRepo = new FileRepoSettings(config.Value.RepoDirectory, config.Value.PhotosDirectory, 
                                        config.Value.DocumentsDirectory, config.Value.UserFilesDirectory);            
        }

        //public async Task<bool> AddFileAsync(IFormFile file)
        public async Task<UserFile> AddFileAsync(IFormFile file)
        {
             //var uploadResult = new ImageUploadResult();

            if (file.Length > 0) 
            {
                //var filePath = _dataRepoConfig.Value.PhotosDirectory + user.Id + "-" + Guid.NewGuid() + "-" + file.FileName;

                string uniqueID = Guid.NewGuid().ToString();

                // /sis/userfiles
                //var filePath = fileRepo.RepoDirectory + "/" + fileRepo.UserFilesDirectory + "/" + uniqueID + "-" + file.FileName;
                var filePath = fileRepo.RepoDirectory + "/" + fileRepo.UserFilesDirectory + "/" + uniqueID;
                // + 
                // _dataRepoConfig.Value.PhotosDirectory + user.Id + "-" + Guid.NewGuid() + "-" + file.FileName;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    //file.CopyTo(fileStream);
                    fileStream.Close();
                    //return true;

                    var result = new UserFile {
                        //Id = 1;
                        Url = "",
                        Description = "",
                        DateAdded = DateTime.Now,
                        isThesis = false,
                        isProject = false,
                        isPrintJob = false,
                        isOther = false,
                        FilePath = filePath,
                        FileName = file.FileName,
                        StorageFileName = filePath,
                        PublicId = uniqueID
                        
                        // Fully Define the relationship
                        //public AppUser AppUser { get; set; }
                        //public int AppUserId { get; set; }  
                    };
                    return result;
                }

                

                // photoDto.FilePath = filePath;

                // Use repo dot add method
                //user.Photos.Add(photo);

                // using var stream = file.OpenReadStream();
                // var uploadParams = new ImageUploadParams {
                //     File = new FileDescription(file.FileName, stream),
                //     Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                // };
                // uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }
            else {
                return null;
                //return false;
            }

            // return uploadResult;
        }

        public void DeleteFileAsync(string publicId)
        {
            throw new System.NotImplementedException();
        }
    }
}