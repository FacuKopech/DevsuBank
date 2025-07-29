namespace BankApi.Commands.Clientes
{
    public interface IDeleteClientCommand
    {
        Task<(bool Success, string Message)> HandleAsync(Guid clientId);
    }
}
