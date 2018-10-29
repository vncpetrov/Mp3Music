namespace Mp3MusicZone.DomainServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandService<TCommand>
    {
        Task ExecuteAsync(TCommand command);
    }
}
