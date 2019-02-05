namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.PageInfoTests
{
    using Mp3MusicZone.DomainServices.QueryServices;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class PageShould
    {
        [Test]
        public void ReturnProperValueWhenGetMethodIsCalled()
        {
            int expectedPage = 5;

            // Arrange
            PageInfo sut = new PageInfo(expectedPage, 3);

            // Act
            int actualPage = sut.Page;

            // Assert
            Assert.AreEqual(expectedPage, actualPage);
        }

        [Test]
        public void SetProperValueWhenSetMethodIsCalled()
        {
            int expectedPage = 5;

            // Arrange
            PageInfo sut = new PageInfo(0, 0);

            // Act
            sut.Page = expectedPage;
            int actualPage = sut.Page;

            // Assert
            Assert.AreEqual(expectedPage, actualPage);
        }
    }
}
