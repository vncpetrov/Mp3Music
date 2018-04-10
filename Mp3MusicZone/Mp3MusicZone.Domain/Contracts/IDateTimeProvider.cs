namespace Mp3MusicZone.Domain.Contracts
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
