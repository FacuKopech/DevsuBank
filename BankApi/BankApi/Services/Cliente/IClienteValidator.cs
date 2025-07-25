using DTOs;

namespace BankApi.Services.Cliente
{
    public interface IClienteValidator
    {
        Task<bool> isFildUniqueAsync(string identificacion);
    }
}
