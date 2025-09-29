using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PoseidonApp.UserControls
{
    public partial class PSDSceltaR1 : UserControl
    {
        public PSDSceltaR1()
        {
            InitializeComponent();
        }

        /*public PoseidonData.DataEntities.PSD_PARAMETRI_R1 PARAMETRI_R1
        {
            set
            {
                this.POS_DEC_R12 = value.POS_DEC_R12;
                this.TIPO_FREQUENZA = value.TIPO_FREQUENZA;
                this.COLPI = value.COLPI;
            }
            get
            {
                PoseidonData.DataEntities.PSD_PARAMETRI_R1 res = new PoseidonData.DataEntities.PSD_PARAMETRI_R1();
                res.COLPI = new List<int>();
                res.TIPO_FREQUENZA = new List<int>();
                res.POS_DEC_R12 = new List<string>();
                res.COLPI = this.COLPI;
                res.TIPO_FREQUENZA = this.TIPO_FREQUENZA;
                res.POS_DEC_R12 = this.POS_DEC_R12;
                return res;
            }
        }*/


        public List<string> POS_DEC_R12
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Control c in gbPosDec.Controls)
                {
                    if(c is CheckBox && Convert.ToInt32(c.Tag) == 1 && (c as CheckBox).Checked)
                    {
                        string s1 = c.Text;
                        foreach (Control cc in gbPosDec.Controls)
                        {
                            if (cc is CheckBox && Convert.ToInt32(cc.Tag) == 2 && (cc as CheckBox).Checked)
                            {
                                res.Add(s1 + cc.Text);
                            }                           
                        }
                    }
                }
                return res;
            }

            set
            {
                foreach (Control c in gbPosDec.Controls)
                {
                    if (c is CheckBox) (c as CheckBox).Checked = false;
                }
                foreach (string s in value)
                {
                    string s1 = s.Substring(0, 1);
                    string s2 = s.Substring(1, 1);
                    foreach (Control c in gbPosDec.Controls)
                    {
                        if (c is CheckBox && Convert.ToInt32(c.Tag) == 1 && c.Text == s1)
                        {
                            (c as CheckBox).Checked = true;
                        }
                        if (c is CheckBox && Convert.ToInt32(c.Tag) == 2 && c.Text == s2)
                        {
                            (c as CheckBox).Checked = true;
                        }
                    }
                }
            }
        }

        public List<int> COLPI
        {
            get
            {
                List<int> res = new List<int>();
                foreach (Control c in gbColpi.Controls)
                {
                    if (c is CheckBox && (c as CheckBox).Checked)
                    {
                        res.Add(Convert.ToInt32(c.Text));
                    }
                }
                return res;
            }
            set
            {
                foreach (Control c in gbColpi.Controls)
                {
                    if (c is CheckBox && value.Contains(Convert.ToInt32(c.Text)))
                    {
                        (c as CheckBox).Checked = true;
                    }
                }
            }
        }

        public List<int> TIPO_FREQUENZA
        {
            get
            {
                List<int> res = new List<int>();
                foreach (Control c in gbTipoFreq.Controls)
                {
                    if (c is CheckBox && (c as CheckBox).Checked)
                    {
                        res.Add(Convert.ToInt32(c.Tag));
                        /*if (Convert.ToInt32(c.Tag) == 12)
                        {
                            res.Add(1);
                            res.Add(2);
                        }
                        if (Convert.ToInt32(c.Tag) == 34)
                        {
                            res.Add(3);
                            res.Add(4);
                        }*/
                    }
                }
                return res;
            }
            set
            {
                if (value.Contains(1)) //|| value.Contains(2))
                    chkTipoFreqDir.Checked = true;
                if (value.Contains(3)) //|| value.Contains(4))
                    chkTipoFreqInd.Checked = true;

                if (value.Contains(2))
                    chkTipoFreqDir2.Checked = true;
                if (value.Contains(4))
                    chkTipoFreqInd2.Checked = true;
            }
        }

        public List<string> ACCOPPIATE_DEC_R12
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Control c in gbAccDecR1R2.Controls)
                {
                    if (c is CheckBox && (c as CheckBox).Checked)
                    {
                        res.Add(c.Text);
                    }
                }
                return res;
            }

            set
            {
                foreach (Control c in gbAccDecR1R2.Controls)
                {
                    if (c is CheckBox) (c as CheckBox).Checked = false;
                }
                foreach (string s in value)
                {
                    foreach (Control c in gbAccDecR1R2.Controls)
                    {
                        if (c is CheckBox && c.Text == s)
                        {
                            (c as CheckBox).Checked = true;
                        }
                    }
                }
            }
        }


        private void ChkPosR11_Click(object sender, EventArgs e)
        {
            bool issel = (sender as CheckBox).Checked;
            chkPosR11.Checked = false;
            chkPosR12.Checked = false;
            chkPosR13.Checked = false;
            chkPosR14.Checked = false;
            chkPosR15.Checked = false;
            (sender as CheckBox).Checked = issel;

        }

        private void ChkPosR21_Click(object sender, EventArgs e)
        {
            bool issel = (sender as CheckBox).Checked;
            chkPosR21.Checked = false;
            chkPosR22.Checked = false;
            chkPosR23.Checked = false;
            chkPosR24.Checked = false;
            chkPosR25.Checked = false;
            (sender as CheckBox).Checked = issel;

        }
    }
}
