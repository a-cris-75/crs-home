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
    public partial class PSDSceltaNumCasuali : UserControl
    {
        //public List<int> NUMERI_CASUALI = new List<int>();
        public Dictionary<string, List<int>> NUMERI_CASUALI = new Dictionary<string, List<int>>();

        public PSDSceltaNumCasuali()
        {
            InitializeComponent();
        }
        public bool IsSetMode {
            set
            {
                //pnlNum.Enabled = value;
                btnSetNums.Enabled = value;
                btnAllRuote.Enabled = value;
                if (!value)
                {
                    btnVedi.BackColor = Color.Lime;
                    btnImposta.BackColor = Color.Gainsboro;
                    ResetBtnsRuote();
                    ResetBtnsCad();
                    ResetBtnsDec();
                    ResetBtnsNum();
                }
                else
                {
                    btnVedi.BackColor = Color.Gainsboro;
                    btnImposta.BackColor = Color.Lime;
                }
            }
            get { return btnSetNums.Enabled; }
        }
        /// <summary>
        /// restituisce la lista delle ruote selezionate
        /// </summary>
        /// <returns></returns>
        private List<string> GetRuoteSelected()
        {
            List<string> res = new List<string>();
            foreach (Control c in pnlRuote.Controls)
            {
                if (c is Button && c.ForeColor == Color.LimeGreen)
                    res.Add(c.Text);
            }
            return res;
        }

        /// <summary>
        /// in base ai numeri e alle ruote selezionate (nella struttura NUMERI_CASUALI)  mette il colore sul testo del bottone 
        /// per identificarlo come selezionato o no
        /// </summary>
        private void SetBtnNumSelectedFromStruct()
        {
            try
            {
                foreach (string ruota in GetRuoteSelected())
                {
                    foreach (Control c in pnlNum.Controls)
                    {
                        if (c is Button)
                        {
                            if (this.NUMERI_CASUALI.Count>0 && this.NUMERI_CASUALI.Keys.Where(X => X == ruota).Count() > 0)
                                if(this.NUMERI_CASUALI[ruota].Contains(Convert.ToInt32(c.Text)))
                                    c.ForeColor = Color.Red;
                                else c.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch { }
        }

        private void ResetBtnsNum()
        {
            try
            {
                foreach (Control c in pnlNum.Controls)
                {
                    if (c is Button)
                    {
                        c.ForeColor = Color.Black;
                    }
                }              
            }
            catch { }
        }
        
        private void ResetBtnsDec()
        {
            try
            {
                foreach (Control c in pnlDec.Controls)
                {
                    if (c is Button)
                    {
                        c.ForeColor = Color.Black;
                    }
                }              
            }
            catch { }
        }

        private void ResetBtnsCad()
        {
            try
            {
                foreach (Control c in pnlCad.Controls)
                {
                    if (c is Button)
                    {
                        c.ForeColor = Color.Black;
                    }
                }
            }
            catch { }
        }

        private void ResetBtnsRuote()
        {
            try
            {
                foreach (Control c in pnlRuote.Controls)
                {
                    if (c is Button)
                    {
                        c.ForeColor = Color.Black;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// seleziona una singola ruota deselezionando le altre
        /// </summary>
        /// <param name="ruota"></param>
        private void SetBtnRuotaSelected(string ruota)
        {
            //return b.ForeColor == Color.LimeGreen;
            foreach (Control c in pnlRuote.Controls)
            {
                if (c is Button)
                {
                    c.ForeColor = Color.Black;
                    if (c.Text == ruota)
                        if (c.ForeColor != Color.LimeGreen) c.ForeColor = Color.LimeGreen;
                        else c.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// seleziona una ruota assieme alle altre già selezionate
        /// </summary>
        /// <param name="ruota"></param>
        private void SetBtnsRuoteSelected(string ruota)
        {
            //return b.ForeColor == Color.LimeGreen;
            foreach (Control c in pnlRuote.Controls)
            {
                if (c is Button)
                {
                    if (c.Text == ruota)
                        if (c.ForeColor != Color.LimeGreen) c.ForeColor = Color.LimeGreen;
                        else c.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// aggiorna la struttura NUMERI_CASUALI, inserendo , rimuovendo o cambiando il numero della ruota passati come parametri
        /// </summary>
        /// <param name="num"></param>
        /// <param name="ruota"></param>
        /// <param name="ins0rem1change2"></param>
        /// <param name="b"></param>
        private void UpdateNumCasuali(int num, string ruota, int ins0rem1change2)
        {
            //bool isAdded = false;
            try
            {
                if (ins0rem1change2 == 0)
                {
                    if (this.NUMERI_CASUALI.Keys.Where(X => X == ruota).Count() == 0)
                    {
                        List<int> lst = new List<int>();
                        lst.Add(num);
                        this.NUMERI_CASUALI.Add(ruota, lst);
                        //isAdded = true;
                    }
                    else
                    {
                        List<int> lst = this.NUMERI_CASUALI[ruota];
                        if (!lst.Contains(num)) lst.Add(num);
                        //this.NUMERI_CASUALI.Add(ruota
                    }
                }
                if (ins0rem1change2 == 1)
                {
                    if (this.NUMERI_CASUALI.Keys.Where(X => X == ruota).Count() > 0)
                    {
                        List<int> lst = this.NUMERI_CASUALI[ruota];
                        if (lst.Contains(num)) lst.Remove(num);
                    }
                }
                // cambia: se esiste togli altrimenti aggiungi
                if (ins0rem1change2 == 2)
                {
                    if (this.NUMERI_CASUALI.Keys.Where(X => X == ruota).Count() > 0)
                    {
                        List<int> lst = this.NUMERI_CASUALI[ruota];
                        if (lst.Contains(num)) lst.Remove(num);
                        else
                        {
                            lst.Add(num);
                            //isAdded = true;
                        }
                    }
                    else
                    {
                        // non esiste: aggiungi
                        List<int> lst = new List<int>();
                        lst.Add(num);
                        this.NUMERI_CASUALI.Add(ruota, lst);
                        //isAdded = true;
                    }
                }
            }
            catch { }

            //if(isAdded) 
            //    b.ForeColor = Color.Red;
            //else b.ForeColor = Color.Black;
        }

        private void btnNum_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsSetMode)
                {
                    /*int num = Convert.ToInt32((sender as Button).Text);

                    foreach (string ruota in GetRuoteSelected())
                    {
                        UpdateNumCasuali(num, ruota, 2, sender as Button);
                    }*/
                    if ((sender as Button).ForeColor == Color.Black)
                        (sender as Button).ForeColor = Color.Red;
                    else (sender as Button).ForeColor = Color.Black;
                }
            }
            catch { }
            
        }

        private void SetColorSelected(Button b, bool isSel)
        {
            if (this.IsSetMode)
            {
                if (isSel)
                    b.ForeColor = Color.Red;
                else b.ForeColor = Color.Black;
            }
        }

        private void btnCad_Click(object sender, EventArgs e)
        {
            if (this.IsSetMode)
            {
                string num = (sender as Button).Text;
                if (num.Length > 1)
                    num = num.Substring(1);
                bool issel = false;

                if ((sender as Button).ForeColor.ToArgb() == Color.Black.ToArgb())
                {
                    (sender as Button).ForeColor = Color.Yellow;
                    issel = true;
                }
                else (sender as Button).ForeColor = Color.Black;

                foreach (Control c in pnlNum.Controls)
                {
                    if (c is Button)
                    {
                        Button b = (c as Button);
                        string cad = b.Text;
                        if (cad.Length > 1)
                            cad = cad.Substring(1);
                        if (cad == num)
                        {
                            SetColorSelected(b, issel);
                        }
                    }
                }
            }
        }

        private void btnDec_Click(object sender, EventArgs e)
        {
            if (this.IsSetMode)
            {
                string num = (sender as Button).Text;
                bool issel = false;

                if ((sender as Button).ForeColor.ToArgb() == Color.Black.ToArgb())
                {
                    (sender as Button).ForeColor = Color.Yellow;
                    issel = true;
                }
                else (sender as Button).ForeColor = Color.Black;

                foreach (Control c in pnlNum.Controls)
                {
                    if (c is Button)
                    {
                        Button b = (c as Button);
                        string dec = c.Text;
                        if (dec.Length > 1 && dec.Substring(1, 1) != "0")
                            dec = (Convert.ToInt32(dec.Substring(0, 1)) + 1).ToString() + "0";
                        //else dec = "10";
                        if (dec == num)
                        {
                            SetColorSelected(b, issel);
                        }
                    }
                }
            }
        }

        private void btnRuota_Click(object sender, EventArgs e)
        {
            if (this.IsSetMode == false)
            {
                SetBtnRuotaSelected((sender as Button).Text);
                ResetBtnsNum();
                SetBtnNumSelectedFromStruct();
            }
            else SetBtnsRuoteSelected((sender as Button).Text);
        }

        private void SetColorRuoteInit()
        {
            foreach (Control c in pnlRuote.Controls)
            {
                if (c is Button && c.Tag != null && !string.IsNullOrEmpty(Convert.ToString(c.Tag)))
                {
                    if (this.NUMERI_CASUALI.Keys.Where(X => X == (c as Button).Text).Count() > 0 && (this.NUMERI_CASUALI[(c as Button).Text].Count>0))
                    {
                         c.BackColor = Color.LightSteelBlue;
                    }
                    else c.BackColor = Color.Gainsboro;
                }
            }
        }

        private void btnAllRuote_Click(object sender, EventArgs e)
        {
            bool selectall = false;
            if((sender as Button).ForeColor == Color.Black)
            {
                (sender as Button).ForeColor = Color.LimeGreen;
                selectall = true;
            }

            foreach (Control c in pnlRuote.Controls)
            {
                if (c is Button && c.Tag != null && !string.IsNullOrEmpty(Convert.ToString(c.Tag)))
                    if (selectall) c.ForeColor = Color.LimeGreen;
                    else c.ForeColor = Color.Black;
            }
        }

        private void btnVedi_Click(object sender, EventArgs e)
        {
            if (btnVedi.BackColor != Color.Lime)
            {             
                this.IsSetMode = false;
                SetColorRuoteInit();
            }
            else
            {
                this.IsSetMode = true;               
            }
        }

        private void btnSetNums_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.IsSetMode)
                {
                    //
                    foreach (string ruota in GetRuoteSelected())
                    {
                        foreach (Control c in pnlNum.Controls)
                        {
                            if (c is Button)
                            {
                                int num = Convert.ToInt32((c as Button).Text);
                                if((c as Button).ForeColor == Color.Red)
                                    UpdateNumCasuali(num, ruota, 0);                                    
                            }
                        }
                    }
                }
            }
            catch { }
        }
        /*
        private void SetNumSelected(int num, Button b)
        {
            b.ForeColor = Color.Red;
            //(sender as Button).Font = true;
            if (!this.NUMERI_CASUALI.Contains(num))
                this.NUMERI_CASUALI.Add(num);  
        }

        private void SetNumNotSelected(int num, Button b)
        {
            this.NUMERI_CASUALI.Remove(num);
            b.ForeColor = Color.Black;
        }*/

        
    }
}
