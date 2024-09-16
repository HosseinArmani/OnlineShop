using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.User.Controllers
{
    
    public class HomeController : UserBaseController
    {
        private readonly IUserServices _userServices;
        public HomeController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {

            ViewBag.User = await _userServices.GetUserByPhoneNumbers(User.Identity.Name);

            return View(await _userServices.GetInformationUserProfile(User.Identity.Name));
        }
    }
}
