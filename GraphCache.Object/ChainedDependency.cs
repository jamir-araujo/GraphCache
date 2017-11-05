using GraphCache.Core;
using static GraphCache.Check;

namespace GraphCache.Object.Tests
{
    public class ChainedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _propertyAccessor;
        private readonly Dependency<object> _dependency;

        public override string Key => _dependency.Key;

        public ChainedDependency(PropertyAccessor propertyAccessor, Dependency<object> dependency)
        {
            NotNull(propertyAccessor, nameof(propertyAccessor));
            NotNull(dependency, nameof(dependency));

            _propertyAccessor = propertyAccessor;
            _dependency = dependency;
        }

        public override object GetValue(object owner)
        {
            NotNull(owner, nameof(owner));

            if (TryGetPropertyOwner(owner, out var propertyOwner))
            {
                return _propertyAccessor.GetValue(propertyOwner);
            }

            return null;
        }

        public override void SetValue(object owner, object value)
        {
            NotNull(owner, nameof(owner));
            NotNull(value, nameof(value));

            if (TryGetPropertyOwner(owner, out var propertyOwner))
            {
                _propertyAccessor.SetValue(propertyOwner, value);
            }
        }

        private bool TryGetPropertyOwner(object owner, out object propertyOwner)
        {
            propertyOwner = _dependency.GetValue(owner);
            return propertyOwner != null;
        }
    }
}
