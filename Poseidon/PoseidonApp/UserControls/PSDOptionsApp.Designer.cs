namespace PoseidonApp.UserControls
{
    partial class PSDOptionsApp
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcTypeOptions = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkDbConnActTest = new System.Windows.Forms.CheckBox();
            this.chkDbConnActStd = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDBPROVIDER_TEST = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnConnectTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDbPwd_TEST = new System.Windows.Forms.TextBox();
            this.txtDbUser_TEST = new System.Windows.Forms.TextBox();
            this.txtDbServer_TEST = new System.Windows.Forms.TextBox();
            this.pnlConn = new System.Windows.Forms.Panel();
            this.cbDBPROVIDER = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDbPwd = new System.Windows.Forms.TextBox();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.txtDbServer = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gbDaysEstr = new System.Windows.Forms.GroupBox();
            this.chkDayEstr6 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr5 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr4 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr3 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr2 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr1 = new System.Windows.Forms.CheckBox();
            this.chkDayEstr0 = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnOpenFileEstr = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cbTipoFileImport = new System.Windows.Forms.ComboBox();
            this.txtURLFileEstr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileEstrPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSqlServerCeUprade = new System.Windows.Forms.Button();
            this.btnSelFileDB = new System.Windows.Forms.Button();
            this.tcTypeOptions.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlConn.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbDaysEstr.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcTypeOptions
            // 
            this.tcTypeOptions.Controls.Add(this.tabPage1);
            this.tcTypeOptions.Controls.Add(this.tabPage2);
            this.tcTypeOptions.Controls.Add(this.tabPage3);
            this.tcTypeOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTypeOptions.Location = new System.Drawing.Point(0, 0);
            this.tcTypeOptions.Name = "tcTypeOptions";
            this.tcTypeOptions.SelectedIndex = 0;
            this.tcTypeOptions.Size = new System.Drawing.Size(575, 350);
            this.tcTypeOptions.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkDbConnActTest);
            this.tabPage1.Controls.Add(this.chkDbConnActStd);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.pnlConn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(567, 324);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data Base";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkDbConnActTest
            // 
            this.chkDbConnActTest.AutoSize = true;
            this.chkDbConnActTest.Location = new System.Drawing.Point(271, 136);
            this.chkDbConnActTest.Name = "chkDbConnActTest";
            this.chkDbConnActTest.Size = new System.Drawing.Size(64, 17);
            this.chkDbConnActTest.TabIndex = 104;
            this.chkDbConnActTest.Text = "ATTIVA";
            this.chkDbConnActTest.UseVisualStyleBackColor = true;
            this.chkDbConnActTest.Click += new System.EventHandler(this.chkDbConnActStd_Click);
            // 
            // chkDbConnActStd
            // 
            this.chkDbConnActStd.AutoSize = true;
            this.chkDbConnActStd.Checked = true;
            this.chkDbConnActStd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDbConnActStd.Location = new System.Drawing.Point(271, 18);
            this.chkDbConnActStd.Name = "chkDbConnActStd";
            this.chkDbConnActStd.Size = new System.Drawing.Size(64, 17);
            this.chkDbConnActStd.TabIndex = 103;
            this.chkDbConnActStd.Text = "ATTIVA";
            this.chkDbConnActStd.UseVisualStyleBackColor = true;
            this.chkDbConnActStd.Click += new System.EventHandler(this.chkDbConnActStd_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(28, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 13);
            this.label9.TabIndex = 102;
            this.label9.Text = "CONNESSIONE TEST";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(28, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 13);
            this.label8.TabIndex = 101;
            this.label8.Text = "CONNESSIONE STANDARD ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDBPROVIDER_TEST);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.btnConnectTest);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtDbPwd_TEST);
            this.panel1.Controls.Add(this.txtDbUser_TEST);
            this.panel1.Controls.Add(this.txtDbServer_TEST);
            this.panel1.Location = new System.Drawing.Point(25, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 82);
            this.panel1.TabIndex = 100;
            // 
            // cbDBPROVIDER_TEST
            // 
            this.cbDBPROVIDER_TEST.FormattingEnabled = true;
            this.cbDBPROVIDER_TEST.Items.AddRange(new object[] {
            "System.Data.SqlServerCe.3.5",
            "System.Data.SQLClient",
            "Oracle.DataAccess.Client",
            "MySql.Data.MySqlClient"});
            this.cbDBPROVIDER_TEST.Location = new System.Drawing.Point(92, 58);
            this.cbDBPROVIDER_TEST.Name = "cbDBPROVIDER_TEST";
            this.cbDBPROVIDER_TEST.Size = new System.Drawing.Size(218, 21);
            this.cbDBPROVIDER_TEST.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "PROVIDER";
            // 
            // btnConnectTest
            // 
            this.btnConnectTest.Location = new System.Drawing.Point(408, 3);
            this.btnConnectTest.Name = "btnConnectTest";
            this.btnConnectTest.Size = new System.Drawing.Size(60, 46);
            this.btnConnectTest.TabIndex = 5;
            this.btnConnectTest.Text = "Connect";
            this.btnConnectTest.UseVisualStyleBackColor = true;
            this.btnConnectTest.Click += new System.EventHandler(this.btnConnectTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "DB User / pwd";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Server";
            // 
            // txtDbPwd_TEST
            // 
            this.txtDbPwd_TEST.Location = new System.Drawing.Point(200, 32);
            this.txtDbPwd_TEST.Name = "txtDbPwd_TEST";
            this.txtDbPwd_TEST.Size = new System.Drawing.Size(110, 20);
            this.txtDbPwd_TEST.TabIndex = 2;
            this.txtDbPwd_TEST.Text = "METB";
            // 
            // txtDbUser_TEST
            // 
            this.txtDbUser_TEST.Location = new System.Drawing.Point(92, 32);
            this.txtDbUser_TEST.Name = "txtDbUser_TEST";
            this.txtDbUser_TEST.Size = new System.Drawing.Size(102, 20);
            this.txtDbUser_TEST.TabIndex = 1;
            this.txtDbUser_TEST.Text = "METB";
            // 
            // txtDbServer_TEST
            // 
            this.txtDbServer_TEST.Location = new System.Drawing.Point(92, 6);
            this.txtDbServer_TEST.Name = "txtDbServer_TEST";
            this.txtDbServer_TEST.Size = new System.Drawing.Size(218, 20);
            this.txtDbServer_TEST.TabIndex = 0;
            this.txtDbServer_TEST.Text = "110.21.3.9:1521/orcl.absacciai.com";
            // 
            // pnlConn
            // 
            this.pnlConn.Controls.Add(this.btnSelFileDB);
            this.pnlConn.Controls.Add(this.btnSqlServerCeUprade);
            this.pnlConn.Controls.Add(this.cbDBPROVIDER);
            this.pnlConn.Controls.Add(this.label11);
            this.pnlConn.Controls.Add(this.btnConnect);
            this.pnlConn.Controls.Add(this.label2);
            this.pnlConn.Controls.Add(this.label1);
            this.pnlConn.Controls.Add(this.txtDbPwd);
            this.pnlConn.Controls.Add(this.txtDbUser);
            this.pnlConn.Controls.Add(this.txtDbServer);
            this.pnlConn.Location = new System.Drawing.Point(25, 35);
            this.pnlConn.Name = "pnlConn";
            this.pnlConn.Size = new System.Drawing.Size(536, 81);
            this.pnlConn.TabIndex = 99;
            // 
            // cbDBPROVIDER
            // 
            this.cbDBPROVIDER.FormattingEnabled = true;
            this.cbDBPROVIDER.Items.AddRange(new object[] {
            "System.Data.SqlServerCe.3.5",
            "System.Data.SqlServerCe.4.0",
            "System.Data.SQLClient",
            "Oracle.DataAccess.Client",
            "MySql.Data.MySqlClient"});
            this.cbDBPROVIDER.Location = new System.Drawing.Point(92, 58);
            this.cbDBPROVIDER.Name = "cbDBPROVIDER";
            this.cbDBPROVIDER.Size = new System.Drawing.Size(218, 21);
            this.cbDBPROVIDER.TabIndex = 9;
            this.cbDBPROVIDER.Text = "System.Data.SqlServerCe.4.0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "PROVIDER";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(408, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(60, 46);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "DB User / pwd";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server";
            // 
            // txtDbPwd
            // 
            this.txtDbPwd.Location = new System.Drawing.Point(200, 32);
            this.txtDbPwd.Name = "txtDbPwd";
            this.txtDbPwd.Size = new System.Drawing.Size(110, 20);
            this.txtDbPwd.TabIndex = 2;
            this.txtDbPwd.Text = "METB";
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(92, 32);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(102, 20);
            this.txtDbUser.TabIndex = 1;
            this.txtDbUser.Text = "METB";
            // 
            // txtDbServer
            // 
            this.txtDbServer.Location = new System.Drawing.Point(92, 6);
            this.txtDbServer.Name = "txtDbServer";
            this.txtDbServer.Size = new System.Drawing.Size(218, 20);
            this.txtDbServer.TabIndex = 0;
            this.txtDbServer.Text = "110.21.3.9:1521/orcl.absacciai.com";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gbDaysEstr);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(567, 324);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parametri Applicazione";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gbDaysEstr
            // 
            this.gbDaysEstr.Controls.Add(this.chkDayEstr6);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr5);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr4);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr3);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr2);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr1);
            this.gbDaysEstr.Controls.Add(this.chkDayEstr0);
            this.gbDaysEstr.Location = new System.Drawing.Point(20, 20);
            this.gbDaysEstr.Name = "gbDaysEstr";
            this.gbDaysEstr.Size = new System.Drawing.Size(141, 196);
            this.gbDaysEstr.TabIndex = 0;
            this.gbDaysEstr.TabStop = false;
            this.gbDaysEstr.Text = "Giorni di estrazione";
            // 
            // chkDayEstr6
            // 
            this.chkDayEstr6.AutoSize = true;
            this.chkDayEstr6.Location = new System.Drawing.Point(25, 165);
            this.chkDayEstr6.Name = "chkDayEstr6";
            this.chkDayEstr6.Size = new System.Drawing.Size(60, 17);
            this.chkDayEstr6.TabIndex = 6;
            this.chkDayEstr6.Text = "Sabato";
            this.chkDayEstr6.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr5
            // 
            this.chkDayEstr5.AutoSize = true;
            this.chkDayEstr5.Location = new System.Drawing.Point(25, 142);
            this.chkDayEstr5.Name = "chkDayEstr5";
            this.chkDayEstr5.Size = new System.Drawing.Size(62, 17);
            this.chkDayEstr5.TabIndex = 5;
            this.chkDayEstr5.Text = "Venerdì";
            this.chkDayEstr5.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr4
            // 
            this.chkDayEstr4.AutoSize = true;
            this.chkDayEstr4.Location = new System.Drawing.Point(25, 119);
            this.chkDayEstr4.Name = "chkDayEstr4";
            this.chkDayEstr4.Size = new System.Drawing.Size(62, 17);
            this.chkDayEstr4.TabIndex = 4;
            this.chkDayEstr4.Text = "Giovedì";
            this.chkDayEstr4.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr3
            // 
            this.chkDayEstr3.AutoSize = true;
            this.chkDayEstr3.Location = new System.Drawing.Point(25, 96);
            this.chkDayEstr3.Name = "chkDayEstr3";
            this.chkDayEstr3.Size = new System.Drawing.Size(72, 17);
            this.chkDayEstr3.TabIndex = 3;
            this.chkDayEstr3.Text = "Mercoledì";
            this.chkDayEstr3.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr2
            // 
            this.chkDayEstr2.AutoSize = true;
            this.chkDayEstr2.Location = new System.Drawing.Point(25, 73);
            this.chkDayEstr2.Name = "chkDayEstr2";
            this.chkDayEstr2.Size = new System.Drawing.Size(61, 17);
            this.chkDayEstr2.TabIndex = 2;
            this.chkDayEstr2.Text = "Martedì";
            this.chkDayEstr2.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr1
            // 
            this.chkDayEstr1.AutoSize = true;
            this.chkDayEstr1.Location = new System.Drawing.Point(25, 50);
            this.chkDayEstr1.Name = "chkDayEstr1";
            this.chkDayEstr1.Size = new System.Drawing.Size(58, 17);
            this.chkDayEstr1.TabIndex = 1;
            this.chkDayEstr1.Text = "Lunedi";
            this.chkDayEstr1.UseVisualStyleBackColor = true;
            // 
            // chkDayEstr0
            // 
            this.chkDayEstr0.AutoSize = true;
            this.chkDayEstr0.Location = new System.Drawing.Point(25, 27);
            this.chkDayEstr0.Name = "chkDayEstr0";
            this.chkDayEstr0.Size = new System.Drawing.Size(74, 17);
            this.chkDayEstr0.TabIndex = 0;
            this.chkDayEstr0.Text = "Domenica";
            this.chkDayEstr0.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnOpenFileEstr);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.cbTipoFileImport);
            this.tabPage3.Controls.Add(this.txtURLFileEstr);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.txtFileEstrPath);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(567, 324);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Import/Export";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnOpenFileEstr
            // 
            this.btnOpenFileEstr.Location = new System.Drawing.Point(469, 27);
            this.btnOpenFileEstr.Name = "btnOpenFileEstr";
            this.btnOpenFileEstr.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFileEstr.TabIndex = 6;
            this.btnOpenFileEstr.Text = "Seleziona";
            this.btnOpenFileEstr.UseVisualStyleBackColor = true;
            this.btnOpenFileEstr.Click += new System.EventHandler(this.btnOpenFileEstr_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Tipo file ";
            // 
            // cbTipoFileImport
            // 
            this.cbTipoFileImport.FormattingEnabled = true;
            this.cbTipoFileImport.Items.AddRange(new object[] {
            "1- da www.estrazionidellotto.com/estratti.txt: una riga per estrazione",
            "2- da https://www.lottomatica.it/STORICO_ESTRAZIONI_LOTTO/storico.zip: una riga p" +
                "er ruota"});
            this.cbTipoFileImport.Location = new System.Drawing.Point(134, 81);
            this.cbTipoFileImport.Name = "cbTipoFileImport";
            this.cbTipoFileImport.Size = new System.Drawing.Size(329, 21);
            this.cbTipoFileImport.TabIndex = 4;
            // 
            // txtURLFileEstr
            // 
            this.txtURLFileEstr.Location = new System.Drawing.Point(134, 55);
            this.txtURLFileEstr.Name = "txtURLFileEstr";
            this.txtURLFileEstr.Size = new System.Drawing.Size(329, 20);
            this.txtURLFileEstr.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "URL";
            // 
            // txtFileEstrPath
            // 
            this.txtFileEstrPath.Location = new System.Drawing.Point(134, 29);
            this.txtFileEstrPath.Name = "txtFileEstrPath";
            this.txtFileEstrPath.Size = new System.Drawing.Size(329, 20);
            this.txtFileEstrPath.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "File estrazioni";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSqlServerCeUprade
            // 
            this.btnSqlServerCeUprade.Location = new System.Drawing.Point(473, 4);
            this.btnSqlServerCeUprade.Name = "btnSqlServerCeUprade";
            this.btnSqlServerCeUprade.Size = new System.Drawing.Size(60, 46);
            this.btnSqlServerCeUprade.TabIndex = 10;
            this.btnSqlServerCeUprade.Text = "Upgrade CE to 4.0";
            this.btnSqlServerCeUprade.UseVisualStyleBackColor = true;
            this.btnSqlServerCeUprade.Click += new System.EventHandler(this.btnSqlServerCeUprade_Click);
            // 
            // btnSelFileDB
            // 
            this.btnSelFileDB.Location = new System.Drawing.Point(316, 4);
            this.btnSelFileDB.Name = "btnSelFileDB";
            this.btnSelFileDB.Size = new System.Drawing.Size(75, 23);
            this.btnSelFileDB.TabIndex = 11;
            this.btnSelFileDB.Text = "Seleziona";
            this.btnSelFileDB.UseVisualStyleBackColor = true;
            this.btnSelFileDB.Click += new System.EventHandler(this.btnSelFileDB_Click);
            // 
            // PSDOptionsApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcTypeOptions);
            this.Name = "PSDOptionsApp";
            this.Size = new System.Drawing.Size(575, 350);
            this.tcTypeOptions.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlConn.ResumeLayout(false);
            this.pnlConn.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gbDaysEstr.ResumeLayout(false);
            this.gbDaysEstr.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcTypeOptions;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pnlConn;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDbPwd;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.TextBox txtDbServer;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtFileEstrPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtURLFileEstr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbDaysEstr;
        private System.Windows.Forms.CheckBox chkDayEstr6;
        private System.Windows.Forms.CheckBox chkDayEstr5;
        private System.Windows.Forms.CheckBox chkDayEstr4;
        private System.Windows.Forms.CheckBox chkDayEstr3;
        private System.Windows.Forms.CheckBox chkDayEstr2;
        private System.Windows.Forms.CheckBox chkDayEstr1;
        private System.Windows.Forms.CheckBox chkDayEstr0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbTipoFileImport;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConnectTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDbPwd_TEST;
        private System.Windows.Forms.TextBox txtDbUser_TEST;
        private System.Windows.Forms.TextBox txtDbServer_TEST;
        private System.Windows.Forms.ComboBox cbDBPROVIDER_TEST;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbDBPROVIDER;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkDbConnActTest;
        private System.Windows.Forms.CheckBox chkDbConnActStd;
        private System.Windows.Forms.Button btnOpenFileEstr;
        private System.Windows.Forms.Button btnSqlServerCeUprade;
        private System.Windows.Forms.Button btnSelFileDB;
    }
}
