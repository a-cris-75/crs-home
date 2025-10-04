namespace Crs.Home.ApocalypsApp.UserControls
{
    partial class HeaderTabellone
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DateTimePicker dateTimeInizio;
        private System.Windows.Forms.DateTimePicker dateTimeFine;
        private System.Windows.Forms.Button btnCaricaDati;
        private System.Windows.Forms.Button btnImporta;
        private System.Windows.Forms.RadioButton radioFormatoSingolaRiga;
        private System.Windows.Forms.RadioButton radioFormatoMultiplaRiga;
        private System.Windows.Forms.GroupBox groupBoxFormato;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

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
            dateTimeInizio = new DateTimePicker();
            dateTimeFine = new DateTimePicker();
            btnCaricaDati = new Button();
            btnImporta = new Button();
            radioFormatoSingolaRiga = new RadioButton();
            radioFormatoMultiplaRiga = new RadioButton();
            groupBoxFormato = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            toolTip1 = new ToolTip(components);
            txtFormatDate = new TextBox();
            txtSeqFields = new TextBox();
            label3 = new Label();
            label4 = new Label();
            chkSaveToDb = new CheckBox();
            groupBoxFormato.SuspendLayout();
            SuspendLayout();
            // 
            // dateTimeInizio
            // 
            dateTimeInizio.Format = DateTimePickerFormat.Short;
            dateTimeInizio.Location = new Point(36, 22);
            dateTimeInizio.Name = "dateTimeInizio";
            dateTimeInizio.Size = new Size(120, 23);
            dateTimeInizio.TabIndex = 1;
            dateTimeInizio.Value = new DateTime(2025, 8, 30, 11, 2, 23, 764);
            // 
            // dateTimeFine
            // 
            dateTimeFine.Format = DateTimePickerFormat.Short;
            dateTimeFine.Location = new Point(186, 22);
            dateTimeFine.Name = "dateTimeFine";
            dateTimeFine.Size = new Size(120, 23);
            dateTimeFine.TabIndex = 3;
            dateTimeFine.Value = new DateTime(2025, 9, 30, 11, 2, 23, 766);
            // 
            // btnCaricaDati
            // 
            btnCaricaDati.BackColor = Color.SteelBlue;
            btnCaricaDati.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCaricaDati.ForeColor = Color.White;
            btnCaricaDati.Location = new Point(312, 11);
            btnCaricaDati.Name = "btnCaricaDati";
            btnCaricaDati.Size = new Size(90, 44);
            btnCaricaDati.TabIndex = 4;
            btnCaricaDati.Text = "Carica Dati";
            btnCaricaDati.UseVisualStyleBackColor = false;
            btnCaricaDati.Click += BtnCaricaDati_Click;
            // 
            // btnImporta
            // 
            btnImporta.BackColor = Color.SteelBlue;
            btnImporta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnImporta.ForeColor = Color.White;
            btnImporta.Location = new Point(1077, 11);
            btnImporta.Name = "btnImporta";
            btnImporta.Size = new Size(112, 44);
            btnImporta.TabIndex = 6;
            btnImporta.Text = "Importa da File";
            btnImporta.UseVisualStyleBackColor = false;
            btnImporta.Click += BtnImporta_Click;
            // 
            // radioFormatoSingolaRiga
            // 
            radioFormatoSingolaRiga.Checked = true;
            radioFormatoSingolaRiga.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioFormatoSingolaRiga.Location = new Point(13, 22);
            radioFormatoSingolaRiga.Name = "radioFormatoSingolaRiga";
            radioFormatoSingolaRiga.Size = new Size(170, 20);
            radioFormatoSingolaRiga.TabIndex = 0;
            radioFormatoSingolaRiga.TabStop = true;
            radioFormatoSingolaRiga.Text = "Singola riga per estrazione";
            // 
            // radioFormatoMultiplaRiga
            // 
            radioFormatoMultiplaRiga.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            radioFormatoMultiplaRiga.Location = new Point(183, 22);
            radioFormatoMultiplaRiga.Name = "radioFormatoMultiplaRiga";
            radioFormatoMultiplaRiga.Size = new Size(180, 20);
            radioFormatoMultiplaRiga.TabIndex = 1;
            radioFormatoMultiplaRiga.Text = "Riga per ruota per estrazione";
            // 
            // groupBoxFormato
            // 
            groupBoxFormato.Controls.Add(radioFormatoSingolaRiga);
            groupBoxFormato.Controls.Add(radioFormatoMultiplaRiga);
            groupBoxFormato.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxFormato.Location = new Point(462, 3);
            groupBoxFormato.Name = "groupBoxFormato";
            groupBoxFormato.Size = new Size(369, 55);
            groupBoxFormato.TabIndex = 5;
            groupBoxFormato.TabStop = false;
            groupBoxFormato.Text = "Formato Importazione";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 0;
            label1.Text = "Da:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(166, 25);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 2;
            label2.Text = "A:";
            // 
            // txtFormatDate
            // 
            txtFormatDate.Location = new Point(931, 10);
            txtFormatDate.Name = "txtFormatDate";
            txtFormatDate.Size = new Size(129, 23);
            txtFormatDate.TabIndex = 7;
            txtFormatDate.Text = "dd/mm/yyyy";
            // 
            // txtSeqFields
            // 
            txtSeqFields.Location = new Point(931, 36);
            txtSeqFields.Name = "txtSeqFields";
            txtSeqFields.Size = new Size(129, 23);
            txtSeqFields.TabIndex = 8;
            txtSeqFields.Text = "data,seq,numeri,ruota";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(851, 13);
            label3.Name = "label3";
            label3.Size = new Size(78, 15);
            label3.TabIndex = 9;
            label3.Text = "Formato data";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(864, 39);
            label4.Name = "label4";
            label4.Size = new Size(65, 15);
            label4.TabIndex = 10;
            label4.Text = "Seq. campi";
            // 
            // chkSaveToDb
            // 
            chkSaveToDb.AutoSize = true;
            chkSaveToDb.Location = new Point(1207, 24);
            chkSaveToDb.Name = "chkSaveToDb";
            chkSaveToDb.Size = new Size(86, 19);
            chkSaveToDb.TabIndex = 11;
            chkSaveToDb.Text = "Salva su DB";
            chkSaveToDb.UseVisualStyleBackColor = true;
            // 
            // HeaderTabellone
            // 
            BackColor = Color.LightSteelBlue;
            Controls.Add(chkSaveToDb);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtSeqFields);
            Controls.Add(txtFormatDate);
            Controls.Add(label1);
            Controls.Add(dateTimeInizio);
            Controls.Add(label2);
            Controls.Add(dateTimeFine);
            Controls.Add(btnCaricaDati);
            Controls.Add(groupBoxFormato);
            Controls.Add(btnImporta);
            Name = "HeaderTabellone";
            Padding = new Padding(10);
            Size = new Size(1343, 66);
            groupBoxFormato.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        private ToolTip toolTip1;
        private TextBox txtFormatDate;
        private TextBox txtSeqFields;
        private Label label3;
        private Label label4;
        private CheckBox chkSaveToDb;
    }
}