using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.User;
using System.Drawing;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserCreateMapper : IViewModelMapper<UserCreateViewModel, User>
    {
        private readonly IUserProfilePictureService _userProfilePictureService;

        public UserCreateMapper(IUserProfilePictureService userProfilePictureService)
        {
            _userProfilePictureService = userProfilePictureService;
        }

        public User Map(UserCreateViewModel source)
        {
            string? profilePicture = GetProfilePicture(source).Result;

            return new User()
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                UserName = source.Email,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate,
                ProfilePicture = profilePicture
            };
        }

        private async Task<string?> GetProfilePicture(UserCreateViewModel createViewModel)
        {
            byte[]? bytes;

            if (createViewModel.ProfilePicture is not null)
            {
                bytes = Convert.FromBase64String(createViewModel.ProfilePicture);
            }
            else
            {
                string defaultProfilePicPath = "../WebApi/Assets/Images/default_profile_pic.jpg";
                bytes = await File.ReadAllBytesAsync(defaultProfilePicPath);
            }

            using (MemoryStream ms = new(bytes))
            {
                var image = Image.FromStream(ms);
                var profilePictureLink = await _userProfilePictureService.UploadAsync(image, createViewModel.Email!, "jpg");

                return profilePictureLink;
            }
        }
    }
}