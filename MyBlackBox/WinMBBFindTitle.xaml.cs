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
using MyBlackBoxCore.DataEntities;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WinMBBFindTitle.xaml
    /// </summary>
    public partial class WinMBBFindTitle : Window
    {
        private int idUser = 0;
        private const int DIM_REDUCED = 260;
        private const int DIM_NORMAL = 535;

        public WinMBBFindTitle()
        {
            InitializeComponent();
        }

        public WinMBBFindTitle(int iduser)
        {
            InitializeComponent();
            this.idUser = iduser;
            this.Activate();
            this.Topmost = true;
            InitFilterFromIni();
        }

        public void InitFilterFromIni()
        {
            try
            {
                ctrlMBBTitle.txtTitle.Text = Convert.ToString(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_TITLE));
                ctrlMBBTitle.ApplyFilter(ctrlMBBTitle.txtTitle.Text);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("InitFilter Error: " + ex.Message, "");
            }
        }

        public void SetEvnSelectTitle(MBBTitle.EventSelect evn)
        {
            this.ctrlMBBTitle.SetEventSelect(evn);
        }

        public bool IsReduced
        {
            set
            {
                this.ctrlMBBTitle.IsHeaderVisible = !value;
                if (value)
                {
                    this.Width = DIM_REDUCED;

                }
                else
                {
                    this.Width = DIM_NORMAL;
                    this.Height = DIM_NORMAL;
                }
            }
            get
            {
                if (this.Width == DIM_NORMAL)
                    return false;
                else return true;
            }
        }

        public void SetLayoutPos(double leftMainWin, double widthMainWin, double topMainWin, double heightMainWin)
        {
            if (this.IsReduced)
            {
                double l = leftMainWin - this.Width;
                if (l > 0)
                    this.Left = l;
                else
                    this.Left = leftMainWin + widthMainWin;
                this.Top = topMainWin;
                this.Height = heightMainWin;
            }
        }

        public bool TypeFindAtLeastOne
        {
            set
            {
                ctrlMBBTitle.TypeFindAtLeastOne = value;
            }
            get
            {
                return ctrlMBBTitle.TypeFindAtLeastOne;
            }
        }

        #region FILL
        public void FillTitle(int iduser)
        {
            ctrlMBBTitle.FillTitle(iduser);
        }

        public void ApplyFilterTitle(string filter)
        {
            ctrlMBBTitle.ApplyFilter(filter);
        }
        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
    }
}
