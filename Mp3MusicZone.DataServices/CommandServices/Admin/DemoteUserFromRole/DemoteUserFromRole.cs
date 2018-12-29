namespace Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole
{
    using Common.Constants;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.DemoteUserFromRole)]
    public class DemoteUserFromRole
    {
        public string UserId { get; set; }

        public string RoleName { get; set; }

        public string LoggedUserId { get; set; }
    }
}
