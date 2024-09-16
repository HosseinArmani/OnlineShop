using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shop.Application.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Order;
using Shop.Domain.ViewModels.Wallet;
using Shop.Web.Extentions;
using System;
using System.Threading.Tasks;

namespace Shop.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        string authority;
        private readonly IUserServices _userServices;
        private readonly IOrderService _orderService;
        private readonly ISiteService _siteService;

        public AccountController(IUserServices userServices, IOrderService orderService, ISiteService siteService)
        {
            _userServices = userServices;
            _orderService = orderService;
            _siteService = siteService;
        }

        #region edit user profile
        [HttpGet("edit-user-profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userServices.GetEditUserProfile(User.GetUserId());
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost("edit-user-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserProfile)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.EditUserProfile(User.GetUserId(), editUserProfile);
                switch (result)
                {
                    case EditUserProfileResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش حساب کاربری با موفقیت انجام شد";
                        return RedirectToAction("Index", "Home");

                }

            }

            return View(editUserProfile);
        }
        #endregion

        #region Change Password
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost("ChangePassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ChangePassword(User.GetUserId(), changePassword);
                switch (result)
                {
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "رمز عبور شما با موفقیت تغییر یافت";
                        return RedirectToAction("index", "home");
                    case ChangePasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ChangePasswordResult.Erorr:
                        TempData[ErrorMessage] = " رمزعبور فعلی خود را اشتباه وارد کردید ";
                        break;

                }

            }
            return View();
        }
        #endregion

        #region Address
        [HttpGet("ShowAddress")]
        public async Task<IActionResult> ShowAddress()
        {
            return View(await _userServices.GetAddresByUserId(User.GetUserId()));
        }
        [HttpGet("CreateAddress")]
        public async Task<IActionResult> CreateAddress()
        {
            var user = await _userServices.GetCreateAddress(User.GetUserId());
            return View(user);
        }
        [HttpPost("CreateAddress"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(CreateAddressViewModel create)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.CreateAddress(User.GetUserId(), create);
                switch (result)
                {
                    case CreateAddressResult.NotFound:
                        return RedirectToAction("ShowAddress");
                    case CreateAddressResult.Success:
                        TempData[SuccessMessage] = "آدرس شما با موفقیت ثبت شد";
                        return RedirectToAction("ShowAddress");
                }
            }
            return View();
        }
        [HttpGet("EditAddress")]
        public async Task<IActionResult> EditAddress(long addressId)
        {
            var address = await _userServices.GetEditAddress(User.GetUserId(), addressId);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }
        [HttpPost("EditAddress"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(EditAddressViewModel edit)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.EditAddress(edit);
                switch (result)
                {
                    case EditAddressResult.NotFound:
                        TempData[ErrorMessage] = "این ادس یافت نشد";
                        break;
                    case EditAddressResult.Success:
                        TempData[SuccessMessage] = "آدرس شما با موفقیت ویرایش شد";
                        return RedirectToAction("ShowAddress");

                }
            }
            return View();
        }
        #endregion
        #region User-Basket
        [HttpGet("Basket/{orderId}")]
        public async Task<IActionResult> ShowBasket(long orderId)
        {
            ViewData["Transprtation"] = await _orderService.GetTransportations();


            var order = await _orderService.GetUserBasketForUserPanel(orderId, User.GetUserId());
            if (order == null)
            {
                return NotFound();
            }

        

            return View(order);

        }

        [HttpPost("UserDicount")]
        public async Task<IActionResult> UserDicount(long orderId, string code)
        {
            var type = await _orderService.UseDiscount(orderId, code);

            return Redirect("/User/Basket/" + orderId + "?type:" + type.ToString());
        }

        [HttpGet("CheckOut/{orderId}")]
        public async Task<IActionResult> CheckOut(long orderId)
        {
            ViewData["Transprtation"] = await _orderService.GetTransportations();
            ViewData["Address"] = await _userServices.GetAddresByUserId(User.GetUserId());
            var order = await _orderService.GetUserBasketForUserPanel(orderId, User.GetUserId());
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost("CheckOut/{orderId}")]
        public async Task<IActionResult> CheckOut(FinallyOrderViewzModel finallyOrder)
        {
            if (finallyOrder.IsPayment)
            {
                var result = await _orderService.FinallyOrder(finallyOrder, User.GetUserId());
                switch (result)
                {
                    case FinallyOrderResult.HasNotUser:
                        TempData[ErrorMessage] = "سفارشی یافت نشد";
                        break;
                    case FinallyOrderResult.Errore:
                        TempData[ErrorMessage] = "موجودی کیف پول شما کافی نمی باشد ";
                        return Redirect("/user/Charge-Wallet");
                    case FinallyOrderResult.Success:
                        TempData[SuccessMessage] = "فاکتور شما با موفقیت ثبت شد";
                        return Redirect("/user/user-wallet");
                    case FinallyOrderResult.NotFound:
                        TempData[ErrorMessage] = "سفارشی یافت نشد";
                        break;

                }
            }
            else
            {
                var order = await _orderService.GetOrderById(finallyOrder.OrderId);
                var transportation = await _orderService.GetTransportations();

                try
                {
                    string orderSum = (order.OrderSum * 10).ToString();
                    string ordersumTransportation = (order.OrderSum * 10 + transportation.Amount * 10).ToString();


                    //RequestParameters Parameters = new RequestParameters("b7531f90-420b-44a3-acf9-3204cc0d7015", (order.OrderSum <= transportation.minBuy) ? ordersumTransportation : orderSum, "پرداخت سفارش", "https://tasland.ir/verify-PaymentOrder/" + order.Id, "", "");



                    //be dalil in ke metadata be sorate araye ast va do meghdare mobile va email dar metadata gharar mmigirad
                    //shoma mitavanid in maghadir ra az kharidar begirid va set konid dar gheir in sorat khali ersal konid

                    var client = new RestClient(URLs.requestUrl);

                    Method method = Method.Post;

                    var request = new RestRequest("", method);

                    request.AddHeader("accept", "application/json");

                    request.AddHeader("content-type", "application/json");

                   // request.AddJsonBody(Parameters);

                    var requestresponse = client.ExecuteAsync(request);

                    JObject jo = JObject.Parse(requestresponse.Result.Content);

                    string errorscode = jo["errors"].ToString();

                    JObject jodata = JObject.Parse(requestresponse.Result.Content);

                    string dataauth = jodata["data"].ToString();


                    if (dataauth != "[]")
                    {


                        authority = jodata["data"]["authority"].ToString();

                        string gatewayUrl = URLs.gateWayUrl + authority;

                        return Redirect(gatewayUrl);

                    }
                    else
                    {

                        TempData[ErrorMessage] = " عملیات پرداخت با مشکل مواجه شد لطفا مجددا امتحان کنید ";
                        return BadRequest("error " + errorscode);


                    }


                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);


                }
            }
            return View();
        }

        [HttpPost("CheckOutCreateOrEditAddress"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutCreateOrEditAddress(long orderId, CreateOrEditAddressViewModel createOrEdit)
        {
            if (string.IsNullOrEmpty(createOrEdit.State) || string.IsNullOrEmpty(createOrEdit.City) || string.IsNullOrEmpty(createOrEdit.PostalCode) || string.IsNullOrEmpty(createOrEdit.FullAddress))
            {
                TempData[ErrorMessage] = " لطفا همه مقادیر را با دقت وارد کنید";
                return Redirect("/user/CheckOut/" + orderId);
            }
            var result = await _userServices.CreateOrEditAddress(User.GetUserId(), createOrEdit);
            switch (result)
            {
                case CreateOrEditAddressResult.NotFound:
                    TempData[ErrorMessage] = "آدرس ثبت نشد";
                    return Redirect("/user/CheckOut/" + orderId);
                case CreateOrEditAddressResult.Success:
                    TempData[SuccessMessage] = "ادرس با موفقیت ثبت شد،لطفا سفارش خود رانهایی کنید";
                    return Redirect("/user/CheckOut/" + orderId);

            }


            return View();
        }
        [HttpGet("delete-orderdetail/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(long orderDetailId)
        {
            var orderId = await _orderService.RemoveOrderDetail(orderDetailId);



            return Redirect("/User/Basket/" + orderId);
        }
        #endregion
        #region FilterUserOrder
        [HttpGet("user-order")]
        public async Task<IActionResult> FilterUserOrder(FilterOrderForUserPanel filteOrder)
        {
            filteOrder.UserId = User.GetUserId();



            return View(await _orderService.FilterOrdersForUserPanel(filteOrder));
        }
        [HttpGet("user-orderdetail")]
        public async Task<IActionResult> UserOrderDetail(long orderId)
        {
            var order = await _orderService.GetOrderDetail(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        #endregion
        #region UserFavorites
        [HttpGet("UserFavorites/{productId}")]
        public async Task<IActionResult> UserFavorites(long productId)
        {
            var result = await _siteService.AddProductToFavorites(User.GetUserId(), productId);
            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر به لیست علاقه مندی ها اضافه شد";
                return RedirectToAction("ShowUserFavorites");

            }
            TempData[WarningMessage] = "محصول مورد نظر قبلا به لیست علاقه مندی ها اضافه شده است";
            return Redirect("/");

        }
        [HttpGet("Show-UserFavorites")]
        public async Task<IActionResult> ShowUserFavorites()
        {
            var favorites = await _siteService.GetUserFavorites(User.GetUserId());
            if (favorites == null)
            {
                return NotFound();
            }
            return View(favorites);
        }
        [HttpGet("Remove-UserFavorites")]
        public async Task<IActionResult> RemoveUserFavorites(long id)
        {
            var Result = await _siteService.RemoveUserFavorites(id);
            if (Result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر از لیست علاقه مندی ها حذف شد";
                return RedirectToAction("ShowUserFavorites");
            }

            TempData[WarningMessage] = "این محصول در لیست وجود ندارد";
            return Redirect("/");
        }

        #endregion
    }
}
