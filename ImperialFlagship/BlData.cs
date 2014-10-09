using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    public class BlData
    {
        public string store_name { get; set; }
        public string seller_username { get; set; }
        public string minimum_purchase { get; set; }
        public string currency_code { get; set; }
        public string[] accepted_currencies { get; set; }
        public string[] shipping_countries { get; set; }
        public BlInventory[] inventories { get; set; }
    }
}
