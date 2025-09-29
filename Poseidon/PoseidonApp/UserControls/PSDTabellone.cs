using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using CRS.DBLibrary;
using PoseidonData;
using PoseidonData.DataEntities;
using CRS.CommonControlsLib;
using CRS.Library;

namespace PoseidonApp.UserControls
{
    public partial class PSDTabellone : UserControl
    {
        public PSDTabellone()
        {
            InitializeComponent();
    
            APPGlobalInfo.PARAMETRI_R1.COLPI = new List<int>(new int[] { 1, 2, 3 });
            APPGlobalInfo.PARAMETRI_R1.TIPO_FREQUENZA = new List<int>(new int[] { 1, 2, 3, 4 });
            APPGlobalInfo.PARAMETRI_R1.POS_DEC_R12 = new List<string>(new string[] { "41" });
            APPGlobalInfo.PARAMETRI_R1.ACCOPPIATE_DEC = new List<string>(new string[] { "BA-CA","FI-GE","MI-NA","PA-RM","TO-VE","VE-NZ" });

            psdViewParamsNumInGioco1.Init(APPGlobalInfo.PARAMETRI_R1, new Dictionary<string, List<int>>());
            psdViewParamsNumInGioco1.TYPE_NUM_IN_GIOCO = 1;

        }

        private DateTime DATA_INIZIO = DateTime.MinValue;
        private DateTime DATA_FINE = DateTime.MinValue;

        public bool SHOW_OPTIONS {
            set { pnlOptions.Visible = value; }
            get { return pnlOptions.Visible; }
        }

        public bool SHOW_PNL_COLORS
        {
            set { pnlColors.Visible = value; }
            get { return pnlColors.Visible; }
        }

        List<PSD_ESTR_SUCCESSI> LST_SUCCESSI = new List<PSD_ESTR_SUCCESSI>();
        List<PSD_ESTR_LOTTO> LST_ESTRAZIONI = new List<PSD_ESTR_LOTTO>();

        public void FillTabellone(DateTime dt1, DateTime dt2)
        {
            this.DATA_INIZIO = dt1;
            this.DATA_FINE = dt2;

            this.LST_ESTRAZIONI = DbDataAccess.GetEstrazioni(this.DATA_INIZIO, this.DATA_FINE);
            bsTabellone.DataSource = this.LST_ESTRAZIONI;
            APPGlobalInfo.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(this.LST_ESTRAZIONI.ToList<object>());//this.LST_ESTRAZIONI;
        }

        public void ColoraTabellone(int tipoevidenza)
        {
            // evidenzio occorrenze (numeri R1 sortiti con successo) con colori diversi: 
            // serve per evidenziare quando la R1 ha funzionato maggiormente

            switch (tipoevidenza)
            {
                case COSTANTS.TIPO_EVIDENZA_1_OCCORRENZE:
                    SetColorsOccR1();
                    break;
                case COSTANTS.TIPO_EVIDENZA_2_DECINE_SCELTI:
                    SetColorsDecineSucc();
                    break;
                case COSTANTS.TIPO_EVIDENZA_5_DECINE_ESTR:
                    SetColorsDecineEstratti();
                    break;
                case COSTANTS.TIPO_EVIDENZA_4_NUM_SCELTI:
                    SetColorsNumScelti();
                    break;
                default:
                    SetColorsOccR1();
                    break;
            }
            
            try
            {
                //psdColorTab1.LST_COLORS
            }
            catch { }
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FillTabellone(APPGlobalInfo.DATA_INIZIO, APPGlobalInfo.DATA_FINE);
            FillSuccessi();
            txtTotRighe.Text = ugTabellone.Rows.Count.ToString();

            this.Cursor = Cursors.Default;
        }

        private void btnEvidenziaCalcola_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ColoraTabellone(psdColorTab2.TIPO_EVIDENZA);
            this.Cursor = Cursors.Default;
        }

        private bool PlayScreenView(int numrows)
        {
            bool endgrid = false;
            UltraGridRow gr = ugTabellone.ActiveRow;
            try
            {

                //if (gr.Index + numrows < maxnumrow)
                if (gr.Index + numrows < ugTabellone.Rows.Count)
                {
                    UltraGridRow grnext = ugTabellone.Rows[gr.Index + numrows];
                    ugTabellone.ActiveRowScrollRegion.ScrollRowIntoView(grnext);
                    ugTabellone.ActiveRow = grnext;
                }
                else endgrid = true;
            }
            catch { }

            return !endgrid;
        }

