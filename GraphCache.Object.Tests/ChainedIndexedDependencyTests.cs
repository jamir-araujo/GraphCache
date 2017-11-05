using GraphCache.Core;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class ChainedIndexedDependencyTests
    {
        private readonly ChainedIndexedDependency _chainedIndexedDependency;
        private readonly Mock<PropertyAccessor> _propertyAccessorMock;
        private readonly Mock<Dependency<object>> _dependencyMock;
        private readonly Mock<IList> _listMock;
        private readonly int _index;
        private readonly object _owner;
        private readonly object _value;

        public ChainedIndexedDependencyTests()
        {
            _propertyAccessorMock = new Mock<PropertyAccessor>(MockBehavior.Strict);
            _dependencyMock = new Mock<Dependency<object>>(MockBehavior.Strict);
            _listMock = new Mock<IList>(MockBehavior.Strict);
            _index = 0;
            _owner = new object();
            _value = new object();

            _chainedIndexedDependency = new ChainedIndexedDependency(_propertyAccessorMock.Object, _index, _dependencyMock.Object);
        }

        [Fact]
        public void Constructor_Should_Throw_When_PropertyAccessorIsNull()
        {
            Assert.Throws<ArgumentNullException>("propertyAccessor", () => new ChainedIndexedDependency(null, _index, _dependencyMock.Object));
        }

        [Fact]
        public void Constructor_Should_Throw_When_IndexIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () => new ChainedIndexedDependency(_propertyAccessorMock.Object, -1, _dependencyMock.Object));
        }

        [Fact]
        public void Constructor_Should_Throw_When_DependencyIsNull()
        {
            Assert.Throws<ArgumentNullException>("dependency", () => new ChainedIndexedDependency(_propertyAccessorMock.Object, _index, null));
        }

        [Fact]
        public void Key_Should_ReturnTheKeyOfTheDependencyProperty()
        {
            var key = "key = 1";

            _dependencyMock
                .Setup(d => d.Key)
                .Returns(key)
                .Verifiable();

            Assert.Equal(key, _chainedIndexedDependency.Key);
            _dependencyMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _chainedIndexedDependency.GetValue(null));
        }

        [Fact]
        public void GetValeu_Should_ReturnNull_When_DependencyGetValueReturnsNull()
        {
            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            var dpValue = _chainedIndexedDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _dependencyMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_PropertyAccessorGetValueReturnsNull()
        {
            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(_value)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_value))
                .Returns(null)
                .Verifiable();

            var dpValue = _chainedIndexedDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_PropertyAccessorGetValueReturnsANotIList()
        {
            var innerValue = new object();
            var stack = new Stack<int>();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(stack)
                .Verifiable();

            var dpValue = _chainedIndexedDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_IndexIsOutsideTheBoundsOfTheList()
        {
            var innerValue = new object();
            var list = new List<int>();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(list)
                .Verifiable();

            var dpValue = _chainedIndexedDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnTheValueAtTheIndex_When_TheIndexFitsTheBoundsOfTheList()
        {
            var innerValue = new object();
            var list = new List<int>() { 1 };

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(list)
                .Verifiable();

            var dpValue = _chainedIndexedDependency.GetValue(_owner);

            Assert.Equal(list[_index], dpValue);
            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _chainedIndexedDependency.SetValue(null, _value));
        }

        [Fact]
        public void SetValeu_Should_Throw_When_ValueIsNull()
        {
            Assert.Throws<ArgumentNullException>("value", () => _chainedIndexedDependency.SetValue(_owner, null));
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_DependencyGetValueReturnsNull()
        {
            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            _chainedIndexedDependency.SetValue(_owner, _value);

            _dependencyMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_PropertyAccessorGetValueReturnsANonIList()
        {
            var innerValue = new object();
            var collectionMock = new Mock<ICollection>(MockBehavior.Strict);

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(collectionMock.Object)
                .Verifiable();

            _chainedIndexedDependency.SetValue(_owner, _value);

            _propertyAccessorMock.VerifyAll();
            _dependencyMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_IndexOfTheListIsNull()
        {
            var innerValue = new object();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(_listMock.Object)
                .Verifiable();

            _listMock
                .Setup(list => list.Count)
                .Returns(0)
                .Verifiable();

            _chainedIndexedDependency.SetValue(_owner, _value);

            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
            _listMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_ElementAtIndexIsNUll()
        {
            var innerValue = new object();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(_listMock.Object)
                .Verifiable();

            _listMock
                .Setup(list => list.Count)
                .Returns(1)
                .Verifiable();

            _listMock
                .Setup(list => list[_index])
                .Returns(null)
                .Verifiable();

            _chainedIndexedDependency.SetValue(_owner, _value);

            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
            _listMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_SetTheValueAtTheIndex_When_IndexIsTheBoundsOfTheList()
        {
            var innerValue = new object();

            _dependencyMock
                .Setup(d => d.GetValue(_owner))
                .Returns(innerValue)
                .Verifiable();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(innerValue))
                .Returns(_listMock.Object)
                .Verifiable();

            _listMock
                .Setup(list => list.Count)
                .Returns(1)
                .Verifiable();

            _listMock
                .Setup(list => list[_index])
                .Returns(_value)
                .Verifiable();

            _listMock
                .SetupSet(list => list[_index] = _value)
                .Verifiable();

            _chainedIndexedDependency.SetValue(_owner, _value);

            _dependencyMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
            _listMock.VerifyAll();
        }
    }
}
