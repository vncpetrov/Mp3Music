namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ISongProvider
    {
        Task WriteAsync(string songName, byte[] song);

        void Rename(string oldSongName, string newSongName);

        void Delete(string songName);

        Task<byte[]> GetAsync(string songName);
    }
}
