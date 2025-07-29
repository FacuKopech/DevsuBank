using BankApi.Commands.Clientes;
using BankApi.Commands.Cuentas;
using BankApi.Services.Cuenta;
using Data.Contracts;
using Data.Repositories;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Net;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaRepository _accountRepository;
        private readonly IClienteRepository _clientRepository;
        private readonly ICreateAccountCommand _createAccountCommands;
        private readonly IUpdateAccountCommand _updateAccountCommands;
        private readonly IDeleteAccountCommand _deleteAccountCommands;

        public CuentaController(ICuentaRepository accountRepository, IClienteRepository clientRepository, 
            ICreateAccountCommand createAccountCommands, IUpdateAccountCommand updateAccountCommands, 
            IDeleteAccountCommand deleteAccountCommands)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
            _createAccountCommands = createAccountCommands;
            _updateAccountCommands = updateAccountCommands;
            _deleteAccountCommands = deleteAccountCommands;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<Cuenta>>> GetAllAccounts()
        {
            ICollection<Cuenta> result = await _accountRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Cuenta>> GetAccount(Guid id)
        {
            Cuenta? accountFound = await _accountRepository.GetEntityAsync(id);
            if (accountFound != null)
            {
                return Ok(accountFound);
            }
            return BadRequest("Account not found.");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<bool>> CreateAccount([FromBody] CuentaDto account)
        {
            var result = await _createAccountCommands.HandleAsync(account);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetAccount), new { id = result.newAccount?.Id }, result.newAccount);
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<bool>> UpdateAccount([FromBody] CuentaDto account)
        {
            var result = await _updateAccountCommands.HandleAsync(account);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(true);
        }


        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteAccount(Guid id)
        {
           var result = await _deleteAccountCommands.HandleAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(true);
        }
    }
}
