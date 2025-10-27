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
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle17 = new DataGridViewCellStyle();
            panelHeader = new Panel();
            btnModGeometricoCubo = new Button();
            btnModCicli = new Button();
            btnAnalisiModQuantistico = new Button();
            btnTest2 = new Button();
            gbRegole = new GroupBox();
            checkBox12 = new CheckBox();
            checkBox11 = new CheckBox();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            checkBox5 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox7 = new CheckBox();
            checkBox8 = new CheckBox();
            checkBox6 = new CheckBox();
            checkBox10 = new CheckBox();
            checkBox9 = new CheckBox();
            btnStartModParametriOscillanti = new Button();
            label3 = new Label();
            lblNum = new Label();
            dtDataTarget = new DateTimePicker();
            txtDbgNum = new TextBox();
            btnTest = new Button();
            groupBox1 = new GroupBox();
            chkBari = new CheckBox();
            chkCagliari = new CheckBox();
            chkNazionale = new CheckBox();
            chkFirenze = new CheckBox();
            chkVenezia = new CheckBox();
            chkGenova = new CheckBox();
            chkTorino = new CheckBox();
            chkMilano = new CheckBox();
            chkRoma = new CheckBox();
            chkNapoli = new CheckBox();
            chkPalermo = new CheckBox();
            gbInterval = new GroupBox();
            label1 = new Label();
            dateTimeInizio = new DateTimePicker();
            label2 = new Label();
            dateTimeFine = new DateTimePicker();
            btnStartModelloArmonia = new Button();
            groupBoxRaggruppamento = new GroupBox();
            radioSettimana = new RadioButton();
            radioMese = new RadioButton();
            radioTrimestre = new RadioButton();
            radioAnno = new RadioButton();
            groupBoxBudget = new GroupBox();
            label5 = new Label();
            numPrev = new NumericUpDown();
            label4 = new Label();
            numBudget = new NumericUpDown();
            btnAggiorna = new Button();
            btnEsporta = new Button();
            panelContenuto = new Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            grigliaRisultati = new DataGridView();
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
            risultatoAnalisiBindingSource = new BindingSource(components);
            tabPage2 = new TabPage();
            grigliaDettagliEsrtrazione = new DataGridView();
            ruotaDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            numPrevistiPesiDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            CountPrevisti = new DataGridViewTextBoxColumn();
            NumEstrattiPesi = new DataGridViewTextBoxColumn();
            NumEstratti = new DataGridViewTextBoxColumn();
            NumVinti = new DataGridViewTextBoxColumn();
            risultatoEstrazioneBindingSource = new BindingSource(components);
            tabPage3 = new TabPage();
            txtResTest = new RichTextBox();
            lblTotali = new Label();
            panelHeader.SuspendLayout();
            gbRegole.SuspendLayout();
            groupBox1.SuspendLayout();
            gbInterval.SuspendLayout();
            groupBoxRaggruppamento.SuspendLayout();
            groupBoxBudget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPrev).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBudget).BeginInit();
            panelContenuto.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).BeginInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grigliaDettagliEsrtrazione).BeginInit();
            ((System.ComponentModel.ISupportInitialize)risultatoEstrazioneBindingSource).BeginInit();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(192, 255, 192);
            panelHeader.Controls.Add(btnModGeometricoCubo);
            panelHeader.Controls.Add(btnModCicli);
            panelHeader.Controls.Add(btnAnalisiModQuantistico);
            panelHeader.Controls.Add(btnTest2);
            panelHeader.Controls.Add(gbRegole);
            panelHeader.Controls.Add(btnStartModParametriOscillanti);
            panelHeader.Controls.Add(label3);
            panelHeader.Controls.Add(lblNum);
            panelHeader.Controls.Add(dtDataTarget);
            panelHeader.Controls.Add(txtDbgNum);
            panelHeader.Controls.Add(btnTest);
            panelHeader.Controls.Add(groupBox1);
            panelHeader.Controls.Add(gbInterval);
            panelHeader.Controls.Add(btnStartModelloArmonia);
            panelHeader.Controls.Add(groupBoxRaggruppamento);
            panelHeader.Controls.Add(groupBoxBudget);
            panelHeader.Controls.Add(btnAggiorna);
            panelHeader.Controls.Add(btnEsporta);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(10);
            panelHeader.Size = new Size(1358, 196);
            panelHeader.TabIndex = 1;
            // 
            // btnModGeometricoCubo
            // 
            btnModGeometricoCubo.BackColor = Color.LightSteelBlue;
            btnModGeometricoCubo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnModGeometricoCubo.ForeColor = Color.White;
            btnModGeometricoCubo.Location = new Point(1202, 15);
            btnModGeometricoCubo.Name = "btnModGeometricoCubo";
            btnModGeometricoCubo.Size = new Size(124, 44);
            btnModGeometricoCubo.TabIndex = 28;
            btnModGeometricoCubo.Text = "Analisi Mod Geometrico Cubo";
            btnModGeometricoCubo.UseVisualStyleBackColor = false;
            btnModGeometricoCubo.Click += btnModGeometricoCubo_Click;
            // 
            // btnModCicli
            // 
            btnModCicli.BackColor = Color.LightSteelBlue;
            btnModCicli.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnModCicli.ForeColor = Color.White;
            btnModCicli.Location = new Point(1072, 15);
            btnModCicli.Name = "btnModCicli";
            btnModCicli.Size = new Size(124, 44);
            btnModCicli.TabIndex = 27;
            btnModCicli.Text = "Analisi Mod Cicli";
            btnModCicli.UseVisualStyleBackColor = false;
            btnModCicli.Click += btnModCicli_Click;
            // 
            // btnAnalisiModQuantistico
            // 
            btnAnalisiModQuantistico.BackColor = Color.LightSteelBlue;
            btnAnalisiModQuantistico.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAnalisiModQuantistico.ForeColor = Color.White;
            btnAnalisiModQuantistico.Location = new Point(942, 15);
            btnAnalisiModQuantistico.Name = "btnAnalisiModQuantistico";
            btnAnalisiModQuantistico.Size = new Size(124, 44);
            btnAnalisiModQuantistico.TabIndex = 26;
            btnAnalisiModQuantistico.Text = "Analisi Mod Quantistico";
            btnAnalisiModQuantistico.UseVisualStyleBackColor = false;
            btnAnalisiModQuantistico.Click += btnAnalisiModQuantistico_Click;
            // 
            // btnTest2
            // 
            btnTest2.BackColor = Color.LightSteelBlue;
            btnTest2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnTest2.ForeColor = Color.White;
            btnTest2.Location = new Point(1234, 126);
            btnTest2.Name = "btnTest2";
            btnTest2.Size = new Size(64, 44);
            btnTest2.TabIndex = 25;
            btnTest2.Text = "Test 2";
            btnTest2.UseVisualStyleBackColor = false;
            btnTest2.Click += btnTest2_Click;
            // 
            // gbRegole
            // 
            gbRegole.Controls.Add(checkBox12);
            gbRegole.Controls.Add(checkBox11);
            gbRegole.Controls.Add(checkBox1);
            gbRegole.Controls.Add(checkBox2);
            gbRegole.Controls.Add(checkBox3);
            gbRegole.Controls.Add(checkBox5);
            gbRegole.Controls.Add(checkBox4);
            gbRegole.Controls.Add(checkBox7);
            gbRegole.Controls.Add(checkBox8);
            gbRegole.Controls.Add(checkBox6);
            gbRegole.Controls.Add(checkBox10);
            gbRegole.Controls.Add(checkBox9);
            gbRegole.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gbRegole.Location = new Point(680, 68);
            gbRegole.Name = "gbRegole";
            gbRegole.Size = new Size(433, 64);
            gbRegole.TabIndex = 24;
            gbRegole.TabStop = false;
            gbRegole.Text = "Regole";
            // 
            // checkBox12
            // 
            checkBox12.AutoSize = true;
            checkBox12.Location = new Point(329, 42);
            checkBox12.Name = "checkBox12";
            checkBox12.Size = new Size(54, 19);
            checkBox12.TabIndex = 16;
            checkBox12.Text = "fisica";
            checkBox12.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            checkBox11.AutoSize = true;
            checkBox11.Location = new Point(329, 19);
            checkBox11.Name = "checkBox11";
            checkBox11.Size = new Size(92, 19);
            checkBox11.TabIndex = 15;
            checkBox11.Text = "coppie segn";
            checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(8, 19);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(46, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "dec";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(8, 42);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(39, 19);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "FA";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(60, 19);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(43, 19);
            checkBox3.TabIndex = 7;
            checkBox3.Text = "pol";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(109, 19);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(48, 19);
            checkBox5.TabIndex = 14;
            checkBox5.Text = "arm";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(60, 42);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(39, 19);
            checkBox4.TabIndex = 8;
            checkBox4.Text = "rit";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Location = new Point(176, 19);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(80, 19);
            checkBox7.TabIndex = 13;
            checkBox7.Text = "auto att 9";
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            checkBox8.AutoSize = true;
            checkBox8.Location = new Point(176, 42);
            checkBox8.Name = "checkBox8";
            checkBox8.Size = new Size(86, 19);
            checkBox8.TabIndex = 9;
            checkBox8.Text = "inter ruota";
            checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(109, 42);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(62, 19);
            checkBox6.TabIndex = 12;
            checkBox6.Text = "diff rit";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            checkBox10.AutoSize = true;
            checkBox10.Location = new Point(266, 42);
            checkBox10.Name = "checkBox10";
            checkBox10.Size = new Size(45, 19);
            checkBox10.TabIndex = 10;
            checkBox10.Text = "seq";
            checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            checkBox9.AutoSize = true;
            checkBox9.Location = new Point(266, 20);
            checkBox9.Name = "checkBox9";
            checkBox9.Size = new Size(63, 19);
            checkBox9.TabIndex = 11;
            checkBox9.Text = "tempo";
            checkBox9.UseVisualStyleBackColor = true;
            // 
            // btnStartModParametriOscillanti
            // 
            btnStartModParametriOscillanti.BackColor = Color.LightSteelBlue;
            btnStartModParametriOscillanti.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnStartModParametriOscillanti.ForeColor = Color.White;
            btnStartModParametriOscillanti.Location = new Point(812, 15);
            btnStartModParametriOscillanti.Name = "btnStartModParametriOscillanti";
            btnStartModParametriOscillanti.Size = new Size(124, 44);
            btnStartModParametriOscillanti.TabIndex = 23;
            btnStartModParametriOscillanti.Text = "Analisi Mod Parametri Oscillanti";
            btnStartModParametriOscillanti.UseVisualStyleBackColor = false;
            btnStartModParametriOscillanti.Click += btnStartModParametriOscillanti_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1154, 101);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 22;
            label3.Text = "Data Target";
            // 
            // lblNum
            // 
            lblNum.AutoSize = true;
            lblNum.Location = new Point(1154, 73);
            lblNum.Name = "lblNum";
            lblNum.Size = new Size(47, 15);
            lblNum.TabIndex = 21;
            lblNum.Text = "Numeri";
            // 
            // dtDataTarget
            // 
            dtDataTarget.Format = DateTimePickerFormat.Short;
            dtDataTarget.Location = new Point(1234, 97);
            dtDataTarget.Name = "dtDataTarget";
            dtDataTarget.Size = new Size(101, 23);
            dtDataTarget.TabIndex = 20;
            dtDataTarget.Value = new DateTime(2025, 7, 29, 11, 2, 0, 0);
            // 
            // txtDbgNum
            // 
            txtDbgNum.Location = new Point(1234, 68);
            txtDbgNum.Name = "txtDbgNum";
            txtDbgNum.Size = new Size(101, 23);
            txtDbgNum.TabIndex = 19;
            txtDbgNum.Text = "45";
            txtDbgNum.TextAlign = HorizontalAlignment.Center;
            // 
            // btnTest
            // 
            btnTest.BackColor = Color.LightSteelBlue;
            btnTest.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnTest.ForeColor = Color.White;
            btnTest.Location = new Point(1164, 126);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(64, 44);
            btnTest.TabIndex = 18;
            btnTest.Text = "Test 1";
            btnTest.UseVisualStyleBackColor = false;
            btnTest.Click += btnTest_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkBari);
            groupBox1.Controls.Add(chkCagliari);
            groupBox1.Controls.Add(chkNazionale);
            groupBox1.Controls.Add(chkFirenze);
            groupBox1.Controls.Add(chkVenezia);
            groupBox1.Controls.Add(chkGenova);
            groupBox1.Controls.Add(chkTorino);
            groupBox1.Controls.Add(chkMilano);
            groupBox1.Controls.Add(chkRoma);
            groupBox1.Controls.Add(chkNapoli);
            groupBox1.Controls.Add(chkPalermo);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.Location = new Point(353, 67);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(316, 64);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ruote";
            // 
            // chkBari
            // 
            chkBari.AutoSize = true;
            chkBari.Location = new Point(23, 19);
            chkBari.Name = "chkBari";
            chkBari.Size = new Size(42, 19);
            chkBari.TabIndex = 5;
            chkBari.Text = "BA";
            chkBari.UseVisualStyleBackColor = true;
            // 
            // chkCagliari
            // 
            chkCagliari.AutoSize = true;
            chkCagliari.Location = new Point(23, 42);
            chkCagliari.Name = "chkCagliari";
            chkCagliari.Size = new Size(41, 19);
            chkCagliari.TabIndex = 6;
            chkCagliari.Text = "CA";
            chkCagliari.UseVisualStyleBackColor = true;
            // 
            // chkNazionale
            // 
            chkNazionale.AutoSize = true;
            chkNazionale.Location = new Point(258, 42);
            chkNazionale.Name = "chkNazionale";
            chkNazionale.Size = new Size(42, 19);
            chkNazionale.TabIndex = 15;
            chkNazionale.Text = "NZ";
            chkNazionale.UseVisualStyleBackColor = true;
            // 
            // chkFirenze
            // 
            chkFirenze.AutoSize = true;
            chkFirenze.Location = new Point(71, 19);
            chkFirenze.Name = "chkFirenze";
            chkFirenze.Size = new Size(36, 19);
            chkFirenze.TabIndex = 7;
            chkFirenze.Text = "FI";
            chkFirenze.UseVisualStyleBackColor = true;
            // 
            // chkVenezia
            // 
            chkVenezia.AutoSize = true;
            chkVenezia.Location = new Point(162, 42);
            chkVenezia.Name = "chkVenezia";
            chkVenezia.Size = new Size(40, 19);
            chkVenezia.TabIndex = 14;
            chkVenezia.Text = "VE";
            chkVenezia.UseVisualStyleBackColor = true;
            // 
            // chkGenova
            // 
            chkGenova.AutoSize = true;
            chkGenova.Location = new Point(71, 42);
            chkGenova.Name = "chkGenova";
            chkGenova.Size = new Size(41, 19);
            chkGenova.TabIndex = 8;
            chkGenova.Text = "GE";
            chkGenova.UseVisualStyleBackColor = true;
            // 
            // chkTorino
            // 
            chkTorino.AutoSize = true;
            chkTorino.Location = new Point(162, 19);
            chkTorino.Name = "chkTorino";
            chkTorino.Size = new Size(42, 19);
            chkTorino.TabIndex = 13;
            chkTorino.Text = "TO";
            chkTorino.UseVisualStyleBackColor = true;
            // 
            // chkMilano
            // 
            chkMilano.AutoSize = true;
            chkMilano.Location = new Point(209, 19);
            chkMilano.Name = "chkMilano";
            chkMilano.Size = new Size(41, 19);
            chkMilano.TabIndex = 9;
            chkMilano.Text = "MI";
            chkMilano.UseVisualStyleBackColor = true;
            // 
            // chkRoma
            // 
            chkRoma.AutoSize = true;
            chkRoma.Location = new Point(116, 42);
            chkRoma.Name = "chkRoma";
            chkRoma.Size = new Size(45, 19);
            chkRoma.TabIndex = 12;
            chkRoma.Text = "RM";
            chkRoma.UseVisualStyleBackColor = true;
            // 
            // chkNapoli
            // 
            chkNapoli.AutoSize = true;
            chkNapoli.Location = new Point(209, 42);
            chkNapoli.Name = "chkNapoli";
            chkNapoli.Size = new Size(43, 19);
            chkNapoli.TabIndex = 10;
            chkNapoli.Text = "NA";
            chkNapoli.UseVisualStyleBackColor = true;
            // 
            // chkPalermo
            // 
            chkPalermo.AutoSize = true;
            chkPalermo.Location = new Point(116, 19);
            chkPalermo.Name = "chkPalermo";
            chkPalermo.Size = new Size(40, 19);
            chkPalermo.TabIndex = 11;
            chkPalermo.Text = "PA";
            chkPalermo.UseVisualStyleBackColor = true;
            // 
            // gbInterval
            // 
            gbInterval.Controls.Add(label1);
            gbInterval.Controls.Add(dateTimeInizio);
            gbInterval.Controls.Add(label2);
            gbInterval.Controls.Add(dateTimeFine);
            gbInterval.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gbInterval.Location = new Point(10, 67);
            gbInterval.Name = "gbInterval";
            gbInterval.Size = new Size(328, 64);
            gbInterval.TabIndex = 16;
            gbInterval.TabStop = false;
            gbInterval.Text = "Intervallo analisi";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(17, 28);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 4;
            label1.Text = "Da:";
            // 
            // dateTimeInizio
            // 
            dateTimeInizio.Format = DateTimePickerFormat.Short;
            dateTimeInizio.Location = new Point(48, 25);
            dateTimeInizio.Name = "dateTimeInizio";
            dateTimeInizio.Size = new Size(120, 23);
            dateTimeInizio.TabIndex = 5;
            dateTimeInizio.Value = new DateTime(2025, 8, 30, 11, 2, 23, 764);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(178, 28);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 6;
            label2.Text = "A:";
            // 
            // dateTimeFine
            // 
            dateTimeFine.Format = DateTimePickerFormat.Short;
            dateTimeFine.Location = new Point(198, 25);
            dateTimeFine.Name = "dateTimeFine";
            dateTimeFine.Size = new Size(120, 23);
            dateTimeFine.TabIndex = 7;
            dateTimeFine.Value = new DateTime(2025, 9, 30, 11, 2, 23, 766);
            // 
            // btnStartModelloArmonia
            // 
            btnStartModelloArmonia.BackColor = Color.LightSteelBlue;
            btnStartModelloArmonia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnStartModelloArmonia.ForeColor = Color.White;
            btnStartModelloArmonia.Location = new Point(680, 15);
            btnStartModelloArmonia.Name = "btnStartModelloArmonia";
            btnStartModelloArmonia.Size = new Size(124, 44);
            btnStartModelloArmonia.TabIndex = 4;
            btnStartModelloArmonia.Text = "Analisi Modello Armonia";
            btnStartModelloArmonia.UseVisualStyleBackColor = false;
            btnStartModelloArmonia.Click += BtnAvviaAnalisi_Click;
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
            groupBoxRaggruppamento.Size = new Size(327, 58);
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
            groupBoxBudget.Controls.Add(label5);
            groupBoxBudget.Controls.Add(numPrev);
            groupBoxBudget.Controls.Add(label4);
            groupBoxBudget.Controls.Add(numBudget);
            groupBoxBudget.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxBudget.Location = new Point(353, 3);
            groupBoxBudget.Name = "groupBoxBudget";
            groupBoxBudget.Size = new Size(316, 58);
            groupBoxBudget.TabIndex = 1;
            groupBoxBudget.TabStop = false;
            groupBoxBudget.Text = "Budget Disponibile";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(173, 24);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 3;
            label5.Text = "max prev";
            // 
            // numPrev
            // 
            numPrev.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numPrev.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numPrev.Location = new Point(240, 22);
            numPrev.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numPrev.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPrev.Name = "numPrev";
            numPrev.Size = new Size(45, 23);
            numPrev.TabIndex = 2;
            numPrev.TextAlign = HorizontalAlignment.Center;
            numPrev.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 24);
            label4.Name = "label4";
            label4.Size = new Size(48, 15);
            label4.TabIndex = 1;
            label4.Text = "Budget";
            // 
            // numBudget
            // 
            numBudget.DecimalPlaces = 2;
            numBudget.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            numBudget.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numBudget.Location = new Point(75, 22);
            numBudget.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numBudget.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numBudget.Name = "numBudget";
            numBudget.Size = new Size(75, 23);
            numBudget.TabIndex = 0;
            numBudget.TextAlign = HorizontalAlignment.Center;
            numBudget.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnAggiorna
            // 
            btnAggiorna.BackColor = Color.LightSteelBlue;
            btnAggiorna.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAggiorna.ForeColor = Color.White;
            btnAggiorna.Location = new Point(124, 138);
            btnAggiorna.Name = "btnAggiorna";
            btnAggiorna.Size = new Size(109, 44);
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
            btnEsporta.Location = new Point(9, 138);
            btnEsporta.Name = "btnEsporta";
            btnEsporta.Size = new Size(109, 44);
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
            panelContenuto.Location = new Point(0, 196);
            panelContenuto.Name = "panelContenuto";
            panelContenuto.Padding = new Padding(5);
            panelContenuto.Size = new Size(1358, 404);
            panelContenuto.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(5, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1348, 369);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(grigliaRisultati);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1340, 341);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Raggruppamento";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // grigliaRisultati
            // 
            grigliaRisultati.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            grigliaRisultati.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grigliaRisultati.AutoGenerateColumns = false;
            grigliaRisultati.BackgroundColor = Color.White;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            grigliaRisultati.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            grigliaRisultati.ColumnHeadersHeight = 27;
            grigliaRisultati.Columns.AddRange(new DataGridViewColumn[] { intervalloDataGridViewTextBoxColumn, dataInizioDataGridViewTextBoxColumn, dataFineDataGridViewTextBoxColumn, numeroEstrazioniDataGridViewTextBoxColumn, numeriGiocatiDataGridViewTextBoxColumn, numeriVintiDataGridViewTextBoxColumn, ambiVintiDataGridViewTextBoxColumn, terniVintiDataGridViewTextBoxColumn, investimentoMinimoDataGridViewTextBoxColumn, guadagnoDataGridViewTextBoxColumn, investimentoPropostoDataGridViewTextBoxColumn, guadagnoPropostoDataGridViewTextBoxColumn });
            grigliaRisultati.DataSource = risultatoAnalisiBindingSource;
            grigliaRisultati.Dock = DockStyle.Fill;
            grigliaRisultati.EnableHeadersVisualStyles = false;
            grigliaRisultati.Location = new Point(3, 3);
            grigliaRisultati.Name = "grigliaRisultati";
            grigliaRisultati.ReadOnly = true;
            grigliaRisultati.Size = new Size(1334, 335);
            grigliaRisultati.TabIndex = 0;
            // 
            // intervalloDataGridViewTextBoxColumn
            // 
            intervalloDataGridViewTextBoxColumn.DataPropertyName = "Intervallo";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            intervalloDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            intervalloDataGridViewTextBoxColumn.FillWeight = 200F;
            intervalloDataGridViewTextBoxColumn.HeaderText = "Intervallo";
            intervalloDataGridViewTextBoxColumn.Name = "intervalloDataGridViewTextBoxColumn";
            intervalloDataGridViewTextBoxColumn.ReadOnly = true;
            intervalloDataGridViewTextBoxColumn.Width = 220;
            // 
            // dataInizioDataGridViewTextBoxColumn
            // 
            dataInizioDataGridViewTextBoxColumn.DataPropertyName = "DataInizio";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataInizioDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
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
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataFineDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
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
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeroEstrazioniDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            numeroEstrazioniDataGridViewTextBoxColumn.FillWeight = 80F;
            numeroEstrazioniDataGridViewTextBoxColumn.HeaderText = "Num Estrazioni";
            numeroEstrazioniDataGridViewTextBoxColumn.Name = "numeroEstrazioniDataGridViewTextBoxColumn";
            numeroEstrazioniDataGridViewTextBoxColumn.ReadOnly = true;
            numeroEstrazioniDataGridViewTextBoxColumn.Width = 112;
            // 
            // numeriGiocatiDataGridViewTextBoxColumn
            // 
            numeriGiocatiDataGridViewTextBoxColumn.DataPropertyName = "NumeriGiocati";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeriGiocatiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            numeriGiocatiDataGridViewTextBoxColumn.HeaderText = "Num Giocati";
            numeriGiocatiDataGridViewTextBoxColumn.Name = "numeriGiocatiDataGridViewTextBoxColumn";
            numeriGiocatiDataGridViewTextBoxColumn.ReadOnly = true;
            numeriGiocatiDataGridViewTextBoxColumn.Width = 99;
            // 
            // numeriVintiDataGridViewTextBoxColumn
            // 
            numeriVintiDataGridViewTextBoxColumn.DataPropertyName = "NumeriVinti";
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            numeriVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            numeriVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            numeriVintiDataGridViewTextBoxColumn.HeaderText = "Num Vinti";
            numeriVintiDataGridViewTextBoxColumn.Name = "numeriVintiDataGridViewTextBoxColumn";
            numeriVintiDataGridViewTextBoxColumn.ReadOnly = true;
            numeriVintiDataGridViewTextBoxColumn.Width = 86;
            // 
            // ambiVintiDataGridViewTextBoxColumn
            // 
            ambiVintiDataGridViewTextBoxColumn.DataPropertyName = "AmbiVinti";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ambiVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            ambiVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            ambiVintiDataGridViewTextBoxColumn.HeaderText = "Ambi Vinti";
            ambiVintiDataGridViewTextBoxColumn.Name = "ambiVintiDataGridViewTextBoxColumn";
            ambiVintiDataGridViewTextBoxColumn.ReadOnly = true;
            ambiVintiDataGridViewTextBoxColumn.Width = 88;
            // 
            // terniVintiDataGridViewTextBoxColumn
            // 
            terniVintiDataGridViewTextBoxColumn.DataPropertyName = "TerniVinti";
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            terniVintiDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            terniVintiDataGridViewTextBoxColumn.FillWeight = 80F;
            terniVintiDataGridViewTextBoxColumn.HeaderText = "Terni Vinti";
            terniVintiDataGridViewTextBoxColumn.Name = "terniVintiDataGridViewTextBoxColumn";
            terniVintiDataGridViewTextBoxColumn.ReadOnly = true;
            terniVintiDataGridViewTextBoxColumn.Width = 85;
            // 
            // investimentoMinimoDataGridViewTextBoxColumn
            // 
            investimentoMinimoDataGridViewTextBoxColumn.DataPropertyName = "InvestimentoMinimo";
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            investimentoMinimoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle11;
            investimentoMinimoDataGridViewTextBoxColumn.FillWeight = 150F;
            investimentoMinimoDataGridViewTextBoxColumn.HeaderText = "Investimento Minimo";
            investimentoMinimoDataGridViewTextBoxColumn.Name = "investimentoMinimoDataGridViewTextBoxColumn";
            investimentoMinimoDataGridViewTextBoxColumn.ReadOnly = true;
            investimentoMinimoDataGridViewTextBoxColumn.Width = 146;
            // 
            // guadagnoDataGridViewTextBoxColumn
            // 
            guadagnoDataGridViewTextBoxColumn.DataPropertyName = "Guadagno";
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Format = "N2";
            guadagnoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle12;
            guadagnoDataGridViewTextBoxColumn.FillWeight = 120F;
            guadagnoDataGridViewTextBoxColumn.HeaderText = "Guadagno";
            guadagnoDataGridViewTextBoxColumn.Name = "guadagnoDataGridViewTextBoxColumn";
            guadagnoDataGridViewTextBoxColumn.ReadOnly = true;
            guadagnoDataGridViewTextBoxColumn.Width = 87;
            // 
            // investimentoPropostoDataGridViewTextBoxColumn
            // 
            investimentoPropostoDataGridViewTextBoxColumn.DataPropertyName = "InvestimentoProposto";
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Format = "N2";
            investimentoPropostoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle13;
            investimentoPropostoDataGridViewTextBoxColumn.FillWeight = 150F;
            investimentoPropostoDataGridViewTextBoxColumn.HeaderText = "Investimento Proposto";
            investimentoPropostoDataGridViewTextBoxColumn.Name = "investimentoPropostoDataGridViewTextBoxColumn";
            investimentoPropostoDataGridViewTextBoxColumn.ReadOnly = true;
            investimentoPropostoDataGridViewTextBoxColumn.Width = 152;
            // 
            // guadagnoPropostoDataGridViewTextBoxColumn
            // 
            guadagnoPropostoDataGridViewTextBoxColumn.DataPropertyName = "GuadagnoProposto";
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.Format = "N2";
            guadagnoPropostoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle14;
            guadagnoPropostoDataGridViewTextBoxColumn.FillWeight = 150F;
            guadagnoPropostoDataGridViewTextBoxColumn.HeaderText = "Guadagno Proposto";
            guadagnoPropostoDataGridViewTextBoxColumn.Name = "guadagnoPropostoDataGridViewTextBoxColumn";
            guadagnoPropostoDataGridViewTextBoxColumn.ReadOnly = true;
            guadagnoPropostoDataGridViewTextBoxColumn.Width = 138;
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
            tabPage2.Size = new Size(1340, 341);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Dettaglio Estrazioni";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // grigliaDettagliEsrtrazione
            // 
            grigliaDettagliEsrtrazione.AllowUserToAddRows = false;
            dataGridViewCellStyle15.BackColor = Color.WhiteSmoke;
            grigliaDettagliEsrtrazione.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            grigliaDettagliEsrtrazione.AutoGenerateColumns = false;
            grigliaDettagliEsrtrazione.BackgroundColor = Color.White;
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = SystemColors.Control;
            dataGridViewCellStyle16.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle16.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.True;
            grigliaDettagliEsrtrazione.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            grigliaDettagliEsrtrazione.ColumnHeadersHeight = 27;
            grigliaDettagliEsrtrazione.Columns.AddRange(new DataGridViewColumn[] { ruotaDataGridViewTextBoxColumn, dataDataGridViewTextBoxColumn, numPrevistiPesiDataGridViewTextBoxColumn, CountPrevisti, NumEstrattiPesi, NumEstratti, NumVinti });
            grigliaDettagliEsrtrazione.DataSource = risultatoEstrazioneBindingSource;
            grigliaDettagliEsrtrazione.Dock = DockStyle.Fill;
            grigliaDettagliEsrtrazione.EnableHeadersVisualStyles = false;
            grigliaDettagliEsrtrazione.Location = new Point(3, 3);
            grigliaDettagliEsrtrazione.Name = "grigliaDettagliEsrtrazione";
            grigliaDettagliEsrtrazione.ReadOnly = true;
            grigliaDettagliEsrtrazione.Size = new Size(1334, 335);
            grigliaDettagliEsrtrazione.TabIndex = 1;
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
            // CountPrevisti
            // 
            CountPrevisti.DataPropertyName = "CountPrevisti";
            CountPrevisti.HeaderText = "# Previsti";
            CountPrevisti.Name = "CountPrevisti";
            CountPrevisti.ReadOnly = true;
            // 
            // NumEstrattiPesi
            // 
            NumEstrattiPesi.DataPropertyName = "NumEstrattiPesi";
            NumEstrattiPesi.HeaderText = "Num Estratti (Pesi)";
            NumEstrattiPesi.Name = "NumEstrattiPesi";
            NumEstrattiPesi.ReadOnly = true;
            NumEstrattiPesi.Width = 250;
            // 
            // NumEstratti
            // 
            NumEstratti.DataPropertyName = "NumEstratti";
            NumEstratti.HeaderText = "Num Estratti";
            NumEstratti.Name = "NumEstratti";
            NumEstratti.ReadOnly = true;
            NumEstratti.Width = 200;
            // 
            // NumVinti
            // 
            NumVinti.DataPropertyName = "NumVinti";
            dataGridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleCenter;
            NumVinti.DefaultCellStyle = dataGridViewCellStyle17;
            NumVinti.HeaderText = "Num Vinti";
            NumVinti.Name = "NumVinti";
            NumVinti.ReadOnly = true;
            // 
            // risultatoEstrazioneBindingSource
            // 
            risultatoEstrazioneBindingSource.DataSource = typeof(RisultatoEstrazione);
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(txtResTest);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1340, 341);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Risultati Test";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtResTest
            // 
            txtResTest.BorderStyle = BorderStyle.None;
            txtResTest.Dock = DockStyle.Fill;
            txtResTest.Location = new Point(3, 3);
            txtResTest.Name = "txtResTest";
            txtResTest.Size = new Size(1334, 335);
            txtResTest.TabIndex = 0;
            txtResTest.Text = "";
            // 
            // lblTotali
            // 
            lblTotali.BackColor = Color.LightGray;
            lblTotali.Dock = DockStyle.Bottom;
            lblTotali.Font = new Font("Arial", 8F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotali.Location = new Point(5, 374);
            lblTotali.Name = "lblTotali";
            lblTotali.Padding = new Padding(5, 0, 0, 0);
            lblTotali.Size = new Size(1348, 25);
            lblTotali.TabIndex = 1;
            lblTotali.Text = "Totali:";
            lblTotali.TextAlign = ContentAlignment.MiddleLeft;
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
            gbRegole.ResumeLayout(false);
            gbRegole.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            gbInterval.ResumeLayout(false);
            gbInterval.PerformLayout();
            groupBoxRaggruppamento.ResumeLayout(false);
            groupBoxRaggruppamento.PerformLayout();
            groupBoxBudget.ResumeLayout(false);
            groupBoxBudget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPrev).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBudget).EndInit();
            panelContenuto.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaRisultati).EndInit();
            ((System.ComponentModel.ISupportInitialize)risultatoAnalisiBindingSource).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grigliaDettagliEsrtrazione).EndInit();
            ((System.ComponentModel.ISupportInitialize)risultatoEstrazioneBindingSource).EndInit();
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
        }
        private BindingSource risultatoAnalisiBindingSource;
        private Button btnStartModelloArmonia;
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
        private GroupBox gbInterval;
        private Label label1;
        private DateTimePicker dateTimeInizio;
        private Label label2;
        private DateTimePicker dateTimeFine;
        private GroupBox groupBox1;
        private Button btnAnalisiModQuantistico;
        private Button btnTest;
        private TabPage tabPage3;
        private RichTextBox txtResTest;
        private DateTimePicker dtDataTarget;
        private TextBox txtDbgNum;
        private Label label3;
        private Label lblNum;
        private Button btnStartModParametriOscillanti;
        private DataGridViewTextBoxColumn ruotaDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn numPrevistiPesiDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn CountPrevisti;
        private DataGridViewTextBoxColumn NumEstrattiPesi;
        private DataGridViewTextBoxColumn NumEstratti;
        private DataGridViewTextBoxColumn NumVinti;
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
        private GroupBox gbRegole;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private CheckBox checkBox9;
        private CheckBox checkBox10;
        private Label label5;
        private NumericUpDown numPrev;
        private Label label4;
        private CheckBox checkBox12;
        private CheckBox checkBox11;
        private Button btnTest2;
        private Button btnModCicli;
        private Button btnModGeometricoCubo;

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
