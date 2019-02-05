namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.PageInfoTests
{
    using Mp3MusicZone.DomainServices.QueryServices;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class PageSizeShould
    {
        [Test]
        public void ReturnProperValueWhenGetMethodIsCalled()
        {
            int expectedPageSize = 5;

            // Arrange
            PageInfo sut = new PageInfo(0, expectedPageSize);

            // Act
            int actualPageSize = sut.PageSize;

            // Assert
            Assert.AreEqual(expectedPageSize, actualPageSize);
        }

        [Test]
        public void SetProperValueWhenSetMethodIsCalled()
        {
            int expectedPageSize = 5;

            // Arrange
            PageInfo sut = new PageInfo(0, 0);

            // Act
            sut.PageSize = expectedPageSize;
            int actualPageSize = sut.PageSize;

            // Assert
            Assert.AreEqual(expectedPageSize, actualPageSize);
        }
    }
}
