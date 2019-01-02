namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount
{
    using Contracts;
    using System;

    public class GetSongsCount : IQuery<int>
    {
        public bool Approved { get; set; }

        public SearchInfo SearchInfo { get; set; }
    }
}
