using Data.Contracts;
using DTOs;

namespace BankApi.Services.Cliente
{
    public class ClienteValidator : IClienteValidator
    {
        private readonly IClienteRepository _clientRepository;

        public ClienteValidator(IClienteRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> isFildUniqueAsync(string identificacion)
        {
            ICollection<Model.Cliente> clients =  await _clientRepository.GetAllAsync();

            return !clients.Any(c => c.Identificacion == identificacion);
        }
    }
}
