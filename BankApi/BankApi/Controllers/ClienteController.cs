using Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Model;
using Microsoft.AspNetCore.Identity;
using DTOs;
using BankApi.Services.Cliente;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clientRepository;
        private readonly IClienteValidator _clientValidator;
        private readonly PasswordHasher<Cliente> _passwordHasher;

        public ClienteController(IClienteRepository clientRepository, IClienteValidator clientValidator, PasswordHasher<Cliente> passwordHasher)
        {
            _clientRepository = clientRepository;
            _clientValidator = clientValidator;
            _passwordHasher = passwordHasher;
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
            bool isUnique = await _clientValidator.isFildUniqueAsync(client.Identificacion);
            if(isUnique)
            {
                client.Id = Guid.NewGuid();
                client.Contraseña = _passwordHasher.HashPassword(client, client.Contraseña);

                bool result = await _clientRepository.AddEntityAsync(client);

                if (result)
                {
                    return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
                }

                return BadRequest("Failed to create the client.");
            }
            return BadRequest("Client already exists.");
        }

        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<bool>> UpdateClient([FromBody] ClienteDto client)
        {
            Cliente? clientFound = await _clientRepository.GetEntityAsync(client.Id);
            if (clientFound == null)
            {
                return NotFound("Client not found.");
            }

            if (clientFound.Identificacion != client.Identificacion)
            {
                bool isUnique = await _clientValidator.isFildUniqueAsync(client.Identificacion);
                if (!isUnique)
                {
                    return BadRequest("Client already exists.");
                }
                clientFound.Identificacion = client.Identificacion;
            }
           
            clientFound.Nombre = client.Nombre;
            clientFound.Genero = client.Genero;
            clientFound.Edad = client.Edad;
            clientFound.Direccion = client.Direccion;
            clientFound.Telefono = client.Telefono;
            clientFound.Estado = client.Estado;

            bool result = await _clientRepository.ModifyEntityAsync(clientFound);

            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Failed to modify the client.");  
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult<bool>> DeleteClient(Guid id)
        {
            bool result = await _clientRepository.DeleteEntityAsync(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Failed to delete the client.");
        }
    }
}
