using System;
using System.Globalization;


namespace Exon.Recab.Infrastructure.Utility.Extension
{
    public static class CustomDateTime
    {
        private static PersianCalendar persianCalendar = new PersianCalendar();

        private static string StringOfChar(char chr, int Len)
        {
            string result;
            result = "";
            for (int i = 1; i <= Len; i++)
                result = result + chr;
            return result;
        }

        private static string AddZeroToString(String MString, int Len, Boolean ToLeft)
        {
            String StrRepeatChar;
            if ((Len - MString.Length) > 0)
                StrRepeatChar = StringOfChar('0', Len - MString.Length);
            else
                StrRepeatChar = "";
            if (ToLeft)
                return StrRepeatChar + MString;
            else
                return MString + StrRepeatChar;
        }

        public static DateTime PersianToGregorian(this String SolarDate)
        {
            int Year = 0, Month = 0, Day = 0, Hour = 0, Miniute = 0, Secend = 0, MiliSecond = 0;
            string[] DateSections = SolarDate.Split('/');
            Year = int.Parse(DateSections[0].ToString());
            Month = int.Parse(DateSections[1].ToString());
            Day = int.Parse(DateSections[2].ToString());
            try
            {
                return persianCalendar.ToDateTime(Year, Month, Day, Hour, Miniute, Secend, MiliSecond);
            }
            catch
            {
                return DateTime.Now.Date;
            }
        }

        public static DateTime PersianToGregorianUTC(this String SolarDate)
        {
            int Year = 0, Month = 0, Day = 0, Hour = 0, Miniute = 0, Secend = 0, MiliSecond = 0;
            string[] DateSections = SolarDate.Split('/');
            Year = int.Parse(DateSections[0].ToString());
            Month = int.Parse(DateSections[1].ToString());
            Day = int.Parse(DateSections[2].ToString());
            try
            {
                DateTime temp = persianCalendar.ToDateTime(Year, Month, Day, Hour, Miniute, Secend, MiliSecond);

               temp= temp.AddHours(-3).AddMinutes(-30);
                return temp;
            }
            catch
            {
                return DateTime.Now.Date;
            }
        }

        public static DateTime PersianToGregorianWithDate(this String SolarDate,string SolarTime)
        {
            int Year = 0, Month = 0, Day = 0, Hour = 0, Miniute = 0, Secend = 0, MiliSecond = 0;

            string[] DateSections = SolarDate.Split('/');
            string[] TimeSection = SolarTime.Split(':');

            Year = int.Parse(DateSections[0].ToString());
            Month = int.Parse(DateSections[1].ToString());
            Day = int.Parse(DateSections[2].ToString());

            Hour = int.Parse(TimeSection[0].ToString());
            Miniute = int.Parse(TimeSection[1].ToString());
            Secend = int.Parse(TimeSection[2].ToString());


            try
            {
                return persianCalendar.ToDateTime(Year, Month, Day, Hour, Miniute, Secend, MiliSecond);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string UTCToPersianDateShort(this DateTime obj)
        {
            obj = obj.AddHours(3).AddMinutes(30);
            try
            {
                return persianCalendar.GetYear(obj) + "/" +
                       AddZeroToString(persianCalendar.GetMonth(obj).ToString(), 2, true) + "/" +
                       AddZeroToString(persianCalendar.GetDayOfMonth(obj).ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string UTCToPersianDateLong(this DateTime obj)
        {
            obj = obj.AddHours(3).AddMinutes(30);
            try
            {
                return persianCalendar.GetYear(obj) + "/" +
                       AddZeroToString(persianCalendar.GetMonth(obj).ToString(), 2, true) + "/" +
                       AddZeroToString(persianCalendar.GetDayOfMonth(obj).ToString(), 2, true) + "  " +
                       AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string UTCToPersianTimeLong(this DateTime obj)
        {
            obj = obj.AddHours(3).AddMinutes(30);
            try
            {
                return AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Second.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string UTCToPersianTimeShort(this DateTime obj)
        {
            obj = obj.AddHours(3).AddMinutes(30);
            try
            {
                return AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Second.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string LocalToPersianDateShort(this DateTime obj)
        {
            try
            {
                return persianCalendar.GetYear(obj) + "/" +
                       AddZeroToString(persianCalendar.GetMonth(obj).ToString(), 2, true) + "/" +
                       AddZeroToString(persianCalendar.GetDayOfMonth(obj).ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string LocalToPersianDateLong(this DateTime obj)
        {

            try
            {
                return persianCalendar.GetYear(obj) + "/" +
                       AddZeroToString(persianCalendar.GetMonth(obj).ToString(), 2, true) + "/" +
                       AddZeroToString(persianCalendar.GetDayOfMonth(obj).ToString(), 2, true) + "  " +
                       AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string LocalToPersianTimeLong(this DateTime obj)
        {

            try
            {
                return AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Second.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string LocalToPersianTimeShort(this DateTime obj)
        {

            try
            {
                return AddZeroToString(obj.Hour.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Minute.ToString(), 2, true) + ":" +
                       AddZeroToString(obj.Second.ToString(), 2, true);
            }
            catch
            {
                return "-";
            }
        }

        public static string UTCToPrettyPersian(this DateTime obj)
        {

            DateTime nowTime = DateTime.UtcNow;

            if (nowTime < obj.AddMinutes(5))
                return " لحظاتی قبل";

            if (nowTime < obj.AddMinutes(14))
                return " دقایقی قبل";

            if (nowTime < obj.AddMinutes(29))
                return " یک ربع قبل";

            if (nowTime < obj.AddMinutes(59))
                return " نیم ساعت قبل";

            if (nowTime < obj.AddHours(1).AddMinutes(59))
                return " یک ساعت قبل";

            if (nowTime < obj.AddHours(2).AddMinutes(59))
                return " دو ساعت قبل";

            if (nowTime < obj.AddHours(3).AddMinutes(59))
                return " سه ساعت قبل";

            if (nowTime < obj.AddHours(4).AddMinutes(59))
                return " چهار ساعت قبل";

            if (nowTime < obj.AddHours(5).AddMinutes(59))
                return " پنج ساعت قبل";

            if (nowTime < obj.AddDays(1))
                return " امروز";

            if (nowTime < obj.AddDays(2))
                return "دیروز";

            if (nowTime < obj.AddDays(3))
                return "دو روز قبل";

            if (nowTime < obj.AddDays(4))
                return "سه روز قبل";

            if (nowTime < obj.AddDays(5))
                return "چهار روز قبل";

            if (nowTime < obj.AddDays(6))
                return "پنج روز قبل";

            if (nowTime < obj.AddDays(7))
                return "شش روز قبل";

            if (nowTime < obj.AddDays(8))
                return "یک هفته قبل";

            return nowTime.UTCToPersianDateLong();


        }

        public static string PersioanPrettyTime(this DateTime obj)
        {
            return "";

            
            

        }
    }

}

