using Microsoft.AspNetCore.Http;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class SiteServices : ISiteService
    {
        private readonly ISiteRepository _siteRepository;
        public SiteServices(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }
        public async Task<List<Slider>> GetAllSlider()
        {
            return await _siteRepository.GetAllSlider();
        }

        public async Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider, IFormFile imgSlider)
        {
            var slider = new Slider
            {
                SliderTitle = createSlider.SliderTitle,
                Url = createSlider.Url,
                IsActive = createSlider.IsActive
            };
            #region AddImage

            slider.SliderImageName = "shopping-cart.png";

            if (imgSlider != null && imgSlider.IsImage())
            {
                slider.SliderImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgSlider.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider", slider.SliderImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgSlider.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider/Thumb", slider.SliderImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }
            #endregion
            await _siteRepository.AddSlider(slider);
            await _siteRepository.SaveChange();
            return CreateSliderResult.Success;

        }

        public async Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile imgSlider)
        {
            var slider = await _siteRepository.GetSliderById(editSlider.SliderId);

            if (slider == null) return EditSliderResult.NotFound;

            slider.SliderTitle = editSlider.SliderTitle;
            slider.Url = editSlider.Url;
            slider.IsActive = editSlider.IsActive;
            #region AddImage
            if (imgSlider != null && imgSlider.IsImage())
            {
                if (slider.SliderTitle != "shopping-cart.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider", slider.SliderImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider/Thumb", slider.SliderImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                slider.SliderImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgSlider.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider", slider.SliderImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgSlider.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider/Thumb", slider.SliderImageName);

                var imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }
            #endregion
            _siteRepository.UpdateSlider(slider);
            await _siteRepository.SaveChange();
            return EditSliderResult.Success;
        }


        public async Task<EditSliderViewModel> GetEditSlider(long sliderId)
        {
            var slider = await _siteRepository.GetSliderById(sliderId);

            return new EditSliderViewModel
            {
                SliderId = slider.Id,
                SliderImageName = slider.SliderImageName,
                SliderTitle = slider.SliderTitle,
                Url = slider.Url,
                IsActive = slider.IsActive,

            };
        }

        public async Task DeleteImageSlider(long sliderId)
        {
            var slider = await _siteRepository.GetSliderById(sliderId);
            if (slider != null)
            {
                string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider", slider.SliderImageName);
                if (File.Exists(deleteimagePath))
                {
                    File.Delete(deleteimagePath);
                }

                string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageSlider/Thumb", slider.SliderImageName);
                if (File.Exists(deletethumbPath))
                {
                    File.Delete(deletethumbPath);
                }
            }
            await _siteRepository.DeleteSlider(sliderId);
        }

        public async Task<bool> AddProductToFavorites(long userId, long productId)
        {
            if (!await _siteRepository.IsExistProductFavorites(userId, productId))
            {
                var favorite = new UserFavorites
                {
                    ProductId = productId,
                    UserId = userId
                };
                await _siteRepository.AddUserFavorites(favorite);
                await _siteRepository.SaveChange();
                return true;
            }
            return false;
        }

        public async Task<int> UserFavoriteCount(long userId)
        {
           return await _siteRepository.UserFavoriteCount(userId);
        }

        public async Task<List<UserFavorites>> GetUserFavorites(long userId)
        {
            return await _siteRepository.GetUserFavorites(userId);
        }

        public async Task<bool> RemoveUserFavorites(long id)
        {
           await _siteRepository.RemoveUserFavorites(id);
           await _siteRepository.SaveChange();

            return true;
        }
    }
}
