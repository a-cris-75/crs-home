using Crs.Home.ApocalypsData;
using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Crs.Home.ApocalypsApp
{
    public partial class PageAnalisi : UserControl
    {
        private List<RisultatoAnalisi> risultati = new List<RisultatoAnalisi>();
        private List<RisultatoEstrazione> risultatiEstr = new List<RisultatoEstrazione>();
        private decimal budgetDisponibile = 1000m;



        public PageAnalisi()
        {
            InitializeComponent();
            //InizializzaColonneGriglia();
            //InizializzaDatiEsempio();
            AggiornaGriglia();
        }

        public void Init()
        {
            if (!risultatiEstr.Any())
            {
                dateTimeInizio.Value = ParametriCondivisi.DataInizioAnalisi;
                dateTimeFine.Value = ParametriCondivisi.DataFineAnalisi;
            }
            else
            {
                dateTimeInizio.Value = risultatiEstr.Min(X => X.Data);
                dateTimeFine.Value = risultatiEstr.Max(X => X.Data);
            }
        }

        private void InizializzaColonneGriglia()
        {
            grigliaRisultati.Columns.Add("Intervallo", "Intervallo Date");
            grigliaRisultati.Columns.Add("Estrazioni", "N° Estrazioni");
            grigliaRisultati.Columns.Add("NumeriGiocati", "Numeri Giocati");
            grigliaRisultati.Columns.Add("NumeriVinti", "Numeri Vinti");
            grigliaRisultati.Columns.Add("AmbiVinti", "Ambi Vinti");
            grigliaRisultati.Columns.Add("TerniVinti", "Terni Vinti");
            grigliaRisultati.Columns.Add("InvestimentoMin", "Invest. Minimo");
            grigliaRisultati.Columns.Add("Guadagno", "Guadagno");
            grigliaRisultati.Columns.Add("InvestimentoProp", "Invest. Proposto");
            grigliaRisultati.Columns.Add("GuadagnoProp", "Guadagno Proposto");
        }

        private void InizializzaDatiEsempio()
        {
            // Simula dati di esempio per dimostrazione
            Random rnd = new Random();
            DateTime dataInizio = new DateTime(2024, 1, 1);

            for (int i = 0; i < 12; i++) // 12 mesi di esempio
            {
                var risultato = new RisultatoAnalisi
                {
                    Intervallo = $"{dataInizio.AddMonths(i):dd/MM/yyyy} - {dataInizio.AddMonths(i + 1).AddDays(-1):dd/MM/yyyy}",
                    DataInizio = dataInizio.AddMonths(i),
                    DataFine = dataInizio.AddMonths(i + 1).AddDays(-1),
                    NumeroEstrazioni = rnd.Next(8, 13),
                    NumeriGiocati = rnd.Next(5, 16),
                    NumeriVinti = rnd.Next(0, 8),
                    AmbiVinti = rnd.Next(0, 5),
                    TerniVinti = rnd.Next(0, 3)
                };

                // Calcola metriche finanziarie
                CalcolaMetricheFinanziarie(risultato, i);
                risultati.Add(risultato);
            }
        }

        private void CalcolaMetricheFinanziarie(RisultatoAnalisi risultato, int indicePeriodo)
        {
            // Costi fissi per giocata
            decimal costoAmbata = 1.0m;
            decimal costoAmbo = 2.0m;
            decimal costoTerno = 5.0m;

            // Vincite medie (semplificate)
            decimal vincitaAmbata = 10.0m;
            decimal vincitaAmbo = 50.0m;
            decimal vincitaTerno = 200.0m;

            // Investimento minimo (gioco tutte le previsioni)
            risultato.InvestimentoMinimo = risultato.NumeriGiocati * costoAmbata;

            // Guadagno teorico
            risultato.Guadagno = (risultato.NumeriVinti * vincitaAmbata) +
                               (risultato.AmbiVinti * vincitaAmbo) +
                               (risultato.TerniVinti * vincitaTerno);

            // Calcola investimento ottimale basato su strategia di recupero
            CalcolaInvestimentoOttimale(risultato, indicePeriodo);
        }

        private void CalcolaInvestimentoOttimale(RisultatoAnalisi risultato, int indicePeriodo)
        {
            // Strategia: se performance scarse, aumento investimento per recuperare
            decimal percentualeSuccesso = risultato.NumeriGiocati > 0 ?
                (decimal)risultato.NumeriVinti / risultato.NumeriGiocati : 0;

            decimal percentualeAttesa = 0.3m; // 30% di successo atteso

            // Fattore di moltiplicazione basato sulla performance
            decimal fattoreInvestimento = 1.0m;

            if (percentualeSuccesso < percentualeAttesa)
            {
                // Calcola quanto siamo sotto la percentuale attesa
                decimal deficit = percentualeAttesa - percentualeSuccesso;
                fattoreInvestimento += deficit * 3; // Moltiplicatore aggressivo
            }

            // Considera l'andamento storico per periodi precedenti
            if (indicePeriodo > 0)
            {
                decimal perditaCumulativa = CalcolaPerditaCumulativa(indicePeriodo);
                if (perditaCumulativa > 0)
                {
                    fattoreInvestimento += perditaCumulativa / 100m;
                }
            }

            // Limita il fattore di investimento per non superare il budget
            fattoreInvestimento = Math.Min(fattoreInvestimento, 5.0m);

            // Calcola investimento proposto
            risultato.InvestimentoProposto = risultato.InvestimentoMinimo * fattoreInvestimento;
            risultato.InvestimentoProposto = Math.Min(risultato.InvestimentoProposto, budgetDisponibile);

            // Ricalcola guadagno con investimento proposto (scalando proporzionalmente)
            decimal fattoreScala = risultato.InvestimentoProposto / risultato.InvestimentoMinimo;
            risultato.GuadagnoProposto = risultato.Guadagno * fattoreScala;
        }

        private decimal CalcolaPerditaCumulativa(int finoAIndice)
        {
            decimal perditaTotale = 0;
            for (int i = 0; i < finoAIndice && i < risultati.Count; i++)
            {
                var risultato = risultati[i];
                decimal perditaPeriodo = risultato.InvestimentoMinimo - risultato.Guadagno;
                if (perditaPeriodo > 0)
                {
                    perditaTotale += perditaPeriodo;
                }
            }
            return perditaTotale;
        }

        private void AggiornaGriglia()
        {
            //grigliaRisultati.Rows.Clear();
            grigliaRisultati.DataSource = risultati;
            grigliaDettagliEsrtrazione.DataSource = risultatiEstr;
            //foreach (var risultato in risultati)
            //{
            //    grigliaRisultati.Rows.Add(
            //        risultato.Intervallo,
            //        risultato.NumeroEstrazioni,
            //        risultato.NumeriGiocati,
            //        risultato.NumeriVinti,
            //        risultato.AmbiVinti,
            //        risultato.TerniVinti,
            //        risultato.InvestimentoMinimo.ToString("C2"),
            //        risultato.Guadagno.ToString("C2"),
            //        risultato.InvestimentoProposto.ToString("C2"),
            //        risultato.GuadagnoProposto.ToString("C2")
            //    );
            //}

            // Calcola totali
            CalcolaTotali();
        }

        private void CalcolaTotali()
        {
            decimal totInvestimentoMin = risultati.Sum(r => r.InvestimentoMinimo);
            decimal totGuadagno = risultati.Sum(r => r.Guadagno);
            decimal totInvestimentoProp = risultati.Sum(r => r.InvestimentoProposto);
            decimal totGuadagnoProp = risultati.Sum(r => r.GuadagnoProposto);
            int numvincenti = risultati.Sum(r => r.NumeriVinti);
            int numambi = risultati.Sum(r => r.AmbiVinti);
            int numterni = risultati.Sum(r => r.TerniVinti);
            int numgiocati = risultati.Sum(r => r.NumeriGiocati);

            lblTotali.Text = $"Totali - Invest.Min: {totInvestimentoMin:C2} | Guadagno: {totGuadagno:C2} | " +
                           $"Invest.Prop: {totInvestimentoProp:C2} | Guadagno Prop: {totGuadagnoProp:C2} | " +
                           $"Bilancio: {(totGuadagnoProp - totInvestimentoProp):C2} | " +
                           $"Tot Giocati: {(numgiocati)} | " +
                           $"Tot Vincenti: {(numvincenti)}  | Tot Ambi: {numambi} | Tot Terni: {numterni}";
        }

        private void BtnAggiorna_Click(object sender, EventArgs e)
        {
            string raggruppamento = OttieniRaggruppamentoSelezionato();
            budgetDisponibile = numBudget.Value;

            // Qui implementerai il raggruppamento reale dei dati
            MessageBox.Show($"Analisi aggiornata con raggruppamento: {raggruppamento}\nBudget: {budgetDisponibile:C2}",
                          "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            AggiornaGriglia();
        }

        private string OttieniRaggruppamentoSelezionato()
        {
            //if (radioEstrazione.Checked) return "Estrazione";
            if (radioSettimana.Checked) return "Settimana";
            if (radioMese.Checked) return "Mese";
            if (radioTrimestre.Checked) return "Trimestre";
            if (radioAnno.Checked) return "Anno";
            return "Mese";
        }

        private void BtnEsporta_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "File CSV (*.csv)|*.csv";
                saveDialog.Title = "Esporta risultati analisi";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    EsportaCsv(saveDialog.FileName);
                    MessageBox.Show("Dati esportati con successo!", "Esportazione",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EsportaCsv(string filePath)
        {
            // Implementazione semplificata esportazione CSV
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("Intervallo;Estrazioni;NumeriGiocati;NumeriVinti;AmbiVinti;TerniVinti;InvestimentoMin;Guadagno;InvestimentoProp;GuadagnoProp");
                foreach (var risultato in risultati)
                {
                    writer.WriteLine($"{risultato.Intervallo};{risultato.NumeroEstrazioni};{risultato.NumeriGiocati};" +
                                   $"{risultato.NumeriVinti};{risultato.AmbiVinti};{risultato.TerniVinti};" +
                                   $"{risultato.InvestimentoMinimo};{risultato.Guadagno};" +
                                   $"{risultato.InvestimentoProposto};{risultato.GuadagnoProposto}");
                }
            }
        }

        private void BtnAvviaAnalisi_Click(object sender, EventArgs e)
        {
            try
            {
                // Ottieni le ruote selezionate
                List<string> ruoteSelezionate = OttieniRuoteSelezionate();

                if (ruoteSelezionate.Count == 0)
                {
                    MessageBox.Show("Seleziona almeno una ruota per l'analisi.", "Attenzione",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ottieni l'intervallo di date dai parametri condivisi
                //DateTime dataInizioAnalisi = ParametriCondivisi.DataInizioAnalisi;
                //DateTime dataFineAnalisi = ParametriCondivisi.DataFineAnalisi;

                DateTime dataInizioAnalisi = dateTimeInizio.Value;
                DateTime dataFineAnalisi = dateTimeFine.Value;

                if (dataInizioAnalisi < ParametriCondivisi.DataInizioAnalisi)
                    dataInizioAnalisi = ParametriCondivisi.DataInizioAnalisi;

                if (dataFineAnalisi > ParametriCondivisi.DataFineAnalisi)
                    dataFineAnalisi = ParametriCondivisi.DataFineAnalisi;


                // Verifica che le date siano valide
                if (dataInizioAnalisi == DateTime.MinValue || dataFineAnalisi == DateTime.MinValue)
                {
                    MessageBox.Show("Intervallo date non valido. Carica prima i dati nel Tabellone.",
                                  "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                // Prepara i risultati
                List<RisultatoAnalisi> nuoviRisultati = new List<RisultatoAnalisi>();
                List<RisultatoEstrazione> nuoviRisultatiDet = new List<RisultatoEstrazione>();

                // Per ogni periodo nel raggruppamento selezionato
                string raggruppamento = OttieniRaggruppamentoSelezionato();
                var periodi = SuddividiInPeriodi(dataInizioAnalisi, dataFineAnalisi, raggruppamento);

                ModelloAdattivo ModelloAdattivo = new ModelloAdattivo(ParametriCondivisi.Estrazioni);

                foreach (var periodo in periodi)
                {
                    // Per ogni ruota selezionata
                    foreach (string ruota in ruoteSelezionate)
                    {
                        // Ottieni le estrazioni per questo periodo e ruota
                        var estrazioniPeriodo = OttieniEstrazioniPerPeriodo(periodo.DataInizio, periodo.DataFine, ruota);

                        foreach (var estrazione in estrazioniPeriodo)
                        {
                            // Chiama il modello adattivo per la previsione
                            var numeriPrevisione = ModelloAdattivo.PrevisioneConAdattivita(
                                ruota: ruota,
                                dataTarget: estrazione.Data,
                                numeriUsciti: estrazione.Numeri, // Passa i numeri realmente usciti per la simulazione
                                out List<Tuple<int, int, List<string>>> numPesiRegole
                            );
                            //var punteggi = ModelloAdattivo.GetPunteggiCompleti(ruota, estrazione.Data);
                            // Qui processi il risultato della previsione e calcoli le metriche
                            // (questa parte dipende dalla struttura del tuo modello)
                            ProcessaRisultatoPrevisione(nuoviRisultati, periodo, estrazione, numeriPrevisione, ruota);

                            RisultatoEstrazione re = new RisultatoEstrazione();
                            re.Ruota = ruota;
                            re.Data = estrazione.Data;
                            re.NumeriPrevisionePeso = numPesiRegole.Select(x => (x.Item1, x.Item2)).ToList();
                            re.NumeriEstrazione = estrazione.Numeri;
                            nuoviRisultatiDet.Add(re);
                        }
                    }
                }

                // Aggiorna i risultati e la griglia
                risultati = nuoviRisultati;
                risultatiEstr = nuoviRisultatiDet;
                AggiornaGriglia();

                this.Cursor = Cursors.Default;

                MessageBox.Show($"Analisi completata! Processate {nuoviRisultati.Count} periodi.",
                              "Analisi Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'analisi: {ex.Message}",
                              "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
            }
        }

        private List<string> OttieniRuoteSelezionate()
        {
            List<string> ruoteSelezionate = new List<string>();

            // Controlla ogni checkbox delle ruote
            if (chkBari.Checked) ruoteSelezionate.Add("BA");
            if (chkCagliari.Checked) ruoteSelezionate.Add("CA");
            if (chkFirenze.Checked) ruoteSelezionate.Add("FI");
            if (chkGenova.Checked) ruoteSelezionate.Add("GE");
            if (chkMilano.Checked) ruoteSelezionate.Add("MI");
            if (chkNapoli.Checked) ruoteSelezionate.Add("NA");
            if (chkPalermo.Checked) ruoteSelezionate.Add("PA");
            if (chkRoma.Checked) ruoteSelezionate.Add("RM");
            if (chkTorino.Checked) ruoteSelezionate.Add("TO");
            if (chkVenezia.Checked) ruoteSelezionate.Add("VE");
            if (chkNazionale.Checked) ruoteSelezionate.Add("NZ");

            return ruoteSelezionate;
        }

        private List<Periodo> SuddividiInPeriodi(DateTime dataInizio, DateTime dataFine, string raggruppamento)
        {
            var periodi = new List<Periodo>();
            DateTime dataCorrente = dataInizio;

            if (raggruppamento == "Estrazione")
            {
                foreach (DateTime dt in ParametriCondivisi.Estrazioni.Select(X => X.Data).Distinct().Where(X => X >= dataInizio && X < dataFine).OrderBy(X => X))
                {
                    periodi.Add(new Periodo
                    {
                        DataInizio = dt,
                        DataFine = dt.AddDays(1)
                    });
                }
            }
            else
            {
                while (dataCorrente < dataFine)
                {
                    DateTime finePeriodo = dataCorrente;

                    switch (raggruppamento)
                    {

                        case "Settimana":
                            finePeriodo = dataCorrente.AddDays(7);
                            if (finePeriodo.DayOfWeek != DayOfWeek.Monday)
                                finePeriodo = finePeriodo.AddDays(-(int)DayOfWeek.Monday);
                            break;
                        case "Mese":
                            finePeriodo = dataCorrente.AddMonths(1);
                            //if (finePeriodo.Day != 1)
                            finePeriodo = finePeriodo.AddDays(-finePeriodo.Day);
                            break;
                        case "Trimestre":
                            finePeriodo = dataCorrente.AddMonths(3);
                            if (finePeriodo.Day != 1)
                                finePeriodo = finePeriodo.AddDays(-finePeriodo.Day);
                            break;
                        case "Anno":
                            finePeriodo = dataCorrente.AddYears(1);
                            //if (finePeriodo.Month != 1)
                            finePeriodo = finePeriodo.AddDays(-finePeriodo.DayOfYear);
                            break;
                    }

                    if (finePeriodo > dataFine)
                        finePeriodo = dataFine;

                    periodi.Add(new Periodo
                    {
                        DataInizio = dataCorrente,
                        DataFine = finePeriodo
                    });

                    dataCorrente = finePeriodo.AddDays(1);
                }
            }
            return periodi;
        }

        private List<Estrazione> OttieniEstrazioniPerPeriodo(DateTime dataInizio, DateTime dataFine, string ruota)
        {
            // Questo metodo dovrebbe recuperare le estrazioni dal database o dalla fonte dati
            // Per ora restituisco una lista vuota - tu implementerai la logica specifica
            return ParametriCondivisi.Estrazioni.Where(X => X.Ruota.Equals(ruota) && X.Data >= dataInizio && X.Data < dataFine).ToList();
        }

        private void ProcessaRisultatoPrevisione(List<RisultatoAnalisi> risultati, Periodo periodo,
                                               Estrazione estrazione, List<int> numeriPrevisione, string ruota)
        {
            // Implementa qui la logica per processare il risultato della previsione
            // Confronta i numeri previsti con quelli realmente usciti
            // Calcola ambi, terni vinti, etc.

            // Esempio semplificato:
            var risultatoEsistente = risultati.FirstOrDefault(r =>
                r.DataInizio == periodo.DataInizio && r.DataFine == periodo.DataFine);

            if (risultatoEsistente == null)
            {
                risultatoEsistente = new RisultatoAnalisi
                {
                    Intervallo = $"{periodo.DataInizio:dd/MM/yyyy} - {periodo.DataFine:dd/MM/yyyy}",
                    DataInizio = periodo.DataInizio,
                    DataFine = periodo.DataFine,
                    NumeroEstrazioni = 0,
                    NumeriGiocati = 0,
                    NumeriVinti = 0,
                    AmbiVinti = 0,
                    TerniVinti = 0
                };
                risultati.Add(risultatoEsistente);
            }

            // Aggiorna le metriche (logica semplificata - adatta alla tua implementazione)
            risultatoEsistente.NumeroEstrazioni++;
            risultatoEsistente.NumeriGiocati += numeriPrevisione?.Count ?? 0;

            // Qui dovresti aggiungere la logica per calcolare i numeri vinti, ambi, terni
            // confrontando numeriPrevisione con estrazione.NumeriEstratti
            risultatoEsistente.NumeriVinti = numeriPrevisione.Where(X => X == estrazione.Numeri[0] || X == estrazione.Numeri[1] ||
                    X == estrazione.Numeri[2] ||
                    X == estrazione.Numeri[3] ||
                    X == estrazione.Numeri[4]).Count();// estrazione.Numeri

            risultatoEsistente.AmbiVinti = CalcolaCombinazioni(risultatoEsistente.NumeriVinti, 2);
            risultatoEsistente.TerniVinti = CalcolaCombinazioni(risultatoEsistente.NumeriVinti, 3);
        }

        private int CalcolaCombinazioni(int n, int k)
        {
            if (k > n) return 0;
            if (k == 0 || k == n) return 1;
            decimal numeratore = 1;
            for (int i = 0; i < k; i++)
            {
                numeratore *= (n - i);
            }
            decimal denominatore = 1;
            for (int i = 1; i <= k; i++)
            {
                denominatore *= i;
            }
            return (int)(numeratore / denominatore);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            ModelloAdattivo ModelloAdattivo = new ModelloAdattivo(ParametriCondivisi.Estrazioni);
            // DEBUG COMPLETO
            DateTime dataTarget = new DateTime(2024, 12, 07);
            var ritardi = ModelloAdattivo.CalcolaRitardiFigure("RM", dataTarget );
            Console.WriteLine($"Ritardo figura {ModelloAdattivo.Figura(89)}: {ritardi[ModelloAdattivo.Figura(89)]}");

            var forzaGrav = ModelloAdattivo.CalcolaForzaGravitazionaleTotale(89, ritardi, "RM", dataTarget);
            var energia = ModelloAdattivo.CalcolaEnergiaPotenziale(89, ritardi);

            txtResTest.Text = $"Ritardo figura {ModelloAdattivo.Figura(89)}: {ritardi[ModelloAdattivo.Figura(89)]}";
            txtResTest.Text += "\nForza gravitazionale: " + forzaGrav.ToString("0.000");
            txtResTest.Text += "\nEnergia: " + energia.ToString("0.000");
        }
    }

    public class RisultatoAnalisi
    {
        public string Intervallo { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public int NumeroEstrazioni { get; set; }
        public int NumeriGiocati { get; set; }
        public int NumeriVinti { get; set; }
        public int AmbiVinti { get; set; }
        public int TerniVinti { get; set; }
        public decimal InvestimentoMinimo { get; set; }
        public decimal Guadagno { get; set; }
        public decimal InvestimentoProposto { get; set; }
        public decimal GuadagnoProposto { get; set; }
    }

    public class RisultatoEstrazione
    {
        public string Ruota { get; set; }
        public DateTime Data{ get; set; }
        public string NumPrevistiPesi 
        { 
            get 
            {
                return string.Join(", ", NumeriPrevisionePeso.Select(x => $"{x.Item1}({x.Item2})"));
            }
        }

        public string NumEstratti
        {
            get
            {
                return string.Join(", ", NumeriEstrazione.Select(x => $"{x.ToString()}"));
            }
        }

        public List<(int,int)> NumeriPrevisionePeso { get; set; }
        public List<int> NumeriEstrazione { get; set; }
    }
}