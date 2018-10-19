namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ISongProvider
    {
        Task WriteAsync(string songTitle, string extension, byte[] song);

        void Rename(string oldSongTitle, string newSongTitle, string extension);

        void Delete(string songTitle, string extension);

        Task<byte[]> GetAsync(string songTitle, string extension);
    }
}
