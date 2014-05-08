using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImperialFlagship
{
    class LegoPart
    {        
        public string Part { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public List<LegoPriceSet> PriceSet { get; set; }
        public int ToBuy { get; set; }
        public decimal TotalPrice { get; set; }

        public LegoPart(string p1, string p2, string p3)
        {
            Part = p1;
            Color = p2;
            Quantity = Convert.ToInt32(p3);
            PriceSet = new List<LegoPriceSet>();
            TotalPrice = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Partnr: ");
            sb.Append(Part);
            sb.Append(" Color: ");
            sb.Append(Color);
            sb.Append(" Quantity: ");
            sb.Append(Quantity);
            if (PriceSet.Count > 0)
            {
                sb.Append(Environment.NewLine);
                if (Quantity <= PriceSet.Sum(p => p.Quantity))
                    sb.AppendLine("Sufficient in stock");
                else
                    sb.AppendLine("INsufficient in stock!");
                ToBuy = Quantity;
                for (int i = 0; ToBuy > 0 && PriceSet.Count > i; i++)
                {
                    if (ToBuy < PriceSet[i].Quantity)
                    {
                        sb.Append("Store ");
                        sb.Append(PriceSet[i].Store.Id.ToString());
                        sb.Append(" has all remaining pieces for ");
                        TotalPrice += (ToBuy * PriceSet[i].Price);
                        sb.AppendLine(TotalPrice.ToString("C"));
                        ToBuy = 0;
                    }
                    else
                    {
                        sb.Append("Store ");
                        sb.Append(PriceSet[i].Store.Id.ToString());
                        sb.Append(" has ");
                        sb.Append(PriceSet[i].Quantity);
                        sb.Append(" pieces for ");
                        TotalPrice += (PriceSet[i].Quantity * PriceSet[i].Price);
                        sb.AppendLine(TotalPrice.ToString("C"));
                        ToBuy -= PriceSet[i].Quantity;
                    }
                }
            }
            else
            {
                sb.AppendLine(Environment.NewLine + "Not for sale at the moment!");
            }

            return sb.ToString();
        }
    }
}