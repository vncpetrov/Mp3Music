namespace Mp3MusicZone.FIleAccess
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
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value should not be empty.", nameof(path));

            this.path = path;
            this.songPath = this.path + "/{1}.mp3";

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
        }

        public void Delete(string songName)
        {
            File.Delete(string.Format(this.songPath, songName));
        }

        public async Task<byte[]> GetAsync(string songName)
        {
            string songFullPath = string.Format(this.songPath, songName);

            return await File.ReadAllBytesAsync(songFullPath);
        }

        public void Rename(string oldSongName, string newSongName)
        {
            string oldSongFullName = string.Format(this.songPath, oldSongName);
            string newSongFullName = string.Format(this.songPath, newSongName);

            File.Move(oldSongFullName, newSongFullName);

        }

        public async Task WriteAsync(string songName, byte[] song)
        {
            string songFullPath = string.Format(this.songPath, songName);

            using (FileStream stream = File.OpenWrite(songFullPath))
            {
                await stream.WriteAsync(song, 0, song.Length);
            }
        }
    }
}
