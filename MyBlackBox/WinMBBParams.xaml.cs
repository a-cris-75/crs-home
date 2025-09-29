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
using MyBlackBoxCore.DataEntities;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WinMBBParams.xaml
    /// </summary>
    public partial class WinMBBParams : Window
    {
        public WinMBBParams()
        {
            InitializeComponent();
            this.ctrlMBBParams.Fill();
            this.ctrlMBBParams.IsUserChanged = false;
            this.ctrlMBBParams.IsRemoteDirChanged = false;
        }

        public void FillSyncroPendings(List<MBBSyncroPending> pendings)
        {
            this.ctrlMBBParams.FillSyncroPendings(pendings);
        }
        
        private void btnCanc_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public bool IsUserChanged = false;
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.ctrlMBBParams.Save();
            //UserGlobalInfo.IsUserChanged = this.ctrlMBBParams.IsUserChanged;
            this.IsUserChanged = this.ctrlMBBParams.IsUserChanged;
            this.DialogResult = true;
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
