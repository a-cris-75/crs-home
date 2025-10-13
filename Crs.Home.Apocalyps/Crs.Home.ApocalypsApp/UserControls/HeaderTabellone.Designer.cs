namespace Crs.Home.ApocalypsApp.UserControls
{
    partial class HeaderTabellone
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCaricaDati;
        private System.Windows.Forms.Button btnImporta;

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
            btnCaricaDati = new Button();
            btnImporta = new Button();
            toolTip1 = new ToolTip(components);
            lblLastDate = new Label();
            gbInterval = new GroupBox();
            label1 = new Label();
            dateTimeInizio = new DateTimePicker();
            label2 = new Label();
            dateTimeFine = new DateTimePicker();
            lblNumEstr = new Label();
            gbInterval.SuspendLayout();
            SuspendLayout();
            // 
            // btnCaricaDati
            // 
            btnCaricaDati.BackColor = Color.LightSteelBlue;
            btnCaricaDati.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnCaricaDati.ForeColor = Color.White;
            btnCaricaDati.Location = new Point(508, 11);
            btnCaricaDati.Name = "btnCaricaDati";
            btnCaricaDati.Size = new Size(90, 44);
            btnCaricaDati.TabIndex = 4;
            btnCaricaDati.Text = "Carica Dati";
            btnCaricaDati.UseVisualStyleBackColor = false;
            btnCaricaDati.Click += BtnCaricaDati_Click;
            // 
            // btnImporta
            // 
            btnImporta.BackColor = Color.LightSteelBlue;
            btnImporta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnImporta.ForeColor = Color.White;
            btnImporta.Location = new Point(604, 11);
            btnImporta.Name = "btnImporta";
            btnImporta.Size = new Size(112, 44);
            btnImporta.TabIndex = 6;
            btnImporta.Text = "Importa da File";
            btnImporta.UseVisualStyleBackColor = false;
            btnImporta.Click += BtnImporta_Click;
            // 
            // lblLastDate
            // 
            lblLastDate.AutoSize = true;
            lblLastDate.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblLastDate.Location = new Point(773, 26);
            lblLastDate.Name = "lblLastDate";
            lblLastDate.Size = new Size(145, 20);
            lblLastDate.TabIndex = 7;
            lblLastDate.Text = "Last Date Imported";
            // 
            // gbInterval
            // 
            gbInterval.Controls.Add(label1);
            gbInterval.Controls.Add(dateTimeInizio);
            gbInterval.Controls.Add(label2);
            gbInterval.Controls.Add(dateTimeFine);
            gbInterval.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            gbInterval.Location = new Point(8, 2);
            gbInterval.Name = "gbInterval";
            gbInterval.Size = new Size(328, 53);
            gbInterval.TabIndex = 17;
            gbInterval.TabStop = false;
            gbInterval.Text = "Intervallo";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(17, 25);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 4;
            label1.Text = "Da:";
            // 
            // dateTimeInizio
            // 
            dateTimeInizio.Format = DateTimePickerFormat.Short;
            dateTimeInizio.Location = new Point(48, 22);
            dateTimeInizio.Name = "dateTimeInizio";
            dateTimeInizio.Size = new Size(120, 23);
            dateTimeInizio.TabIndex = 5;
            dateTimeInizio.Value = new DateTime(2025, 8, 30, 11, 2, 23, 764);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(178, 25);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 6;
            label2.Text = "A:";
            // 
            // dateTimeFine
            // 
            dateTimeFine.Format = DateTimePickerFormat.Short;
            dateTimeFine.Location = new Point(198, 22);
            dateTimeFine.Name = "dateTimeFine";
            dateTimeFine.Size = new Size(120, 23);
            dateTimeFine.TabIndex = 7;
            dateTimeFine.Value = new DateTime(2025, 9, 30, 11, 2, 23, 766);
            // 
            // lblNumEstr
            // 
            lblNumEstr.AutoSize = true;
            lblNumEstr.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblNumEstr.Location = new Point(357, 27);
            lblNumEstr.Name = "lblNumEstr";
            lblNumEstr.Size = new Size(75, 20);
            lblNumEstr.TabIndex = 18;
            lblNumEstr.Text = "Num Estr";
            // 
            // HeaderTabellone
            // 
            BackColor = Color.LightSteelBlue;
            Controls.Add(lblNumEstr);
            Controls.Add(gbInterval);
            Controls.Add(lblLastDate);
            Controls.Add(btnCaricaDati);
            Controls.Add(btnImporta);
            Name = "HeaderTabellone";
            Padding = new Padding(10);
            Size = new Size(1343, 66);
            gbInterval.ResumeLayout(false);
            gbInterval.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private ToolTip toolTip1;
        private Label lblLastDate;
        private GroupBox gbInterval;
        private Label label1;
        private DateTimePicker dateTimeInizio;
        private Label label2;
        private DateTimePicker dateTimeFine;
        private Label lblNumEstr;
    }
}