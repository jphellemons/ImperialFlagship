using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ImperialFlagship
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayAscii();

            //List<LegoPart> lpl = LoadLegoPartsByCsv("testfile.csv");
            List<LegoPart> lpl = LoadLegoPartsByCsv("rebrickable_parts_10210-1.csv");  
            
            foreach (var p in lpl)
            {
                GetPrice(p); // this should be async
            }
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            foreach (var p in lpl)
            {
                Console.Write(p.ToString());
            }
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            var shopLess = lpl.Where(p => p.PriceSet.Count == 0).ToList();
            foreach(var sl in shopLess)
                Console.Write(sl.ToString());
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            int totalBricks = lpl.Sum(t => t.Quantity);
            int totalBricksLeft = lpl.Sum(t => t.ToBuy);
            int totalAvailable = totalBricks - totalBricksLeft;
            decimal totalPrice = lpl.Sum(k => k.TotalPrice);
            Console.WriteLine(totalAvailable + " of " + totalBricks + " can be bought now");
            Console.WriteLine(Math.Round((100.0 * totalAvailable)/totalBricks,2) + "% can be bought now for " + totalPrice.ToString("C"));

            Console.ReadKey();
        }

        private static List<LegoPart> LoadLegoPartsByCsv(string filename)
        {
            List<LegoPart> lpl = new List<LegoPart>();
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var reader = new StreamReader(File.OpenRead(currentDirectory + "\\" + filename));
            string line;
            bool skipHeader = true;
            while ((line = reader.ReadLine()) != null)
            {
                if (!skipHeader)
                {
                    if (line.Contains(","))
                    {
                        var values = line.Split(',');

                        lpl.Add(new LegoPart(values[0], values[1], values[2]));
                    }
                }
                else
                    skipHeader = false;
            }
            return lpl;
        }

        private static void GetPrice(LegoPart p)
        {
            WebClient wc = new WebClient();
            string url = String.Format("http://www.bricklink.com/catalogPG.asp?itemType=P&itemNo={0}&itemSeq=1&colorID={1}&v=P&priceGroup=Y&prDec=2", 
                            p.Part, p.Color);
            string s = wc.DownloadString(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(s);
            try {
                var newPrices = doc.DocumentNode.SelectSingleNode("/html/body/center/table[3]//table[3]/tr[4]/td[3]");
                var usedPrices = doc.DocumentNode.SelectSingleNode("/html/body/center/table[3]//table[3]/tr[4]/td[4]");

                if (newPrices != null)
                {
                    if (newPrices.InnerText.Contains("Currently Available"))
                    {
                        var pt = newPrices.SelectSingleNode("table[3]/tr/td");
                        var totalQuantity = pt.SelectSingleNode("table/tr[2]/td[2]").InnerText;
                        int q = 0;
                        int.TryParse(totalQuantity, out q);

                        if (q > p.Quantity) // first store has enough new bricks
                        {
                            p.PriceSet.Add(GetPriceSet(pt, true));
                        }
                        else
                        {
                            try
                            {
                                int rowNr = 2;
                                do
                                {
                                    p.PriceSet.Add(GetPriceSet(pt, true, rowNr));
                                    rowNr++;
                                } while (rowNr <= 12);
                            }
                            catch { }
                        }
                        // Total Qty
                    }
                    else
                    {
                        if (usedPrices != null)
                        {
                            if (usedPrices.InnerText.Contains("Currently Available"))
                            {
                                var pt = usedPrices.SelectSingleNode("table[3]/tr/td");
                                var totalQuantity = pt.SelectSingleNode("table/tr[2]/td[2]").InnerText;
                                int q = 0;
                                int.TryParse(totalQuantity, out q);

                                if (q > p.Quantity) // first store has enough new bricks
                                {
                                    p.PriceSet.Add(GetPriceSet(pt, false));
                                }
                                else
                                {
                                    try
                                    {
                                        int rowNr = 2;
                                        do
                                        {
                                            p.PriceSet.Add(GetPriceSet(pt, false, rowNr));
                                            rowNr++;
                                        } while (rowNr <= 12);
                                    }
                                    catch { }
                                }
                                // Total Qty
                            }
                        }
                    }
                }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }

        private static LegoPriceSet GetPriceSet(HtmlNode pt, bool isNew, int rowNr =2)
        {
            LegoPriceSet lps = new LegoPriceSet();

            var quantityNode = pt.SelectSingleNode("table/tr[" + rowNr + "]/td[2]").InnerText;
            int quantity = 0;
            int.TryParse(quantityNode, out quantity);

            var priceNode = pt.SelectSingleNode("table/tr[" + rowNr + "]/td[3]").InnerText.Substring(16).Replace(".", ",");
            decimal price = 0;
            decimal.TryParse(priceNode, out price);

            //296901&itemID=30262749
            var storeNode = pt.SelectSingleNode("table/tr[" + rowNr + "]/td[1]/a").Attributes["href"].Value.Substring(15);
            int storeId = Convert.ToInt32(storeNode.Substring(0, storeNode.IndexOf('&')));
            int itemId = Convert.ToInt32(storeNode.Substring(storeNode.IndexOf('=')+1));

            lps.Quantity = quantity;
            lps.New = true;
            lps.Store = new LegoStore(storeId);
            lps.Price = price;
            lps.ItemId = itemId;

            return lps;
        }

        private static void DisplayAscii()
        {
            Console.Write("    Lego 10210: The imperial flagship                                           ");
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