using Crs.Base.CommonControlsLibrary;
using Crs.Home.ApocalypsData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Crs.Home.ApocalypsApp.UserControls
{
    public partial class PSDColorTab : UserControl
    {
        public PSDColorTab()
        {
            InitializeComponent();
        }

        public int TIPO_EVIDENZA
        {
            set {
                if (value == COSTANTS.TIPO_EVIDENZA_1_OCCORRENZE) rbOcc.Checked = true;
                if (value == COSTANTS.TIPO_EVIDENZA_2_DECINE_SCELTI) rbDecR1NumScelti.Checked = true;
                if (value == COSTANTS.TIPO_EVIDENZA_3_TIPO_R1) rbTipo.Checked = true;
                if (value == COSTANTS.TIPO_EVIDENZA_4_NUM_SCELTI) rbNumScelti.Checked = true;
                if (value == COSTANTS.TIPO_EVIDENZA_5_DECINE_ESTR) rbDecAllEstr.Checked = true;
            }
            get {
                if (rbOcc.Checked) return COSTANTS.TIPO_EVIDENZA_1_OCCORRENZE;
                if (rbDecR1NumScelti.Checked) return COSTANTS.TIPO_EVIDENZA_2_DECINE_SCELTI;
                if (rbTipo.Checked) return COSTANTS.TIPO_EVIDENZA_3_TIPO_R1;
                if (rbNumScelti.Checked) return COSTANTS.TIPO_EVIDENZA_4_NUM_SCELTI;
                if (rbDecAllEstr.Checked) return COSTANTS.TIPO_EVIDENZA_5_DECINE_ESTR;
                return 0;
            }
        }

        /// <summary>
        /// Lista dei colori associata, in base alla scelta, a: 
        ///     1) occorrenze di successo dei numeri
        ///     2) numero previsto estratto
        ///     3) tipologia di previsione (es: per la R1 dir ind colpo 1 2 3)
        ///     4) decina del numero previsto estratto
        /// sul tabellone per ogni occorrenza è dedidcato un numero
        /// </summary>
        public List<Tuple<Color, bool>> LST_COLORS
        {
            get {
                List<Color> colors = new List<Color>();
                List<Control> ctrl = new List<Control>();
                List<CheckBox> chk = new List<CheckBox>();
                List< Tuple < Color, bool>> res = new List< Tuple < Color, bool>> ();
                foreach (Control C in gbColors.Controls)
                {
                    if(C is Panel && C.Tag != null)
                    {
                        ctrl.Add(C);
                    }
                    if (C is CheckBox && C.Tag != null)
                    {
                        chk.Add(C as CheckBox);
                    }
                }

                ctrl = ctrl.OrderBy(X => Convert.ToInt32(X.Tag)).ToList();
                chk = chk.OrderBy(X => Convert.ToInt32(X.Tag)).ToList();
                foreach (Control c in ctrl)
                {
                    Tuple<Color, bool> t = new Tuple<Color, bool>(c.BackColor, chk.Where(X => X.Tag == c.Tag).First().Checked);
                    res.Add(t);
                }
                return res;
            }

        }

        private void chkAll_Click(object sender, EventArgs e)
        {
            foreach (Control C in gbColors.Controls)
            {
                if (C is CheckBox && C.Tag != null)
                {
                    (C as CheckBox).Checked = chkAll.Checked;
                }
            }

        }

        private void rbOcc_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in gbColors.Controls)
            {
                c.Enabled = true;
                if (c is CheckBox) c.Text = "";
            }
            if (rbOcc.Checked)
            {
                foreach (Control c in gbColors.Controls)
                {
                    if (Convert.ToInt32(c.Tag) > 6)
                    {
                        c.Enabled = false;
                        if (c is CheckBox)
                            (CheckBox)c.Checked = false;
                    }

                }
            }
            if (rbTipo.Checked) {
                foreach (Control c in gbColors.Controls)
                {
                    if (Convert.ToInt32(c.Tag) == 2 || Convert.ToInt32(c.Tag) == 3 || Convert.ToInt32(c.Tag) == 5 || Convert.ToInt32(c.Tag) == 6 || Convert.ToInt32(c.Tag) == 8 || Convert.ToInt32(c.Tag) == 9)
                    {
                        if (c is CheckBox) { 
                            if (Convert.ToInt32(c.Tag) == 2) c.Text = "Colpo 1 Dir";
                            if (Convert.ToInt32(c.Tag) == 3) c.Text = "Colpo 1 Ind";
                            if (Convert.ToInt32(c.Tag) == 5) c.Text = "Colpo 2 Dir";
                            if (Convert.ToInt32(c.Tag) == 6) c.Text = "Colpo 2 Ind";
                            if (Convert.ToInt32(c.Tag) == 8) c.Text = "Colpo 3 Dir";
                            if (Convert.ToInt32(c.Tag) == 9) c.Text = "Colpo 3 Ind";
                        }
                    }
                    else
                    {
                        c.Enabled = false;
                        if (c is CheckBox)
                            (CheckBox)c.Checked = false;
                    }
                }
            }
            if (rbNumScelti.Checked) { }
            if (rbDecR1NumScelti.Checked)
            {
                foreach (Control c in gbColors.Controls)
                {
                    if (Convert.ToInt32(c.Tag) > 10)
                    {
                        c.Enabled = false;
                        if (c is CheckBox)
                            (c as CheckBox).Checked = false;
                    }
                    else
                    {
                        if (c is CheckBox) c.Text = c.Tag.ToString() + "0";
                    }
                }
            }
            if (rbDecAllEstr.Checked)
            {
                foreach (Control c in gbColors.Controls)
                {
                    if (Convert.ToInt32(c.Tag) > 10)
                    {
                        c.Enabled = false;
                        if (c is CheckBox)
                            (c as CheckBox).Checked = false;
                    }
                    else
                    {
                        if (c is CheckBox) c.Text = c.Tag.ToString() + "0";
                    }

                }
            }

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            CrsMessageBox.Show("Colora il tabellone in base alle occorrenze. \n" +
                "Colori diversi hanno questo senso:\n" +
                "   1 - colora in base alle occorrenze della R1: lo stesso numero derivato dalla regola può essere dedotto su colpi diversi" +
                "   2 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n" +
                "   3 - colore 1 per TIPO \n" +

                "   4 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n" +
                "   5 - colore 1 per decina 1-9, colore 2 per decina 10-19 ..\n");
        }
    }
}
