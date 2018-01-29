using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class PropertyAccessorFactoryTests
    {
        private PropertyAccessorFactory _factory;

        public PropertyAccessorFactoryTests()
        {
            _factory = new PropertyAccessorFactory();
        }

        [Fact]
        public void Create_Should_Throw_When_PropertyIsNull()
        {
            Assert.Throws<ArgumentNullException>("property", () => _factory.Create(null));
        }

        [Fact]
        public void Create_Should_ReturnAValidaPropertyAccessor()
        {
            var propertyInfo = typeof(Data).GetProperty("Value");

            var propertyAccessor = _factory.Create(propertyInfo);

            Assert.Same(propertyInfo, propertyAccessor.PropertyInfo);
        }

        public class Data
        {
            public int Value { get; set; }
        }
    }
}
