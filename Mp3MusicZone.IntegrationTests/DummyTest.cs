using Mp3MusicZone.Auth;
using Mp3MusicZone.EfDataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            EmailSenderService sender = new EmailSenderService(TestsInitializer.EmailSettings);

            Task.Run(async () =>
            {
                await sender.SendEmailAsync("b2h.klo@abv.bg", "Testing Integration Testing", "Thats for the tests");
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
