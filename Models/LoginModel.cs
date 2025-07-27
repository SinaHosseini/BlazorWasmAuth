using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; }
    }
}
