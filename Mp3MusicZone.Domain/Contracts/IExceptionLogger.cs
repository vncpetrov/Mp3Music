namespace Mp3MusicZone.Domain.Contracts
{
    using System;

    public interface IExceptionLogger
    {
        void Log(Exception exception, string additionalInfo = null);
    }
}
