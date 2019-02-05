namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.PageInfoTests
{
    using Mp3MusicZone.DomainServices.QueryServices;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void SetProperPageWhenTheObjectIsConstructed()
        {
            int expectedPage = 5;

            // Arrange && Act
            PageInfo sut = new PageInfo(expectedPage, 3);

            // Assert
            Assert.AreEqual(expectedPage, sut.Page);
        }

        [Test]
        public void SetProperPageSizeWhenTheObjectIsConstructed()
        {
            int expectedPageSize = 5;

            // Arrange && Act
            PageInfo sut = new PageInfo(3, expectedPageSize);

            // Assert
            Assert.AreEqual(expectedPageSize, sut.PageSize);
        }
    }
}
