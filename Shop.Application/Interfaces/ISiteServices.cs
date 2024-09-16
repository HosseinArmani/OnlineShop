using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface ISiteService
    {
        Task<List<Slider>> GetAllSlider(); 
        Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider,IFormFile imgSlider);
        Task<EditSliderViewModel> GetEditSlider(long sliderId); 
        Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile imgSlider);
        Task DeleteImageSlider(long sliderId);
        Task<bool> AddProductToFavorites(long userId, long productId);
        Task<int> UserFavoriteCount(long userId);
        Task<List<UserFavorites>> GetUserFavorites(long userId);
        Task<bool> RemoveUserFavorites(long id);
    }
}
