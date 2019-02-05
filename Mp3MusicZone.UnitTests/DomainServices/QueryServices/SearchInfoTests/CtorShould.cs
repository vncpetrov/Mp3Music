namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.SearchInfoTests
{
    using Mp3MusicZone.DomainServices.QueryServices;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CtorShould
    {
        [Test]
        public void SetProperSearchTermWhenTheObjectIsConstructed()
        {
            string expectedSearchTerm = "unit testing";

            // Arrange && Act
            SearchInfo sut = new SearchInfo(expectedSearchTerm);

            // Arrange
            string actualSearchTerm = sut.SearchTerm;

            Assert.AreEqual(expectedSearchTerm, actualSearchTerm);
        }
    }
}
