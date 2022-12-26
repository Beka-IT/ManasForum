using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Services;

public class AccountService : IAccountService
{
    private AppDbContext _db;
    
    public AccountService(AppDbContext context)
    {
        _db = context;
    }
    
    public async Task<Account> LoginAsync(string login, string password)
    {
        var user = await _db.Accounts.FirstOrDefaultAsync(a => a.Login == login);

        if (user != null)
        {
            user = BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }

        return user;
    }

    public async Task<AccountSignUpResponse> SignUpAsync(AccountSignUpDto account)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@manas.edu.kg");
        
        Match match = regex.Match(account.Login);

        if (!match.Success)
        {
            return new AccountSignUpResponse()
            {
                Account = null,
                message = "Вы можете зарегистрироваться только через email КТУМ"
            };
        }
        
        if (account.Password == "")
        {
            return new AccountSignUpResponse()
            {
                Account = null,
                message = "Введите пароль!"
            };
        }

        var user = await _db.Accounts.FirstOrDefaultAsync(a => a.Login == account.Login);

        if (user != null)
        {
            return new AccountSignUpResponse()
            {
                Account = null,
                message = "Это имя пользователя уже занято!"
            };
        }

        var newAccount = new Account()
            {
                Login = account.Login,
                Fullname = account.Fullname,
                Role = account.Role,
                Department = account.Department,
                Years = account.Years
            };
        
        newAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        await _db.Accounts.AddAsync(newAccount);
        await _db.SaveChangesAsync();

        newAccount = await _db.Accounts.FirstOrDefaultAsync(a => a.Login == account.Login);

        return new AccountSignUpResponse()
        {
            Account = newAccount,
            message = "Вы успешно зарегистрировались!"
        };
    }
}