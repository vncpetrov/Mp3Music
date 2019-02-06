namespace Mp3MusicZone.UnitTests.DomainServices.QueryServices.Users.GetUserCount.GetUsersCountTests
{
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class GetUsersCountShould
    {
        [Test]
        public void ImplementIQueryInterface()
        {
            // Arrange && Act && Assert
            Assert.IsTrue(
                typeof(GetUsersCount)
                .GetInterfaces()
                .Any(i => i.IsGenericType 
                          && i.GetGenericTypeDefinition() == typeof(IQuery<>)));
        }
    }
}
