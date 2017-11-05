using System.Reflection;

namespace GraphCache.Object
{
    public abstract class PropertyAccessor
    {
        public PropertyInfo PropertyInfo { get; }

        public abstract object GetValue(object owner);
        public abstract void SetValue(object owner, object value);
    }
}
