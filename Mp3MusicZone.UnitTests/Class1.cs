using Mp3MusicZone.Auth;
using NUnit.Framework;
using System;

namespace Mp3MusicZone.UnitTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void EmailSenderDummyTest()
        {
            EmailSenderService emailSender = new EmailSenderService(new EmailSettings());

            Assert.IsTrue(true); 
        }
    }
}
