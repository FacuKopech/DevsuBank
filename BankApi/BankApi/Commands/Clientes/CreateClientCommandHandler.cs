using BankApi.Services.Cliente;
using Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Model;
using System.Net;

namespace BankApi.Commands.Clientes
{
    public class CreateClientCommandHandler : ICreateClientCommand
    {
        private readonly IClienteRepository _clientRepository;
        private readonly IClienteValidator _validator;
        private readonly PasswordHasher<Cliente> _passwordHasher;

        public CreateClientCommandHandler(IClienteRepository clientRepository, IClienteValidator validator, PasswordHasher<Cliente> passwordHasher) 
        {
            _clientRepository = clientRepository;
            _validator = validator;
            _passwordHasher = passwordHasher;
        }
        public async Task<(bool Success, string Message, Cliente? newClient)> HandleAsync(Cliente client)
        {
            bool isUnique = await _validator.isFildUniqueAsync(client.Identificacion);
            if (isUnique)
            {
                client.Id = Guid.NewGuid();
                client.Contraseña = _passwordHasher.HashPassword(client, client.Contraseña);

                bool result = await _clientRepository.AddEntityAsync(client);

                if (result)
                {
                    return (true, "Client created successfully", client);
                }

                return (false, "Failed to create the client.", null);
            }
            return (false, "Client already exists.", null);
        }
    }
}
