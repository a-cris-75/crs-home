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

namespace MyBlackBoxCore
{
    /// <summary>
    /// Interaction logic for MBBNotes.xaml
    /// </summary>
    public partial class MBBNotes : UserControl
    {
        private string lang;
        private int idUser;
        List<DbNote> notes = new List<DbNote>();
        public delegate void EventSelect(Int64 idnote, int iduser);
        private EventSelect eventSelect;

        public delegate void EventOpenNotes(List<DbNote> note);
        private EventOpenNotes eventOpen;

        public delegate void EventDoubleClick(DbNote note);
        private EventDoubleClick eventDblClick;

        // mostro più note in una: successivamante posso salvare la nota unificata
        public delegate void EventMergeNotes(List<DbNote> notes);
        private EventMergeNotes eventMerge;

        //public delegate void EventMergeNotes(List<DbNote> notes);
        //private EventMergeNotes eventMregeMultiple;

        public delegate void EventRefreshData();
        private EventRefreshData eventRefreshData;

        //CRSIni FileSyncroDB_DEL;
        private MBBFindParams findParams;

        public MBBNotes()
        {
            InitializeComponent();
        }

        public void Init(int iduser, EventSelect evn)
        {
            this.lang = CRSIniFile.GetLanguageFromIni();
            this.eventSelect = evn;
            this.idUser = iduser;
            this.Translate();
        }

