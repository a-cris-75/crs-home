using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyBlackBoxCore.DataEntities;
using CRS.WPFControlsLib;
using CRS.Library;
using System.Globalization;
//using System.Windows.Forms;
using System.Security.RightsManagement;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for WinMBBStickNotes.xaml
    /// </summary>
    public partial class WinMBBStickNotes : Window
    {
        const int MAX_WDT = 300;
        const int MAX_HGT = 350;
        public delegate void EventFillSingleNote(DbNote n);
        private EventFillSingleNote eventFillSingleNote;
        public delegate void EventHide();
        private EventHide eventHide;

        public MBBUserInfo UserInfo = null;
        private List<DbNote> LST_NOTES;
        private List<DbNote> LST_NOTES_FILTERED;
        private int CURRENT_PAGE
        {
            set { txtNumPage.Text = value.ToString(); }
            get { return Convert.ToInt32(txtNumPage.Text); }
        }
        public WinMBBStickNotes()
        {
            InitializeComponent();
            this.UserInfo = new MBBUserInfo();
            this.CtrlAcivities.SHOW_TOP_HEADER = false;
            SetDefaultDim();
        }

        #region MTHODS
        public System.Drawing.Point GetCorePosition(out double wdt, out double hgt)
        {
            System.Drawing.Point res = new System.Drawing.Point();
            res.Y = Convert.ToInt32(this.Top + 34);
            Point pointToWindow = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(pointToWindow);
            //res.X = Convert.ToInt32(this.Width - pnlGridMain.ColumnDefinitions[1].Width.Value) + 14;
            res.X = Convert.ToInt32(this.Left + pnlGridMain.ColumnDefinitions[0].ActualWidth) + 2;

            wdt = pnlGridMain.ColumnDefinitions[1].ActualWidth - 2;
            hgt = pnlGridMain.RowDefinitions[0].ActualHeight - pnlFilters.ActualHeight - 36;

            return res;
        }

        private void SetDefaultDim()
        {
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            double screenWgt = System.Windows.SystemParameters.WorkArea.Width - 12;
            this.Left = 6;
            this.Top = 6;
            this.Width = screenWgt - 12;
            this.Height = screenHeight - 12;
        }

        public void Init(MBBUserInfo user, List<DbNote> lstNotes, EventFillSingleNote evn1, EventHide evn2, int typecolor_0Day_1Activity_2AllDiff, bool showActivitiesFilter = false)
        {
            this.UserInfo = user;
            this.CtrlAcivities.UserInfo = user;
            this.pnlActivities.Visibility = (showActivitiesFilter) ? Visibility.Visible : Visibility.Collapsed;
            this.eventFillSingleNote = evn1;
            this.eventHide = evn2;
            this.LST_NOTES = lstNotes.ToList();
            this.LST_NOTES_FILTERED = this.LST_NOTES.ToList();
            FillNotes(lstNotes,0, typecolor_0Day_1Activity_2AllDiff);
            FillKeysTitle();
        }


        //private MBBActivity CURRENT_CTRL_ACTIVITY = null;
        public void SetParamsFilterControlActivity(DateTime dt1, DateTime dt2, bool onlywithnotes)
        {
            //DateTime dt1; DateTime dt2; bool onlywithnotes;
            //this.CtrlAcivities.GetParams(out dt1, out dt2, out onlywithnotes);
            this.CtrlAcivities.SetParams(dt1, dt2, onlywithnotes);
            
        }

        public Visibility GetVisAcivities()
        { return this.pnlActivities.Visibility; }

        private void FillNotes(List<DbNote> lstNotes, int currpage, int typecolor_0Day_1Activity)
        {            
            int idx1 = 0;
            int idx2 = 0;
            List<string> txt = new List<string>();
            try
            {
                pnlWrapMain.Children.Clear();
                if (lstNotes.Count() > 0)
                {
                    txtDateInterval.Text = "interval: " + lstNotes.Min(X => X.DateTimeLastMod).ToShortDateString() + " " +
                                            lstNotes.Min(X => X.DateTimeLastMod).ToShortTimeString() + " - " +
                                            lstNotes.Max(X => X.DateTimeLastMod).ToShortDateString() + " " +
                                            lstNotes.Max(X => X.DateTimeLastMod).ToShortTimeString();
                    txtNumNotes.Text = "tot: " + lstNotes.Count().ToString();
                    //dtFilter1.SelectedDate = lstNotes.Min(X => X.DateTimeLastMod);
                    //dtFilter2.SelectedDate = lstNotes.Max(X => X.DateTimeLastMod);
                }
                List <DbNote> lstNotesFiltered = lstNotes.OrderByDescending(X => X.DateTimeInserted).ToList();
                if (lstNotes.Count() > 30)
                {
                    lstNotesFiltered = lstNotesFiltered.OrderByDescending(X => X.DateTimeInserted).Skip(30*currpage).Take(30).ToList();
                }
                List<long> lstday = lstNotesFiltered.Select(X => (long)X.DateTimeLastMod.DayOfYear).Distinct().ToList();
                List<long> lstidact = lstNotesFiltered.Select(X => X.IDActivity).Distinct().ToList();

                foreach (DbNote n in lstNotesFiltered)
                {
                    // per evitare di inserire una nota nuova senza ancora testo
                    if (!string.IsNullOrEmpty(n.Text))
                    {
                        txt.Clear();
                        txt.AddRange(n.Text.Split('\n'));

                        CultureInfo myCI = new CultureInfo("en-US");
                        System.Globalization.Calendar myCal = myCI.Calendar;
                        CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;

                        long idxcolor = myCal.GetWeekOfYear(n.DateTimeInserted, myCWR, DayOfWeek.Monday);
                        if (typecolor_0Day_1Activity == 1)
                            idxcolor = lstidact.IndexOf(n.IDActivity);
                        else
                        if (typecolor_0Day_1Activity == 2)
                            idxcolor = lstday.IndexOf(n.DateTimeLastMod.DayOfYear);
                      
                        CRSContainerData c = new CRSContainerData(idxcolor, idx2++, null, txt, n.Title, "", n.DateTimeInserted,"", false, true, true, true, this.UserInfo.Language, n.ID, 0, MAX_WDT);
                        c.MAX_HEIGHT = MAX_HGT;

                        c.SetHeigth(0, true);
                        if (c.Width > MAX_WDT)
                            c.Width = MAX_WDT;

                        // in questo caso dato che sono già in ambito note devo comunque mostrare il pannello se esiste un documento
                        if (n.Docs != null)
                        {
                            c.SHOW_PNLBTN_GOTONOTES = (n.Docs.Count() > 0);
                        }

                        c.SetEventSelect(SelectItemUserControlEvent);
                        c.SetEventDblClick(DblClickItemMenuEvent);
                        c.SetEventImgClick(ClickImgEvent);
                        c.SetEventClose(RemoveNotesHided);

                        pnlWrapMain.Children.Add(c);
                        idx1++;
                    }
                }
            }
            catch { }
        }

        internal void FillActivitiesParams(bool v)
        {
            this.CtrlAcivities.FillActivitiesParams(v);
        }

        private void RemoveNotesHided()
        {
            try
            {
                List<CRSContainerData> lst = new List<CRSContainerData>();
                foreach (Control c in pnlWrapMain.Children)
                {
                    if (c is CRSContainerData && (c as CRSContainerData).Visibility == Visibility.Visible)
                        lst.Add(c as CRSContainerData); 
                }
                pnlWrapMain.Children.Clear();
                foreach (CRSContainerData c in lst)
                {
                    pnlWrapMain.Children.Add(c);                  
                }
            }
            catch { }
        }

        private CRSContainerData GetSelectedNote()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                    {
                        res = c as CRSContainerData;
                        break;
                    }
                }              
            }
            return res;
        }

        private CRSContainerData GetSelectedNoteWin()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSWinContainer)
                {
                    if ((c as CRSWinContainer).GetControl() is CRSContainerData)
                    {
                        CRSContainerData cd = (c as CRSWinContainer).GetControl() as CRSContainerData;
                        if (cd.IS_SELECTED)
                            res = cd;
                    }
                }
            }
            return res;
        }

        private List<DbNote> FilterNotes()
        {
            List<DbNote> lst = new List<DbNote>();
            List<DbNote> tmplst = new List<DbNote>();
            if (!string.IsNullOrEmpty(txtFilterTitle.Text))
            {
                List<string> delimiterChars = new List<string>(' ');
                List<string> lstword = txtFilterTitle.Text.Split(delimiterChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                lstword = lstword.Distinct().ToList();
                if (Convert.ToBoolean( rbFilterAND.IsChecked))
                {
                    lst.AddRange(this.LST_NOTES.Where(x => x.Title.ToLower().Contains(txtFilterTitle.Text.ToLower())).ToList());                    
                    lst = this.LST_NOTES.ToList();
                    // tutti i titoli che contengono tutte le parole del nuovo titolo
                    foreach (string s in lstword)
                    {
                        // memorizzo tutti i titoli che non contengono le parole del filtro
                        tmplst = lst.Where(X => X.Title.ToLower().Contains(s.ToLower()) == false).ToList();
                        foreach (DbNote ss in tmplst)
                            lst.Remove(ss);
                    }
                }
                else
                {
                    foreach (string s in lstword)
                        lst.AddRange(this.LST_NOTES.Where(x => x.Title.ToLower().Contains(s.Trim().ToLower())).ToList());                   
                }
            }
            else lst.AddRange(this.LST_NOTES.ToList());

            if (calendar.SelectedDates.Count > 0)
            {
                lst = lst.Where(x => x.DateTimeInserted >= calendar.SelectedDates.First() &&
                        x.DateTimeInserted <= calendar.SelectedDates.Last()).ToList();
            }

            lst = lst.DistinctBy(X => X.ID).ToList();
            FillNotes(lst, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
            return lst;
        }

        /*private List<string> GetStatus()
        {
            List<string> res = new List<string>();
            if (chkStsClose.IsChecked.Value) res.Add("Closed");
            if (chkStsDel.IsChecked.Value) res.Add("Deleted");
            if (chkStsStart.IsChecked.Value) res.Add("Started");
            if (chkStsWait.IsChecked.Value) res.Add("Waiting");
            return res;
        }*/

        /*public void FillActivities(List<string> status, DateTime? dt1, DateTime? dt2, string typeWhAndOr, bool withnotes)
        {
            List<DbActivity> lst = new List<DbActivity>();
            int typefilter = rbFindIns.IsChecked.Value ? 0 : 1;
            if (typeWhAndOr.ToUpper().Contains("AND"))
                lst = DbDataAccess.GetActivities(this.UserInfo.IDUser, dt1, dt2, status, typefilter, withnotes);
            else
            {
                List<DbActivity> lsttmp = DbDataAccess.GetActivities(this.UserInfo.IDUser, null, null, status, typefilter, withnotes);
                lst = DbDataAccess.GetActivities(this.UserInfo.IDUser, dt1, dt2, null, typefilter, withnotes);
                lst.AddRange(lsttmp.Where(Y => lst.Where(X => Y.TitleActivity == X.TitleActivity).Count() == 0));
            }

            int idx = 0;
            pnlActivitiesContainer.Children.Clear();
            this.LST_NOTES.Clear();
            foreach (DbActivity a in lst)
            {
                DateTime dtheader = a.LastStartLogActivity;
                if (dtheader == null || dtheader != null && dtheader == DateTime.MinValue)
                    dtheader = a.StartActivity;
                CRSContainerData c = new CRSContainerData(idx, idx, null, new List<string> { a.TextActivity }, a.TitleActivity, "", dtheader, "", false, true, true, true, this.UserInfo.Language, a.IDActivity, 0, 0);
                c.IS_FIXED_SIZE = false;
                c.SHOW_RESIZE = false;
                c.SetHeigth((int)c.GetAutoHeigthTitle(), false);

                DockPanel.SetDock(c, Dock.Top);
                c.SetEventSelect(DeselectActivities);

                // per poter fare un SetHeight che funzioni prma devo attaccare il controllo al children
                pnlActivitiesContainer.Children.Add(c);

                idx++;
            }
        }*/

        private void FillKeysTitle()
        {
            List<KeyValuePair<string,int>> lst = MBBCommon.GetListDirKeysOccurrences(this.LST_NOTES.Select(X => X.Title).ToList());
            lstviewListKeys.Items.Clear();
            foreach (KeyValuePair<string, int> k in lst.OrderBy(X => X.Key).OrderByDescending(X => X.Value))
            {
                //CRSLabelControl i = new CRSLabelControl("checkbox", k.Key + " (" + k.Value.ToString() + ")");
                CheckBox i = new CheckBox();
                i.Content = k.Key + " (" + k.Value.ToString() + ")";
                i.Click += I_Click;
                lstviewListKeys.Items.Add(i);
            }
        }

        private void I_Click(object sender, RoutedEventArgs e)
        {
            //string cnt = Convert.ToString((sender as CheckBox).Content);
            string txt = Convert.ToString((sender as CheckBox).Content);
            txt = txt.Substring(0, txt.IndexOf("(")).Trim();
            if (Convert.ToBoolean((sender as CheckBox).IsChecked))
            {
                txtFilterTitle.Text = txtFilterTitle.Text.Trim() + " " + txt;
            }
            else
            {
                if (txtFilterTitle.Text.Contains(txt))
                {
                    int idx = txtFilterTitle.Text.IndexOf(txt);
                    txtFilterTitle.Text = txtFilterTitle.Text.Substring(0, idx) + " " +
                                          txtFilterTitle.Text.Substring(idx + txt.Length);
                }
            }
            txtFilterTitle.Text = txtFilterTitle.Text.Trim();
        }
        /*
        private CRSContainerData GetSelectedActivity()
        {        
            foreach (Control c in pnlActivitiesContainer.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                    {
                        res = c as CRSContainerData;
                        break;
                    }
                }
            }
            return res;
        }*/

        /*private void DeselectActivities()
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlActivitiesContainer.Children)
                {
                    if (c is CRSContainerData)
                    {
                        (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }*/
        #endregion

        #region EVENTS DELEGATE
        private void DblClickItemMenuEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DbNote n = DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, GetSelectedNote().ID, true);
            if (this.eventFillSingleNote != null)
                this.eventFillSingleNote(n);
        }

        private void ClickImgEvent()
        {
            DbNote n = DbDataAccess.GetNoteAndDocs(this.UserInfo.IDUser, GetSelectedNote().ID, true);
            WinMBBDocs wind = new WinMBBDocs(this.UserInfo.IDUser);
            wind.FillDocs(n.Docs);
            wind.ShowDialog();
        }

        private void SelectItemWinEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlWrapMain.Children)
                {
                    if (c is CRSWinContainer)
                    {
                        if ((c as CRSWinContainer).GetControl() is CRSContainerData)
                        {
                            CRSContainerData cd = (c as CRSWinContainer).GetControl() as CRSContainerData;
                            cd.IS_SELECTED = false;
                        }
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void SelectItemUserControlEvent()
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                foreach (Control c in pnlWrapMain.Children)
                {
                    if (c is CRSContainerData)
                    {
                        (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }

        #endregion
        
        #region EVENTS CONTROLS
        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.eventHide != null)
                this.eventHide();
            this.Close();
        }

        private void txtFilterTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
               this.LST_NOTES_FILTERED = FilterNotes();
            }
        }

        private void btnSetFilter_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            this.LST_NOTES_FILTERED = FilterNotes();
            this.Cursor = Cursors.Arrow;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            FillNotes(this.LST_NOTES, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
            this.LST_NOTES_FILTERED = this.LST_NOTES.ToList();
        }

        private void imgMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void imgMaximize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
            {
                this.WindowState = WindowState.Normal;
                SetDefaultDim();
            }
        }

        private void imgResizeNotes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(imgResizeNotes.Tag) == 0)          
                imgResizeNotes.Tag = 1;           
            else imgResizeNotes.Tag = 0;           
            
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSContainerData && (c as CRSContainerData).Visibility == Visibility.Visible)
                {
                    //(c as CRSContainerData).SHOW_RESIZE = false;
                    if (Convert.ToInt32(imgResizeNotes.Tag) == 0)
                        (c as CRSContainerData).SetHeigth(0, true);
                    else
                    {
                        (c as CRSContainerData).SHOW_RESIZE = false;
                        (c as CRSContainerData).SetHeigth((int)(c as CRSContainerData).GetAutoHeigthTitle(), false);
                    }
                }
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            // goto prev page
            if (this.CURRENT_PAGE > 0)
            {
                this.CURRENT_PAGE--;
                FillNotes(this.LST_NOTES_FILTERED, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            // goto prev page
            if (this.CURRENT_PAGE < (int) this.LST_NOTES_FILTERED.Count / Convert.ToInt32(txtNumPerPage.Text))
            {
                this.CURRENT_PAGE++;
                FillNotes(this.LST_NOTES_FILTERED, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
            }
        }

        private void imgSetColorNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Convert.ToInt32(imgSetColorNote.Tag) == 0)
                imgSetColorNote.Tag = 1;
            else imgSetColorNote.Tag = 0;
            FillNotes(this.LST_NOTES_FILTERED, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
        }

        private void txtNumPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillNotes(this.LST_NOTES_FILTERED, this.CURRENT_PAGE, Convert.ToInt32(imgSetColorNote.Tag));
            }
        }

        private void btnChainEmptyActivityNotes_Click(object sender, RoutedEventArgs e)
        {
            pnlActivities.Visibility = Visibility.Collapsed;
            bool toshow = false;
            if (Convert.ToInt32(btnChainEmptyActivityNotes.Tag) == 1)
            {
                toshow = true;
                pnlActivities.Visibility = Visibility.Visible;

                this.CtrlAcivities.FillActivitiesParams(false);

                /*DateTime? dt1 = null;
                DateTime? dt2 = null;
                if (dtFilter1.SelectedDate != null && dtFilter1.SelectedDate.Value > DateTime.MinValue)
                    dt1 = dtFilter1.SelectedDate.Value;
                // devo aggiungere 24 H dato che altrimenti la data fine è il giorno a mezzanotte
                if (dtFilter2.SelectedDate != null && dtFilter2.SelectedDate.Value > DateTime.MinValue)
                    dt2 = dtFilter2.SelectedDate.Value.AddDays(1);

                List<string> sts = this.GetStatus();
                FillActivities(sts, dt1, dt2, "AND", chkStsWithNotes.IsChecked.Value);
                */
            }
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSContainerData)
                {
                    (c as CRSContainerData).SHOW_CHK_SELECTED = toshow;
                }
            }

            if (Convert.ToInt32(btnChainEmptyActivityNotes.Tag) == 1)
                btnChainEmptyActivityNotes.Tag = 0;
            else btnChainEmptyActivityNotes.Tag = 1;
        }

        /*private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dt1 = null;
            DateTime? dt2 = null;
            if (dtFilter1.SelectedDate != null && dtFilter1.SelectedDate.Value > DateTime.MinValue)
                dt1 = dtFilter1.SelectedDate.Value;
            // devo aggiungere 24 H dato che altrimenti la data fine è il giorno a mezzanotte
            if (dtFilter2.SelectedDate != null && dtFilter2.SelectedDate.Value > DateTime.MinValue)
                dt2 = dtFilter2.SelectedDate.Value.AddDays(1);

            List<string> sts = this.GetStatus();
            FillActivities(sts, dt1, dt2, "AND", chkStsWithNotes.IsChecked.Value);           
        }*/
       
        private void btnShowFilter_Click(object sender, RoutedEventArgs e)
        {
            if (pnlGridMain.RowDefinitions[0].Height.Value <= 40)
            {
                pnlGridMain.RowDefinitions[0].Height = new GridLength(138);
            }
            else
            {
                pnlGridMain.RowDefinitions[0].Height = new GridLength(34);
            }
        }

        private void chkShowSelInNotes_Click(object sender, RoutedEventArgs e)
        {
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSContainerData)
                {
                    (c as CRSContainerData).SHOW_CHK_SELECTED = true;
                    (c as CRSContainerData).IS_CHK_SELECTED = chkShowSelInNotes.IsChecked.Value;
                }
            }
        }

        /*private void dtFilter1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dtFilter1.SelectedDate != null && dtFilter1.SelectedDate > DateTime.MinValue) ||
               (dtFilter2.SelectedDate != null && dtFilter2.SelectedDate > DateTime.MinValue) &&
                chkStsWait.IsChecked.Value)
            {
                chkStsStart.IsChecked = true;
            }
        }*/

        private void imgNoteActivityOk_Click(object sender, RoutedEventArgs e)
        {
            CRSContainerData res = CtrlAcivities.GetSelectedActivity();

            if (res != null)
            {
                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_SAVE_ASSOCIATION_NOTE_ACTIVITY, this.UserInfo.Language) +
                                                    "\n" + CRSTranslation.TranslateDialog(CRSTranslation.MSG_DO_YOU_WANT_CONTINUE, this.UserInfo.Language),
                                       CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {

                    long idact = -1;
                    idact = res.ID;
                    foreach (CRSContainerData c in pnlWrapMain.Children)
                    {
                        if (c.SHOW_CHK_SELECTED)
                        {
                            DbDataUpdate.InsertActivityNote(c.ID, idact);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_HAVE_TO_SEL_ITEM, this.UserInfo.Language) ,
                                       CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language));
            }
        }
       
        private void imgNoteActivityCanc_Click(object sender, RoutedEventArgs e)
        {
            pnlActivities.Visibility = Visibility.Collapsed;
            foreach (Control c in pnlWrapMain.Children)
            {
                if (c is CRSContainerData) (c as CRSContainerData).SHOW_CHK_SELECTED = false;              
            }
        }
       
        private void btnGoToDocs_Click(object sender, RoutedEventArgs e)
        {
            List<DbNoteDoc> docs = new List<DbNoteDoc>();
            if (this.LST_NOTES.Count > 0) 
            {          
                foreach(DbNote n in this.LST_NOTES_FILTERED)
                {
                    docs.AddRange(n.Docs);
                }
            }
            WinMBBDocs wind = new WinMBBDocs(this.UserInfo.IDUser);
            wind.FillDocs(docs);
            wind.ShowDialog();
        }
        #endregion
    }
}
 