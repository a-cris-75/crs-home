namespace PoseidonApp.Forms
{
    partial class FPSDResCalcoli
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcResGrid = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.psdResGroupBy2 = new PoseidonApp.UserControls.PSDResGroupBy();
            this.tcResGrid.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcResGrid
            // 
            this.tcResGrid.Controls.Add(this.tabPage3);
            this.tcResGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcResGrid.Location = new System.Drawing.Point(0, 0);
            this.tcResGrid.Name = "tcResGrid";
            this.tcResGrid.SelectedIndex = 0;
            this.tcResGrid.Size = new System.Drawing.Size(1144, 427);
            this.tcResGrid.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.psdResGroupBy2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1136, 401);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Calcoli per gruppi di occorenze / estrazioni";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // psdResGroupBy2
            // 
            this.psdResGroupBy2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.psdResGroupBy2.GROUPBY_NUM_ESTR = 35;
            this.psdResGroupBy2.GROUPBY_OCCORRENZE = 150;
            this.psdResGroupBy2.Location = new System.Drawing.Point(3, 3);
            this.psdResGroupBy2.Name = "psdResGroupBy2";
            this.psdResGroupBy2.SHOW_HEADER_PARAMS = false;
            this.psdResGroupBy2.Size = new System.Drawing.Size(1130, 395);
            this.psdResGroupBy2.TabIndex = 1;
            this.psdResGroupBy2.TYPE_CALC = 1;
            // 
            // FPSDResCalcoli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 427);
            this.Controls.Add(this.tcResGrid);
            this.Name = "FPSDResCalcoli";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Risultati calcoli";
            this.TopMost = true;
            this.tcResGrid.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcResGrid;
        private System.Windows.Forms.TabPage tabPage3;
        public UserControls.PSDResGroupBy psdResGroupBy2;
    }
}