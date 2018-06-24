using Mp3MusicZone.Auth;
using Mp3MusicZone.EfDataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Console.WriteLine(TestsInitializer.Password);
            Assert.IsTrue(TestsInitializer.Password == "LenovoY720");
        }
    }
}
