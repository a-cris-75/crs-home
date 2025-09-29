using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PoseidonData.DataEntities;
using CRS.CommonControlsLib;

namespace PoseidonApp.UserControls
{
    public partial class PSDStarSystem : UserControl
    {
        public PSDStarSystem()
        {
            InitializeComponent();
        }

        private List<PSD_MATH_STAR_SYSTEM_RES> STAR_SYSTEMS = new List<PSD_MATH_STAR_SYSTEM_RES>();
        public List<PSD_MATH_MATRIX_NUM> MATRIX1 = new List<PSD_MATH_MATRIX_NUM>();
        public List<PSD_MATH_MATRIX_NUM> MATRIX2 = new List<PSD_MATH_MATRIX_NUM>();

        #region LAYOUT
        public bool SHOW_HEADER
        {
            set { pnlHeader.Visible = value; }
            get { return pnlHeader.Visible; }
        }

      
        #endregion

        #region METHODS FILL CALC
        public void FillStarSystem(List<PSD_MATH_MATRIX_NUM>m1, List<PSD_MATH_MATRIX_NUM> m2, bool ordMinDistTotPlanet, bool ordMinDistPlanetTot, int maxdistSat1, int maxdistSat2, int maxdistSat3)
        {
            this.MATRIX1 = m1;
            this.MATRIX2 = m2;
            this.STAR_SYSTEMS = MathCalc.GetStarSystems(m1, m2, "M1", maxdistSat1,maxdistSat2,maxdistSat3);
            bsRes.DataSource = this.STAR_SYSTEMS;
           
            if (ordMinDistPlanetTot)
            {
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Min(y => y.DISTANCE_ORBIT)).ToList();
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Sum(y => y.DISTANCE_ORBIT)).ToList();
            }
            if (ordMinDistTotPlanet)
            {
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Sum(y => y.DISTANCE_ORBIT)).ToList();
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Min(y => y.DISTANCE_ORBIT)).ToList();
            }
            // rappresentazione su matrice: su colonne i pianeti e sulle righe le stelle
            bsResMatrixStarPlanetDist.DataSource = GetDataTableResStarSystem(this.STAR_SYSTEMS, ordMinDistTotPlanet/*, ordMinDistTotPlanet || ordMinDistPlanetTot*/);

            try
            {
                lblMatrix1.Text = "MATRICE 1: " + m1.First().DATENUM.ToShortDateString() + " - " + m1.Last().DATENUM.ToShortDateString();
                lblMatrix2.Text = "MATRICE 2: " + m2.First().DATENUM.ToShortDateString() + " - " + m2.Last().DATENUM.ToShortDateString();
            }
            catch { }

            List<PSD_MATH_PLANET_NUM> lstMinDistTot = new List<PSD_MATH_PLANET_NUM>();
            List<Tuple<string, int, int>> lstResMinDis = GetPlanetsMinDist(out lstMinDistTot);

            DataTable dt = (DataTable)bsResPlanetsMinDist.DataSource;
            bsResPlanetsMinDist.DataSource = GetDataTableResMinDist(dt, 0, lstResMinDis, 1);//lstResMinDis;
            txtNumPlanets.Text = m1.Count().ToString();
            txtNumMinDistUnivoci.Text = lstResMinDis.Where(X => X.Item2 == 1).Count().ToString();
            txtNumMinDistNonUnivoci.Text = lstResMinDis.Where(X => X.Item2 > 1).Count().ToString();
            txtNumMinDistNonAccoppiati.Text = (m1.Count() - lstResMinDis.Where(X => X.Item2 == 1).Count() - lstResMinDis.Where(X => X.Item2 > 1).Count()).ToString();
        }

        /// <summary>
        /// L'intento è proporre un risultato leggibile per mostrare come sono posizionati i pianeti satellite intorno ad ogni stella della matrice delle stelle
        /// Quindi sulle colonne avremo la lista dei pianeti (matrice 2), sulle righe la lista delle stelle (matrice 1), ogni casella della matrice rappresenta la distanza 
        /// stella - pianeta (riga - colonna)
        /// </summary>
        public static DataTable GetDataTableResStarSystem(List<PSD_MATH_STAR_SYSTEM_RES> starsystem, bool orderByMinDist)
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
            DataTable dt = new DataTable();
            
            try
            {
                // costruisco la tabelle con le colonne che corrispondono ai pianeti
                // i pianeti sono sempre gli stessi per ogni stella: corrispondono alla matrice 2
                //dt.Columns.Add("STAR", "STAR");
                DataColumn c = new DataColumn("STAR", typeof(string));
                c.Caption = "POS X-Y";
                dt.Columns.Add(c);

                List<PSD_MATH_PLANET_NUM> lstplanetsOrd = starsystem.First().PLANETS.ToList();
                // il primo elemento è la stella i cui primi pianeti sono alla minima distanza e la somma delle distanze è minima
                //if(orderByMinDist) lstplanetsOrd = starsystem.First().PLANETS.OrderBy(x => x.DISTANCE_ORBIT).ToList();

                int idx = 1;
                foreach (PSD_MATH_PLANET_NUM p in lstplanetsOrd)
                {
                    // se voglio identificare il nome della colonna con la pos X Y
                    // dt.Columns.Add("P_" + p.POSX.ToString() + "_" + p.POSY.ToString(), "PLANET " + p.POSX.ToString() + "-" + p.POSY.ToString());
                    //dt.Columns.Add("P_" + idx.ToString(), "PLANET " + p.POSX.ToString() + "-" + p.POSY.ToString());
                    DataColumn cc = new DataColumn("P_" + idx.ToString(), typeof(Int32));
                    cc.Caption = "P " + p.ID;

                    if (orderByMinDist) cc.Caption = idx.ToString();

                    dt.Columns.Add(cc);
                    idx++;
                }

                // COLONNA SOMMA RIGHE
                DataColumn csum = new DataColumn("SUM_DIST" , typeof(Int32));
                csum.Caption = "SUM DIST ";
                dt.Columns.Add(csum);

                DataColumn cnump = new DataColumn("NUM_STARS_MIN_DIST", typeof(Int32));
                cnump.Caption = "NUM STAR MIN DIST ";
                dt.Columns.Add(cnump);

                List<int> sumcol = new List<int>();
                // per ogni stella aggiungi 2 colonne:
                //  1) col totale delle distanze 
                //  2) col numero di pianeti alla minima distanza 
                foreach (PSD_MATH_STAR_SYSTEM_RES s in starsystem)
                {
                    DataRow rr = dt.NewRow();
                    rr[0] = "STAR " + s.ID;//s.POSX.ToString() + "-" + "(" + s.POSGROUPCOL + ")";
                    idx = 1;
                    int sumdist = 0;
                    int numpmindist = 0;

                    int mindist = s.PLANETS.Min(X => X.DISTANCE_ORBIT);
                    if (orderByMinDist) s.PLANETS = s.PLANETS.OrderBy(X => X.DISTANCE_ORBIT).ToList();
                    foreach (PSD_MATH_PLANET_NUM p in s.PLANETS)
                    {
                        rr[idx] = p.DISTANCE_ORBIT;
                        idx++;

                        sumdist = sumdist + p.DISTANCE_ORBIT;
                        if (p.DISTANCE_ORBIT == mindist)
                            numpmindist++;
                    }
                    rr[idx] = sumdist;
                    rr[idx+1] = numpmindist;
                    dt.Rows.Add(rr);
                }

                DataRow rsum = dt.NewRow();
                DataRow rnumsmindist = dt.NewRow();
                // per ogni pianeta (colonna) aggiungi due righe:
                //  1) una con le somme delle distanze
                //  2) una con il numero di stelle alla minima distanza
                for (int idxcol = 1; idxcol < dt.Columns.Count - 1;  idxcol++)
                {
                    rsum[idxcol] = 0;
                    int mindist = 1000;
                    foreach (DataRow v in dt.Rows)
                    {
                        rsum[idxcol] = Convert.ToInt32(rsum[idxcol]) + Convert.ToInt32(v[idxcol]);
                        if (mindist > Convert.ToInt32(v[idxcol])) mindist = Convert.ToInt32(v[idxcol]);
                    }
                    foreach (DataRow v in dt.Rows)
                    {
                        if(Convert.ToInt32(v[idxcol]) == mindist)
                            rnumsmindist[idxcol] = Convert.ToInt32(rnumsmindist[idxcol]) + 1;
                    }
                }
                dt.Rows.Add(rsum);
                dt.Rows.Add(rnumsmindist);
            }
            catch { }

            return dt;

        }

        /// <summary>
        /// DA FARE: VEDI NOTE
        /// - per semplificare dovrei passare starSystem e planetSystem:
        ///     - dove planet system è la struttura risultante dell'estrazione successiva rispetto alla precedente
        ///     - starSystem è la struttura dell'estrazione rispetto alla successiva 
        /// Aggiunge una riga colonna alla matrice indicando il numero di pianeti su riga e stelle su colonna finali che distano alla prima distanza maggiore di
        /// minDistFrom: serve per verificare in calcoli successivi le altre soluzioni degli accoppiamenti.
        /// Se per esempio per una minima distanza ho più soluzioni, oppure se un pianeta potrebbe essere associato ad un'altra stella salla medesima distanza vorrei
        /// sapere qual'è la prossima alternativa
        /// </summary>
        /// <param name="starsystem"></param>
        /// <param name="dtMatrix"></param>
        /// <param name="miDisatFrom"></param>
        /// <returns></returns>
        public static DataTable AddMinDistColToDataTableResStarSystem(List<PSD_MATH_STAR_SYSTEM_RES> starsystem, DataTable dtMatrix, int minDistFrom)
        {
           
            try
            {
               
                //DataRow rsum = dtMatrix.NewRow();
                DataRow rnumsmindist = dtMatrix.NewRow();
                for (int idxcol = 1; idxcol < dtMatrix.Columns.Count - 1; idxcol++)
                {
                    //rsum[idxcol] = 0;
                    int mindist = 1000;
                    foreach (DataRow v in dtMatrix.Rows)
                    {
                        //rsum[idxcol] = Convert.ToInt32(rsum[idxcol]) + Convert.ToInt32(v[idxcol]);
                        if (mindist > Convert.ToInt32(v[idxcol])) mindist = Convert.ToInt32(v[idxcol]);
                    }
                    foreach (DataRow v in dtMatrix.Rows)
                    {
                        if (Convert.ToInt32(v[idxcol]) == mindist)
                            rnumsmindist[idxcol] = Convert.ToInt32(rnumsmindist[idxcol]) + 1;
                    }
                }
                //dtMatrix.Rows.Add(rsum);
                dtMatrix.Rows.Add(rnumsmindist);
            }
            catch { }

            return dtMatrix;

        }

        /// <summary>
        /// Dovrebbe rappresentare una tabella con tutte le elaborazioni successive rappresentate sulle colonne
        /// Sulle righe trovo l'ID_PLANET che rappresenta i valori relativi ad un pianeta della matrice 2
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="numelab"></param>
        /// <param name="lstres"></param>
        /// <param name="typeOrderID0MinDist1NumStar2"></param>
        /// <returns></returns>
        public static DataTable GetDataTableResMinDist( DataTable dt, int numelab, List<Tuple<string, int, int>> lstres, int typeOrderID0MinDist1NumStar2 = 1)
        {
            // lstres contiene: ID pianeta, num stelle a cui è vicino ad una minima distaza 
            //DataTable dt = new DataTable();

            try
            {
                // costruisco la tabelle strutturata così:
                //  1) COLONNE: 
                //      - id planet
                //      - per ogni elaborazione: num stars alla minima distanza
                //      - valore minima istanza
                //  2) RIGHE:
                //      - in base all'elaborazione compilo la colonna relativa con valori: num stelle alla min dist, valore min dist

                if (!dt.Columns.Contains("ID_PLANET"))
                { 
                    DataColumn c1 = new DataColumn("ID_PLANET", typeof(string));
                    c1.Caption = "ID PLANET " + numelab.ToString();
                    c1.Unique = true;
                    dt.Columns.Add(c1);
                }
                DataColumn c2 = new DataColumn("NUM_STARS_" + numelab.ToString(), typeof(Int32));
                c2.Caption = "NUM STARS vicine " + numelab.ToString();
                dt.Columns.Add(c2);
                DataColumn c3 = new DataColumn("DIST_ORB_" + numelab.ToString(), typeof(Int32));
                c3.Caption = "DIST orb " + numelab.ToString();
                dt.Columns.Add(c3);

                List<Tuple<string, int, int>> lstplanetsOrd = lstres.OrderBy(X => X.Item2).ToList() ;
                if (typeOrderID0MinDist1NumStar2 == 0) lstplanetsOrd = lstres.OrderBy(X => X.Item1).ToList();
                if (typeOrderID0MinDist1NumStar2 == 2) lstplanetsOrd = lstres.OrderBy(X => X.Item3).ToList();

                foreach (Tuple<string, int, int> r in lstplanetsOrd)
                {
                    int idxcol = numelab * 2 + 1;
                    if (!dt.Rows.Contains(r.Item1))
                    {
                        DataRow newrow = dt.NewRow();
                        newrow[idxcol - 1] = r.Item1;
                        newrow[idxcol] = r.Item2;
                        newrow[idxcol + 1] = r.Item3;
                        dt.Rows.Add(newrow);
                    }
                    else
                    {
                        DataRow rr = dt.Rows.Find(r.Item1);
                        rr[idxcol] = r.Item2;
                        rr[idxcol + 1] = r.Item3;
                    }
                }
            }
            catch { }

            return dt;

        }

        private void FillOrdered(bool ordMinDistStarPlanet
               //, bool ordMinDistTotPlanet
               , int maxdistSat1, int maxdistSat2, int maxdistSat3
               )
        {
            this.STAR_SYSTEMS = MathCalc.GetStarSystems(this.MATRIX1, this.MATRIX2, "M1", maxdistSat1, maxdistSat2, maxdistSat3);
            bsRes.DataSource = this.STAR_SYSTEMS;

            if (ordMinDistStarPlanet)
            {
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Min(y => y.DISTANCE_ORBIT)).ToList();
                this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Sum(y => y.DISTANCE_ORBIT)).ToList();
            }
            //if (ordMinDistTotPlanet)
            //{
            //    this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Sum(y => y.DISTANCE_ORBIT)).ToList();
            //    this.STAR_SYSTEMS = this.STAR_SYSTEMS.OrderBy(x => x.PLANETS.Min(y => y.DISTANCE_ORBIT)).ToList();
            //}

            bsResMatrixStarPlanetDist.DataSource = GetDataTableResStarSystem(this.STAR_SYSTEMS, ordMinDistStarPlanet);

            try
            {
                lblMatrix1.Text = "MATRICE 1: " + this.MATRIX1.First().DATENUM.ToShortDateString() + " - " + this.MATRIX1.Last().DATENUM.ToShortDateString();
                lblMatrix2.Text = "MATRICE 2: " + this.MATRIX2.First().DATENUM.ToShortDateString() + " - " + this.MATRIX2.Last().DATENUM.ToShortDateString();
            }
            catch { }
        }

        /// <summary>
        /// Determino i pianeti alla minima distanza unici.
        /// Serve per capire quante soluzioni univoche trovo, cioè quanti pianeti sono vicini a una sola stella in modo da poter fare una scelta univoca
        /// Restituisce una lista con l'ID del planet, il numero di stelle diverse a cui si trova alla minima distanza, e la minima distanza
        /// </summary>
        public List<Tuple<string, int, int>> GetPlanetsMinDist(out List<PSD_MATH_PLANET_NUM> lstMinDistTot)
        {
            lstMinDistTot = new List<PSD_MATH_PLANET_NUM>();
            // determino la lista per ogni STAR dei pianeti alla minima distanza
            // ottengo una lista di pianeti che possono ripetersi se sono vicini a più stelle
            // se nella lista ho una sola riga per un pianeta questo sarà l'unico vicino ad una stella
            foreach (PSD_MATH_STAR_SYSTEM_RES row in this.STAR_SYSTEMS)
            {
                List<PSD_MATH_PLANET_NUM> lstMinDist = row.Get_PLANETS_MIN_DIST();
                foreach (PSD_MATH_PLANET_NUM md in lstMinDist)
                {
                    // se nella lista il planet è già presente ma ha una distanza maggiore, eliminalo e inserisci nella lista quello appena trovato
                    if (lstMinDistTot.Where(X => X.ID == md.ID && X.DISTANCE_ORBIT > md.DISTANCE_ORBIT).Count() > 0)
                    {
                        lstMinDistTot.RemoveAll(X => X.POSGROUPCOL == md.POSGROUPCOL && X.DISTANCE_ORBIT > md.DISTANCE_ORBIT);
                        lstMinDistTot.Add(md);
                    }
                    else lstMinDistTot.Add(md);
                }
                lstMinDistTot.AddRange(lstMinDist);              
            }
            // raggruppo per id così ottengo per ogni pianeta il numero di stelle a cui è più vicino in egual misura
            var group = lstMinDistTot.GroupBy(X => new { X.ID, X.DISTANCE_ORBIT });
            List<Tuple<string, int, int>> res = new List<Tuple<string, int, int>>();
            foreach (var num in group)
            {
                res.Add(new Tuple<string, int, int>(num.Key.ID, num.Count(),num.Key.DISTANCE_ORBIT));
            }
            return res;
        }

        /*private void ColoraCoppie()
        {
            if (bsRes.Current != null)
            {
                PSD_MATH_STAR_SYSTEM_GRID s = (PSD_MATH_STAR_SYSTEM_GRID)bsRes.Current;
                for (int i = 0; i < ugCoppie.Rows.Count; i++)
                {
                    if (ugCoppie.Rows[i].Cells["STARNUM_ID"].Value != null && Convert.ToString(ugCoppie.Rows[i].Cells["STARNUM_ID"].Value) == s.ID)
                    {
                        ugCoppie.Rows[i].Appearance.BackColor = Color.Aqua;
                    }
                    else
                        ugCoppie.Rows[i].Appearance.BackColor = Color.Transparent;
                }
            }

        }*/

        #endregion

        #region EVENTS CTRL

        private void bsRes_PositionChanged(object sender, EventArgs e)
        {
            if (bsRes.Current != null)
            {
                PSD_MATH_STAR_SYSTEM_RES s = (PSD_MATH_STAR_SYSTEM_RES)bsRes.Current;
                if (this.STAR_SYSTEMS.Where(X => X.POSX == s.POSX && X.POSY == s.POSY && X.STARNUM == s.STARNUM).Count() > 0)
                {
                    int maxdistSat = Convert.ToInt32(txtMaxDistSatToShow.Text);
                    if(chkShowPlanetsMaxDistSat.Checked)
                        bsResPlanets.DataSource = this.STAR_SYSTEMS.Where(X => X.POSX == s.POSX && X.POSY == s.POSY && X.STARNUM == s.STARNUM).First()
                            .PLANETS.Where(Y=>Y.DISTANCE_ORBIT<= maxdistSat)
                            .OrderBy(Z=>Z.DISTANCE_ORBIT).ToList();
                    else
                        bsResPlanets.DataSource = this.STAR_SYSTEMS.Where(X => X.POSX == s.POSX && X.POSY == s.POSY && X.STARNUM == s.STARNUM).First()
                            .PLANETS
                            .OrderBy(Z=>Z.DISTANCE_ORBIT);
                    List<PSD_MATH_PLANET_NUM> lst = new List<PSD_MATH_PLANET_NUM>();
                    int maxdist2 = Convert.ToInt32(txtMaxDistSat1.Text);
                    lst = (this.STAR_SYSTEMS.Where(X => X.ID == s.ID).First().Get_PLANETS_TO_DIST(maxdist2));
                    bsResPlanetsLiv12.DataSource = lst;
                    bsResPlanetsLiv12.ResumeBinding();
                    chtPianetiMinDist.DataBind();
                }

                //this.ColoraCoppie();
            }
        }

        private void ugSolarSystemPlanets_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells["DISTANCE_ORBIT"].Value != null)
            {
                if (Convert.ToInt32(e.Row.Cells["DISTANCE_ORBIT"].Value) < Convert.ToInt32(txtDistOrb1.Text))
                    e.Row.Cells["DISTANCE_ORBIT"].Appearance.BackColor = pnlColorDistOrb1.BackColor;
                else
                if (Convert.ToInt32(e.Row.Cells["DISTANCE_ORBIT"].Value) < Convert.ToInt32(txtDistOrb2.Text))
                    e.Row.Cells["DISTANCE_ORBIT"].Appearance.BackColor = pnlColorDistOrb2.BackColor;
                else
                if (Convert.ToInt32(e.Row.Cells["DISTANCE_ORBIT"].Value) < Convert.ToInt32(txtDistOrb3.Text))
                    e.Row.Cells["DISTANCE_ORBIT"].Appearance.BackColor = pnlColorDistOrb3.BackColor;
                else
                    e.Row.Cells["DISTANCE_ORBIT"].Appearance.BackColor = pnlColorDistOrb4.BackColor;
            }
        }

        private void ugMatrixStarPlanet_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            int idx = 1;
            // scorro tutte le colonne
            foreach (PSD_MATH_PLANET_NUM p in this.STAR_SYSTEMS.First().PLANETS)
            {
                string namecol = "P_" + idx.ToString();
                //if (e.Row.Cells.Contains(p.POSX.ToString()+"_"+p.POSY.ToString()))
                if(rbColorDist.Checked)
                {
                    if (chkShowColor1.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value )<= Convert.ToInt32( txtDistOrb1.Text))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb1.BackColor;
                    if (chkShowColor2.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) > Convert.ToInt32(txtDistOrb1.Text) && Convert.ToInt32(e.Row.Cells[namecol].Value) <= Convert.ToInt32(txtDistOrb2.Text))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb2.BackColor;
                    if (chkShowColor3.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) > Convert.ToInt32(txtDistOrb2.Text) && Convert.ToInt32(e.Row.Cells[namecol].Value) <= Convert.ToInt32(txtDistOrb3.Text))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb3.BackColor;
                    if (chkShowColor4.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) > Convert.ToInt32(txtDistOrb3.Text) && Convert.ToInt32(e.Row.Cells[namecol].Value) <= Convert.ToInt32(txtDistOrb4.Text))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb4.BackColor;
                }
                else
                {
                    // colora la cella se il pianeta è del primo, secondo,.. livello
                    if (chkShowColor1.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) == this.STAR_SYSTEMS[e.Row.Index].Get_DIST_PLANETS_LEVEL(0))
                    //if (chkShowColor1.Checked && this.STAR_SYSTEMS[e.Row.Index].Get_DIST_PLANETS_LEVEL(0).)
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb1.BackColor;
                    if (chkShowColor2.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) == this.STAR_SYSTEMS[e.Row.Index].Get_DIST_PLANETS_LEVEL(1))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb2.BackColor;
                    if (chkShowColor3.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) == this.STAR_SYSTEMS[e.Row.Index].Get_DIST_PLANETS_LEVEL(2))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb3.BackColor;
                    if (chkShowColor4.Checked && Convert.ToInt32(e.Row.Cells[namecol].Value) == this.STAR_SYSTEMS[e.Row.Index].Get_DIST_PLANETS_LEVEL(3))
                        e.Row.Cells[namecol].Appearance.BackColor = pnlColorDistOrb4.BackColor;
                }
                idx++;
            }
        }

        private void btnZoomP_Click(object sender, EventArgs e)
        {
            ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth = ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth + 3;

        }

        private void btnZoomM_Click(object sender, EventArgs e)
        {
            if(ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth > 5)
                ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth = ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth - 3;

            if (ugMatrixStarPlanet.DisplayLayout.Override.DefaultRowHeight > 5)
                ugMatrixStarPlanet.DisplayLayout.Override.DefaultRowHeight = ugMatrixStarPlanet.DisplayLayout.Override.DefaultRowHeight - 3;
        }

        private void btnZoomReset_Click(object sender, EventArgs e)
        {
            ugMatrixStarPlanet.DisplayLayout.Override.DefaultColWidth = 20;
            ugMatrixStarPlanet.DisplayLayout.Override.DefaultRowHeight = -1;
        }
       
        private void btnOrderStarMinDistTotPlanet_Click(object sender, EventArgs e)
        {
            int maxdistSat1 = Convert.ToInt32(txtMaxDistSat1.Text);
            int maxdistSat2 = Convert.ToInt32(txtMaxDistSat2.Text);
            int maxdistSat3 = Convert.ToInt32(txtMaxDistSat3.Text);
            FillOrdered(true, maxdistSat1, maxdistSat2, maxdistSat3);

        }

        private void btnOrderStarMinDistPlanetTot_Click(object sender, EventArgs e)
        {
            int maxdistSat1 = Convert.ToInt32(txtMaxDistSat1.Text);
            int maxdistSat2 = Convert.ToInt32(txtMaxDistSat2.Text);
            int maxdistSat3 = Convert.ToInt32(txtMaxDistSat3.Text);
            FillOrdered(false,maxdistSat1, maxdistSat2,maxdistSat3);
        }     

        private void lblInfo_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show(txtInfo.Text); 
        }

        private void lblHeaderCoppieNonUnivoche_Click(object sender, EventArgs e)
        {
            

        }

        private void lblHeaderPianetiSistemaSolare_Click(object sender, EventArgs e)
        {
            if (pnlGridPlanets.Height > 25)
                pnlGridPlanets.Height = 25;
            else pnlGridPlanets.Height = 250;

        }

        private void btnCalcMinDistUnivoche_Click(object sender, EventArgs e)
        {
            //FillGridCalcCoppieAlgorithmOnlyUnivoche();
        }

        private void rbColorDist_Click(object sender, EventArgs e)
        {
            if (rbColorDist.Checked)
            {
                /*txtDistOrb1.Text = "2";
                txtDistOrb2.Text = "4";
                txtDistOrb3.Text = "6";
                txtDistOrb4.Text = "8";*/
                txtDistOrb1.Enabled = true;
                txtDistOrb2.Enabled = true;
                txtDistOrb3.Enabled = true;
                txtDistOrb4.Enabled = true;
            }
            else
            {
                /*txtDistOrb1.Text = "1";
                txtDistOrb2.Text = "2";
                txtDistOrb3.Text = "3";
                txtDistOrb4.Text = "4";*/
                txtDistOrb1.Enabled = false;
                txtDistOrb2.Enabled = false;
                txtDistOrb3.Enabled = false;
                txtDistOrb4.Enabled = false;
            }
        }

        private void btnShowResAcc_Click(object sender, EventArgs e)
        {
            pnlCoppieResults.Visible = !pnlCoppieResults.Visible;
        }
        #endregion

        private void btnInfoFunzioni_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("GRIGLIA:\nRappresenta la lista delle stelle (eventi della matrice 1, confrontati con i pianeti della matrice 2)" +
                "\n\nVALORI: " +
                "\n1) per ogni stella ho i valori indicanti le distanze da tutti i pianeti " +
                "\n2) per ogni stella ho i valori indicanti le minime distanze e la saturazione " +
                "\n\nSATURAZIONE: valore indicante il numero e la distanza della stella dai pianeti secondo la formula => numplanets / dist ."+
                "\n  Ha senso entro una certa distanza (livello)." + 
                "\n\nNB: il livello e la distanza sono due cose dicerse: "+
                "\nLIVELLO: indica il numero dell'orbitale occupato dal pianeta" + 
                "\nDISTANZA: indica la distanza della stella dal pianeta (un orbitale di livello 1 ha una certa distanza) "

                );

        }

        private void btnRecalcDistMaxSat_Click(object sender, EventArgs e)
        {
            int maxdistSat1 = Convert.ToInt32(txtMaxDistSat1.Text);
            int maxdistSat2 = Convert.ToInt32(txtMaxDistSat2.Text);
            int maxdistSat3 = Convert.ToInt32(txtMaxDistSat3.Text);
            this.FillStarSystem(this.MATRIX1, this.MATRIX2, false, false, maxdistSat1, maxdistSat2, maxdistSat3);
        }
    }
}
