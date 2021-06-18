using System;
using System.Collections.Generic;
using NorthwoodLib.Pools;

namespace SCP343
{
    public class ds<T>
    {
        public static readonly ds<T> Shared = new ds<T>();
        public void Return(List<T> list)
        {
            T d = default;
            list.Add(d);
        }
    }
}
