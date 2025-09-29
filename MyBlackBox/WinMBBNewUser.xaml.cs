using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CRS.WPFControlsLib;
using CRS.Library;
using CRS.DBLibrary;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WinNewUser.xaml
    /// </summary>
    public partial class WinMBBNewUser : Window
    {
        private DbFactory connSql;
        private static string connstr;
        private static string provider;
        bool isOffline = false;

        public WinMBBNewUser()
        {
            InitializeComponent();
            ctrlUser.Focus();
        }

        public bool IsOffline
        {
            set { isOffline = value; }
            get { return isOffline; }
        }

        public string User
        {
            set { ctrlUser.SetControlValue((string)value); }
            get { return ctrlUser.GetControlValue().ToString(); }
        }

        public bool SetParameters(string conn, string connProvider/*, EventExit evExit*/)
        {
            bool res = true;
            connstr = conn;
            provider = connProvider;
            connSql = new DbFactory(connstr, provider);
            //eventExit = evExit;

            if (connSql.TestConnection() == false)
            {
                //-- evidenzia mancanza di connessione e prova in locale
                imgOffline.Visibility = System.Windows.Visibility.Visible;
                textBlockOffline.Visibility = System.Windows.Visibility.Visible;
                this.isOffline = true;
                connSql.Dispose();
                connSql = null;
                res = false;
            }

            return res;
        }

        private void btnLogin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool res = SaveNewUser();
            if (res) this.Close();
        }

        private bool SaveNewUser()
        {
            try
            {
                bool res = false;
                ClearErrors();

                bool isValid = ctrlConfirmPwd.ValidateControl();

                if (isValid)
                {
                    isValid = (ctrlConfirmPwd.GetControlValue().ToString() == ctrlPwd.GetControlValue().ToString());
                    if (isValid == false)
                    {
                        ctrlConfirmPwd.SetValidationError("Confirmation error!! Retype confirm..");
                    }
                }

                if (isValid)
                {                   
                    int idusr = DbDataAccess.GetIDUser(ctrlUser.GetControlValue().ToString());
                    if (idusr < 0)
                    {
                        isValid = false;
                        res = DbDataUpdate.InsertNewUser(ctrlUser.GetControlValue().ToString(), ctrlPwd.GetControlValue().ToString());
                        if(res){
                            string s = ctrlUser.GetControlValue().ToString();
                            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USER, s);
                            string pwd = ctrlPwd.GetControlValue().ToString();
                            string appName = CRSIniFile.GetApplicationNameFromIni();
                            string appVers = CRSIniFile.GetApplicationVersionFromIni();
                            pwd = CRSLicense.EncodeRegCode(appName, appVers, pwd, 1);
                            CRSIniFile.SetPropertyValue(APPConstants.SEC_APPL, APPConstants.PROP_USERPWD, pwd);
                        }
                    }
                    else
                    {
                        textBlockError.Text = "User already exists !!";
                    }
                }
                this.DialogResult = res;
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error sane new user: " + ex.Message, "");
                return false; 
            }
        }

        private void ClearErrors()
        {
            ctrlUser.ClearValidationError();
            ctrlPwd.ClearValidationError();
            textBlockError.Text = "";
            ctrlConfirmPwd.ClearValidationError();
        }

        private void btnExit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
