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
    public partial class PSDViewParamsNumInGioco : UserControl
    {
        private int type_num_in_Gioco = 1;
        private PSD_PARAMETRI_R1 PARAMETRI_R1 = new PSD_PARAMETRI_R1();
        private Dictionary<string, List<int>> NUMERI_CASUALI = new Dictionary<string, List<int>>();
        public  int TYPE_NUM_IN_GIOCO
        {
            set
            {
                type_num_in_Gioco = value;
                if (value == 1)
                {
                    btnPlayNumCasuali.BackColor = SystemColors.Control;
                    btnPlayR1.BackColor = Color.Lime;
                    lblNumInGiocoTitle.Text = "NUMERI RICAVATI DALLA R1";
                    lblNumInGiocoDesc.Text =
                        "QTA NUM IN GIOCO per ESTRAZIONE e per RUOTA: " + GetQtaNumInGioco().ToString() + 
                        GetRegola1Desc();
                    lblParamsTitle.Text = "Parametri";
                    toolTip1.SetToolTip(lblNumInGiocoDesc, GetRegola1Desc());
                }
                else
                {
                    btnPlayNumCasuali.BackColor = Color.Lime;
                    btnPlayR1.BackColor = SystemColors.Control; 
                    lblNumInGiocoTitle.Text = "NUMERI SCELTI MANUALMENTE";
                    lblParamsTitle.Text = "";
                    string txt = "";
                    string txtQTA = "";

                    int idx = 0;
                    foreach (KeyValuePair<string,List<int>> k in APPGlobalInfo.NUMERI_CASUALI)
                    {
                        if (k.Value.Count > 0)
                        {
                            if(!string.IsNullOrEmpty(txt))
                                txt = txt + "\n" + k.Key + ": " + string.Join(",", k.Value);
                            else txt = k.Key + ": " + string.Join(",", k.Value);

                            string acapo = "";
                            if (idx == 6) acapo = "\n";
                            if (!string.IsNullOrEmpty(txtQTA))
                                txtQTA = txtQTA + ", " + acapo + k.Key + ": " + k.Value.Count.ToString();
                            else txtQTA = k.Key + ": " + k.Value.Count.ToString();
                        }
                        idx++;
                    }

                    lblNumInGiocoDesc.Text =
                        "QTA NUM IN GIOCO tot: " + GetQtaNumInGioco() + 
                        "\nQTA NUM IN GIOCO per RUOTA: " + txtQTA;
                    toolTip1.SetToolTip(lblNumInGiocoDesc, txt);
                }
            }
            get { return type_num_in_Gioco; }
        }

        public PSDViewParamsNumInGioco()
        {
            InitializeComponent();
        }

        public void Init(PSD_PARAMETRI_R1 PARR1, Dictionary<string, List<int>> NUMCASUALI)
        {
            this.PARAMETRI_R1 = PARR1;
            this.NUMERI_CASUALI = NUMCASUALI;
        }

        private int GetQtaNumInGioco()
        {
            int res = 0;
            if (this.TYPE_NUM_IN_GIOCO == 1)
            {
                if (PARAMETRI_R1.TIPO_FREQUENZA.Count() == 1)
                {
                    res = PARAMETRI_R1.COLPI.Count;
                }
                else
                //if (PARAMETRI_R1.TIPO_FREQUENZA.Count() == 4)
                {
                    res = PARAMETRI_R1.COLPI.Count * 2;
                }

                // se considero i diretti o solo gli indiretti i num in gioco sono i colpi
                /*if(PARAMETRI_R1.TIPO_FREQUENZA.Count() == 2)
                {
                    res = PARAMETRI_R1.COLPI.Count;
                }
                if (PARAMETRI_R1.TIPO_FREQUENZA.Count() == 4)
                {
                    res = PARAMETRI_R1.COLPI.Count * 2;
                }*/
            }
            else
            {
                res = NUMERI_CASUALI.Sum(X => X.Value.Count);
            }
            return res;
        }

        private string GetRegola1Desc()
        {
            string res ="\n   - DECINE: " + string.Join(",", PARAMETRI_R1.POS_DEC_R12);
            res = res + "\n   - COLPI: " + string.Join(",", PARAMETRI_R1.COLPI);
            res = res + "\n   - TIPO FREQ: ";
            if (PARAMETRI_R1.TIPO_FREQUENZA.Contains(1) || PARAMETRI_R1.TIPO_FREQUENZA.Contains(2))
                res = res + " DIRETTI";
            if (PARAMETRI_R1.TIPO_FREQUENZA.Contains(3) || PARAMETRI_R1.TIPO_FREQUENZA.Contains(4))
                res = res + " INDIRETTI";
            return res;

        }

        private void BtnPlayNumCasuali_Click(object sender, EventArgs e)
        {
            Forms.FPSDSceltaNumCasuali f = new Forms.FPSDSceltaNumCasuali();
            f.NUMERI_CASUALI = new Dictionary<string, List<int>>();
            f.NUMERI_CASUALI = APPGlobalInfo.NUMERI_CASUALI;
            DialogResult r = f.ShowDialog();
            if (r == DialogResult.OK)
            {
                this.Init(new PSD_PARAMETRI_R1(), APPGlobalInfo.NUMERI_CASUALI);
                this.TYPE_NUM_IN_GIOCO = 2;
            }
        }

        private void BtnPlayR1_Click(object sender, EventArgs e)
        {

            Forms.FPSDSceltaR1 f = new Forms.FPSDSceltaR1();
           
            f.Init(APPGlobalInfo.PARAMETRI_R1);
            DialogResult r = f.ShowDialog();
            if (r == DialogResult.OK)
            {
                this.Init(APPGlobalInfo.PARAMETRI_R1, new Dictionary<string, List<int>>());
                this.TYPE_NUM_IN_GIOCO = 1;
            }
        }

        private void BtnInfoParametriCalcVis_Click(object sender, EventArgs e)
        {
            ABSMessageBox.Show("Evidenzia i 'successi' sulla griglia in base alla REGOLA 1 o ai NUMERI SCELTI" +
                "\n\nI numeri della regola 1 sono ricavati dalle estrazioni precedenti in base ad alcuni parametri: " +
                "\n - DECINA DEL NUMERO IN POSIZIONE 1-5: ottengo il numero unendo la decina del numero in posizione N della prima ruota dell'accoppiata (BA-CA, FI-GE..) col secondo numero dell'accoppiata" +
                "\n - TIPO FREQUENZA: se DIR (diretto) il numero ottenuto è DEC prima ruota + DEC seconsa ruota, IND (indiretto) viceversa" +
                "\n - COLPO: il numero ottenuto proviene dall'ennesima (colpo) estrazione precedente" +
                "\n\nNB: incrociando i parametri i numeri giocati in base alla regola vanno da un min di 1 a un max di 6)"+
                "\n\nI numeri scelti sono dei numeri fissi scelti dal tabellone, quindi vanno da 1 a 90"
                );
        }
    }
}
