using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Extensions
{
    public static class JsonExtensions
    {
        public static string ObjectToJson<T>(this T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }
        public static T JsonToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default;

            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }
    }
}
