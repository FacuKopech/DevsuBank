using Data.Contracts;

namespace BankApi.Commands.Cuentas
{
    public class DeleteAccountCommandHandler : IDeleteAccountCommand
    {
        private readonly ICuentaRepository _accountRepository;

        public DeleteAccountCommandHandler(ICuentaRepository accountRepository) 
        {
            _accountRepository = accountRepository;
        }

        public async Task<(bool Success, string Message)> HandleAsync(Guid accountId)
        {
            bool result = await _accountRepository.DeleteEntityAsync(accountId);
            if (result)
            {
                return (true, "Account deleted successfully");
            }
            return (false, "Failed to delete account.");
        }
    }
}
