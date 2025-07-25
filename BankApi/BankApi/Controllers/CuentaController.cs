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
        private readonly ICuentaValidator _accountValidator;
        private readonly IClienteRepository _clientRepository;

        public CuentaController(ICuentaRepository accountRepository, ICuentaValidator accountValidator,IClienteRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _accountValidator = accountValidator;
            _clientRepository = clientRepository;
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
            bool areUnique = await _accountValidator.AreFildsUniqueAsync(account);
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
                        return CreatedAtAction(nameof(GetAccount), new { id = newAccount.Id }, newAccount);
                    }
                    return BadRequest("Failed to create account.");
                }
                return NotFound("Client not found.");
            }
            return BadRequest("Account already exists.");
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<bool>> UpdateAccount([FromBody] CuentaDto account)
            {
            Cuenta? accountFound = await _accountRepository.GetEntityAsync(account.Id);
            if (accountFound == null)
            {
                return NotFound("Account not found.");
            }

            if (accountFound.NumeroCuenta != account.NumeroCuenta)
            {
                bool areUnique = await _accountValidator.AreFildsUniqueAsync(account);
                if (!areUnique)
                {
                    return BadRequest("Account already exists.");
                }
                accountFound.NumeroCuenta = account.NumeroCuenta;
            }
            
            if (account.ClienteId != accountFound.ClienteId)
            {
                Cliente? clientFound = await _clientRepository.GetEntityAsync(account.ClienteId);
                if (clientFound == null)
                {
                    return NotFound("Client not found.");
                }

                accountFound.ClienteId = clientFound.Id;
                accountFound.Cliente = clientFound;
            }

            accountFound.TipoCuenta = account.TipoCuenta;
            accountFound.SaldoInicial = account.SaldoInicial;
            accountFound.Estado = account.Estado;

            bool result = await _accountRepository.ModifyEntityAsync(accountFound);

            return result ? Ok(result) : BadRequest("Failed to modify the account.");
            
        }


        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteAccount(Guid id)
        {
            bool result = await _accountRepository.DeleteEntityAsync(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Failed to delete account.");
        }
    }
}
