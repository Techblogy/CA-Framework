using System.Linq;
using System.Text.RegularExpressions;

namespace CAF.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static bool ValidateTCKNO(this string text)
        {
            //Boş olamaz
            if (string.IsNullOrEmpty(text)) return false;

            //11 karakter olmalıdır
            if (text.Length != 11) return false;

            //Sadece rakam olmalıdır
            var regex = new Regex("^[0-9]+$");
            if (!regex.IsMatch(text)) return false;

            //Sıfır ile başlayamaz
            if (text[0] == '0') return false;

            //1'den 10. karaktere kadar rakamların toplamının mod 10 sonucu ile 11. rakam eşit olmalıdır.
            var sum10 = text.Substring(0, 10).ToList().Select(x => int.Parse(x.ToString())).Sum();
            var lasNumber = int.Parse(text[10].ToString());

            if (sum10 % 10 != lasNumber) return false;


            return true;
        }

    }
}
