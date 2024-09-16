using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Site;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class SiteController : BaseAdminController
    {
        private readonly ISiteService _siteService;
        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }
        [PermissionCheker(23)]
        public async Task< IActionResult> ShowSlider()
        {
            return View(await _siteService.GetAllSlider());
        }
        [PermissionCheker(24)]
        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(CreateSliderViewModel createSlider,IFormFile imgSlider)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteService.CreateSlider(createSlider, imgSlider);
                switch (result)
                {
                    case CreateSliderResult.Success:
                        return RedirectToAction("ShowSlider");
               
                }
            }
            return View(createSlider);
        }
        [PermissionCheker(26)]
        [HttpGet]
        public async Task<IActionResult> EditSlider(long SliderId)
        {
            var slider = await _siteService.GetEditSlider(SliderId);
            if (slider == null)
            {
                return null;
            }
            return View(slider);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlider(EditSliderViewModel editSlider, IFormFile imgSlider)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteService.EditSlider(editSlider, imgSlider);
                switch (result)
                {
                    case EditSliderResult.Success:
                        return RedirectToAction("ShowSlider");
                    case EditSliderResult.NotFound:
                        break;
                  
                }

            }
            return View(editSlider);
        }
        [PermissionCheker(28)]
        public async Task<IActionResult> DeleteSlider(long sliderId)
        {
            await _siteService.DeleteImageSlider(sliderId);
            return RedirectToAction("ShowSlider");
        }
    }
}
