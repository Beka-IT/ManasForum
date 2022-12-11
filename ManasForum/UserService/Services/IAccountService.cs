using UserService.Models;

namespace UserService.Services;

public interface IAccountService
{
    Task<Account> LoginAsync(string login, string password);

    Task<AccountSignUpResponse> SignUpAsync(AccountSignUpDto account);
}