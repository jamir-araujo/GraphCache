using System;
using System.Collections.Generic;
using System.Text;

namespace GraphCache.Core
{
    public interface IConvention<T>
    {
        bool FitsConvetion(T value);
        string CreateKey(T value);
    }
}
