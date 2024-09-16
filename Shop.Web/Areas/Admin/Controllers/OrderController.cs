using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Web.Extentions;
using Shop.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [PermissionCheker(18)]
        public async Task<IActionResult> ShowDiscount()
        {
            return View(await _orderService.GetAllDiscount());
        }
        [PermissionCheker(19)]
        [HttpGet]
        public IActionResult CreateDiscount()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscount(CreateDiscountCodeViewModel create)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.CreateDiscount(create);
                switch (result)
                {
                    case CreateDiscountCodeResult.Sucesss:
                        return Redirect("ShowDiscount");
                    
                }
            }

            return View(create);
        }
        [PermissionCheker(20)]
        [HttpGet]
        public async Task<IActionResult> EditDiscount(long discountId)
        {
            var discount = await _orderService.GetEditDiscount(discountId);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDiscount(EditDiscountCodeViewModel edit)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.EditDiscount(edit);
                switch (result)
                {
                    case EditDiscountCodeResult.NotFound:
                        break;
                    case EditDiscountCodeResult.Success:
                        return Redirect("ShowDiscount");

                }

            }
            return View();
        }

        [PermissionCheker(21)]
        public async Task<IActionResult> FilterOrder(FilterOrdersViewModel filterOrders)
        {
            return View(await _orderService.FilterOrders(filterOrders));
        }
        public async Task<IActionResult> ChangeStateToSend(long orderId)
        {
            var result = await _orderService.ChangeStateToSend(orderId);
            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
            
        }
        [PermissionCheker(22)]
        public async Task<IActionResult> OrderDetail(long orderId)
        {
            var order =await _orderService.GetOrderDetail(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        public async Task<IActionResult> ShowAllExpiredOrders(FilterExpiredOrders filterExpired)
        {
            return View(await _orderService.FilterExpiredOrders(filterExpired));
        }
        public async Task<IActionResult> RemoveExpiredOrder(long orderId)
        {
            var Result = await _orderService.RemoveOrderExpiredForAdmin(orderId);
            if (Result)
            {
                TempData[SuccessMessage] = "سفارش مورد نظر از لیست  حذف شد";
                return RedirectToAction("ShowAllExpiredOrders");
            }

            TempData[WarningMessage] = "این سفارش در لیست وجود ندارد";
            return RedirectToAction("ShowAllExpiredOrders");
        }

        public async Task<IActionResult> ShowTransportation()
        {
            return View(await _orderService.GetTransportations());
        }
        [HttpGet]
        public async Task<IActionResult> EditTransportation(long transportationId)
        {
            var transportation = await _orderService.GetEditTransportation(transportationId);
            if (transportation == null)
            {
                return NotFound();
            }
            return View(transportation);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransportation(EditTransportationViewModel editTransportation)
        {
            
                var result = await _orderService.EditTransportation(editTransportation);
                switch (result)
                {
                    case EditTransportationResult.NotFound:
                        TempData[ErrorMessage] = "یافت نشد";
                        break;
                    case EditTransportationResult.Success:
                        TempData[SuccessMessage] = "ثبت با موفقیت انجام شد ";
                        return RedirectToAction("ShowTransportation");
                  
                }
            
            return View(editTransportation);
        }
    }
}
