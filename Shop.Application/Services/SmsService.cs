using Shop.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class SmsService : ISmsServece
    {
        public string apiKey = "35556C365137515A4458385164504537366E637A7178626E4633666872626769426E393776554D316953493D";
        public async Task SendVerifictionCode(string mobile, string activeCode)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey); 

            await api.VerifyLookup(mobile,activeCode,"Verify");
        }
    }
}
