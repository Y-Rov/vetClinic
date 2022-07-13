using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Repositories;

public interface IImageRepository
{
    public Task<string> UploadFromIFormFile(IFormFile file, string folder, string fileName = "");
    public Task DeleteAsync(string imageName, string folder);
}