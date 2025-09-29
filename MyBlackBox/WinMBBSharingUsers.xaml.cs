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
    /// Interaction logic for WinMBBSharingUsers.xaml
    /// </summary>
    public partial class WinMBBSharingUsers : Window
    {
        private Int64 idNote;
        private int idUser;

        public WinMBBSharingUsers(int iduser, Int64 idnote, MBBSharingUsers.EventModifyData evn)
        {
            InitializeComponent();
            this.Activate();
            this.Topmost = true;
            this.idUser = iduser;
            this.idNote = idnote;
            ctrlMBBShare.Init(iduser,idnote, evn);
            //ctrlMBBShare.ShowGroups = false;
            ctrlMBBShare.FillUsersSharing(idnote);
        }

        public void SetLayoutPos(double leftMainWin, double widthMainWin, double topMainWin, double heightMainWin, int posBottomTop0LeftRigth1)
        {
            switch (posBottomTop0LeftRigth1)
            {
                case 0:
                    double v = topMainWin - this.Height;
                    if (v > 0)
                        this.Top = v;
                    else
                        this.Top = topMainWin + heightMainWin;
                    this.Left = leftMainWin;
                    this.Width = widthMainWin;
                    break;
                case 1:
                    double l = leftMainWin - this.Width;
                    if (l > 0)
                        this.Left = l;
                    else
                        this.Left = leftMainWin + widthMainWin;
                    this.Top = topMainWin;
                    this.Height = heightMainWin;
                    break;
            }
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            List<string> users = ctrlMBBShare.GetSharing();
            bool res = DbDataUpdate.InsertNoteSharing(users, this.idNote, this.idUser);
        }
    }
}
