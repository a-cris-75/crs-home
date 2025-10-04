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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            panelHeader = new Panel();
            groupBoxRaggruppamento = new GroupBox();
            radioSettimana = new RadioButton();
            radioMese = new RadioButton();
            radioTrimestre = new RadioButton();
            radioAnno = new RadioButton();
            groupBoxBudget = new GroupBox();
            numBudget = new NumericUpDown();
            btnAggiorna = new Button();
            btnEsporta = new Button();
            panelContenuto = new Panel();
            grigliaRisultati = new DataGridView();
            risultatoAnalisiBindingSource = new BindingSource(components);
            lblTotali = new Label();
            intervalloDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataInizioDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataFineDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numeroEstrazioniDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numeriGiocatiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numeriVintiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            ambiVintiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            terniVintiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            investimentoMinimoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            guadagnoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            investimentoPropostoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            guadagnoPropostoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            panelHeader.SuspendLayout();
            groupBoxRaggruppamento.SuspendLayout();
            groupBoxBudget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBudget).BeginInit();
            panelContenuto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).BeginInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(192, 255, 192);
            panelHeader.Controls.Add(groupBoxRaggruppamento);
            panelHeader.Controls.Add(groupBoxBudget);
            panelHeader.Controls.Add(btnAggiorna);
            panelHeader.Controls.Add(btnEsporta);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(10);
            panelHeader.Size = new Size(1358, 68);
            panelHeader.TabIndex = 1;
            // 
            // groupBoxRaggruppamento
            // 
            groupBoxRaggruppamento.Controls.Add(radioSettimana);
            groupBoxRaggruppamento.Controls.Add(radioMese);
            groupBoxRaggruppamento.Controls.Add(radioTrimestre);
            groupBoxRaggruppamento.Controls.Add(radioAnno);
            groupBoxRaggruppamento.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxRaggruppamento.Location = new Point(10, 3);
            groupBoxRaggruppamento.Name = "groupBoxRaggruppamento";
            groupBoxRaggruppamento.Size = new Size(300, 60);
            groupBoxRaggruppamento.TabIndex = 0;
            groupBoxRaggruppamento.TabStop = false;
            groupBoxRaggruppamento.Text = "Raggruppamento";
            // 
            // radioSettimana
            // 
            radioSettimana.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioSettimana.Location = new Point(10, 24);
            radioSettimana.Name = "radioSettimana";
            radioSettimana.Size = new Size(70, 20);
            radioSettimana.TabIndex = 0;
            radioSettimana.Text = "Settimana";
            // 
            // radioMese
            // 
            radioMese.AutoSize = true;
            radioMese.Checked = true;
            radioMese.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioMese.Location = new Point(80, 24);
            radioMese.Name = "radioMese";
            radioMese.Size = new Size(53, 19);
            radioMese.TabIndex = 1;
            radioMese.TabStop = true;
            radioMese.Text = "Mese";
            // 
            // radioTrimestre
            // 
            radioTrimestre.AutoSize = true;
            radioTrimestre.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioTrimestre.Location = new Point(142, 24);
            radioTrimestre.Name = "radioTrimestre";
            radioTrimestre.Size = new Size(74, 19);
            radioTrimestre.TabIndex = 2;
            radioTrimestre.Text = "Trimestre";
            // 
            // radioAnno
            // 
            radioAnno.AutoSize = true;
            radioAnno.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioAnno.Location = new Point(225, 24);
            radioAnno.Name = "radioAnno";
            radioAnno.Size = new Size(54, 19);
            radioAnno.TabIndex = 3;
            radioAnno.Text = "Anno";
            // 
            // groupBoxBudget
            // 
            groupBoxBudget.Controls.Add(numBudget);
            groupBoxBudget.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxBudget.Location = new Point(320, 3);
            groupBoxBudget.Name = "groupBoxBudget";
            groupBoxBudget.Size = new Size(150, 60);
            groupBoxBudget.TabIndex = 1;
            groupBoxBudget.TabStop = false;
            groupBoxBudget.Text = "Budget Disponibile";
            // 
            // numBudget
            // 
            numBudget.DecimalPlaces = 2;
            numBudget.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numBudget.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numBudget.Location = new Point(10, 25);
            numBudget.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numBudget.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numBudget.Name = "numBudget";
            numBudget.Size = new Size(120, 23);
            numBudget.TabIndex = 0;
            numBudget.TextAlign = HorizontalAlignment.Center;
            numBudget.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnAggiorna
            // 
            btnAggiorna.BackColor = Color.SteelBlue;
            btnAggiorna.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAggiorna.ForeColor = Color.White;
            btnAggiorna.Location = new Point(512, 12);
            btnAggiorna.Name = "btnAggiorna";
            btnAggiorna.Size = new Size(120, 44);
            btnAggiorna.TabIndex = 2;
            btnAggiorna.Text = "Aggiorna Analisi";
            btnAggiorna.UseVisualStyleBackColor = false;
            btnAggiorna.Click += BtnAggiorna_Click;
            // 
            // btnEsporta
            // 
            btnEsporta.BackColor = Color.DarkOrange;
            btnEsporta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnEsporta.ForeColor = Color.White;
            btnEsporta.Location = new Point(642, 12);
            btnEsporta.Name = "btnEsporta";
            btnEsporta.Size = new Size(100, 44);
            btnEsporta.TabIndex = 3;
            btnEsporta.Text = "Esporta CSV";
            btnEsporta.UseVisualStyleBackColor = false;
            btnEsporta.Click += BtnEsporta_Click;
            // 
            // panelContenuto
            // 
            panelContenuto.BackColor = Color.White;
            panelContenuto.Controls.Add(grigliaRisultati);
            panelContenuto.Controls.Add(lblTotali);
            panelContenuto.Dock = DockStyle.Fill;
            panelContenuto.Location = new Point(0, 68);
            panelContenuto.Name = "panelContenuto";
            panelContenuto.Padding = new Padding(5);
            panelContenuto.Size = new Size(1358, 532);
            panelContenuto.TabIndex = 0;
            // 
            // grigliaRisultati
            // 
            grigliaRisultati.AllowUserToAddRows = false;
            grigliaRisultati.AutoGenerateColumns = false;
            grigliaRisultati.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grigliaRisultati.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            grigliaRisultati.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            grigliaRisultati.ColumnHeadersHeight = 27;
            grigliaRisultati.Columns.AddRange(new DataGridViewColumn[] { intervalloDataGridViewTextBoxColumn, dataInizioDataGridViewTextBoxColumn, dataFineDataGridViewTextBoxColumn, numeroEstrazioniDataGridViewTextBoxColumn, numeriGiocatiDataGridViewTextBoxColumn, numeriVintiDataGridViewTextBoxColumn, ambiVintiDataGridViewTextBoxColumn, terniVintiDataGridViewTextBoxColumn, investimentoMinimoDataGridViewTextBoxColumn, guadagnoDataGridViewTextBoxColumn, investimentoPropostoDataGridViewTextBoxColumn, guadagnoPropostoDataGridViewTextBoxColumn });
            grigliaRisultati.DataSource = risultatoAnalisiBindingSource;
            grigliaRisultati.Dock = DockStyle.Fill;
            grigliaRisultati.EnableHeadersVisualStyles = false;
            grigliaRisultati.Location = new Point(5, 5);
            grigliaRisultati.Name = "grigliaRisultati";
            grigliaRisultati.ReadOnly = true;
            grigliaRisultati.Size = new Size(1348, 497);
            grigliaRisultati.TabIndex = 0;
            // 
            // risultatoAnalisiBindingSource
            // 
            risultatoAnalisiBindingSource.DataSource = typeof(RisultatoAnalisi);
            // 
            // lblTotali
            // 
            lblTotali.BackColor = Color.LightGray;
            lblTotali.Dock = DockStyle.Bottom;
            lblTotali.Font = new Font("Arial", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotali.Location = new Point(5, 502);
            lblTotali.Name = "lblTotali";
            lblTotali.Padding = new Padding(5, 0, 0, 0);
            lblTotali.Size = new Size(1348, 25);
            lblTotali.TabIndex = 1;
            lblTotali.Text = "Totali:";
            lblTotali.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // intervalloDataGridViewTextBoxColumn
            // 
            intervalloDataGridViewTextBoxColumn.DataPropertyName = "Intervallo";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            intervalloDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            intervalloDataGridViewTextBoxColumn.FillWeight = 200F;
            intervalloDataGridViewTextBoxColumn.HeaderText = "Intervallo";
            intervalloDataGridViewTextBoxColumn.Name = "intervalloDataGridViewTextBoxColumn";
            intervalloDataGridViewTextBoxColumn.ReadOnly = true;
            intervalloDataGridViewTextBoxColumn.Width = 81;
            // 
            // dataInizioDataGridViewTextBoxColumn
            // 
            dataInizioDataGridViewTextBoxColumn.DataPropertyName = "DataInizio";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataInizioDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            dataInizioDataGridViewTextBoxColumn.FillWeight = 150F;
            dataInizioDataGridViewTextBoxColumn.HeaderText = "DataInizio";
            dataInizioDataGridViewTextBoxColumn.Name = "dataInizioDataGridViewTextBoxColumn";
            dataInizioDataGridViewTextBoxColumn.ReadOnly = true;
            dataInizioDataGridViewTextBoxColumn.Width = 84;
            // 
            // dataFineDataGridViewTextBoxColumn
            // 
            dataFineDataGridViewTextBoxColumn.DataPropertyName = "DataFine";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataFineDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            dataFineDataGridViewTextBoxColumn.FillWeight = 150F;
            dataFineDataGridViewTextBoxColumn.HeaderText = "DataFine";
            dataFineDataGridViewTextBoxColumn.Name = "dataFineDataGridViewTextBoxColumn";
            dataFineDataGridViewTextBoxColumn.ReadOnly = true;
            dataFineDataGridViewTextBoxColumn.Width = 78;
            // 
            // numeroEstrazioniDataGridViewTextBoxColumn
            // 
            numeroEstrazioniDataGridViewTextBoxColumn.DataPropertyName = "NumeroEstrazioni";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeroEstrazioniDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            numeroEstrazioniDataGridViewTextBoxColumn.FillWeight = 80F;
            numeroEstrazioniDataGridViewTextBoxColumn.HeaderText = "Num Estrazioni";
            numeroEstrazioniDataGridViewTextBoxColumn.Name = "numeroEstrazioniDataGridViewTextBoxColumn";
            numeroEstrazioniDataGridViewTextBoxColumn.ReadOnly = true;
            numeroEstrazioniDataGridViewTextBoxColumn.Width = 112;
            // 
            // numeriGiocatiDataGridViewTextBoxColumn
            // 
            numeriGiocatiDataGridViewTextBoxColumn.DataPropertyName = "NumeriGiocati";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeriGiocatiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            numeriGiocatiDataGridViewTextBoxColumn.HeaderText = "Num Giocati";
            numeriGiocatiDataGridViewTextBoxColumn.Name = "numeriGiocatiDataGridViewTextBoxColumn";
            numeriGiocatiDataGridViewTextBoxColumn.ReadOnly = true;
            numeriGiocatiDataGridViewTextBoxColumn.Width = 99;
            // 
            // numeriVintiDataGridViewTextBoxColumn
            // 
            numeriVintiDataGridViewTextBoxColumn.DataPropertyName = "NumeriVinti";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeriVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            numeriVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            numeriVintiDataGridViewTextBoxColumn.HeaderText = "Num Vinti";
            numeriVintiDataGridViewTextBoxColumn.Name = "numeriVintiDataGridViewTextBoxColumn";
            numeriVintiDataGridViewTextBoxColumn.ReadOnly = true;
            numeriVintiDataGridViewTextBoxColumn.Width = 86;
            // 
            // ambiVintiDataGridViewTextBoxColumn
            // 
            ambiVintiDataGridViewTextBoxColumn.DataPropertyName = "AmbiVinti";
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ambiVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            ambiVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            ambiVintiDataGridViewTextBoxColumn.HeaderText = "Ambi Vinti";
            ambiVintiDataGridViewTextBoxColumn.Name = "ambiVintiDataGridViewTextBoxColumn";
            ambiVintiDataGridViewTextBoxColumn.ReadOnly = true;
            ambiVintiDataGridViewTextBoxColumn.Width = 88;
            // 
            // terniVintiDataGridViewTextBoxColumn
            // 
            terniVintiDataGridViewTextBoxColumn.DataPropertyName = "TerniVinti";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            terniVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            terniVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            terniVintiDataGridViewTextBoxColumn.HeaderText = "Terni Vinti";
            terniVintiDataGridViewTextBoxColumn.Name = "terniVintiDataGridViewTextBoxColumn";
            terniVintiDataGridViewTextBoxColumn.ReadOnly = true;
            terniVintiDataGridViewTextBoxColumn.Width = 85;
            // 
            // investimentoMinimoDataGridViewTextBoxColumn
            // 
            investimentoMinimoDataGridViewTextBoxColumn.DataPropertyName = "InvestimentoMinimo";
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            investimentoMinimoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            investimentoMinimoDataGridViewTextBoxColumn.FillWeight = 150F;
            investimentoMinimoDataGridViewTextBoxColumn.HeaderText = "Investimento Minimo";
            investimentoMinimoDataGridViewTextBoxColumn.Name = "investimentoMinimoDataGridViewTextBoxColumn";
            investimentoMinimoDataGridViewTextBoxColumn.ReadOnly = true;
            investimentoMinimoDataGridViewTextBoxColumn.Width = 146;
            // 
            // guadagnoDataGridViewTextBoxColumn
            // 
            guadagnoDataGridViewTextBoxColumn.DataPropertyName = "Guadagno";
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
            guadagnoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
            guadagnoDataGridViewTextBoxColumn.FillWeight = 120F;
            guadagnoDataGridViewTextBoxColumn.HeaderText = "Guadagno";
            guadagnoDataGridViewTextBoxColumn.Name = "guadagnoDataGridViewTextBoxColumn";
            guadagnoDataGridViewTextBoxColumn.ReadOnly = true;
            guadagnoDataGridViewTextBoxColumn.Width = 87;
            // 
            // investimentoPropostoDataGridViewTextBoxColumn
            // 
            investimentoPropostoDataGridViewTextBoxColumn.DataPropertyName = "InvestimentoProposto";
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
            investimentoPropostoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
            investimentoPropostoDataGridViewTextBoxColumn.FillWeight = 150F;
            investimentoPropostoDataGridViewTextBoxColumn.HeaderText = "Investimento Proposto";
            investimentoPropostoDataGridViewTextBoxColumn.Name = "investimentoPropostoDataGridViewTextBoxColumn";
            investimentoPropostoDataGridViewTextBoxColumn.ReadOnly = true;
            investimentoPropostoDataGridViewTextBoxColumn.Width = 152;
            // 
            // guadagnoPropostoDataGridViewTextBoxColumn
            // 
            guadagnoPropostoDataGridViewTextBoxColumn.DataPropertyName = "GuadagnoProposto";
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter;
            guadagnoPropostoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle13;
            guadagnoPropostoDataGridViewTextBoxColumn.FillWeight = 150F;
            guadagnoPropostoDataGridViewTextBoxColumn.HeaderText = "Guadagno Proposto";
            guadagnoPropostoDataGridViewTextBoxColumn.Name = "guadagnoPropostoDataGridViewTextBoxColumn";
            guadagnoPropostoDataGridViewTextBoxColumn.ReadOnly = true;
            guadagnoPropostoDataGridViewTextBoxColumn.Width = 138;
            // 
            // PageAnalisi
            // 
            BackColor = Color.White;
            Controls.Add(panelContenuto);
            Controls.Add(panelHeader);
            Name = "PageAnalisi";
            Size = new Size(1358, 600);
            panelHeader.ResumeLayout(false);
            groupBoxRaggruppamento.ResumeLayout(false);
            groupBoxRaggruppamento.PerformLayout();
            groupBoxBudget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numBudget).EndInit();
            panelContenuto.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).EndInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).EndInit();
            ResumeLayout(false);
        }
        private BindingSource risultatoAnalisiBindingSource;
        private DataGridViewTextBoxColumn intervalloDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataInizioDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataFineDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numeroEstrazioniDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numeriGiocatiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numeriVintiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn ambiVintiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn terniVintiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn investimentoMinimoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn guadagnoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn investimentoPropostoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn guadagnoPropostoDataGridViewTextBoxColumn;

        //private void InizializzaColonneGriglia()
        //{
        //    grigliaRisultati.Columns.Add("Intervallo", "Intervallo Date");
        //    grigliaRisultati.Columns.Add("Estrazioni", "N° Estrazioni");
        //    grigliaRisultati.Columns.Add("NumeriGiocati", "Numeri Giocati");
        //    grigliaRisultati.Columns.Add("NumeriVinti", "Numeri Vinti");
        //    grigliaRisultati.Columns.Add("AmbiVinti", "Ambi Vinti");
        //    grigliaRisultati.Columns.Add("TerniVinti", "Terni Vinti");
        //    grigliaRisultati.Columns.Add("InvestimentoMin", "Invest. Minimo");
        //    grigliaRisultati.Columns.Add("Guadagno", "Guadagno");
        //    grigliaRisultati.Columns.Add("InvestimentoProp", "Invest. Proposto");
        //    grigliaRisultati.Columns.Add("GuadagnoProp", "Guadagno Proposto");
        //}
    }
}
