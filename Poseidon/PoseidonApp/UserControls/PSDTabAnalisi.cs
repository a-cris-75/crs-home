using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoseidonData.DataEntities;
using PoseidonData;
using Infragistics.Win.UltraWinGrid;
using CRS.CommonControlsLib;
using CRS.Library;
using CRS.DBLibrary;

namespace PoseidonApp.UserControls
{
    public partial class PSDTabAnalisi : UserControl
    {
        public PSDTabAnalisi()
        {
            InitializeComponent();
        }

        public List<PSD_ESTR_SUCCESSI> LST_SUCCESSI = new List<PSD_ESTR_SUCCESSI>();
        public List<PSD_ESTR_LOTTO> LST_ESTRAZIONI = new List<PSD_ESTR_LOTTO>();
        private DataTable GRID_CALC = new DataTable();

        public int GROUPBY_OCCORRENZE
        {
            set { txtGroupByOcc.Text = value.ToString(); }
            get { return Convert.ToInt32(txtGroupByOcc.Text); }
        }

        public List<PSD_MATH_MATRIX_NUM> LST_MATRIX = new List<PSD_MATH_MATRIX_NUM>();
        public PSD_CALCS CALCOLI = new PSD_CALCS();


        /// <summary>
        /// Riempie la griglia dei risultati di sinistra: in questa griglia rappresento per ogni riga una matrice di estrazioni, con data iniziale finale del calcolo,
        /// media occorrenze (successi dei numeri ottenuti in base ad una determinata regola, es R1), laposizione del baricentro cioè il centro di massa della matrice 
        /// in base alle righe e alle colonne.
        /// Prende i valori dalla struttura PSD_RES_TOT
        /// </summary>
        /// <param name="numestrgrp"></param>
        /// <param name="lstsucc"></param>
        /// <param name="lstestr"></param>
        public void FillResByNumEstr(int numestrgrp, List<PSD_ESTR_SUCCESSI> lstsucc, List<PSD_ESTR_LOTTO> lstestr)
        {
            this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(lstestr.ToList<object>());
            this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(lstsucc.ToList<object>());
            List<PSD_RES_TOT> res = PSDCalc.GetResTotGroupByNumEstr(numestrgrp, lstsucc, lstestr);
            bsRES.DataSource = res;
        }

        /// <summary>
        /// Come sopra solo che i risultati vengono calcolati delimitqndo la matrice in base al numero di occorrenze: in altre parole la matrice rappresentante una riga,
        /// è determinata nella sua estensione in base alle occorrenze.
        /// Sopra la dimensione in altezza della matrice (numero di Y) è determinata in maniera fissa, cioè ogni matrice a la stessa altezza 
        /// </summary>
        /// <param name="occgrp"></param>
        /// <param name="lstsucc"></param>
        /// <param name="lstestr"></param>
        public void FillResByOcc(int occgrp, List<PSD_ESTR_SUCCESSI> lstsucc, List<PSD_ESTR_LOTTO> lstestr)
        {
            this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(lstestr.ToList<object>());
            this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(lstsucc.ToList<object>());
            List<PSD_RES_TOT> res = PSDCalc.GetResTotGroupByOcc(occgrp, lstsucc, lstestr);
            bsRES.DataSource = res;
        }

