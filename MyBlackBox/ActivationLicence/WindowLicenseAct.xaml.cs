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
using CRS.Library;

namespace ApplicationLicence
{
    /// <summary>
    /// Interaction logic for WindowLicenseAct.xaml
    /// </summary>
    public partial class WindowLicenseAct : Window
    {
        public WindowLicenseAct()
        {
            InitializeComponent();
            //-- UserGlobalInfo non è ancora inizializzato in caso di nuova registrazione
            ctrlLicenseAct.IsGenerationCodeModeUse = true;
            ctrlLicenseAct.SeteEventExit(EventExit);
        }

        //-- scrive su file ini i nuovi utenti registrati
        private void EventExit()
        {
            CRSIni iniFile = new CRSIni();
            iniFile.Name = CRSGlobalParams.IniRegisteredUsersPathName;

            string sec = ctrlLicenseAct.GetRegName() + "-" + ctrlLicenseAct.GetNumUsers();
            iniFile.SetValue(sec, "RegistrationName", ctrlLicenseAct.GetRegName());
            iniFile.SetValue(sec, "ActivationCode", ctrlLicenseAct.GetActCode());
            iniFile.SetValue(sec, "DateRegistration", ctrlLicenseAct.GetDateReg());
            iniFile.SetValue(sec, "NumUsers", ctrlLicenseAct.GetNumUsers());

            this.Close();
        }
    }
}
