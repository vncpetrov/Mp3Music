using Mp3MusicZone.Auth;
using NUnit.Framework;
using System;

namespace Mp3MusicZone.UnitTests
{
    [TestFixture]
    public class EmailSenderTests
    {
        [Test]
        public void EmailSenderDummyTest()
        {
            EmailSenderService emailSender = new EmailSenderService(new EmailSettings());

            Assert.IsTrue(true);
        }

        [Test]
        public void EmailSenderDummyTest2()
        {
            EmailSettings emailSettings = new EmailSettings();
            emailSettings.Domain = "Test"; 

            Assert.IsNotNull(emailSettings.Domain);
        }
    }
}
