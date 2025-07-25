using Data.Contracts;
using Model;

namespace BankApi.Commands.Movimientos
{
    public class DeleteTransactionCommandHandler : IDeleteTransactionCommands
    {
        private readonly IMovimientoRepository _transactionRepository;
        private readonly ICuentaRepository _accountRepository;

        public DeleteTransactionCommandHandler(IMovimientoRepository transactionRepo, ICuentaRepository accountRepo)
        {
            _transactionRepository = transactionRepo;
            _accountRepository = accountRepo;
        }
        public async Task<(bool Success, string Message)> HandleAsync(Guid transactionId)
        {
            Movimiento? transactionFound = await _transactionRepository.GetEntityAsync(transactionId);
            if (transactionFound == null)
            {
                return (false, "Transaction not found.");
            }

            Cuenta? accountFound = await _accountRepository.GetEntityAsync(transactionFound.CuentaId);
            if (accountFound == null)
            {
                return (false, "Account not found.");
            }

            accountFound.SaldoInicial -= transactionFound.Valor;
            await _accountRepository.ModifyEntityAsync(accountFound);

            bool result = await _transactionRepository.DeleteEntityAsync(transactionId);

            return result ? (true, "") : (false, "Failed to delete transaction.");
        }
    }
}
