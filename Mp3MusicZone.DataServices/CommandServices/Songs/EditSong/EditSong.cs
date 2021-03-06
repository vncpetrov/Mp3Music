﻿namespace Mp3MusicZone.DomainServices.CommandServices.Songs.EditSong
{
    using Common.Constants;
    using Domain.Attributes;
    using QueryServices.Songs.GetLastApproved;
    using System;

    [Permission(Permissions.EditSong)]
    [InvalidateCacheFor(nameof(GetLastApprovedSongs))] 
    public class EditSong
    {
        public string SongId { get; set; }

        public string Title { get; set; }

        public string Singer { get; set; }

        public int ReleasedYear { get; set; }

        public string FileExtension { get; set; }

        public byte[] SongFile { get; set; }
    }
}
