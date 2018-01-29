using GraphCache.Core;
using System.Collections;
using static GraphCache.Check;

namespace GraphCache.Object
{
    internal class DirectIndexedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _propertyAccessor;
        private readonly int _index;
        private readonly string _key;

        public override string Key => _key;

        public DirectIndexedDependency(PropertyAccessor propertyAccessor, int index, string key)
        {
            NotNull(propertyAccessor, nameof(propertyAccessor));
            GreaterThanZero(index, nameof(index));
            NotNullOrWhiteSpace(key, nameof(key));

            _propertyAccessor = propertyAccessor;
            _index = index;
            _key = key;
        }

        public override object GetValue(object owner)
        {
            NotNull(owner, nameof(owner));

            if (TryGetList(owner, out var list))
            {
                return list[_index];
            }
            else
            {
                return null;
            }
        }

        public override void SetValue(object owner, object value)
        {
            NotNull(owner, nameof(owner));
            NotNull(value, nameof(value));

            if (TryGetList(owner, out var list))
            {
                if (list[_index] != null)
                {
                    list[_index] = value;
                }
            }
        }

        private bool TryGetList(object owner, out IList list)
        {
            list = _propertyAccessor.GetValue(owner) as IList;

            return (list != null && list.Count > _index);
        }
    }
}