
using NPOI.SS.UserModel;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CAF.Core.Extensions
{
    public static class CommonExtensions
    {
        public static string GetExceptionMessage(this System.Exception exception, int maxInnerException = 10,
            int currentCount = 0)
        {
            if (exception == null || maxInnerException < currentCount) return "";

            var message = exception.Message;

            if (exception.InnerException != null)
            {
                currentCount++;
                message += Environment.NewLine + GetExceptionMessage(exception.InnerException, maxInnerException, currentCount);
            }

            return message;
        }

        public static string GetMailTemplateString(string templateName)
        {
            var assembly = typeof(CAF.Core.Helper.MailHelper).GetTypeInfo().Assembly;
            Stream resource = assembly.GetManifestResourceStream("CAF.Core.Constant.MailTemplates." + templateName);
            using (StreamReader reader = new StreamReader(resource))
            {
                return reader.ReadToEnd();
            }
        }
        public static string GetBase64StringExtension(this string base64String)
        {
            var mime = base64String.Substring("data:".Length, (base64String.IndexOf(";base64") - "data:".Length));
            return mime.Split('/').LastOrDefault();
        }
        public static string RemovePrefixBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return string.Empty;

            var split = ";base64,";
            if (base64String.Contains(split))
            {
                var base64Split = base64String.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                if (base64Split.Length > 1) base64String = base64Split[1];
            }

            return base64String;
        }
        public static string ToAmountText(this double amount)
        {
            return amount.ToString("C0", new CultureInfo("tr-TR"));
        }
        public static string UppercaseFirst(this string s)
        {
            try
            {
                // Check for empty string.
                if (string.IsNullOrEmpty(s))
                {
                    return string.Empty;
                }
                var words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => char.ToUpper(x[0]) + x.Substring(1));

                return string.Join(" ", words);
            }
            catch
            {
                return s;
            }
        }
        public static string AppendWordWithCount(this string resource, string word, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
                sb.Append(word);

            return resource + sb.ToString();

        }
        /// <summary>
        /// CAF.CoreConstant.Files içindeki dosyanın stream halini verir
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream GetConstantFile(string fileName)
        {
            var assembly = typeof(Helper.MailHelper).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream($"CAF.CoreConstant.Files.{fileName}");
        }
        public static byte[] GetFileBytes(string fileName)
        {
            var assembly = typeof(Helper.MailHelper).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"CAF.CoreConstant.Files.{fileName}");

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        /// <summary>
        /// CAF.CoreConstant.Files içindeki dosyanın içindeki metinini verir
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetStringByConstantFile(string fileName)
        {
            var stream = GetConstantFile(fileName);
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static DateTime? ToDate(this string dateStr)
        {
            if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }
        public static DateTime? ToDate(this ICell cell)
        {
            try
            {
                if (cell.CellType == CellType.Numeric)
                {
                    return cell.DateCellValue;
                }
                else return cell.GetFormattedCellValue().ToDate();
            }
            catch
            {
                return cell.GetFormattedCellValue().ToDate();
            }
        }
        public static T ToEnumByDisplayName<T>(this string enumStr) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            foreach (var item in values)
            {
                if (enumStr.ToLower() == item.GetDisplayNameValue().ToLower()) return item;
            }

            return default(T);
        }
        public static double ToDouble(this string doubleStr)
        {
            if (double.TryParse(doubleStr, out double number))
                return number;
            else return default;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime ToMonthDateTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

    

    }
}
