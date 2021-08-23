using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Helper
{
    public static class ReflectionHelper
    {
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }
    }
}
