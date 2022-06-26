using Core.Entities;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserCreateMapper : IViewModelMapper<UserCreateViewModel, User>
    {
        public User Map(UserCreateViewModel source)
        {
            byte[] profilePicture = GetProfilePicture(source.ProfilePicture);

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

        private static byte[] GetProfilePicture(string sourcePicture)
        {
            byte[] profilePicture;

            if (!string.IsNullOrEmpty(sourcePicture))
            {
                profilePicture = Convert.FromBase64String(sourcePicture);
            }
            else
            {
                string defaultProfilePicPath = $"../WebApi/Images/default_profile_pic.jpg";

                using (var fileStream = new FileStream(defaultProfilePicPath, FileMode.Open, FileAccess.Read))
                {
                    profilePicture = File.ReadAllBytes(defaultProfilePicPath);
                    fileStream.Read(profilePicture, 0, Convert.ToInt32(fileStream.Length));
                }
            }

            return profilePicture;
        }
    }
}