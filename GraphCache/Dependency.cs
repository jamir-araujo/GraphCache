using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache.Core
{
    public abstract class Dependency<T>
    {
        public abstract string Key { get; }

        public abstract T GetValue(T owner);
        public abstract void SetValue(T owner, object value);
    }
}
