using GraphCache.Core;
using Moq;
using System;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class ChainedDependencyTests
    {
        private readonly Mock<PropertyAccessor> _propertyAccessorMock;
        private readonly Mock<Dependency<object>> _dependencyMock;
        private readonly ChainedDependency _chainedDependency;
        private readonly object _owner;
        private readonly object _value;

        public ChainedDependencyTests()
        {
            _propertyAccessorMock = new Mock<PropertyAccessor>(MockBehavior.Strict);
            _dependencyMock = new Mock<Dependency<object>>(MockBehavior.Strict);
            _owner = new object();
            _value = new object();

            _chainedDependency = new ChainedDependency(_propertyAccessorMock.Object, _dependencyMock.Object);
        }

        [Fact]
        public void Constructor_Should_Throw_When_PropertyAccessorIsNull()
        {
            Assert.Throws<ArgumentNullException>("propertyAccessor", () => new ChainedDependency(null, _dependencyMock.Object));
        }

        [Fact]
        public void Constructor_Should_Throw_When_DependencyIsNull()
        {
            Assert.Throws<ArgumentNullException>("dependency", () => new ChainedDependency(_propertyAccessorMock.Object, null));
        }

        [Fact]
        public void Key_Should_ReturnTheKeyOfTheDependencyProperty()
        {
            var key = "key";

            _dependencyMock
                .Setup(d => d.Key)
                .Returns(key)
                .Verifiable();

            Assert.Equal(key, _chainedDependency.Key);
        }

        [Fact]
        public void GetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _chainedDependency.GetValue(null));
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_InnerDependencyReturnsNull()
        {
            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            var dpValue = _chainedDependency.GetValue(_owner);

            Assert.Null(dpValue);
        }

        [Fact]
        public void GetValeu_Should_ReturnTheValueReturnedByPropertyAccessor()
        {
            var innerObject = new object();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerObject)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerObject))
                .Returns(_value)
                .Verifiable();

            var dpValeu = _chainedDependency.GetValue(_owner);

            Assert.Equal(_value, dpValeu);
        }

        [Fact]
        public void SetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _chainedDependency.SetValue(null, _value));
        }

        [Fact]
        public void SetValue_Should_Throw_When_ValueIsNull()
        {
            Assert.Throws<ArgumentNullException>("value", () => _chainedDependency.SetValue(_owner, null));
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_GetValueOnDependencyReturnsNull()
        {
            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            _chainedDependency.SetValue(_owner, _value);

            _propertyAccessorMock.VerifyAll();
            _dependencyMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_CallSetValueOnPropertyAccessor()
        {
            var innerValue = new object();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.SetValue(innerValue, _value))
                .Verifiable();

            _chainedDependency.SetValue(_owner, _value);

            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }
    }
}
