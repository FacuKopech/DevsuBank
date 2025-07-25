using BankApi.Helpers;
using Data.Contracts;
using DTOs;
using Model;

namespace BankApi.Commands.Movimientos
{
    public class UpdateTransactionCommandHandler : IUpdateTransactionCommands
    {
        private readonly IMovimientoRepository _transactionRepository;
        private readonly ICuentaRepository _accountRepository;

        public UpdateTransactionCommandHandler(IMovimientoRepository transactionRepo, ICuentaRepository accountRepo)
        {
            _transactionRepository = transactionRepo;
            _accountRepository = accountRepo;
        }

        public async Task<(bool Success, string Message)> HandleAsync(MovimientoDto transaction)
        {
            Movimiento? transactionFound = await _transactionRepository.GetEntityAsync(transaction.Id);
            if (transactionFound == null)
            {
                return (false, "Transaction not found.");
            }

            bool accountChanged = transactionFound.CuentaId != transaction.CuentaId;

            Cuenta? originalAccount = await _accountRepository.GetEntityAsync(transactionFound.CuentaId);
            if (originalAccount == null)
            {
                return (false, "Original account not found.");
            }

            Cuenta? currentAccount = accountChanged
                ? await _accountRepository.GetEntityAsync(transaction.CuentaId)
                : originalAccount;

            if (currentAccount == null)
            {
                return (false, "Target account not found.");
            }

            bool wasDebit = MovimientoHelper.IsDebit(transactionFound.TipoMovimiento);
            decimal oldSignedValue = wasDebit ? -Math.Abs(transactionFound.Valor) : Math.Abs(transactionFound.Valor);
            originalAccount.SaldoInicial -= oldSignedValue;
            await _accountRepository.ModifyEntityAsync(originalAccount);

            bool isNowDebit = MovimientoHelper.IsDebit(transaction.TipoMovimiento);
            decimal newSignedValue = isNowDebit ? -Math.Abs(transaction.Valor) : Math.Abs(transaction.Valor);

            currentAccount.SaldoInicial += newSignedValue;
            await _accountRepository.ModifyEntityAsync(currentAccount);

            transactionFound.TipoMovimiento = transaction.TipoMovimiento;
            transactionFound.Valor = newSignedValue;
            transactionFound.CuentaId = transaction.CuentaId;
            transactionFound.Fecha = DateTime.Now;
            transactionFound.Cuenta = currentAccount;
            transactionFound.Saldo = currentAccount.SaldoInicial;

            bool result = await _transactionRepository.ModifyEntityAsync(transactionFound);

            return result ? (true, "") : (false, "Failed to modify transaction.");
        }
    }
}
