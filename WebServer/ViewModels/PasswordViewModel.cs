using System.ComponentModel.DataAnnotations;
namespace WebServer.ViewModels
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
