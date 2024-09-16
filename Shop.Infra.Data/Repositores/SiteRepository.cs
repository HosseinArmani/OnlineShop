using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Site;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{
    public class SiteRepository : ISiteRepository
    {
        private readonly ShopDbContext _context;
        public SiteRepository(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<Slider>> GetAllSlider()
        {
          return  await _context.Sliders.ToListAsync();
        }
        public async Task AddSlider(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
        }

        public void UpdateSlider(Slider slider)
        {
            _context.Sliders.Update(slider);
        }

      

        public async Task<Slider> GetSliderById(long sliderId)
        {
            return await _context.Sliders.FindAsync(sliderId);
        }

        public async Task SaveChange()
        {
           await  _context.SaveChangesAsync();
        }

        public async Task DeleteSlider(long sliderId)
        {
           var slider=  await _context.Sliders.FindAsync(sliderId);
            if (slider != null)
            {
                _context.Sliders.Remove(slider);
                await SaveChange();
            }
        }

        public async Task AddUserFavorites(UserFavorites userFavorites)
        {
            await _context.UserFavorites.AddAsync(userFavorites);
        }

        public async Task<bool> IsExistProductFavorites(long productId, long userId)
        {
          return await _context.UserFavorites.AsQueryable().AnyAsync(f => f.UserId == userId && f.ProductId == productId);
           
        }

        public async Task<int> UserFavoriteCount(long userId)
        {
            return await _context.UserFavorites.AsQueryable().Where(u => u.UserId == userId).CountAsync();
        }

        public async Task<List<UserFavorites>> GetUserFavorites(long userId)
        {
            return await _context.UserFavorites.Include(u=>u.Product).AsQueryable().Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task RemoveUserFavorites(long id)
        {
            var favorite = await _context.UserFavorites.SingleOrDefaultAsync(f => f.Id == id);
            if (favorite != null)
            {
                _context.UserFavorites.Remove(favorite);
            }
           
        }
    }
}
