using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CAF.Core.Modules.HttpPutObject
{
    public class HttpPutObjectConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetInterface("IHttpPutObject") != null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retval = null;
            Type[] underlyingTypes = objectType.GetGenericArguments();

            if (underlyingTypes.Length != 1)
            {
                //todo spesific exception
                throw new System.Exception($"{objectType} is not valid type for convert to HttpPutObject");
            }
            else
            {
                Type underlyingHttpPutObjectType = underlyingTypes[0];
                if (reader.Value != null)
                {
                    retval = GetHttpPutObjectObject(reader.Value, reader.ValueType, typeof(HttpPutObject<>), underlyingHttpPutObjectType, objectType);
                }
            }

            return retval;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type type = value.GetType();
            PropertyInfo valuePropertyInfo = type.GetProperty("Value");
            if (valuePropertyInfo != null)
            {
                object val = valuePropertyInfo.GetValue(value);

                if (val != null)
                {
                    JToken t = JToken.FromObject(val);
                    t.WriteTo(writer);
                }
                else
                {
                    writer.WriteNull();
                }
            }
            else
            {
                throw new System.Exception("Object that implement 'IHttpPutObject' must have 'Value' property");
            }
        }

        private static object GetHttpPutObjectObject(object value, Type valueType, Type baseType, Type underlyingType, Type objectType)
        {
            if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                Type nullableUnderlyingType = Nullable.GetUnderlyingType(underlyingType);
                value = GetHttpPutObjectObject(value, valueType, typeof(Nullable<>), nullableUnderlyingType, objectType);
            }
            else if (valueType != underlyingType)
            {
                if (!(value is IConvertible))
                {
                    //todo spesific exception
                    throw new System.Exception($"{valueType} must be implemented IConvertible interface to convert to {objectType}");
                }
                value = Convert.ChangeType(value, underlyingType);
            }

            var genericBase = baseType;
            var combinedType = genericBase.MakeGenericType(underlyingType);
            return Activator.CreateInstance(combinedType, value);
        }
    }
}
