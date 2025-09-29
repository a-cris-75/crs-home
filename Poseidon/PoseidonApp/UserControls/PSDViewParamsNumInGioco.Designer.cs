namespace PoseidonApp.UserControls
{
    partial class PSDViewParamsNumInGioco
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
            this.lblNumInGiocoTitle = new System.Windows.Forms.Label();
            this.lblNumInGiocoDesc = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblParamsTitle = new System.Windows.Forms.Label();
            this.gbPlay = new System.Windows.Forms.GroupBox();
            this.btnPlayNumCasuali = new System.Windows.Forms.Button();
            this.btnPlayR1 = new System.Windows.Forms.Button();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInfoParametriCalcVis = new System.Windows.Forms.Button();
            this.gbPlay.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNumInGiocoTitle
            // 
            this.lblNumInGiocoTitle.AutoSize = true;
            this.lblNumInGiocoTitle.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumInGiocoTitle.Location = new System.Drawing.Point(18, 7);
            this.lblNumInGiocoTitle.Name = "lblNumInGiocoTitle";
            this.lblNumInGiocoTitle.Size = new System.Drawing.Size(117, 14);
            this.lblNumInGiocoTitle.TabIndex = 0;
            this.lblNumInGiocoTitle.Text = "Numeri della regola 1:";
            // 
            // lblNumInGiocoDesc
            // 
            this.lblNumInGiocoDesc.AutoSize = true;
            this.lblNumInGiocoDesc.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumInGiocoDesc.Location = new System.Drawing.Point(184, 7);
            this.lblNumInGiocoDesc.Name = "lblNumInGiocoDesc";
            this.lblNumInGiocoDesc.Size = new System.Drawing.Size(159, 28);
            this.lblNumInGiocoDesc.TabIndex = 1;
            this.lblNumInGiocoDesc.Text = "numeri in gioco per estrazione:\r\nregola: \r\n";
            // 
            // lblParamsTitle
            // 
            this.lblParamsTitle.AutoSize = true;
            this.lblParamsTitle.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamsTitle.Location = new System.Drawing.Point(82, 24);
            this.lblParamsTitle.Name = "lblParamsTitle";
            this.lblParamsTitle.Size = new System.Drawing.Size(66, 14);
            this.lblParamsTitle.TabIndex = 2;
            this.lblParamsTitle.Text = "Parametri: ";
            // 
            // gbPlay
            // 
            this.gbPlay.Controls.Add(this.btnPlayNumCasuali);
            this.gbPlay.Controls.Add(this.btnPlayR1);
            this.gbPlay.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbPlay.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPlay.Location = new System.Drawing.Point(0, 25);
            this.gbPlay.Name = "gbPlay";
            this.gbPlay.Size = new System.Drawing.Size(171, 82);
            this.gbPlay.TabIndex = 9;
            this.gbPlay.TabStop = false;
            this.gbPlay.Text = "Gioca con ";
            // 
            // btnPlayNumCasuali
            // 
            this.btnPlayNumCasuali.Location = new System.Drawing.Point(8, 19);
            this.btnPlayNumCasuali.Name = "btnPlayNumCasuali";
            this.btnPlayNumCasuali.Size = new System.Drawing.Size(78, 45);
            this.btnPlayNumCasuali.TabIndex = 6;
            this.btnPlayNumCasuali.Text = "Numeri casuali";
            this.btnPlayNumCasuali.UseVisualStyleBackColor = true;
            this.btnPlayNumCasuali.Click += new System.EventHandler(this.BtnPlayNumCasuali_Click);
            // 
            // btnPlayR1
            // 
            this.btnPlayR1.Location = new System.Drawing.Point(88, 19);
            this.btnPlayR1.Name = "btnPlayR1";
            this.btnPlayR1.Size = new System.Drawing.Size(78, 45);
            this.btnPlayR1.TabIndex = 7;
            this.btnPlayR1.Text = "Numeri Regola 1";
            this.btnPlayR1.UseVisualStyleBackColor = false;
            this.btnPlayR1.Click += new System.EventHandler(this.BtnPlayR1_Click);
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblNumInGiocoDesc);
            this.pnlInfo.Controls.Add(this.lblNumInGiocoTitle);
            this.pnlInfo.Controls.Add(this.lblParamsTitle);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Location = new System.Drawing.Point(171, 25);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(418, 82);
            this.pnlInfo.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(589, 25);
            this.label2.TabIndex = 23;
            this.label2.Text = "PARAMETRI NUMERI in GIOCO";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnInfoParametriCalcVis
            // 
            this.btnInfoParametriCalcVis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfoParametriCalcVis.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnInfoParametriCalcVis.Location = new System.Drawing.Point(570, 1);
            this.btnInfoParametriCalcVis.Name = "btnInfoParametriCalcVis";
            this.btnInfoParametriCalcVis.Size = new System.Drawing.Size(18, 23);
            this.btnInfoParametriCalcVis.TabIndex = 25;
            this.btnInfoParametriCalcVis.Text = "?";
            this.btnInfoParametriCalcVis.UseVisualStyleBackColor = false;
            this.btnInfoParametriCalcVis.Click += new System.EventHandler(this.BtnInfoParametriCalcVis_Click);
            // 
            // PSDViewParamsNumInGioco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInfoParametriCalcVis);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.gbPlay);
            this.Controls.Add(this.label2);
            this.Name = "PSDViewParamsNumInGioco";
            this.Size = new System.Drawing.Size(589, 107);
            this.gbPlay.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNumInGiocoTitle;
        private System.Windows.Forms.Label lblNumInGiocoDesc;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblParamsTitle;
        private System.Windows.Forms.GroupBox gbPlay;
        private System.Windows.Forms.Button btnPlayNumCasuali;
        private System.Windows.Forms.Button btnPlayR1;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInfoParametriCalcVis;
    }
}
