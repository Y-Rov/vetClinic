using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.User;
using System.Drawing;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserUpdateMapper : IViewModelMapperUpdater<UserUpdateViewModel, User>
    {
        private readonly IUserProfilePictureService _userProfilePictureService;

        public UserUpdateMapper(IUserProfilePictureService userProfilePictureService)
        {
            _userProfilePictureService = userProfilePictureService;
        }

        public User Map(UserUpdateViewModel source)
        {
            return new User()
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate
            };
        }

        public void Map(UserUpdateViewModel source, User dest)
        {
            string profilePicture = GetProfilePicture(dest, source.ProfilePicture).Result;

            dest.FirstName = source.FirstName;
            dest.LastName = source.LastName;
            dest.PhoneNumber = source.PhoneNumber;
            dest.BirthDate = source.BirthDate;
            dest.ProfilePicture = profilePicture;
        }

        private async Task<string> GetProfilePicture(User user, string profilePicture)
        {
            var bytes = Convert.FromBase64String(profilePicture);

            using (MemoryStream ms = new(bytes))
            {
                var image = Image.FromStream(ms);
                var profilePictureLink = await _userProfilePictureService.UploadAsync(image, user.Email, "jpg");

                return profilePictureLink;
            }
        }
    }
}