        /// <summary>
        /// Coloro in base alla decina dell'estratto, un colore una decina
        /// </summary>
        private void SetColorsDecineEstratti()
        {
            try
            {
                //ugPartite.ActiveRowScrollRegion.ScrollRowIntoView(gr_figlio);
                foreach (UltraGridRow gr in ugTabellone.Rows)
                {
                    for (int r = 0; r < 11; r++)
                    {
                        string ruota = COSTANTS.RUOTE[r];

                        int v1 = Convert.ToInt32(gr.GetCellValue(ruota + "1"));
                        int v2 = Convert.ToInt32(gr.GetCellValue(ruota + "2"));
                        int v3 = Convert.ToInt32(gr.GetCellValue(ruota + "3"));
                        int v4 = Convert.ToInt32(gr.GetCellValue(ruota + "4"));
                        int v5 = Convert.ToInt32(gr.GetCellValue(ruota + "5"));

                        int D1 = v1 / 10;
                        int D2 = v2 / 10;
                        int D3 = v3 / 10;
                        int D4 = v4 / 10;
                        int D5 = v5 / 10;

                        if (v1 > 0 && v1 % 10 == 0) D1 = D1 - 1;
                        if (v2 > 0 && v2 % 10 == 0) D2 = D2 - 1;
                        if (v3 > 0 && v3 % 10 == 0) D3 = D3 - 1;
                        if (v4 > 0 && v4 % 10 == 0) D4 = D4 - 1;
                        if (v5 > 0 && v5 % 10 == 0) D5 = D5 - 1;

                        if (psdColorTab2.LST_COLORS[D1].Item2)
                            gr.Cells[ruota + "1"].Appearance.BackColor = psdColorTab2.LST_COLORS[D1].Item1;
                        if (psdColorTab2.LST_COLORS[D2].Item2)
                            gr.Cells[ruota + "2"].Appearance.BackColor = psdColorTab2.LST_COLORS[D2].Item1;
                        if (psdColorTab2.LST_COLORS[D3].Item2)
                            gr.Cells[ruota + "3"].Appearance.BackColor = psdColorTab2.LST_COLORS[D3].Item1;
                        if (psdColorTab2.LST_COLORS[D4].Item2)
                            gr.Cells[ruota + "4"].Appearance.BackColor = psdColorTab2.LST_COLORS[D4].Item1;
                        if (psdColorTab2.LST_COLORS[D5].Item2)
                            gr.Cells[ruota + "5"].Appearance.BackColor = psdColorTab2.LST_COLORS[D5].Item1;
                    }

                }
            }
            catch { }
        }

