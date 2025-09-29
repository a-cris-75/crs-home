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

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBTitle.xaml
    /// </summary>
    public partial class MBBTitle : UserControl
    {
        private string lang;
        List<string> titles = new List<string>();
        public delegate void EventSelect(string val);
        private EventSelect eventSelect;

        public MBBTitle()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.lang = CRSIniFile.GetLanguageFromIni();
            this.Translate();
            txtSepChar.Text = MBBCommon.SeparationCharTitle;
        }

        private void Translate()
        {
            chkAtLeastOneWord.Content = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_LOOK_AND_WORDS, this.lang);
            lblSepChar.Content = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_SEPARATOR, this.lang);
        }

        public string GetFilter()
        {
            return txtTitle.Text;
        }

        public void SetEventSelect(EventSelect evn)
        {
            this.eventSelect = evn;
        }

        public bool TypeFindAtLeastOne
        {
            set
            {
                chkAtLeastOneWord.IsChecked = value;
            }
            get
            {
                return chkAtLeastOneWord.IsChecked.Value;
            }
        }

        public bool IsHeaderVisible
        {
            set
            {
                if (value)
                {
                    pnlMain.RowDefinitions[0].Height = new GridLength(32);
                    txtTitle.Visibility = System.Windows.Visibility.Visible;
                    txtTitle.IsTabStop = true;
                }
                else
                {
                    pnlMain.RowDefinitions[0].Height = new GridLength(0);
                    txtTitle.Visibility = System.Windows.Visibility.Hidden;
                    txtTitle.IsTabStop = false;
                }
            }
            get
            {
                if (pnlMain.RowDefinitions[0].Height.Value == 0)
                    return false;
                else return true;
            }
        }

        public void FillTitle(int iduser)
        {
            try
            {
                titles = DbDataAccess.GetTitles(iduser);
                lvTitles.ItemsSource = titles;
            }
            catch(Exception ex)
            {
                CRSLog.WriteLog("Error FillTitle: " + ex.Message, "");
            }
        }

        public void FillFavoritesTitle(int iduser)
        {
            try
            {
                titles = DbDataAccess.GetFavoritesTitles(iduser);
                lvTitles.ItemsSource = titles;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error FillFavoritesTitle: " + ex.Message, "");
            }
        }


        public void FillExtTitle(List<string> exttitles)
        {
            try
            {
                titles = exttitles.ToList();
                lvTitles.ItemsSource = titles;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Error FillExtTitle: " + ex.Message, "");
            }
        }

        public void ApplyFilter(string filter)
        {
            List<string> lst = new List<string>();
            List<string> newlst = new List<string>();
            List<string> tmplst = new List<string>();
            foreach (string s in this.titles)
                lst.Add(s);

            if (TypeFindAtLeastOne)
            {
                List<string> delimiterChars = new List<string>(' ');
                delimiterChars.Add( txtSepChar.Text );
                List<string> l = filter.Split(delimiterChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                
                // tutti i titoli che contengono almeno una parola del nuovo titolo
                foreach (string s in l)
                    newlst.AddRange(lst.Where(X => X.ToLower().Contains(s.ToLower())).ToList());

                newlst = newlst.Distinct().ToList();
            }
            else
            {
                //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                //List<string> l = filter.Split(delimiterChars).ToList();
                List<string> delimiterChars = new List<string>(' ');
                delimiterChars.Add(txtSepChar.Text);
                List<string> l = filter.Split(delimiterChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                
                // tutti i titoli che contengono tutte le parole del nuovo titolo
                foreach (string s in l)
                {
                    // memorizzo tutti i titoli che non contengono le parole del filtro
                    tmplst = lst.Where(X => X.ToLower().Contains(s.ToLower())==false).ToList();
                    foreach (string ss in tmplst)
                        lst.Remove(ss);
                }
                newlst.AddRange(lst);
            }

            lvTitles.ItemsSource = newlst;
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter(txtTitle.Text);
        }

        private void lvTitles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //-- set wait cursor
            //-- crea problemi dato che la selezione scrive su title di mbbcore che a sua volta scatena evento di scrittura su filtro
            //-- di questo controllo
           /*Mouse.SetCursor(Cursors.Wait);
           if (this.eventSelect != null && lvTitles.SelectedItems.Count>0) 
               this.eventSelect(Convert.ToString(lvTitles.SelectedItems[0]));
            //-- reset cursor
            Mouse.SetCursor(Cursors.Arrow);  */
        }

        private void lvTitles_KeyDown(object sender, KeyEventArgs e)
        {
            //-- set wait cursor
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                if (this.eventSelect != null && lvTitles.SelectedItems.Count > 0)
                    this.eventSelect(Convert.ToString(lvTitles.SelectedItems[0]));
            }
            catch { }
            //-- reset cursor
            Mouse.SetCursor(Cursors.Arrow);  
        }

        private void lvTitles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //-- set wait cursor
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                if (this.eventSelect != null && lvTitles.SelectedItems.Count > 0)
                    this.eventSelect(Convert.ToString(lvTitles.SelectedItems[0]));
            }
            catch { }
            //-- reset cursor
            Mouse.SetCursor(Cursors.Arrow);  
        }
    }
}
