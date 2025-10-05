using Crs.Home.ApocalypsData.DataEntities;
using Crs.Home.ApocalypsData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp.UserControls
{
    public partial class HeaderTabellone : UserControl
    {
        public DataGridView GrigliaDestinazione { get; set; }

        public HeaderTabellone()
        {
            InitializeComponent();
        }

        public void SetUltimaDataImportata(DateTime dt)
        {
            lblLastDate.Text = "Ultima data importata: " + dt.ToShortDateString();
        }

        public event EventHandler OnImportazioneCompletata;

        private void BtnCaricaDati_Click(object sender, EventArgs e)
        {
            
            if (GrigliaDestinazione != null)
            {
                // Simula il caricamento dati
                CaricaDatiGriglia();
                MessageBox.Show("Dati caricati nella griglia!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Griglia di destinazione non impostata!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void BtnImporta_Click(object sender, EventArgs e)
        {
            //bool formatoSingolaRiga = radioFormatoSingolaRiga.Checked;
            //ApriDialogImportazione(formatoSingolaRiga, txtSeqFields.Text, txtFormatDate.Text, chkSaveToDb.Checked);
            ApriDialogImportazione(Parameters.TypeFieldsFile == COSTANTS.TYPE_FILE_ESTRAZIONI_1_TUTTE_LE_RUOTE, Parameters.SeqFields, Parameters.FormatDateFile, Parameters.SaveToDb);
        }

        private void ApriDialogImportazione(bool formatoSingolaRiga, string seqfields, string patterndate, bool savetodb)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "File di testo (*.txt)|*.txt|Tutti i file (*.*)|*.*";
                openFileDialog.Title = "Seleziona file estrazioni";
                openFileDialog.InitialDirectory = Path.GetDirectoryName(Parameters.PathFileStoricoEstrazioni);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ImportaDatiDaFile(filePath, formatoSingolaRiga, seqfields, patterndate, savetodb);
                }
            }
        }

        private void ImportaDatiDaFile(string filePath, bool formatoSingolaRiga, string seqfields, string patterndate, bool savetodb)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //// Simula l'importazione dei dati
                //var estrazioniImportate = formatoSingolaRiga ?
                //    ImportaFormatoSingolaRiga(filePath) :
                //    ImportaFormatoMultiplaRiga(filePath);
                int filetype = formatoSingolaRiga ? 2 : 1;
                List<Estrazione> lste = DbDataAccess.ReadFileEstrSeq(filePath, filetype, seqfields, patterndate, out string resread);

                string ssavedb = "";
                if (savetodb)
                    ssavedb = "\nContinua con inserimento dati su DB..";

                // Qui puoi salvare nel database o processare i dati
                MessageBox.Show($"Importate {lste.Count} estrazioni dal file!",
                    "Importazione Completata" + ssavedb, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (savetodb)
                {
                    DateTime lastdate = DbDataAccess.GetLastDataEstrImported();
                    DateTime lastdatel = DbDataAccess.GetLastDataLottoImported();
                    if (lste.Where(X => X.Data > lastdate || X.Data > lastdatel).Any())
                    {
                        DateTime mind = lastdatel;
                        if (lastdate < lastdatel) mind = lastdate;
                        DialogResult r = MessageBox.Show($"Attenzione! L'ultima data presente a DB è {mind.ToShortDateString()}. Non verranno importate estrazioni con questa data o precedenti.",
                            "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bool isok = true; bool isokall = true;
                        if (r == DialogResult.OK)
                        {
                            DbDataUpdate.ImportDB(lste);
                        }

                        if (isokall)
                            MessageBox.Show($"Importate {lste.Where(X => X.Data > lastdate).Count()} estrazioni a DB!",
                                "Importazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show($"Errore nell'importazione a DB!",
                                "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                OnImportazioneCompletata?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'importazione: {ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private System.Collections.Generic.List<Estrazione> ImportaFormatoSingolaRiga(string filePath)
        {
            // Simula l'importazione formato singola riga per estrazione
            var estrazioni = new System.Collections.Generic.List<Estrazione>();

            // Esempio: leggere il file e parsare i dati
            // string[] lines = System.IO.File.ReadAllLines(filePath);
            // foreach (string line in lines) { ... }

            // Dati di esempio
            //Random rnd = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    var estrazione = new Estrazione
            //    {
            //        DataEstrazione = DateTime.Now.AddDays(-i),
            //        NumeroEstrazione = 1000 + i,
            //        Ruota = "Tutte",
            //        NumeriEstratti = new System.Collections.Generic.List<int>
            //        {
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91),
            //            rnd.Next(1, 91)
            //        }
            //    };
            //    estrazioni.Add(estrazione);
            //}

            return estrazioni;
        }

        private System.Collections.Generic.List<Estrazione> ImportaFormatoMultiplaRiga(string filePath)
        {
            // Simula l'importazione formato multipla riga (una per ruota per estrazione)
            var estrazioni = new System.Collections.Generic.List<Estrazione>();
            string[] ruote = { "BA", "CA", "FI", "GE", "MI", "NA", "PA", "RO", "TO", "VE", "NZ" };



            Random rnd = new Random();
            for (int i = 0; i < 5; i++) // 5 estrazioni
            {
                foreach (string ruota in ruote)
                {
                    //var estrazione = new Estrazione
                    //{
                    //    Data = DateTime.Now.AddDays(-i),
                    //    //Numeri = 1000 + i,
                    //    Ruota = ruota,
                    //    SeqAnno = i + 1,
                    //    Numeri = new List<int>
                    //    {
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91),
                    //        rnd.Next(1, 91)
                    //    }

                    //};
                    //estrazioni.Add(estrazione);
                }
            }

            return estrazioni;
        }

        private void CaricaDatiGriglia()
        {
            if (GrigliaDestinazione == null) return;

            this.Cursor = Cursors.WaitCursor;
            // Pulisci la griglia esistente
            GrigliaDestinazione.Rows.Clear();

            DateTime dataInizio = dateTimeInizio.Value;
            DateTime dataFine = dateTimeFine.Value;

            ParametriCondivisi.Estrazioni = new System.Collections.Generic.List<Estrazione>();
            ParametriCondivisi.DataInizioAnalisi = dataInizio;
            ParametriCondivisi.DataFineAnalisi = dataFine;

            List<Estrazione> lstBA = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "BA");
            List<Estrazione> lstCA = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "CA");
            List<Estrazione> lstFI = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "FI");
            List<Estrazione> lstGE = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "GE");
            List<Estrazione> lstMI = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "MI");
            List<Estrazione> lstNA = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "NA");
            List<Estrazione> lstPA = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "PA");
            List<Estrazione> lstRM = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "RM");
            List<Estrazione> lstTO = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "TO");
            List<Estrazione> lstVE = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "VE");
            List<Estrazione> lstNZ = DbDataAccess.GetEstrazioniSuRuota(dataInizio, dataFine, "NZ");

            List<Estrazione> lst = new List<Estrazione>();
            lst.AddRange(lstBA);
            lst.AddRange(lstCA);
            lst.AddRange(lstFI);
            lst.AddRange(lstGE);
            lst.AddRange(lstMI);
            lst.AddRange(lstNA);
            lst.AddRange(lstPA);
            lst.AddRange(lstRM);
            lst.AddRange(lstTO);
            lst.AddRange(lstVE);
            lst.AddRange(lstNZ);

            List<string> err = new List<string>();

            ParametriCondivisi.Estrazioni = lst;
            int idx = 0;
            foreach (Estrazione e in lstBA)
            {
                object[] riga = new object[57];
                riga[0] = e.Data.ToString("dd/MM/yyyy");
                riga[1] = idx.ToString();
                riga = SetRowRuota(riga, e.Data, e, err);
                riga = SetRowRuota(riga, e.Data, lstCA[idx], err);
                riga = SetRowRuota(riga, e.Data, lstFI[idx], err);
                riga = SetRowRuota(riga, e.Data, lstGE[idx], err);
                riga = SetRowRuota(riga, e.Data, lstMI[idx], err);
                riga = SetRowRuota(riga, e.Data, lstNA[idx], err);
                riga = SetRowRuota(riga, e.Data, lstPA[idx], err);
                riga = SetRowRuota(riga, e.Data, lstRM[idx], err);
                riga = SetRowRuota(riga, e.Data, lstTO[idx], err);
                riga = SetRowRuota(riga, e.Data, lstVE[idx], err);
                riga = SetRowRuota(riga, e.Data, lstNZ[idx], err);
                GrigliaDestinazione.Rows.Add(riga);
                idx++;
            }

            if (err.Any())
            {
                btnCaricaDati.BackColor = Color.Red;
                toolTip1.SetToolTip(btnCaricaDati, "Errori nel caricamento:\n" + string.Join("\n", err));
            }

            this.Cursor = Cursors.Default;

            //Random rnd = new Random();
            //for (int i = 0; i < 15; i++)
            //{
            //    object[] riga = new object[57];
            //    riga[0] = dataInizio.AddDays(i).ToString("dd/MM/yyyy");
            //    riga[1] = 1000 + i;

            //    for (int j = 2; j < 57; j++)
            //    {
            //        riga[j] = rnd.Next(1, 91);
            //    }

            //    GrigliaDestinazione.Rows.Add(riga);
            //}
        }

        private object[] SetRowRuota(object[] riga, DateTime dt, Estrazione e, List<string> err)
        {
            int startidx = 2;
            if (e.Data == dt)
            {
                switch (e.Ruota)
                {
                    case "BA":
                        startidx = 2;
                        break;
                    case "CA":
                        startidx = 7;
                        break;
                    case "FI":
                        startidx = 12;
                        break;
                    case "GE":
                        startidx = 17;
                        break;
                    case "MI":
                        startidx = 22;
                        break;
                    case "NA":
                        startidx = 27;
                        break;
                    case "PA":
                        startidx = 32;
                        break;
                    case "RM":
                        startidx = 37;
                        break;
                    case "TO":
                        startidx = 42;
                        break;
                    case "VE":
                        startidx = 47;
                        break;
                    case "NZ":
                        startidx = 52;
                        break;
                }
            }
            else err.Add(dt.ToShortDateString() + " " + e.Ruota);

            riga[startidx] = e.Numeri[0];
            riga[startidx + 1] = e.Numeri[1];
            riga[startidx + 2] = e.Numeri[2];
            riga[startidx + 3] = e.Numeri[3];
            riga[startidx + 4] = e.Numeri[4];
            return riga;
        }
    }

    //public class Estrazione
    //{
    //    public DateTime DataEstrazione { get; set; }
    //    public int NumeroEstrazione { get; set; }
    //    public string Ruota { get; set; }
    //    public System.Collections.Generic.List<int> NumeriEstratti { get; set; }
    //}
}