using CAF.Core.Entities;
using CAF.Core.Extensions;

using System;
using System.Globalization;

namespace CAF.Core.Helper
{
    public static class UtilitiesHelper
    {
        public static DateTime DateTimeNow()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now,
                 GetTimeZoneInfo());
        }
        public static TimeZoneInfo GetTimeZoneInfo()
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
        }
        public static string GetRandomCode()
        {
            return GetRandomCode(3);
        }
        public static string GetRandomCode(int depth)
        {
            var str = "";
            for (int i = 0; i < depth; i++)
            {
                str += Guid.NewGuid().ToString();
            }
            string GuidString = Base64Encode(str);
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            return GuidString;

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string GetMonthName(int month)
        {
            CultureInfo cultureInfo = new CultureInfo("tr-TR");
            return new DateTime(2020, month, 1).ToString("MMMM", cultureInfo);
        }

        public static long ToTotalMiliseconds(DateTime dt)
        {
            return (long)(dt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
