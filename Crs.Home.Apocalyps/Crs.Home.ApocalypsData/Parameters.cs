using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crs.Base.DBaseLibrary;

namespace Crs.Home.ApocalypsData
{
    public static class Parameters
    {
        private static string connectionString = "";
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
