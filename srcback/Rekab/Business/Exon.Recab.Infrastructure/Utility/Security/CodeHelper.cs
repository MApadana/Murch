using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Exon.Recab.Infrastructure.Utility.Security
{
    public class CodeHelper
    {
        public static string NewKey()
        {
            Dictionary<string, char> dictionary = new Dictionary<string, char>();

            int k = 0;
            for (int i = 48; i < 58; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            for (int i = 65; i < 91; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            for (int i = 97; i < 123; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            long randomKey = 0;

            DateTime date = DateTime.UtcNow;

            randomKey = date.Year - 2000;
            randomKey = randomKey * 101 + date.Month;
            randomKey = randomKey * 101 + date.Day;
            randomKey = randomKey * 101 + date.Hour;
            randomKey = randomKey * 101 + date.Minute;
            randomKey = randomKey * 101 + date.Second;
            randomKey = randomKey * 1001 + date.Millisecond;

            string model = "";

            int cursor = 62;

            while (0 < randomKey)
            {
                long a = randomKey % cursor;

                model = model + dictionary[(a).ToString()];

                randomKey = randomKey / cursor;
            }


            return model;
        }

        public static string NewKey(long id)
        {
            Dictionary<string, char> dictionary = new Dictionary<string, char>();

            int k = 0;
            for (int i = 48; i < 58; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            for (int i = 65; i < 91; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            for (int i = 97; i < 123; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            long randomKey = (long.MaxValue / 1024) - (int.MaxValue + id);

            string model = "";

            int cursor = 62;

            while (0 < randomKey)
            {
                long a = randomKey % cursor;

                model = model + dictionary[(a).ToString()];

                randomKey = randomKey / cursor;
            }


            return model;
        }

        public static string NewVoucher()
        {
            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            byte[] hash = new byte[20];
            randomGenerator.GetNonZeroBytes(hash);

            ulong key = BitConverter.ToUInt64(hash, 5);

            ulong divid = key % 2251875390625;

            if (divid < 64339296875)
                divid = divid + 64339296876;

            ulong randomKey = divid;

            Dictionary<string, char> dictionary = new Dictionary<string, char>();

            uint k = 0;
            for (int i = 49; i < 58; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            for (int i = 97; i < 123; i++)
            {
                dictionary.Add(k.ToString(), Convert.ToChar(i));
                k++;
            }

            string model = "";

            uint cursor = 35;

            while (0 < randomKey)
            {
                ulong a = randomKey % cursor;

                model = model + dictionary[(a).ToString()];

                randomKey = randomKey / cursor;
            }

            return model;


        }

        public static string NewVerification()
        {
            RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();
            byte[] hash = new byte[20];
            randomGenerator.GetNonZeroBytes(hash);

            ulong key = BitConverter.ToUInt64(hash, 5);

            ulong divid = key % 80000;

            divid = divid + 10000;

            return divid.ToString();

        }

        public static string NewToken()
        {
            string part = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "");

            part = part + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "");

            part.Replace("+", "!").Replace("/", "@");

            return new string(part.Take(25).ToArray());


        }
    }
}
