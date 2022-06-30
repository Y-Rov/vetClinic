using System.Drawing;

namespace Core.Interfaces.Services;

public interface IArticleImageManager
{
    public Task<string> UploadAsync(Image image, string imageFormat);

}