        private void SetColorsDecineSucc()
        {
            try
            {
                //ugPartite.ActiveRowScrollRegion.ScrollRowIntoView(gr_figlio);
                foreach (UltraGridRow gr in ugTabellone.Rows)
                {
                    DateTime dt = Convert.ToDateTime(gr.GetCellValue("Data"));
                    List<PSD_ESTR_SUCCESSI> succ = this.LST_SUCCESSI.Where(X => X.Data == dt).ToList();
                    for (int r = 0; r < 11; r++)
                    {
                        string ruota = COSTANTS.RUOTE[r];


                        int v1 = Convert.ToInt32(gr.GetCellValue(ruota + "1"));
                        int v2 = Convert.ToInt32(gr.GetCellValue(ruota + "2"));
                        int v3 = Convert.ToInt32(gr.GetCellValue(ruota + "3"));
                        int v4 = Convert.ToInt32(gr.GetCellValue(ruota + "4"));
                        int v5 = Convert.ToInt32(gr.GetCellValue(ruota + "5"));

                        List<PSD_ESTR_SUCCESSI> succtmp = succ.Where(X => X.Ruota == ruota &&
                                                                (X.NumeroA == v1 ||
                                                                X.NumeroA == v2 ||
                                                                X.NumeroA == v3 ||
                                                                X.NumeroA == v4 ||
                                                                X.NumeroA == v5)).ToList();
                        if (succtmp.Count() > 0)
                        {

                            int D1 = v1 / 10;
                            int D2 = v2 / 10;
                            int D3 = v3 / 10;
                            int D4 = v4 / 10;
                            int D5 = v5 / 10;

                            if (v1 > 0 && v1 % 10 == 0) D1 = D1 - 1;
                            if (v2 > 0 && v2 % 10 == 0) D2 = D2 - 1;
                            if (v3 > 0 && v3 % 10 == 0) D3 = D3 - 1;
                            if (v4 > 0 && v4 % 10 == 0) D4 = D4 - 1;
                            if (v5 > 0 && v5 % 10 == 0) D5 = D5 - 1;

                            if (succtmp.Where(X => X.NumeroA == v1).Count() > 0 && psdColorTab2.LST_COLORS[D1].Item2)
                                gr.Cells[ruota + "1"].Appearance.BackColor = psdColorTab2.LST_COLORS[D1].Item1;
                            if (succtmp.Where(X => X.NumeroA == v2).Count() > 0 && psdColorTab2.LST_COLORS[D2].Item2)
                                gr.Cells[ruota + "2"].Appearance.BackColor = psdColorTab2.LST_COLORS[D2].Item1;
                            if (succtmp.Where(X => X.NumeroA == v3).Count() > 0 && psdColorTab2.LST_COLORS[D3].Item2)
                                gr.Cells[ruota + "3"].Appearance.BackColor = psdColorTab2.LST_COLORS[D3].Item1;
                            if (succtmp.Where(X => X.NumeroA == v4).Count() > 0 && psdColorTab2.LST_COLORS[D4].Item2)
                                gr.Cells[ruota + "4"].Appearance.BackColor = psdColorTab2.LST_COLORS[D4].Item1;
                            if (succtmp.Where(X => X.NumeroA == v5).Count() > 0 && psdColorTab2.LST_COLORS[D5].Item2)
                                gr.Cells[ruota + "5"].Appearance.BackColor = psdColorTab2.LST_COLORS[D5].Item1;
                        }
                    }

                }
            }
            catch { }
        }

        private void SetColorsOccR1()
        {
            // OTTENGO UN RAGGRUPPAMENTO dei successi per data, ruota, numero estratto, in modo da poter rappresentare sul tabelone per ogni numero 
            // un valore di occorrenza rappresentato da un colore
            //List<PSD_RES> lstgrpby = DbDataAccess.GetResR1_GROUPBY(this.DATA_INIZIO, this.DATA_FINE, COSTANTS.RUOTE.ToList<string>()
            //    , APPGlobalInfo.PARAMETRI_R1
            //    , true, true);

            if(this.LST_SUCCESSI.Count==0)
                this.LST_SUCCESSI = DbDataAccess.GetResR1NoDbTable(this.DATA_INIZIO, this.DATA_FINE, COSTANTS.RUOTE.ToList<string>(), APPGlobalInfo.PARAMETRI_R1);

            SetColorGrid(this.LST_SUCCESSI, Color.Transparent);
           
        }

        private void SetColorsNumScelti()
        {
            // OTTENGO UN RAGGRUPPAMENTO dei successi per data, ruota, numero estratto, in modo da poter rappresentare sul tabelone per ogni numero 
            // un valore di occorrenza rappresentato da un colore
            List<PSD_ESTR_SUCCESSI> lstgrpby = DbDataAccess.GetResNumScelti(this.DATA_INIZIO, this.DATA_FINE, APPGlobalInfo.NUMERI_CASUALI);
            SetColorGrid(lstgrpby, Color.Transparent);
        }

        public void SetColorGrid(List<PSD_ESTR_SUCCESSI> lstgrpby, Color c)
        {
            try
            {
                foreach (UltraGridRow gr in ugTabellone.Rows)
                {
                    int ID = Convert.ToInt32(gr.GetCellValue("IDEstrazione"));
                    List<PSD_ESTR_SUCCESSI> lstgrpbyTMP = lstgrpby.Where(X => X.IDEstrazione == ID).ToList();
                    if (lstgrpbyTMP.Count > 0)
                    {

                        for (int r = 0; r < 11; r++)
                        {
                            string ruota = COSTANTS.RUOTE[r];
                           
                            foreach(PSD_ESTR_SUCCESSI s in lstgrpbyTMP.Where(X => X.Ruota == ruota).ToList())
                            {
                                SetColorCell(gr, ruota, s.Occorrenze, Convert.ToString(s.Posizione), c);
                            }
                            /*int occorrenze = lstgrpbyTMP.Where(X => X.Ruota == ruota).Sum(X => X.Occorrenze);
                            if (occorrenze > 0)
                            {
                                int N1 = Convert.ToInt32(gr.GetCellValue(ruota + "1"));
                                int N2 = Convert.ToInt32(gr.GetCellValue(ruota + "2"));
                                int N3 = Convert.ToInt32(gr.GetCellValue(ruota + "3"));
                                int N4 = Convert.ToInt32(gr.GetCellValue(ruota + "4"));
                                int N5 = Convert.ToInt32(gr.GetCellValue(ruota + "5"));
                                //gr.Appearance.BackColor = psdColorTab1.LST_COLORS[]
                                SetColorCell(gr, lstgrpbyTMP, ruota, N1, "1", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N2, "2", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N3, "3", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N4, "4", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N5, "5", c);
                            }*/
                        }
                    }
                }
            }
            catch { }
        }

