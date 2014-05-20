using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    public class LegoStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal ShippingCosts { get; set; }

        public LegoStore()
        { }

        public LegoStore(int id)
        {
            this.Id = id;
        }
    }
}
