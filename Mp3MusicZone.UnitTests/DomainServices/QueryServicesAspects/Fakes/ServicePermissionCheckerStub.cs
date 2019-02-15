namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes
{
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.DomainServices;
    using System;

    public class ServicePermissionCheckerStub : ServicePermissionChecker<QueryStub>
    {
        public ServicePermissionCheckerStub(IUserPermissionChecker permissionChecker) 
            : base(permissionChecker)
        {
        }
    }
}
