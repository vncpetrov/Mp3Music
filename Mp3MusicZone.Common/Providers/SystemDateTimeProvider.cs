namespace Mp3MusicZone.Common.Providers
{
    using Mp3MusicZone.Domain.Contracts;
    using System;

    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
