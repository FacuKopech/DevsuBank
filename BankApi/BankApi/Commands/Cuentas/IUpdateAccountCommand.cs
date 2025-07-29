using DTOs;

namespace BankApi.Commands.Cuentas
{
    public interface IUpdateAccountCommand
    {
        Task<(bool Success, string Message)> HandleAsync(CuentaDto account);
    }
}
