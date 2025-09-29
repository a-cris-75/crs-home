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
    public partial class PSDAccoppiamenti : UserControl
    {
        public PSDAccoppiamenti()
        {
            InitializeComponent();
        }

        private List<PSD_MATH_STAR_SYSTEM_RES> STAR_SYSTEMS = new List<PSD_MATH_STAR_SYSTEM_RES>();
        public List<PSD_MATH_MATRIX_NUM> MATRIX1 = new List<PSD_MATH_MATRIX_NUM>();
        public List<PSD_MATH_MATRIX_NUM> MATRIX2 = new List<PSD_MATH_MATRIX_NUM>();

      
        public bool SHOW_HEADER_BTNS
        {
            set
            {
                pnlHeaderBtns.Visible = value;
                //pnlHeader.Height = (pnlHeaderOptionsLegend.Visible && pnlHeaderBtns.Visible) ? 110 : (pnlHeaderOptionsLegend.Visible || pnlHeaderBtns.Visible) ? 55 : 0;
            }
            get { return pnlHeaderBtns.Visible; }
        }


        public int PARAM_MINDIST1
        {
            set { txtDist1.Text = Convert.ToString(value); }
            get { return Convert.ToInt32(txtDist1.Text); }
        }

        public int PARAM_MINDIST2
        {
            set { txtDist2.Text = Convert.ToString(value); }
            get { return Convert.ToInt32(txtDist2.Text); }
        }


        #region ALGORITMI RICERCA COPPIE
        /// <summary>
        /// Usa MathCalc.CalcolaCoppie2
        /// da testare
        /// </summary>
        public void FillGridCalcCoppieAlgorithm2()
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstres = MathCalc.CalcolaCoppie2(this.STAR_SYSTEMS);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrd = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrdNU = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES r in lstres)
            {
                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID g = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID();
                g.DISTANCE = r.PLANET.DISTANCE;
                g.DISTANCE_ORBIT = r.PLANET.DISTANCE_ORBIT;
                g.STARNUM_NUM = r.STARNUM.NUM;
                g.STARNUM_POSX = r.STARNUM.POSX;
                g.STARNUM_POSY = r.STARNUM.POSY;
                g.STARNUM_DATENUM = r.STARNUM.DATENUM;
                g.PLANET_NUM = r.PLANET.NUM;
                g.PLANET_POSX = r.PLANET.POSX;
                g.PLANET_POSY = r.PLANET.POSY;
                g.PLANET_DATENUM = r.PLANET.DATENUM;
                g.IS_COPPIA_UNIVOCA = r.IS_COPPIA_UNIVOCA;
                //g.INDEX_COPPIA_MIGLIOR_DISTANZA = r.INDEX_COPPIA_MIGLIOR_DISTANZA;
                g.NUM_CALC = r.NUM_CALC;
                g.STARNUM_POSGROUPCOL = r.STARNUM.POSGROUPCOL;
                g.PLANET_POSGROUPCOL = r.PLANET.POSGROUPCOL;
                g.STARNUM_ID = r.STARNUM.ID;
                g.PLANET_ID = r.PLANET.ID;

                if (g.IS_COPPIA_UNIVOCA)
                    lstresgrd.Add(g);
                else lstresgrdNU.Add(g);
            }

            bsResCoppie.DataSource = lstresgrd;
            bsResCoppieEqDist.DataSource = lstresgrdNU;
            FillResultsTxt(lstres);
        }

        /// <summary>
        /// Usa MathCalc.CalcolaCoppieParam e riporta i risultati in griglia 
        /// </summary>
        public void FillGridCalcCoppieAlgorithmParam(int mindistMin, int mindistMax)
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstres = MathCalc.CalcolaCoppieUnivocheMinDistParam(this.STAR_SYSTEMS, mindistMin, mindistMax);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrd = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrdNU = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES r in lstres)
            {
                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID g = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID();
                g.DISTANCE = r.PLANET.DISTANCE;
                g.DISTANCE_ORBIT = r.PLANET.DISTANCE_ORBIT;
                g.STARNUM_NUM = r.STARNUM.NUM;
                g.STARNUM_POSX = r.STARNUM.POSX;
                g.STARNUM_POSY = r.STARNUM.POSY;
                g.STARNUM_DATENUM = r.STARNUM.DATENUM;
                g.PLANET_NUM = r.PLANET.NUM;
                g.PLANET_POSX = r.PLANET.POSX;
                g.PLANET_POSY = r.PLANET.POSY;
                g.PLANET_DATENUM = r.PLANET.DATENUM;
                g.IS_COPPIA_UNIVOCA = r.IS_COPPIA_UNIVOCA;
                //g.INDEX_COPPIA_MIGLIOR_DISTANZA = r.INDEX_COPPIA_MIGLIOR_DISTANZA;
                g.NUM_CALC = r.NUM_CALC;
                g.STARNUM_POSGROUPCOL = r.STARNUM.POSGROUPCOL;
                g.PLANET_POSGROUPCOL = r.PLANET.POSGROUPCOL;
                g.STARNUM_ID = r.STARNUM.ID;
                g.PLANET_ID = r.PLANET.ID;

                if (g.IS_COPPIA_UNIVOCA)
                    lstresgrd.Add(g);
                else lstresgrdNU.Add(g);
            }

            bsResCoppie.DataSource = lstresgrd;
            bsResCoppieEqDist.DataSource = lstresgrdNU;
            FillResultsTxt(lstres);
        }

        public void FillGridCalcCoppieAlgorithmOnlyUnivoche()
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstres = MathCalc.CalcolaCoppieUnivocheAtMinDist(this.STAR_SYSTEMS);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrd = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrdNU = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES r in lstres)
            {
                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID g = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID();
                g.DISTANCE = r.PLANET.DISTANCE;
                g.DISTANCE_ORBIT = r.PLANET.DISTANCE_ORBIT;
                g.STARNUM_NUM = r.STARNUM.NUM;
                g.STARNUM_POSX = r.STARNUM.POSX;
                g.STARNUM_POSY = r.STARNUM.POSY;
                g.STARNUM_DATENUM = r.STARNUM.DATENUM;
                g.PLANET_NUM = r.PLANET.NUM;
                g.PLANET_POSX = r.PLANET.POSX;
                g.PLANET_POSY = r.PLANET.POSY;
                g.PLANET_DATENUM = r.PLANET.DATENUM;
                g.IS_COPPIA_UNIVOCA = r.IS_COPPIA_UNIVOCA;
                //g.INDEX_COPPIA_MIGLIOR_DISTANZA = r.INDEX_COPPIA_MIGLIOR_DISTANZA;
                g.NUM_CALC = r.NUM_CALC;
                g.STARNUM_POSGROUPCOL = r.STARNUM.POSGROUPCOL;
                g.PLANET_POSGROUPCOL = r.PLANET.POSGROUPCOL;
                g.STARNUM_ID = r.STARNUM.ID;
                g.PLANET_ID = r.PLANET.ID;

                if (g.IS_COPPIA_UNIVOCA)
                    lstresgrd.Add(g);
                else lstresgrdNU.Add(g);
            }

            bsResCoppie.DataSource = lstresgrd;
            bsResCoppieEqDist.DataSource = lstresgrdNU;

            FillResultsTxt(lstres);
        }

        private void FillResultsTxt(List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstres)
        {
            //FillRes(lstres)

            List<Tuple<string, int>> res = new List<Tuple<string, int>>();
            /*foreach (var num in group)
            {
                res.Add(new Tuple<string, int, int>(num.Key.ID, num.Count(), num.Key.DISTANCE_ORBIT));
            }*/
            var groupstelle = lstres.GroupBy(X => new { X.STARNUM.ID });
            //int numstelledistinteacc = groupstelle.Count();
            //int numpianetidistintiacc = lstres.GroupBy(X => new { X.PLANET.ID }).Count();

            foreach (var num in groupstelle)
            {
                res.Add(new Tuple<string, int>(num.Key.ID, num.Count()));
            }
            int numstelleconpiupianeti = res.Where(X => X.Item2 > 1).Count();
            int numstelleconunpianeta = res.Where(X => X.Item2 == 1).Count();

            var grouppianeti = lstres.GroupBy(X => new { X.PLANET.ID });
            res.Clear();
            foreach (var num in grouppianeti)
            {
                res.Add(new Tuple<string, int>(num.Key.ID, num.Count()));
            }
            int numnpianeticonpiustelle = res.Where(X => X.Item2 > 1).Count();
            int numnpianeticonunastella = res.Where(X => X.Item2 == 1).Count();

            List<PSD_MATH_STAR_SYSTEM_RES> tmp = this.STAR_SYSTEMS.ToList();
            int numstarnonacc = tmp.RemoveAll(X => lstres.Select(Y => Y.STARNUM.ID).Distinct().Contains(X.ID));
            int numcoppiedistinte = lstres.Select(X => new Tuple<string, string>(X.STARNUM.ID, X.PLANET.ID)).Distinct().Count();


            txtResults.AppendText("NUM STELLE ACCOPPIATE (univoche e non): " + groupstelle.Count().ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("NUM PIANETI ACCOPPIATI (univoci e non): " + grouppianeti.Count().ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("COPPIE UNIVOCHE DISTINTE: " + numcoppiedistinte.ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("NUM STELLE ACCOPPIATE (CON 1 PIANETA): " + numstelleconunpianeta.ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("NUM PIANETI ACCOPPIATI (CON 1 STELLA): " + numnpianeticonunastella.ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("COPPIE NON UNIVOCHE");
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("NUM STELLE CON PIU' PIANETI: " + numstelleconpiupianeti.ToString());
            txtResults.AppendText(Environment.NewLine);
            txtResults.AppendText("NUM PIANETI CON PIU' STELLE: " + numnpianeticonpiustelle.ToString());

            DataTable dt = new DataTable();
            DataColumn cnump = new DataColumn("RISULTATO", typeof(string));
            cnump.Caption = "RISULTATO";
            dt.Columns.Add(cnump);
            DataColumn csum = new DataColumn("VALORE", typeof(Int32));
            csum.Caption = "VALORE";
            dt.Columns.Add(csum);

            DataRow rr = dt.NewRow();
            rr[0] = "NUM STELLE ACCOPPIATE (univoche e non)";
            rr[1] = groupstelle.Count();
            dt.Rows.Add(rr);

            rr = dt.NewRow();
            rr[0] = "NUM PIANETI ACCOPPIATI (univoci e non)";
            rr[1] = grouppianeti.Count();

            rr = dt.NewRow();
            rr[0] = "COPPIE UNIVOCHE DISTINTE: ";
            rr[1] = numcoppiedistinte;
            dt.Rows.Add(rr);
            rr = dt.NewRow();
            rr[0] = "NUM STELLE ACCOPPIATE (CON 1 PIANETA): ";
            rr[1] = numstelleconunpianeta;
            dt.Rows.Add(rr);
            rr = dt.NewRow();
            rr[0] = "NUM PIANETI ACCOPPIATI (CON 1 STELLA): ";
            rr[1] = numnpianeticonunastella;
            dt.Rows.Add(rr);
            rr = dt.NewRow();
            rr[0] = "COPPIE NON UNIVOCHE";
            dt.Rows.Add(rr);
            rr = dt.NewRow();
            rr[0] = "NUM STELLE CON PIU' PIANETI: ";
            rr[1] = numstelleconpiupianeti;
            dt.Rows.Add(rr);
            rr = dt.NewRow();
            rr[0] = "NUM PIANETI CON PIU' STELLE: ";
            rr[1] = numnpianeticonpiustelle;
            dt.Rows.Add(rr);

            ugRes.DataSource = dt;
            //ugRes.DisplayLayout.Override.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            ugRes.DisplayLayout.Bands[0].Columns[0].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left;
            ugRes.DisplayLayout.Bands[0].Columns[0].Width = 250;
        }

        /// <summary>
        /// Usa MathCalc.CalcolaCoppie
        /// </summary>
        public void FillGridCalcCoppieAlgorithm1()
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstCoppieEqDist;
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstres = MathCalc.CalcolaCoppie(this.STAR_SYSTEMS, out lstCoppieEqDist);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrd = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID> lstresgrdNU = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID>();
            foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES r in lstres)
            {
                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID g = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID();
                g.DISTANCE = r.PLANET.DISTANCE;
                g.DISTANCE_ORBIT = r.PLANET.DISTANCE_ORBIT;
                g.STARNUM_NUM = r.STARNUM.NUM;
                g.STARNUM_POSX = r.STARNUM.POSX;
                g.STARNUM_POSY = r.STARNUM.POSY;
                g.STARNUM_DATENUM = r.STARNUM.DATENUM;
                g.PLANET_NUM = r.PLANET.NUM;
                g.PLANET_POSX = r.PLANET.POSX;
                g.PLANET_POSY = r.PLANET.POSY;
                g.PLANET_DATENUM = r.PLANET.DATENUM;
                g.IS_COPPIA_UNIVOCA = r.IS_COPPIA_UNIVOCA;
                //g.INDEX_COPPIA_MIGLIOR_DISTANZA = r.INDEX_COPPIA_MIGLIOR_DISTANZA;
                g.STARNUM_POSGROUPCOL = r.STARNUM.POSGROUPCOL;
                g.PLANET_POSGROUPCOL = r.PLANET.POSGROUPCOL;
                g.STARNUM_ID = r.STARNUM.ID;
                g.PLANET_ID = r.PLANET.ID;

                lstresgrd.Add(g);
            }
            foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES r in lstCoppieEqDist)
            {
                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID g = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES_GRID();
                g.DISTANCE = r.PLANET.DISTANCE;
                g.DISTANCE_ORBIT = r.PLANET.DISTANCE_ORBIT;
                g.STARNUM_NUM = r.STARNUM.NUM;
                g.STARNUM_POSX = r.STARNUM.POSX;
                g.STARNUM_POSY = r.STARNUM.POSY;
                g.STARNUM_DATENUM = r.STARNUM.DATENUM;
                g.PLANET_NUM = r.PLANET.NUM;
                g.PLANET_POSX = r.PLANET.POSX;
                g.PLANET_POSY = r.PLANET.POSY;
                g.PLANET_DATENUM = r.PLANET.DATENUM;
                g.IS_COPPIA_UNIVOCA = r.IS_COPPIA_UNIVOCA;
                //g.INDEX_COPPIA_MIGLIOR_DISTANZA = r.INDEX_COPPIA_MIGLIOR_DISTANZA;
                g.STARNUM_POSGROUPCOL = r.STARNUM.POSGROUPCOL;
                g.PLANET_POSGROUPCOL = r.PLANET.POSGROUPCOL;
                g.STARNUM_ID = r.STARNUM.ID;
                g.PLANET_ID = r.PLANET.ID;

                lstresgrdNU.Add(g);
            }

            bsResCoppie.DataSource = lstresgrd;
            bsResCoppieEqDist.DataSource = lstresgrdNU;
            FillResultsTxt(lstres);
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
                res.Add(new Tuple<string, int, int>(num.Key.ID, num.Count(), num.Key.DISTANCE_ORBIT));
            }
            return res;
        }

        private void ColoraCoppie(PSD_MATH_STAR_SYSTEM_RES stella)
        {
            //if (bsRes.Current != null)
            {
                //PSD_MATH_STAR_SYSTEM_GRID s = (PSD_MATH_STAR_SYSTEM_GRID)bsRes.Current;
                for (int i = 0; i < ugCoppie.Rows.Count; i++)
                {
                    if (ugCoppie.Rows[i].Cells["STARNUM_ID"].Value != null && Convert.ToString(ugCoppie.Rows[i].Cells["STARNUM_ID"].Value) == stella.ID)
                    {
                        ugCoppie.Rows[i].Appearance.BackColor = Color.Aqua;
                    }
                    else
                        ugCoppie.Rows[i].Appearance.BackColor = Color.Transparent;
                }
            }

        }

        #endregion
        private void btnCalcMinDistUnivoche_Click(object sender, EventArgs e)
        {
            FillGridCalcCoppieAlgorithmOnlyUnivoche();
        }

        private void btnCalcMinDistUnivocheParams_Click(object sender, EventArgs e)
        {
            FillGridCalcCoppieAlgorithmParam(Convert.ToInt32(txtDist1.Text), Convert.ToInt32(txtDist2.Text));
        }

        private void btnCalcolaMinDist1_Click(object sender, EventArgs e)
        {
            FillGridCalcCoppieAlgorithm1();
        }

        private void btnCalcolaMinDist2_Click(object sender, EventArgs e)
        {
            FillGridCalcCoppieAlgorithm2();
        }

        private void lblHeaderCoppieNonUnivoche_Click(object sender, EventArgs e)
        {
            ugCoppieEqDist.Visible = !ugCoppieEqDist.Visible;
        }
    }
}
