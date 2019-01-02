namespace Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount
{
    using Contracts;
    using System;

    public class GetUsersCount : IQuery<int>
    {
        public SearchInfo SearchInfo { get; set; }
    }
}
