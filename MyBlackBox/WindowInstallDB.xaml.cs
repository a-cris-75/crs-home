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

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WindowInstallDB.xaml
    /// </summary>
    public partial class WindowInstallDB : Window
    {
        public WindowInstallDB()
        {
            InitializeComponent();
            ctrlCRSManageDB.Init();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {          
            System.Windows.Forms.Application.Restart();
            this.Close();
        }
    }
}
