using System.Threading.Tasks;
using Web.Model.Login;
using Web.Model.Profile;

namespace Web.Domain.UserProfile
{
    public interface IProfile
    {
        Task<UserProfileDetailsViewModel> UserProfile(LoginViewModel model);
    }
}