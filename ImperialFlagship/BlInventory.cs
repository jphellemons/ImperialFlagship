using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    public class BlInventory
    {
        public BlItem item { get; set; }
        public int inventory_id { get; set; }
        public int color_id { get; set; }
        public string color_name { get; set; }
        public int quantity { get; set; }
        public int bulk { get; set; }
        public string condition { get; set; }
        public string unit_price { get; set; }
    }
}
