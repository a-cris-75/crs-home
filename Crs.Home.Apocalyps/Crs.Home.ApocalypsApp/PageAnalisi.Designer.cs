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

        //private System.Windows.Forms.GroupBox groupBoxRuote;
        //private System.Windows.Forms.CheckBox chkBari;
        //private System.Windows.Forms.CheckBox chkCagliari;
        //private System.Windows.Forms.CheckBox chkFirenze;
        //private System.Windows.Forms.CheckBox chkGenova;
        //private System.Windows.Forms.CheckBox chkMilano;
        //private System.Windows.Forms.CheckBox chkNapoli;
        //private System.Windows.Forms.CheckBox chkPalermo;
        //private System.Windows.Forms.CheckBox chkRoma;
        //private System.Windows.Forms.CheckBox chkTorino;
        //private System.Windows.Forms.CheckBox chkVenezia;
        //private System.Windows.Forms.CheckBox chkNazionale;
        //private System.Windows.Forms.Button btnAvviaAnalisi;
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
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
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
            chkNazionale = new CheckBox();
            chkVenezia = new CheckBox();
            chkTorino = new CheckBox();
            chkRoma = new CheckBox();
            chkPalermo = new CheckBox();
            chkNapoli = new CheckBox();
            chkMilano = new CheckBox();
            chkGenova = new CheckBox();
            chkFirenze = new CheckBox();
            chkCagliari = new CheckBox();
            chkBari = new CheckBox();
            btnStartModello = new Button();
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            grigliaRisultati = new DataGridView();
            risultatoAnalisiBindingSource = new BindingSource(components);
            tabPage2 = new TabPage();
            grigliaDettagliEsrtrazione = new DataGridView();
            lblTotali = new Label();
            risultatoEstrazioneBindingSource = new BindingSource(components);
            ruotaDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numPrevistiPesiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
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
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).BeginInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaDettagliEsrtrazione).BeginInit();
            ((System.ComponentModel.ISupportInitialize)risultatoEstrazioneBindingSource).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(192, 255, 192);
            panelHeader.Controls.Add(chkNazionale);
            panelHeader.Controls.Add(chkVenezia);
            panelHeader.Controls.Add(chkTorino);
            panelHeader.Controls.Add(chkRoma);
            panelHeader.Controls.Add(chkPalermo);
            panelHeader.Controls.Add(chkNapoli);
            panelHeader.Controls.Add(chkMilano);
            panelHeader.Controls.Add(chkGenova);
            panelHeader.Controls.Add(chkFirenze);
            panelHeader.Controls.Add(chkCagliari);
            panelHeader.Controls.Add(chkBari);
            panelHeader.Controls.Add(btnStartModello);
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
            // chkNazionale
            // 
            chkNazionale.AutoSize = true;
            chkNazionale.Location = new Point(1208, 36);
            chkNazionale.Name = "chkNazionale";
            chkNazionale.Size = new Size(42, 19);
            chkNazionale.TabIndex = 15;
            chkNazionale.Text = "NZ";
            chkNazionale.UseVisualStyleBackColor = true;
            // 
            // chkVenezia
            // 
            chkVenezia.AutoSize = true;
            chkVenezia.Location = new Point(1112, 36);
            chkVenezia.Name = "chkVenezia";
            chkVenezia.Size = new Size(39, 19);
            chkVenezia.TabIndex = 14;
            chkVenezia.Text = "VE";
            chkVenezia.UseVisualStyleBackColor = true;
            // 
            // chkTorino
            // 
            chkTorino.AutoSize = true;
            chkTorino.Location = new Point(1112, 12);
            chkTorino.Name = "chkTorino";
            chkTorino.Size = new Size(41, 19);
            chkTorino.TabIndex = 13;
            chkTorino.Text = "TO";
            chkTorino.UseVisualStyleBackColor = true;
            // 
            // chkRoma
            // 
            chkRoma.AutoSize = true;
            chkRoma.Location = new Point(1066, 36);
            chkRoma.Name = "chkRoma";
            chkRoma.Size = new Size(44, 19);
            chkRoma.TabIndex = 12;
            chkRoma.Text = "RM";
            chkRoma.UseVisualStyleBackColor = true;
            // 
            // chkPalermo
            // 
            chkPalermo.AutoSize = true;
            chkPalermo.Location = new Point(1066, 12);
            chkPalermo.Name = "chkPalermo";
            chkPalermo.Size = new Size(40, 19);
            chkPalermo.TabIndex = 11;
            chkPalermo.Text = "PA";
            chkPalermo.UseVisualStyleBackColor = true;
            // 
            // chkNapoli
            // 
            chkNapoli.AutoSize = true;
            chkNapoli.Location = new Point(1159, 36);
            chkNapoli.Name = "chkNapoli";
            chkNapoli.Size = new Size(43, 19);
            chkNapoli.TabIndex = 10;
            chkNapoli.Text = "NA";
            chkNapoli.UseVisualStyleBackColor = true;
            // 
            // chkMilano
            // 
            chkMilano.AutoSize = true;
            chkMilano.Location = new Point(1159, 12);
            chkMilano.Name = "chkMilano";
            chkMilano.Size = new Size(40, 19);
            chkMilano.TabIndex = 9;
            chkMilano.Text = "MI";
            chkMilano.UseVisualStyleBackColor = true;
            // 
            // chkGenova
            // 
            chkGenova.AutoSize = true;
            chkGenova.Location = new Point(1021, 36);
            chkGenova.Name = "chkGenova";
            chkGenova.Size = new Size(40, 19);
            chkGenova.TabIndex = 8;
            chkGenova.Text = "GE";
            chkGenova.UseVisualStyleBackColor = true;
            // 
            // chkFirenze
            // 
            chkFirenze.AutoSize = true;
            chkFirenze.Location = new Point(1021, 12);
            chkFirenze.Name = "chkFirenze";
            chkFirenze.Size = new Size(35, 19);
            chkFirenze.TabIndex = 7;
            chkFirenze.Text = "FI";
            chkFirenze.UseVisualStyleBackColor = true;
            // 
            // chkCagliari
            // 
            chkCagliari.AutoSize = true;
            chkCagliari.Location = new Point(973, 36);
            chkCagliari.Name = "chkCagliari";
            chkCagliari.Size = new Size(42, 19);
            chkCagliari.TabIndex = 6;
            chkCagliari.Text = "CA";
            chkCagliari.UseVisualStyleBackColor = true;
            // 
            // chkBari
            // 
            chkBari.AutoSize = true;
            chkBari.Location = new Point(973, 12);
            chkBari.Name = "chkBari";
            chkBari.Size = new Size(41, 19);
            chkBari.TabIndex = 5;
            chkBari.Text = "BA";
            chkBari.UseVisualStyleBackColor = true;
            // 
            // btnStartModello
            // 
            btnStartModello.BackColor = Color.LightSteelBlue;
            btnStartModello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnStartModello.ForeColor = Color.White;
            btnStartModello.Location = new Point(576, 12);
            btnStartModello.Name = "btnStartModello";
            btnStartModello.Size = new Size(120, 44);
            btnStartModello.TabIndex = 4;
            btnStartModello.Text = "Avvia analisi";
            btnStartModello.UseVisualStyleBackColor = false;
            btnStartModello.Click += BtnAvviaAnalisi_Click;
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
            groupBoxRaggruppamento.Size = new Size(342, 60);
            groupBoxRaggruppamento.TabIndex = 0;
            groupBoxRaggruppamento.TabStop = false;
            groupBoxRaggruppamento.Text = "Raggruppamento";
            // 
            // radioSettimana
            // 
            radioSettimana.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioSettimana.Location = new Point(23, 24);
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
            radioMese.Location = new Point(99, 24);
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
            radioTrimestre.Location = new Point(164, 24);
            radioTrimestre.Name = "radioTrimestre";
            radioTrimestre.Size = new Size(74, 19);
            radioTrimestre.TabIndex = 2;
            radioTrimestre.Text = "Trimestre";
            // 
            // radioAnno
            // 
            radioAnno.AutoSize = true;
            radioAnno.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioAnno.Location = new Point(247, 24);
            radioAnno.Name = "radioAnno";
            radioAnno.Size = new Size(54, 19);
            radioAnno.TabIndex = 3;
            radioAnno.Text = "Anno";
            // 
            // groupBoxBudget
            // 
            groupBoxBudget.Controls.Add(numBudget);
            groupBoxBudget.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxBudget.Location = new Point(361, 3);
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
            btnAggiorna.BackColor = Color.LightSteelBlue;
            btnAggiorna.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAggiorna.ForeColor = Color.White;
            btnAggiorna.Location = new Point(702, 12);
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
            btnEsporta.Location = new Point(832, 12);
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
            panelContenuto.Controls.Add(tabControl1);
            panelContenuto.Controls.Add(lblTotali);
            panelContenuto.Dock = DockStyle.Fill;
            panelContenuto.Location = new Point(0, 68);
            panelContenuto.Name = "panelContenuto";
            panelContenuto.Padding = new Padding(5);
            panelContenuto.Size = new Size(1358, 532);
            panelContenuto.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(5, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1348, 497);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(grigliaRisultati);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1340, 469);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Raggruppamento";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // grigliaRisultati
            // 
            grigliaRisultati.AllowUserToAddRows = false;
            grigliaRisultati.AutoGenerateColumns = false;
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
            grigliaRisultati.Location = new Point(3, 3);
            grigliaRisultati.Name = "grigliaRisultati";
            grigliaRisultati.ReadOnly = true;
            grigliaRisultati.Size = new Size(1334, 463);
            grigliaRisultati.TabIndex = 0;
            // 
            // risultatoAnalisiBindingSource
            // 
            risultatoAnalisiBindingSource.DataSource = typeof(RisultatoAnalisi);
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(grigliaDettagliEsrtrazione);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1340, 469);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Dettaglio Estrazioni";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // grigliaDettagliEsrtrazione
            // 
            grigliaDettagliEsrtrazione.AllowUserToAddRows = false;
            grigliaDettagliEsrtrazione.AutoGenerateColumns = false;
            grigliaDettagliEsrtrazione.BackgroundColor = Color.White;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = SystemColors.Control;
            dataGridViewCellStyle14.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle14.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.True;
            grigliaDettagliEsrtrazione.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            grigliaDettagliEsrtrazione.ColumnHeadersHeight = 27;
            grigliaDettagliEsrtrazione.Columns.AddRange(new DataGridViewColumn[] { ruotaDataGridViewTextBoxColumn, dataDataGridViewTextBoxColumn, numPrevistiPesiDataGridViewTextBoxColumn });
            grigliaDettagliEsrtrazione.DataSource = risultatoEstrazioneBindingSource;
            grigliaDettagliEsrtrazione.Dock = DockStyle.Fill;
            grigliaDettagliEsrtrazione.EnableHeadersVisualStyles = false;
            grigliaDettagliEsrtrazione.Location = new Point(3, 3);
            grigliaDettagliEsrtrazione.Name = "grigliaDettagliEsrtrazione";
            grigliaDettagliEsrtrazione.ReadOnly = true;
            grigliaDettagliEsrtrazione.Size = new Size(1334, 463);
            grigliaDettagliEsrtrazione.TabIndex = 1;
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
            // risultatoEstrazioneBindingSource
            // 
            risultatoEstrazioneBindingSource.DataSource = typeof(RisultatoEstrazione);
            // 
            // ruotaDataGridViewTextBoxColumn
            // 
            ruotaDataGridViewTextBoxColumn.DataPropertyName = "Ruota";
            ruotaDataGridViewTextBoxColumn.HeaderText = "Ruota";
            ruotaDataGridViewTextBoxColumn.Name = "ruotaDataGridViewTextBoxColumn";
            ruotaDataGridViewTextBoxColumn.ReadOnly = true;
            ruotaDataGridViewTextBoxColumn.Width = 63;
            // 
            // dataDataGridViewTextBoxColumn
            // 
            dataDataGridViewTextBoxColumn.DataPropertyName = "Data";
            dataDataGridViewTextBoxColumn.HeaderText = "Data";
            dataDataGridViewTextBoxColumn.Name = "dataDataGridViewTextBoxColumn";
            dataDataGridViewTextBoxColumn.ReadOnly = true;
            dataDataGridViewTextBoxColumn.Width = 120;
            // 
            // numPrevistiPesiDataGridViewTextBoxColumn
            // 
            numPrevistiPesiDataGridViewTextBoxColumn.DataPropertyName = "NumPrevistiPesi";
            numPrevistiPesiDataGridViewTextBoxColumn.HeaderText = "Num Previsti (Pesi)";
            numPrevistiPesiDataGridViewTextBoxColumn.Name = "numPrevistiPesiDataGridViewTextBoxColumn";
            numPrevistiPesiDataGridViewTextBoxColumn.ReadOnly = true;
            numPrevistiPesiDataGridViewTextBoxColumn.Width = 300;
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
            intervalloDataGridViewTextBoxColumn.Width = 220;
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
            dataInizioDataGridViewTextBoxColumn.Visible = false;
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
            dataFineDataGridViewTextBoxColumn.Visible = false;
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
            panelHeader.PerformLayout();
            groupBoxRaggruppamento.ResumeLayout(false);
            groupBoxRaggruppamento.PerformLayout();
            groupBoxBudget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numBudget).EndInit();
            panelContenuto.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).EndInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaDettagliEsrtrazione).EndInit();
            ((System.ComponentModel.ISupportInitialize)risultatoEstrazioneBindingSource).EndInit();
            ResumeLayout(false);
        }
        private BindingSource risultatoAnalisiBindingSource;
        private Button btnStartModello;
        private CheckBox chkNapoli;
        private CheckBox chkMilano;
        private CheckBox chkGenova;
        private CheckBox chkFirenze;
        private CheckBox chkCagliari;
        private CheckBox chkBari;
        private CheckBox chkNazionale;
        private CheckBox chkVenezia;
        private CheckBox chkTorino;
        private CheckBox chkRoma;
        private CheckBox chkPalermo;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView grigliaDettagliEsrtrazione;
        private DataGridViewTextBoxColumn numPrevisitPesiDataGridViewTextBoxColumn;
        private BindingSource risultatoEstrazioneBindingSource;
        private DataGridViewTextBoxColumn ruotaDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numPrevistiPesiDataGridViewTextBoxColumn;
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
