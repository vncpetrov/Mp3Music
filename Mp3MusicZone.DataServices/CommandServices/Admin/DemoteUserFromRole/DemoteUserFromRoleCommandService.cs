namespace Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DemoteUserFromRoleCommandService : ICommandService<DemoteUserFromRole>
    {
        private readonly IEfRepository<User> userRepository;
        private readonly IEfRepository<Role> roleRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public DemoteUserFromRoleCommandService(
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

        public async Task ExecuteAsync(DemoteUserFromRole command)
        {
            User user = await this.userRepository.All(eagerLoading: true)
                .FirstOrDefaultAsync(u => u.Id == command.UserId);
            
            if (user is null)
            {
                throw new NotFoundException(
                    $"User with id {command.UserId} does not exists!");
            }

            if (command.LoggedUserId == user.Id)
            {
                throw new InvalidOperationException(
                    $"User cannot demote himself!");
            }

            if (command.RoleName != "Administrator"
                && user.Roles.Any(r => r.Name == "Administrator")) 
            { 
                throw new InvalidOperationException(
                    $"User with Administrator role cannot be demoted from {command.RoleName} role!");
            }

            bool roleExists = await this.roleRepository.All()
                .AnyAsync(r => r.Name == command.RoleName);

            if (!roleExists)
            {
                throw new NotFoundException(
                    $"{command.RoleName} role does not exists!");
            }

            Role role = user.Roles.FirstOrDefault(r => r.Name == command.RoleName);
            user.Roles.Remove(role);
            this.userRepository.Update(user);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
