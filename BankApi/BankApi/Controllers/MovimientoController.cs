using BankApi.Commands.Movimientos;
using BankApi.Services.Cuenta;
using Data.Contracts;
using Data.Repositories;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Principal;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMovimientoRepository _transactionRepository;
        private readonly ICuentaRepository _accountRepository;
        private readonly ICreateTransactionCommands _createTransactionCommands;
        private readonly IUpdateTransactionCommands _updateTransactionCommands;
        private readonly IDeleteTransactionCommands _deleteTransactionCommands;

        public MovimientoController(IMovimientoRepository transactionRepository, ICuentaRepository accountRepository,
            IUpdateTransactionCommands updateTransactionCommands, ICreateTransactionCommands createTransactionCommands,
            IDeleteTransactionCommands deleteTransactionCommands)       
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _createTransactionCommands = createTransactionCommands;
            _updateTransactionCommands = updateTransactionCommands;
            _deleteTransactionCommands = deleteTransactionCommands; 
        }
       

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<Movimiento>>> GetAllTransactions()
        {
            ICollection<Movimiento> result = await _transactionRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Movimiento>> GetTransaction(Guid id)
        {
            Movimiento? transactionFound = await _transactionRepository.GetEntityAsync(id);
            if (transactionFound != null)
            {
                return Ok(transactionFound);
            }
            return BadRequest("Transaction not found.");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<bool>> CreateTransaction([FromBody] MovimientoDto transaction)
        {
            var result = await _createTransactionCommands.HandleAsync(transaction);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetTransaction), new { id = result.newTransaction?.Id }, result.newTransaction);
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<bool>> UpdateTransaction([FromBody] MovimientoDto transaction)
        {
            var result = await _updateTransactionCommands.HandleAsync(transaction);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            
            return Ok(true);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteTransaction(Guid id)
        {
            var result = await _deleteTransactionCommands.HandleAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(true);
        }

    }
}
