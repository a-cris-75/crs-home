using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoseidonData;
using PoseidonData.DataEntities;
using Infragistics.Win.UltraWinGrid;
using CRS.CommonControlsLib;
using CRS.Library;

namespace PoseidonApp.UserControls
{
    public partial class PSDResGroupBy : UserControl
    {
        public PSDResGroupBy()
        {
            InitializeComponent();
        }

        private DockStyle CURRENT_POS
        {
            set {
                pnlChart.Dock = value;
                splitter1.Dock = value;
            }
            get { return pnlChart.Dock; }
        }

        public int TYPE_CALC
        {
            set {
                if (value == COSTANTS.TYPE_RES_TOT_GRUOPBY_OCC)
                {
                    rbGroupByOcc.Checked = true;
                    //ugResIntervalli.DisplayLayout.Bands[0].Columns["OCCORRENZE_MEDIA_INTERVALLO_TOT"].Hidden = true;
                    //ugResIntervalli.DisplayLayout.Bands[0].Columns["NUM_ESTRAZIONI_MEDIA_INTERVALLO_TOT"].Hidden = false;
                }
                if (value == COSTANTS.TYPE_RES_TOT_GRUOPBY_NUMESTR)
                {
                    rbGroupByNumEstr.Checked = true;
                    //ugResIntervalli.DisplayLayout.Bands[0].Columns["OCCORRENZE_MEDIA_INTERVALLO_TOT"].Hidden = false;
                    //ugResIntervalli.DisplayLayout.Bands[0].Columns["NUM_ESTRAZIONI_MEDIA_INTERVALLO_TOT"].Hidden = true;
                }
            }
            get {
                if (rbGroupByNumEstr.Checked)
                    return COSTANTS.TYPE_RES_TOT_GRUOPBY_NUMESTR;
                else return COSTANTS.TYPE_RES_TOT_GRUOPBY_OCC;
            }
        }

        public int GROUPBY_NUM_ESTR {
            set { txtGroupByNumEstr.Text = value.ToString(); }
            get { return Convert.ToInt32(txtGroupByNumEstr.Text); }
        }

        public int GROUPBY_OCCORRENZE
        {
            set { txtGroupByOcc.Text = value.ToString(); }
            get { return Convert.ToInt32(txtGroupByOcc.Text); }
        }

        public delegate void EventSelectPeriod();
        private EventSelectPeriod eventSelectPeriod;

        public void SetEventSelectPeriod(EventSelectPeriod evn)
        {
            this.eventSelectPeriod = evn;
        }


        public bool SHOW_HEADER_PARAMS
        {
            set { pnlParams.Visible= value; }
            get { return pnlParams.Visible; }
        }

        public List<PSD_ESTR_SUCCESSI> LST_SUCCESSI = new List<PSD_ESTR_SUCCESSI>();
        public List<PSD_ESTR_LOTTO> LST_ESTRAZIONI = new List<PSD_ESTR_LOTTO>();

        public void FillResByNumEstr(int numestrgrp, List<PSD_ESTR_SUCCESSI> lstsucc, List<PSD_ESTR_LOTTO> lstestr)
        {
            this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(lstestr.ToList<object>());
            this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(lstsucc.ToList<object>());
            this.GROUPBY_NUM_ESTR = numestrgrp;
            List<PSD_RES_TOT> res = PSDCalc.GetResTotGroupByNumEstr(numestrgrp, lstsucc, lstestr);
            bsRES.DataSource = res;
            psdChartAnalysis1.Init(res);
        }

        public void FillResByOcc(int occgrp, List<PSD_ESTR_SUCCESSI> lstsucc, List<PSD_ESTR_LOTTO> lstestr)
        {
            this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(lstestr.ToList<object>());
            this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(lstsucc.ToList<object>());
            this.GROUPBY_OCCORRENZE = occgrp;
            List<PSD_RES_TOT> res = PSDCalc.GetResTotGroupByOcc(occgrp, lstsucc, lstestr);
            bsRES.DataSource = res;
            psdChartAnalysis1.Init(res);
        }
       
