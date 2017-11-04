using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GraphCache.Object.Tests
{
    public class IndexedDirectDependencyTests
    {
        private readonly Mock<PropertyAccessor> _propertyAccessorMock;
        private readonly Mock<IList> _listMock;
        private readonly string _defaultKey;
        private readonly int _index;
        private readonly object _owner;
        private readonly IndexedDirectDependency _indexedDirectDependency;

        public IndexedDirectDependencyTests()
        {
            _propertyAccessorMock = new Mock<PropertyAccessor>(MockBehavior.Strict);
            _listMock = new Mock<IList>(MockBehavior.Strict);
            _defaultKey = "key = 1";
            _index = 0;
            _owner = new { Data = "test" };

            _indexedDirectDependency = new IndexedDirectDependency(_propertyAccessorMock.Object, _index, _defaultKey);
        }

        [Fact]
        public void Constructor_Should_Throw_When_PropertyAccessorIsNull()
        {
            Assert.Throws<ArgumentNullException>("propertyAccessor", () => new IndexedDirectDependency(null, 0, _defaultKey));
        }

        [Fact]
        public void Constructor_Should_Throw_When_IndexIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>("index", () => new IndexedDirectDependency(_propertyAccessorMock.Object, -1, _defaultKey));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_KeyIsNullEmptyOrWhiteSpaces(string key)
        {
            Assert.Throws<ArgumentNullException>("key", () => new IndexedDirectDependency(_propertyAccessorMock.Object, 0, key));
        }

        [Fact]
        public void Key_Should_HaveTheValuePassedOnConstructor()
        {
            var key = "key";
            var dependency = new IndexedDirectDependency(_propertyAccessorMock.Object, 0, key);

            Assert.Equal(dependency.Key, key);
        }

        [Fact]
        public void GetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _indexedDirectDependency.GetValue(null));
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_PropertyAccessorReturnsNull()
        {
            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            var dpValue = _indexedDirectDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_PropertyAccessorReturnsANotAnIList()
        {
            var value = new Stack<int>();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(value)
                .Verifiable();

            var dpValue = _indexedDirectDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnNull_When_IndexIsOutsideTheBoundsOfTheList()
        {
            var list = new List<int>();

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(list)
                .Verifiable();

            var dpValue = _indexedDirectDependency.GetValue(_owner);

            Assert.Null(dpValue);
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void GetValue_Should_ReturnTheValueAtTheIndex_When_TheIndexFitsTheBoundsOfTheList()
        {
            var list = new List<int> { 1 };

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(list)
                .Verifiable();

            var dpValue = _indexedDirectDependency.GetValue(_owner);

            Assert.Equal(list[0], dpValue);
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_Throw_When_OwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => _indexedDirectDependency.SetValue(null, 1));
        }

        [Fact]
        public void SetValue_Should_Throw_When_ValueIsNull()
        {
            Assert.Throws<ArgumentNullException>("value", () => _indexedDirectDependency.SetValue(new { }, null));
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_PropertyAccessorReturnsNull()
        {
            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(null)
                .Verifiable();

            _indexedDirectDependency.SetValue(_owner, 1);

            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_IndexIsOutsiteOfTheBoundOfTheList()
        {
            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(_listMock.Object)
                .Verifiable();

            _listMock
                .Setup(list => list.Count)
                .Returns(0)
                .Verifiable();

            _indexedDirectDependency.SetValue(_owner, 1);

            _listMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_DoNothing_When_IndexOfTheListIsNull()
        {
            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
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

            _indexedDirectDependency.SetValue(_owner, 1);

            _listMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }

        [Fact]
        public void SetValue_Should_SetTheValueAtTheIndex_When_IndexIsTheBoundsOfTheList()
        {
            var newValue = 1;

            _propertyAccessorMock
                .Setup(pa => pa.GetValue(_owner))
                .Returns(_listMock.Object)
                .Verifiable();

            _listMock
                .Setup(list => list.Count)
                .Returns(1)
                .Verifiable();

            _listMock
                .Setup(list => list[_index])
                .Returns(0)
                .Verifiable();

            _listMock
                .SetupSet(list => list[_index] = newValue)
                .Verifiable();

            _indexedDirectDependency.SetValue(_owner, newValue);

            _listMock.VerifyAll();
            _propertyAccessorMock.VerifyAll();
        }
    }
}
