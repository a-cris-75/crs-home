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
using CRS.Library;
using MyBlackBoxCore.DataEntities;

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBSharingUsers.xaml
    /// </summary>
    public partial class MBBSharingUsers : UserControl
    {

        private string lang;
        private int idUser;
        private Int64 idNote;

        public delegate void EventModifyData();
        private EventModifyData eventModifyData;

        public MBBSharingUsers()
        {
            InitializeComponent();
        }

        public void Init(int iduser, Int64 idnote, EventModifyData evn)
        {
            this.idUser = iduser;
            this.idNote = idnote;
            this.lang = CRSIniFile.GetLanguageFromIni();
            this.eventModifyData = evn;
            //FillUsersSharing(idnote);
            FillUsers("",lvAllUsers);
            FillUsers("", lvAllUsers2);
            FillGroups();
        }

        private void Translate()
        {
            lblGroup.Content = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_GROUPS, this.lang);
            gbUsersLst.Header = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_USERS, this.lang);
            gbSharingUsersSel.Header =  CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_SHARE, this.lang);
        }


        #region PROPERTIRS
        public bool IsChangedList
        {
            set;
            get;
        }

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

        public bool IsBtnOpenVisible
        {
            set
            {
                if (value) btnOpen.Visibility = System.Windows.Visibility.Visible;
                else btnOpen.Visibility = System.Windows.Visibility.Collapsed;
            }
            get
            {
                if (btnOpen.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else return false;
            }
        }

        public bool ShowModeInsert
        {
            set
            {
                if (value)
                {
                    pnlGridUsers.RowDefinitions[0].Height = new GridLength(35);
                    pnlGridUsers.RowDefinitions[2].Height = new GridLength(40);
                    pnlTopUsers.Visibility = System.Windows.Visibility.Visible;
                    pnlAllUsers.Visibility = System.Windows.Visibility.Visible;
                    pnlSelectShUsers.ColumnDefinitions[0].Width = new GridLength(120, GridUnitType.Star);                   
                    btnOpen.LabelText = "<";
                }
                else
                {
                    pnlGridUsers.RowDefinitions[0].Height = new GridLength(0);
                    pnlGridUsers.RowDefinitions[2].Height = new GridLength(0);
                    pnlTopUsers.Visibility = System.Windows.Visibility.Collapsed;
                    pnlAllUsers.Visibility = System.Windows.Visibility.Collapsed;
                    pnlSelectShUsers.ColumnDefinitions[0].Width = new GridLength(0);
                    btnOpen.LabelText = "+";
                }
            }
            get
            {
                bool res = false;
                if(pnlAllUsers.Visibility == System.Windows.Visibility.Visible)
                    res = true;
                return res;
            }
        }
        #endregion

        #region METHODS FILL
        public int FillUsersSharing(Int64 idnote)
        {
            //-- lo user è inizializzatoalmomento dell'istenziazione
            this.idNote = idnote;
            List<string> l = DbDataAccess.GetSharedUsers(this.idNote,this.idUser);
            FillListWithItems(l, lvUsersShared,false, false);
            textBlockNumItems.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_USERS, this.lang) + ": " + l.Count.ToString();
            return l.Count;
        }

        public int FillUsersSharing(List<string> l, Int64 idnote)
        {
            //-- lo user è inizializzatoalmomento dell'istenziazione
            this.idNote = idnote;            
            FillListWithItems(l, lvUsersShared, false, false);
            textBlockNumItems.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_USERS, this.lang) + ": " + l.Count.ToString();
            return l.Count;
        }

        private int FillUsers(string group, ListView lv)
        {
            List<string> l = DbDataAccess.GetUsers(group, this.idUser);
            FillListWithItems(l, lv, false, false);
            return l.Count;
        }

        private int FillGroups()
        {
            List<string> l = DbDataAccess.GetGroups();
            
            cmbGrp.ItemsSource = l;
            cmbGrp2.ItemsSource = l;
            return l.Count;
        }
    
        private void FillListWithItems(List<string> lst, ListView lv, bool inAdding, bool withChk)
        {
            if(!inAdding)lv.Items.Clear();
            if (lst != null && lst.Count > 0)
            {
                foreach (string d in lst)
                {
                    CRSListViewItemIco itm = new CRSListViewItemIco(d, false, withChk);
                    //itm.MouseDown += CheckItemEvent;
                    itm.HeaderText = d;
                    itm.Tag = d;
                    lv.Items.Add(itm);
                }
            }
        }

        #endregion

        #region METHODS SAVE

        /// <summary>
        /// Salva le condivisioni in base alla lista lvUsersShared
        /// </summary>
        public List<string> GetSharing()
        {
            try
            {
                List<string>users = new List<string>();
                foreach (CRSListViewItemIco n in lvUsersShared.Items)      
                {
                    users.Add(Convert.ToString(n.Tag));
                }

                return users;
            }
            catch(Exception ex){
                CRSLog.WriteLog("Error GetSharing: " + ex.Message, "MBB");
                return null;
            }
        }

        private void CheckItemEvent(object sender, RoutedEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            object itemtag = (sender as CRSListViewItemIco).Tag;
            //SaveSharing();
            Mouse.SetCursor(Cursors.Arrow);
        }
        #endregion

        #region EVENTS
        private void imgExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideWindow();
        }

        private void cmbGrp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillUsers(Convert.ToString(cmbGrp.SelectedItem),lvAllUsers);
            //pnlUsersGroup.Header =  cmbGrp2.Text;                           
        }

        private void btnSaveGrp_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            foreach (string s in (cmbGrp.ItemsSource as List<string>))
                l.Add(s);

            string grp = Convert.ToString(txtGrpName.GetControlValue());
            if (l.Contains(grp) == false)
            {
                l.Add(grp);
            }

            cmbGrp.ItemsSource = l;
            cmbGrp2.ItemsSource = l;
            cmbGrp2.Text = Convert.ToString(txtGrpName.GetControlValue());
            FillUsers(cmbGrp2.Text, lvUsersGroup);
        }

        private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>(); //(lvAllUsers.ItemsSource as List<string>);
            string user = Convert.ToString(txtUserName.GetControlValue());
            List<string> lexists = DbDataAccess.GetSharedUsers(this.idNote, this.idUser);
            l.Add(user);
            l.AddRange(lexists);
            DbDataUpdate.InsertNewUser(user, CRSGlobalParams.NEW_USER_PWD);
            FillUsers("",lvAllUsers);

            bool res = DbDataUpdate.InsertNoteSharing(l.Distinct().ToList(), this.idNote, this.idUser);
            FillUsersSharing(this.idNote);
            this.IsChangedList = true;
            if (eventModifyData != null)
                this.eventModifyData();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // 1) pnlGridUsers: mostra row 0 con intestazione per inserire nuovo utente
            // 2) pnlAllUsers: visible
            //pnlGridUsers.RowDefinitions[0].Height = new GridLength(80);
            //pnlAllUsers.Visibility = System.Windows.Visibility.Visible;
            this.ShowModeInsert = !this.ShowModeInsert; //true;
            if (this.ShowModeInsert)
            {
                FillUsers("", lvAllUsers);
                FillUsers("", lvAllUsers2);
                FillGroups();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            this.ShowModeInsert = false;
        }

        private void btnDelGrp_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show(
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_USERS_GROUP, this.lang),
                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.lang), MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                bool res = DbDataUpdate.DeleteUsersGroup(cmbGrp2.Text);
            }
        }
        #endregion

        private void CloseWindow()
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent!=null && parent is Window)
                (parent as Window).Close();
        }

        private void HideWindow()
        {
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null && parent is Window)
                (parent as Window).Hide();
        }

        private void btnMoveSelDx_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            foreach (CRSListViewItemIco n in lvUsersShared.Items)
            {
                l.Add(n.HeaderText);
            }
            foreach (CRSListViewItemIco n in lvAllUsers.SelectedItems)
            {
                l.Add(n.HeaderText);
            }

            FillListWithItems(l, lvUsersShared, true, false);

            bool res = DbDataUpdate.InsertNoteSharing(l, this.idNote, this.idUser);
            FillUsersSharing(this.idNote);
            this.IsChangedList = true;
            if (eventModifyData != null)
                this.eventModifyData();
        }

        private void btnMoveSelSx_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            List<string> ldel = new List<string>();

            foreach (CRSListViewItemIco n in lvUsersShared.SelectedItems)
            {
                ldel.Add(n.HeaderText);
            }
            foreach (CRSListViewItemIco n in lvUsersShared.Items)
            {
                l.Add(n.HeaderText);
            }

            foreach (string s in ldel)
                l.Remove(s);

            bool res = DbDataUpdate.InsertNoteSharing(l, this.idNote, this.idUser);
            FillUsersSharing(this.idNote);
            this.IsChangedList = true;
            if (eventModifyData != null)
                this.eventModifyData();
        }

        private void btnMoveAllDx_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            foreach (CRSListViewItemIco n in lvUsersShared.Items)
            {
                l.Add(n.HeaderText);
            }
            foreach (CRSListViewItemIco n in lvAllUsers.Items)
            {
                l.Add(n.HeaderText);
            }

            FillListWithItems(l, lvUsersShared, true, false);

            bool res = DbDataUpdate.InsertNoteSharing(l, this.idNote, this.idUser);
            FillUsersSharing(this.idNote);
            this.IsChangedList = true;
            if (eventModifyData != null)
                this.eventModifyData();
        }

        private void btnMoveAllSx_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            List<string> ldel = new List<string>();
            foreach (CRSListViewItemIco n in lvUsersShared.Items)
            {
                ldel.Add(n.HeaderText);
            }
            foreach (CRSListViewItemIco n in lvAllUsers.Items)
            {
                l.Add(n.HeaderText);
            }

            foreach (string s in ldel)
                l.Remove(s);

            bool res = DbDataUpdate.InsertNoteSharing(l, this.idNote, this.idUser);
            FillUsersSharing(this.idNote);
            this.IsChangedList = true;
            if (eventModifyData != null)
                this.eventModifyData();
        }

        private void btnMoveDxGrp_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            foreach (CRSListViewItemIco n in lvUsersGroup.Items)
            {
                l.Add(n.HeaderText);
            }

            foreach (CRSListViewItemIco n in lvAllUsers2.SelectedItems)
            {
                l.Add(n.HeaderText);
                DbDataUpdate.InsertUserGroup(n.HeaderText, cmbGrp2.Text);
            }

            FillUsers(cmbGrp2.Text, lvUsersGroup);
        }

        private void btnMoveSxGrp_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            foreach (CRSListViewItemIco n in lvUsersGroup.SelectedItems)
            {
                bool res = DbDataUpdate.DeleteUserGroup(Convert.ToString(n.Tag), cmbGrp2.Text);
            }

            FillUsers(cmbGrp2.Text, lvUsersGroup);
        }

        private void chkSelUsersGrp_Click(object sender, RoutedEventArgs e)
        {
            cmbGrp.IsEnabled = chkSelUsersGrp.IsChecked.Value;
            FillGroups();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void cmbGrp2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillUsers(Convert.ToString(cmbGrp2.SelectedItem), lvUsersGroup);
            pnlUsersGroup.Header = Convert.ToString(cmbGrp2.SelectedItem);
        }
    }
}
