using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Model.Register
{
    public class RegisterViewModel : DataViewModel
    {  
        [DisplayName("Username")]
        [Required(ErrorMessage = "{0} Required")]
        public string Username              { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "{0} Required")]
        public string FirstName             { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "{0} Required")]
        public string LastName              { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} Required")]
        public string UserEmail             { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} Required")]
        public string UserPassword          { get; set; }
        [DisplayName("Re-Enter Password")]
        [Compare("UserPassword", ErrorMessage = "Passwords do not match")]
        public string CompareUserPassword   { get; set; }
    }

    public class DataViewModel
    {
        public string ClientId          { get; set; }
        public string Encryptionkey     { get; set; }
        public string VerificationToken { get; set; }
    }
}
