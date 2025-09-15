using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstProject.Models
{
    // [SuppressMessage("ReSharper", "Mvc.TemplateNotResolved")]
    // [BindProperties(SupportsGet = true)]
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string? UserName { get; set; }

        [Required]
        // [UIHint("password")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
