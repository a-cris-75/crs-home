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
using CRS.DBLibrary;
using CRS.Library;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WindowLicenseAct.xaml
    /// </summary>
    public partial class WindowLicenseAct : Window
    {
        public WindowLicenseAct(string connString, string provider)
        {
            InitializeComponent();
            string lang = CRSIniFile.GetLanguageFromIni();
            //-- UserGlobalInfo non è ancora inizializzato in caso di nuova registrazione
            //DbFactory conn = new DbFactory(CRSGlobalParams.IniPathName, UserGlobalInfo.DBTypeApplication);
            DbFactory conn = new DbFactory(connString,provider);
            ctrlLicenseAct.Init(conn,lang,EventExit);
            ctrlLicenseAct.Translate(lang);
        }

        private void EventExit()
        {   
            this.Close();
        }

    
    }
}
