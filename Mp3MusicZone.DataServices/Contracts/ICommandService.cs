namespace Mp3MusicZone.DataServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandService<TCommand>
    {
        Task ExecuteAsync(TCommand command);
    }
}