        private void Translate()
        {
            lblFilterText.Content = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_FILTER, this.lang);
            lblOrder.Content = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_ORDERBY, this.lang);
        }

        //-- è inizializzato da MBBCore e passa attraverso WinMBBFind
        //-- se fosse gestito interamente da WinMBBFind lo gestirei come l'evento this.eventSelect
        public void SetEvnDblClick(EventDoubleClick evn)
        {
            this.eventDblClick = evn;
        }

        public void SetEvnOpenNotes(EventOpenNotes evn)
        {
            this.eventOpen = evn;
        }

        public void SetEvnMergeNotes(EventMergeNotes evn)
        {
            this.eventMerge = evn;
        }

        public void SetEvnRefresh(EventRefreshData evn)
        {
            this.eventRefreshData = evn;
        }

        #region PROPERTIES
        public bool IsBtnExitVisible
        {
            set {
                /*if (value) imgExit.Visibility = System.Windows.Visibility.Visible;
                else imgExit.Visibility = System.Windows.Visibility.Collapsed;
                */
                
                if (value) pnlBtns.ColumnDefinitions[0].Width = new GridLength(34);
                else pnlBtns.ColumnDefinitions[0].Width = new GridLength(0);
            }
            get
            {
                if (imgExit.Visibility == System.Windows.Visibility.Visible)
                    return true;
                else return false;
            }
        }
        #endregion

        #region FILL & FILTER
        
        public void ApplyFilter(string filterTitle)
        {
            List<DbNote> lst = new List<DbNote>();

            foreach (DbNote s in this.notes)
                lst.Add(s);

            lst = lst.Where(X => string.IsNullOrEmpty(X.Title) == false && X.Title.Contains(filterTitle)).ToList();
            FillListWithItems(lst);

            textBlockNumNotes.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_TITLE, this.lang) + ": "+ filterTitle + "    "+
                                    CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.lang) + ": " + lvNotes.Items.Count.ToString();
        }

        public void ApplyFilterText(string filterText)
        {
            List<DbNote> lst = new List<DbNote>();
            List<string> filt = new List<string>();

            //foreach (DbNote s in this.notes)
            //    lst.Add(s);

            lst.AddRange(this.notes);

            //filt = filterText.Split(' ').ToList();
            filt = filterText.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string s in filt)
            {
                lst = lst.Where(X => X.Text.Contains(s)).ToList();
            }
            FillListWithItems(lst);

            textBlockNumNotes.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.lang) + ": " + lvNotes.Items.Count.ToString();
        }

        public List<DbNote> FillNotes(MBBFindParams p)
        {
            this.findParams = p;
            this.notes = DbDataAccess.GetNotesAndDocs(p.IDUser, p.Dt1, p.Dt2, p.Rate, p.SimbolRate, p.Title, p.TypeFindAtLeastOne, p.OnlyFavorites);
            this.notes = MBBCommon.MergeNotesParent(this.notes);
            List<DbNote> lstord = OrderNotes();
            FillListWithItems(lstord);
            textBlockNumNotes.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_TITLE, this.lang) + ": " + p.Title + "    " +
                                    CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.lang) + ": " + lvNotes.Items.Count.ToString();

            return this.notes;
        }

        private void FillListWithItems(List<DbNote> lst)
        {
            lvNotes.Items.Clear();
            if (lst != null && lst.Count > 0)
            {
                foreach (DbNote d in lst)
                {
                    CRSListViewItemNote itm = new CRSListViewItemNote(d.DateTimeInserted, d.Text, d.Title, this.Width);
                    itm.Tag = d;
                    itm.SetSelectEvent(SelectItemMenuEvent);
                    itm.SetDblClickEvent(DblClickItemMenuEvent);
                    lvNotes.Items.Add(itm);
                }
            }
        }

        public void FillNotesExt(List<DbNote> lst)
        {
            this.notes = lst;
            FillListWithItems(lst);
            textBlockNumNotes.Text = CRSTranslation.TranslateDefaultWords(CRSTranslation.WRD_NOTES, this.lang) + ": " + lvNotes.Items.Count.ToString();
        }
        /// <summary>
        /// Cerca la nota nella lista per cui esistono tutte le chiavi del titolo e solo quelle
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="lst"></param>
        /// <returns></returns>
        private List<DbNote> FindNotes(string keys, List<DbNote> lst)
        {
            List<string> lstkeys = keys.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();

            // elimino le note con numero di chiavi diverso
            //lst = lst.Where(X => X.Title.Split(' ').Count() == lstkeys.Count).ToList();
            lst = lst.Where(X => X.Title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Count() == lstkeys.Count).ToList();
            lst = lst.Where(X => X.Title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).Except(lstkeys).Count() == 0).ToList();

            return lst;
        }
        /*
        private List<DbNote> MergeNotesParent(List<DbNote> notes)
        {

            List<DbNote> res = new List<DbNote>();

            try
            {
                // 1) aggiungo le note per cui non esiste un padre
                res.AddRange(notes.Where(X => X.IDNoteParent == 0).ToList());

                var grp = notes.Where(X => X.IDNoteParent > 0).GroupBy(X => X.IDNoteParent);
                List<Int64> keys = grp.Select(X => X.Key).Distinct().ToList();
                foreach (Int64 k in keys)
                {
                    // la clausola X.ID = k è utile nel caso che per la nota radice l'IDNoteParent sia 0
                    List<DbNote> notesgrp = notes.Where(X => X.IDNoteParent == k || X.ID == k).ToList();
                    if (notesgrp.Count > 0)
                    {
                        DbNote newnote = new DbNote();
                        string txt = string.Join(" ", notesgrp.Select(Y => Y.Text).ToList());

                        newnote = notesgrp.First();
                        newnote.Text = txt;

                        res.Add(newnote);
                    }
                }
                res = res.OrderBy(X => X.DateTimeInserted).ToList();
            }
            catch { }
            
            return res;
        }*/
        #endregion

        #region ORDINA NOTE IN CARTELLE secondo 3 algoritmi
        
        /// <summary>
        /// Metodo ricorsivo
        /// </summary>
        /// <param name="namekey"></param>
        /// <param name="lsttitles"></param>
        /// <param name="lstnotes"></param>
        /// <param name="parentitm"></param>
        private void CreteTreeViewDirTitlesOrderByOccurrencyKeyRecursive(string namekey, List<string> lsttitles, List<DbNote> lstnotes, TreeViewItem parentitm)
        {
            tvNotes.Items.Clear();

            // FUNZIONAMENTO: 
            // voglio ottenere un albero a directory in cui la cartella più esterna è la parola chiave più usata, all'interno di questa la cartella con la parola chiave più usata dopo l aprima e così via
            // - nella directory radice avrò quindi la parola più usata e via via le cartelle che rappresenrtano le parole chiave delle note non incluse nella prima cartella
            // - in questo modo dovrei ottenere un posto per ogni nota
            //
            // MOTIVAZIONE:
            // - la parola più usata rappresenta la radice che unisce tutte le note con la stessa radice
            // - le note rimanenti subiscono lo stesso ragionamento
            // - in pochi passaggi dovrei esaurire tutte le note che una volta trovato posto nopn compaiono negli altri gruppi(per gruppo inendo un insieme di note con la stessa rdice)

            // ES di titoli di note => 
            //      .titolo 1: fisica meccanica formule
            //      .titolo 2: fisica fluidodinamica teoria 
            //      .titolo 3: fisica fluidodinamica formule
            //      .titolo 4: fisica teoria fluidodinamica TODESCHINI
            //      .titolo 5: fisica teoria fluidodinamica einstein
            //      .titolo 6: fisica fluidodinamica
            // OTTENGO
            //      .FISICA(6)
            //          .FLUIDODINAMICA(5)
            //              .TEORIA(3)
            //                  .TODESCHINI(1)
            //                  .EINSTEIN (1)
            //              .FORMULE(1)
            //          .MECCANICA(1)
            //              .FORMULE(1)
            // non basta!!: SE HO 5 ELEMENTI CON (1) SIGNIFICA CHE HO SISTEMATO TUTTE LE NOTE
            // es: titolo 6 = dopo aver costruito l'albero, scorro per ogni nodo che ha un tag = lista delle parole chiavve che copmpongono i padri,
            // e trovo le note con il title = al tag del nodo dell'albero
            // es: FISICA
            //      .FLUIDODINAMICA
            //          .TEORIA 
            //  => tag = FISICA FLUIDODINAMICA TEORIA



            // PREMESSA : in lst titles ho solo titoli univoci (usa NormalizeListTitles)
            lsttitles = MBBCommon.NormalizeListTitles(lsttitles);
            List<string> newlsttitles = lsttitles.ToList();
            // while (newlsttitles.Count > 0)
            {
                newlsttitles = lsttitles.Where(X => X == namekey).ToList();
                List<KeyValuePair<string, int>> lstkeysocc = MBBCommon.GetListDirKeysOccurrences(newlsttitles);
                foreach (KeyValuePair<string, int> k in lstkeysocc)
                {
                    // INSERISCO IL NODO CARTELLA, EVVENTUALMENTE AL NODO CARTELLA ASSOCIO LE NOTE SE ESISTONO (QUELLE CON LE CHIAVI = ALBERO DELLE CHIAVI)
                    TreeViewItem rootitm = new TreeViewItem();
                    rootitm.Header = k.Key;
                    rootitm.Tag = k.Key;
                    string titletags = k.Key;
                    if (parentitm != null)
                    {
                        parentitm.Items.Add(rootitm);
                        titletags = parentitm.Tag + MBBCommon.SeparationCharTitle + k.Key;
                        rootitm.Tag = titletags;
                    }
                    
                    List<DbNote> lsttmp = this.FindNotes(titletags, lstnotes);
                    if (lstnotes.Count() > 0)
                    {
                        // significa che ho delle note il cui titolo ha le chiavi e solo quelle in titletags
                        // AGGIUNGO LE NOITE COME NODI
                        foreach (DbNote n in lsttmp)
                        {
                            CRSListViewItemNote itm = new CRSListViewItemNote(n.DateTimeInserted, n.Text, n.Title, this.Width);
                            itm.Tag = n;
                            itm.SetSelectEvent(SelectItemMenuEvent);
                            itm.SetDblClickEvent(DblClickItemMenuEvent);
                            rootitm.Items.Add(itm);
                           
                            //List<string> tmp = n.Title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();// string.Join(MBBCommon.SeparationCharTitle, lstkeys.OrderBy(X => X).ToArray());
                            //newlsttitles.RemoveAll(X => X == string.Join(MBBCommon.SeparationCharTitle, tmp.OrderBy(Y=>Y).ToList()));
                        }
                        lstnotes.RemoveAll(Y => lsttmp.Where(X => X.ID == Y.ID).Count() > 0);
                        
                    }
                    List<string> tmp = titletags.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();// string.Join(MBBCommon.SeparationCharTitle, lstkeys.OrderBy(X => X).ToArray());
                    newlsttitles.RemoveAll(X => X == string.Join(MBBCommon.SeparationCharTitle, tmp.OrderBy(Y => Y).ToList()));

                    if (newlsttitles.Count>0 && lstnotes.Count>0)
                        CreteTreeViewDirTitlesOrderByOccurrencyKeyRecursive(namekey, newlsttitles, lstnotes, rootitm);
                }
            }

            //return lsttitlesnew;
        }
        
        /// <summary>
        /// Genera un albero treeView, in cui ogni nodo è una parola chiave.
        /// LOGICA:
        ///     1) parto da una lista di titoli in cui l'ordine delle parole è dato:
        ///         .o dalle occorrenze delle parole chiave in tutta la lista di titoli
        ///         .o dalla posizione e dalle occorrenze
        ///     2) per ogni titolo lo scompone in tutte le parole chiave
        ///     3) crea nodo con parola chiave
        ///     4) associa al nodo una nota se ne esiste una col titolo uguale alla somma delle parole chiave di tutti i padri del nodo attuale
        ///  Dovrei otteenre una serie di dir annidate e su ogni foglia la nota associata a tutti nodi precedenti 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="typecalcdir"></param>
        private void FillTreeViewWithItems(List<DbNote> lst, int typecalcdir)
        {
            // in lsttitlesnew ottengo la lista di tutti i possibili path delle note
            List<string> lsttitlesnew = lst.Select(X => X.Title).Distinct().ToList();
            List<KeyValuePair<string, int>> lstkeysocc = MBBCommon.GetListDirKeysOccurrences(lsttitlesnew);
            // per ora uso questa
            if (typecalcdir == 2) lsttitlesnew = MBBCommon.GetListDirTitlesOrderByOccurrencyKey(lsttitlesnew);
            else
            if (typecalcdir == 1)
            {
                lsttitlesnew = MBBCommon.GetListDirTitlesOrderByOccurrencyPositionKey(lsttitlesnew);
                //lsttitlesnew = GetListDirTitlesOrderByMatchSmaller(lsttitlesnew);
            }
            List<DbNote> lstnotes = this.notes.ToList();          

            try
            {
                // per ogni titolo scomponilo e crea l'albero
                foreach (string s in lsttitlesnew)
                {
                    
                    // la lista di chiavi è ordinata in base al calcolo iniziale(per occorrenze di presenza o per occorenza di posizione)
                    List<string> lstkeys = s.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (lstkeys.Count > 0)
                    {
                        // lista delle note con le stesse chiavi dell'attuali (anche in ordine diverso)
                        List<DbNote> lsttmp = this.FindNotes(s, lstnotes);

                        #region INSERISCO LA ROOT
                        TreeViewItem rootitm = new TreeViewItem();
                        string h = lstkeys.First();
                        //rootitm.Header = h;
                        rootitm.Header = lstkeysocc.Where(X => X.Key == h).First().Key + "(" + lstkeysocc.Where(X => X.Key == h).First().Value + ")";
                        rootitm.Tag = h;

                        //TreeViewItem parentitm = GetExistingItem(h, tvNotes.Items);//GetItemInTree(h, tvNotes);
                        TreeViewItem parentitm = MBBCommon.GetItemFirstLevelTree(h, tvNotes.Items);
                        if (parentitm == null)
                            tvNotes.Items.Add(rootitm);
                        else rootitm = parentitm;
                        #endregion

                        #region PER OGNI PAROLA CHIAVE COSTRUISCO I NODI
                        // per ogni parola chiave scorri l'albero finchè non trovi la foglia: ora inserisci la nuova foglia
                        foreach (string k in lstkeys)
                        {
                            // salto il primo giro dato che sono sulla root
                            if (h == k)
                                parentitm = rootitm; 
                            else
                            {                                
                                // crea un item per ogni chiave del titolo
                                h = h + " " + k;
                                TreeViewItem itm = MBBCommon.GetExistingItem(h, tvNotes.Items);
                                //TreeViewItem itm = GetExistingItem(s, tvNotes.Items);
                                if (itm == null)
                                {
                                    itm = new TreeViewItem();
                                    itm.Header = lstkeysocc.Where(X => X.Key == k).First().Key + "(" + lstkeysocc.Where(X => X.Key == k).First().Value + ")";
                                    itm.Tag = h;
                                    parentitm.Items.Add(itm);                                  
                                }

                                parentitm = itm;
                            }
                        }

                        // QUI C'è L'ERRORE: ASSOCIO SEMPRE A PARENTITM quando questo invece cambia
                        // Questo ciclo va messo nel ciclo foreach precedente filtrando lsttmp con le note della parola chiave corrente (h)
                        foreach (DbNote n in lsttmp)
                        {
                            CRSListViewItemNote itm = new CRSListViewItemNote(n.DateTimeInserted, n.Text, n.Title, this.Width);
                            itm.Tag = n;
                            itm.SetSelectEvent(SelectItemMenuEvent);
                            itm.SetDblClickEvent(DblClickItemMenuEvent);
                            parentitm.Items.Add(itm);
                        }
                        lstnotes.RemoveAll(Y => lsttmp.Where(X => X.ID == Y.ID).Count() > 0);
                        #endregion

                         #region NUOVO
                        /*
                        string h = "";
                        TreeViewItem parentitm = GetItemFirstLevelTree(lstkeys.First(), tvNotes.Items);
                        int idx = 0;
                        foreach (string k in lstkeys)
                        {
                            #region CREO NODO DIRECTORY PER OGNI PAROLA CHIAVE
                            if (string.IsNullOrEmpty(h))
                                h = k;
                            else
                                // crea un item per ogni chiave del titolo
                                h = h + " " + k;

                            TreeViewItem rootitm = new TreeViewItem();
                            rootitm.Header = lstkeysocc.Where(X => X.Key == k).First().Key + "(" + lstkeysocc.Where(X => X.Key == k).First().Value + ")"; //k;
                            rootitm.Tag = h;

                            //TreeViewItem parentitm = GetExistingItem(h, tvNotes.Items);//GetItemInTree(h, tvNotes);
                            //TreeViewItem parentitm = GetItemFirstLevelTree(h, tvNotes.Items);
                            if (idx>0)
                                parentitm = GetExistingItem(h, tvNotes.Items);//GetItemFirstLevelTree(k, tvNotes.Items);
                            if (parentitm == null)
                            {
                                tvNotes.Items.Add(rootitm);
                                //parentitm = rootitm;
                            }
                            else
                            {
                                parentitm.Items.Add(rootitm);
                                //rootitm = parentitm;
                            }
                            parentitm = rootitm;
                            idx++;
                            #endregion

                            #region CREO NODI NOTA PER OGNI NOTA CHE HA IL TITLE UGUALE A QUELLO CHE STO ANALIZZANDO
                            List<DbNote> lsttmp = this.FindNotes(h, lstnotes);
                            foreach (DbNote n in lsttmp)
                            {
                                CRSListViewItemNote itm = new CRSListViewItemNote(n.DateTimeInserted, n.Text, n.Title, this.Width);
                                itm.Tag = n;
                                itm.SetSelectEvent(SelectItemMenuEvent);
                                itm.SetDblClickEvent(DblClickItemMenuEvent);
                                parentitm.Items.Add(itm);
                            }
                            lstnotes.RemoveAll(Y => lsttmp.Where(X => X.ID == Y.ID).Count() > 0);
                            #endregion
                            
                        }*/
                        #endregion

                    }
                }
            }
            catch
            { }
        }
        /// <summary>
        /// Data una lista di titles con i tags, parole chiave, restituisce una lista di titoli non duplicati con le parole ordinate per ordine alfabetico:
        /// - es: fisica todeschini einstein, fisica einstein todeschini = einstein fisica todeschini 
        /// </summary>
        /// <returns></returns>
        /*private List<string> NormalizeListTitles(List<string> lsttitles)
        {
            List<string> res = new List<string>();
            foreach(string t in lsttitles)
            {
                List<string> lstkeys = t.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                string newt = string.Join(MBBCommon.SeparationCharTitle, lstkeys.OrderBy(X => X).ToArray());
                if(res.Where(X=>X==newt).Count() ==0) res.Add(newt);
            }
            return res;

        } 
        /// <summary>
        /// Semplifica l'albero compattando i nodi che non hanno foglie e i rami singoli
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="typecalcdir"></param>
        private TreeViewItem NormalizeTreeView(ItemCollection nodes)
        {
            TreeViewItem itemRes = null;
            //TreeViewItem nodestoremove = new TreeViewItem();
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is TreeViewItem)
                    {
                        TreeViewItem item = (TreeViewItem)nodes[i];
                        // se sul nodo ho un solo figlio attacco il figlio al padre ed elimino il nodo
                        if (item.Items.Count <= 1)
                        {
                            //item.Header = " to remove: "+ item.Tag.ToString();
                            //item.Tag = "toremove";
                            if (item.Items[0] is TreeViewItem && item.Parent is TreeViewItem)
                            {
                                TreeViewItem newchild = (TreeViewItem)item.Items[0];
                                TreeViewItem parent = (TreeViewItem)item.Parent;
                                parent.Header = parent.Header + " " + item.Header;
                                item.Items.Remove(newchild);

                                parent.Items.Add(newchild);
                                parent.Items.Remove(item);
                            }
                            else
                            {
                                if (item.Items[0] is CRSListViewItemNote && item.Parent is TreeViewItem)
                                {
                                    CRSListViewItemNote newchild = (CRSListViewItemNote)item.Items[0];
                                    TreeViewItem parent = (TreeViewItem)item.Parent;
                                    parent.Header = parent.Header + " " + item.Header;
                                    item.Items.Remove(newchild);

                                    parent.Items.Add(newchild);
                                    parent.Items.Remove(item);
                                }
                            }
                            
                        }
                        else
                        {
                            itemRes = NormalizeTreeView(item.Items);
                        }
                        itemRes = null;
                    }
                    else return null;
                }

                return itemRes;

            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return null;
            }
        }
   
        private TreeViewItem GetItemFirstLevelTree(string itemtag, ItemCollection nodes)
        {
            TreeViewItem res = null;
            foreach (TreeViewItem itm in nodes)
            {
                if (itm.Tag.ToString() == itemtag)
                {
                    res = itm;
                    break;
                }
            }
            return res;
        }

        public TreeViewItem GetExistingItem(string itemtag, ItemCollection nodes)
        {
            TreeViewItem itemRes = null;
            try
            {
                for (int i = 0; i < nodes.Count; i++ )
                {
                    //if (item is TreeViewItem)
                    if (nodes[i] is TreeViewItem)
                    {
                        TreeViewItem item = (TreeViewItem)nodes[i];
                        //if (item.Tag.Equals(itemtag))
                        if (item.Tag.Equals(itemtag))
                        {
                            itemRes = item;
                            break;
                        }
                        else
                        {
                            // continua solo se la prima parte batte (itemtag.contains(item.tag))
                            string itmtg = item.Tag.ToString();
                            if (itmtg.Length <= itemtag.Length && itmtg.Equals(itemtag.Substring(0, itmtg.Length)))
                            {
                                itemRes = GetExistingItem(itemtag, item.Items);
                                if (itemRes != null) break;
                            }
                            //else break;
                        }
                    }
                }
                return itemRes;

            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return null;
            }
        }
        */
        #endregion      

        #region EVENTS DELEGATES
        private void SelectItemMenuEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                object itemtag = (sender as CRSListViewItemNote).Tag;
                DbNote s = (DbNote)itemtag;
                if (eventSelect != null) eventSelect(s.ID, s.IDUser);
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }

        private void DblClickItemMenuEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Mouse.SetCursor(Cursors.Wait);
            try
            {
                object itemtag = (sender as CRSListViewItemNote).Tag;
                DbNote s = (DbNote)itemtag;
                if (eventDblClick != null) eventDblClick(s);
            }
            catch { }
            Mouse.SetCursor(Cursors.Arrow);
        }
        #endregion

        #region FILL DEL SYNCRO
        private void FillNoteSyncroDelDBFile(Int64 idnote, MBBSyncroParams p)
        {
            string user = DbDataAccess.GetUser(this.idUser);         
            DateTime dt = DateTime.Now;
            MBBSyncroUtility.WriteDelNoteSyncroFile(dt, idnote, user, p);
            
            List<string> users = DbDataAccess.GetSharedUsers(idnote, this.idUser);
            foreach (string dest in users) 
                MBBSyncroUtility.WriteDelNoteSyncroFileShared(dt, idnote, user, dest, p);
        }
        #endregion

        #region EVENTS ctrls
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
            List<DbNote> lst = new List<DbNote>();
            foreach (CRSListViewItemNote n in lvNotes.Items)
            {
                if (n.IsChkSelected)
                {
                    DbNote d = n.Tag as DbNote;
                    lst.Add(d);
                }

            }
            /*if (lvNotes.SelectedItems.Count > 0)
            {
                DbNote n = (lvNotes.SelectedItems[0] as CRSListViewItemNote).Tag as DbNote;
                if (this.eventShowMultiple != null) evenDblClick(n);
            }*/
            if (lst.Count>0 && this.eventOpen != null)
                this.eventOpen(lst);

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            bool issel = false;
            foreach (CRSListViewItemNote n in lvNotes.Items)
            {
                if (n.IsChkSelected)
                {
                    issel = true;
                    break;
                }
                 
            }
            if (issel)
            {
                MessageBoxResult r = MessageBox.Show(CRSTranslation.TranslateDialog(CRSTranslation.MSG_DEL_ITEMS, this.lang),
                                    CRSTranslation.TranslateDialog(CRSTranslation.MSG_WARNING, this.lang), MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    MBBSyncroParams p = new MBBSyncroParams();
                    p.DropBoxRootDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_DROPBOXDIR);
                    p.SharingDir = CRSIniFile.GetPropertyValueMBB(APPConstants.PROP_SHARINGDIR);
                    foreach (CRSListViewItemNote n in lvNotes.Items)
                    {
                        if (n.IsChkSelected)
                        {
                            DbNote d = n.Tag as DbNote;
                            //-- prima scrive su file di cancellazione delle note per allineamento
                            //-- poi cancella altrimenti elimino i riferimenti su db per scrittura su file delle condivisioni
                            FillNoteSyncroDelDBFile(d.ID, p);
                            bool res = DbDataUpdate.DeleteNote(d.ID, d.IDUser, true);
                        }
                    }
                    List<DbNote> notes = new List<DbNote>();
                    if (this.findParams != null) 
                        notes = FillNotes(this.findParams);
                    else lvNotes.SelectedItems[0] = null;

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
            foreach (CRSListViewItemNote n in lvNotes.Items)
            {
                if (chkSelAll.IsChecked.Value)
                    n.IsChkSelected = true;
                else n.IsChkSelected = false;
            }
        }

        private void txtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterText(txtFind.Text);
        }
        
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }
        #endregion

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
             //NB: E' DA IMPLEMETARE IL SALVATAGGIO DELLA NUOVA NOTA E L'ELIMINAZIONE DELLE NOTE COMPONENTI !!!
            List<DbNote> lst = new List<DbNote>();
            foreach (CRSListViewItemNote n in lvNotes.Items)
            {
                if (n.IsChkSelected)
                {
                    DbNote d = n.Tag as DbNote;
                    lst.Add(d);
                }
            }

            if (lst.Count > 0 && this.eventMerge != null)
                this.eventMerge(lst);
        }

        private void btnTreeView_Click(object sender, RoutedEventArgs e)
        {
            if (btnTreeView.Background == Brushes.Lime)
            {
                FillListWithItems(this.notes);
                pnlGridMain.RowDefinitions[2].Height = new GridLength(0);
                pnlGridMain.RowDefinitions[1].Height = new GridLength(200, GridUnitType.Star);
                btnTreeView.Background = Brushes.WhiteSmoke;
                btnDel.IsEnabled = true;
                btnOpen.IsEnabled = true;
                btnMerge.IsEnabled = true;
            }
            else
            {
                int typecalcdir = 1;
                if (!string.IsNullOrEmpty(cbTypeDir.Text))
                    typecalcdir = Convert.ToInt32(cbTypeDir.Text.Substring(0, 1));
                tvNotes.Items.Clear();
                FillTreeViewWithItems(this.notes, typecalcdir);
                pnlGridMain.RowDefinitions[1].Height = new GridLength(0);
                pnlGridMain.RowDefinitions[2].Height = new GridLength(200, GridUnitType.Star);
                btnTreeView.Background = Brushes.Lime;
                btnDel.IsEnabled = false;
                btnOpen.IsEnabled = false;
                btnMerge.IsEnabled = false;
            }
        }

        private void btnNormalize_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = MBBCommon.NormalizeTreeView(tvNotes.Items);
            //tvNotes.Items.Clear();
            //vNotes.Items.Add(item);
        }

        private void cbOrderBy_DropDownClosed(object sender, EventArgs e)
        {
            OrderNotes();
            this.FillNotesExt(this.notes);
        }

        private List<DbNote> OrderNotes()
        {
            List<DbNote> lstord = this.notes.ToList();
            if (cbOrderBy.Text.Contains("1"))
                lstord = lstord.OrderBy(X => X.DateTimeInserted).ToList();
            else
            if (cbOrderBy.Text.Contains("2"))
                lstord = lstord.OrderByDescending(X => X.DateTimeInserted).ToList();
            else
            if (cbOrderBy.Text.Contains("3"))
                lstord = lstord.OrderBy(X => X.Title).ToList();

            return lstord;
        }
    }
}
