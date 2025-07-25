using DTOs;

namespace BankApi.Services.Cuenta
{
    public interface ICuentaValidator
    {
        Task<bool> AreFildsUniqueAsync(CuentaDto client);
    }
}
