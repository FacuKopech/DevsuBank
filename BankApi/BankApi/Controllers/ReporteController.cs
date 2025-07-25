using BankApi.Helpers;
using BankApi.Services.Cliente;
using Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using DTOs;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ReporteController : ControllerBase
    {
        private readonly ICuentaRepository _accountRepository;
        private readonly IMovimientoRepository _transactionRepository;

        public ReporteController(ICuentaRepository accountRepository, IMovimientoRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        [Route("reportes")]
        public async Task<ActionResult<Reporte>> Reportes([FromQuery] ReporteDto request)
        {
            ICollection<Cuenta> accounts = await _accountRepository.GetAllAsync();
            List<Cuenta>? clientAccounts = accounts.Where(c => c.ClienteId == request.ClienteId).ToList();

            if (!clientAccounts.Any())
            {
                return NotFound("Client does not have any Account");
            }
                

            var transactions = await _transactionRepository.GetAllAsync();
            var transactionsInRange = transactions
                .Where(t => t.Fecha.Date >= request.FechaInicio.Date &&
                            t.Fecha.Date <= request.FechaFin.Date && 
                            t.Cuenta.ClienteId == request.ClienteId)
                .ToList();

            var resumenes = clientAccounts.Select(account =>
            {
                var accountTransactions = transactionsInRange.Where(m => m.CuentaId == account.Id);

                return new Resumen
                {
                    NumeroCuenta = account.NumeroCuenta,
                    Saldo = account.SaldoInicial,
                    TotalDebitos = accountTransactions.Count(m => MovimientoHelper.IsDebit(m.TipoMovimiento)),
                    TotalCreditos = accountTransactions.Count(m => !MovimientoHelper.IsDebit(m.TipoMovimiento))
                };
            }).ToList();

            var reporte = new Reporte
            {
                ClienteId = request.ClienteId,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Resumenes = resumenes
            };

            return Ok(reporte);
        }
    }
}
