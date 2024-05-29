using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostOfficeInfrastructure.ViewModel
{
    public class RegisterViewModel
    {
        [DisplayName(displayName: "Ім'я")]
        public string Name { get; set; }

        [Required]
        [DisplayName(displayName: "Номер телефону")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [Required]
        [DisplayName(displayName: "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DisplayName(displayName: "Підтвердити пароль")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
