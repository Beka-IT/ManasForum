using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class Account
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string? Role { get; set; }
    public string? Department { get; set; }
    public int? Years { get; set; }
}