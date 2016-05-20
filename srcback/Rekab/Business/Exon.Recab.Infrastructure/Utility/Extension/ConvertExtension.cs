using System;
using System.Linq;

namespace Exon.Recab.Infrastructure.Utility.Extension
{
   public static class ConvertExtension
    {

        public static Double StringToDouble(this string obj)
        {

            Double data = 0;

            bool doteCame = false;

            int doteCount = 0;

            char[] ApplyChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

            foreach (char item in obj.ToArray())
            {
                if (ApplyChar.Any(c => c == item))
                {


                    if (doteCame && item != '.')
                    {
                        doteCount++;

                        data = data + ((Math.Pow(10 ,-doteCount)) * Convert.ToInt16(item.ToString()));
                    }
                    else
                    {

                        if (item == '.')
                        {
                            doteCame = true;
                        }
                        else {

                            data = (data * 10) + Convert.ToInt16(item.ToString());

                        }
                    }

                }

                else
                {
                    return data;
                }

            }

            return data;

        }
    }
}
