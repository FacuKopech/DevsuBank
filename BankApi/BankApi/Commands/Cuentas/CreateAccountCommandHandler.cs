using BankApi.Services.Cuenta;
using Data.Contracts;
using DTOs;
using Model;
using System.Security.Principal;

namespace BankApi.Commands.Cuentas
{
    public class CreateAccountCommandHandler : ICreateAccountCommand
    {
        private readonly ICuentaRepository _accountRepository;
        private readonly ICuentaValidator _validator;
        private readonly IClienteRepository _clientRepository;

        public CreateAccountCommandHandler(ICuentaRepository accountRepository, ICuentaValidator accountValidator, IClienteRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _validator = accountValidator;
            _clientRepository = clientRepository;
        }
        public async Task<(bool Success, string Message, Cuenta? newAccount)> HandleAsync(CuentaDto account)
        {
            bool areUnique = await _validator.AreFildsUniqueAsync(account);
            if (areUnique)
            {
                Cliente? clientFound = await _clientRepository.GetEntityAsync(account.ClienteId);
                if (clientFound != null)
                {
                    Cuenta newAccount = new Cuenta
                    {
                        Id = Guid.NewGuid(),
                        NumeroCuenta = account.NumeroCuenta,
                        TipoCuenta = account.TipoCuenta,
                        SaldoInicial = account.SaldoInicial,
                        Estado = account.Estado,
                        ClienteId = clientFound.Id,
                        Cliente = clientFound,
                        Movimientos = new List<Movimiento>()
                    };
                    bool result = await _accountRepository.AddEntityAsync(newAccount);

                    if (result)
                    {
                        return (true, "Account created successfully", newAccount);
                    }
                    return (false, "Failed to create account.", null);
                }
                return (false, "Client not found.", null);
            }
            return (false, "Account already exists.", null);
        }
    }
}
