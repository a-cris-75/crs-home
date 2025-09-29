namespace PoseidonApp.UserControls
{
    partial class PSDChartAnalysis
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSDChartAnalysis));
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.View3DAppearance view3DAppearance1 = new Infragistics.UltraChart.Resources.Appearance.View3DAppearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chtPSD_L = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlTopOptions = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClearLastSeries = new System.Windows.Forms.Button();
            this.btnSumLines = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtMsecTimer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlTopLinesSpec = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSinH = new System.Windows.Forms.TextBox();
            this.txtSinR = new System.Windows.Forms.TextBox();
            this.txtSinMaxPercDifLen = new System.Windows.Forms.TextBox();
            this.chkSinH = new System.Windows.Forms.CheckBox();
            this.chkSinRad = new System.Windows.Forms.CheckBox();
            this.btnAddSinus = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlTopLines = new System.Windows.Forms.Panel();
            this.btnExportXls = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbColValX = new System.Windows.Forms.ComboBox();
            this.cbColValY = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddSeriesLine = new System.Windows.Forms.Button();
            this.cbColValZ = new System.Windows.Forms.ComboBox();
            this.btnAddSeriesPoint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlTopLayout = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnChartLeft = new System.Windows.Forms.Button();
            this.btnChartRight = new System.Windows.Forms.Button();
            this.btnChartZoomPlus = new System.Windows.Forms.Button();
            this.btnChartZoomMinus = new System.Windows.Forms.Button();
            this.chkZoomX = new System.Windows.Forms.CheckBox();
            this.chkZoomY = new System.Windows.Forms.CheckBox();
            this.txtDeltaZoom = new System.Windows.Forms.TextBox();
            this.btnResetZoom = new System.Windows.Forms.Button();
            this.chk3D = new System.Windows.Forms.CheckBox();
            this.pnlFillChart = new System.Windows.Forms.Panel();
            this.uchtPSD = new Infragistics.Win.UltraWinChart.UltraChart();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tcChrts = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chtPSD_P = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chtPSD_L)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlTopOptions.SuspendLayout();
            this.pnlTopLinesSpec.SuspendLayout();
            this.pnlTopLines.SuspendLayout();
            this.pnlTopLayout.SuspendLayout();
            this.pnlFillChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uchtPSD)).BeginInit();
            this.tcChrts.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtPSD_P)).BeginInit();
            this.SuspendLayout();
            // 
            // chtPSD_L
            // 
            this.chtPSD_L.AccessibleName = "\\";
            this.chtPSD_L.BorderlineColor = System.Drawing.Color.DarkGray;
            this.chtPSD_L.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Calibri", 8F);
            chartArea1.AxisX2.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.LineColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.BorderColor = System.Drawing.Color.Gray;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            chartArea1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chtPSD_L.ChartAreas.Add(chartArea1);
            this.chtPSD_L.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chtPSD_L.Legends.Add(legend1);
            this.chtPSD_L.Location = new System.Drawing.Point(0, 0);
            this.chtPSD_L.Name = "chtPSD_L";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.chtPSD_L.Series.Add(series1);
            this.chtPSD_L.Size = new System.Drawing.Size(1273, 210);
            this.chtPSD_L.TabIndex = 1;
            this.chtPSD_L.Text = "chart1";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlTopOptions);
            this.pnlTop.Controls.Add(this.pnlTopLinesSpec);
            this.pnlTop.Controls.Add(this.pnlTopLines);
            this.pnlTop.Controls.Add(this.pnlTopLayout);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1287, 111);
            this.pnlTop.TabIndex = 2;
            // 
            // pnlTopOptions
            // 
            this.pnlTopOptions.Controls.Add(this.label4);
            this.pnlTopOptions.Controls.Add(this.btnClearLastSeries);
            this.pnlTopOptions.Controls.Add(this.btnSumLines);
            this.pnlTopOptions.Controls.Add(this.btnPlay);
            this.pnlTopOptions.Controls.Add(this.txtMsecTimer);
            this.pnlTopOptions.Controls.Add(this.label7);
            this.pnlTopOptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopOptions.Location = new System.Drawing.Point(1048, 0);
            this.pnlTopOptions.Name = "pnlTopOptions";
            this.pnlTopOptions.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTopOptions.Size = new System.Drawing.Size(233, 111);
            this.pnlTopOptions.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(229, 22);
            this.label4.TabIndex = 32;
            this.label4.Text = "OPZIONI";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearLastSeries
            // 
            this.btnClearLastSeries.Location = new System.Drawing.Point(6, 27);
            this.btnClearLastSeries.Name = "btnClearLastSeries";
            this.btnClearLastSeries.Size = new System.Drawing.Size(108, 40);
            this.btnClearLastSeries.TabIndex = 9;
            this.btnClearLastSeries.Text = "Elimina ultima linea ";
            this.btnClearLastSeries.UseVisualStyleBackColor = true;
            this.btnClearLastSeries.Click += new System.EventHandler(this.btnClearLastSeries_Click);
            // 
            // btnSumLines
            // 
            this.btnSumLines.Location = new System.Drawing.Point(6, 68);
            this.btnSumLines.Name = "btnSumLines";
            this.btnSumLines.Size = new System.Drawing.Size(108, 40);
            this.btnSumLines.TabIndex = 19;
            this.btnSumLines.Text = "Somma linee consecutive";
            this.btnSumLines.UseVisualStyleBackColor = true;
            this.btnSumLines.Click += new System.EventHandler(this.btnSumLines_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPlay.ImageIndex = 24;
            this.btnPlay.ImageList = this.imageList1;
            this.btnPlay.Location = new System.Drawing.Point(117, 27);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(111, 40);
            this.btnPlay.TabIndex = 31;
            this.btnPlay.Text = "Scorri ultima linea";
            this.btnPlay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dice.ico");
            this.imageList1.Images.SetKeyName(1, "gear_wheel.png");
            this.imageList1.Images.SetKeyName(2, "lock.png");
            this.imageList1.Images.SetKeyName(3, "magnifying_glass.png");
            this.imageList1.Images.SetKeyName(4, "mailbox.ico");
            this.imageList1.Images.SetKeyName(5, "Norton Symantec.png");
            this.imageList1.Images.SetKeyName(6, "Norton System Works.png");
            this.imageList1.Images.SetKeyName(7, "Windows Media Player Alternate.png");
            this.imageList1.Images.SetKeyName(8, "Windows Stand By.png");
            this.imageList1.Images.SetKeyName(9, "Windows Turn Off.png");
            this.imageList1.Images.SetKeyName(10, "apple.ico");
            this.imageList1.Images.SetKeyName(11, "opt dossiers.ico");
            this.imageList1.Images.SetKeyName(12, "steam.ico");
            this.imageList1.Images.SetKeyName(13, "tux.ico");
            this.imageList1.Images.SetKeyName(14, "windows.ico");
            this.imageList1.Images.SetKeyName(15, "avedesk.ico");
            this.imageList1.Images.SetKeyName(16, "symantec.ico");
            this.imageList1.Images.SetKeyName(17, "wmp.ico");
            this.imageList1.Images.SetKeyName(18, "User.ico");
            this.imageList1.Images.SetKeyName(19, "EjectDisabled.ico");
            this.imageList1.Images.SetKeyName(20, "EjectNormalRed.ico");
            this.imageList1.Images.SetKeyName(21, "Play1Disabled.ico");
            this.imageList1.Images.SetKeyName(22, "Play1Hot.ico");
            this.imageList1.Images.SetKeyName(23, "Play1Pressed.ico");
            this.imageList1.Images.SetKeyName(24, "StepForwardPressed.ico");
            this.imageList1.Images.SetKeyName(25, "StepForwardPressedBlue.ico");
            this.imageList1.Images.SetKeyName(26, "Stop1NormalOrange.ico");
            this.imageList1.Images.SetKeyName(27, "PauseDisabled.ico");
            this.imageList1.Images.SetKeyName(28, "PauseHot.ico");
            this.imageList1.Images.SetKeyName(29, "PauseNormalRed.ico");
            this.imageList1.Images.SetKeyName(30, "Activity Monitor.ico");
            this.imageList1.Images.SetKeyName(31, "Earth.ico");
            this.imageList1.Images.SetKeyName(32, "iSync.ico");
            this.imageList1.Images.SetKeyName(33, "Setting.ico");
            this.imageList1.Images.SetKeyName(34, "System Preferences.ico");
            // 
            // txtMsecTimer
            // 
            this.txtMsecTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsecTimer.Location = new System.Drawing.Point(170, 78);
            this.txtMsecTimer.Name = "txtMsecTimer";
            this.txtMsecTimer.Size = new System.Drawing.Size(56, 23);
            this.txtMsecTimer.TabIndex = 33;
            this.txtMsecTimer.Text = "200";
            this.txtMsecTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(129, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "msec";
            // 
            // pnlTopLinesSpec
            // 
            this.pnlTopLinesSpec.Controls.Add(this.label9);
            this.pnlTopLinesSpec.Controls.Add(this.txtSinH);
            this.pnlTopLinesSpec.Controls.Add(this.txtSinR);
            this.pnlTopLinesSpec.Controls.Add(this.txtSinMaxPercDifLen);
            this.pnlTopLinesSpec.Controls.Add(this.chkSinH);
            this.pnlTopLinesSpec.Controls.Add(this.chkSinRad);
            this.pnlTopLinesSpec.Controls.Add(this.btnAddSinus);
            this.pnlTopLinesSpec.Controls.Add(this.label5);
            this.pnlTopLinesSpec.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopLinesSpec.Location = new System.Drawing.Point(711, 0);
            this.pnlTopLinesSpec.Name = "pnlTopLinesSpec";
            this.pnlTopLinesSpec.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTopLinesSpec.Size = new System.Drawing.Size(337, 111);
            this.pnlTopLinesSpec.TabIndex = 36;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(2, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(333, 22);
            this.label9.TabIndex = 31;
            this.label9.Text = "SINUSOIDE";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSinH
            // 
            this.txtSinH.Enabled = false;
            this.txtSinH.Location = new System.Drawing.Point(99, 41);
            this.txtSinH.Name = "txtSinH";
            this.txtSinH.Size = new System.Drawing.Size(35, 22);
            this.txtSinH.TabIndex = 22;
            this.txtSinH.Text = "0";
            this.txtSinH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSinR
            // 
            this.txtSinR.Location = new System.Drawing.Point(99, 67);
            this.txtSinR.Name = "txtSinR";
            this.txtSinR.Size = new System.Drawing.Size(35, 22);
            this.txtSinR.TabIndex = 23;
            this.txtSinR.Text = "0";
            this.txtSinR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSinMaxPercDifLen
            // 
            this.txtSinMaxPercDifLen.Location = new System.Drawing.Point(140, 67);
            this.txtSinMaxPercDifLen.Name = "txtSinMaxPercDifLen";
            this.txtSinMaxPercDifLen.Size = new System.Drawing.Size(38, 22);
            this.txtSinMaxPercDifLen.TabIndex = 24;
            this.txtSinMaxPercDifLen.Text = "0.1";
            this.txtSinMaxPercDifLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtSinMaxPercDifLen, "Max % diffferenza di lunghezza dalla linea generatrice");
            // 
            // chkSinH
            // 
            this.chkSinH.AutoSize = true;
            this.chkSinH.Location = new System.Drawing.Point(11, 45);
            this.chkSinH.Name = "chkSinH";
            this.chkSinH.Size = new System.Drawing.Size(63, 18);
            this.chkSinH.TabIndex = 26;
            this.chkSinH.Text = "Raggio";
            this.toolTip1.SetToolTip(this.chkSinH, "Se check imposta il raggio, altrimenti calcola la media dei valori di y diviso du" +
        "e della linea generatrice");
            this.chkSinH.UseVisualStyleBackColor = true;
            this.chkSinH.Click += new System.EventHandler(this.chkSinAutoH_Click);
            // 
            // chkSinRad
            // 
            this.chkSinRad.AutoSize = true;
            this.chkSinRad.Location = new System.Drawing.Point(11, 71);
            this.chkSinRad.Name = "chkSinRad";
            this.chkSinRad.Size = new System.Drawing.Size(72, 18);
            this.chkSinRad.TabIndex = 27;
            this.chkSinRad.Text = "Radianti";
            this.toolTip1.SetToolTip(this.chkSinRad, "Se check imposta il radianti del cerchio o ellisse usata per generare la sinusoid" +
        "e, \r\naltrimenti utilizza il cerchio di raggio impostato sopra ");
            this.chkSinRad.UseVisualStyleBackColor = true;
            // 
            // btnAddSinus
            // 
            this.btnAddSinus.Location = new System.Drawing.Point(221, 46);
            this.btnAddSinus.Name = "btnAddSinus";
            this.btnAddSinus.Size = new System.Drawing.Size(108, 40);
            this.btnAddSinus.TabIndex = 30;
            this.btnAddSinus.Text = "Aggiungi linea sinusoide";
            this.btnAddSinus.UseVisualStyleBackColor = true;
            this.btnAddSinus.Click += new System.EventHandler(this.btnAddSinus_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 14);
            this.label5.TabIndex = 28;
            this.label5.Text = "%";
            // 
            // pnlTopLines
            // 
            this.pnlTopLines.Controls.Add(this.btnExportXls);
            this.pnlTopLines.Controls.Add(this.label8);
            this.pnlTopLines.Controls.Add(this.label1);
            this.pnlTopLines.Controls.Add(this.cbColValX);
            this.pnlTopLines.Controls.Add(this.cbColValY);
            this.pnlTopLines.Controls.Add(this.label2);
            this.pnlTopLines.Controls.Add(this.btnAddSeriesLine);
            this.pnlTopLines.Controls.Add(this.cbColValZ);
            this.pnlTopLines.Controls.Add(this.btnAddSeriesPoint);
            this.pnlTopLines.Controls.Add(this.label3);
            this.pnlTopLines.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopLines.Location = new System.Drawing.Point(200, 0);
            this.pnlTopLines.Name = "pnlTopLines";
            this.pnlTopLines.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTopLines.Size = new System.Drawing.Size(511, 111);
            this.pnlTopLines.TabIndex = 35;
            // 
            // btnExportXls
            // 
            this.btnExportXls.Location = new System.Drawing.Point(397, 26);
            this.btnExportXls.Name = "btnExportXls";
            this.btnExportXls.Size = new System.Drawing.Size(108, 40);
            this.btnExportXls.TabIndex = 31;
            this.btnExportXls.Text = "Esporta in Xls (linee 2D o 3D)";
            this.btnExportXls.UseVisualStyleBackColor = true;
            this.btnExportXls.Click += new System.EventHandler(this.btnExportXls_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(2, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(507, 22);
            this.label8.TabIndex = 30;
            this.label8.Text = "LINEE";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "Val asse X";
            // 
            // cbColValX
            // 
            this.cbColValX.FormattingEnabled = true;
            this.cbColValX.Location = new System.Drawing.Point(79, 29);
            this.cbColValX.Name = "cbColValX";
            this.cbColValX.Size = new System.Drawing.Size(188, 22);
            this.cbColValX.TabIndex = 4;
            // 
            // cbColValY
            // 
            this.cbColValY.FormattingEnabled = true;
            this.cbColValY.Location = new System.Drawing.Point(79, 54);
            this.cbColValY.Name = "cbColValY";
            this.cbColValY.Size = new System.Drawing.Size(188, 22);
            this.cbColValY.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "Val asse Y";
            // 
            // btnAddSeriesLine
            // 
            this.btnAddSeriesLine.Location = new System.Drawing.Point(284, 26);
            this.btnAddSeriesLine.Name = "btnAddSeriesLine";
            this.btnAddSeriesLine.Size = new System.Drawing.Size(108, 40);
            this.btnAddSeriesLine.TabIndex = 8;
            this.btnAddSeriesLine.Text = "Aggiungi in grafico Linee";
            this.toolTip1.SetToolTip(this.btnAddSeriesLine, "Genera una linea nel grafico in base alle colonne scelte (Val asse X, Y)");
            this.btnAddSeriesLine.UseVisualStyleBackColor = true;
            this.btnAddSeriesLine.Click += new System.EventHandler(this.btnAddSeries_Click);
            // 
            // cbColValZ
            // 
            this.cbColValZ.FormattingEnabled = true;
            this.cbColValZ.Location = new System.Drawing.Point(79, 79);
            this.cbColValZ.Name = "cbColValZ";
            this.cbColValZ.Size = new System.Drawing.Size(188, 22);
            this.cbColValZ.TabIndex = 14;
            // 
            // btnAddSeriesPoint
            // 
            this.btnAddSeriesPoint.Location = new System.Drawing.Point(284, 67);
            this.btnAddSeriesPoint.Name = "btnAddSeriesPoint";
            this.btnAddSeriesPoint.Size = new System.Drawing.Size(108, 40);
            this.btnAddSeriesPoint.TabIndex = 29;
            this.btnAddSeriesPoint.Text = "Aggiungi in grafico Punti";
            this.btnAddSeriesPoint.UseVisualStyleBackColor = true;
            this.btnAddSeriesPoint.Click += new System.EventHandler(this.btnAddSeriesPoint_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 14);
            this.label3.TabIndex = 15;
            this.label3.Text = "Val asse Z";
            // 
            // pnlTopLayout
            // 
            this.pnlTopLayout.Controls.Add(this.label6);
            this.pnlTopLayout.Controls.Add(this.btnChartLeft);
            this.pnlTopLayout.Controls.Add(this.btnChartRight);
            this.pnlTopLayout.Controls.Add(this.btnChartZoomPlus);
            this.pnlTopLayout.Controls.Add(this.btnChartZoomMinus);
            this.pnlTopLayout.Controls.Add(this.chkZoomX);
            this.pnlTopLayout.Controls.Add(this.chkZoomY);
            this.pnlTopLayout.Controls.Add(this.txtDeltaZoom);
            this.pnlTopLayout.Controls.Add(this.btnResetZoom);
            this.pnlTopLayout.Controls.Add(this.chk3D);
            this.pnlTopLayout.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLayout.Name = "pnlTopLayout";
            this.pnlTopLayout.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTopLayout.Size = new System.Drawing.Size(200, 111);
            this.pnlTopLayout.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(2, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 22);
            this.label6.TabIndex = 17;
            this.label6.Text = "LAYOUT";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChartLeft
            // 
            this.btnChartLeft.Location = new System.Drawing.Point(3, 27);
            this.btnChartLeft.Name = "btnChartLeft";
            this.btnChartLeft.Size = new System.Drawing.Size(50, 23);
            this.btnChartLeft.TabIndex = 0;
            this.btnChartLeft.Text = "<";
            this.btnChartLeft.UseVisualStyleBackColor = true;
            this.btnChartLeft.Click += new System.EventHandler(this.btnChartLeft_Click);
            // 
            // btnChartRight
            // 
            this.btnChartRight.Location = new System.Drawing.Point(57, 27);
            this.btnChartRight.Name = "btnChartRight";
            this.btnChartRight.Size = new System.Drawing.Size(50, 23);
            this.btnChartRight.TabIndex = 1;
            this.btnChartRight.Text = ">";
            this.btnChartRight.UseVisualStyleBackColor = true;
            this.btnChartRight.Click += new System.EventHandler(this.btnChartRight_Click);
            // 
            // btnChartZoomPlus
            // 
            this.btnChartZoomPlus.Location = new System.Drawing.Point(3, 53);
            this.btnChartZoomPlus.Name = "btnChartZoomPlus";
            this.btnChartZoomPlus.Size = new System.Drawing.Size(50, 23);
            this.btnChartZoomPlus.TabIndex = 2;
            this.btnChartZoomPlus.Text = "+";
            this.btnChartZoomPlus.UseVisualStyleBackColor = true;
            this.btnChartZoomPlus.Click += new System.EventHandler(this.btnChartZoomPlus_Click);
            // 
            // btnChartZoomMinus
            // 
            this.btnChartZoomMinus.Location = new System.Drawing.Point(57, 53);
            this.btnChartZoomMinus.Name = "btnChartZoomMinus";
            this.btnChartZoomMinus.Size = new System.Drawing.Size(50, 23);
            this.btnChartZoomMinus.TabIndex = 3;
            this.btnChartZoomMinus.Text = "-";
            this.btnChartZoomMinus.UseVisualStyleBackColor = true;
            this.btnChartZoomMinus.Click += new System.EventHandler(this.btnChartZoomMinus_Click);
            // 
            // chkZoomX
            // 
            this.chkZoomX.AutoSize = true;
            this.chkZoomX.Checked = true;
            this.chkZoomX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZoomX.Location = new System.Drawing.Point(115, 56);
            this.chkZoomX.Name = "chkZoomX";
            this.chkZoomX.Size = new System.Drawing.Size(32, 18);
            this.chkZoomX.TabIndex = 10;
            this.chkZoomX.Text = "X";
            this.chkZoomX.UseVisualStyleBackColor = true;
            // 
            // chkZoomY
            // 
            this.chkZoomY.AutoSize = true;
            this.chkZoomY.Checked = true;
            this.chkZoomY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZoomY.Location = new System.Drawing.Point(115, 75);
            this.chkZoomY.Name = "chkZoomY";
            this.chkZoomY.Size = new System.Drawing.Size(32, 18);
            this.chkZoomY.TabIndex = 11;
            this.chkZoomY.Text = "Y";
            this.chkZoomY.UseVisualStyleBackColor = true;
            // 
            // txtDeltaZoom
            // 
            this.txtDeltaZoom.Location = new System.Drawing.Point(113, 30);
            this.txtDeltaZoom.Name = "txtDeltaZoom";
            this.txtDeltaZoom.Size = new System.Drawing.Size(56, 22);
            this.txtDeltaZoom.TabIndex = 12;
            this.txtDeltaZoom.Text = "1000";
            this.txtDeltaZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnResetZoom
            // 
            this.btnResetZoom.Location = new System.Drawing.Point(3, 79);
            this.btnResetZoom.Name = "btnResetZoom";
            this.btnResetZoom.Size = new System.Drawing.Size(104, 23);
            this.btnResetZoom.TabIndex = 13;
            this.btnResetZoom.Text = "ZOOM reset";
            this.btnResetZoom.UseVisualStyleBackColor = true;
            this.btnResetZoom.Click += new System.EventHandler(this.btnResetZoom_Click);
            // 
            // chk3D
            // 
            this.chk3D.AutoSize = true;
            this.chk3D.Checked = true;
            this.chk3D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk3D.Location = new System.Drawing.Point(154, 75);
            this.chk3D.Name = "chk3D";
            this.chk3D.Size = new System.Drawing.Size(40, 18);
            this.chk3D.TabIndex = 16;
            this.chk3D.Text = "3D";
            this.chk3D.UseVisualStyleBackColor = true;
            this.chk3D.Click += new System.EventHandler(this.chk3D_Click);
            // 
            // pnlFillChart
            // 
            this.pnlFillChart.Controls.Add(this.chtPSD_L);
            this.pnlFillChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFillChart.Location = new System.Drawing.Point(3, 3);
            this.pnlFillChart.Name = "pnlFillChart";
            this.pnlFillChart.Size = new System.Drawing.Size(1273, 210);
            this.pnlFillChart.TabIndex = 3;
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.uchtPSD.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart3D;
            // 
            // uchtPSD
            // 
            this.uchtPSD.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.uchtPSD.Axis.PE = paintElement1;
            this.uchtPSD.Axis.X.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.uchtPSD.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.uchtPSD.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.uchtPSD.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.uchtPSD.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.X.LineThickness = 1;
            this.uchtPSD.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.X.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.X.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.X.Visible = true;
            this.uchtPSD.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.uchtPSD.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.uchtPSD.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.X2.Labels.Visible = false;
            this.uchtPSD.Axis.X2.LineThickness = 1;
            this.uchtPSD.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.X2.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.X2.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.X2.Visible = false;
            this.uchtPSD.Axis.Y.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.uchtPSD.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Y.LineThickness = 1;
            this.uchtPSD.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Y.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Y.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.Y.TickmarkInterval = 10D;
            this.uchtPSD.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.Y.Visible = true;
            this.uchtPSD.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.uchtPSD.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Y2.Labels.Visible = false;
            this.uchtPSD.Axis.Y2.LineThickness = 1;
            this.uchtPSD.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Y2.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Y2.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.Y2.TickmarkInterval = 10D;
            this.uchtPSD.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.Y2.Visible = false;
            this.uchtPSD.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.Z.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.uchtPSD.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Z.LineThickness = 1;
            this.uchtPSD.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Z.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Z.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.Z.Visible = true;
            this.uchtPSD.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.Z2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.uchtPSD.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.uchtPSD.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.uchtPSD.Axis.Z2.Labels.Visible = false;
            this.uchtPSD.Axis.Z2.LineThickness = 1;
            this.uchtPSD.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.uchtPSD.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Z2.MajorGridLines.Visible = true;
            this.uchtPSD.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.uchtPSD.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.uchtPSD.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.uchtPSD.Axis.Z2.MinorGridLines.Visible = false;
            this.uchtPSD.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.uchtPSD.Axis.Z2.Visible = false;
            this.uchtPSD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.uchtPSD.ColorModel.AlphaLevel = ((byte)(255));
            this.uchtPSD.ColorModel.ColorBegin = System.Drawing.Color.Pink;
            this.uchtPSD.ColorModel.ColorEnd = System.Drawing.Color.DarkRed;
            this.uchtPSD.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.uchtPSD.ColorModel.Scaling = Infragistics.UltraChart.Shared.Styles.ColorScaling.Increasing;
            this.uchtPSD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uchtPSD.Effects.Effects.Add(gradientEffect1);
            this.uchtPSD.Location = new System.Drawing.Point(3, 3);
            this.uchtPSD.Name = "uchtPSD";
            this.uchtPSD.Size = new System.Drawing.Size(1273, 210);
            this.uchtPSD.TabIndex = 2;
            this.uchtPSD.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.uchtPSD.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            view3DAppearance1.XRotation = 130F;
            view3DAppearance1.YRotation = 5F;
            this.uchtPSD.Transform3D = view3DAppearance1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 353);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1287, 13);
            this.pnlBottom.TabIndex = 0;
            this.pnlBottom.Visible = false;
            // 
            // tcChrts
            // 
            this.tcChrts.Controls.Add(this.tabPage1);
            this.tcChrts.Controls.Add(this.tabPage2);
            this.tcChrts.Controls.Add(this.tabPage3);
            this.tcChrts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcChrts.Location = new System.Drawing.Point(0, 111);
            this.tcChrts.Name = "tcChrts";
            this.tcChrts.SelectedIndex = 0;
            this.tcChrts.Size = new System.Drawing.Size(1287, 242);
            this.tcChrts.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlFillChart);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1279, 216);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Linee";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uchtPSD);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1279, 216);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Linee 3D";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chtPSD_P);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1279, 216);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Punti";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chtPSD_P
            // 
            this.chtPSD_P.BorderlineColor = System.Drawing.Color.DarkGray;
            this.chtPSD_P.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisX.LineColor = System.Drawing.Color.DarkGray;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Calibri", 8F);
            chartArea2.AxisX2.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisY.LineColor = System.Drawing.Color.DarkGray;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisY2.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea2.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.BorderColor = System.Drawing.Color.Gray;
            chartArea2.CursorX.IsUserEnabled = true;
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.CursorY.IsUserEnabled = true;
            chartArea2.CursorY.IsUserSelectionEnabled = true;
            chartArea2.Name = "ChartArea1";
            chartArea2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chtPSD_P.ChartAreas.Add(chartArea2);
            this.chtPSD_P.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chtPSD_P.Legends.Add(legend2);
            this.chtPSD_P.Location = new System.Drawing.Point(3, 3);
            this.chtPSD_P.Name = "chtPSD_P";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.YValuesPerPoint = 2;
            this.chtPSD_P.Series.Add(series2);
            this.chtPSD_P.Size = new System.Drawing.Size(1273, 210);
            this.chtPSD_P.TabIndex = 2;
            this.chtPSD_P.Text = "chart1";
            // 
            // timer1
            // 
            this.timer1.Tag = "0";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PosChartAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcChrts);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "PosChartAnalysis";
            this.Size = new System.Drawing.Size(1287, 366);
            ((System.ComponentModel.ISupportInitialize)(this.chtPSD_L)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTopOptions.ResumeLayout(false);
            this.pnlTopOptions.PerformLayout();
            this.pnlTopLinesSpec.ResumeLayout(false);
            this.pnlTopLinesSpec.PerformLayout();
            this.pnlTopLines.ResumeLayout(false);
            this.pnlTopLines.PerformLayout();
            this.pnlTopLayout.ResumeLayout(false);
            this.pnlTopLayout.PerformLayout();
            this.pnlFillChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uchtPSD)).EndInit();
            this.tcChrts.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtPSD_P)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chtPSD_L;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnChartRight;
        private System.Windows.Forms.Button btnChartLeft;
        private System.Windows.Forms.Panel pnlFillChart;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnChartZoomMinus;
        private System.Windows.Forms.Button btnChartZoomPlus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbColValY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbColValX;
        private System.Windows.Forms.Button btnAddSeriesLine;
        private System.Windows.Forms.Button btnClearLastSeries;
        private System.Windows.Forms.CheckBox chkZoomY;
        private System.Windows.Forms.CheckBox chkZoomX;
        private System.Windows.Forms.TextBox txtDeltaZoom;
        private System.Windows.Forms.Button btnResetZoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbColValZ;
        private System.Windows.Forms.CheckBox chk3D;
        private Infragistics.Win.UltraWinChart.UltraChart uchtPSD;
        private System.Windows.Forms.TabControl tcChrts;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnSumLines;
        private System.Windows.Forms.TextBox txtSinR;
        private System.Windows.Forms.TextBox txtSinH;
        private System.Windows.Forms.TextBox txtSinMaxPercDifLen;
        private System.Windows.Forms.CheckBox chkSinH;
        private System.Windows.Forms.CheckBox chkSinRad;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddSeriesPoint;
        private System.Windows.Forms.Button btnAddSinus;
        private System.Windows.Forms.TextBox txtMsecTimer;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtPSD_P;
        private System.Windows.Forms.Panel pnlTopLayout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlTopOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlTopLinesSpec;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlTopLines;
        private System.Windows.Forms.Button btnExportXls;
        private System.Windows.Forms.Label label8;
    }
}
