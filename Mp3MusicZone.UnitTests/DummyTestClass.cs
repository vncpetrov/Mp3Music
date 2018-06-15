using Mp3MusicZone.Auth;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mp3MusicZone.UnitTests
{
    [TestFixture]
    public class DummyTestClass
    {
        [Test]
        public void EmailSenderDummyTest()
        {
            EmailSenderService emailSender = new EmailSenderService(new EmailSettings());

            Assert.IsTrue(true);
        }
    }
}
