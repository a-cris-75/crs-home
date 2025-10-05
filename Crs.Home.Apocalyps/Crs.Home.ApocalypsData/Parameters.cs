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
        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=..\\DB\\DB_LOTTO_APOCALYPS.mdf; Integrated Security=true;";
        private static string providerName = DbConst.PROVIDER_SQLSERVER;
        private static ConfigManager ConfiguratorManagerDB = new ConfigManager("", "DatabaseSettings");
        private static ConfigManager ConfiguratorManagerApp = new ConfigManager("", "AppSettings");

        public static string SeqFields {
            get
            {
                string? s = ConfiguratorManagerApp.GetValue("SEQ_FIELDS_FILE");
                return (string.IsNullOrEmpty(s)? "yyyy/mm/dd" : s);
            }
        }

        public static string PathFileStoricoEstrazioni
        {
            get
            {
                string? s = ConfiguratorManagerApp.GetValue("FILE_PATH_ESTRAZIONI");
                return (string.IsNullOrEmpty(s) ? "Estrazioni.txt" : s);
            }
        }
      
        public static string FormatDateFile
        {
            get
            {
                 string? s = ConfiguratorManagerApp.GetValue("FORMAT_DATE_FILE");
                return (string.IsNullOrEmpty(s) ? "dd/MM/yyyy" : s);
            }
        }

        public static bool SaveToDb
        {
            get
            {
                string? s = ConfiguratorManagerApp.GetValue("SAVE_TO_DB");
                if (string.IsNullOrEmpty(s))
                    return true;
                else
                {
                    bool res = true;
                    bool.TryParse(s, out res);
                    return res;
                }
            }
        }

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

        // TYPE_FIELDS_FILE
        public static int TypeFieldsFile
        {
            get
            {
                string? s = ConfiguratorManagerApp.GetValue("TYPE_FIELDS_FILE");
                if (string.IsNullOrEmpty(s))
                    return 1;
                else
                {
                    int res = 1;
                    int.TryParse(s, out res);
                    return res;
                }
            }
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
                if (ConfiguratorManagerDB.GetValue("ProviderName" + LBL_DB) != null)
                {
                    string? provname1 = ConfiguratorManagerDB.GetValue("ProviderName" + LBL_DB);
                    provname = !string.IsNullOrEmpty(provname1) ? provname1 : DbConst.PROVIDER_SQLSERVER;
                }
            }

            return (!string.IsNullOrEmpty(connstr) ? connstr : string.Empty);
        }

        private static string? GetConnectionString_fromJson(string db_label, out string formatdate)
        {
            string? res = "";
            res = ConfiguratorManagerDB.GetValue("ConnectionString" + db_label);
            string? formatdate1 = ConfiguratorManagerDB.GetValue("FormatDate" + db_label);
            if (string.IsNullOrEmpty(formatdate1))
                formatdate = "MM/dd/yyyy HH:mm:ss";
            else
                formatdate = formatdate1;

            return res;
        }
    }
}
