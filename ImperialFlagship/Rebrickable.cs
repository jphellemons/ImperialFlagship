using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    /// <summary>
    /// class for extracting partlists of http://rebrickable.com/
    /// </summary>
    static class Rebrickable
    {
        internal static RbSet LoadLegoParts(string setId)
        {
            WebClient w = new WebClient();
            string result = w.DownloadString("http://rebrickable.com/api/get_set_parts?key=" +
                ConfigurationManager.AppSettings.Get("rebrickableApiKey") + "&set=" + setId + "&format=json");

            return JsonConvert.DeserializeObject<List<RbSet>>(result).First();
        }
    }
}
