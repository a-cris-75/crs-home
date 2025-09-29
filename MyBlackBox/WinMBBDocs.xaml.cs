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
    /// Interaction logic for WinMBBDocs.xaml
    /// </summary>
    public partial class WinMBBDocs : Window
    {
        public WinMBBDocs(int iduser)
        {
            InitializeComponent();
            this.Activate();
            this.Topmost = true;

            ctrlMBBDocs.Init(iduser);
        }

        public int FillDocs(long idnote, int iduser)
        {            
            return ctrlMBBDocs.FillDocs(idnote, iduser);
        }

        public int FillDocs(int iduser)
        {
            return ctrlMBBDocs.FillDocs(iduser);
        }

        public int FillDocs(int iduser, DateTime dt1, DateTime dt2)
        {
            ctrlMBBDocs.dtFilter1.SelectedDate = dt1;
            ctrlMBBDocs.dtFilter2.SelectedDate = dt2;
            return ctrlMBBDocs.FillDocs(iduser, dt1, dt2);
        }

        public int FillDocs(List<DbNoteDoc> l)
        {
            return ctrlMBBDocs.FillDocs(l);
        }

        //-- serve esclusivamente ad inizializzare MBBNotes con l'evento di evnSetFilter passato dall'esterno
        public void SetEventRefresh(MBBDocs.EventRefreshData evn)
        {
            ctrlMBBDocs.SetEvnRefresh(evn);
        }

        public void SetLayoutPos(double leftMainWin, double widthMainWin, double topMainWin, double heightMainWin)
        {
            double l = leftMainWin - this.Width;
            if (l > 0)
                this.Left = l;
            else
                this.Left = leftMainWin + widthMainWin;
            this.Top = topMainWin;
            this.Height = heightMainWin;           
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
