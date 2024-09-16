using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Application.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Web.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        private IUserServices _userServices;
        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        #region Register

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register, IFormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LeLlb8kAAAAAHvKI1wAArU2h3xgXyP53JHZs02C"; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "Sorry, please validate the reCAPTCHA";
                return View();
            }

            // go ahead and write code to validate username password against database
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var resultRegister = await _userServices.RegisterUser(register);
                    switch (resultRegister)
                    {
                        case RegisterUserResult.Success:
                            TempData[SuccessMessage] = "شما با موفقیت ثبث نام کردید";
                            return RedirectToAction("ActiveAccount", "Account", new { mobile = register.PhoneNumber });

                    }
                }
                catch (Exception ex)
                {
                  
                    Exception innerException = ex;
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;
                    }
                    var checkResult = await _userServices.CheckActiveCode(register);
                    if (checkResult == true)
                    {
                        TempData[SuccessMessage] = "شما قبلا ثبت نام کردید لطفا کد فعالسازی را وارد کنید";
                        await _userServices.AgainActiveCode(register);
                        return RedirectToAction("ActiveAccount", "Account", new { mobile = register.PhoneNumber });
                    }
                  else if (innerException.Message.Contains("unique"))
                    {
                        ModelState.AddModelError("PhoneNumber", "با این شماره موبایل قبلا ثبت نام کرده اید");

                    }

                }
            }
            return View(register);
        }
   
        #endregion

        #region login

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login, IFormCollection form)
        {
            //string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            //string secretKey = "6LeLlb8kAAAAAHvKI1wAArU2h3xgXyP53JHZs02C"; // change this
            //string gRecaptchaResponse = form["g-recaptcha-response"];

            //var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            //// send post data
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            //request.Method = "POST";
            //request.ContentLength = postData.Length;
            //request.ContentType = "application/x-www-form-urlencoded";

            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    streamWriter.Write(postData);
            //}

            //// receive the response now
            //string result = string.Empty;
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    using (var reader = new StreamReader(response.GetResponseStream()))
            //    {
            //        result = reader.ReadToEnd();
            //    }
            //}

            //// validate the response from Google reCaptcha
            //var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            //if (!captChaesponse.Success)
            //{
            //    ViewBag.Message = "Sorry, please validate the reCAPTCHA";
            //    return View();
            //}

            //// go ahead and write code to validate username password against database

            if (ModelState.IsValid)
            {
                var resultlogin = await _userServices.LoginUser(login);
                switch (resultlogin)
                {
                    case LoginUserResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
                    case LoginUserResult.NoActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نمیباشد";
                        break;
                    case LoginUserResult.IsBlock:
                        TempData[WarningMessage] = "حساب شما توسط واحد پشتیبانی مسدود شده است";
                        TempData[InfoMessage] = "جهت اطلاع بیشتر لطفا به قسمت تماس باما مراجعه کنید";
                        break;
                    case LoginUserResult.success:
                        var user = await _userServices.GetUserByPhoneNumbers(login.PhoneNumber);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.PhoneNumber),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principle, properties);
                        TempData[SuccessMessage] = "شما با موفقیت وارد شدید";
                        return Redirect("/");
                }
            }

            return View(login);
        }
        #endregion

        #region log-out

        [HttpGet("log-Out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
           TempData[InfoMessage] = "شما با موفقیت خارج شدید";
            return Redirect("/");
        }
        #endregion

        #region Active-Account

        [HttpGet("activate-account/{mobile}")]
        public IActionResult  ActiveAccount(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var active = new ActiveAccountViewModel { PhoneNumber = mobile };
            return View(active); 
        }
        [HttpPost("activate-account/{mobile}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveAccount(ActiveAccountViewModel activeAcount)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ActiveAcount(activeAcount);
                switch (result)
                {
                    case ActiveAccountResult.Error:
                        TempData[ErrorMessage] = " عملیات فعالسازی حساب کاربری باشکست مواجه شد";
                        break;
                    case ActiveAccountResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
                    case ActiveAccountResult.Success:
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        TempData[InfoMessage] = "لطفا  وارد حساب کاربری خود بشوید";
                        return RedirectToAction("Login");
                  
                }
            }
            return View(activeAcount);
        }
        #endregion

        #region Forgot Password

        [HttpGet("forgotpassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost("forgotpassword"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot,IFormCollection form)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LeLlb8kAAAAAHvKI1wAArU2h3xgXyP53JHZs02C"; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];

            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.Message = "Sorry, please validate the reCAPTCHA";
                return View();
            }

            // go ahead and write code to validate username password against database

            if (ModelState.IsValid)
            {
                var resultforgot = await _userServices.ForgotPassword(forgot);
                switch (resultforgot)
                {
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = " کد فعالسازی به موبایل شما ارسال شد ";
                        return RedirectToAction("ActiveForgotPassword", "Account",new { mobile = forgot.PhoneNumber });
                      
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
               
                }
            }
            return View(forgot);
        }
        [HttpGet("activate-forgot/{mobile}")]
        public IActionResult ActiveForgotPassword(string mobile)
        {
            var activeForgot = new ActiveForgotPasswordViewModel { PhoneNumber = mobile };

            return View(activeForgot);
        }

        [HttpPost("activate-forgot/{mobile}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveForgotPassword(ActiveForgotPasswordViewModel activeForgot)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ActiveForgotPassword(activeForgot);
                switch (result)
                {
                    case ActiveForgotPasswordResult.Error:
                        TempData[ErrorMessage] = " عملیات بازیابی رمز عبور باشکست مواجه شد لطفا مجداد اقدام کنید";
                        break;
                    case ActiveForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری یافت نشد";
                        break;
                    case ActiveForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "لطفا رمز عبور خود را تغییر دهید";
                        return RedirectToAction("ResetPassword", "Account", new { activeCode = activeForgot.ActiveCode });               
                }
            }

            return View(activeForgot);
        }
        [HttpGet("rest-password/{activeCode}")]
        public IActionResult ResetPassword(string activecode)
        {
            var activeCode = new ResetPasswordViewModel { ActiveCode = activecode };
            return View(activeCode);
        }
        [HttpPost("rest-password/{activeCode}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                var result =await _userServices.ResetPassword(resetPassword);
                switch (result)
                {
                    case ResetPasswordResult.Success:
                       TempData[SuccessMessage] = "رمز عبور با موفقیت تغییر کرد";
                        TempData[InfoMessage] = "لطفا  وارد حساب کاربری خود بشوید";
                        return RedirectToAction("Login");
                    case ResetPasswordResult.NotFound:
                        TempData[WarningMessage] = "کد فعالسازی اشتباه است ";
                        break;
                  
                }
            }
            return View(resetPassword);
        }
        #endregion

    }

}

