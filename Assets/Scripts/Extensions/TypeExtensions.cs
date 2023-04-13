using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class TypeExtensions
    {
        public static List<Type> GetBaseTypes(this Type t, bool includeSelf = false)
        {
            var types = new List<Type>();
            while (t.BaseType != null)
            {
                types.Add(t);
                t = t.BaseType;
            }

            return types;
        }
    }
}