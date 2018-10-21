namespace Mp3MusicZone.DataServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IUploadSongService
    {
        Task UploadAsync(
            string title,
            string extension,
            string singer,
            int releasedYear,
            string uploaderId,
            byte[] file);
    }
}
