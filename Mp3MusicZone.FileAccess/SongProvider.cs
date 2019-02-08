namespace Mp3MusicZone.FileAccess
{
    using Domain.Contracts;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SongProvider : ISongProvider
    {
        private readonly string path;
        private readonly string songPath;

        public SongProvider(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value should not be empty.", nameof(path));

            this.path = path;
            this.songPath = this.path + "/{0}.{1}";

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
        }

        public void Delete(string songName, string extension)
        {
            File.Delete(string.Format(this.songPath, songName, extension));
        }

        public async Task<byte[]> GetAsync(string songName, string extension)
        {
            string songFullPath = string.Format(this.songPath, songName, extension);

            return await File.ReadAllBytesAsync(songFullPath);
        }

        public void Update(string oldSongName, string newSongName, string extension)
        {
            string oldSongFullName = string.Format(this.songPath, oldSongName, extension);
            string newSongFullName = string.Format(this.songPath, newSongName, extension);

            File.Move(oldSongFullName, newSongFullName);
        }

        public async Task AddAsync(string songName, string extension, byte[] song)
        {
            string songFullPath = string.Format(this.songPath, songName, extension);

            using (FileStream stream = File.OpenWrite(songFullPath))
            {
                await stream.WriteAsync(song, 0, song.Length);
            }
        }
    }
}
