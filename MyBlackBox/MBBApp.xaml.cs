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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CRS.Library;
using System.IO;
using System.Diagnostics;
using CRS.WPFControlsLib;
using Microsoft.Win32;
using System.Windows.Threading;
using MyBlackBoxCore.DataEntities;
//using System.Windows.Forms;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBApp.xaml
    /// </summary>
    public partial class MBBApp : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                         (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void EventHide();
        private EventHide eventHide;

        // PID | FappProcessName | FappExePath | FappExeName | DateTime last focus
        List<CRSProcessType> LST_RUNNING_PROCESS = new List<CRSProcessType>();

        const int SECONDS_TIMER = 6;
        readonly DispatcherTimer timerReadCurrProc = new DispatcherTimer();
        private string LOG_FILE_NAME;
        public MBBUserInfo UserInfo = null;

        public MBBApp()
        {
            InitializeComponent();
        }

        private void SetEventHide(EventHide evn)
        {
            this.eventHide = evn;
        }

        public void Init(EventHide evn, MBBUserInfo user)
        {
            SetEventHide(evn);
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userFilePath = System.IO.Path.Combine(localAppData,"MyBlackBox");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);

            this.LOG_FILE_NAME = System.IO.Path.Combine(userFilePath, "LogApp.txt");
            this.UserInfo = user;//new MBBUserInfo();
            SetTimer();
            this.timerReadCurrProc.Start();

            dtFromCount.SelectedDateTime = DateTime.Now.AddHours(-DateTime.Now.Hour + 8)
                .AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);
            
            List <CRSProcessType> lst = FillRecentApp(dtFromCount.SelectedDateTime);
            FillPersonalApp(lst);
        }

        public void SetDimPos(int step, double fixedwdt)
        {
            Window win = Window.GetWindow(this.Parent);
            // lascio un po di spaio in basso
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 64;
            double screenWgt = System.Windows.SystemParameters.WorkArea.Width - 12;

            if (fixedwdt > 0)
                win.Width = fixedwdt;
            // step 0: auto heigth
            // step 1: sposta su lato dx schermo a 3/4
            // step 2: sposta su lato dx schermo in tutta altezza
            // step 3: sposta su lato dx schermo in tutta altezza
            if (step == 0)
            {
                double hgtact = 0;
                foreach (CRSContainerData c in pnlMainRecent.Children)
                {
                    hgtact = hgtact + c.Height;
                }

                hgtact = pnlBtnsRecent.Height +
                    pnlGridMain.RowDefinitions[0].Height.Value +
                    hgtact + 12 + 36;

                if (hgtact >= screenHeight)
                {
                    //altezza di default
                    hgtact = screenHeight;
                }

                win.Height = hgtact;
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 1)
            {
                win.Height = (screenHeight / 2);
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 2)
            {
                win.Height = (screenHeight / 3) * 2;
                win.Top = screenHeight - win.Height;
                win.Left = screenWgt - win.Width;
            }
            else
            if (step == 3)
            {
                win.Top = 12;
                win.Height = screenHeight;
                win.Left = screenWgt - win.Width;
            }
        }


        public void FillPersonalApp(List<CRSProcessType> lstrecentproc)
        {
            
            CRSIni iniFile = new CRSIni();
            iniFile.Name = CRSIniFile.IniPathName;
            if (iniFile.HasSection(APPConstants.SEC_APP_USING) == false)
            {
                iniFile.SetSection(APPConstants.SEC_APP_USING);
            }
            int idx = 0;
            List<string> entries = iniFile.GetEntryNames(APPConstants.SEC_APP_USING);

            pnlMainPersonal.Children.Clear();
            foreach (string s in entries)
            {
                string val = Convert.ToString(iniFile.GetValue(APPConstants.SEC_APP_USING, s));
                string valexe = val;
                string valdt = "";
                if (val.IndexOf("||") >= 0)
                {
                    valexe = val.Substring(0, val.IndexOf("||"));
                    valdt = val.Substring(val.IndexOf("||") + 2);
                }
                DateTime dt = DateTime.MinValue;
                if(!string.IsNullOrEmpty(valdt)) dt = Convert.ToDateTime(valdt);

                string lblval = CRSTranslation.TranslateDialog(CRSTranslation.WRD_APP, this.UserInfo.Language);
                bool isContent = false;
                if(lstrecentproc.Where(X=>X.AppExePath == valexe).Count()>0)
                {
                    isContent = true;
                    CRSProcessType p = lstrecentproc.Where(X => X.AppExePath == val).First();
                    val = p.LastFocus +
                          "\n" + ((p.RunningSeconds / 8 * 60 * 60) * 100).ToString("0.00") + " %";

                    lblval = CRSTranslation.TranslateDialog(CRSTranslation.MSG_LAST_RUN, this.UserInfo.Language)+": " +
                        "\n% in 8h: ";
                }
                CRSContainerData d = CreateAppCtrl(idx, s, val, lblval, isContent,dt, this.UserInfo.Language);
                DockPanel.SetDock(d, Dock.Top);
                pnlMainPersonal.Children.Add(d);
                idx++;
            }
        }

        public List<CRSProcessType> FillRecentApp(DateTime? fromDt)
        {
            // scrivendo ogni 6 sec (10 volte al minuto) mantengo almeno una giornata 
            // (nel caso peggiore in cui ho registrato qualcosa) per stabilire le applicazioni recenti
            // 10*60*24
            int numrows = 240 * 60;
            if (fromDt!=null && fromDt > DateTime.MinValue)
            {
                numrows = (int)Math.Truncate( DateTime.Now.Subtract(Convert.ToDateTime(fromDt)).TotalSeconds / 6);
            }

            List<string> entries = CRSFileUtil.ReadFile(numrows, this.LOG_FILE_NAME);
            List<CRSProcessType> lstproc = new List<CRSProcessType>();
            List<CRSProcessType> lstsingleproc = new List<CRSProcessType>(); 
            try
            {
                if (entries != null && entries.Count() > 0)
                {
                    foreach (string s in entries)
                    {
                        CRSProcessType p = new CRSProcessType();

                        int idx1 = s.IndexOf("[");
                        int idx2 = s.IndexOf("]");
                        DateTime dt = DateTime.MinValue;
                        if (idx2 > 0)
                        {
                            string dateins = s.Substring(idx1 + 1, idx2 - idx1 - 1);
                            dt = Convert.ToDateTime(dateins);
                            p.LastFocus = dt;
                        }

                        idx1 = s.IndexOf("PATH:");
                        idx2 = s.IndexOf("|");
                        if (idx1 > 0 && idx2 > 0)
                            p.AppExePath = s.Substring(idx1 + 5, idx2 - idx1 - 5);

                        idx1 = s.IndexOf("PROCNAME:");
                        idx2 = s.LastIndexOf("|");
                        if (idx1 > 0 && idx2 > 0)
                            p.ProcName = s.Substring(idx1 + 9, idx2 - idx1 - 9);

                        lstproc.Add(p);
                    }
                    // considero solo le ultime 24 ore
                    
                    //lstproc = lstproc.Where(X => X.LastFocus >= DateTime.Now.AddDays(-1)).ToList();
                    lstproc = lstproc.OrderBy(X => X.LastFocus).ToList();

                    List<Tuple<string, int>> lstNameApp = new List<Tuple<string, int>>();//lstproc.Select(X => X.AppExeName).Distinct().ToList();

                    // ho una lista di stringhe con rilevazioni dei processi attivi 
                    // ho una riga nuova solo se il processo attivo è diverso dal precedente al momento della rilevazione
                    for (int idx = 0; idx < lstproc.Count() - 1; idx++)
                    {
                        DateTime dt1 = lstproc.ElementAt(idx).LastFocus;
                        DateTime dt2 = lstproc.ElementAt(idx + 1).LastFocus;
                        int totsec = (int)Math.Truncate(dt2.Subtract(dt1).TotalSeconds);
                        if (lstsingleproc.Where(X => X.ProcName == lstproc.ElementAt(idx).ProcName).Count() > 0)
                        {
                            CRSProcessType p = lstsingleproc.Where(X => X.ProcName == lstproc.ElementAt(idx).ProcName).First();
                            p.RunningSeconds = p.RunningSeconds + totsec;
                            p.LastFocus = dt1;
                        }
                        else lstsingleproc.Add(lstproc.ElementAt(idx));
                    }
                    if(pnlMainRecent.Children.Count>0)
                        pnlMainRecent.Children.Clear();
                    int idx3 = 0;
                    string lbltxt =
                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_LAST_RUN, this.UserInfo.Language) + ": " +
                        "\n% (hh:mm:ss): ";
                        //"\n% in 8H: ";
                    int totrangeSec = 0;
                    if (lstproc.Count>0)
                        totrangeSec = Convert.ToInt32 (lstproc.Max(X=>X.LastFocus).Subtract(lstproc.Min(Y=>Y.LastFocus)).TotalSeconds);

                    foreach (CRSProcessType p in lstsingleproc.OrderByDescending(X=>X.RunningSeconds).ToList())
                    {
                        int hh = Convert.ToInt32( Math.Truncate((double)p.RunningSeconds / 60 / 60));
                        int mm = Convert.ToInt32(Math.Truncate((double)(p.RunningSeconds - hh * 60 * 60) / 60));
                        int ss = Convert.ToInt32(Math.Truncate((double)(p.RunningSeconds - hh * 60 * 60 - mm*60)));
                        double perc = 0;
                        if (totrangeSec > 0) perc = ((double)p.RunningSeconds / totrangeSec) * 100;
                        CRSContainerData d = CreateAppCtrl(
                                  idx3
                                , p.ProcName
                                , p.LastFocus +
                                  " " + Math.Round(perc).ToString("#.##") + 
                                   "% (" + hh.ToString().PadLeft(2, '0') + 
                                   ":" + mm.ToString().PadLeft(2, '0') +
                                   ":" + ss.ToString().PadLeft(2, '0') +
                                   ")"
                                   //"\n" + Math.Round(((double)p.RunningSeconds / (8 * 60 * 60)) * 100).ToString("#.##") + "%"
                                , lbltxt
                                , true, p.LastFocus
                                , this.UserInfo.Language);
                        
                        DockPanel.SetDock(d,Dock.Top);
                        pnlMainRecent.Children.Add(d);
                        idx3++;
                    }
                }
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
            CRSFileUtil.TruncFile(240, this.LOG_FILE_NAME);

            return lstsingleproc;
        }

        private void TimerExp(object sender, EventArgs e)
        {
            // PID | FappProcessName | FappExePath | FappExeName | DateTime last focus
            CRSProcessType proc = CRSProcess.GetForegroudProcess_Info();
            if (proc != null)
            {
                if (this.LST_RUNNING_PROCESS.Count() > 0 && this.LST_RUNNING_PROCESS.Last().AppExePath == proc.AppExePath)
                {
                    this.LST_RUNNING_PROCESS.Last().LastFocus = DateTime.Now;
                    this.LST_RUNNING_PROCESS.Last().RunningSeconds = this.LST_RUNNING_PROCESS.Last().RunningSeconds + SECONDS_TIMER;
                }
                else
                {
                    CRSLog.WriteLog(
                        "PATH:" + proc.AppExePath + "|" +
                        "PID:" + Convert.ToString(proc.PID) + "|" +
                        "EXENAME:" + proc.AppExeName + "|" +
                        "PROCNAME:" + proc.ProcName + "|", this.UserInfo.Name, this.LOG_FILE_NAME);
                    CRSProcessType procExist = new CRSProcessType();
                    if (this.LST_RUNNING_PROCESS.Where(X => X.PID == proc.PID).Count() > 0)
                    {
                        procExist = this.LST_RUNNING_PROCESS.Where(X => X.PID == proc.PID).First();
                        procExist.RunningSeconds = procExist.RunningSeconds + SECONDS_TIMER;
                    }
                    if (procExist.PID > 0)
                        procExist.LastFocus = DateTime.Now;
                    else
                    {
                        this.LST_RUNNING_PROCESS.Add(proc);
                    }
                }

                // riordino in mnodo che l'elemnto vada lla fine della lista
                this.LST_RUNNING_PROCESS = this.LST_RUNNING_PROCESS.OrderBy(X => X.LastFocus).ToList();
            }
        }

        private CRSContainerData CreateAppCtrl(int idx, string title, string txt, string lbltxt, bool showContentTxt, DateTime dt, string lang)
        {
            List<string> lsttxt = new List<string>();
            List<string> lstlbl = new List<string>();

            if (txt.IndexOf("\n") > 0)
                lsttxt = txt.Split('\n').ToList();
            else lsttxt.Add(txt);

            if (lbltxt.IndexOf("\n") > 0)
                lstlbl = lbltxt.Split('\n').ToList();
            else lstlbl.Add(lbltxt);

            CRSContainerData c = new CRSContainerData(idx,idx, lstlbl, lsttxt, title, "", dt,"", false, showContentTxt, true, true, lang, 0, 0,0);
            c.SetEventSelect(SelectItemMenuEvent);
            c.SetEventDblClick(DblClickItemMenuEvent);
            return c;
        }

        #region EVENTS DELEGATE
        private void DblClickItemMenuEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!string.IsNullOrEmpty(GetSelectedPersonalApp().TEXT))
                OpenApp(GetSelectedPersonalApp().TEXT);
            else OpenApp(GetSelectedRecentApp().TEXT);
        }

        private void SelectItemMenuEvent()
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                //object itemtag = (sender as CRSContainerData).Tag;
                //this.idxCurrentSel = (sender as CRSContainerData).IDX;
                foreach(Control c in pnlMainRecent.Children)
                {
                    if(c is CRSContainerData)
                    {
                        (c as CRSContainerData).IS_SELECTED = false;
                    }
                }
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }
        #endregion

        private void SetTimer()
        {
            timerReadCurrProc.Tick += new EventHandler(TimerExp);
            timerReadCurrProc.Interval = new TimeSpan(0, 0, SECONDS_TIMER);
        }

        public void SetAppToIni(string filePath)
        {
            CRSIni iniFile = new CRSIni();
            iniFile.Name = CRSIniFile.IniPathName;
            if (iniFile.HasSection(APPConstants.SEC_APP_USING) == false)
            {
                iniFile.SetSection(APPConstants.SEC_APP_USING);
            }

            string filename = System.IO.Path.GetFileName(filePath).Trim(' ');
            iniFile.SetValue(APPConstants.SEC_APP_USING, filename, filePath + "||" + DateTime.Now.ToShortDateString());
        }

        private bool OpenApp(string filePath)
        {
            bool res = true;
            string lang = CRSIniFile.GetLanguageFromIni();
            try
            {
                //-- ora gestisco più docs per nota
                if (string.IsNullOrEmpty(filePath) == false)
                {
                    if (File.Exists(filePath)) Process.Start(filePath);
                    else
                    {
                        MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_FILE_NOT_FOUND, lang),
                                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, lang));
                        res = false;
                    }
                }
                return res;
            }
            catch
            {
                return false;
            }
        }

        private CRSContainerData GetSelectedPersonalApp()
        {
            CRSContainerData res = new CRSContainerData();
            foreach(Control c in pnlMainPersonal.Children)
            {
                if(c is CRSContainerData)
                {
                    //if ((c as CRSContainerData).IDX == this.idxCurrentSel)
                    if ((c as CRSContainerData).IS_SELECTED)
                        res = c as CRSContainerData;
                }
            }
            return res;
        }

        private CRSContainerData GetSelectedRecentApp()
        {
            CRSContainerData res = new CRSContainerData();
            foreach (Control c in pnlMainRecent.Children)
            {
                if (c is CRSContainerData)
                {
                    if ((c as CRSContainerData).IS_SELECTED)
                        res = c as CRSContainerData;
                }
            }
            return res;
        }

        private void AddAppFromFolder(string filename)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (!string.IsNullOrEmpty(filename))
                fd.InitialDirectory = System.IO.Path.GetDirectoryName(filename);
            if (Convert.ToBoolean(fd.ShowDialog()))
            {
                AddNewApps(fd.FileNames.ToList());
            }
        }

        private void AddNewApps(List<string> filepaths)
        {
            foreach (string file in filepaths)
            {
                SetAppToIni(file);
            }
            List<CRSProcessType> lst = FillRecentApp(dtFromCount.SelectedDateTime);
            FillPersonalApp(lst);
        }

        #region DROP files
        public void DropEvn(System.Windows.DragEventArgs e)
        {
            string lang = CRSIniFile.GetLanguageFromIni();
            //-- salva la lista dei documenti in attesa di scrittura su db, che avviene alla pressione del tasto 
            //-- di conferma           
            string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            files = files.Distinct().ToArray();

            List<string> filepaths = new List<string>();

            /*foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    //Add files from folder
                    filepaths.AddRange(Directory.GetFiles(file));
                }
                else
                {
                    //Add filepath
                    filepaths.Add(file);
                }
            }
            int fromidx = pnlMainPersonal.Children.Count + 1;
            foreach (string file in filepaths)
            {
                string filename = System.IO.Path.GetFileName(file);
                pnlMainPersonal.Children.Add(CreateAppCtrl(fromidx, filename, file, "path file: ", false, lang));
                fromidx++;
            }*/

            AddNewApps(filepaths);
        }

        private void UserControl_Drop(object sender, System.Windows.DragEventArgs e)
        {
            DropEvn(e);
        }
        #endregion

        #region EVENTS CONTROLS
        private void btnAddApp_Click(object sender, RoutedEventArgs e)
        {
            AddAppFromFolder("");
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            //bool issel = false;

        }

        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            if (this.eventHide != null)
                eventHide();
        }

        private void btnMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            if (this.eventHide != null)
                eventHide();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenApp(GetSelectedPersonalApp().TEXT);
        }

        private void btnOpenRecent_Click(object sender, RoutedEventArgs e)
        {
            OpenApp(GetSelectedRecentApp().TEXT);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

           (parent as Window).Hide();
        }

        // not used
        private void cbOrderBy_DropDownClosed(object sender, EventArgs e)
        {
            /*if (this.LIST_DOCS.Count > 0)
            {
                if (cbOrderBy.Text.Contains("1"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.DateTimeLastMod).ToList();
                else
                if (cbOrderBy.Text.Contains("2"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.DocName).ToList();
                else
                if (cbOrderBy.Text.Contains("3"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.IDNote).ToList();

                FillDocs(this.LIST_DOCS);
            }*/
        }

        #endregion

        private void tcMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tc = sender as TabControl; //The sender is a type of TabControl...

            if (tc != null)
            {
                List<CRSProcessType> lst = FillRecentApp(dtFromCount.SelectedDateTime);
                FillPersonalApp(lst);
            }
        }

        private void imgResize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //ResizeAutoHgt();
            SetDimPos(Convert.ToInt32(imgResize.Tag), 0);
            imgResize.Tag = Convert.ToInt32(imgResize.Tag) + 1;

            if (Convert.ToInt32(imgResize.Tag) == 4)
                imgResize.Tag = 0;
        }

        private void imgGoToRigth_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window win = Window.GetWindow(this.Parent);

            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            double screenWgt = System.Windows.SystemParameters.WorkArea.Width - 12;
            win.Top = 3;
            win.Height = screenHeight;
            win.Left = screenWgt - win.Width - 3;
        }

        /// <summary>
        /// obsoleta: sostituita da SetDimPos come per MBbActivity
        /// </summary>
        /// <returns></returns>
        private int ResizeAutoHgt()
        {
            Window win = Window.GetWindow(this.Parent);

            double screenHeight = System.Windows.SystemParameters.WorkArea.Height - 12;
            
            if (win.Height >= screenHeight)
            {
                //altezza di default
                win.Height = screenHeight;
                win.Top = 6;
            }
            else
            {
                double totctrlH1 = 76;
                double totctrlH2 = 76;

                foreach (Control c in pnlMainPersonal.Children)
                {
                    if (c is CRSContainerData)
                    {
                        totctrlH1 += (c as CRSContainerData).Height;
                    }
                }
                foreach (Control c in pnlMainRecent.Children)
                {
                    if (c is CRSContainerData)
                    {
                        totctrlH2 += (c as CRSContainerData).Height;
                    }
                }

                if (totctrlH2 > totctrlH1)
                    totctrlH1 = totctrlH2;

                if (totctrlH1 > screenHeight)
                    totctrlH1 = screenHeight;

                win.Height = totctrlH1;
                if (win.Top + win.Height > screenHeight)
                    win.Top = screenHeight - win.Height - 3;
               // win.Top = screenHeight - totctrlH1 - 3;
               //win.Left = screenWgt - win.Width - 3;
            }
            return Convert.ToInt32(win.Height);
        }

        private void dtFromCount_SelDateChanged(object sender, EventArgs e)
        {
            FillRecentApp(dtFromCount.SelectedDateTime);

        }
    }
}