        //TO DO
        private void SetValuesResGrid()
        {
            try
            {
                //ugPartite.ActiveRowScrollRegion.ScrollRowIntoView(gr_figlio);
                foreach (UltraGridRow gr in ugResIntervalli.Rows)
                {
                    for (int r = 0; r < 11; r++)
                    {
                        string ruota = COSTANTS.RUOTE[r];
                    }

                }
            }
            catch { }
        }

        private void btnCalcola_Click(object sender, EventArgs e)
        {
            bsRES.DataSource = PSDCalc.GetResTotGroupByNumEstr(this.GROUPBY_NUM_ESTR, this.LST_SUCCESSI, this.LST_ESTRAZIONI);
        }

        private void ugTabellone_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            /*if (e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Value != null && 
                Convert.ToString(e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Value) == ">")               
            {
                e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Appearance.BackColor = pnlColorSpostBar1.BackColor;
            }
            if (e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Value != null &&
                Convert.ToString(e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Value) == "<")
            {
                e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_X"].Appearance.BackColor = pnlColorSpostBar2.BackColor;
            }
            if (e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Value != null &&
               Convert.ToString(e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Value) == "^")
            {
                e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Appearance.BackColor = pnlColorSpostBar3.BackColor;
            }
            if (e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Value != null &&
              Convert.ToString(e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Value) == "V")
            {
                e.Row.Cells["DIREZIONE_SPOSTAMENTO_BARICENTRO_Y"].Appearance.BackColor = pnlColorSpostBar4.BackColor;
            }
            */
            if (e.Row.Cells["DELTA_X_BAR_PREC"].Value != null)
            {
                if (Convert.ToSingle(e.Row.Cells["DELTA_X_BAR_PREC"].Value) > 0)
                    e.Row.Cells["DELTA_X_BAR_PREC"].Appearance.BackColor = pnlColorSpostBar1.BackColor;
                if (Convert.ToSingle(e.Row.Cells["DELTA_X_BAR_PREC"].Value) < 0)
                    e.Row.Cells["DELTA_X_BAR_PREC"].Appearance.BackColor = pnlColorSpostBar2.BackColor;
            }

            if (e.Row.Cells["DELTA_Y_BAR_PREC"].Value != null)
            {
               if (Convert.ToSingle(e.Row.Cells["DELTA_Y_BAR_PREC"].Value) > 0)
                    e.Row.Cells["DELTA_Y_BAR_PREC"].Appearance.BackColor = pnlColorSpostBar3.BackColor;
                if (Convert.ToSingle(e.Row.Cells["DELTA_Y_BAR_PREC"].Value) < 0)
                    e.Row.Cells["DELTA_Y_BAR_PREC"].Appearance.BackColor = pnlColorSpostBar4.BackColor;
            }


            if (e.Row.Cells["DELTA_X_BAR_MATRIX"].Value != null)
            {
                if (Convert.ToSingle(e.Row.Cells["DELTA_X_BAR_MATRIX"].Value) > 0)
                    e.Row.Cells["DELTA_X_BAR_MATRIX"].Appearance.BackColor = pnlColorSpostBar1.BackColor;
                if (Convert.ToSingle(e.Row.Cells["DELTA_X_BAR_MATRIX"].Value) < 0)
                    e.Row.Cells["DELTA_X_BAR_MATRIX"].Appearance.BackColor = pnlColorSpostBar2.BackColor;
            }

            if (e.Row.Cells["DELTA_Y_BAR_MATRIX"].Value != null)
            {
                if (Convert.ToSingle(e.Row.Cells["DELTA_Y_BAR_MATRIX"].Value) > 0)
                    e.Row.Cells["DELTA_Y_BAR_MATRIX"].Appearance.BackColor = pnlColorSpostBar3.BackColor;
                if (Convert.ToSingle(e.Row.Cells["DELTA_Y_BAR_MATRIX"].Value) < 0)
                    e.Row.Cells["DELTA_Y_BAR_MATRIX"].Appearance.BackColor = pnlColorSpostBar4.BackColor;

            }
        }

