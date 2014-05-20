using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace ImperialFlagship
{
    class Program
    {
        static void Main(string[] args)
        {
            string setId = "10210-1";
            DisplayAscii();

            if(args!= null && args.Length > 0)
                setId = args[0].ToString();

            Set legoSet = LoadLegoPartsFromRebrickable(setId);
            legoSet.GetPartPrices(); // unless http://api.bricklink.com/pages/api/welcome.page has an update
            legoSet.DisplayUnavailable();
            legoSet.CalculateTotalPrice();
            // perhaps fill store info (storename, shipping costs, min order etc.)

            Console.ReadKey();
        }

        private static Set LoadLegoPartsFromRebrickable(string setId)
        {
            WebClient w = new WebClient();
            string result = w.DownloadString("http://rebrickable.com/api/get_set_parts?key=" +
                ConfigurationManager.AppSettings.Get("rebrickableApiKey") + "&set=" + setId + "&format=json");

            return JsonConvert.DeserializeObject<List<Set>>(result).First();
        }

        private static void DisplayAscii()
        {
            Console.Write("    Lego 10210-1: The imperial flagship                                         ");
            Console.Write("                                                                                ");
            Console.Write("                                   :-`      .+o/-                               ");
            Console.Write("                                    -/`/`     `.-` .`                           ");
            Console.Write("                                 ` .-`:`     .-..` `   .`                       ");
            Console.Write("                                   `.`-`.` `-::..``     :                       ");
            Console.Write("                                    `.`-`. --::`.``     .-  ..                  ");
            Console.Write("                                    .y `-:+-...`.+` -/::-.```                   ");
            Console.Write("                                 ``--:::.``  .-/:..`````.`````                  ");
            Console.Write("                                ```-`:`.  ..::-//.. ` ``.````                   ");
            Console.Write("                             `/  ` ....`.-.--::/-.`` ` ```--   `.-:`            ");
            Console.Write("                     ``` `````` `` `.`:`.``-:/::..-.``   ..```.:.               ");
            Console.Write("                `   ````.``.:    `  .`-``..-..`./+s//o//:+-.``                  ");
            Console.Write(".+o++. `  ``   `  `````.``::      `:yy//../.`-/+//:...-.-.-.. `                 ");
            Console.Write("  `:sdm+` `      ````..``::       -+ysss++:-:/++:/..`` ``.-..` `                ");
            Console.Write("     `.+yy/.     ` ``-``:-   `-::+yosos+/+ -//+/--``` ` ``--:/ --.   .`         ");
            Console.Write("          -oys:`   `-```.-:--.` .+s+yos./+`-:::.-`` `    ``:-d/+soo/:.`         ");
            Console.Write("             -+syo:```/`         :os+o:/s+--:--`.`.`.. `  `+yms-`++.`. `+`.     ");
            Console.Write("              -o:+yyyy+/.        `s/+o-:/`+..`` ++o:y:./o/-o-/y.`./+/s-+so+/:-/.");
            Console.Write("              .hhho. /yyddy+/:.` :-/:s/o+/+``-//syysdo-`/+o+  h::+:yshsdy/+osy+/");
            Console.Write("                -+yds+++ysNNhshdyhhshhdyhoo:.`-so/soys/y-oho/ssyoyymhhddm/mso+` ");
            Console.Write("                   `-+sssyddNNNNNNNdhhdhyms:..-+++ssoy/yo:syyhsyddmdhysmM+ym:m` ");
            Console.Write("                        :hdmNMN////dNMhy+d/dhyhyyysyoos+sd/y:s:++o-+hy+oMssM+:. ");
            Console.Write("                         :dNNMh-://dNNssoodsodo+hsyyyo/yooss:o+/so:y+os:/-//:/. ");
            Console.Write("                          `sNNy+osydmNmysmNs/mo/mssysy:hsssys+hsyhoyysyhoyso:`  ");
            Console.Write("                            :dddmmNNNNMddmmmdmo/d+hs/m/yyysoh/mssod+msomoNo/-   ");
            Console.Write("                             .hmmmNNNMMhddhdmNmdmddddmdmmhhdmdmdddmdmhddddy+-..-");
            Console.Write(" .++++o+ooosssssssyyyyyyyyyhhhdhmNNNNNNmmNNNNmmmmmmmmmmmmmmNNNNMMMMMMMMMMNmmdso:");
            Console.Write("           ```.-::///++++ssyhdmNdNNNNNNmmmmdddhhhyhyyysssoo++///::---..``       ");
        }
    }
}