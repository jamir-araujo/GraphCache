using GraphCache.Core;
using System;
using System.Collections;
using static GraphCache.Check;

namespace GraphCache.Object
{
    public class ChainedIndexedDependency : Dependency<object>
    {
        private readonly PropertyAccessor _propertyAccessor;
        private readonly int _index;
        private readonly Dependency<object> _dependency;

        public override string Key => _dependency.Key;

        public ChainedIndexedDependency(PropertyAccessor propertyAccessor, int index, Dependency<object> dependency)
        {
            NotNull(propertyAccessor, nameof(propertyAccessor));
            GreaterThanZero(index, nameof(index));
            NotNull(dependency, nameof(dependency));

            _propertyAccessor = propertyAccessor;
            _index = index;
            _dependency = dependency;
        }

        public override object GetValue(object owner)
        {
            NotNull(owner, nameof(owner));

            if (TryGetList(owner, out var list))
            {
                return list[_index];
            }

            return null;
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
            list = null;

            owner = _dependency.GetValue(owner);
            if (owner != null)
            {
                list = _propertyAccessor.GetValue(owner) as IList;
                if (list != null && list.Count > _index)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
