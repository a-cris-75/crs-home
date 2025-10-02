using Crs.Home.ApocalypsData.DataEntities;

namespace Crs.Home.ApocalypsApp
{
    partial class PageAnalisi
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContenuto;
        private System.Windows.Forms.DataGridView grigliaRisultati;
        private System.Windows.Forms.GroupBox groupBoxRaggruppamento;
        private System.Windows.Forms.RadioButton radioSettimana;
        private System.Windows.Forms.RadioButton radioMese;
        private System.Windows.Forms.RadioButton radioTrimestre;
        private System.Windows.Forms.RadioButton radioAnno;
        private System.Windows.Forms.GroupBox groupBoxBudget;
        private System.Windows.Forms.NumericUpDown numBudget;
        private System.Windows.Forms.Button btnAggiorna;
        private System.Windows.Forms.Button btnEsporta;
        private System.Windows.Forms.Label lblTotali;



        //------------------------
        // Aggiungi questi controlli all'header:

        private System.Windows.Forms.GroupBox groupBoxRuote;
        private System.Windows.Forms.CheckBox chkBari;
        private System.Windows.Forms.CheckBox chkCagliari;
        private System.Windows.Forms.CheckBox chkFirenze;
        private System.Windows.Forms.CheckBox chkGenova;
        private System.Windows.Forms.CheckBox chkMilano;
        private System.Windows.Forms.CheckBox chkNapoli;
        private System.Windows.Forms.CheckBox chkPalermo;
        private System.Windows.Forms.CheckBox chkRoma;
        private System.Windows.Forms.CheckBox chkTorino;
        private System.Windows.Forms.CheckBox chkVenezia;
        private System.Windows.Forms.CheckBox chkNazionale;
        private System.Windows.Forms.Button btnAvviaAnalisi;
        private System.Windows.Forms.Label lblPeriodoAnalisi;
        private System.Windows.Forms.Label lblDataInizio;
        private System.Windows.Forms.Label lblDataFine;




        //------------------------


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelContenuto = new System.Windows.Forms.Panel();
            this.grigliaRisultati = new System.Windows.Forms.DataGridView();
            this.groupBoxRaggruppamento = new System.Windows.Forms.GroupBox();
            this.radioSettimana = new System.Windows.Forms.RadioButton();
            this.radioMese = new System.Windows.Forms.RadioButton();
            this.radioTrimestre = new System.Windows.Forms.RadioButton();
            this.radioAnno = new System.Windows.Forms.RadioButton();
            this.groupBoxBudget = new System.Windows.Forms.GroupBox();
            this.numBudget = new System.Windows.Forms.NumericUpDown();
            this.btnAggiorna = new System.Windows.Forms.Button();
            this.btnEsporta = new System.Windows.Forms.Button();
            this.lblTotali = new System.Windows.Forms.Label();

            // Main UserControl
            this.SuspendLayout();
            this.Size = new System.Drawing.Size(1000, 600);
            this.BackColor = System.Drawing.Color.White;

            // Panel Header
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 80;
            this.panelHeader.BackColor = System.Drawing.Color.LightGreen;
            this.panelHeader.Padding = new System.Windows.Forms.Padding(10);

            // Panel Contenuto
            this.panelContenuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenuto.BackColor = System.Drawing.Color.White;
            this.panelContenuto.Padding = new System.Windows.Forms.Padding(5);

