using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    /// <summary>
    /// ReBrickable Set
    /// </summary>
    public class RbSet
    {
        public string set_id { get; set; }
        public string descr { get; set; }
        public string set_img_url { get; set; }
        public RbPart[] parts { get; set; }

        internal void GetInventory()
        {
            string u = "https://api.bricklink.com/api/public/v1/inventories?region=europe"; // please note that this is hardcoded to europe to minimize my shippingcosts
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

            StringBuilder sb = new StringBuilder();
            if (parts.Count() > 0)
            {
                sb.Append("[");
                foreach (RbPart p in parts.Take(3)) // remove take, this is just to make the test request smaller
                {
                    // this can be made prettier by adding a tojson method or serialize the part object.
                    sb.Append("{ \"item\": {\"no\":\"" + p.part_id +
                        "\", \"type\":\"PART\"}, \"color_id\":" + p.ldraw_color_id + 
                        ", \"condition\": \"N\" },");
                }
                if (sb.ToString().EndsWith(",")) // remove trailing comma
                    sb.Remove(sb.Length - 1, 1);

                sb.Append("]");
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] content = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
                    stream.Write(content, 0, content.Length);
                }
            }
            WebResponse response = request.GetResponse();

            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
            string resp = sr.ReadToEnd().Trim();
            
            var a = Newtonsoft.Json.JsonConvert.DeserializeObject<BlResponse>(resp);

            if(a.meta.code ==  200) // OK
            {
                var bestStore = a.data.Select(s => s.inventories.Count());
                Console.WriteLine(bestStore);
            }
            Console.WriteLine(resp);
        }
    }
}