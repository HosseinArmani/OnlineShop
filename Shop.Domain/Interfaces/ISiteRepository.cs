using Shop.Domain.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
   public interface ISiteRepository
    {
        Task<List<Slider>> GetAllSlider();
        Task AddSlider(Slider slider);
        void UpdateSlider(Slider slider);
        Task<Slider> GetSliderById(long sliderId);
        Task DeleteSlider(long sliderId);
        Task AddUserFavorites(UserFavorites userFavorites);
        Task<bool> IsExistProductFavorites(long productId, long userId);
        Task<int> UserFavoriteCount(long userId);
        Task<List<UserFavorites>> GetUserFavorites(long userId);
        Task RemoveUserFavorites(long id);
        Task SaveChange();
    }
}
