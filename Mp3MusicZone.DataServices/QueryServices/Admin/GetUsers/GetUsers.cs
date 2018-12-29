namespace Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers
{
    using Contracts;
    using Common.Constants;
    using Domain.Attributes;
    using Domain.Models;
    using System;
    using System.Collections.Generic;

    [Permission(Permissions.GetUsers)]
    public class GetUsers : IQuery<IEnumerable<User>>
    {
    }
}
