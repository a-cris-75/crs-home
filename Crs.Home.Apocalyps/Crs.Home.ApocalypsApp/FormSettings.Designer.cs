namespace Crs.Home.ApocalypsApp
{
    partial class FormSettings
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            pnlMainSettings = new Panel();
            btnOpenPath = new Button();
            txtPathStorico = new TextBox();
            label1 = new Label();
            chkSaveToDb = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            txtSeqFields = new TextBox();
            txtFormatDate = new TextBox();
            groupBoxFormato = new GroupBox();
            rbFormatRuoteEstr = new RadioButton();
            rbFormatRuotaEstr = new RadioButton();
            tabPage2 = new TabPage();
            btnOpedDialogPathDB = new Button();
            txtLocalPathDB = new TextBox();
            label2 = new Label();
            pnlFooter = new Panel();
            BtnCancel = new Button();
            brnSave = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            pnlMainSettings.SuspendLayout();
            groupBoxFormato.SuspendLayout();
            tabPage2.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(517, 408);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(pnlMainSettings);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(509, 380);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Estrazioni";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlMainSettings
            // 
            pnlMainSettings.Controls.Add(btnOpenPath);
            pnlMainSettings.Controls.Add(txtPathStorico);
            pnlMainSettings.Controls.Add(label1);
            pnlMainSettings.Controls.Add(chkSaveToDb);
            pnlMainSettings.Controls.Add(label4);
            pnlMainSettings.Controls.Add(label3);
            pnlMainSettings.Controls.Add(txtSeqFields);
            pnlMainSettings.Controls.Add(txtFormatDate);
            pnlMainSettings.Controls.Add(groupBoxFormato);
            pnlMainSettings.Dock = DockStyle.Fill;
            pnlMainSettings.Location = new Point(3, 3);
            pnlMainSettings.Name = "pnlMainSettings";
            pnlMainSettings.Size = new Size(503, 374);
            pnlMainSettings.TabIndex = 0;
            // 
            // btnOpenPath
            // 
            btnOpenPath.Location = new Point(137, 192);
            btnOpenPath.Name = "btnOpenPath";
            btnOpenPath.Size = new Size(48, 23);
            btnOpenPath.TabIndex = 20;
            btnOpenPath.Text = "...";
            btnOpenPath.UseVisualStyleBackColor = true;
            btnOpenPath.Click += btnOpenPath_Click;
            // 
            // txtPathStorico
            // 
            txtPathStorico.Location = new Point(15, 216);
            txtPathStorico.Name = "txtPathStorico";
            txtPathStorico.Size = new Size(483, 23);
            txtPathStorico.TabIndex = 19;
            txtPathStorico.Text = "D:\\CRS\\DEV\\REPOSITORY_GIT\\crs-home\\Crs.Home.Apocalyps\\Crs.Home.ApocalypsDB";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 196);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 18;
            label1.Text = "Path file stlorico";
            // 
            // chkSaveToDb
            // 
            chkSaveToDb.AutoSize = true;
            chkSaveToDb.Checked = true;
            chkSaveToDb.CheckState = CheckState.Checked;
            chkSaveToDb.Location = new Point(11, 148);
            chkSaveToDb.Name = "chkSaveToDb";
            chkSaveToDb.Size = new Size(86, 19);
            chkSaveToDb.TabIndex = 17;
            chkSaveToDb.Text = "Salva su DB";
            chkSaveToDb.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 104);
            label4.Name = "label4";
            label4.Size = new Size(65, 15);
            label4.TabIndex = 16;
            label4.Text = "Seq. campi";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 78);
            label3.Name = "label3";
            label3.Size = new Size(78, 15);
            label3.TabIndex = 15;
            label3.Text = "Formato data";
            // 
            // txtSeqFields
            // 
            txtSeqFields.Location = new Point(138, 101);
            txtSeqFields.Name = "txtSeqFields";
            txtSeqFields.Size = new Size(129, 23);
            txtSeqFields.TabIndex = 14;
            txtSeqFields.Text = "data,ruota,numeri";
            // 
            // txtFormatDate
            // 
            txtFormatDate.Location = new Point(138, 75);
            txtFormatDate.Name = "txtFormatDate";
            txtFormatDate.Size = new Size(129, 23);
            txtFormatDate.TabIndex = 13;
            txtFormatDate.Text = "yyyy/MM/dd";
            // 
            // groupBoxFormato
            // 
            groupBoxFormato.Controls.Add(rbFormatRuoteEstr);
            groupBoxFormato.Controls.Add(rbFormatRuotaEstr);
            groupBoxFormato.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxFormato.Location = new Point(2, 14);
            groupBoxFormato.Name = "groupBoxFormato";
            groupBoxFormato.Size = new Size(369, 55);
            groupBoxFormato.TabIndex = 12;
            groupBoxFormato.TabStop = false;
            groupBoxFormato.Text = "Formato Importazione";
            // 
            // rbFormatRuoteEstr
            // 
            rbFormatRuoteEstr.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            rbFormatRuoteEstr.Location = new Point(13, 22);
            rbFormatRuoteEstr.Name = "rbFormatRuoteEstr";
            rbFormatRuoteEstr.Size = new Size(170, 20);
            rbFormatRuoteEstr.TabIndex = 0;
            rbFormatRuoteEstr.Text = "Singola riga per estrazione";
            rbFormatRuoteEstr.CheckedChanged += radioFormatoSingolaRiga_CheckedChanged;
            // 
            // rbFormatRuotaEstr
            // 
            rbFormatRuotaEstr.Checked = true;
            rbFormatRuotaEstr.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            rbFormatRuotaEstr.Location = new Point(183, 22);
            rbFormatRuotaEstr.Name = "rbFormatRuotaEstr";
            rbFormatRuotaEstr.Size = new Size(180, 20);
            rbFormatRuotaEstr.TabIndex = 1;
            rbFormatRuotaEstr.TabStop = true;
            rbFormatRuotaEstr.Text = "Riga per ruota per estrazione";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnOpedDialogPathDB);
            tabPage2.Controls.Add(txtLocalPathDB);
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(509, 380);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "DataBase";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnOpedDialogPathDB
            // 
            btnOpedDialogPathDB.Location = new Point(145, 22);
            btnOpedDialogPathDB.Name = "btnOpedDialogPathDB";
            btnOpedDialogPathDB.Size = new Size(48, 23);
            btnOpedDialogPathDB.TabIndex = 21;
            btnOpedDialogPathDB.Text = "...";
            btnOpedDialogPathDB.UseVisualStyleBackColor = true;
            btnOpedDialogPathDB.Click += btnOpedDialogPathDB_Click;
            // 
            // txtLocalPathDB
            // 
            txtLocalPathDB.Location = new Point(19, 46);
            txtLocalPathDB.Name = "txtLocalPathDB";
            txtLocalPathDB.Size = new Size(472, 23);
            txtLocalPathDB.TabIndex = 20;
            txtLocalPathDB.Text = "D:\\CRS\\DEV\\REPOSITORY_GIT\\crs-home\\Crs.Home.Apocalyps\\Crs.Home.ApocalypsDB\\DB_LOTTO_APOCALYPS.mdf";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 26);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 19;
            label2.Text = "Local path DB:";
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(BtnCancel);
            pnlFooter.Controls.Add(brnSave);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 408);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(517, 42);
            pnlFooter.TabIndex = 21;
            // 
            // BtnCancel
            // 
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            BtnCancel.Location = new Point(252, 4);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(87, 35);
            BtnCancel.TabIndex = 1;
            BtnCancel.Text = "Cancella";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // brnSave
            // 
            brnSave.DialogResult = DialogResult.OK;
            brnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            brnSave.Location = new Point(159, 4);
            brnSave.Name = "brnSave";
            brnSave.Size = new Size(87, 35);
            brnSave.TabIndex = 0;
            brnSave.Text = "Salva";
            brnSave.UseVisualStyleBackColor = true;
            brnSave.Click += brnSave_Click;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(517, 450);
            Controls.Add(tabControl1);
            Controls.Add(pnlFooter);
            Name = "FormSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Impostazioni";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            pnlMainSettings.ResumeLayout(false);
            pnlMainSettings.PerformLayout();
            groupBoxFormato.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Panel pnlMainSettings;
        private TabPage tabPage2;
        private TextBox txtPathStorico;
        private Label label1;
        private CheckBox chkSaveToDb;
        private Label label4;
        private Label label3;
        private TextBox txtSeqFields;
        private TextBox txtFormatDate;
        private GroupBox groupBoxFormato;
        private RadioButton rbFormatRuoteEstr;
        private RadioButton rbFormatRuotaEstr;
        private Button btnOpenPath;
        private Panel pnlFooter;
        private Button BtnCancel;
        private Button brnSave;
        private Label label2;
        private TextBox txtLocalPathDB;
        private Button btnOpedDialogPathDB;
    }
}