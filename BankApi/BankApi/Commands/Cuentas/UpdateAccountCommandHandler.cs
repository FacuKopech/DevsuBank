using BankApi.Services.Cuenta;
using Data.Contracts;
using DTOs;
using Model;

namespace BankApi.Commands.Cuentas
{
    public class UpdateAccountCommandHandler : IUpdateAccountCommand
    {
        private readonly ICuentaRepository _accountRepository;
        private readonly ICuentaValidator _validator;
        private readonly IClienteRepository _clientRepository;

        public UpdateAccountCommandHandler(ICuentaRepository accountRepository, ICuentaValidator accountValidator, IClienteRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _validator = accountValidator;
            _clientRepository = clientRepository;
        }

        public async Task<(bool Success, string Message)> HandleAsync(CuentaDto account)
        {
            Cuenta? accountFound = await _accountRepository.GetEntityAsync(account.Id);
            if (accountFound == null)
            {
                return (false, "Account not found.");
            }

            if (accountFound.NumeroCuenta != account.NumeroCuenta)
            {
                bool areUnique = await _validator.AreFildsUniqueAsync(account);
                if (!areUnique)
                {
                    return (false, "Account already exists.");
                }
                accountFound.NumeroCuenta = account.NumeroCuenta;
            }

            if (account.ClienteId != accountFound.ClienteId)
            {
                Cliente? clientFound = await _clientRepository.GetEntityAsync(account.ClienteId);
                if (clientFound == null)
                {
                    return (false, "Client not found.");
                }

                accountFound.ClienteId = clientFound.Id;
                accountFound.Cliente = clientFound;
            }

            accountFound.TipoCuenta = account.TipoCuenta;
            accountFound.SaldoInicial = account.SaldoInicial;
            accountFound.Estado = account.Estado;

            bool result = await _accountRepository.ModifyEntityAsync(accountFound);

            return result ? (true, "Account modified successfully") 
                : (false, "Failed to modify the account.");
        }
    }
}