        public static void SetCellImage(UltraGridRow row, string columnKey, Bitmap bmp, string tooltip)
        {
            if (row.Cells.Exists(columnKey))
            {
                row.Cells[columnKey].Appearance.ImageBackground = bmp;
                row.Cells[columnKey].Appearance.Cursor = System.Windows.Forms.Cursors.Hand;
                if (tooltip != null)
                    row.Cells[columnKey].ToolTipText = tooltip;
            }
        }

        private void btnShowFcolpoart_Click(object sender, EventArgs e)
        {
            if (pnlChart.Visible && this.CURRENT_POS == DockStyle.Right)
                this.CURRENT_POS = DockStyle.Bottom;
            else
            if(pnlChart.Visible && this.CURRENT_POS == DockStyle.Bottom)
            {
                pnlChart.Visible = false;
            }
            else
            if (!pnlChart.Visible)
            {
                pnlChart.Visible = true;
                this.CURRENT_POS = DockStyle.Right;
            }

        }

        private void bsRES_PositionChanged(object sender, EventArgs e)
        {
            if (this.eventSelectPeriod != null)
                this.eventSelectPeriod();

            //posChartAnalysis1.
        }

        private void btnShowChart_Click(object sender, EventArgs e)
        {
            pnlChart.Visible = !pnlChart.Visible;
            tcRes.SelectedIndex = 2;
        }

        private void rbGroupByOcc_Click(object sender, EventArgs e)
        {
            this.TYPE_CALC = COSTANTS.TYPE_RES_TOT_GRUOPBY_OCC;
        }

        private void rbGroupByNumEstr_Click(object sender, EventArgs e)
        {
            this.TYPE_CALC = COSTANTS.TYPE_RES_TOT_GRUOPBY_NUMESTR;
        }

