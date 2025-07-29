namespace BankApi.Commands.Cuentas
{
    public interface IDeleteAccountCommand
    {
        Task<(bool Success, string Message)> HandleAsync(Guid accountId);
    }
}
