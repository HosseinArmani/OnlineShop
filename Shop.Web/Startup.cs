using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.infra.Ioc;
using Shop.Infra.Data.Context;
using Shop.Web.Extentions;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebMarkupMin.AspNetCore5;

namespace Shop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddWebMarkupMin().AddHtmlMinification().AddHttpCompression();

            services.AddControllersWithViews();

            #region context
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion
            #region authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/log-Out";
                options.ExpireTimeSpan = TimeSpan.FromDays(360);

            });
          string path=  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/KeyDirectory/");

            services.AddDataProtection()
                // This helps surviving a restart: a same app will find back its keys. Just ensure to create the folder.
                .PersistKeysToFileSystem(new DirectoryInfo(path))
                // This helps surviving a site update: each app has its own store, building the site creates a new app
                .SetApplicationName("CookeiTasland")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(360));
            #endregion

            #region Ioc
            RegisterService(services);
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //var options = new RewriteOptions();
            //options.AddRedirectToHttps();
            //options.Rules.Add(new RedirectToWwwRule());
            //app.UseRewriter(options);

            app.UseWebMarkupMin();

            app.UseStatusCodePagesWithReExecute("/Home/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

          
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        #region Ioc
        public static void RegisterService(IServiceCollection services)
        {
            DependencyContainer.RegisterService(services);
        }
        #endregion
    }
}
