using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class Account
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
}