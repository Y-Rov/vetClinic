using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Repositories;

public interface IImageRepository
{
    Task<string> UploadFromIFormFile(IFormFile file, string folder, string fileName = "");
    Task DeleteAsync(string imageName, string folder);
}