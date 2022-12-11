using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class AccountSignUpDto
{
    [Required(ErrorMessage ="Введите email!")]
    public string Login { get; set; }
    
    [Required(ErrorMessage ="Придумайте пароль!")]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
    public string PasswordConfirm { get; set; }
    
    [Required(ErrorMessage = "Введите ФИО!")]
    public string Fullname { get; set; }
}