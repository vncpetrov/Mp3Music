namespace Mp3MusicZone.Domain.Contracts
{
    using System;

    public interface IFileProvider
    {
        void WriteAllBytes(string filePath, byte[] file);

        void Rename(string oldFileName, string newFileName);

        void Delete(string fileName);
    }
}
