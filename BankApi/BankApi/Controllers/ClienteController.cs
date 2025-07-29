using Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.AspNetCore.Identity;
using DTOs;
using BankApi.Services.Cliente;
using BankApi.Commands.Clientes;
using BankApi.Commands.Movimientos;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clientRepository;
        private readonly ICreateClientCommand _createClientCommands;
        private readonly IUpdateClientCommand _updateClientCommands;
        private readonly IDeleteClientCommand _deleteClientCommands;
        
        public ClienteController(IClienteRepository clientRepository, ICreateClientCommand createClientCommands,
            IUpdateClientCommand updateClientCommands, IDeleteClientCommand deleteClientCommands)
        {
            _clientRepository = clientRepository;
            _createClientCommands = createClientCommands;
            _updateClientCommands = updateClientCommands;
            _deleteClientCommands = deleteClientCommands;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ICollection<Cliente>>> GetAllClients()
        {
            ICollection<Cliente> clients = await _clientRepository.GetAllAsync();
            IEnumerable<ClienteDto> clientDtos = clients.Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Genero = c.Genero,
                Edad = c.Edad,
                Identificacion = c.Identificacion,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Estado = c.Estado
            });
            return Ok(clientDtos);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Cliente>> GetClient(Guid id)
        {
            Cliente? clientFound = await _clientRepository.GetEntityAsync(id);
            if (clientFound != null)
            {
                return Ok(clientFound);
            }
            return BadRequest("Client not found.");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<bool>> CreateClient([FromBody] Cliente client)
        {
            var result = await _createClientCommands.HandleAsync(client);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetClient), new { id = result.newClient?.Id }, result.newClient);
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<bool>> UpdateClient([FromBody] ClienteDto client)
        {
            var result = await _updateClientCommands.HandleAsync(client);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(true);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteClient(Guid id)
        {
            var result = await _deleteClientCommands.HandleAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(true);
        }
    }
}
