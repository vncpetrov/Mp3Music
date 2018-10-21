namespace Mp3MusicZone.DataServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IEditSongService
    {
        Task EditAsync(
            int songId,
            string title,
            string extension,
            string singer,
            int releasedYear,
            byte[] file);
    }
}
