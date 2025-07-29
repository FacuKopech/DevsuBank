using Model;

namespace BankApi.Commands.Clientes
{
    public interface ICreateClientCommand
    {
        Task<(bool Success, string Message, Cliente? newClient)> HandleAsync(Cliente client);
    }
}
