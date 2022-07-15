using Azure;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System.Drawing;

namespace Application.Services
{
    public class UserProfilePictureService : IUserProfilePictureService
    {
        private readonly IUserProfilePictureRepository _userProfilePictureRepository;
        private readonly ILoggerManager _loggerManager;

        public UserProfilePictureService(
            IUserProfilePictureRepository userProfilePictureRepository, 
            ILoggerManager loggerManager)
        {
            _userProfilePictureRepository = userProfilePictureRepository;
            _loggerManager = loggerManager;
        }

        public async Task DeleteAsync(string imageLink)
        {
            try
            {
                await _userProfilePictureRepository.DeleteAsync(imageLink);
            }
            catch(RequestFailedException)
            {
                _loggerManager.LogWarn("The image with the provided link could no be deleted");
                throw new BadRequestException("The image with the provided link could no be deleted");
            }
        }

        public async Task<string> UploadAsync(Image image, string email, string imageFormat)
        {
            string fileName;

            try
            {
                fileName = await _userProfilePictureRepository.UploadAsync(image, email, imageFormat);
            }
            catch(RequestFailedException)
            {
                _loggerManager.LogWarn("The image with the provided link could no be uploaded");
                throw new BadRequestException("The image with the provided link could no be uploaded");
            }

            return fileName;
        }
    }
}
