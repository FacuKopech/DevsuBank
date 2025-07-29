using BankApi.Services.Cliente;
using Data.Contracts;

namespace BankApi.Commands.Clientes
{
    public class DeleteClientCommandHandler : IDeleteClientCommand
    {
        private readonly IClienteRepository _clientRepository;

        public DeleteClientCommandHandler(IClienteRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<(bool Success, string Message)> HandleAsync(Guid clientId)
        {
            bool result = await _clientRepository.DeleteEntityAsync(clientId);
            if (result)
            {
                return (true, "Client deleted successfully");
            }
            return (false, "Failed to delete the client.");
        }
    }
}
