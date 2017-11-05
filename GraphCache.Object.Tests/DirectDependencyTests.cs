using Moq;
using System;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class DirectDependencyTests
    {
        private readonly Mock<PropertyAccessor> _propertyAccessorMock;
        private readonly string _defaultKey;
        private readonly DirectDependency _directDependency;

        public DirectDependencyTests()
        {
            _propertyAccessorMock = new Mock<PropertyAccessor>(MockBehavior.Strict);
            _defaultKey = "key = 1";

            _directDependency = new DirectDependency(_propertyAccessorMock.Object, _defaultKey);
        }

        [Fact]
        public void Constructor_Should_Throw_When_PropertyAccessorIsNull()
        {
            Assert.Throws<ArgumentNullException>("propertyAccessor", () => new DirectDependency(null, _defaultKey));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_KeyIsNullEmptyOrWhiteSpaces(string key)
        {
            Assert.Throws<ArgumentNullException>("key", () => new DirectDependency(_propertyAccessorMock.Object, key));
        }

        [Fact]
        public void Key_Should_HaveTheValuePassedOnConstructor()
        {
            var key = "key";
            var dependency = new DirectDependency(_propertyAccessorMock.Object, key);

            Assert.Equal(key, dependency.Key);
        }

        [Fact]
        public void GetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _directDependency.GetValue(null));
        }

        [Fact]
        public void GetValeu_Should_ReturnTheValueReturnedByPropertyAccessor()
        {
            var owner = new object();
            var value = new object();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(owner))
                .Returns(value)
                .Verifiable();

            var ddpValue = _directDependency.GetValue(owner);

            Assert.Same(value, ddpValue);
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _directDependency.SetValue(null, 1));
        }

        [Fact]
        public void SetValue_Should_Throw_When_ValueIsNull()
        {
            Assert.Throws<ArgumentNullException>("value", () => _directDependency.SetValue(new { }, null));
        }

        [Fact]
        public void SetValue_Should_CallSetValueOnPropertyAccessor()
        {
            var owner = new object();
            var value = new object();

            _propertyAccessorMock
                .Setup(pa => pa.SetValue(owner, value))
                .Verifiable();

            _directDependency.SetValue(owner, value);

            _propertyAccessorMock.VerifyAll();
        }
    }
}
