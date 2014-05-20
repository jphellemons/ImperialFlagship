using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    public class LegoPriceSet
    {
        public LegoStore Store { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool New {get; set;}
        public int ItemId { get; set; }

        public LegoPriceSet()
        {
            Store = new LegoStore();
        }
    }
}
