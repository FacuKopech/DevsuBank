using DTOs;

namespace BankApi.Commands.Movimientos
{
    public interface IDeleteTransactionCommands
    {
        Task<(bool Success, string Message)> HandleAsync(Guid transactionId);
    }
}
