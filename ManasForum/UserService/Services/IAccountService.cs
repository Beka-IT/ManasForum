using UserService.Models;

namespace UserService.Services;

public interface IAccountService
{
    Account Login(Account account);

    Account SignUp(Account account);
}