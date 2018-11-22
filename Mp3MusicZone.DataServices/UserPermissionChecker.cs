namespace Mp3MusicZone.DomainServices
{
    using Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Linq;

    public class UserPermissionChecker : IUserPermissionChecker
    {
        private readonly IEfRepository<User> userRepository;
        private readonly IUserContext userContext;

        public UserPermissionChecker(
            IEfRepository<User> userRepository,
            IUserContext userContext)
        {
            if (userRepository is null)
                throw new ArgumentNullException(nameof(userRepository));

            if (userContext is null)
                throw new ArgumentNullException(nameof(userContext));

            this.userRepository = userRepository;
            this.userContext = userContext;
        }

        public void CheckPermission(string permissionId)
        {
            string userId = this.userContext.GetCurrentUserId();
            if (userId is null)
            {
                return;
            }

            bool hasUserPermission = this.userRepository.All(eagerLoading: true)
                .Where(u => u.Id == userId)
                .Any(u => u.Roles
                           .Any(r => r.Permissions
                                      .Any(p => p.Id == permissionId)));

            if (!hasUserPermission)
            {
                throw new InvalidOperationException("User is not permitted to execute this action");
            }
        }
    }
}
