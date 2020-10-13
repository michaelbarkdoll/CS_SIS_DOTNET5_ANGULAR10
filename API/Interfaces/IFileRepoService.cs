using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IFileRepoService
    {
        //Task<IActionResult> AddPhotoAsync(IFormFile file);
        //void AddFileAsync(IFormFile file);
        //Task<IActionResult> DeletePhotoAsync(string publicId);
        //Task<bool> AddFileAsync(IFormFile file);
        Task<UserFile> AddFileAsync(IFormFile file);
        void DeleteFileAsync(string publicId);
    }
}