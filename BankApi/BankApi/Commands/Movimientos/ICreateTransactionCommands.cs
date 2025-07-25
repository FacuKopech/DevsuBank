using DTOs;
using Model;

namespace BankApi.Commands.Movimientos
{
    public interface ICreateTransactionCommands
    {
        Task<(bool Success, string Message, Movimiento? newTransaction)> HandleAsync(MovimientoDto transaction);
    }
}
