using System;
using System.Reflection;
using static GraphCache.Check;

namespace GraphCache.Object
{
    public abstract class PropertyAccessor
    {
        public PropertyInfo PropertyInfo { get; }

        protected PropertyAccessor(PropertyInfo propertyInfo)
        {
            NotNull(propertyInfo, nameof(propertyInfo));

            PropertyInfo = propertyInfo;
        }

        public abstract object GetValue(object owner);
        public abstract void SetValue(object owner, object value);
    }

    public class PropertyAccessor<TOwner, TProperty> : PropertyAccessor
    {
        private Func<TOwner, TProperty> _getter;
        private Action<TOwner, TProperty> _setter;

        public PropertyAccessor(PropertyInfo propertyInfo, Func<TOwner, TProperty> getter, Action<TOwner, TProperty> setter)
            : base(propertyInfo)
        {
            NotNull(getter, nameof(getter));
            NotNull(setter, nameof(setter));

            _getter = getter;
            _setter = setter;
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
