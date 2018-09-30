namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ISongProvider
    {
        Task WriteAsync(string songName, string extension, byte[] song);

        void Rename(string oldSongName, string newSongName, string extension);

        void Delete(string songName, string extension);

        Task<byte[]> GetAsync(string songName, string extension);
    }
}