        /// <summary>
        /// Riempi il tabellone 
        /// </summary>
        /// <param name="starsystem"></param>
        /// <param name="minSaturazione"></param>
        /// <param name="distSaturazione"></param>
        /// <param name="typeData0NumEstr1Dist2Sat"></param>
        /// <param name="showOnlySaturi"></param>
        /// <param name="createColumns"></param>
        public void FillTabellone(List<PSD_MATH_STAR_SYSTEM_RES> starsystem, float minSaturazione, int distSaturazione, int numcalc, int typeData0NumEstr1Dist2Sat, bool showOnlySaturi, bool createColumns)
        {
            //bsTabellone.DataSource = DbDataAccess.GetEstrazioni(APPGlobalInfo.DATA_INIZIO, APPGlobalInfo.DATA_FINE);
            //List<PSD_ESTR_LOTTO> lstlotto = new List<PSD_ESTR_LOTTO>();

            //var origin = bsTabellone.DataSource;
            //DataTable dt = new DataTable();

            
            //DataTable dtOrigin = Utils.ToDataTable<PSD_ESTR_LOTTO_SAT>(new List<PSD_ESTR_LOTTO_SAT>());

            DataTable dtOrigin = this.GRID_CALC;
            if(this.GRID_CALC.Rows.Count == 0)
                dtOrigin = Utils.ToDataTable<PSD_ESTR_LOTTO_SAT>(new List<PSD_ESTR_LOTTO_SAT>());


            bsTabellone.DataSource = GetDataTableResStarSystem(dtOrigin, starsystem, 50, numcalc, distSaturazione, minSaturazione, typeData0NumEstr1Dist2Sat, showOnlySaturi, createColumns);
            this.GRID_CALC = (DataTable)bsTabellone.DataSource;
        }

