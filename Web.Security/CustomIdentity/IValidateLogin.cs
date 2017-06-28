namespace Web.Security.CustomIdentity
{
    public interface IValidateLogin
    {
        bool Isvalid(string firstName,string lastName, string password, string savedPassword, string memberId,string savedRole);
        void SignOut();
    }
}
