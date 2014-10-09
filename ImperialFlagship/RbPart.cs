using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    /// <summary>
    /// ReBrickable part
    /// </summary>
    public class RbPart
    {
        public string part_id { get; set; }
        public string qty { get; set; }
        public string ldraw_color_id { get; set; }
        public int type { get; set; }
        public string part_name { get; set; }
        public string color_name { get; set; }
        public string part_img_url { get; set; }
        public object element_id { get; set; }
        public string element_img_url { get; set; }
        public int ToBuy { get; set; }
        public decimal TotalPrice { get; set; }

        public RbPart()
        {
            
        }

        public override string ToString()
        {
            return part_name + " X " + qty + " (" + color_name + ")\n";
        }
    }
}