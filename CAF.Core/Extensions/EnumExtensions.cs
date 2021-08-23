using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using ExtensionCore;

namespace CAF.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayNameValue(this Enum value)
        {
            if (value == null) return "";

            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null) return "";
            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];


            if (descriptionAttributes == null) return value.ToString();
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
        public static bool IsEnum<T>(this T value) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return values.Contains(value);
        }
        public static List<TEnum> EnumValuesToList<TEnum>(this string enumsValues, char separator) where TEnum : Enum
        {
            if (string.IsNullOrEmpty(enumsValues)) return new List<TEnum>();

            var subRolesStr = enumsValues.Trim(' ');
            if (string.IsNullOrEmpty(subRolesStr)) return new List<TEnum>();

            var ids = subRolesStr.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(x => x.ToTryInt()).Where(x => x > 0).ToList();

            var result = new List<TEnum>();
            foreach (var item in ids)
            {
                var enumType = typeof(TEnum);
                if (Enum.IsDefined(enumType, item))
                {
                    result.Add((TEnum)Enum.ToObject(enumType, item));
                }
            }

            return result;
        }
    }
}
