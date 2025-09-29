namespace PoseidonApp.UserControls
{
    partial class PSDSavedCalcs
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            this.plnLstCalcs = new System.Windows.Forms.Panel();
            this.ugCalcs = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.bsCalcs = new System.Windows.Forms.BindingSource(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlTopLayout = new System.Windows.Forms.Panel();
            this.btnFillCalcFromDB = new System.Windows.Forms.Button();
            this.btnDelCalc = new System.Windows.Forms.Button();
            this.btnAddCalcToDB = new System.Windows.Forms.Button();
            this.btnInfoFunzioni = new System.Windows.Forms.Button();
            this.btnInfoFunctions = new System.Windows.Forms.Button();
            this.btnCalcSegnali = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnRicarica = new System.Windows.Forms.Button();
            this.pnlSegnals = new System.Windows.Forms.Panel();
            this.ugSegn = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.bsSegn = new System.Windows.Forms.BindingSource(this.components);
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.chkFilterBySegn = new System.Windows.Forms.CheckBox();
            this.plnLstCalcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCalcs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCalcs)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlTopLayout.SuspendLayout();
            this.pnlSegnals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSegn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSegn)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plnLstCalcs
            // 
            this.plnLstCalcs.Controls.Add(this.ugCalcs);
            this.plnLstCalcs.Controls.Add(this.pnlTop);
            this.plnLstCalcs.Dock = System.Windows.Forms.DockStyle.Left;
            this.plnLstCalcs.Location = new System.Drawing.Point(0, 0);
            this.plnLstCalcs.Name = "plnLstCalcs";
            this.plnLstCalcs.Size = new System.Drawing.Size(547, 413);
            this.plnLstCalcs.TabIndex = 0;
            // 
            // ugCalcs
            // 
            this.ugCalcs.DataSource = this.bsCalcs;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugCalcs.DisplayLayout.Appearance = appearance1;
            this.ugCalcs.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.ugCalcs.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugCalcs.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugCalcs.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugCalcs.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ugCalcs.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugCalcs.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ugCalcs.DisplayLayout.MaxColScrollRegions = 1;
            this.ugCalcs.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugCalcs.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            this.ugCalcs.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ugCalcs.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.ugCalcs.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugCalcs.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ugCalcs.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            appearance8.TextVAlignAsString = "Middle";
            this.ugCalcs.DisplayLayout.Override.CellAppearance = appearance8;
            this.ugCalcs.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugCalcs.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ugCalcs.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.ugCalcs.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ugCalcs.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugCalcs.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ugCalcs.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.ugCalcs.DisplayLayout.Override.RowAppearance = appearance12;
            this.ugCalcs.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugCalcs.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.ugCalcs.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ugCalcs.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugCalcs.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugCalcs.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugCalcs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugCalcs.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugCalcs.Location = new System.Drawing.Point(0, 115);
            this.ugCalcs.Name = "ugCalcs";
            this.ugCalcs.Size = new System.Drawing.Size(547, 298);
            this.ugCalcs.TabIndex = 3;
            this.ugCalcs.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.UgResIntervalli_InitializeLayout);
            // 
            // bsCalcs
            // 
            this.bsCalcs.DataSource = typeof(PoseidonData.DataEntities.PSD_CALCS);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlTopLayout);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(547, 115);
            this.pnlTop.TabIndex = 4;
            // 
            // pnlTopLayout
            // 
            this.pnlTopLayout.AutoSize = true;
            this.pnlTopLayout.Controls.Add(this.btnFillCalcFromDB);
            this.pnlTopLayout.Controls.Add(this.btnDelCalc);
            this.pnlTopLayout.Controls.Add(this.btnAddCalcToDB);
            this.pnlTopLayout.Controls.Add(this.btnInfoFunzioni);
            this.pnlTopLayout.Controls.Add(this.btnInfoFunctions);
            this.pnlTopLayout.Controls.Add(this.btnCalcSegnali);
            this.pnlTopLayout.Controls.Add(this.label8);
            this.pnlTopLayout.Controls.Add(this.btnRicarica);
            this.pnlTopLayout.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTopLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLayout.Name = "pnlTopLayout";
            this.pnlTopLayout.Padding = new System.Windows.Forms.Padding(3);
            this.pnlTopLayout.Size = new System.Drawing.Size(364, 115);
            this.pnlTopLayout.TabIndex = 39;
            // 
            // btnFillCalcFromDB
            // 
            this.btnFillCalcFromDB.BackColor = System.Drawing.Color.Silver;
            this.btnFillCalcFromDB.Location = new System.Drawing.Point(116, 71);
            this.btnFillCalcFromDB.Name = "btnFillCalcFromDB";
            this.btnFillCalcFromDB.Size = new System.Drawing.Size(104, 39);
            this.btnFillCalcFromDB.TabIndex = 26;
            this.btnFillCalcFromDB.Text = "Recupera i calcoli da DB";
            this.btnFillCalcFromDB.UseVisualStyleBackColor = false;
            this.btnFillCalcFromDB.Visible = false;
            this.btnFillCalcFromDB.Click += new System.EventHandler(this.BtnFillCalcFromDB_Click);
            // 
            // btnDelCalc
            // 
            this.btnDelCalc.BackColor = System.Drawing.Color.Silver;
            this.btnDelCalc.Location = new System.Drawing.Point(226, 31);
            this.btnDelCalc.Name = "btnDelCalc";
            this.btnDelCalc.Size = new System.Drawing.Size(104, 39);
            this.btnDelCalc.TabIndex = 25;
            this.btnDelCalc.Text = "Elimina calcoli selezionati da DB";
            this.btnDelCalc.UseVisualStyleBackColor = false;
            this.btnDelCalc.Visible = false;
            this.btnDelCalc.Click += new System.EventHandler(this.BtnDelCalc_Click);
            // 
            // btnAddCalcToDB
            // 
            this.btnAddCalcToDB.BackColor = System.Drawing.Color.Silver;
            this.btnAddCalcToDB.Location = new System.Drawing.Point(116, 31);
            this.btnAddCalcToDB.Name = "btnAddCalcToDB";
            this.btnAddCalcToDB.Size = new System.Drawing.Size(104, 39);
            this.btnAddCalcToDB.TabIndex = 24;
            this.btnAddCalcToDB.Text = "Salva i calcoli su DB";
            this.btnAddCalcToDB.UseVisualStyleBackColor = false;
            this.btnAddCalcToDB.Visible = false;
            this.btnAddCalcToDB.Click += new System.EventHandler(this.BtnAddCalcToDB_Click);
            // 
            // btnInfoFunzioni
            // 
            this.btnInfoFunzioni.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnInfoFunzioni.Location = new System.Drawing.Point(340, 4);
            this.btnInfoFunzioni.Name = "btnInfoFunzioni";
            this.btnInfoFunzioni.Size = new System.Drawing.Size(18, 23);
            this.btnInfoFunzioni.TabIndex = 22;
            this.btnInfoFunzioni.Text = "?";
            this.btnInfoFunzioni.UseVisualStyleBackColor = false;
            // 
            // btnInfoFunctions
            // 
            this.btnInfoFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfoFunctions.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnInfoFunctions.Location = new System.Drawing.Point(343, 4);
            this.btnInfoFunctions.Name = "btnInfoFunctions";
            this.btnInfoFunctions.Size = new System.Drawing.Size(18, 23);
            this.btnInfoFunctions.TabIndex = 23;
            this.btnInfoFunctions.Text = "?";
            this.btnInfoFunctions.UseVisualStyleBackColor = false;
            // 
            // btnCalcSegnali
            // 
            this.btnCalcSegnali.Location = new System.Drawing.Point(6, 71);
            this.btnCalcSegnali.Name = "btnCalcSegnali";
            this.btnCalcSegnali.Size = new System.Drawing.Size(104, 39);
            this.btnCalcSegnali.TabIndex = 18;
            this.btnCalcSegnali.Text = "2 - Mostra i segnali";
            this.btnCalcSegnali.UseVisualStyleBackColor = true;
            this.btnCalcSegnali.Click += new System.EventHandler(this.BtnCalcSaturazione_Click);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(358, 25);
            this.label8.TabIndex = 17;
            this.label8.Text = "FUNZIONI";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRicarica
            // 
            this.btnRicarica.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRicarica.Location = new System.Drawing.Point(6, 31);
            this.btnRicarica.Name = "btnRicarica";
            this.btnRicarica.Size = new System.Drawing.Size(104, 39);
            this.btnRicarica.TabIndex = 19;
            this.btnRicarica.Text = "1- Ricarica i dati";
            this.btnRicarica.UseVisualStyleBackColor = true;
            this.btnRicarica.Click += new System.EventHandler(this.BtnRicarica_Click);
            // 
            // pnlSegnals
            // 
            this.pnlSegnals.Controls.Add(this.ugSegn);
            this.pnlSegnals.Controls.Add(this.pnlFilter);
            this.pnlSegnals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSegnals.Location = new System.Drawing.Point(547, 0);
            this.pnlSegnals.Name = "pnlSegnals";
            this.pnlSegnals.Size = new System.Drawing.Size(502, 413);
            this.pnlSegnals.TabIndex = 5;
            // 
            // ugSegn
            // 
            this.ugSegn.DataSource = this.bsSegn;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugSegn.DisplayLayout.Appearance = appearance14;
            this.ugSegn.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.ugSegn.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugSegn.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance15.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.BorderColor = System.Drawing.SystemColors.Window;
            this.ugSegn.DisplayLayout.GroupByBox.Appearance = appearance15;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugSegn.DisplayLayout.GroupByBox.BandLabelAppearance = appearance16;
            this.ugSegn.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugSegn.DisplayLayout.GroupByBox.PromptAppearance = appearance17;
            this.ugSegn.DisplayLayout.MaxColScrollRegions = 1;
            this.ugSegn.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ugSegn.DisplayLayout.Override.ActiveCellAppearance = appearance18;
            appearance19.FontData.BoldAsString = "True";
            this.ugSegn.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.ugSegn.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.ugSegn.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugSegn.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance20.BackColor = System.Drawing.SystemColors.Window;
            this.ugSegn.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Silver;
            appearance21.TextHAlignAsString = "Center";
            appearance21.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            appearance21.TextVAlignAsString = "Middle";
            this.ugSegn.DisplayLayout.Override.CellAppearance = appearance21;
            this.ugSegn.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugSegn.DisplayLayout.Override.CellPadding = 0;
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            appearance22.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance22.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.BorderColor = System.Drawing.SystemColors.Window;
            this.ugSegn.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.TextHAlignAsString = "Center";
            appearance23.TextVAlignAsString = "Middle";
            this.ugSegn.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.ugSegn.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugSegn.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance24.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ugSegn.DisplayLayout.Override.RowAlternateAppearance = appearance24;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.Color.Silver;
            this.ugSegn.DisplayLayout.Override.RowAppearance = appearance25;
            this.ugSegn.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugSegn.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.ugSegn.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ugSegn.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugSegn.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugSegn.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugSegn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugSegn.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugSegn.Location = new System.Drawing.Point(0, 28);
            this.ugSegn.Name = "ugSegn";
            this.ugSegn.Size = new System.Drawing.Size(502, 385);
            this.ugSegn.TabIndex = 4;
            // 
            // bsSegn
            // 
            this.bsSegn.DataSource = typeof(PoseidonData.DataEntities.PSD_ESTR_SEGNALI);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.chkFilterBySegn);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(502, 28);
            this.pnlFilter.TabIndex = 5;
            // 
            // chkFilterBySegn
            // 
            this.chkFilterBySegn.AutoSize = true;
            this.chkFilterBySegn.Location = new System.Drawing.Point(10, 6);
            this.chkFilterBySegn.Name = "chkFilterBySegn";
            this.chkFilterBySegn.Size = new System.Drawing.Size(106, 17);
            this.chkFilterBySegn.TabIndex = 0;
            this.chkFilterBySegn.Text = "Filtra per segnale";
            this.chkFilterBySegn.UseVisualStyleBackColor = true;
            this.chkFilterBySegn.Click += new System.EventHandler(this.ChkFilterBySegn_Click);
            // 
            // PSDSavedCalcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSegnals);
            this.Controls.Add(this.plnLstCalcs);
            this.Name = "PSDSavedCalcs";
            this.Size = new System.Drawing.Size(1049, 413);
            this.plnLstCalcs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugCalcs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCalcs)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlTopLayout.ResumeLayout(false);
            this.pnlSegnals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSegn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSegn)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plnLstCalcs;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugCalcs;
        private System.Windows.Forms.BindingSource bsCalcs;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlTopLayout;
        private System.Windows.Forms.Button btnAddCalcToDB;
        private System.Windows.Forms.Button btnInfoFunzioni;
        private System.Windows.Forms.Button btnInfoFunctions;
        private System.Windows.Forms.Button btnCalcSegnali;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRicarica;
        private System.Windows.Forms.Panel pnlSegnals;
        private System.Windows.Forms.BindingSource bsSegn;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugSegn;
        private System.Windows.Forms.Button btnDelCalc;
        private System.Windows.Forms.Button btnFillCalcFromDB;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.CheckBox chkFilterBySegn;
    }
}
