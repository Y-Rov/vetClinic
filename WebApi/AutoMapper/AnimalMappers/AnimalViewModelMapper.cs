using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AnimalViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AnimalMappers
{
    public class AnimalViewModelMapper : IViewModelMapperUpdater<AnimalViewModel, Animal>
    {
        private readonly IAnimalPhotoService _animalPhotoService;

        public AnimalViewModelMapper(IAnimalPhotoService animalPhotoService)
        {
            _animalPhotoService = animalPhotoService;
        }

        public Animal Map(AnimalViewModel source)
        {
            var animalViewModel = new Animal
            {
                Id = source.Id,
                OwnerId = source.OwnerId,
                NickName = source.NickName,
                BirthDate = source.BirthDate,
                PhotoUrl = GetLink(source).Result
            };

            return animalViewModel;
        }

        public void Map(AnimalViewModel source, Animal dest)
        {
            if (source.PhotoUrl == dest.PhotoUrl)
            {
                dest.Id = source.Id;
                dest.OwnerId = source.OwnerId;
                dest.NickName = source.NickName;
                dest.BirthDate = source.BirthDate;
                dest.PhotoUrl = source.PhotoUrl;
            }
            else
            {
                dest.Id = source.Id;
                dest.OwnerId = source.OwnerId;
                dest.NickName = source.NickName;
                dest.BirthDate = source.BirthDate;
                dest.PhotoUrl = GetLink(source).Result;
            }
        }

        private async Task<string?> GetLink(AnimalViewModel source)
        {
            byte[]? bytes;

            if (source.PhotoUrl is not null)
            {
                bytes = Convert.FromBase64String(source.PhotoUrl);
            }
            else
            {
                string defaultProfilePicPath = "../WebApi/Assets/Images/default_animal_pic.jpg";
                bytes = await File.ReadAllBytesAsync(defaultProfilePicPath);
            }

            using (MemoryStream ms = new(bytes))
            {
                var image = Image.FromStream(ms);
                var profilePictureLink = await _animalPhotoService.UploadAsync(image, source.NickName!, "jpg");

                return profilePictureLink;
            }
        }
    }
}
