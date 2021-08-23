using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CAF.Core
{
    public class HttpPutObject<T> : IHttpPutObject
    {
        public bool IsFilled { get; private set; }

        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                IsFilled = true;
                _value = value;
            }
        }

        public HttpPutObject(T value)
        {
            Value = value;
            IsFilled = true;
        }

        #region implicit explicit
        public static implicit operator HttpPutObject<T>(T value)
        {
            return new HttpPutObject<T>(value);
        }

        public static explicit operator T(HttpPutObject<T> value)
        {
            if (value == null)
            {
                return default(T);
            }
            return value.Value;
        }
        #endregion

        #region IHttpPutObject
        public object GetValue()
        {
            return Value;
        }

        public IHttpPutObject Clone()
        {
            return new HttpPutObject<T>(Value);
        }
        #endregion

        #region == != > < >= <=

        public static bool operator ==(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            if ((!(left is null || ReferenceEquals(left.Value, null)) && (right is null || ReferenceEquals(right.Value, null))) ||
                ((left is null || ReferenceEquals(left.Value, null)) && !(right is null || ReferenceEquals(right.Value, null))))
            {
                return false;
            }
            return (left is null && right is null) || ReferenceEquals(left, right) ||
                    ((left is null || ReferenceEquals(left.Value, null)) && (right is null || ReferenceEquals(right.Value, null))) ||
                    EqualityComparer<T>.Default.Equals(left.Value, right.Value);
        }

        public static bool operator !=(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            if ((left is null && right is null) || ReferenceEquals(left, right) ||
                ((left is null || ReferenceEquals(left.Value, null)) && (right is null || ReferenceEquals(right.Value, null))) ||
                (!(left is null) && !(right is null) && EqualityComparer<T>.Default.Equals(left.Value, right.Value)))
            {
                return false;
            }

            return true;
        }

        public static bool operator >(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            Type comparableType = GetComparableType();
            ControlIComparableImplementation(comparableType);

            //null > value = false
            if (left is null || ReferenceEquals(left.Value, null))
            {
                return false;
            }

            //value > null = true
            if (right is null || ReferenceEquals(right.Value, null))
            {
                return true;
            }

            int result = InvokeCompareTo(left, right, comparableType);
            return result > 0;
        }

        public static bool operator <(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            Type comparableType = GetComparableType();
            ControlIComparableImplementation(comparableType);

            //null < value = true
            if (left is null || ReferenceEquals(left.Value, null))
            {
                return true;
            }

            //value < null = false
            if (right is null || ReferenceEquals(right.Value, null))
            {
                return false;
            }

            int result = InvokeCompareTo(left, right, comparableType);
            return result < 0;
        }

        public static bool operator >=(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            Type comparableType = GetComparableType();
            ControlIComparableImplementation(comparableType);

            //null >= value = false
            if (left is null || ReferenceEquals(left.Value, null))
            {
                return false;
            }

            //value >= null = true
            if (right is null || ReferenceEquals(right.Value, null))
            {
                return true;
            }

            int result = InvokeCompareTo(left, right, comparableType);
            return result >= 0;
        }

        public static bool operator <=(HttpPutObject<T> left, HttpPutObject<T> right)
        {
            Type comparableType = GetComparableType();
            ControlIComparableImplementation(comparableType);

            //null <= value = true
            if (left is null || ReferenceEquals(left.Value, null))
            {
                return true;
            }

            //value <= null = false
            if (right is null || ReferenceEquals(right.Value, null))
            {
                return false;
            }

            int result = InvokeCompareTo(left, right, comparableType);
            return result <= 0;
        }
        #endregion

        #region Private
        private static void ControlIComparableImplementation(Type comparableType)
        {
            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (!type.GetInterfaces().Any(i => i == comparableType))
            {
                throw new System.Exception($"Cant compare {typeof(T)}. Its not implements {typeof(IComparable)}");
            }
        }
        private static int InvokeCompareTo(HttpPutObject<T> left, HttpPutObject<T> right, Type comparableType)
        {
            //var changed = Convert.ChangeType(left.Value, comparableType);
            MethodInfo methodInfo = comparableType.GetMethod("CompareTo");
            object[] prms = new object[] { right.Value };
            int result = (int)methodInfo.Invoke(left.Value, prms);
            return result;
        }
        private static Type GetComparableType()
        {
            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            var genericBase = typeof(IComparable<>);
            return genericBase.MakeGenericType(type);
        }
        #endregion


    }

    public interface IHttpPutObject
    {
        IHttpPutObject Clone();

        object GetValue();
    }
}
