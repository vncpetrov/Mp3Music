namespace Mp3MusicZone.DomainServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandService<TCommand>
    {
        // Exposing async from domain layer is kind of leaky abstraction
        Task ExecuteAsync(TCommand command);
    }
}
