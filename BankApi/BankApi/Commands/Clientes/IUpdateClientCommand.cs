using DTOs;

namespace BankApi.Commands.Clientes
{
    public interface IUpdateClientCommand
    {
        Task<(bool Success, string Message)> HandleAsync(ClienteDto client);
    }
}
