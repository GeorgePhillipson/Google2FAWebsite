using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Web.Model.Login;
using Web.Model.Profile;

namespace Web.Domain.UserProfile
{
    public class Profile : IProfile
    {
        public async Task<UserProfileDetailsViewModel> UserProfile(LoginViewModel model)
        {
            try
            {

                const string spName = "dbo.UserProfile_Select";

                using (var cn = new SqlConnection(DbWebConfigConnectionClass.ConnectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(spName, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", model.Username);

                        var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.Default);

                        var data = new UserProfileDetailsViewModel();
                        if (!rdr.Read())
                        {
                            //TODO update database with failed login
                            throw new InvalidOperationException("No records match that username.");
                        }
                        data.UserId         = rdr.GetString(rdr.GetOrdinal("UserId"));
                        data.FirstName      = rdr.GetString(rdr.GetOrdinal("FirstName"));
                        data.LastName       = rdr.GetString(rdr.GetOrdinal("LastName"));
                        data.PasswordHash   = rdr.GetString(rdr.GetOrdinal("PasswordHash"));
                        return data;
                    }
                }
            }
            catch (SqlException e)
            {
                throw new ApplicationException(e.ToString());
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());
            }
        }
    }
}
