using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Web.Extentions;
using System.Threading.Tasks;

namespace Shop.Web.Areas.User.ViewComponents
{
    public class UserSideBarViewComponent:ViewComponent
    {
      
        private readonly IUserServices _userService;
        public UserSideBarViewComponent(IUserServices userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                return View("UserSideBar", user);
            }

            return View("UserSideBar");
        }
        
    }

}
