using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Domain.Interfaces;
using Shop.Infra.Data.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.infra.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IPermissionServices, PermissionServices>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISiteService, SiteServices>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IArticleService, ArticleService>();
            #endregion

            #region Reposiotores
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            #endregion

            #region Tools
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<ISmsServece, SmsService>();
            #endregion
        }

    }
}
