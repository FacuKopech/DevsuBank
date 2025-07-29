using BankApi.Services.Cliente;
using Data.Contracts;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Model;

namespace BankApi.Commands.Clientes
{
    public class UpdateClientCommandHandler : IUpdateClientCommand
    {
        private readonly IClienteRepository _clientRepository;
        private readonly IClienteValidator _validator;
        
        public UpdateClientCommandHandler(IClienteRepository clientRepository, IClienteValidator validator)
        {
            _clientRepository = clientRepository;
            _validator = validator;
        }

        public async Task<(bool Success, string Message)> HandleAsync(ClienteDto client)
        {
            Cliente? clientFound = await _clientRepository.GetEntityAsync(client.Id);
            if (clientFound == null)
            {
                return (false, "Client not found.");
            }

            if (clientFound.Identificacion != client.Identificacion)
            {
                bool isUnique = await _validator.isFildUniqueAsync(client.Identificacion);
                if (!isUnique)
                {
                    return (false, "Client already exists.");
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
                return (true, "Client modified succesfully");
            }
            return (false, "Failed to modify the client.");
        }
    }
}
