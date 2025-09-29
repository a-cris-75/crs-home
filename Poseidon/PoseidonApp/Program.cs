using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRS.MainFormIniLib;
using PoseidonApp.UserControls;
using CRS.Library;
using CRS.DBLibrary;
using CRS.CommonControlsLib;
using System.Diagnostics;
using System.Reflection;

namespace PoseidonApp
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;

            CRSIniFile.InitApplicationIni(assemblyname);
            //-- serve a copiare il file sdf nella dir dell'applicazione in modo da non sovrascrivere successivametne il file per ogni upgrade
            //-- dell'applicazione; se il file sdf esiste già non fa niente 

            Utils.InitDbFile(CRSIniFile.GetDBNameFromIni(), assemblyname);//PoseidonBase.sdf

            DbFactory conn = new DbFactory(CRSIniFile.GetConnectionStringFromIniSQLServerCE(assemblyname), CRSIniFile.GetProviderFromIni());
            if (conn.TestConnection() == false)
                ABSMessageBox.Show("Connessione al DB non riuscita!");

            // -- INIZIALIZZO DLL DI ACCESSO AI DATI
            PoseidonData.Parameters.ConnectionString = conn.ConnectionString;
            PoseidonData.Parameters.ProviderName = conn.ProviderName;

            MainFormIni winmain = new MainFormIni();

            winmain.ASSEMBLY_NAME = assemblyname;
            winmain.TITLE_APP = "POSEIDON";

            PSDParams p = new PSDParams();
            p.Dock = DockStyle.Fill;

            PSDOptionsAppMain po = new PSDOptionsAppMain();
            po.InitFromGlobal();
            po.Dock = DockStyle.Fill;

            APPGlobalInfo.InitIniInfo();

            winmain.UC_HEADER_PARAMS1 = p;
            winmain.UC_HEADER_PARAMS2 = po;

            Application.Run(winmain);
        }
    }
}
