namespace Mp3MusicZone.Domain.Contracts
{
    using System;

    public interface ICacheManager
    {
        void Add(string key, object item, int absoluteDurationInSeconds);

        bool Exists(string key);

        void Remove(string key);

        T Get<T>(string key) where T : class;
    }
}
