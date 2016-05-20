using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Infrastructure.Utility.Extension
{
    public static class PirceExtention
    {
        public static string ToString(this string obj, string currency)
        {
            long price = 0;

            if (long.TryParse(obj, out price))
            {
                if (price == 0)
                {
                    return "0 " + currency;
                }

                return string.Format("{0} {1}", price.ToString("##,###"), currency);
            }

            return "";
        }

        public static string ToStringToman(this string obj)
        {
            long price = 0;

            if (long.TryParse(obj, out price))
            {
                if (price == 0)
                {
                    return "0 " + "تومان";
                }

                return string.Format("{0} {1}", price.ToString("##,###"), "تومان");
            }
            return "";
        }

        public static string ToStringRial(this string obj)
        {
            long price = 0;

            if (long.TryParse(obj, out price))
            {
                if (price == 0)
                {
                    return "0 " + "ریال";
                }

                return string.Format("{0} {1}", price.ToString("##,###"), "ریال");
            }
            return "";
        }

        public static string ToStringToman(this long obj)
        {
            if (obj == 0)
            {
                return "0 " + "تومان";
            }

            return string.Format("{0} {1}", obj.ToString("##,###"), "تومان");


        }

        public static string ToStringRial(this long obj)
        {
            if (obj == 0)
            {
                return "0 " + "ریال";
            }

            return string.Format("{0} {1}", obj.ToString("##,###"), "ریال");


        }
    }
}
