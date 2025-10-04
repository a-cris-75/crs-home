using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crs.Base.DBaseLibrary;

namespace Crs.Home.ApocalypsData
{
    public static class Parameters
    {
        private static string connectionString1 = "Data Source=(localdb)\\\\MSSQLLocalDB;AttachDbFilename=D:\\CRS\\DEV\\REPOSITORY_GIT\\crs-home\\Crs.Home.Apocalyps\\Crs.Home.ApocalypsDB\\DB_LOTTO_APOCALYPS.mdf; Integrated Security=true;";

        private static string connectionString = "Data Source=(localdb)\\\\..\\DB_LOTTO_APOCALYPS.mdf; Integrated Security=true;";
        private static string providerName = DbConst.PROVIDER_SQLSERVER;

        public static string ConnectionString
        {
            set { connectionString = value; }
            get { return connectionString; }
        }
        public static string ProviderName
        {
            set { providerName = value; }
            get { return providerName; }
        }
    }
}
