using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CRS.Library;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /*public partial class App : Application
    {
    }*/

    public partial class App : Application
    {
        // spostato e gestito in WinMBBCore
        // gestito in WinMBBCore e MBBCore
        //CRSEventListener KListener = new CRSEventListener();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            //if (WinMBBCore.IS_ACTIVE_KEY_EVENTS)
            {
                if (args.Key.ToString().Contains("ESC"))
                {
                    //WinMBBCore.evn 
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //KListener.Dispose();
        }
    }
}
