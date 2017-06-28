using Web.Model.Register;

namespace Web.Domain.CreateAccount
{
    public interface ICreateUserAccount
    {
        void InsertNewMember(RegisterViewModel model);
    }
}