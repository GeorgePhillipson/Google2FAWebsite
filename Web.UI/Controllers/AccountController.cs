using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Authenticator;
using Web.Domain.CreateAccount;
using Web.Domain.UserProfile;
using Web.Model.Login;
using Web.Model.Register;
using Web.Security.CustomIdentity;
using Web.Security.CustomIdenty;
using Web.UI.Infastructure.GuidGenerator;

namespace Web.UI.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IEncryptData           _iEncryptData;
        private readonly IPasswordService       _iPasswordService;
        private readonly ICreateUserAccount     _iCreateUserAccount;
        private readonly IProfile               _iProfile;
        private readonly IValidateLogin         _iValidateLogin;

        public AccountController(IEncryptData iEncryptData, 
                                 IPasswordService iPasswordService, 
                                 ICreateUserAccount iCreateUserAccount, 
                                 IProfile iProfile, 
                                 IValidateLogin iValidateLogin)
        {
            _iEncryptData           = iEncryptData;
            _iPasswordService       = iPasswordService;
            _iCreateUserAccount     = iCreateUserAccount;
            _iProfile               = iProfile;
            _iValidateLogin         = iValidateLogin;
        }

        private const string Key = "&%$£po78AD?#";//This would be put somewhere else, just added security

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            bool status = false;

            var loginUser = await _iProfile.UserProfile(model);

            bool isValidLogin = _iValidateLogin.Isvalid(loginUser.FirstName, loginUser.LastName, model.Password, loginUser.PasswordHash, loginUser.UserId, "Role User");

            if (isValidLogin)
            {
                status = true;

                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string userId = _iEncryptData.WindowsEncrypted($"{loginUser.UserId}{Key}");

                var tfaCode = tfa.GenerateSetupCode("GeorgePhillipsonLtd", model.Username, userId, 300, 300);

                ViewBag.Status          = status;
                TempData["UserId"]      = userId;
                ViewBag.Message         = "Please scan QR code with your phone.";
                ViewBag.BarcodeImage    = tfaCode.QrCodeSetupImageUrl;

                return View("~/Views/Login/Login.cshtml");

            }
            ViewBag.Message     = "Username or passwaord incorrect.";
            ViewBag.Status      = status;

            return View("~/Views/Login/Login.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return PartialView("~/Views/Partials/pvRegister.cshtml");
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            if (Request.IsAuthenticated)
            {
                var identity        = (ClaimsIdentity)User.Identity;
                var user            = identity.Name;
                ViewBag.FullName    = user;

                return View("~/Views/Profile/Profile.cshtml");
            }
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyTfa(LoginViewModel model)
        {
            var token = model.TfaCode;

            TwoFactorAuthenticator tfa  = new TwoFactorAuthenticator();
            string userKey              = TempData["UserId"].ToString();
            bool isValid                = tfa.ValidateTwoFactorPIN(userKey, token, TimeSpan.FromMinutes(5));

            if (isValid)
            {
                return RedirectToAction("UserProfile");
            }

            ViewBag.Message = "Incorrect token value.";

            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return RedirectToAction("Index", "Home");

                string clientId                 = CreateGuid.NewId();
                string clientEmail              = model.UserEmail;
                string verificationCode         = SecurityStamp.EncryptSecurityStamp("Create-Account", clientId);
                var encryptEmail                = _iEncryptData.Encrypt(model.UserEmail);
                string encryptedEmail           = encryptEmail.Item1;
                string encryptedEmailkey        = encryptEmail.Item2;
                string hashPassword             = _iPasswordService.HashPassword(model.UserPassword);
                var encryptedVerificationCode   = _iEncryptData.Encrypt(verificationCode);


                model.ClientId                  = clientId;
                model.VerificationToken         = encryptedVerificationCode.ToString();
                model.UserEmail                 = encryptedEmail;
                model.Encryptionkey             = encryptedEmailkey;
                model.UserPassword              = hashPassword;

                _iCreateUserAccount.InsertNewMember(model);

                var callbackUrl = CallbackUrl(encryptedVerificationCode.ToString());

                SendEmail(callbackUrl, clientEmail);

                TempData["Message"] = MvcHtmlString.Create("<p class=\"alert alert-danger\">Please check your email to confirm your address.</p>");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = MvcHtmlString.Create("<p class=\"alert alert-danger\">" + e.Message + "</p>");

                return RedirectToAction("Index", "Home");

            }
        }

        public ActionResult ConfirmEmail(string id, string code)
        {
            //TODO in future post
            return View();
        }
    }
}