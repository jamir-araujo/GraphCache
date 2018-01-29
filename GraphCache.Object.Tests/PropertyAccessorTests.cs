using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class PropertyAccessorTests
    {
        private readonly Type _dataType;
        private readonly PropertyInfo _propertyInfo;
        private readonly PropertyAccessor<Data, int> _defaultAccessor;

        public PropertyAccessorTests()
        {
            _dataType = typeof(Data);
            _propertyInfo = _dataType.GetProperty("Value");

            _defaultAccessor = new PropertyAccessor<Data, int>(_propertyInfo, data => data.Value, (data, value) => data.Value = value);
        }

        [Fact]
        public void Constructor_Should_Throw_When_PropertyInfoIsNull()
        {
            Assert.Throws<ArgumentNullException>("propertyInfo", () => new PropertyAccessor<Data, int>(null, (a) => 0, (a, b) => { }));
        }

        [Fact]
        public void Constructor_Should_Throw_When_GetterIsNull()
        {
            Assert.Throws<ArgumentNullException>("getter", () => new PropertyAccessor<Data, int>(_propertyInfo, null, (a, b) => { }));
        }

        [Fact]
        public void Constructor_Should_Throw_When_SetterIsNull()
        {
            Assert.Throws<ArgumentNullException>("setter", () => new PropertyAccessor<Data, int>(_propertyInfo, a => 0, null));
        }

        [Fact]
        public void GetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _defaultAccessor.GetValue(null));
        }

        [Fact]
        public void GetValue_Should_ReturnTheCorrectValue()
        {
            var data = new Data
            {
                Value = 1
            };

            var value = (int)_defaultAccessor.GetValue(data);

            Assert.Equal(data.Value, value);
        }

        [Fact]
        public void SetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _defaultAccessor.SetValue(null, 0));
        }

        [Fact]
        public void SetValue_Should_Throw_When_ValueIsNull()
        {
            Assert.Throws<ArgumentNullException>("value", () => _defaultAccessor.SetValue(new Data(), null));
        }

        [Fact]
        public void SetValue_Should_SetTheValueProperty()
        {
            var data = new Data();

            _defaultAccessor.SetValue(data, 30);

            Assert.Equal(30, data.Value);
        }

        class Data
        {
            public int Value { get; set; }
        }
    }
}
