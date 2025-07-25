using DTOs;

namespace BankApi.Commands.Movimientos
{
    public interface IUpdateTransactionCommands
    {
        Task<(bool Success, string Message)> HandleAsync(MovimientoDto transaction);
    }
}
