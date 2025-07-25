using BankApi.Helpers;
using Data.Contracts;
using DTOs;
using Model;

namespace BankApi.Commands.Movimientos
{
    public class CreateTransactionCommandHandler : ICreateTransactionCommands
    {
        private readonly IMovimientoRepository _transactionRepository;
        private readonly ICuentaRepository _accountRepository;

        public CreateTransactionCommandHandler(IMovimientoRepository transactionRepo, ICuentaRepository accountRepo)
        {
            _transactionRepository = transactionRepo;
            _accountRepository = accountRepo;
        }

        public async Task<(bool Success, string Message, Movimiento? newTransaction)> HandleAsync(MovimientoDto transaction)
        {
            Cuenta? accountFound = await _accountRepository.GetEntityAsync(transaction.CuentaId);
            if (accountFound == null)
            {
                return (false, "Failed to create Transaction: Account not found", null);
            }

            bool esDebito = MovimientoHelper.IsDebit(transaction.TipoMovimiento);
            decimal valorMovimiento = esDebito ? -Math.Abs(transaction.Valor) : Math.Abs(transaction.Valor);

            if (esDebito && accountFound.SaldoInicial + valorMovimiento < 0)
            {
                return (false, "Saldo no disponible", null);
            }

            if (esDebito)
            {
                DateTime fechaActual = DateTime.Today;

                var movimientosHoy = await _transactionRepository.GetAllAsync();
                var totalRetiradoHoy = movimientosHoy
                    .Where(m => m.CuentaId == accountFound.Id
                        && m.Fecha.Date == fechaActual
                        && m.TipoMovimiento == TipoMovimiento.Extraccion)
                    .Sum(m => Math.Abs(m.Valor));

                if (totalRetiradoHoy + Math.Abs(valorMovimiento) > 1000)
                {
                    return (false, "Cupo diario Excedido", null);
                }
            }

            Movimiento newTransaction = new Movimiento
            {
                Id = Guid.NewGuid(),
                Fecha = DateTime.Now,
                TipoMovimiento = transaction.TipoMovimiento,
                Valor = valorMovimiento,
                Saldo = accountFound.SaldoInicial + valorMovimiento,
                CuentaId = transaction.CuentaId,
                Cuenta = accountFound
            };

            accountFound.SaldoInicial += valorMovimiento;
            await _accountRepository.ModifyEntityAsync(accountFound);

            bool result = await _transactionRepository.AddEntityAsync(newTransaction);

            if (result)
            {
                return (true, "Transaction created successfully", newTransaction);
            }

            return (false, "Failed to create transaction", null);
        }

    }
}
