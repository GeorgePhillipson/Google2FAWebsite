using System;
using System.Data;
using System.Data.SqlClient;
using Web.Model.Register;

namespace Web.Domain.CreateAccount
{
    public class CreateUserAccount : ICreateUserAccount
    {
        public void InsertNewMember(RegisterViewModel model)
        {
            try
            {
                const string spName = "dbo.NewAccount_Insert";

                using (var cn = new SqlConnection(DbWebConfigConnectionClass.ConnectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(spName, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientId",        model.ClientId);
                        cmd.Parameters.AddWithValue("@Username",        model.Username);
                        cmd.Parameters.AddWithValue("@FirstName",       model.FirstName);
                        cmd.Parameters.AddWithValue("@LastName",        model.LastName);
                        cmd.Parameters.AddWithValue("@ClientEmail",     model.UserEmail);
                        cmd.Parameters.AddWithValue("@ClientPassword",  model.UserPassword);
                        cmd.Parameters.AddWithValue("@EncryptionKey",   model.Encryptionkey);
                        cmd.Parameters.AddWithValue("@SecurityStamp",   model.VerificationToken);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                if (e.Errors.Count > 0)
                {
                    throw new ApplicationException(e.Message);
                }
            }
            catch (Exception e)
            {

                throw new ApplicationException(e.ToString());
            }
        }
    }
}
