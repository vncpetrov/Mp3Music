namespace Mp3MusicZone.DataServices.Contracts
{
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISongService
    {
        Task UploadAsync(
             string title,
             string extension,
             string singer,
             int releasedYear,
             string uploaderId,
             byte[] file);

        Task EditAsync(
            int songId,
            string title,
            string extension,
            string singer,
            int releasedYear,
            byte[] file);

        Task<IEnumerable<Song>> GetLastApprovedAsync(int count);

        Task<Song> GetByIdAsync(int id);

        Task<byte[]> GetSongFileAsync(Song song);
    }
}
