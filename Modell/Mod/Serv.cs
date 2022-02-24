using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Demo
{
    public partial class Serv
    {
        public Nullable<double> Sale { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public bool IsHaveDiscount
        {
            get
            {
                if (Sale > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public double? ActualPrice
        {
            get
            {
                if (IsHaveDiscount)
                {
                    return Math.Round(Convert.ToDouble(Cost * Convert.ToDecimal((1 - Sale))), 2);
                }
                else
                {
                    return Math.Round(Convert.ToDouble(Cost), 2);
                }
            }
        }
        public string DiscountToString
        {
            get
            {
                return $"{Convert.ToString(Sale * 100)}%";
            }
        }
        public static bool IsClient { get; set; }
    }
}
