using System.ComponentModel.DataAnnotations;

namespace PostOfficeInfrastructure.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Номер телефону")]
        public string ContactNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }

}
