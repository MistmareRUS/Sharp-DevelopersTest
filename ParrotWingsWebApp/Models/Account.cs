using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParrotWingsWebApp.Models
{
    /// <summary>
    /// Хранятся модели пользователя и унаследованного от него регистрируемого пользователя
    /// </summary>
    public class LoginAccount
    {
        [Required]
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина пароля должна составлять от 5 до 50 символов")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

    }
    public class RegisterAccount : LoginAccount
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Compare("Pass", ErrorMessage = "Введенные пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }
    }
    public class ChangePass
    {       
        [Required]
        [Display(Name ="Старый пароль")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина пароля должна составлять от 5 до 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Новые пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Повтор нового пароля")]
        public string NewPasswordRepeat { get; set; }
    }
}