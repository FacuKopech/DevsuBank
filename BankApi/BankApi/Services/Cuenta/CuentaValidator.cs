using Data.Contracts;
using DTOs;

namespace BankApi.Services.Cuenta
{
    public class CuentaValidator : ICuentaValidator
    {
        private readonly ICuentaRepository _accountRepository;

        public CuentaValidator(ICuentaRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> AreFildsUniqueAsync(CuentaDto account)
        {
            ICollection<Model.Cuenta> accounts = await _accountRepository.GetAllAsync();

            return !accounts.Any(a => a.NumeroCuenta == account.NumeroCuenta);
        }
    }
}
