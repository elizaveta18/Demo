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
        /// <summary>
        /// Проверяет наличие скидки
        /// </summary>
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
        public Nullable<decimal> Cost { get; set; }
        public Nullable<double> Sale { get; set; }

        /// <summary>
        /// Возвращает итоговую сумму за услугу с учётом скидки
        /// </summary>
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
