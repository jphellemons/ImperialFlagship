using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImperialFlagship
{
    public class Set
    {
        public string set_id { get; set; }
        public string descr { get; set; }
        public string set_img_url { get; set; }
        public Part[] parts { get; set; }

        internal void GetPartPrices()
        {
            foreach (Part p in parts)
            {
                WebClient wc = new WebClient();
                string url = String.Format("http://www.bricklink.com/catalogPG.asp?itemType=P&itemNo={0}&itemSeq=1&colorID={1}&v=P&priceGroup=Y&prDec=2",
                                p.part_id, p.ldraw_color_id);
                string s = wc.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(s);
                try
                {
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

                            if (q > Convert.ToInt32(p.qty)) // first store has enough new bricks
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

                                    if (q > Convert.ToInt32(p.qty)) // first store has enough new bricks
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static LegoPriceSet GetPriceSet(HtmlNode pt, bool isNew, int rowNr = 2)
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
            int itemId = Convert.ToInt32(storeNode.Substring(storeNode.IndexOf('=') + 1));

            lps.Quantity = quantity;
            lps.New = true;
            lps.Store = new LegoStore(storeId);
            lps.Price = price;
            lps.ItemId = itemId;

            return lps;
        }

        internal void DisplayUnavailable()
        {
            var shopLess = parts.Where(p => p.PriceSet.Count == 0).ToList();
            foreach (var sl in shopLess)
                Console.Write(sl.ToString());
            Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        }

        internal void CalculateTotalPrice()
        {
            foreach (Part p in parts)
            {
                p.ToBuy = Convert.ToInt32(p.qty);
                if(p.PriceSet.Count > 0)
                {
                    for (int i = 0; p.ToBuy > 0 && p.PriceSet.Count > i; i++)
                    {
                        if (p.ToBuy < p.PriceSet[i].Quantity)
                        {
                            p.TotalPrice += (p.ToBuy * p.PriceSet[i].Price);
                            p.ToBuy = 0;
                        }
                        else
                        {
                            p.TotalPrice += (p.PriceSet[i].Quantity * p.PriceSet[i].Price);
                            p.ToBuy -= p.PriceSet[i].Quantity;
                        }
                    }
                }
            }
            int totalBricks = parts.Sum(t => Convert.ToInt32(t.qty));
            int totalBricksLeft = parts.Sum(t => t.ToBuy);
            int totalAvailable = totalBricks - totalBricksLeft;
            decimal totalPrice = parts.Sum(k => k.TotalPrice);
            Console.WriteLine(totalAvailable + " of " + totalBricks + " can be bought now");
            Console.WriteLine(Math.Round((100.0 * totalAvailable) / totalBricks, 2) + "% can be bought now for " + totalPrice.ToString("C"));   
        }
    }
}
