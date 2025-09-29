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
    /// Interaction logic for WinMBBFind.xaml
    /// </summary>
    public partial class WinMBBFind : Window
    {
        private int idUser = 0;
        //-- ALTEZZA E LARGHERZZA UGUALI
        private const int DIM_REDUCED = 260;
        private const int DIM_NORMAL = 650;

        public delegate void EventSetFilter(/*List<DbNote> lst*/);
        private EventSetFilter eventSetFilter;
        
        public WinMBBFind(int iduser)
        {
            InitializeComponent();
            this.idUser = iduser;
            this.Activate();
            this.Topmost = true;
            InitFilterFromIni();
            this.IsHeaderExpanded = false;
            InitCtrls();
        }

        public WinMBBFind(int iduser, List<DbNote> lstnotes)
        {
            InitializeComponent();
            this.idUser = iduser;
            this.Activate();
            this.Topmost = true;
            this.IsHeaderExpanded = false;
            InitCtrls();
            FillExtNotes(lstnotes);
        }

        public string HEADER
        {
            set { expFilter.Header = value; }
            get { return expFilter.Header.ToString(); }
        }

        #region INIT and params from INI
        public void SaveInIni()
        {
            try
            {
                string sign = "greater";
                if (rbEqual.IsChecked.Value)
                    sign = "equal";
                if (rbLessThan.IsChecked.Value)
                    sign = "less";

                DateTime dt1 = DateTime.Now.AddDays(-7);
                DateTime dt2 = DateTime.Now.AddDays(1);
                if (dtDate1.SelectedDate != null)
                    dt1 = dtDate1.SelectedDate.Value;

                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_PERIOD, dt2.Subtract(dt1).Days);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_RATE, GetRate());
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_RATE_SIGN, sign);
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_TITLE, ctrlMBBTitle.txtTitle.Text);
                int val = 0;
                if (ctrlMBBTitle.TypeFindAtLeastOne)
                    val = 1;
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_FINDATLEASTONEWORD, val);
                val = 0;
                if (chkOnlyFavorites.IsChecked.Value)
                    val = 1;
                CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_FILTER_ONLYFAVORITES, val);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Save Error: " + ex.Message, "");
            }
        }

        public void InitFilterFromIni()
        {
            try
            {
                int period = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_PERIOD, 7));
                dtDate1.SelectedDate = DateTime.Now.AddDays(-period);
                dtDate2.SelectedDate = DateTime.Now.AddDays(1);

                rbWeek.IsChecked = true;
                if (period > 7)
                    rbMonth.IsChecked = true;
                if (period > 30)
                    rb3Month.IsChecked = true;
                if (period > 90)
                    rbYear.IsChecked = true;
                if (period > 365)
                    rbAll.IsChecked = true;

                string sign = Convert.ToString(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_RATE_SIGN));
                rbLessThan.IsChecked = true;
                if (sign == "equal")
                    rbEqual.IsChecked = true;
                if (sign == "greater")
                    rbGreaterThan.IsChecked = true;

                int rate = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_RATE));
                SetRate(rate);

                ctrlMBBTitle.txtTitle.Text = Convert.ToString(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_TITLE));
                int b = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_FINDATLEASTONEWORD,0));
                ctrlMBBTitle.TypeFindAtLeastOne = Convert.ToBoolean(b);

                b = Convert.ToInt32(CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_FILTER_ONLYFAVORITES, 0)); 
                chkOnlyFavorites.IsChecked = Convert.ToBoolean(b);

                ctrlMBBTitle.ApplyFilter(ctrlMBBTitle.txtTitle.Text);
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("InitFilter Error: " + ex.Message, "");
            }
        }

        private void InitCtrls()
        {
            ctrlMBBNotes.IsBtnExitVisible = false;
            ctrlMBBDocs.IsBtnExitVisible = false;
            ctrlMBBDocs.Init(this.idUser);
            ctrlMBBNotes.Init(this.idUser, EventSelectNote);           
            //-- questo evento serve solo per aggiornare anche la lista di note in MBBCore
            //-- dopo aver eventualmente eliminato una nota
            ctrlMBBNotes.SetEvnRefresh(RefreshData);
        }

        private void Translate()
        {

        }
        #endregion

        #region SET & GET
        public MBBFindParams GetFilterParams()
        {
            MBBFindParams res = new DataEntities.MBBFindParams();
            try
            {
                res.IDUser = this.idUser;
                if (dtDate1.SelectedDate != null)
                    res.Dt1 = dtDate1.SelectedDate.Value;
                res.Dt2 = dtDate2.SelectedDate.Value;
                res.Rate = GetRate();
                //-- simbolo di confronto con rate
                res.SimbolRate = ">=";
                if (rbLessThan.IsChecked.Value)
                    res.SimbolRate = "<";
                if (rbEqual.IsChecked.Value)
                    res.SimbolRate = "=";
                //-- titolo LIKE
                res.Title = ctrlMBBTitle.txtTitle.Text;
                res.TypeFindAtLeastOne = ctrlMBBTitle.TypeFindAtLeastOne;
                res.OnlyFavorites = chkOnlyFavorites.IsChecked.Value;
            }
            catch(Exception ex)
            {
                CRSLog.WriteLog("Exception in GetFilterParams: \n" + ex.Message, "") ;
            }
            return res;
        }

        public void SetFilterTitle(string title)
        {
            ctrlMBBTitle.txtTitle.Text = title;
        }

        public void SetEvnSelectTitle(MBBTitle.EventSelect evn)
        {
            this.ctrlMBBTitle.SetEventSelect(evn);
        }

        public void SetEvnSetFilter(EventSetFilter evn)
        {
            this.eventSetFilter = evn;            
        }

        public void SetEvnDblClickNote(MBBNotes.EventDoubleClick evn)
        {
            ctrlMBBNotes.SetEvnDblClick(evn);
        }

        public void SetEvnOpenNotes(MBBNotes.EventOpenNotes evn)
        {
            ctrlMBBNotes.SetEvnOpenNotes(evn);
        }

        public void SetEvnMergeNotes(MBBNotes.EventMergeNotes evn)
        {
            ctrlMBBNotes.SetEvnMergeNotes(evn);
        }

        public void EventSelectNote(Int64 idnote, int iduser)
        {
            ctrlMBBDocs.SetFocusDocs(idnote, iduser);
        }
        #endregion

        #region PROPERTIES
        public bool IsHeaderExpanded
        {
            set
            {
                //-- il senso è che se seleziono il filtro temporale cerco il testo 
                //-- altrimenti sto cercando solo il titolo
                //-- il filtro sul titolo al click sul tab item, filtra i dati per titolo
                if (value)
                {
                    pnlMain.RowDefinitions[0].Height = new GridLength(130/*60, GridUnitType.Star*/);
                    pnlMain.RowDefinitions[0].MaxHeight = 130;
                    pnlMain.RowDefinitions[0].MinHeight = 130;
                    pnlFilter.Visibility = System.Windows.Visibility.Visible;
                    expFilter.IsExpanded = true;
                    tcFind.SelectedItem = 1;
                    pnlWinBtns.Orientation = Orientation.Vertical;
                }
                else
                {
                    pnlMain.RowDefinitions[0].Height = new GridLength(36/*, GridUnitType.Star*/);
                    pnlMain.RowDefinitions[0].MaxHeight = 40;
                    pnlMain.RowDefinitions[0].MinHeight = 40;
                    pnlFilter.Visibility = System.Windows.Visibility.Hidden;
                    expFilter.IsExpanded = false;
                    tcFind.SelectedItem = 0;
                    pnlWinBtns.Orientation = Orientation.Horizontal;
                }
            }
            get
            {
                if (pnlMain.RowDefinitions[0].Height.Value <= 40)
                    return false;
                else return true;
            }
        }

        public bool IsReduced
        {
            set {
                this.ctrlMBBTitle.IsHeaderVisible = !value;
                if (value)
                {
                    this.Width = DIM_REDUCED;
                    //this.Height = DIM_REDUCED + 60;
                    ctrlMBBNotes.Width = DIM_REDUCED - 3;
                    
                    expFilter.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.Width = DIM_NORMAL;
                    this.Height = DIM_NORMAL;
                    expFilter.Visibility = System.Windows.Visibility.Visible;
                    CenterWindowOnScreen();
                }
            }

            get {
                if (this.Width == DIM_NORMAL)
                    return false;
                else return true;
            }
        }

        public int TabItemSelectedIndex
        {
            set { tcFind.SelectedIndex = value; }
            get { return tcFind.SelectedIndex; }
        }

        #endregion

        #region METHODS
        /*public void TabItem(string s)
        {
            ctrlMBBTitle.ApplyFilter(s);
            ctrlMBBNotes.ApplyFilter(s);
        }*/
        #endregion

        #region FILL
        public void FillTitle(int iduser)
        {
            ctrlMBBTitle.FillTitle(iduser);
        }

        public void FillFavoritesTitle(int iduser)
        {
            ctrlMBBTitle.FillFavoritesTitle(iduser);
        }

        public List<DbNote> FillNotes(MBBFindParams p)
        {
            return ctrlMBBNotes.FillNotes(p);
        }

        public void FillDocs(MBBFindParams p)
        {
            ctrlMBBDocs.FillDocs(p);
        }

        public void FillExtNotes(List<DbNote> lstnotes)
        {
            ctrlMBBTitle.FillExtTitle(lstnotes.Select(X => X.Title).Distinct().ToList());
            ctrlMBBTitle.txtTitle.Text = "";
            ctrlMBBNotes.FillNotesExt(lstnotes);
            List<DbNoteDoc> lstdocs = new List<DbNoteDoc>();
            foreach (DbNote n in lstnotes)
                lstdocs.AddRange(n.Docs);

            lstdocs = lstdocs.Distinct().ToList();

            ctrlMBBDocs.FillDocs(lstdocs);
        }


        public void ApplyFilterTitle(string filter)
        {
            ctrlMBBTitle.ApplyFilter(filter);
            ctrlMBBNotes.ApplyFilter(filter);
        }

        /// <summary>
        /// Serve esclusivamente ad inizializzare MBBNotes con l'evento di evnSetFilter passato dall'esterno. 
        /// Viene passato al controllo in modo che dopo aver eliminato una nota. ciò sia rilevabile anche in MBBCore,
        /// che subisce l'effetto di eventSetFilter(..).
        /// </summary>
        private void RefreshData()
        {
            MBBFindParams p = this.GetFilterParams();
            if(this.eventSetFilter!=null) 
                eventSetFilter(/*this.FillNotes(p)*/);
        }
        #endregion
       
        #region utilities
        public int GetRate()
        {
            int rate = 0;
            if (Convert.ToInt32(imgRat1.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat2.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat3.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat4.Tag) >= 10) rate++;
            if (Convert.ToInt32(imgRat5.Tag) >= 10) rate++;

            return rate;
        }

        private void SetRateFromImg(Image sender)
        {
            string star = CRSGlobalParams.IconRateYellowPathName;
            if (Convert.ToInt32((sender as Image).Tag) >= 10)
                star = CRSGlobalParams.IconRateGrayPathName;

            (sender as Image).Source = CRSImage.CreateImage(star);
            if (Convert.ToInt32((sender as Image).Tag) == 2)
            {
                imgRat1.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 3)
            {
                imgRat1.Source = CRSImage.CreateImage(star);
                imgRat2.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 4)
            {
                imgRat1.Source = CRSImage.CreateImage(star);
                imgRat2.Source = CRSImage.CreateImage(star);
                imgRat3.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 5)
            {
                imgRat1.Source = CRSImage.CreateImage(star);
                imgRat2.Source = CRSImage.CreateImage(star);
                imgRat3.Source = CRSImage.CreateImage(star);
                imgRat4.Source = CRSImage.CreateImage(star);
            }

            if (Convert.ToInt32((sender as Image).Tag) == 40)
            {
                imgRat5.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 30)
            {
                imgRat4.Source = CRSImage.CreateImage(star);
                imgRat5.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 20)
            {
                imgRat5.Source = CRSImage.CreateImage(star);
                imgRat4.Source = CRSImage.CreateImage(star);
                imgRat3.Source = CRSImage.CreateImage(star);
            }
            if (Convert.ToInt32((sender as Image).Tag) == 10)
            {
                imgRat5.Source = CRSImage.CreateImage(star);
                imgRat2.Source = CRSImage.CreateImage(star);
                imgRat3.Source = CRSImage.CreateImage(star);
                imgRat4.Source = CRSImage.CreateImage(star);
            }

            if (Convert.ToInt32((sender as Image).Tag) >= 10)
                (sender as Image).Tag = Convert.ToInt32((sender as Image).Tag) / 10;
            else (sender as Image).Tag = Convert.ToInt32((sender as Image).Tag) * 10;
        }

        private void SetRate(int rate)
        {
            string star = CRSGlobalParams.IconRateYellowPathName;

            imgRat1.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat2.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat3.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat4.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat5.Source = CRSImage.CreateImage(CRSGlobalParams.IconRateGrayPathName);
            imgRat1.Tag = 1;
            imgRat2.Tag = 2;
            imgRat3.Tag = 3;
            imgRat4.Tag = 4;
            imgRat5.Tag = 5;
            switch (rate)
            {
                case 1:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    break;
                case 2:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    break;
                case 3:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    break;
                case 4:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat4.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    imgRat4.Tag = 40;
                    break;
                case 5:
                    imgRat1.Source = CRSImage.CreateImage(star);
                    imgRat2.Source = CRSImage.CreateImage(star);
                    imgRat3.Source = CRSImage.CreateImage(star);
                    imgRat4.Source = CRSImage.CreateImage(star);
                    imgRat5.Source = CRSImage.CreateImage(star);
                    imgRat1.Tag = 10;
                    imgRat2.Tag = 20;
                    imgRat3.Tag = 30;
                    imgRat4.Tag = 40;
                    imgRat5.Tag = 50;
                    break;
            }
        }
        
        public void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        #endregion

        #region events controls
        private void imgRat5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetRateFromImg(sender as Image);
        }

        private void expFilter_Expanded(object sender, RoutedEventArgs e)
        {
            //pnlFilter.RowDefinitions[0].Height = new GridLength(60, GridUnitType.Star); //new GridLength(0);
            this.IsHeaderExpanded = true;
        }

        private void expFilter_Collapsed(object sender, RoutedEventArgs e)
        {
            this.IsHeaderExpanded = false;
        }

        private void btnSetFilter_Click(object sender, RoutedEventArgs e)
        {
            SaveInIni();
            MBBFindParams p = this.GetFilterParams();
            if(this.eventSetFilter!=null) 
                eventSetFilter(/*this.FillNotes(p)*/);

            FillDocs(p);
            tiNote.IsSelected = true;       
        }

        private void ctrlMBBTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    SaveInIni();
                    MBBFindParams p = this.GetFilterParams();
                    CRSLog.WriteLog("Find params: " +
                        "\n - Date from: " + p.Dt1.ToShortDateString() +
                        "\n - Date to: " + p.Dt2.ToShortDateString() +
                        "\n - Title: " + p.Title +
                        "\n - Rate: " + p.Rate.ToString() +
                        "\n - Only favorites: " + p.OnlyFavorites.ToString() +
                        "\n - At least one: " + p.TypeFindAtLeastOne.ToString()
                        , "");

                    if (this.eventSetFilter != null)
                        eventSetFilter(/*this.FillNotes(p)*/);
                    FillDocs(p);
                    tiNote.IsSelected = true;
                }
                catch(Exception ex)
                {
                    CRSLog.WriteLog("Exception in find: " +
                       ex.Message
                        , "");
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void rbToday_Click(object sender, RoutedEventArgs e)
        {
            int days = -1;
            if(rbToday.IsChecked.Value)
            {
                days = -1;
            }
            if (rbWeek.IsChecked.Value)
            {
                days = -7;
            }
            if (rbMonth.IsChecked.Value)
            {
                days = -30;
            }
            if (rb3Month.IsChecked.Value)
            {
                days = -90;
            }
            if (rbYear.IsChecked.Value)
            {
                days = -365;
            }
            if (rbAll.IsChecked.Value)
            {
                days = -3650;
            }

            dtDate1.SelectedDate = DateTime.Now.AddDays(days);
            dtDate2.SelectedDate = DateTime.Now.AddDays(1);
            dtDate1.DisplayDate = DateTime.Now.AddDays(days);
            dtDate2.DisplayDate = DateTime.Now.AddDays(1);
        }
       
        private void imgMaximize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
                this.WindowState = System.Windows.WindowState.Maximized;
            else this.WindowState = System.Windows.WindowState.Normal;
        }
        #endregion

    }
}