        private void btnCalcSaturazione_Click(object sender, EventArgs e)
        {
            /*if (bsRES.Current != null) {

                //int numrighe = Convert.ToInt32(txtNumRighe.Text);
                DateTime dt1 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_DA;
                DateTime dt2 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_A.AddDays(1);

                // se seleziono solo una riga per default considero la riga successiva da confrontare
                if (ugResIntervalli.Selected.Rows.Count == 1)
                {
                    UltraGridRow row = ugResIntervalli.Rows[ugResIntervalli.Selected.Rows[0].ListIndex + 1];
                    ugResIntervalli.Selected.Rows.Add(row);
                }

                ClearCtrlsStarSystem();
                int numidx = ugResIntervalli.Selected.Rows.Count;
                SelectedRowsCollection rows = ugResIntervalli.Selected.Rows;
                int maxdistSat1 = Convert.ToInt32(txtMaxDistSat1.Text);
                int maxdistSat2 = Convert.ToInt32(txtMaxDistSat2.Text);
                int maxdistSat3 = Convert.ToInt32(txtMaxDistSat3.Text);
                for (int idxM1 = 1; idxM1 < numidx; idxM1++)
                {
                        
                    PSD_RES_TOT r1 = ((PSD_RES_TOT)bsRES[rows[idxM1-1].ListIndex]);
                    dt1 = r1.DATA_EVENTO_DA;
                       
                    for (int idxM2 = idxM1 + 1; idxM2 <= numidx; idxM2++)
                    {
                        PSD_RES_TOT r2 = ((PSD_RES_TOT)bsRES[rows[idxM2 - 1].ListIndex]);
                        dt2 = r2.DATA_EVENTO_DA;
                        AddCtrlStarSystem("M" + idxM1.ToString() + "-M" + idxM2.ToString(), dt1, dt2, maxdistSat1,maxdistSat2,maxdistSat3);
                    }                   
                }

                tcRes.SelectedIndex = 1;
            }*/



            if (bsRES.Current != null && ugResIntervalli.Selected.Rows.Count>0)
            {
                
                int intervals = 1;
                if (rbConfrontaNum.Checked)
                {
                    intervals = Convert.ToInt32(txtConfrontaNumRows.Text);
                }
                else
                if (rbConfrontaTutte.Checked)
                {
                    intervals = ugResIntervalli.Rows.Count - ugResIntervalli.Selected.Rows[0].ListIndex;
                }

                // se seleziono solo una riga per default considero la riga successiva da confrontare
                if (rbConfrontaNum.Checked || rbConfrontaTutte.Checked)
                {
                    //int num = Convert.ToInt32(txtConfrontaNumRows.Text);
                    
                    for (int i = 0; i < intervals - 1; i++)
                    {
                        if (ugResIntervalli.Rows.Count > ugResIntervalli.Selected.Rows[0].ListIndex + i + 1)
                        {
                            UltraGridRow row = ugResIntervalli.Rows[ugResIntervalli.Selected.Rows[0].ListIndex + i + 1];
                            ugResIntervalli.Selected.Rows.Add(row);
                        }
                    }
                }
                int numidx = ugResIntervalli.Selected.Rows.Count;

                int maxdistSat1 = Convert.ToInt32(txtMaxDistSat1.Text);
                int maxdistSat2 = Convert.ToInt32(txtMaxDistSat2.Text);
                int maxdistSat3 = Convert.ToInt32(txtMaxDistSat3.Text);

                List<PSD_RES_TOT_SAT> lstresCalc = new List<PSD_RES_TOT_SAT>();
                DateTime dt1M1 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_DA;
                DateTime dt2M1 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_A.AddDays(1);
                DateTime dt1M2 = dt2M1;
                DateTime dt2M2 = dt2M1.AddDays(1);
                int idx = 0;
                foreach (UltraGridRow r in ugResIntervalli.Selected.Rows)
                {
                    if (idx + 1 < ugResIntervalli.Selected.Rows.Count)
                    {
                        int idxM1 = r.ListIndex;
                        int idxM2 = ugResIntervalli.Selected.Rows[idx + 1].ListIndex;
                        PSD_RES_TOT r1 = ((PSD_RES_TOT)bsRES[idxM1]);
                        dt1M1 = r1.DATA_EVENTO_DA;
                        dt2M1 = r1.DATA_EVENTO_A;
                        PSD_RES_TOT r2 = ((PSD_RES_TOT)bsRES[idxM2]);
                        dt1M2 = r2.DATA_EVENTO_DA;
                        dt2M2 = r2.DATA_EVENTO_A;

                        AddCtrlStarSystem("M" + idxM1.ToString() + "-M" + idxM2.ToString(), dt1M1, dt2M1, dt1M2, dt2M2, maxdistSat1, maxdistSat2, maxdistSat3);

                    }
                    idx++;
                }
                tcRes.SelectedIndex = 1;
                
            }
            else ABSMessageBox.Show("Seleziona una riga da cui cominciare..");
        }

        private void AddCtrlStarSystem(string captionPage, DateTime dt1M1, DateTime dt2M1, DateTime dt1M2, DateTime dt2M2, int maxdistSat1, int maxdistSat2, int maxdistSat3)
        {
            PSDStarSystem ctrl = new PSDStarSystem();
            ctrl.SHOW_HEADER = true;
            ctrl.Dock = DockStyle.Fill;

            List<PSD_MATH_MATRIX_NUM> matrix1 = new List<PSD_MATH_MATRIX_NUM>();
            List<PSD_MATH_MATRIX_NUM> matrix2 = new List<PSD_MATH_MATRIX_NUM>();

            DialogResult r = DialogResult.Yes;
            PSDCalc.GetMatrix(dt1M1, dt2M1, dt1M2, dt2M2, /*Convert.ToInt32(txtGroupByOcc.Text),*/chkParificaOcc.Checked, this.LST_SUCCESSI,out matrix1, out matrix2);
            if (matrix1.Count != matrix2.Count && !chkParificaOcc.Checked)
            {
               r =  ABSMessageBox.Show("ATTENZIONE: il numero di stelle e di pianeti non è uguale: per poter stabilire associazione univoca devono essere uguali!"+
                   "\nVuoi continuare?", "OCCORRENZE", MessageBoxButtons.YesNo);
            }
            if(r == DialogResult.Yes)
            { 
                ctrl.FillStarSystem(matrix1, matrix2, false, false, maxdistSat1,maxdistSat2, maxdistSat3);
                tcMatrix.TabPages.Clear();
                tcMatrix.TabPages.Add(captionPage, captionPage);
                tcMatrix.TabPages[tcMatrix.TabPages.Count - 1].Controls.Add(ctrl);
            }
        }

