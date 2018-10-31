﻿namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetById
{
    using Contracts;
    using Domain.Models;
    using System;

    public class GetSongById : IQuery<Song>
    {
        public int SongId { get; set; }
    }
}
