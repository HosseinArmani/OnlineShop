using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Articles;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.Production;
using Shop.Domain.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Context
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }

        #region User
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Address> Addresses { get; set; }

        #endregion

        #region UserWallet
        public DbSet<UserWallet> userWallets { get; set; }

        #endregion

        #region Product
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }

        public DbSet<ProductGallery> productGalleries { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }



        #endregion

        #region Site
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<UserFavorites> UserFavorites { get; set; } 
        #endregion
        #region Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }

        #endregion
        #region Article
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; } 
        #endregion


    }
}
