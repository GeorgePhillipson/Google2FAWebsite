﻿namespace Web.Security.CustomIdenty
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        bool VerifyPassword(string hashedPassword, string userPassword);
    }
}
