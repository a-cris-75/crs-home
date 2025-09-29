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
    /// Interaction logic for WinMBBSharingNotes.xaml
    /// </summary>
    public partial class WinMBBSharingNotes : Window
    {
        /*public WinMBBSharingNotes()
        {
            InitializeComponent();
            // posiziono la fienstra in basso a dx
            SetLayoutPosCorner(false, false);
            ctrlMBBSharingNotes.Init();
        }*/

        public WinMBBSharingNotes(DbNote currentRootNote)
        {
            InitializeComponent();
            // posiziono la fienstra in basso a dx
            SetLayoutPosCorner(false, false);
            ctrlMBBSharingNotes.Init(currentRootNote);
        }

        public void FillSharingNotes(List<DbNote> lst)
        {
            ctrlMBBSharingNotes.FillSharingNotes(lst);
        }


        public void FillSharingNotes(MBBFindParams p)
        {
            ctrlMBBSharingNotes.FillSharingNotes(p);
        }

        
        public void SetLayoutPos(double leftMainWin, double widthMainWin, double topMainWin, double heightMainWin, int posBottomTop0LeftRigth1)
        {
            switch (posBottomTop0LeftRigth1) {
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

        public void SetLayoutPosCorner(bool isInTop, bool isInLeft)
        {
            this.Left = 0;
            this.Top = 0;
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width - 12;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            if (!isInLeft)
                this.Left = screenWidth - this.Width;
            if (!isInTop)
                this.Top = screenHeight - this.Height;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
