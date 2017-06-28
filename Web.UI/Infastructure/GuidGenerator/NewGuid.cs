using System;

namespace Web.UI.Infastructure.GuidGenerator
{
    public static class CreateGuid
    {
        public static string NewId()
        {
            Guid gd = Guid.NewGuid();
            return gd.ToString();
        }
    }
}