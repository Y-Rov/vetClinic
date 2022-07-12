using System.Drawing;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Repositories;

public interface IImageRepository
{
    public Task<string> UploadFromIFormFile(
        IFormFile file, 
        int authorId, 
        string folder, 
        string fileName = "");
    public Task DeleteAsync(string imageName, string folder);
}