namespace Mp3MusicZone.DomainServices.CommandServices.Admin.PromoteUserToRole
{
    using Common.Constants;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.PromoteUserToRole)]
    public class PromoteUserToRole
    {
        public string UserId { get; set; }
        
        public string RoleName { get; set; }
    }
}
