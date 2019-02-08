namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    // Async shouldn't be part of the abstraction
    public interface ISongProvider
    {
        Task AddAsync(string songTitle, string extension, byte[] song);

        void Update(string oldSongTitle, string newSongTitle, string extension);

        void Delete(string songTitle, string extension);

        Task<byte[]> GetAsync(string songTitle, string extension);
    }
}