        private void ClearCtrlsStarSystem()
        {
            int numpages = tcMatrix.TabPages.Count;
            for (int idx = 1; idx < numpages; idx++)
            {
                if (tcMatrix.TabPages.Count>1 && tcMatrix.TabPages[idx].Controls[0] is PSDStarSystem)
                    tcMatrix.TabPages[idx].Controls[0].Dispose();

                tcMatrix.TabPages.RemoveAt(idx);
            }
        }
        /*
        private void GetMatrix(DateTime dtM1Start, DateTime dtM2Start, int numoccorrenze, out List<PSD_MATH_MATRIX_NUM> matrix1_lstSTAR, out List<PSD_MATH_MATRIX_NUM> matrix2_lstPLANETS)
        {
            matrix1_lstSTAR = new List<PSD_MATH_MATRIX_NUM>();
            matrix2_lstPLANETS = new List<PSD_MATH_MATRIX_NUM>();

            List<PSD_ESTR_SUCCESSI> lstsucc1 = this.LST_SUCCESSI.Where(X => X.Data >= dtM1Start).ToList();
            lstsucc1.RemoveRange(numoccorrenze, lstsucc1.Count - numoccorrenze);
            List<PSD_ESTR_SUCCESSI> lstsucc2 = this.LST_SUCCESSI.Where(X => X.Data >= dtM2Start).ToList();
            lstsucc2.RemoveRange(numoccorrenze, lstsucc2.Count - numoccorrenze);

            lstsucc1 = lstsucc1.OrderBy(X => X.IDEstrazione).ThenBy(X=>X.Ruota).ToList();
            int idestr1 = lstsucc1.First().IDEstrazione;

            foreach(PSD_ESTR_SUCCESSI s in lstsucc1)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATANUM = s.Data;
                n.GROUPCOL = s.Ruota;
                n.GROUPROW = s.IDEstrazione.ToString();
                n.NUM = s.NumeroA;
                n.POSX = s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.Ruota + s.Posizione.ToString();
                // ID: pos X + grop col + pos in group (es:BA1010 => estratto a Ba in pos 1 alla 10ma riga della matrice)
                n.ID = n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix1_lstSTAR.Add(n);
            }
            idestr1 = lstsucc2.First().IDEstrazione;
            foreach (PSD_ESTR_SUCCESSI s in lstsucc2)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATANUM = s.Data;
                n.GROUPCOL = s.Ruota;
                n.GROUPROW = s.IDEstrazione.ToString();
                n.NUM = s.NumeroA;
                n.POSX = s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.Ruota + s.Posizione.ToString();
                n.ID = n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix2_lstPLANETS.Add(n);
            }
        }
        */
        private void btnCalcolaMinDist1_Click(object sender, EventArgs e)
        {
            int numpages = tcMatrix.TabPages.Count;
            for (int idx = 1; idx < numpages; idx++)
            {
                if (tcMatrix.TabPages.Count > 1 && tcMatrix.TabPages[idx].Controls[0] is PSDStarSystem)
                {
                    PSDStarSystem c = tcMatrix.TabPages[idx].Controls[0] as PSDStarSystem;
                    //c.FillGridCalcCoppieAlgorithmOnlyUnivoche();
                }
            }
        }

