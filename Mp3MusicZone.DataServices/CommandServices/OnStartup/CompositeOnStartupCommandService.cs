namespace Mp3MusicZone.DomainServices.CommandServices.OnStartup
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CompositeOnStartupCommandService : ICommandService<OnStartupNullObject>
    {
        private IEnumerable<ICommandService<OnStartupNullObject>> commandServices;

        public CompositeOnStartupCommandService(
            IEnumerable<ICommandService<OnStartupNullObject>> commandServices)
        {
            if (commandServices is null)
                throw new ArgumentNullException(nameof(commandServices));

            this.commandServices = commandServices;
        }

        public async Task ExecuteAsync(OnStartupNullObject command)
        {
            foreach (var service in this.commandServices)
            {
                await service.ExecuteAsync(command);
            }
        }
    }
}
