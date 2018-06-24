using Mp3MusicZone.EfDataAccess;
using NUnit.Framework;
using System;
using System.Linq;

namespace Mp3MusicZone.IntegrationTests
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void DummyTest1()
        {
            MusicZoneDbContext context = new MusicZoneDbContext(TestsInitializer.ConnectionString);
            bool any = context.Users.Any();

            Assert.IsFalse(any);
        }
    }
}