        /// <summary>
        /// Crea un DataTable da passare alla griglia direttamente. Se la griglia è gia formattata createColumns = false.
        /// La struttura deve essere:  DATA, NUMERO ESTR (o in base al significato altro..) BA1..NZ5 
        /// 
        /// </summary>
        /// <param name="starsystem"></param>
        /// <param name="distSaturazione"></param>
        /// <param name="minSaturazione"></param>
        /// <param name="typeData0NumEstr1Dist2Sat"></param>
        /// <param name="showOnlySaturi"></param>
        /// <param name="createColumns"></param>
        /// <returns></returns>
        private DataTable GetDataTableResStarSystem(DataTable dtOrigin, List<PSD_MATH_STAR_SYSTEM_RES> starsystem, int numcols, int numcalc, int distSaturazione, float minSaturazione, int typeData0NumEstr1Dist2Sat, bool showOnlySaturi, bool createColumns)
        {
            // Vorrei creare una griglia in cui le colonne sono i numeri satellite della matrice dei pianeti, sulle righe ci sono le stelle.
            // Nelle caselle della griglia per ogni stella pianeta, trovo il valore della distanza orbitale
            // RISULTATO VISIVO:
            //  1) Dovrei essere in grado di vedere, evidenziandoli, i pianeti vicini rispetto ad una stella
            //  2) se tutti i pianeti sono evidenziati (l'evidenziazione deve essere impostata in base alla distanza) significa che tutti possono essere associati ad una stella vicina
            //  3) quindi dovrebbe valere la regola della minima distanza
            // COME IDENTIFICARE E STELLE E I PIANETI SATELLITE
            //  1) sono numeri associati alle ruote che potrebbero ripetersi in estrazioni diverse
            //  2) sono stringhe la cui univocità è data dalla pos X e Y sul tabellone
            //  3) per distinguere stelle da pianeti è utile usare un prefisso (solo per distinguere intestazioni righe da intestazione colonne)

            // ULTERIORE UTILITA':
            //  1) sulla stessa griglia, dovrei essere in grado di sovrapporre più matrici
            //  2) ne risulterebbe visivamente, UN SOVRAPPORSI di gruppi di saturazione, dove un gruppo di saturazione nella mia terminologia, è un insieme di stella più satelliti
            //  3) OPERATIVAMENTE: 
            //      a. sulla griglia di sx ho le matrici (una per riga) identificate da una data iniziale e finale
            //      b. selziono una mtrice e aggiungo sulla griglia di destra i gruppio di saturazione che si sovrappongono
            //      c. vedo come si copre lo spazio a disposizione (che è dato dalla dimensione della matrice originale)
            //  4) ESTENSIONE AL RAGIONAMENTO:
            //      a. ogni gruppo può essere rappresentato da un numero che è l'indice di saturazione (che rappresenta un valore di massa della stessa associata ai suoi pianeti)
            //      b. così riesco ad occupare una sola casella della matrice: è la stella, con posizione uguale alla sua posizione originale nella matrice di appartenenza, che corrisponde 
            //         al baricentro rispetto all'insieme stella/satelliti, con valore l'indice di saturazione
            //      c. in altre parole: 
            //          - OTTENGO UNA GRIGLIA/MATRICE I CUI ELEMENTI SONO LE VARIE STELLE CHE DA UN GRUPPO DI ESTRAZIONI AL SUCCESSIVO MAN MANO SI SOVRAPPONGONO  
            //          - TALI ELEMENTI (STELLE) HANNO LA CARATTERISTICA DI AVERE UN CERTO INDICE DI SATURAZIONE
            //          - IN QUESTO MODO LIMITO L'INSIEME DEGLI ELEMENTI
            //          - ALL FINE OTTENGO UN LEGAME VERTICALE          IN UNO SPAZIO LEGO INFORMAZIONI OTTENUTE IN TEMPI DIVERSI
            //          -           SPAZIO - TEMPO


            DataTable dt = new DataTable();
            if (dtOrigin != null)
                dt = dtOrigin;

            try
            {

                #region COLONNE = PIANETI
                // costruisco la tabella con le colonne che corrispondono ai pianeti
                // i pianeti sono sempre gli stessi per ogni stella: corrispondono alla matrice 2
                if (createColumns)
                {
                    DataColumn c1 = new DataColumn("DATA", typeof(DateTime));
                    c1.Caption = "DATA";
                    dt.Columns.Add(c1);

                    DataColumn c2 = new DataColumn("ID_ESTRAZIONE", typeof(string));
                    c2.Caption = "ID";
                    dt.Columns.Add(c2);

                    DataColumn c3 = new DataColumn("NUM_CALC", typeof(string));
                    c3.Caption = "NUM CALC";
                    dt.Columns.Add(c3);

                    List<PSD_MATH_PLANET_NUM> lstplanetsOrd = starsystem.First().PLANETS.ToList();

                    //int idx = 1;
                    //foreach (PSD_MATH_PLANET_NUM p in lstplanetsOrd)
                    for(int i = 1; i<=numcols; i++)
                    {
                        // se voglio identificare il nome della colonna con la pos X Y
                        DataColumn cc = new DataColumn("P_" + i.ToString(), typeof(Int32));
                        cc.Caption = "P " + i.ToString();

                        dt.Columns.Add(cc);
                        //idx++;
                    }
                }
                #endregion

                #region GENERA RIGHE PER OGNI ESTRAZIONE SU DATATABLE
                //int numrows = starsystem.Select(x=>x.DATENUM).Distinct().Count();
                /*if (dt.Rows.Count > numrows)
                    numrows = dt.Rows.Count;
                // genero le righe se non esitono già
                //int idxrow = 0;
                if (dt.Rows.Count == 0)
                {
                    for (int idxrow = 0; idxrow < numrows; idxrow++)
                    {
                        DataRow rr = dt.NewRow();
                        //rr[0] = "ESTR " + (idxrow + 1).ToString();
                        //rr[1] = "-- ";
                        rr[1] = idxrow+1;
                        rr[2] = numcalc;
                        dt.Rows.Add(rr);
                    }
                }
                // completo le righe se con i nuovi dati ho un intervallo temporale maggiore
                if (dt.Rows.Count < numrows)
                {
                    for (int idxrow = dt.Rows.Count - 1; idxrow < numrows; idxrow++)
                    {
                        DataRow rr = dt.NewRow();
                        rr[1] = idxrow + 1;
                        rr[2] = numcalc;
                        dt.Rows.Add(rr);
                    }
                }*/
                #endregion

                #region FILTRA STELLE PIANETI
                List<PSD_MATH_STAR_SYSTEM_RES> starsystemFiltered = starsystem.ToList();
                if (showOnlySaturi)
                {
                    starsystemFiltered = starsystem.Where(X => X.Get_INDICE_SATURAZIONE_TO_DIST(distSaturazione) >= minSaturazione).ToList();
                    // 1) scorro solo le stelle i cui pianeti hanno un indice di saturazione voluto
                    // 2) filtro solo i pianeti alla distanza per cui è rispettata la 1)
                    foreach (PSD_MATH_STAR_SYSTEM_RES p in starsystemFiltered)
                    {
                        //int numplanets = p.PLANETS.Where(X => X.DISTANCE_ORBIT <= distSaturazione).Count();
                        //res = res + numplanets / dist;
                        p.PLANETS = p.PLANETS.Where(X => X.DISTANCE_ORBIT <= distSaturazione).ToList();
                    }
                }
                #endregion

                #region RIEMPIE RIGHE = ESTRAZIONI

                List<int> sumcol = new List<int>();
                // per ogni stella aggiunge o scrive nelle celle esistenti (sovrapponendo) i nuovi valori dei gruppi di pianeti saturi 
                foreach (PSD_MATH_STAR_SYSTEM_RES s in starsystemFiltered)
                {
                    int idxrow = s.POSY;
                    //DataRow rr = dt.Rows[idxrow];
                    
                    //if (starsystemFiltered.Where(X => X.ID == s.ID).Count() > 0)
                    {
                        float saturazioneStella = s.Get_INDICE_SATURAZIONE_TO_DIST(distSaturazione);
                        foreach (PSD_MATH_PLANET_NUM p in s.PLANETS.OrderBy(X => X.POSY).ToList())
                        {
                            int idx = p.POSX + 2;
                            int idy = p.POSY;
                            
                            float val = p.DISTANCE_ORBIT;
                            if (typeData0NumEstr1Dist2Sat == 0)
                            {
                                val = 1;
                                if (dt.Rows.Count >= s.POSY && dt.Rows[idxrow] != null && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[idxrow][idx])))
                                    val = Convert.ToInt32(dt.Rows[idxrow][idx]) + 1;
                            }
                            //val = p.NUM;
                            if (typeData0NumEstr1Dist2Sat == 2)
                            {
                                val = 0;
                                if (dt.Rows.Count> idxrow && dt.Rows[idxrow] != null && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[idxrow][idx])))
                                    val = Convert.ToSingle(dt.Rows[idxrow][idx]);
                                val = val + saturazioneStella;//.ToString("0.0");
                            }

                            AddRowsGrid(ref dt, idx, idy, numcalc, val);
                        }
                    }
                }
                #endregion
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;

        }

        // Aggiunge le righe mancanti se necessario
        private void AddRowsGrid(ref DataTable dt, int newidx, int newidy, int numcalc, float val)
        {
            if (dt.Rows.Count <= newidy)
            {
                for (int idy = dt.Rows.Count; idy <= newidy; idy++)
                {
                    DataRow rr = dt.NewRow();
                    rr[1] = idy + 1;
                    rr[2] = numcalc;
                    //rr[newidx] = 0;
                    if (idy == newidy)
                        rr[newidx] = Math.Round( val, 1);
                    dt.Rows.Add(rr);
                }
            }
            else dt.Rows[newidy][newidx] = val;
        }

        private void btnCalcNew_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (rbGroupByNumEstr.Checked)
                this.FillResByNumEstr(Convert.ToInt32(txtGroupByNumEstr.Text), this.LST_SUCCESSI, this.LST_ESTRAZIONI);
            else
                this.FillResByOcc(Convert.ToInt32(txtGroupByOcc.Text), this.LST_SUCCESSI, this.LST_ESTRAZIONI);

            this.Cursor = Cursors.Default;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            bsTabellone.DataSource = new List<PSD_ESTR_LOTTO_SAT>();
            this.GRID_CALC = new DataTable();
        }

        private void btnRicarica_Click(object sender, EventArgs e)
        {
            this.LST_SUCCESSI = Utils.CopyListToList<PSD_ESTR_SUCCESSI>(APPGlobalInfo.LST_SUCCESSI.ToList<object>());
            this.LST_ESTRAZIONI = Utils.CopyListToList<PSD_ESTR_LOTTO>(APPGlobalInfo.LST_ESTRAZIONI.ToList<object>());

            if(APPGlobalInfo.TYPE_CALC_OCC0_ESTR1 == 0)
            {
                this.FillResByOcc(APPGlobalInfo.NUM_OCC_GROUP, this.LST_SUCCESSI, this.LST_ESTRAZIONI);
            }
            else
            {
                this.FillResByNumEstr(APPGlobalInfo.NUM_ESTR_GROUP, this.LST_SUCCESSI, this.LST_ESTRAZIONI);
            }
        }

        private void btnCalcSaturazione_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (bsRES.Current != null && ugResIntervalli.Selected.Rows.Count > 0)
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
                SelectedRowsCollection rows = ugResIntervalli.Selected.Rows;

                int maxDisatOrbitToCalcSat = Convert.ToInt32(txtDistOrb1.Text);
                float indiceSat = Convert.ToSingle(txtSaturazione.Text);

                APPGlobalInfo.PARAMETRI_SEGN_SAT.MAX_DIST_ORBIT_SAT = maxDisatOrbitToCalcSat;
                APPGlobalInfo.PARAMETRI_SEGN_SAT.MIN_IDX_SAT= indiceSat;
                APPGlobalInfo.PARAMETRI_SEGN_SAT.MIN_IDX_SAT_TO_Y = Convert.ToSingle(txtSaturazioneToY.Text);

                List <PSD_RES_TOT_SAT> lstresCalc = new List<PSD_RES_TOT_SAT>();

                DateTime dt1M1 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_DA;
                DateTime dt2M1 = ((PSD_RES_TOT)bsRES.Current).DATA_EVENTO_A.AddDays(1);
                DateTime dt1M2 = dt2M1;
                DateTime dt2M2 = dt2M1.AddDays(1);


                PSD_CALCS calc = this.CALCOLI;
                calc.LST_PARAMETRI_CALCOLO = APPGlobalInfo.GetParamsCalc();
                calc.LST_PARAMETRI_SEGNALE = APPGlobalInfo.GetParamsSegnSat();
                calc.DATE_BEGIN = dt1M1;
                calc.DATE_END = dt2M2;
                PSD_RES_TOT rlast = ((List<PSD_RES_TOT>)bsRES.DataSource).LastOrDefault();
                if (rlast != null) calc.DATE_END = rlast.DATA_EVENTO_A;
                //calc.IDCalcolo = 1;
                //if (APPGlobalInfo.LST_CALCOLI!=null && APPGlobalInfo.LST_CALCOLI.Count>0)
                //    calc.IDCalcolo = APPGlobalInfo.LST_CALCOLI.Max(X => X.IDCalcolo) + 1;

                calc.DESC_PARAMS_SEGN = APPGlobalInfo.GetParamsDesc(calc.LST_PARAMETRI_SEGNALE);
                calc.DESC_PARAMS_CALC = APPGlobalInfo.GetParamsDesc(calc.LST_PARAMETRI_CALCOLO);

                bool exists;
                calc.IDCalcolo = DbDataUpdate.InsertCalcInDB(calc, out exists);

                int idx = 0;
                foreach (UltraGridRow r in ugResIntervalli.Selected.Rows)
                {
                    if (r.ListIndex + 1 < ((List<PSD_RES_TOT>)bsRES.DataSource).Count())
                    {
                        PSD_RES_TOT r1 = ((PSD_RES_TOT)bsRES[r.ListIndex]);
                        dt1M1 = r1.DATA_EVENTO_DA;
                        dt2M1 = r1.DATA_EVENTO_A;
                        PSD_RES_TOT r2 = ((PSD_RES_TOT)bsRES[r.ListIndex + 1]);
                        dt1M2 = r2.DATA_EVENTO_DA;
                        dt2M2 = r2.DATA_EVENTO_A;

                        List<PSD_MATH_MATRIX_NUM> matrix1 = new List<PSD_MATH_MATRIX_NUM>();
                        List<PSD_MATH_MATRIX_NUM> matrix2 = new List<PSD_MATH_MATRIX_NUM>();

                        PSDCalc.GetMatrix(dt1M1, dt2M1, dt1M2, dt2M2, chkParificaOcc.Checked, this.LST_SUCCESSI, out matrix1, out matrix2);
                        List<PSD_MATH_STAR_SYSTEM_RES> res = MathCalc.GetStarSystems(matrix1, matrix2, "M" + idx.ToString(), maxDisatOrbitToCalcSat, 0, 0);

                        FillTabellone(res, indiceSat, maxDisatOrbitToCalcSat, idx + 1, 2, true, false);
                        PSD_RES_TOT_SAT resCalc = PSDCalc.GetResTotSat(Convert.ToSingle(this.txtSaturazione.Text), Convert.ToSingle(this.txtSaturazioneToY.Text)
                            , maxDisatOrbitToCalcSat
                            , Convert.ToInt32(this.txtDistOrbitFromY.Text), idx, res, matrix2, idx, idx + 1, dt1M1, r1.DATA_EVENTO_A, r2.DATA_EVENTO_DA, r2.DATA_EVENTO_A
                            , calc);

                        lstresCalc.Add(resCalc);
                    }
                    idx++;
                }

                calc.DATE_END = dt2M2;
                calc.DESC_RES = "% pianeti dopo Y a max dist sat (su RUOTA): " + ((float)lstresCalc.Sum(X => X.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_STARS_GROUPCOLUMN) / lstresCalc.Count()).ToString("0.00")
                                + "% stelle con succ rispetto a stella senza succ (su RUOTA): " + ((float)lstresCalc.Sum(X => X.PERC_GRPSAT_GREATERTHANSAT_TO_POSY_WITH_PLANETS_DISTSAT_AFTERY) / lstresCalc.Count()).ToString("0.00");
                //this.LST_CALCOLI.Add(calc);
                DbDataUpdate.UpdateResCalcInDB(calc);

                bsResM1M2.DataSource = lstresCalc.ToList();
            }
            else ABSMessageBox.Show("Seleziona una riga da cui cominciare..");

            this.Cursor = Cursors.Default;
        }

        private void BtnInfoOpz_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("RAGGRUPPAMENTI: \nNel calcolo dei risultati raggruppa le estrazioni per:" +
               "\n  - OCCORRENZE: per un numero di estrazioni che comprende un numero determinato di occorrenze" +
               "\n  - NUM ESTRAZIONI: per un numero fisso di estrazioni " +
               "\n    PARIFICA OCCORRENZE: nell'intervallo temporale determinato da NUM ESTRAZIONI elimina le occorrenze della matrice che ne ha di più." +
               "\nServe per il corretto calcolo delle coppie univoche (quando l'algoritmo sarà in grado di determinarle..)"
              );
        }

        private void UgResultsM1M2_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            // 
            try
            {
                DateTime dt1M1 = Convert.ToDateTime(e.Row.Cells["DATA_EVENTO_DA_M1"].Value);
                DateTime dt2M1 = Convert.ToDateTime(e.Row.Cells["DATA_EVENTO_A_M1"].Value);
                DateTime dt1M2 = Convert.ToDateTime(e.Row.Cells["DATA_EVENTO_DA_M2"].Value);
                DateTime dt2M2 = Convert.ToDateTime(e.Row.Cells["DATA_EVENTO_A_M2"].Value);
                //PSD_RES_TOT_SAT 
                Forms.FPSDTabellone f = new Forms.FPSDTabellone();

                List<PSD_MATH_MATRIX_NUM> matrix1 = new List<PSD_MATH_MATRIX_NUM>();
                List<PSD_MATH_MATRIX_NUM> matrix2 = new List<PSD_MATH_MATRIX_NUM>();
                PSDCalc.GetMatrix(dt1M1, dt2M1, dt1M2, dt2M2, chkParificaOcc.Checked, this.LST_SUCCESSI, out matrix1, out matrix2);


                int distSat = Convert.ToInt32(txtDistOrb1.Text);

                float maxvalSatToY = Convert.ToSingle(txtSaturazioneToY.Text);
                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2 = MathCalc.GetStarSystems(matrix1, matrix2, "M1", distSat, 0, 0);
                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2FilteredToY = resM1M2.Where(X => X.INDICE_SATURAZIONE_TO_DIST_TO_POSY >= maxvalSatToY).ToList();
                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2FilteredToY2 = resM1M2.Where(X => X.INDICE_SATURAZIONE_TO_DIST_TO_POSY < maxvalSatToY).ToList();

                f.FillTabellone(dt1M2, dt2M2);
                //f.ColoraTabellone(lstSucc, Color.Yellow);
                // coloro le stelle
                f.ColoraTabelloneByPos(matrix1,1,Color.Transparent, Color.Blue, 2);
                // coloro i pianeti
                f.ColoraTabelloneByPos(matrix2,2, Color.Transparent, Color.Red, 2);

                // coloro le stelle sature fino a y
                List<PSD_MATH_MATRIX_NUM> matrix1Filtered = new List<PSD_MATH_MATRIX_NUM>();

                List<PSD_MATH_MATRIX_NUM> planets = new List<PSD_MATH_MATRIX_NUM>();
                // coloro i pianeti saturi
                foreach (PSD_MATH_STAR_SYSTEM_RES r in resM1M2FilteredToY)
                {
                    planets.AddRange( Utils.CopyListToList< PSD_MATH_MATRIX_NUM > (r.PLANETS.Where(Y => Y.DISTANCE_ORBIT <= r.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > r.POSY).ToList<object>()));
                    PSD_MATH_MATRIX_NUM n = new PSD_MATH_MATRIX_NUM();
                    n.DATENUM = r.DATENUM;
                    n.GROUPCOL = r.GROUPCOL;
                    n.GROUPROW = r.GROUPROW;
                    n.POSGROUPCOL = r.POSGROUPCOL;
                    n.POSX = r.POSX;
                    n.POSY = r.POSY;
                    n.ID = r.ID;
                    matrix1Filtered.Add(n);
                }
                f.ColoraTabelloneByPos<PSD_MATH_MATRIX_NUM>(planets, 3, Color.Yellow, Color.Transparent, 2);

                List<PSD_MATH_MATRIX_NUM> planets2 = new List<PSD_MATH_MATRIX_NUM>(); 
                foreach (PSD_MATH_STAR_SYSTEM_RES r in resM1M2FilteredToY2)
                {
                    planets2.AddRange(Utils.CopyListToList<PSD_MATH_MATRIX_NUM>(r.PLANETS.Where(Y => Y.DISTANCE_ORBIT <= r.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > r.POSY).ToList<object>()));
                    
                   
                }
                f.ColoraTabelloneByPos<PSD_MATH_MATRIX_NUM>(planets2, 4, Color.Orange, Color.Transparent, 2);


                f.ColoraTabelloneByPos<PSD_MATH_MATRIX_NUM>(matrix1Filtered, 5, Color.Transparent, Color.Fuchsia, 2);
                f.Show();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void BtnAddCalcToRes_Click(object sender, EventArgs e)
        {
            if (this.CALCOLI != null && this.CALCOLI.IDCalcolo > 0 && APPGlobalInfo.LST_CALCOLI.Where(X => X.IDCalcolo == this.CALCOLI.IDCalcolo).Count() == 0)
            {
                APPGlobalInfo.LST_CALCOLI.Add(this.CALCOLI);
                //foreach(PSD_ESTR_SEGNALI s in this.CALCOLI.LST_SEGNALI)
                   
            }
        }
    }
}