            // GroupBox Raggruppamento
            this.groupBoxRaggruppamento.Text = "Raggruppamento";
            this.groupBoxRaggruppamento.Location = new System.Drawing.Point(10, 10);
            this.groupBoxRaggruppamento.Size = new System.Drawing.Size(300, 60);
            this.groupBoxRaggruppamento.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);

            // Radio Buttons Raggruppamento
            this.radioSettimana.Text = "Settimana";
            this.radioSettimana.Location = new System.Drawing.Point(10, 20);
            this.radioSettimana.Size = new System.Drawing.Size(70, 20);
            this.radioSettimana.Font = new System.Drawing.Font("Arial", 7);

            this.radioMese.Text = "Mese";
            this.radioMese.Location = new System.Drawing.Point(80, 20);
            this.radioMese.Size = new System.Drawing.Size(50, 20);
            this.radioMese.Font = new System.Drawing.Font("Arial", 7);
            this.radioMese.Checked = true;

            this.radioTrimestre.Text = "Trimestre";
            this.radioTrimestre.Location = new System.Drawing.Point(130, 20);
            this.radioTrimestre.Size = new System.Drawing.Size(70, 20);
            this.radioTrimestre.Font = new System.Drawing.Font("Arial", 7);

            this.radioAnno.Text = "Anno";
            this.radioAnno.Location = new System.Drawing.Point(200, 20);
            this.radioAnno.Size = new System.Drawing.Size(50, 20);
            this.radioAnno.Font = new System.Drawing.Font("Arial", 7);

            // GroupBox Budget
            this.groupBoxBudget.Text = "Budget Disponibile";
            this.groupBoxBudget.Location = new System.Drawing.Point(320, 10);
            this.groupBoxBudget.Size = new System.Drawing.Size(150, 60);
            this.groupBoxBudget.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);

            // Numeric UpDown Budget
            this.numBudget.Location = new System.Drawing.Point(10, 25);
            this.numBudget.Size = new System.Drawing.Size(120, 20);
            this.numBudget.Value = 1000;
            this.numBudget.Minimum = 100;
            this.numBudget.Maximum = 10000;
            this.numBudget.DecimalPlaces = 2;

            // Button Aggiorna
            this.btnAggiorna.Text = "Aggiorna Analisi";
            this.btnAggiorna.Size = new System.Drawing.Size(120, 30);
            this.btnAggiorna.Location = new System.Drawing.Point(480, 25);
            this.btnAggiorna.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAggiorna.ForeColor = System.Drawing.Color.White;
            this.btnAggiorna.Click += new System.EventHandler(this.BtnAggiorna_Click);

            // Button Esporta
            this.btnEsporta.Text = "Esporta CSV";
            this.btnEsporta.Size = new System.Drawing.Size(100, 30);
            this.btnEsporta.Location = new System.Drawing.Point(610, 25);
            this.btnEsporta.BackColor = System.Drawing.Color.DarkOrange;
            this.btnEsporta.ForeColor = System.Drawing.Color.White;
            this.btnEsporta.Click += new System.EventHandler(this.BtnEsporta_Click);

            // Label Totali
            this.lblTotali.Text = "Totali:";
            this.lblTotali.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotali.Height = 25;
            this.lblTotali.BackColor = System.Drawing.Color.LightGray;
            this.lblTotali.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotali.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            this.lblTotali.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);

            // DataGridView
            this.grigliaRisultati.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grigliaRisultati.BackgroundColor = System.Drawing.Color.White;
            this.grigliaRisultati.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grigliaRisultati.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.grigliaRisultati.AllowUserToAddRows = false;
            this.grigliaRisultati.ReadOnly = true;

            // Inizializza colonne griglia
            InizializzaColonneGriglia();

            // Aggiungi controlli ai GroupBox
            this.groupBoxRaggruppamento.Controls.Add(this.radioSettimana);
            this.groupBoxRaggruppamento.Controls.Add(this.radioMese);
            this.groupBoxRaggruppamento.Controls.Add(this.radioTrimestre);
            this.groupBoxRaggruppamento.Controls.Add(this.radioAnno);

            this.groupBoxBudget.Controls.Add(this.numBudget);

            // Aggiungi controlli al Panel Header
            this.panelHeader.Controls.Add(this.groupBoxRaggruppamento);
            this.panelHeader.Controls.Add(this.groupBoxBudget);
            this.panelHeader.Controls.Add(this.btnAggiorna);
            this.panelHeader.Controls.Add(this.btnEsporta);

            // Aggiungi controlli al Panel Contenuto
            this.panelContenuto.Controls.Add(this.grigliaRisultati);
            this.panelContenuto.Controls.Add(this.lblTotali);

            // Layout principale
            this.Controls.Add(this.panelContenuto);
            this.Controls.Add(this.panelHeader);

            // Nel metodo InitializeComponent aggiungi:

            // Label Periodo Analisi
            this.lblPeriodoAnalisi.Text = "Periodo di analisi:";
            this.lblPeriodoAnalisi.Location = new System.Drawing.Point(720, 15);
            this.lblPeriodoAnalisi.AutoSize = true;
            this.lblPeriodoAnalisi.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);

            // Label Data Inizio
            this.lblDataInizio.Text = ParametriCondivisi.DataInizioAnalisi.ToString("dd/MM/yyyy");
            this.lblDataInizio.Location = new System.Drawing.Point(720, 35);
            this.lblDataInizio.AutoSize = true;
            this.lblDataInizio.Font = new System.Drawing.Font("Arial", 7);

            // Label Data Fine
            this.lblDataFine.Text = ParametriCondivisi.DataFineAnalisi.ToString("dd/MM/yyyy");
            this.lblDataFine.Location = new System.Drawing.Point(820, 35);
            this.lblDataFine.AutoSize = true;
            this.lblDataFine.Font = new System.Drawing.Font("Arial", 7);

            // GroupBox Ruote
            this.groupBoxRuote.Text = "Ruote da analizzare";
            this.groupBoxRuote.Location = new System.Drawing.Point(10, 75);
            this.groupBoxRuote.Size = new System.Drawing.Size(600, 50);
            // ... aggiungi tutte le checkbox per le ruote ...

            // Button Avvia Analisi
            this.btnAvviaAnalisi.Text = "Avvia Analisi";
            this.btnAvviaAnalisi.Size = new System.Drawing.Size(100, 30);
            this.btnAvviaAnalisi.Location = new System.Drawing.Point(620, 85);
            this.btnAvviaAnalisi.BackColor = System.Drawing.Color.DarkRed;
            this.btnAvviaAnalisi.ForeColor = System.Drawing.Color.White;
            this.btnAvviaAnalisi.Click += new System.EventHandler(this.BtnAvviaAnalisi_Click);

            this.ResumeLayout(false);
        }

        private void InizializzaColonneGriglia()
        {
            grigliaRisultati.Columns.Add("Intervallo", "Intervallo Date");
            grigliaRisultati.Columns.Add("Estrazioni", "N° Estrazioni");
            grigliaRisultati.Columns.Add("NumeriGiocati", "Numeri Giocati");
            grigliaRisultati.Columns.Add("NumeriVinti", "Numeri Vinti");
            grigliaRisultati.Columns.Add("AmbiVinti", "Ambi Vinti");
            grigliaRisultati.Columns.Add("TerniVinti", "Terni Vinti");
            grigliaRisultati.Columns.Add("InvestimentoMin", "Invest. Minimo");
            grigliaRisultati.Columns.Add("Guadagno", "Guadagno");
            grigliaRisultati.Columns.Add("InvestimentoProp", "Invest. Proposto");
            grigliaRisultati.Columns.Add("GuadagnoProp", "Guadagno Proposto");
        }
    }
}
