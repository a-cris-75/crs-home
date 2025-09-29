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
using System.Collections.ObjectModel;
using MyBlackBoxCore.DataEntities;
using CRS.Library;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBSyncro.xaml
    /// </summary>
    public partial class MBBSyncro : UserControl
    {

        public MBBSyncro()
        {
            InitializeComponent();
        }

        public string Lang{set; get;}

        public void FillGrid(int iduser)
        {
            List<DataEntities.DbNoteDoc> l = DbDataAccess.GetDocsUser(iduser);
            ObservableCollection<DataEntities.DbNoteDoc> oc = new ObservableCollection<DataEntities.DbNoteDoc>(l);
            dgPendings.ItemsSource = oc;
        }

        public void FillDocsSyncro(List<MBBSyncroPending> pendings)
        {
            try
            {
                List<DbNoteDocSyncro> lst = new List<DbNoteDocSyncro>();
                foreach (MBBSyncroPending p in pendings)
                {
                    DbNoteDocSyncro d = new DbNoteDocSyncro();

                    if (p.DocLocal != null)
                    {
                        d.DateTimeLastMod = p.DocLocal.DateTimeLastMod;
                        d.DocName = p.DocLocal.DocName;
                        d.FileNameLocal = p.DocLocal.FileNameLocal;
                        d.InternalDirRemote = p.DocLocal.InternalDirRemote;
                        d.Version = p.DocLocal.Version;
                    }

                    if (p.DocRemote != null)
                    {
                        if (d.DateTimeLastMod < p.DocRemote.DateTimeLastMod)
                            d.DateTimeLastMod = p.DocRemote.DateTimeLastMod;
                        if (string.IsNullOrEmpty(d.DocName))
                            d.DocName = p.DocRemote.DocName;
                        if (string.IsNullOrEmpty(d.FileNameLocal))
                            d.FileNameLocal = p.DocRemote.FileNameLocal;

                        if (string.IsNullOrEmpty(d.InternalDirRemote))
                            d.InternalDirRemote = p.DocRemote.InternalDirRemote;

                        if (d.Version <= 0)
                            d.Version = p.DocRemote.Version;
                    }

                    d.IsPermitted = p.IsPermitted;
                    d.Message = p.Message;
                    d.UploadOrDownload = p.UploadOrDownload;
                    d.UserNoteOwner = p.UserOwner;
                    d.DateTimeSyncro = p.DateTimeSyncro;
                    lst.Add(d);
                }
                //ObservableCollection<MBBSyncroPending> oc = new ObservableCollection<MBBSyncroPending>(pendings);
                ObservableCollection<DbNoteDocSyncro> oc = new ObservableCollection<DbNoteDocSyncro>(lst);
                dgPendings.ItemsSource = oc;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error Syncro: " + ex.Message, "MBBSyncro");
            }
        }

        public void FillDocsSyncro(List<DbNoteDocSyncro> syncrodocs)
        {
            ObservableCollection<DbNoteDocSyncro> oc = new ObservableCollection<DbNoteDocSyncro>(syncrodocs);
            dgPendings.ItemsSource = oc;
        }

        private void dgPendings_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string field = e.Column.Header.ToString();
            if (field == "IDNote" ||
                field == "IDUser" ||
                field == "IDUserOwner" ||
                field == "IDUserNoteOwner" ||
                field == "UserMod" ||
                field == "FileNameRemote")
                e.Column.Visibility = System.Windows.Visibility.Collapsed;

            if (field == "DocName")
                e.Column.Header = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_DOC, this.Lang);

            if (field == "Version")
                e.Column.Header = "Ver.";

            if (field == "DateTimeLastMod")
                e.Column.Header = "Mod";

            //if (field == "FileNameRemote")
            //    e.Column.Header = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILE_NAME, this.Lang);

            if (field == "FileNameLocal")
                e.Column.Header = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILE_NAME, this.Lang);

            if (field == "Message")
                e.Column.Header = "Msg.";

            if (field == "UploadOrDownload")
                e.Column.Header = "Uplaod/Download";

            if (field == "IsPermitted")
                e.Column.Header = "OK";
        }

    }
}
