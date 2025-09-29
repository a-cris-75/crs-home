namespace PoseidonApp
{
    partial class PSDParams
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
            this.pnlIntevalTime = new System.Windows.Forms.Panel();
            this.gbPeriodo = new System.Windows.Forms.GroupBox();
            this.btnSetNumEstr = new System.Windows.Forms.Button();
            this.chkAutoUpdR1 = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.btnUpdR1 = new System.Windows.Forms.Button();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.txtNumEstr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetIntervalDate = new System.Windows.Forms.Button();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this.gbPlay = new System.Windows.Forms.GroupBox();
            this.btnPlayNumCasuali = new System.Windows.Forms.Button();
            this.btnPlayR1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlIntevalTime.SuspendLayout();
            this.gbPeriodo.SuspendLayout();
            this.pnlBtns.SuspendLayout();
            this.gbPlay.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIntevalTime
            // 
            this.pnlIntevalTime.Controls.Add(this.gbPeriodo);
            this.pnlIntevalTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIntevalTime.Location = new System.Drawing.Point(0, 0);
            this.pnlIntevalTime.Name = "pnlIntevalTime";
            this.pnlIntevalTime.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlIntevalTime.Size = new System.Drawing.Size(323, 105);
            this.pnlIntevalTime.TabIndex = 0;
            // 
            // gbPeriodo
            // 
            this.gbPeriodo.Controls.Add(this.btnSetNumEstr);
            this.gbPeriodo.Controls.Add(this.chkAutoUpdR1);
            this.gbPeriodo.Controls.Add(this.btnImport);
            this.gbPeriodo.Controls.Add(this.dtpBegin);
            this.gbPeriodo.Controls.Add(this.btnUpdR1);
            this.gbPeriodo.Controls.Add(this.dtpEnd);
            this.gbPeriodo.Controls.Add(this.txtNumEstr);
            this.gbPeriodo.Controls.Add(this.label1);
            this.gbPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPeriodo.Location = new System.Drawing.Point(0, 0);
            this.gbPeriodo.Name = "gbPeriodo";
            this.gbPeriodo.Size = new System.Drawing.Size(320, 105);
            this.gbPeriodo.TabIndex = 4;
            this.gbPeriodo.TabStop = false;
            this.gbPeriodo.Text = "Estrazioni DA - A";
            // 
            // btnSetNumEstr
            // 
            this.btnSetNumEstr.Location = new System.Drawing.Point(162, 70);
            this.btnSetNumEstr.Name = "btnSetNumEstr";
            this.btnSetNumEstr.Size = new System.Drawing.Size(28, 31);
            this.btnSetNumEstr.TabIndex = 6;
            this.btnSetNumEstr.Text = ">";
            this.btnSetNumEstr.UseVisualStyleBackColor = true;
            this.btnSetNumEstr.Click += new System.EventHandler(this.btnSetNumEstr_Click);
            // 
            // chkAutoUpdR1
            // 
            this.chkAutoUpdR1.AutoSize = true;
            this.chkAutoUpdR1.Checked = true;
            this.chkAutoUpdR1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoUpdR1.Location = new System.Drawing.Point(287, 36);
            this.chkAutoUpdR1.Name = "chkAutoUpdR1";
            this.chkAutoUpdR1.Size = new System.Drawing.Size(15, 14);
            this.chkAutoUpdR1.TabIndex = 5;
            this.chkAutoUpdR1.UseVisualStyleBackColor = true;
            this.chkAutoUpdR1.Click += new System.EventHandler(this.chkAutoUpdR1_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(198, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(115, 43);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Importa ultime estrazioni";
            this.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dtpBegin
            // 
            this.dtpBegin.Location = new System.Drawing.Point(14, 17);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(176, 22);
            this.dtpBegin.TabIndex = 0;
            this.dtpBegin.ValueChanged += new System.EventHandler(this.dtpBegin_ValueChanged);
            // 
            // btnUpdR1
            // 
            this.btnUpdR1.Enabled = false;
            this.btnUpdR1.Location = new System.Drawing.Point(198, 59);
            this.btnUpdR1.Name = "btnUpdR1";
            this.btnUpdR1.Size = new System.Drawing.Size(115, 43);
            this.btnUpdR1.TabIndex = 3;
            this.btnUpdR1.Text = "Aggiorna archivio num Regola 1";
            this.btnUpdR1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdR1.UseVisualStyleBackColor = true;
            this.btnUpdR1.Click += new System.EventHandler(this.btnUpdR1_Click);
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(14, 42);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(176, 22);
            this.dtpEnd.TabIndex = 1;
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpBegin_ValueChanged);
            // 
            // txtNumEstr
            // 
            this.txtNumEstr.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumEstr.Location = new System.Drawing.Point(103, 71);
            this.txtNumEstr.Name = "txtNumEstr";
            this.txtNumEstr.Size = new System.Drawing.Size(56, 29);
            this.txtNumEstr.TabIndex = 2;
            this.txtNumEstr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Num estrazioni";
            // 
            // btnSetIntervalDate
            // 
            this.btnSetIntervalDate.Location = new System.Drawing.Point(206, 49);
            this.btnSetIntervalDate.Name = "btnSetIntervalDate";
            this.btnSetIntervalDate.Size = new System.Drawing.Size(83, 43);
            this.btnSetIntervalDate.TabIndex = 1;
            this.btnSetIntervalDate.Text = "Imposta periodo analisi";
            this.btnSetIntervalDate.UseVisualStyleBackColor = true;
            this.btnSetIntervalDate.Visible = false;
            this.btnSetIntervalDate.Click += new System.EventHandler(this.btnSetIntervalDate_Click);
            // 
            // pnlBtns
            // 
            this.pnlBtns.Controls.Add(this.gbPlay);
            this.pnlBtns.Controls.Add(this.btnSetIntervalDate);
            this.pnlBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBtns.Location = new System.Drawing.Point(323, 0);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(95, 105);
            this.pnlBtns.TabIndex = 3;
            // 
            // gbPlay
            // 
            this.gbPlay.Controls.Add(this.btnPlayNumCasuali);
            this.gbPlay.Controls.Add(this.btnPlayR1);
            this.gbPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPlay.Location = new System.Drawing.Point(0, 0);
            this.gbPlay.Name = "gbPlay";
            this.gbPlay.Size = new System.Drawing.Size(95, 105);
            this.gbPlay.TabIndex = 8;
            this.gbPlay.TabStop = false;
            this.gbPlay.Text = "Gioca con ";
            // 
            // btnPlayNumCasuali
            // 
            this.btnPlayNumCasuali.Location = new System.Drawing.Point(8, 16);
            this.btnPlayNumCasuali.Name = "btnPlayNumCasuali";
            this.btnPlayNumCasuali.Size = new System.Drawing.Size(78, 43);
            this.btnPlayNumCasuali.TabIndex = 6;
            this.btnPlayNumCasuali.Text = "Numeri casuali";
            this.btnPlayNumCasuali.UseVisualStyleBackColor = true;
            this.btnPlayNumCasuali.Click += new System.EventHandler(this.btnScegliNumCasuali_Click);
            // 
            // btnPlayR1
            // 
            this.btnPlayR1.BackColor = System.Drawing.Color.Lime;
            this.btnPlayR1.Location = new System.Drawing.Point(8, 59);
            this.btnPlayR1.Name = "btnPlayR1";
            this.btnPlayR1.Size = new System.Drawing.Size(78, 43);
            this.btnPlayR1.TabIndex = 7;
            this.btnPlayR1.Text = "Numeri Regola 1";
            this.btnPlayR1.UseVisualStyleBackColor = false;
            this.btnPlayR1.Click += new System.EventHandler(this.btnPlayR1_Click);
            // 
            // PSDParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlBtns);
            this.Controls.Add(this.pnlIntevalTime);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PSDParams";
            this.Size = new System.Drawing.Size(418, 105);
            this.pnlIntevalTime.ResumeLayout(false);
            this.gbPeriodo.ResumeLayout(false);
            this.gbPeriodo.PerformLayout();
            this.pnlBtns.ResumeLayout(false);
            this.gbPlay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIntevalTime;
        private System.Windows.Forms.TextBox txtNumEstr;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.Button btnSetIntervalDate;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Button btnUpdR1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbPeriodo;
        private System.Windows.Forms.CheckBox chkAutoUpdR1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnPlayNumCasuali;
        private System.Windows.Forms.Button btnPlayR1;
        private System.Windows.Forms.GroupBox gbPlay;
        private System.Windows.Forms.Button btnSetNumEstr;
    }
}
