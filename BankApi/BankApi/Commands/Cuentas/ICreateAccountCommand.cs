using DTOs;
using Model;

namespace BankApi.Commands.Cuentas
{
    public interface ICreateAccountCommand
    {
        Task<(bool Success, string Message, Cuenta? newAccount)> HandleAsync(CuentaDto account);
    }
}
