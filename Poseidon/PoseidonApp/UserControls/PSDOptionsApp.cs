using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CRS.Library;
using CRS.DBLibrary;
using CRS.CommonControlsLib;
using System.Data.SqlServerCe;

namespace PoseidonApp.UserControls
{
    public partial class PSDOptionsApp : UserControl
    {
        public PSDOptionsApp()
        {
            InitializeComponent();
        }

        public string GetConnectionStringStandard()
        {
            string res = "";
            string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;
            if (string.IsNullOrEmpty(cbDBPROVIDER.Text))
                cbDBPROVIDER.Text = "System.Data.SqlServerCe.4.0";
            res = Utils.GetConnectionStringFromParams(cbDBPROVIDER.Text, txtDbServer.Text, txtDbUser.Text, txtDbPwd.Text, "", assemblyname);
            return res;
        }
        public string GetConnectionStringTest()
        {
            string res = "";
            string assemblyname = Assembly.GetExecutingAssembly().GetName().Name;
            if(string.IsNullOrEmpty(cbDBPROVIDER_TEST.Text))
                cbDBPROVIDER_TEST.Text = "System.Data.SqlServerCe.4.0";
            res = Utils.GetConnectionStringFromParams(cbDBPROVIDER_TEST.Text, txtDbServer_TEST.Text, txtDbUser_TEST.Text, txtDbPwd_TEST.Text, "", assemblyname);
            return res;
        }

        public string GetProviderNameStandard()
        {
            string res = "";
            if (string.IsNullOrEmpty(cbDBPROVIDER.Text))
                cbDBPROVIDER.Text = "System.Data.SqlServerCe.4.0";
            res = cbDBPROVIDER.Text;
            return res;
        }

        public string GetProviderNameTest()
        {
            string res = "";
            res = cbDBPROVIDER_TEST.Text;
            return res;
        }

        public void SaveIniInfo()
        {
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBSERVER, txtDbServer.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBSERVER, txtDbServer_TEST.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBUSER, txtDbUser.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBUSER, txtDbUser_TEST.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBPWD, txtDbPwd.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBUSER, txtDbPwd_TEST.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_DBTYPE, chkDbConnActStd.Checked ? 1 : 2);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_DBSECTION, chkDbConnActStd.Checked ? "DBSERVER" : "DBTEST");

            CRSIniFile.SetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILENAME, txtFileEstrPath.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILEWEBURL, txtURLFileEstr.Text);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILETYPE, cbTipoFileImport.Text);

            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR1DOM, chkDayEstr0.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR2LUN, chkDayEstr1.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR3MAR, chkDayEstr2.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR4MER, chkDayEstr3.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR5GIO, chkDayEstr4.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR6VEN, chkDayEstr5.Checked ? 1 : 0);
            CRSIniFile.SetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR7SAB, chkDayEstr6.Checked ? 1 : 0);

            APPGlobalInfo.InitIniInfo();
        }

        public void GetIniInfo()
        {
            txtDbServer.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBSERVER);
            txtDbServer_TEST.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBSERVER);
            txtDbUser.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBUSER );
            txtDbUser_TEST.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBUSER );
            txtDbPwd.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBSERVER, APPConstants.PROP_DBPWD );
            txtDbPwd_TEST.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_DBTEST, APPConstants.PROP_DBUSER);

            txtFileEstrPath.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILENAME);
            txtURLFileEstr.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILEWEBURL);
            cbTipoFileImport.Text = CRSIniFile.GetPropertyValue(APPConstants.SEC_FILE_IMPORT, APPConstants.PROP_IMPORTFILETYPE);

            chkDayEstr0.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR1DOM)=="1";
            chkDayEstr1.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR2LUN)=="1";
            chkDayEstr2.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR3MAR)=="1";
            chkDayEstr3.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR4MER)=="1";
            chkDayEstr4.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR5GIO)=="1";
            chkDayEstr5.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR6VEN)=="1";
            chkDayEstr6.Checked = CRSIniFile.GetPropertyValue(APPConstants.SEC_APP_PARAMS, APPConstants.PROP_ESTR7SAB)=="1";

            string DB_SECTION = CRSIniFile.GetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_DBSECTION);
            APPGlobalInfo.CONNECTION_STRING = CRSIniFile.GetConnectionStringFromIni(DB_SECTION);
            APPGlobalInfo.PROVIDER_NAME = CRSIniFile.GetProviderFromIni(DB_SECTION);
              
            APPGlobalInfo.IMPORT_FILE_PATH = txtFileEstrPath.Text;
            APPGlobalInfo.IMPORT_FILE_URL = txtURLFileEstr.Text;
            APPGlobalInfo.IMPORT_FILE_TYPE = cbTipoFileImport.Text;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (DbFactory.TestConnection(this.GetConnectionStringStandard(), this.GetProviderNameStandard()))
                ABSMessageBox.Show("CONNESSIONE RIUSCITA!");
            else ABSMessageBox.Show("CONNESSIONE FALLITA!!\nControlla i parametri di connessione..");
        }

        private void btnConnectTest_Click(object sender, EventArgs e)
        {
            if (DbFactory.TestConnection(this.GetConnectionStringTest(), this.GetProviderNameTest()))
                ABSMessageBox.Show("CONNESSIONE RIUSCITA!");
            else ABSMessageBox.Show("CONNESSIONE FALLITA!!\nControlla i parametri di connessione..");
        }

        private void chkDbConnActStd_Click(object sender, EventArgs e)
        {
            chkDbConnActStd.Checked = !chkDbConnActTest.Checked;
            chkDbConnActTest.Checked = !chkDbConnActStd.Checked;
        }

        private void btnOpenFileEstr_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = txtFileEstrPath.Text;
            DialogResult r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                txtFileEstrPath.Text = filename;
            }
        }

        private void btnSqlServerCeUprade_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCeEngine engine = new SqlCeEngine(GetConnectionStringStandard());
                engine.Upgrade();

                SqlCeConnection conn = null;
                conn = new SqlCeConnection(GetConnectionStringStandard());
                conn.Open();

                if (DbFactory.TestConnection(this.GetConnectionStringStandard(), this.GetProviderNameStandard()))
                    ABSMessageBox.Show("CONNESSIONE RIUSCITA!");
                else ABSMessageBox.Show("CONNESSIONE FALLITA!!\nControlla i parametri di connessione..");
            }
            catch { throw; }
        }

        private void btnSelFileDB_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = txtDbServer.Text;
            DialogResult r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                txtDbServer.Text = filename;
            }
        }
    }
}
