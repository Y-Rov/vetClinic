using System.Drawing;

namespace Core.Interfaces.Repositories
{
    public interface IUserProfilePictureRepository
    {
        Task<string> UploadAsync(Image image, string email, string imageFormat);
        Task DeleteAsync(string imageLink);
    }
}
