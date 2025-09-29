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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyBlackBoxCore.DataEntities;
using System.IO;
using System.Diagnostics;
using CRS.Library;
using CRS.WPFControlsLib;
using Microsoft.Win32;
//using System.Windows.Forms.;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBDocs.xaml
    /// </summary>
    public partial class MBBDocs : UserControl
    {
        //private string lang;
        //private int idUser;
        public MBBUserInfo UserInfo = null;
        
        //private MBBSyncroParams syncroParams = new MBBSyncroParams();
        public delegate void EventRefreshData();
        private EventRefreshData eventRefreshData;

        private DbNote currentNote;
        private List<DbNoteDoc> LIST_DOCS = new List<DbNoteDoc>();
        private int type_view = 0;
        public int TYPE_VIEW
        {
            set { this.type_view = value; }
            get { return type_view; }
        }

        public MBBDocs()
        {
            InitializeComponent();
        }

        public void Init(int iduser)
        {
            //this.lang = CRSIniFile.GetLanguageFromIni();
            //this.idUser = iduser;
            this.UserInfo = new MBBUserInfo();
            lvDocs.Items.Clear();
            this.Translate();
            InitSyncroParams();
        }

        private void Translate()
        {
            //
        }

        private void InitSyncroParams()
        {
            try
            {
                MBBCommon.syncroParams.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                MBBCommon.syncroParams.LocalTempDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_LOCALTMPDIR);

                if (string.IsNullOrEmpty(MBBCommon.syncroParams.LocalTempDir))
                {
                    MBBCommon.syncroParams.LocalTempDir = AppDomain.CurrentDomain.BaseDirectory + "Docs";
                    CRSIniFile.SetPropertyValue(APPConstants.SEC_MYBLACKBOX, APPConstants.PROP_LOCALTMPDIR, MBBCommon.syncroParams.LocalTempDir);
                    if (Directory.Exists(MBBCommon.syncroParams.LocalTempDir) == false)
                        Directory.CreateDirectory(MBBCommon.syncroParams.LocalTempDir);
                }
                MBBCommon.syncroParams.AutoSync = false;
                string s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSYNC);
                if (string.IsNullOrEmpty(s) == false && (s=="1" || s.ToLower() == "true"))
                    MBBCommon.syncroParams.AutoSync = true;
                MBBCommon.syncroParams.AutoSave = true;
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_AUTOSAVE);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    MBBCommon.syncroParams.AutoSave = true;
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_ISUNIQUEFILENAME, 1);
                if (string.IsNullOrEmpty(s) == false && (s == "1" || s.ToLower() == "true"))
                    MBBCommon.syncroParams.IsUniqueFileName = true;               
                s = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_OPNOOVERWRITEOWNER, 1);
                if (string.IsNullOrEmpty(s) == false && (s=="1" || s.ToLower() == "true"))
                    MBBCommon.syncroParams.SyncroOnlyOwner = true;
            }
            catch{}
        }

        #region PROPERTIRS
        public bool IsBtnExitVisible
        {
            set
            {
                if (value) imgExit.Visibility = System.Windows.Visibility.Visible;
                else imgExit.Visibility = System.Windows.Visibility.Collapsed;
            }
            get
            {
                if (imgExit.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else return false;
            }
        }

        public bool IsBtnDelVisible
        {
            set
            {
                if (value) btnDel.Visibility = System.Windows.Visibility.Visible;
                else btnDel.Visibility = System.Windows.Visibility.Collapsed;
            }
            get
            {
                if (btnDel.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else return false;
            }
        }
        #endregion

        #region METHODS
        public int FillDocs(long idnote, int iduser)
        {
            this.currentNote = DbDataAccess.GetNote(iduser, idnote);
            this.LIST_DOCS = DbDataAccess.GetDocsNote(idnote,iduser);
            FillListWithItems(this.LIST_DOCS);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) +": " + lvDocs.Items.Count.ToString();
            return this.LIST_DOCS.Count;
        }

        public int FillDocs(int iduser)
        {
            this.LIST_DOCS = DbDataAccess.GetDocsUser(iduser);
            List<DbNoteDoc> docs = ApplyFilter();
            FillListWithItems(docs);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": " + lvDocs.Items.Count.ToString();
            return docs.Count;
        }

        public int FillDocs(int iduser, DateTime dt1, DateTime dt2)
        {
            this.LIST_DOCS = DbDataAccess.GetDocsFromNotes(iduser, dt1,dt2, 0, "", "", false, false);
            List<DbNoteDoc> docs = ApplyFilterText(txtFind.Text);
            FillListWithItems(docs);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": " + lvDocs.Items.Count.ToString();
            return docs.Count;
        }

        public int FillDocs(MBBFindParams p)
        {
            this.LIST_DOCS = DbDataAccess.GetDocsFromNotes(p.IDUser,p.Dt1,p.Dt2,p.Rate,p.SimbolRate,p.Title,p.TypeFindAtLeastOne, p.OnlyFavorites);
            FillListWithItems(this.LIST_DOCS);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_TITLE, this.UserInfo.Language) + ": " + p.Title + "    " +
                                    CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": "+ lvDocs.Items.Count.ToString();
            return this.LIST_DOCS.Count;
        }

        public int FillDocs(List<DbNoteDoc> l)
        {
            this.LIST_DOCS = l;
            FillListWithItems(l);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": " + lvDocs.Items.Count.ToString();
            if (l != null && l.Count > 0)
                return l.Count;
            else return 0;
        }
        
        private void FillListWithItems(List<DbNoteDoc> lst)
        {
            lvDocs.Items.Clear();
            // 0: tipo di vista utilizzando il controllo CRSListViewItemDoc
            // 1: tipo di vista utilizzando il controllo CRSContainerData
            if (this.TYPE_VIEW == 0)
            {
                if (lst != null && lst.Count > 0)
                {
                    int idx = 0;
                    foreach (DbNoteDoc d in lst)
                    {
                        string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, d.InternalDirRemote);
                        string filenameremote = System.IO.Path.Combine(intDirRemote, d.DocName);
                        CRSListViewItemDoc itm = new CRSListViewItemDoc(d.DateTimeLastMod, d.FileNameLocal, filenameremote, d.DocName, this.UserInfo.Language, true, idx, lvDocs.Width);

                        itm.Tag = d;
                        itm.SetDblClickEvent(DblClickItemMenuEvent);
                        lvDocs.Items.Add(itm);
                        idx++;
                    }
                }
            }
            else
            {
                if (lst != null && lst.Count > 0)
                {
                    int idx = 0;
                    foreach (DbNoteDoc d in lst)
                    {
                        string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, d.InternalDirRemote);
                        string filenameremote = System.IO.Path.Combine(intDirRemote, d.DocName);
                        string txt = d.FileNameLocal + "\n" + filenameremote;
                        string lbltxt = CRSTranslation.TranslateDialog(CRSTranslation.WRD_DOC, this.UserInfo.Language) + ": ";

                        List<string> lstlbl = new List<string>();
                        List<string> lsttxt = new List<string>();
                        lstlbl.Add(lbltxt);
                        lsttxt.Add(txt);

                        CRSContainerData itm = new CRSContainerData(idx, idx, lstlbl, lsttxt, d.DocName, "", d.DateTimeLastMod,"", true, false, true, true, this.UserInfo.Language, 0, 0, 0);

                        itm.Tag = d;
                        itm.SetEventDblClick(DblClickItemMenuEvent);
                        lvDocs.Items.Add(itm);

                        idx++;
                    }
                }
            }
        }

        private bool OpenDoc(DbNoteDoc doc, int openLocal0Remote1)
        {
            bool res = true;
            string intDirRemote = ""; 
                
            try
            {
                res = MBBCommon.OpenDoc(doc, openLocal0Remote1, this.UserInfo.Language);
            }
            catch
            {
                AddDocsFromFolder(intDirRemote);
                return false;
            }
            return res;
        }

        private void AddDocsFromFolder(string filename)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if(!string.IsNullOrEmpty(filename))
                fd.InitialDirectory = System.IO.Path.GetDirectoryName(filename);
            if (Convert.ToBoolean(fd.ShowDialog()))
            {
                AddNewDocs(fd.FileNames.ToList());

            }

        }

        private void AddNewDocs(List<string> filepaths)
        {
            foreach (string file in filepaths)
            {
                bool found = false;
                foreach (DbNoteDoc d in this.currentNote.Docs)
                {
                    if (d.FileNameLocal == file)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    DbNoteDoc doc = new DbNoteDoc();

                    doc.DateTimeLastMod = System.IO.File.GetLastWriteTime(file);
                    doc.DocName = fileName;
                    doc.FileNameLocal = file;
                    //-- verrà inizializzata successivamente
                    doc.IDNote = this.currentNote.ID;
                    doc.IDUser = this.currentNote.IDUser;
                    doc.UserMod = this.UserInfo.Name;
                    doc.UserNoteOwner = this.UserInfo.Name;
                    doc.IsNewFile = false;

                    this.currentNote.Docs.Add(doc);
                }
            }
            this.FillDocs(this.currentNote.Docs);
        }


        public void SetFocusDocs(Int64 idnote, int iduser)
        {
            try
            {
                foreach (CRSListViewItemDoc d in lvDocs.Items)
                {
                    if (d.Tag!=null) {
                        DbNoteDoc dd = (DbNoteDoc)d.Tag;
                        if(dd.IDNote == idnote && dd.IDUser==iduser){
                            d.IsFocusSelected = true;
                        }
                        else d.IsFocusSelected = false;
                    }
                }
            }
            catch { }
        }

        public void SetEvnRefresh(EventRefreshData evn)
        {
            this.eventRefreshData = evn;
        }
        #endregion

        #region EVENTS DELEGATE
        private void DblClickItemMenuEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvDocs.SelectedItems.Count > 0)
            {
                DbNoteDoc d = (lvDocs.SelectedItems[0] as CRSListViewItemDoc).Tag as DbNoteDoc;
                OpenDoc(d,1);
            }
        }
        #endregion

        #region EVENTS
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            (parent as Window).Hide();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (lvDocs.SelectedItems.Count > 0)
            {
                if (TYPE_VIEW == 0)
                {                
                    foreach (CRSListViewItemDoc n in lvDocs.Items)
                    {
                        if (n.IsChkSelected)
                        {
                            DbNoteDoc d = n.Tag as DbNoteDoc;
                            OpenDoc(d, 1);
                        }
                    }
                }
                else
                {
                    foreach (CRSContainerData n in lvDocs.Items)
                    {
                        if (n.IS_CHK_SELECTED)
                        {
                            DbNoteDoc d = n.Tag as DbNoteDoc;
                            OpenDoc(d, 1);
                        }
                    }
                }
            }
        }

        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            AddDocsFromFolder("");
            /*if (lvDocs.SelectedItems.Count > 0)
            {
                foreach (ListViewItem n in lvDocs.Items)
                {
                    DbNoteDoc d = n.Tag as DbNoteDoc;
                    bool isSel = false;
                    if (n is CRSListViewItemDoc && (n as CRSListViewItemDoc).IsChkSelected) isSel = true;
                    if (isSel)
                    {
                        string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, d.InternalDirRemote);
                        string filePath = System.IO.Path.Combine(intDirRemote, d.DocName);
                        AddDocsFromFolder(filePath);
                    }
                } 
            }*/
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
             bool issel = false;
            foreach (CRSListViewItemDoc n in lvDocs.Items)
            {
                if (n.IsChkSelected)
                {
                    issel = true;
                    break;
                }               
            }
            if (issel)
            {
               
                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_ITEMS, this.UserInfo.Language),
                                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    List<DbNoteDoc> lstdocs = new List<DbNoteDoc>();
                    List<Int64> lstnotes = new List<Int64>();
                    int i = 0;
                    //-- normalmente i docs appartengono ad una sola nota 
                    foreach (CRSListViewItemDoc n in lvDocs.Items)
                    {
                        DbNoteDoc d = n.Tag as DbNoteDoc;
                        if (n.IsChkSelected)
                        {
                            bool res = DbDataUpdate.DeleteDocNote(d.IDNote, d.IDUser, d.DocName);
                        }
                        else
                        {
                            lstdocs.Add(d);
                            if (lstnotes.Contains(d.IDNote) == false) lstnotes.Add(d.IDNote);
                            i++;
                        }
                    }
                    FillDocs(lstdocs);
                    string user = DbDataAccess.GetUser(this.UserInfo.IDUser);
                    DateTime dtFill = DateTime.Now;
                    
                    //-- scrivo anche nei file degli utenti che condividono la nota
                    //-- ATTENZIONE: se la nota è condivisa in questo momento, potrebbe non essere stata inserita nei file
                    //-- degli utenti che condividono
                    foreach (int n in lstnotes)
                    {
                        List<string> users = DbDataAccess.GetSharedUsers(n, this.UserInfo.IDUser);
                        List<DbNoteDoc> lstdocsnote = lstdocs.Where(X => X.IDNote == n).ToList();

                        //MBBSyncroUtility.FillDocsSyncroDBFile(user, user, lt, this.syncroParams.RemoteDir);
                        MBBSyncroUtility.WriteDocsSyncroFile(n, dtFill, user, users, lstdocs,MBBCommon.syncroParams);
                        MBBSyncroUtility.WriteDocsSyncroFileShared(n, dtFill, user, lstdocsnote, users, MBBCommon.syncroParams);                     
                    }
                            
                    //-- dato che la cancellazione coinvolge MBBCore (in cui sono caricati i dati)
                    //-- devo laciare l'evento di refresh per poter aggiornare anche la lista in MBBCore
                    if (this.eventRefreshData != null)
                        this.eventRefreshData();

                }              
            }
        }

        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            (parent as Window).Hide();
        }

        private void chkSelAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CRSListViewItemDoc n in lvDocs.Items)
            {
                if (chkSelAll.IsChecked.Value)
                    n.IsChkSelected = true;
                else n.IsChkSelected = false;
            }
        }
       
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }
        #endregion

        private void btnSyncroLocal_Click(object sender, RoutedEventArgs e)
        {
            //if (lvDocs.SelectedItems.Count > 0)
            {
                foreach(CRSListViewItemDoc d in lvDocs.Items)
                {
                    if (d.IsChkSelected)
                    {
                        if (d.IsDifferentFiles())
                        {
                            MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_SYNCRONIZE_FILES, this.UserInfo.Language) +
                                "\n - " + d.GetNameFileOrigin() +
                                "\n - " + d.GetNameFileMBB(),
                                        CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.UserInfo.Language), MessageBoxButton.YesNo);
                            if (r == MessageBoxResult.Yes)
                                MBBSyncroUtility.SyncroFilesLocal(d.GetNameFileOrigin(), d.GetNameFileMBB());
                        }
                    }
                }

                List<DbNoteDoc> lt = new List<DbNoteDoc>();
                //-- normalmente i docs appartengono ad una sola nota 
                foreach (CRSListViewItemDoc n in lvDocs.Items)
                {
                    DbNoteDoc d = n.Tag as DbNoteDoc;
                    lt.Add(d);
                }
                FillDocs(lt);

                if (this.eventRefreshData != null)
                    this.eventRefreshData();
            }
        }

        private void cbOrderBy_DropDownClosed(object sender, EventArgs e)
        {
            if (this.LIST_DOCS.Count > 0) {
                if (cbOrderBy.Text.Contains("1"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.DateTimeLastMod).ToList();
                else
                if (cbOrderBy.Text.Contains("2"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderByDescending(X => X.DateTimeLastMod).ToList();
                else
                if (cbOrderBy.Text.Contains("3"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.DocName).ToList();
                else
                if (cbOrderBy.Text.Contains("4"))
                    this.LIST_DOCS = this.LIST_DOCS.OrderBy(X => X.IDNote).ToList();

                FillDocs(this.LIST_DOCS);
            }
        }

        private void txtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterText(txtFind.Text);
            //ApplyFilter();
        }

        public List<DbNoteDoc> ApplyFilter()
        {
            List<DbNoteDoc> lst1 = ApplyFilterText(txtFind.Text);
            List<DbNoteDoc> lst2 = ApplyFilterDate(dtFilter1.SelectedDate,dtFilter2.SelectedDate);
            lst1.AddRange(lst2);
            return lst1;
        }

         public List<DbNoteDoc> ApplyFilterText(string filterText)
        {
            List<DbNoteDoc> lst = new List<DbNoteDoc>();
            List<string> filt = new List<string>();
            lst.AddRange(this.LIST_DOCS);

            filt = filterText.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string s in filt)
            {
                lst = lst.Where(X => X.DocName.Contains(s)).ToList();
            }
            FillListWithItems(lst);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": " + lvDocs.Items.Count.ToString();
            return lst;
        }

        public List<DbNoteDoc> ApplyFilterDate(DateTime? dt1,DateTime? dt2)
        {
            List<DbNoteDoc> lst = new List<DbNoteDoc>();
            List<string> filt = new List<string>();
            lst.AddRange(this.LIST_DOCS);

            if (dt1 > DateTime.MinValue && dt2 > DateTime.MinValue)
                lst = lst.Where(X => X.DateTimeLastMod >= dt1 && X.DateTimeLastMod<= dt2).ToList();
            else
            if (dt1 > DateTime.MinValue)
                lst = lst.Where(X => X.DateTimeLastMod >= dt1 ).ToList();
            else
            if( dt2 > DateTime.MinValue)
                lst = lst.Where(X => X.DateTimeLastMod <= dt2).ToList();

            FillListWithItems(lst);
            textBlockNumDocs.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILES, this.UserInfo.Language) + ": " + lvDocs.Items.Count.ToString();
            return lst;
        }

        private void textBlockNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(textBlockNote.Background == Brushes.Lime)
            {
                FillDocs(this.UserInfo.IDUser);
                textBlockNote.Background = Brushes.Transparent;
            }
            else
            {
                if (this.currentNote!=null && this.currentNote.ID > 0)
                {
                    FillDocs(this.currentNote.ID, this.UserInfo.IDUser);
                    textBlockNote.Background = Brushes.Lime;
                }
            }
        }

        private void dtFilter1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //ApplyFilter();
        }

        private void dtFilter2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //ApplyFilter();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            FillDocs(this.UserInfo.IDUser, dtFilter1.DisplayDate, dtFilter2.DisplayDate);
        }

        private void btnShowFilter_Click(object sender, RoutedEventArgs e)
        {
            if (pnlFilters.Visibility == Visibility.Visible)
            {
                pnlFilters.Visibility = Visibility.Collapsed;
                pnlGridMain.RowDefinitions[0].Height = new GridLength(34);
            }
            else
            {
                pnlFilters.Visibility = Visibility.Visible;
                pnlGridMain.RowDefinitions[0].Height = new GridLength(136);
            }
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            //if (lvDocs.SelectedItems.Count > 0)
            try
            {
                if (TYPE_VIEW == 0)
                {
                    foreach (CRSListViewItemDoc n in lvDocs.Items)
                    {
                        if (n.IsChkSelected)
                        {
                            OpenFileDialog fd = new OpenFileDialog();
                            DbNoteDoc dd = (DbNoteDoc)n.Tag;

                            
                            if (!string.IsNullOrEmpty(dd.FileNameLocal) && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(dd.FileNameLocal)))
                            {
                                fd.InitialDirectory = System.IO.Path.GetDirectoryName(dd.FileNameLocal);
                                fd.ShowDialog();
                            }

                            string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, dd.InternalDirRemote);
                            string filePath = System.IO.Path.Combine(intDirRemote, dd.DocName);

                            OpenFileDialog fd2 = new OpenFileDialog();
                            if (!string.IsNullOrEmpty(intDirRemote) && System.IO.Directory.Exists(intDirRemote))
                            { 
                                fd2.InitialDirectory = intDirRemote;
                                fd2.FileName = System.IO.Path.GetFileName( filePath);
                                fd2.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    foreach (CRSContainerData n in lvDocs.Items)
                    {
                        if (n.IS_CHK_SELECTED)
                        {
                            DbNoteDoc dd = n.Tag as DbNoteDoc;
                            OpenFileDialog fd = new OpenFileDialog();

                            if (!string.IsNullOrEmpty(dd.FileNameLocal) && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(dd.FileNameLocal)))
                            {
                                fd.InitialDirectory = System.IO.Path.GetDirectoryName(dd.FileNameLocal);
                                fd.ShowDialog();
                            }
                            string intDirRemote = System.IO.Path.Combine(MBBCommon.syncroParams.DropBoxRootDir, dd.InternalDirRemote);
                            string filePath = System.IO.Path.Combine(intDirRemote, dd.DocName);

                            OpenFileDialog fd2 = new OpenFileDialog();
                            if (!string.IsNullOrEmpty(intDirRemote) && System.IO.Directory.Exists(intDirRemote))
                            {
                                fd2.InitialDirectory = intDirRemote;
                                fd2.FileName = System.IO.Path.GetFileName(filePath);
                                fd2.ShowDialog();
                            }
                        }
                    }
                    
                }
            }
            catch { }
        }
    }
}
