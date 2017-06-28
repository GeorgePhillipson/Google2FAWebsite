using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.Security.CustomIdenty;

namespace Web.Security.CustomIdentity
{
    public class ValidateLogin : IValidateLogin
    {
        private readonly IPasswordService _iPasswordService;

        public ValidateLogin(IPasswordService iPasswordService)
        {
            _iPasswordService = iPasswordService;
        }

        public void SignOut()
        {
            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("TestSite");
        }

        public bool Isvalid(string firstName, string lastName, string password, string savedPassword, string memberId, string savedRole = "user" )
        {

            var validatePassword = _iPasswordService.VerifyPassword(savedPassword, password);

            if (!validatePassword) return false;

            var claimsIdentity = new ClaimsIdentity("TestSite");

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, savedRole));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, memberId));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, $"{firstName} {lastName}"));

            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
            return true;
        }
    }

    public sealed class PasswordServiceAdaptor : IPasswordService, IPasswordHasher
    {

        private readonly PasswordHasher _hasher;

        public PasswordServiceAdaptor()
        {
            _hasher = new PasswordHasher();
        }

        string IPasswordHasher.HashPassword(string password)
        {
            return _hasher.HashPassword(password);
        }

        PasswordVerificationResult IPasswordHasher.VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return _hasher.VerifyHashedPassword(hashedPassword, providedPassword);
        }

        string IPasswordService.HashPassword(string password)
        {
            return AsPasswordHasher().HashPassword(password);
        }

        bool IPasswordService.VerifyPassword(string hashedPassword, string userPassword)
        {
            var result = AsPasswordHasher().VerifyHashedPassword(hashedPassword, userPassword);

            return result == PasswordVerificationResult.Success;
        }

        private IPasswordHasher AsPasswordHasher()
        {
            return this;
        }
    }
}
