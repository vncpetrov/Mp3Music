namespace Mp3MusicZone.DomainServices.CommandServices.Admin.PromoteUserToRole
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class PromoteUserToRoleCommandService : ICommandService<PromoteUserToRole>
    {
        private readonly IEfRepository<User> userRepository;
        private readonly IEfRepository<Role> roleRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public PromoteUserToRoleCommandService(
            IEfRepository<User> userRepository,
            IEfRepository<Role> roleRepository,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (userRepository is null)
                throw new ArgumentNullException(nameof(userRepository));

            if (roleRepository is null)
                throw new ArgumentNullException(nameof(roleRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task ExecuteAsync(PromoteUserToRole command)
        {
            User user = await this.userRepository.All(eagerLoading: true)
                .FirstOrDefaultAsync(u => u.Id == command.UserId);

            if (user is null)
            {
                throw new NotFoundException(
                    $"User with id {command.UserId} does not exists!");
            }

            Role role = await this.roleRepository.All()
                .FirstOrDefaultAsync(r => r.Name == command.RoleName);

            if (role is null)
            {
                throw new NotFoundException(
                    $"Role with id {command.RoleName} does not exists!");
            }

            user.Roles.Add(role);
            this.userRepository.Update(user);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
