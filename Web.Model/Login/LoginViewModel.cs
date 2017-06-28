using System.ComponentModel;

namespace Web.Model.Login
{
    public class LoginViewModel
    {
        [DisplayName("Username")]
        public string Username  { get; set; }
        [DisplayName("Password")]
        public string Password  { get; set; }
        [DisplayName("2FA Code")]
        public string TfaCode   { get; set; }
    }

}