        public void SetColorGrid2(List<PSD_RES> lstgrpby, Color c)
        {
            try
            {
                foreach (UltraGridRow gr in ugTabellone.Rows)
                {
                    int ID = Convert.ToInt32(gr.GetCellValue("NUMERO"));
                    List<PSD_RES> lstgrpbyTMP = lstgrpby.Where(X => X.ID_ESTRAZIONE == ID).ToList();
                    if (lstgrpbyTMP.Count > 0)
                    {

                        for (int r = 0; r < 11; r++)
                        {
                            string ruota = COSTANTS.RUOTE[r];
                            // occorrenze per ruota / data
                            //List<PSD_RES> lstgrpbyTMP2 = lstgrpbyTMP.Where(X => X.RUOTA == ruota).ToList();
                            int occorrenze = lstgrpbyTMP.Where(X => X.RUOTA == ruota).Sum(X => X.OCCORRENZE_GROUPBY);
                            if (occorrenze > 0)
                            {
                                int N1 = Convert.ToInt32(gr.GetCellValue(ruota + "1"));
                                int N2 = Convert.ToInt32(gr.GetCellValue(ruota + "2"));
                                int N3 = Convert.ToInt32(gr.GetCellValue(ruota + "3"));
                                int N4 = Convert.ToInt32(gr.GetCellValue(ruota + "4"));
                                int N5 = Convert.ToInt32(gr.GetCellValue(ruota + "5"));
                                //gr.Appearance.BackColor = psdColorTab1.LST_COLORS[]
                                SetColorCell(gr, lstgrpbyTMP, ruota, N1, "1", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N2, "2", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N3, "3", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N4, "4", c);
                                SetColorCell(gr, lstgrpbyTMP, ruota, N5, "5", c);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Colora la griglia in base alla posizione dei numeri della matrice.
        /// deltaX serve per tradurre la coordinata X a partire da: infatti posX=1 rappresenta BA1 ma la griglia ha anche le prime due colonne
        /// DATA e NUMERO
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="cbackground"></param>
        /// <param name="deltaX_fromCol1"></param>
        public void SetColorGridByPos(List<PSD_MATH_MATRIX_NUM> matrix, Color cbackground, Color cforeground, int deltaX_fromCol1)
        {
            try
            {
                foreach (PSD_MATH_MATRIX_NUM n in matrix)
                {
                    if (cbackground != Color.Transparent) ugTabellone.Rows[n.POSY].Cells[n.POSX + deltaX_fromCol1].Appearance.BackColor = cbackground;
                    if (cforeground != Color.Transparent) ugTabellone.Rows[n.POSY].Cells[n.POSX + deltaX_fromCol1].Appearance.ForeColor = cforeground;
                    ugTabellone.Rows[n.POSY].Cells[n.POSX + deltaX_fromCol1].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                }              
            }
            catch { }
        }

        public void SetColorGridByPos<T>(List<T> matrix, Color cbackground, Color cforeground, int deltaX_fromCol1)
        {
            try
            {
                foreach (T n in matrix)
                {
                    int posX = (n is PSD_MATH_MATRIX_NUM) ? (n as PSD_MATH_MATRIX_NUM).POSX : (n as PSD_MATH_PLANET_NUM).POSX;
                    int posY = (n is PSD_MATH_MATRIX_NUM) ? (n as PSD_MATH_MATRIX_NUM).POSY : (n as PSD_MATH_PLANET_NUM).POSY;
                    if (cbackground != Color.Transparent) ugTabellone.Rows[posY].Cells[posX + deltaX_fromCol1].Appearance.BackColor = cbackground;
                    if (cforeground != Color.Transparent) ugTabellone.Rows[posY].Cells[posX + deltaX_fromCol1].Appearance.ForeColor = cforeground;
                    ugTabellone.Rows[posY].Cells[posX + deltaX_fromCol1].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                }
            }
            catch { }
        }

        private void SetColorCell(UltraGridRow gr, List<PSD_RES> lstgrpbyTMP, string ruota, int N1, string pos, Color c)
        {
            int occorrenze = 0;
            if (lstgrpbyTMP.Where(X => X.RUOTA == ruota && X.NUM_ESTRATTO == N1).Count() > 0)
            {
                occorrenze = lstgrpbyTMP.Where(X => X.RUOTA == ruota && X.NUM_ESTRATTO == N1).First().OCCORRENZE_GROUPBY;

                if (c == Color.Transparent)
                {
                    if (occorrenze <= psdColorTab2.LST_COLORS.Count - 1 && psdColorTab2.LST_COLORS[occorrenze - 1].Item2)
                        gr.Cells[ruota + pos].Appearance.BackColor = psdColorTab2.LST_COLORS[occorrenze - 1].Item1;
                }
                else gr.Cells[ruota + pos].Appearance.BackColor = c;
            }
        }
        private void SetColorCell(UltraGridRow gr, string ruota, int occorrenze, string pos, Color c)
        {
            ///int occorrenze = 0;
            //PSD_ESTR_SUCCESSI estr = lstgrpbyTMP.Where(X => X.Ruota == ruota && X.Numero == N1).FirstOrDefault();
            //if (estr!=null && estr.IDEstrazione > 0)
            {
                //occorrenze = estr.Occorrenze;
                if (c == Color.Transparent)
                {
                    if (occorrenze <= psdColorTab2.LST_COLORS.Count - 1 && psdColorTab2.LST_COLORS[occorrenze - 1].Item2)
                        gr.Cells[ruota + pos].Appearance.BackColor = psdColorTab2.LST_COLORS[occorrenze - 1].Item1;
                }
                else gr.Cells[ruota + pos].Appearance.BackColor = c;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (Convert.ToInt32(timer1.Tag) == 0)
            {
                timer1.Interval = Convert.ToInt32(txtMsecTimer.Text);
                timer1.Start();
                timer1.Tag = 1;
            }
            else {
                timer1.Stop();
                timer1.Tag = 0;
            }
            this.Cursor = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool endgrid = !PlayScreenView(Convert.ToInt32(txtTotRighe.Text));
            if (endgrid)
            {
                timer1.Stop();
                timer1.Tag = 0;
            }
        }

        private void btnCalcRes_Click(object sender, EventArgs e)
        {
            FillSuccessi();
        }

        private void FillSuccessi()
        {
            // evidenzio le decine dei numeri della R1 o dei numeri scelti:
            // serve per vedere come si muovono le decine, ha più senso per i numeri della R1 
            // POSSIBILITA:
            // 1) se fra i numeri scelti per esempio scelgo una decina particolare avrò lo stesso colore in tutto il tabellone:
            //    in questo caso posso vedere come si muove lo sciame della decina
            // 2) scelgo per ogni ruota decine diverse in modo da differenziare il colore:
            //    serve per vedere come si muove lo sciamecol colore che cambia da sx a dx sul tabellone
            if (psdViewParamsNumInGioco1.TYPE_NUM_IN_GIOCO == 1)
                //this.LST_SUCCESSI = DbDataAccess.GetResR1(this.DATA_INIZIO, this.DATA_FINE, COSTANTS.RUOTE.ToList<string>(), APPGlobalInfo.PARAMETRI_R1);
                this.LST_SUCCESSI = DbDataAccess.GetResR1NoDbTable(this.DATA_INIZIO, this.DATA_FINE, COSTANTS.RUOTE.ToList<string>(), APPGlobalInfo.PARAMETRI_R1);
            else this.LST_SUCCESSI = DbDataAccess.GetResNumScelti(this.DATA_INIZIO, this.DATA_FINE, APPGlobalInfo.NUMERI_CASUALI);

            APPGlobalInfo.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(this.LST_SUCCESSI.ToList<object>());           
        }

        private void btnNumInGioco_Click(object sender, EventArgs e)
        {
            //pnlParamsCalc.Visible = !pnlParamsCalc.Visible;
            if(psdViewParamsNumInGioco1.TYPE_NUM_IN_GIOCO == 1)
            {
                Forms.FPSDSceltaR1 f = new Forms.FPSDSceltaR1();
                //f.PARAMETRI_R1 = APPGlobalInfo.PARAMETRI_R1;
                f.Init(APPGlobalInfo.PARAMETRI_R1);
                f.Show();
            }
            else
            {
                Forms.FPSDSceltaNumCasuali f = new Forms.FPSDSceltaNumCasuali();
                f.NUMERI_CASUALI = APPGlobalInfo.NUMERI_CASUALI;
                f.Show();
            }
        }


        private void btnInfoFunzioni_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("Disegna il tabellone delle estrazioni e con i tasti a destra colora le caselle (gli estratti) che rappresentanon i successi dei numeri casuali scelti (o in base alla regola 1 o in base ai numeri scelti manualmente).\n" +
               "Il tabellone si colora in base ai parametri impostati nella sezione a destra relativa ai colori.\n" +
               "Più colori hanno senso in base alla scelta: per esempio colori diversi cioè più occorrenze hanno senso per i numeri scelti con la regola 1, o per la scelta di evidenziare le decina in maniera diversa");
        }

        private void btnInfoParametriCalc_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("Evidenzia i 'successi' sulla griglia in base alla Regola1 o ai numeri scelti");
        }

        private void btnInfoEvidenzia_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("EVIDENZIA: \nColora il tabellone in base alle occorrenze. \n" +
                "Colori diversi hanno questo senso:\n" +
                "   1 - colora in base alle occorrenze della R1: lo stesso numero derivato dalla regola può essere dedotto su colpi diversi" +
                "   2 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n" +
                "   3 - colore 1 per TIPO \n" +

                "   4 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n" +
                "   5 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n" + 
                "\n\nSCORRI:\nMostra consecutivamente blocchi di estrazioni consecutivi in base ai parametri di intervallo d frequenza e nmero di righe del blocco.\n" +
                "L'intento è mostrare visivamente l'evoluzione del sistema come in un film ");
        }


        private void BtnEvidenzia_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            // evidenzio occorrenze (numeri R1 sortiti con successo) con colori diversi: 
            // serve per evidenziare quando la R1 ha funzionato maggiormente
            if (psdViewParamsNumInGioco1.TYPE_NUM_IN_GIOCO == 1)
            {
                SetColorsOccR1();
            }
            else
            {
                SetColorsNumScelti();
            }

            this.Cursor = Cursors.Default;
        }

        private void BtnInfoParametriCalcRis_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("Nel calcolo dei risultati raggruppa le estrazioni: \n  - NUM ESTRAZIONI: per un numero fisso di estrazioni \n  - OCCORRENZE: per un numero di estrazioni che comprende un numero determinato di occorrenze");
        }

        private void BtnDimP_Click(object sender, EventArgs e)
        {
            foreach (UltraGridColumn c in ugTabellone.DisplayLayout.Bands[0].Columns)
            {
                if(c.Width >= 10 && c.Width<=30)
                    c.Width = c.Width + 1;
                if (c.Width < 20 && c.Width >= 18)
                    c.CellAppearance.FontData.SizeInPoints = (float)7.5;
                if (c.Width < 18 && c.Width >= 16)
                    c.CellAppearance.FontData.SizeInPoints = (float)7;
                if (c.Width < 16 && c.Width >= 14)
                    c.CellAppearance.FontData.SizeInPoints = (float)6.5;
                if (c.Width < 14 && c.Width >= 10)
                    c.CellAppearance.FontData.SizeInPoints = (float)6;
            }
        }

        private void BtnDimM_Click(object sender, EventArgs e)
        {
            foreach (UltraGridColumn c in ugTabellone.DisplayLayout.Bands[0].Columns)
            {
                if (c.Width >= 10 && c.Width <= 30)
                    c.Width = c.Width - 1;
                if (c.Width < 20 && c.Width >=18)                
                    c.CellAppearance.FontData.SizeInPoints = (float)7.5;
                if (c.Width < 18 && c.Width >= 16)
                    c.CellAppearance.FontData.SizeInPoints = (float)7;
                if (c.Width < 16 && c.Width >= 14)
                    c.CellAppearance.FontData.SizeInPoints = (float)6.5;
                if (c.Width < 14 && c.Width >= 10)
                    c.CellAppearance.FontData.SizeInPoints = (float)6;
            }
        }
    }
}
