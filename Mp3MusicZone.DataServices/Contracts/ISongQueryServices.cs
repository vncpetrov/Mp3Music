namespace Mp3MusicZone.DataServices.Contracts
{
    using Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISongQueryServices
    {
        Task<IEnumerable<Song>> GetLastApprovedAsync(int count);

        Task<Song> GetByIdAsync(int id);

        Task<byte[]> GetSongFileAsync(Song song);
    }
}
