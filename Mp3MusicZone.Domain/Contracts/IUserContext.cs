namespace Mp3MusicZone.Domain.Contracts
{
    using Models.Enums;
    using System;

    public interface IUserContext
    {
        bool IsInRole(RoleType role);

        string GetCurrentUserId();
    }
}
