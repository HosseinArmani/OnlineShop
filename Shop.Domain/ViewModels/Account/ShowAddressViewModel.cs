using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
 public  class ShowAddressViewModel
    {
     
        public string State { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string FullAddress { get; set; }
    }
}
