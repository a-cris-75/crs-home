using Crs.Base.CommonUtilsLibrary;
using Crs.Base.DBaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crs.Home.ApocalypsData
{
    public static class Parameters
    {
        //private static string connectionString1 = "Data Source=(localdb)\\\\MSSQLLocalDB;AttachDbFilename=D:\\CRS\\DEV\\REPOSITORY_GIT\\crs-home\\Crs.Home.Apocalyps\\Crs.Home.ApocalypsDB\\DB_LOTTO_APOCALYPS.mdf; Integrated Security=true;";

        //private static string connectionStringCFG = Parameters.GetConnStringProvider_fromJson("LOCAL", out providerNameCFG, out formatdateCFG);

        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=..\\DB\\DB_LOTTO_APOCALYPS.mdf; Integrated Security=true;";
        private static string providerName = DbConst.PROVIDER_SQLSERVER;
        public static ConfigManager ConfiguratorManager = new ConfigManager("", "DatabaseSettings");

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

        public static string GetConnStringProvider_fromJson(string LBL_DB, out string provname, out string formatdate)
        {
            provname = "";
            formatdate = "";

            // IL METODO VA BENE ANCHE PER LOG: in entrambi i casi se non specifico LBL_DB il db è quello di configurazione centralizzato
            string? connstr = GetConnectionString_fromJson(LBL_DB, out formatdate);

            if (!string.IsNullOrEmpty(connstr) && connstr.Contains("Initial Catalog"))
                provname = DbConst.PROVIDER_SQLSERVER;
            else
            {
                if (ConfiguratorManager.GetValue("ProviderName" + LBL_DB) != null)
                {
                    string? provname1 = ConfiguratorManager.GetValue("ProviderName" + LBL_DB);
                    provname = !string.IsNullOrEmpty(provname1) ? provname1 : DbConst.PROVIDER_SQLSERVER;
                }
            }

            return (!string.IsNullOrEmpty(connstr) ? connstr : string.Empty);
        }

        private static string? GetConnectionString_fromJson(string db_label, out string formatdate)
        {
            string? res = "";
            //string? CONNSTRING_SOURCE = ConfiguratorManager.GetValue(DbConst.CONNSTRING_SOURCE);

            //if (CONNSTRING_SOURCE == "APPCONFIG")
            {
                res = ConfiguratorManager.GetValue("ConnectionString" + db_label);
            }
            //else
            //if (string.IsNullOrEmpty(dev))
            //    res = CONNECTION_STRING_CFG_DEFAULT;
            //else res = CONNECTION_STRING_CFG_DEFAULT_DEV;

            string? formatdate1 = ConfiguratorManager.GetValue("FormatDate" + db_label);
            if (string.IsNullOrEmpty(formatdate1))
                formatdate = "MM/dd/yyyy HH:mm:ss";
            else
                formatdate = formatdate1;

            return res;
        }
    }
}
