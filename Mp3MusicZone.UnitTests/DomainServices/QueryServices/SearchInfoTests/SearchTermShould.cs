namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.SearchInfoTests
{
    using Mp3MusicZone.DomainServices.QueryServices;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SearchTermShould
    { 
        [Test]
        public void ReturnEmptyStringWhenTheSearchTermIsNullWhenTheGetMethodIsCalled()
        {
            string expectedSearchTerm = string.Empty;

            // Arrange
            SearchInfo sut = new SearchInfo(null);

            // Act
            string actualSearchTerm = sut.SearchTerm;

            // Assert
            Assert.AreEqual(expectedSearchTerm, actualSearchTerm);
        }

        [Test]
        public void SetProperValueWhenSetMethodIsCalled()
        {
            string expectedSearchTerm = "UnitTesting";

            // Arrange
            SearchInfo sut = new SearchInfo(expectedSearchTerm);

            // Act
            string actualSearchTerm = sut.SearchTerm;

            // Assert
            Assert.AreEqual(expectedSearchTerm, actualSearchTerm);
        }
    }
}
