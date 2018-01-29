using System;
using System.Reflection;
using static GraphCache.Check;

namespace GraphCache.Object
{
    internal abstract class PropertyAccessor
    {
        public abstract PropertyInfo PropertyInfo { get; }

        public abstract object GetValue(object owner);
        public abstract void SetValue(object owner, object value);
    }

    internal class PropertyAccessor<TOwner, TProperty> : PropertyAccessor
    {
        private readonly Func<TOwner, TProperty> _getter;
        private readonly Action<TOwner, TProperty> _setter;
        private readonly PropertyInfo _propertyInfo;

        public override PropertyInfo PropertyInfo => _propertyInfo;

        public PropertyAccessor(PropertyInfo propertyInfo, Func<TOwner, TProperty> getter, Action<TOwner, TProperty> setter)
        {
            NotNull(getter, nameof(getter));
            NotNull(setter, nameof(setter));
            NotNull(propertyInfo, nameof(propertyInfo));

            _getter = getter;
            _setter = setter;
            _propertyInfo = propertyInfo;
        }


        public override object GetValue(object owner)
        {
            NotNull(owner, nameof(owner));

            return _getter((TOwner)owner);
        }

        public override void SetValue(object owner, object value)
        {
            NotNull(owner, nameof(owner));
            NotNull(value, nameof(value));

            _setter((TOwner)owner, (TProperty)value);
        }
    }
}
