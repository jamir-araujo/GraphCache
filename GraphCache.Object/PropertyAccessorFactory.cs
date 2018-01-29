using System;
using System.Reflection;
using static GraphCache.Check;

namespace GraphCache.Object
{
    internal interface IPropertyAccessorFactory
    {
        PropertyAccessor Create(PropertyInfo property);
    }

    public class PropertyAccessorFactory : IPropertyAccessorFactory
    {
        private static MethodInfo _generatePropertyAccessorMethodInfo = typeof(PropertyAccessorFactory)
            .GetMethod(nameof(GeneratePropertyAccessor), BindingFlags.Static | BindingFlags.NonPublic);

        public PropertyAccessor Create(PropertyInfo property)
        {
            NotNull(property, nameof(property));

            var generatePropertyAccessor = _generatePropertyAccessorMethodInfo.MakeGenericMethod(property.DeclaringType, property.PropertyType);

            return (PropertyAccessor)generatePropertyAccessor.Invoke(null, new[] { property });
        }

        private static PropertyAccessor GeneratePropertyAccessor<TOwner, TProperty>(PropertyInfo property)
        {
            var getter = (Func<TOwner, TProperty>)Delegate.CreateDelegate(typeof(Func<TOwner, TProperty>), property.GetMethod);
            var setter = (Action<TOwner, TProperty>)Delegate.CreateDelegate(typeof(Action<TOwner, TProperty>), property.SetMethod);

            return new PropertyAccessor<TOwner, TProperty>(property, getter, setter);
        }
    }
}
