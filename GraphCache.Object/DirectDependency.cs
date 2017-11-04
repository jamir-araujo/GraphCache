using GraphCache.Core;
using static GraphCache.Check;

namespace GraphCache.Object
{
    public class DirectDependency : Dependency<object>
    {
        private PropertyAccessor _propertyAccessor;
        private string _key;

        public override string Key => _key;

        public DirectDependency(PropertyAccessor propertyAccessor, string key)
        {
            NotNull(propertyAccessor, nameof(propertyAccessor));
            NotNullOrWhiteSpace(key, nameof(key));

            _propertyAccessor = propertyAccessor;
            _key = key;
        }

        public override object GetValue(object owner)
        {
            NotNull(owner, nameof(owner));

            return _propertyAccessor.GetValue(owner);
        }

        public override void SetValue(object owner, object value)
        {
            NotNull(owner, nameof(owner));
            NotNull(value, nameof(value));

            _propertyAccessor.SetValue(owner, value);
        }
    }
}
