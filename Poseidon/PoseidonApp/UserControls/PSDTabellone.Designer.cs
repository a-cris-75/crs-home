namespace PoseidonApp.UserControls
{
    partial class PSDTabellone
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSDTabellone));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.pnlParamsCalc = new System.Windows.Forms.Panel();
            this.pnlR1Params = new System.Windows.Forms.Panel();
            this.psdSceltaR12 = new PoseidonApp.UserControls.PSDSceltaR1();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlParamsCalcSettings = new System.Windows.Forms.Panel();
            this.psdViewParamsNumInGioco1 = new PoseidonApp.UserControls.PSDViewParamsNumInGioco();
            this.pnlBtnsTop = new System.Windows.Forms.Panel();
            this.btnEvidenzia = new System.Windows.Forms.Button();
            this.btnInfoFunzioni = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnNumInGioco = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.btnCalcRes = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotRighe = new System.Windows.Forms.TextBox();
            this.txtMsecTimer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnEvidenziaCalcola = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.tcResGrid = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ugTabellone = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.bsTabellone = new System.Windows.Forms.BindingSource(this.components);
            this.pnlDim = new System.Windows.Forms.Panel();
            this.btnDimM = new System.Windows.Forms.Button();
            this.btnDimP = new System.Windows.Forms.Button();
            this.pnlColors = new System.Windows.Forms.Panel();
            this.psdColorTab2 = new PoseidonApp.UserControls.PSDColorTab();
            this.pnlBtnsEvidenzia = new System.Windows.Forms.Panel();
            this.btnInfoEvidenzia = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlOptions.SuspendLayout();
            this.pnlParamsCalc.SuspendLayout();
            this.pnlR1Params.SuspendLayout();
            this.pnlParamsCalcSettings.SuspendLayout();
            this.pnlBtnsTop.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.tcResGrid.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugTabellone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTabellone)).BeginInit();
            this.pnlDim.SuspendLayout();
            this.pnlColors.SuspendLayout();
            this.pnlBtnsEvidenzia.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.pnlParamsCalc);
            this.pnlOptions.Controls.Add(this.pnlBtnsTop);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Padding = new System.Windows.Forms.Padding(2);
            this.pnlOptions.Size = new System.Drawing.Size(1708, 112);
            this.pnlOptions.TabIndex = 0;
            // 
            // pnlParamsCalc
            // 
            this.pnlParamsCalc.Controls.Add(this.pnlR1Params);
            this.pnlParamsCalc.Controls.Add(this.pnlParamsCalcSettings);
            this.pnlParamsCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParamsCalc.Location = new System.Drawing.Point(113, 2);
            this.pnlParamsCalc.Name = "pnlParamsCalc";
            this.pnlParamsCalc.Size = new System.Drawing.Size(1593, 108);
            this.pnlParamsCalc.TabIndex = 15;
            // 
            // pnlR1Params
            // 
            this.pnlR1Params.Controls.Add(this.psdSceltaR12);
            this.pnlR1Params.Controls.Add(this.label9);
            this.pnlR1Params.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlR1Params.Location = new System.Drawing.Point(674, 0);
            this.pnlR1Params.Name = "pnlR1Params";
            this.pnlR1Params.Padding = new System.Windows.Forms.Padding(2);
            this.pnlR1Params.Size = new System.Drawing.Size(445, 108);
            this.pnlR1Params.TabIndex = 5;
            this.pnlR1Params.Visible = false;
            // 
            // psdSceltaR12
            // 
            this.psdSceltaR12.ACCOPPIATE_DEC_R12 = ((System.Collections.Generic.List<string>)(resources.GetObject("psdSceltaR12.ACCOPPIATE_DEC_R12")));
            this.psdSceltaR12.COLPI = ((System.Collections.Generic.List<int>)(resources.GetObject("psdSceltaR12.COLPI")));
            this.psdSceltaR12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdSceltaR12.Location = new System.Drawing.Point(2, 27);
            this.psdSceltaR12.Name = "psdSceltaR12";
            this.psdSceltaR12.POS_DEC_R12 = ((System.Collections.Generic.List<string>)(resources.GetObject("psdSceltaR12.POS_DEC_R12")));
            this.psdSceltaR12.Size = new System.Drawing.Size(441, 79);
            this.psdSceltaR12.TabIndex = 0;
            this.psdSceltaR12.TIPO_FREQUENZA = ((System.Collections.Generic.List<int>)(resources.GetObject("psdSceltaR12.TIPO_FREQUENZA")));
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(2, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(441, 25);
            this.label9.TabIndex = 21;
            this.label9.Text = "PARAMETRI Regola 1";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlParamsCalcSettings
            // 
            this.pnlParamsCalcSettings.Controls.Add(this.psdViewParamsNumInGioco1);
            this.pnlParamsCalcSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlParamsCalcSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlParamsCalcSettings.Name = "pnlParamsCalcSettings";
            this.pnlParamsCalcSettings.Padding = new System.Windows.Forms.Padding(2);
            this.pnlParamsCalcSettings.Size = new System.Drawing.Size(674, 108);
            this.pnlParamsCalcSettings.TabIndex = 16;
            // 
            // psdViewParamsNumInGioco1
            // 
            this.psdViewParamsNumInGioco1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdViewParamsNumInGioco1.Location = new System.Drawing.Point(2, 2);
            this.psdViewParamsNumInGioco1.Name = "psdViewParamsNumInGioco1";
            this.psdViewParamsNumInGioco1.Size = new System.Drawing.Size(670, 104);
            this.psdViewParamsNumInGioco1.TabIndex = 0;
            this.psdViewParamsNumInGioco1.TYPE_NUM_IN_GIOCO = 1;
            // 
            // pnlBtnsTop
            // 
            this.pnlBtnsTop.Controls.Add(this.btnEvidenzia);
            this.pnlBtnsTop.Controls.Add(this.btnInfoFunzioni);
            this.pnlBtnsTop.Controls.Add(this.label6);
            this.pnlBtnsTop.Controls.Add(this.btnNumInGioco);
            this.pnlBtnsTop.Controls.Add(this.btnFill);
            this.pnlBtnsTop.Controls.Add(this.btnCalcRes);
            this.pnlBtnsTop.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBtnsTop.Location = new System.Drawing.Point(2, 2);
            this.pnlBtnsTop.Name = "pnlBtnsTop";
            this.pnlBtnsTop.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBtnsTop.Size = new System.Drawing.Size(111, 108);
            this.pnlBtnsTop.TabIndex = 3;
            // 
            // btnEvidenzia
            // 
            this.btnEvidenzia.BackColor = System.Drawing.Color.LightGray;
            this.btnEvidenzia.Location = new System.Drawing.Point(3, 66);
            this.btnEvidenzia.Name = "btnEvidenzia";
            this.btnEvidenzia.Size = new System.Drawing.Size(104, 39);
            this.btnEvidenzia.TabIndex = 24;
            this.btnEvidenzia.Text = "Evidenzia";
            this.btnEvidenzia.UseVisualStyleBackColor = false;
            this.btnEvidenzia.Click += new System.EventHandler(this.BtnEvidenzia_Click);
            // 
            // btnInfoFunzioni
            // 
            this.btnInfoFunzioni.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnInfoFunzioni.Location = new System.Drawing.Point(202, 3);
            this.btnInfoFunzioni.Name = "btnInfoFunzioni";
            this.btnInfoFunzioni.Size = new System.Drawing.Size(18, 23);
            this.btnInfoFunzioni.TabIndex = 23;
            this.btnInfoFunzioni.Text = "?";
            this.btnInfoFunzioni.UseVisualStyleBackColor = false;
            this.btnInfoFunzioni.Click += new System.EventHandler(this.btnInfoFunzioni_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(2, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 25);
            this.label6.TabIndex = 18;
            this.label6.Text = "FUNZIONI";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNumInGioco
            // 
            this.btnNumInGioco.BackColor = System.Drawing.Color.LightGray;
            this.btnNumInGioco.Location = new System.Drawing.Point(110, 67);
            this.btnNumInGioco.Name = "btnNumInGioco";
            this.btnNumInGioco.Size = new System.Drawing.Size(104, 39);
            this.btnNumInGioco.TabIndex = 12;
            this.btnNumInGioco.Text = "Numeri in gioco";
            this.btnNumInGioco.UseVisualStyleBackColor = false;
            this.btnNumInGioco.Visible = false;
            this.btnNumInGioco.Click += new System.EventHandler(this.btnNumInGioco_Click);
            // 
            // btnFill
            // 
            this.btnFill.Location = new System.Drawing.Point(3, 27);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(104, 39);
            this.btnFill.TabIndex = 0;
            this.btnFill.Text = "1 - Carica le estrazioni";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            // 
            // btnCalcRes
            // 
            this.btnCalcRes.Location = new System.Drawing.Point(110, 27);
            this.btnCalcRes.Name = "btnCalcRes";
            this.btnCalcRes.Size = new System.Drawing.Size(104, 39);
            this.btnCalcRes.TabIndex = 11;
            this.btnCalcRes.Text = "2 - Calcola i risultati";
            this.btnCalcRes.UseVisualStyleBackColor = true;
            this.btnCalcRes.Visible = false;
            this.btnCalcRes.Click += new System.EventHandler(this.btnCalcRes_Click);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(2, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(460, 22);
            this.label7.TabIndex = 19;
            this.label7.Text = "PARAMETRI R1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(54, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "tot righe";
            // 
            // txtTotRighe
            // 
            this.txtTotRighe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotRighe.Location = new System.Drawing.Point(114, 90);
            this.txtTotRighe.Name = "txtTotRighe";
            this.txtTotRighe.ReadOnly = true;
            this.txtTotRighe.Size = new System.Drawing.Size(56, 23);
            this.txtTotRighe.TabIndex = 9;
            this.txtTotRighe.Text = "150";
            this.txtTotRighe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMsecTimer
            // 
            this.txtMsecTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsecTimer.Location = new System.Drawing.Point(114, 116);
            this.txtMsecTimer.Name = "txtMsecTimer";
            this.txtMsecTimer.Size = new System.Drawing.Size(56, 23);
            this.txtMsecTimer.TabIndex = 8;
            this.txtMsecTimer.Text = "200";
            this.txtMsecTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(54, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "msec";
            // 
            // btnPlay
            // 
            this.btnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPlay.ImageIndex = 24;
            this.btnPlay.ImageList = this.imageList1;
            this.btnPlay.Location = new System.Drawing.Point(10, 45);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(160, 39);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Scorri";
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
            // btnEvidenziaCalcola
            // 
            this.btnEvidenziaCalcola.Location = new System.Drawing.Point(10, 6);
            this.btnEvidenziaCalcola.Name = "btnEvidenziaCalcola";
            this.btnEvidenziaCalcola.Size = new System.Drawing.Size(160, 39);
            this.btnEvidenziaCalcola.TabIndex = 1;
            this.btnEvidenziaCalcola.Text = "Evidenzia";
            this.btnEvidenziaCalcola.UseVisualStyleBackColor = true;
            this.btnEvidenziaCalcola.Click += new System.EventHandler(this.btnEvidenziaCalcola_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.tcResGrid);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 112);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1708, 623);
            this.pnlGrid.TabIndex = 1;
            // 
            // tcResGrid
            // 
            this.tcResGrid.Controls.Add(this.tabPage1);
            this.tcResGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcResGrid.Location = new System.Drawing.Point(0, 0);
            this.tcResGrid.Name = "tcResGrid";
            this.tcResGrid.SelectedIndex = 0;
            this.tcResGrid.Size = new System.Drawing.Size(1708, 623);
            this.tcResGrid.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ugTabellone);
            this.tabPage1.Controls.Add(this.pnlDim);
            this.tabPage1.Controls.Add(this.pnlColors);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1700, 597);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabellone Colorato";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ugTabellone
            // 
            this.ugTabellone.DataSource = this.bsTabellone;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugTabellone.DisplayLayout.Appearance = appearance1;
            this.ugTabellone.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugTabellone.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugTabellone.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugTabellone.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugTabellone.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugTabellone.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugTabellone.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugTabellone.DisplayLayout.MaxColScrollRegions = 1;
            this.ugTabellone.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugTabellone.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            this.ugTabellone.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ugTabellone.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.ugTabellone.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugTabellone.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ugTabellone.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.ForeColor = System.Drawing.Color.Gray;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            appearance8.TextVAlignAsString = "Middle";
            this.ugTabellone.DisplayLayout.Override.CellAppearance = appearance8;
            this.ugTabellone.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugTabellone.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ugTabellone.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.ugTabellone.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ugTabellone.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugTabellone.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ugTabellone.DisplayLayout.Override.RowAppearance = appearance11;
            this.ugTabellone.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugTabellone.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ugTabellone.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugTabellone.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugTabellone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugTabellone.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugTabellone.Location = new System.Drawing.Point(3, 3);
            this.ugTabellone.Name = "ugTabellone";
            this.ugTabellone.Size = new System.Drawing.Size(1469, 591);
            this.ugTabellone.TabIndex = 0;
            // 
            // bsTabellone
            // 
            this.bsTabellone.DataSource = typeof(PoseidonData.DataEntities.PSD_ESTR_LOTTO);
            // 
            // pnlDim
            // 
            this.pnlDim.Controls.Add(this.btnDimM);
            this.pnlDim.Controls.Add(this.btnDimP);
            this.pnlDim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDim.Location = new System.Drawing.Point(1472, 3);
            this.pnlDim.Name = "pnlDim";
            this.pnlDim.Size = new System.Drawing.Size(43, 591);
            this.pnlDim.TabIndex = 26;
            // 
            // btnDimM
            // 
            this.btnDimM.Location = new System.Drawing.Point(5, 45);
            this.btnDimM.Name = "btnDimM";
            this.btnDimM.Size = new System.Drawing.Size(34, 39);
            this.btnDimM.TabIndex = 1;
            this.btnDimM.Text = "-";
            this.btnDimM.UseVisualStyleBackColor = true;
            this.btnDimM.Click += new System.EventHandler(this.BtnDimM_Click);
            // 
            // btnDimP
            // 
            this.btnDimP.Location = new System.Drawing.Point(5, 6);
            this.btnDimP.Name = "btnDimP";
            this.btnDimP.Size = new System.Drawing.Size(34, 39);
            this.btnDimP.TabIndex = 0;
            this.btnDimP.Text = "+";
            this.btnDimP.UseVisualStyleBackColor = true;
            this.btnDimP.Click += new System.EventHandler(this.BtnDimP_Click);
            // 
            // pnlColors
            // 
            this.pnlColors.Controls.Add(this.psdColorTab2);
            this.pnlColors.Controls.Add(this.pnlBtnsEvidenzia);
            this.pnlColors.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlColors.Location = new System.Drawing.Point(1515, 3);
            this.pnlColors.Name = "pnlColors";
            this.pnlColors.Size = new System.Drawing.Size(182, 591);
            this.pnlColors.TabIndex = 1;
            // 
            // psdColorTab2
            // 
            this.psdColorTab2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdColorTab2.Location = new System.Drawing.Point(0, 148);
            this.psdColorTab2.Name = "psdColorTab2";
            this.psdColorTab2.Padding = new System.Windows.Forms.Padding(3);
            this.psdColorTab2.Size = new System.Drawing.Size(182, 443);
            this.psdColorTab2.TabIndex = 0;
            this.psdColorTab2.TIPO_EVIDENZA = 1;
            // 
            // pnlBtnsEvidenzia
            // 
            this.pnlBtnsEvidenzia.Controls.Add(this.btnInfoEvidenzia);
            this.pnlBtnsEvidenzia.Controls.Add(this.label4);
            this.pnlBtnsEvidenzia.Controls.Add(this.btnEvidenziaCalcola);
            this.pnlBtnsEvidenzia.Controls.Add(this.txtTotRighe);
            this.pnlBtnsEvidenzia.Controls.Add(this.txtMsecTimer);
            this.pnlBtnsEvidenzia.Controls.Add(this.btnPlay);
            this.pnlBtnsEvidenzia.Controls.Add(this.label3);
            this.pnlBtnsEvidenzia.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBtnsEvidenzia.Location = new System.Drawing.Point(0, 0);
            this.pnlBtnsEvidenzia.Name = "pnlBtnsEvidenzia";
            this.pnlBtnsEvidenzia.Size = new System.Drawing.Size(182, 148);
            this.pnlBtnsEvidenzia.TabIndex = 1;
            // 
            // btnInfoEvidenzia
            // 
            this.btnInfoEvidenzia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfoEvidenzia.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnInfoEvidenzia.Location = new System.Drawing.Point(10, 90);
            this.btnInfoEvidenzia.Name = "btnInfoEvidenzia";
            this.btnInfoEvidenzia.Size = new System.Drawing.Size(18, 24);
            this.btnInfoEvidenzia.TabIndex = 25;
            this.btnInfoEvidenzia.Text = "?";
            this.btnInfoEvidenzia.UseVisualStyleBackColor = false;
            this.btnInfoEvidenzia.Click += new System.EventHandler(this.btnInfoEvidenzia_Click);
            // 
            // timer1
            // 
            this.timer1.Tag = "0";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PSDTabellone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlOptions);
            this.Name = "PSDTabellone";
            this.Size = new System.Drawing.Size(1708, 735);
            this.pnlOptions.ResumeLayout(false);
            this.pnlParamsCalc.ResumeLayout(false);
            this.pnlR1Params.ResumeLayout(false);
            this.pnlParamsCalcSettings.ResumeLayout(false);
            this.pnlBtnsTop.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.tcResGrid.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugTabellone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTabellone)).EndInit();
            this.pnlDim.ResumeLayout(false);
            this.pnlColors.ResumeLayout(false);
            this.pnlBtnsEvidenzia.ResumeLayout(false);
            this.pnlBtnsEvidenzia.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.BindingSource bsTabellone;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugTabellone;
        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnlColors;
        private System.Windows.Forms.Button btnEvidenziaCalcola;
        private System.Windows.Forms.Panel pnlBtnsTop;
        private System.Windows.Forms.Panel pnlR1Params;
        //private UserControls.PSDSceltaR1 psdSceltaR11;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox txtMsecTimer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTotRighe;
        private System.Windows.Forms.TabControl tcResGrid;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnCalcRes;
        private System.Windows.Forms.Panel pnlBtnsEvidenzia;
        private System.Windows.Forms.Panel pnlParamsCalc;
        private System.Windows.Forms.Button btnNumInGioco;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnInfoFunzioni;
        private UserControls.PSDSceltaR1 psdSceltaR12;
        private System.Windows.Forms.Label label9;
        private UserControls.PSDColorTab psdColorTab2;
        private System.Windows.Forms.Button btnInfoEvidenzia;
        private System.Windows.Forms.Button btnEvidenzia;
        private System.Windows.Forms.Panel pnlParamsCalcSettings;
        private UserControls.PSDViewParamsNumInGioco psdViewParamsNumInGioco1;
        private System.Windows.Forms.Panel pnlDim;
        private System.Windows.Forms.Button btnDimM;
        private System.Windows.Forms.Button btnDimP;
    }
}
