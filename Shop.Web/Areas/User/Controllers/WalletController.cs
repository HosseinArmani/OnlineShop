using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Wallet;
using Shop.Web.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Areas.User.Controllers
{
    public class WalletController : UserBaseController
    {
        string authority;
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {

            _walletService = walletService;
        }

        #region Charge Wallet
        [HttpGet("Charge-Wallet")]
        public IActionResult ChargeWallet()
        {
            return View();
        }
        [HttpPost("Charge-Wallet"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel charge)
        {
            
            if (ModelState.IsValid)
            {
                var walletId = await _walletService.ChargeWallet(User.GetUserId(), charge, "شارژ حساب");
                var amount = charge.Amount * 10;
                try
                {
                    //RequestParameters Parameters = new RequestParameters("b7531f90-420b-44a3-acf9-3204cc0d7015", amount.ToString(), "شارژ کیف پول", "https://tasland.ir/verify-Payment/" + walletId, "", "");



                    //be dalil in ke metadata be sorate araye ast va do meghdare mobile va email dar metadata gharar mmigirad
                    //shoma mitavanid in maghadir ra az kharidar begirid va set konid dar gheir in sorat khali ersal konid

                    var client = new RestClient(URLs.requestUrl);

                    Method method = Method.Post;

                    var request = new RestRequest("", method);

                    request.AddHeader("accept", "application/json");

                    request.AddHeader("content-type", "application/json");

                    //request.AddJsonBody(Parameters);

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
            return View(charge);
        }
      
        #endregion

        #region User Wallet
        [HttpGet("user-wallet")]
        public async Task<IActionResult> UserWallet(FilterWalletViewModel filter)
        {
            filter.UserId = User.GetUserId();

            return View(await _walletService.FilterWallet(filter));
        }
        #endregion
        

    }
}
