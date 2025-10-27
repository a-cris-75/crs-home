using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public class ModelloCicliIntelligente
    {
        private Dictionary<string, List<int>> _famiglieNumeri;
        private Dictionary<string, StatisticheCluster> _statisticheCluster;
        private Dictionary<string, CicloCluster> _mappaCicli;

        public ModelloCicliIntelligente()
        {
            _famiglieNumeri = LottoMath.CaricaFamiglieNumeri();
            // Questi verrebbero caricati da analisi precalcolata
            _statisticheCluster = CaricaStatisticheCluster();
            _mappaCicli = CaricaMappaCicli();
        }

        public List<int> PrevediNumeri(List<List<int>> ultime3Estrazioni, int quantiNumeri, out List<Tuple<int, int, List<string>>> numPesiRegole)
        {
            // 🎯 1. ANALISI CLUSTER ATTUALI
            var clusterAttuali = AnalizzaClusterAttuali(ultime3Estrazioni);

            // 🎯 2. IDENTIFICA CLUSTER IN ESAURIMENTO (soglia critica)
            var clusterInEsaurimento = TrovaClusterInEsaurimento(clusterAttuali);

            // 🎯 3. PREVEDI PROSSIMI CLUSTER (basato su cicli temporali)
            var prossimiCluster = PrevediProssimiCluster(clusterAttuali, clusterInEsaurimento);

            // 🎯 4. COSTRUISCI PREVISIONE FINALE
            var previsioni = CostruisciPrevisioneFinale(clusterInEsaurimento, prossimiCluster, quantiNumeri);

            // uso peso simbolico fisso in questo modello
            numPesiRegole = previsioni.Select(pe => Tuple.Create(pe, (int)(100), new List<string> { "Energia Modello Cicli Intelligente * 100" }))
               .ToList();

            return previsioni;
        }

        // 🎯 1. ANALISI CLUSTER ATTUALI
        private List<ClusterAttivo> AnalizzaClusterAttuali(List<List<int>> estrazioni)
        {
            var clusterAttivi = new List<ClusterAttivo>();
            var tipiCluster = new[] { "figura", "decina", "unita" };

            foreach (var tipo in tipiCluster)
            {
                var clusterPresenti = GetClusterPresentiInEstrazioni(estrazioni, tipo);

                foreach (var clusterValore in clusterPresenti)
                {
                    int durata = CalcolaDurataClusterContinua(clusterValore, tipo, estrazioni);
                    double forza = CalcolaForzaCluster(clusterValore, tipo, estrazioni);

                    clusterAttivi.Add(new ClusterAttivo
                    {
                        Tipo = tipo,
                        Valore = clusterValore,
                        DurataAttuale = durata,
                        ForzaAttuale = forza,
                        InEsaurimento = durata >= _statisticheCluster[tipo].DurataMedia
                    });
                }
            }

            return clusterAttivi;
        }

        // 🎯 2. IDENTIFICA CLUSTER IN ESAURIMENTO
        private List<ClusterAttivo> TrovaClusterInEsaurimento(List<ClusterAttivo> clusterAttivi)
        {
            return clusterAttivi.Where(c => c.InEsaurimento).ToList();
        }

        // 🎯 3. PREVEDI PROSSIMI CLUSTER
        private List<ClusterAttivo> PrevediProssimiCluster(List<ClusterAttivo> attuali, List<ClusterAttivo> inEsaurimento)
        {
            var prossimi = new List<ClusterAttivo>();

            // STRATEGIA A: TRANSIZIONI ARMONICHE
            foreach (var clusterEsauriente in inEsaurimento)
            {
                var transizioni = TrovaTransizioniArmoniche(clusterEsauriente);
                prossimi.AddRange(transizioni);
            }

            // STRATEGIA B: CICLI TEMPORALI
            var clusterCiclici = PrevediClusterDaCicliTemporali(attuali);
            prossimi.AddRange(clusterCiclici);

            // STRATEGIA C: RIGENERAZIONE NATURALE
            var clusterRigeneranti = TrovaClusterInRigenerazione(attuali);
            prossimi.AddRange(clusterRigeneranti);

            return prossimi.DistinctBy(c => $"{c.Tipo}_{c.Valore}").ToList();
        }

        // 🎯 4. COSTRUISCI PREVISIONE FINALE
        private List<int> CostruisciPrevisioneFinale(List<ClusterAttivo> inEsaurimento,
                                                   List<ClusterAttivo> prossimi,
                                                   int quantiNumeri)
        {
            // ESCLUDI numeri dai cluster in esaurimento
            var numeriDaEscludere = GetNumeriDaCluster(inEsaurimento);

            // INCLUDE numeri dai prossimi cluster
            var numeriDaIncludere = GetNumeriDaCluster(prossimi);

            // NUMERI DI TRANSIZIONE (ponte tra cluster)
            var numeriTransizione = TrovaNumeriTransizione(inEsaurimento, prossimi);

            // COMBINA TUTTI I CANDIDATI
            var candidati = new HashSet<int>();

            // Priorità 1: Prossimi cluster (50%)
            candidati.UnionWith(numeriDaIncludere.Take(quantiNumeri / 2));

            // Priorità 2: Numeri di transizione (30%)
            candidati.UnionWith(numeriTransizione.Take((int)(quantiNumeri * 0.3)));

            // Priorità 3: Riempimento con numeri "freschi" (20%)
            var numeriFreschi = TrovaNumeriFreschi(numeriDaEscludere, quantiNumeri - candidati.Count);
            candidati.UnionWith(numeriFreschi);

            return candidati.Take(quantiNumeri).ToList();
        }

        // === METODI DI SUPPORTO ===

        private List<ClusterAttivo> TrovaTransizioniArmoniche(ClusterAttivo cluster)
        {
            var transizioni = new List<ClusterAttivo>();

            // TRANSIZIONI PER FIGURA
            if (cluster.Tipo == "figura")
            {
                var transizioniFigura = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 6, 8 } }, { 2, new List<int> { 7, 9 } },
                { 3, new List<int> { 7, 1 } }, { 4, new List<int> { 9, 2 } },
                { 6, new List<int> { 1, 3 } }, { 7, new List<int> { 3, 2 } },
                { 8, new List<int> { 2, 4 } }, { 9, new List<int> { 4, 1 } }
            };

                if (transizioniFigura.ContainsKey(cluster.Valore))
                {
                    foreach (var figuraTarget in transizioniFigura[cluster.Valore])
                    {
                        transizioni.Add(new ClusterAttivo { Tipo = "figura", Valore = figuraTarget });
                    }
                }
            }

            // TRANSIZIONI PER DECINA (complementari)
            if (cluster.Tipo == "decina")
            {
                int decinaTarget = 8 - cluster.Valore; // Complemento a 8
                if (decinaTarget >= 0 && decinaTarget <= 8)
                {
                    transizioni.Add(new ClusterAttivo { Tipo = "decina", Valore = decinaTarget });
                }
            }

            return transizioni;
        }

        private List<ClusterAttivo> PrevediClusterDaCicliTemporali(List<ClusterAttivo> attuali)
        {
            var prossimi = new List<ClusterAttivo>();

            // CICLI PER FIGURA (basato su mappa temporale)
            var figureAttive = attuali.Where(c => c.Tipo == "figura").Select(c => c.Valore).ToList();
            var figureInattive = Enumerable.Range(1, 9).Except(figureAttive).ToList();

            // Prevedi le figure più "in ritardo" nel loro ciclo
            foreach (var figura in figureInattive)
            {
                var ciclo = _mappaCicli["figura"];
                double probabilita = CalcolaProbabilitaAttivazione(figura, ciclo);
                if (probabilita > 0.6) // Soglia di attivazione
                {
                    prossimi.Add(new ClusterAttivo { Tipo = "figura", Valore = figura });
                }
            }

            return prossimi;
        }

        private List<int> TrovaNumeriTransizione(List<ClusterAttivo> inEsaurimento, List<ClusterAttivo> prossimi)
        {
            var numeriTransizione = new List<int>();

            // Numeri che fanno da ponte tra cluster in esaurimento e prossimi
            foreach (var esauriente in inEsaurimento)
            {
                foreach (var prossimo in prossimi)
                {
                    // Trova numeri che appartengono a entrambi i cluster (intersezione)
                    var numeriEsauriente = GetNumeriDaCluster(new List<ClusterAttivo> { esauriente });
                    var numeriProssimo = GetNumeriDaCluster(new List<ClusterAttivo> { prossimo });

                    var intersezione = numeriEsauriente.Intersect(numeriProssimo).ToList();
                    numeriTransizione.AddRange(intersezione);
                }
            }

            return numeriTransizione.Distinct().ToList();
        }

        private List<int> TrovaNumeriFreschi(List<int> daEscludere, int quanti)
        {
            // Numeri che non sono usciti recentemente e non sono nei cluster in esaurimento
            return Enumerable.Range(1, 90)
                            .Except(daEscludere)
                            .OrderBy(n => new Random().Next()) // Random ma escludendo quelli in esaurimento
                            .Take(quanti)
                            .ToList();
        }

        // === CLASSI DI SUPPORTO ===

        public class ClusterAttivo
        {
            public string Tipo { get; set; } // "figura", "decina", "unita"
            public int Valore { get; set; }  // 1-9 per figura, 0-8 per decina, 0-9 per unità
            public int DurataAttuale { get; set; }
            public double ForzaAttuale { get; set; }
            public bool InEsaurimento { get; set; }
        }

        public class StatisticheCluster
        {
            public string Tipo { get; set; }
            public double DurataMedia { get; set; }
            public double DurataMassima { get; set; }
            public double FrequenzaRiapparizione { get; set; }
        }

        public class CicloCluster
        {
            public string Tipo { get; set; }
            public double FrequenzaAttivazione { get; set; }
            public double DurataMediaCiclo { get; set; }
            public Dictionary<int, double> DistribuzioneTemporale { get; set; }
        }

        // === METODI AUSILIARI ===

        private List<int> GetNumeriDaCluster(List<ClusterAttivo> cluster)
        {
            var numeri = new List<int>();

            foreach (var c in cluster)
            {
                string key = $"{c.Tipo}_{c.Valore}";
                if (_famiglieNumeri.ContainsKey(key))
                {
                    numeri.AddRange(_famiglieNumeri[key]);
                }
            }

            return numeri.Distinct().ToList();
        }

        private List<int> GetClusterPresentiInEstrazioni(List<List<int>> estrazioni, string tipo)
        {
            var clusterPresenti = new HashSet<int>();

            foreach (var estrazione in estrazioni)
            {
                var clusterEstrazione = GetClusterEstrazione(estrazione, tipo);
                foreach (var cluster in clusterEstrazione)
                {
                    clusterPresenti.Add(cluster);
                }
            }

            return clusterPresenti.ToList();
        }

        private int CalcolaDurataClusterContinua(int clusterValore, string tipo, List<List<int>> estrazioni)
        {
            int durata = 0;

            for (int i = estrazioni.Count - 1; i >= 0; i--)
            {
                var clusterEstrazione = GetClusterEstrazione(estrazioni[i], tipo);
                if (clusterEstrazione.Contains(clusterValore))
                    durata++;
                else
                    break;
            }

            return durata;
        }

        // === METODI DI INIZIALIZZIONE (dati precalcolati) ===

        private Dictionary<string, StatisticheCluster> CaricaStatisticheCluster()
        {
            // Dati dall'analisi precedente
            return new Dictionary<string, StatisticheCluster>
        {
            { "figura", new StatisticheCluster { Tipo = "figura", DurataMedia = 2.1, DurataMassima = 8, FrequenzaRiapparizione = 4.2 } },
            { "decina", new StatisticheCluster { Tipo = "decina", DurataMedia = 1.8, DurataMassima = 6, FrequenzaRiapparizione = 3.7 } },
            { "unita", new StatisticheCluster { Tipo = "unita", DurataMedia = 1.5, DurataMassima = 5, FrequenzaRiapparizione = 4.8 } }
        };
        }

        private Dictionary<string, CicloCluster> CaricaMappaCicli()
        {
            return new Dictionary<string, CicloCluster>
        {
            { "figura", new CicloCluster {
                Tipo = "figura",
                FrequenzaAttivazione = 4.2,
                DurataMediaCiclo = 6.0,
                DistribuzioneTemporale = new Dictionary<int, double>
                {
                    { 1, 0.15 }, { 2, 0.12 }, { 3, 0.18 }, { 4, 0.10 },
                    { 5, 0.08 }, { 6, 0.14 }, { 7, 0.11 }, { 8, 0.07 }, { 9, 0.05 }
                }
            }},
            { "decina", new CicloCluster {
                Tipo = "decina",
                FrequenzaAttivazione = 3.7,
                DurataMediaCiclo = 5.2,
                DistribuzioneTemporale = new Dictionary<int, double>
                {
                    { 0, 0.18 }, { 1, 0.12 }, { 2, 0.10 }, { 3, 0.09 },
                    { 4, 0.11 }, { 5, 0.08 }, { 6, 0.13 }, { 7, 0.09 }, { 8, 0.10 }
                }
            }}
        };
        }

        private double CalcolaForzaCluster(int clusterValore, string tipo, List<List<int>> estrazioni)
        {
            // La forza misura quanto è "dominante" un cluster nelle ultime estrazioni
            double forza = 0.0;
            double[] pesiTemporali = { 1.0, 0.7, 0.4 }; // Pesi decrescenti

            for (int i = 0; i < estrazioni.Count; i++)
            {
                var estrazione = estrazioni[i];
                int occorrenzeCluster = 0;

                foreach (var numero in estrazione)
                {
                    bool appartieneAlCluster = false;

                    switch (tipo)
                    {
                        case "figura":
                            appartieneAlCluster = LottoMath.CalcolaFigura(numero) == clusterValore;
                            break;
                        case "decina":
                            appartieneAlCluster = (numero / 10) == clusterValore;
                            break;
                        case "unita":
                            appartieneAlCluster = (numero % 10) == clusterValore;
                            break;
                    }

                    if (appartieneAlCluster)
                        occorrenzeCluster++;
                }

                // Normalizza per il numero massimo possibile in un'estrazione (5 numeri)
                double forzaEstrazione = (double)occorrenzeCluster / 5.0;
                forza += forzaEstrazione * pesiTemporali[i];
            }

            return forza / estrazioni.Count; // Media pesata
        }

        private List<ClusterAttivo> TrovaClusterInRigenerazione(List<ClusterAttivo> attuali)
        {
            var inRigenerazione = new List<ClusterAttivo>();

            // Cerca cluster che sono stati assenti per un po' ma stanno per rigenerarsi
            foreach (var tipo in new[] { "figura", "decina", "unita" })
            {
                var clusterAttuali = attuali.Where(c => c.Tipo == tipo).Select(c => c.Valore).ToList();
                var tuttiCluster = tipo == "figura" ? Enumerable.Range(1, 9) :
                                  tipo == "decina" ? Enumerable.Range(0, 9) :
                                  Enumerable.Range(0, 10);

                var clusterAssenti = tuttiCluster.Except(clusterAttuali).ToList();

                foreach (var cluster in clusterAssenti)
                {
                    // Un cluster è in rigenerazione se:
                    // 1. È assente da più della sua frequenza media di riapparizione
                    // 2. Ha una alta probabilità di attivazione nel ciclo corrente
                    double probabilitaAttivazione = CalcolaProbabilitaRigenerazione(cluster, tipo);
                    if (probabilitaAttivazione > 0.7)
                    {
                        inRigenerazione.Add(new ClusterAttivo
                        {
                            Tipo = tipo,
                            Valore = cluster,
                            ForzaAttuale = probabilitaAttivazione
                        });
                    }
                }
            }

            return inRigenerazione;
        }

        private double CalcolaProbabilitaRigenerazione(int cluster, string tipo)
        {
            // Basata sulla frequenza storica di riapparizione dopo periodi di assenza
            var stats = _statisticheCluster[tipo];

            // Probabilità inversamente proporzionale alla frequenza di riapparizione
            // (cluster che appaiono più raramente hanno maggiore probabilità di rigenerazione)
            double baseProb = 1.0 / stats.FrequenzaRiapparizione;

            // Aggiusta in base alla distribuzione temporale del ciclo
            if (_mappaCicli.ContainsKey(tipo) &&
                _mappaCicli[tipo].DistribuzioneTemporale.ContainsKey(cluster))
            {
                baseProb *= _mappaCicli[tipo].DistribuzioneTemporale[cluster];
            }

            return Math.Min(baseProb * 3.0, 0.95); // Cap a 95%
        }

        private double CalcolaProbabilitaAttivazione(int figura, CicloCluster ciclo)
        {
            if (ciclo.DistribuzioneTemporale.ContainsKey(figura))
            {
                return ciclo.DistribuzioneTemporale[figura];
            }

            // Fallback: distribuzione uniforme per figure non nella mappa
            return 1.0 / 9.0;
        }

        private List<int> GetClusterEstrazione(List<int> estrazione, string tipo)
        {
            var cluster = new HashSet<int>();

            foreach (var numero in estrazione)
            {
                switch (tipo)
                {
                    case "figura":
                        cluster.Add(LottoMath.CalcolaFigura(numero));
                        break;
                    case "decina":
                        cluster.Add(numero / 10);
                        break;
                    case "unita":
                        cluster.Add(numero % 10);
                        break;
                }
            }

            return cluster.ToList();
        }

        private bool ClusterPresente(List<int> estrazione, ClusterAttivo cluster)
        {
            var clusterEstrazione = GetClusterEstrazione(estrazione, cluster.Tipo);
            return clusterEstrazione.Contains(cluster.Valore);
        }
    }

}
