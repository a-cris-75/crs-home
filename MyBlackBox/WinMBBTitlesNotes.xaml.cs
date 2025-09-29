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
    /// Interaction logic for WinMBBTitlesNotes.xaml
    /// </summary>
    public partial class WinMBBTitlesNotes : Window
    {
        private int idUser = 0;
        //-- ALTEZZA E LARGHERZZA UGUALI
        private const int DIM_REDUCED = 260;
        private const int DIM_NORMAL = 535;

        //public delegate void EventSetFilter();
        //private EventSetFilter eventSetFilter;
        public WinMBBTitlesNotes()
        {
            InitializeComponent();
        }

        public WinMBBTitlesNotes(int iduser)
        {
            InitializeComponent();
            this.idUser = iduser;
            this.Activate();
            this.Topmost = true;
            //InitFilterFromIni();
        }

         private void InitCtrls()
        {
            ctrlMBBNotes.IsBtnExitVisible = false;
            ctrlMBBTitle.Init();
            ctrlMBBTitle.FillFavoritesTitle(this.idUser);
            ctrlMBBNotes.Init(this.idUser, null);           
            //-- questo evento serve solo per aggiornare anche la lista di note in MBBCore
            //-- dopo aver eventualmente eliminato una nota
            //trlMBBNotes.SetEvnRefresh(RefreshData);
        }
         public void SetEvnDblClickNote(MBBNotes.EventDoubleClick evn)
         {
             ctrlMBBNotes.SetEvnDblClick(evn);
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
    }
}