        private void btnCalcNew_Click(object sender, EventArgs e)
        {
            
            //if (this.LST_SUCCESSI.Count == 0)
                this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(APPGlobalInfo.LST_SUCCESSI.ToList<object>());

            //if (this.LST_ESTRAZIONI.Count == 0)
                this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(APPGlobalInfo.LST_ESTRAZIONI.ToList<object>());

            APPGlobalInfo.NUM_ESTR_GROUP = Convert.ToInt32(txtGroupByNumEstr.Text);
            APPGlobalInfo.NUM_OCC_GROUP = Convert.ToInt32(txtGroupByOcc.Text);
            APPGlobalInfo.TYPE_CALC_OCC0_ESTR1 = rbGroupByOcc.Checked ? 0 : 1;

            if (rbGroupByNumEstr.Checked)
                this.FillResByNumEstr(Convert.ToInt32(txtGroupByNumEstr.Text), this.LST_SUCCESSI, this.LST_ESTRAZIONI);
            else
                this.FillResByOcc(Convert.ToInt32(txtGroupByOcc.Text), this.LST_SUCCESSI, this.LST_ESTRAZIONI);

            if (chkShowModalFormRes.Checked)
            {
                PoseidonApp.Forms.FPSDResCalcoli f = new Forms.FPSDResCalcoli();
                f.psdResGroupBy2.FillResByOcc(Convert.ToInt32(txtGroupByOcc.Text), this.LST_SUCCESSI, this.LST_ESTRAZIONI);
                f.ShowDialog();

            }
        }

        private void btnInfoFunzioni_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("CONFRONTA RIGHE:\nConfronta righe (num righe) in due modi: \n - se eselezioni una, con le successiva \n - righe diverse selezionate con tasto ctrl" +
                "\n\nFUNZIONAMENTO:\n 1) crea le matrici stelle/piantei fra due righe (la prima le stelle, la seconda i pianeti)"+
                "\n 2) determina la saturazione di ogni stella e altri parametri " + 
                "\n    saturazione: indice di riempimento delle orbite della stella con i pianeti della matrice 2" +
                "\n 3) mostra i risultati nel tab 'Confronta matrici e pianeti'" + 
                "\n\nCALCOLA COPPIE UNIVOCHE: "+
                "\nCalcola le coopie univoche: cioè mette insieme una stella con un solo pianeta secondo la regola che fra i due c'è la minima distanza e nessun altro pianeta e stella hanno questa condizione");
        }

        private void btnInfoOpz_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("RICALCOLA:\nSeleziona una riga di riferimento per stabilire l'intervallo temporale iniziale e confronta le occorrenze con la riga successiva o con la seconda riga scelta (tasto ctrl).\n" +
               "Funzionamento: " +
               "\n   1) per ogni occorrenza della prima riga (matrice M1) determina una lista con tutte le occorrenze della riga successiva (matrice M2)" +
               "\n   2) stabilisce per ogni elemento (numero STAR) tutte le distanze dagli elementi di M2 " +
               "\n   3) cerca di accoppiare i numeri di M1 con i numeri di M2 secondo il concetto di minima distanza " +
               "\n\nUtilità: " +
               "\nServe per capire come può evolvere il sistema da un gruppo di estrazioni ad uno consecutivo, e se può valere la regola della minima distanza" +
               "\n\nMOSTRA GRAFICI:\nMostra il grafico deducibile dalle colonne dei risultati.\n"
               );
        }

        private void BtnInfoParametriCalcRis_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("2 - RAGGRUPPAMENTI: \nNel calcolo dei risultati raggruppa le estrazioni per:" +          
                "\n  - OCCORRENZE: per un numero di estrazioni che comprende un numero determinato di occorrenze"+
                "\n  - NUM ESTRAZIONI: per un numero fisso di estrazioni " +
                "\n    PARIFICA OCCORRENZE: nell'intervallo temporale determinato da NUM ESTRAZIONI elimina le occorrenze della matrice che ne ha di più." +
                "\nServe per il corretto calcolo delle coppie univoche (quando l'algoritmo sarà in grado di determinarle..)" +
                "\n\n3 - SATURAZIONI: \nI parametri indicano la distanza di sarturazione max da considerare, su cui viene calcolato l'indice. " + 
                "\nServe per considerare un numero di gruppi stella/pianeti limitato: i gruppi sono solo dei gruppi consistenti significativi " + 
                "\n INDICE DI SATURAZIONE: SUM[(num pianeti a dist) x (1 / (distanza + 1))]" )
               ;
        }
    }
}
