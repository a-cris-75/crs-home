using Crs.Home.ApocalypsData;
using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crs.Home.ApocalypsData
{
    public class ModelloAdattivo3 : ModelloLotto
    {
        private Dictionary<string, RegolaPerformance> performanceRegole;
        private int estrazioniDaUltimoAggiornamento = 0;
        private const int INTERVALLO_AGGIORNAMENTO = 30;
        private OndaNumerica onda = new OndaNumerica();

        public ModelloAdattivo3(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
            : base(estrazioniStoriche, parametri)
        {
            InizializzaPerformanceRegole();
        }

        private void InizializzaPerformanceRegole()
        {
            performanceRegole = new Dictionary<string, RegolaPerformance>
            {
                { "Decina", new RegolaPerformance { NomeRegola = "Decina", UltimoReset = DateTime.Now } },
                { "FiguraAntifigura", new RegolaPerformance { NomeRegola = "FiguraAntifigura", UltimoReset = DateTime.Now } },
                { "Polarizzazione", new RegolaPerformance { NomeRegola = "Polarizzazione", UltimoReset = DateTime.Now } },
                { "Ritardo", new RegolaPerformance { NomeRegola = "Ritardo", UltimoReset = DateTime.Now } },
                { "Armonia", new RegolaPerformance { NomeRegola = "Armonia", UltimoReset = DateTime.Now } },
                { "DifferenzaRitardi", new RegolaPerformance { NomeRegola = "DifferenzaRitardi", UltimoReset = DateTime.Now } },
                { "AutoAttrazione9", new RegolaPerformance { NomeRegola = "AutoAttrazione9", UltimoReset = DateTime.Now } },
                { "InterRuota", new RegolaPerformance { NomeRegola = "InterRuota", UltimoReset = DateTime.Now } },
                { "Temporale", new RegolaPerformance { NomeRegola = "Temporale", UltimoReset = DateTime.Now } },
                { "Sequenza", new RegolaPerformance { NomeRegola = "Sequenza", UltimoReset = DateTime.Now } },
                { "Fisica", new RegolaPerformance { NomeRegola = "Fisica", UltimoReset = DateTime.Now } }
            };
        }

        public Tuple<int, List<string>> CalcolaBonusConRegole(int numero, string ruota, DateTime dataTarget)
        {
            var regoleAttivate = new List<string>();

            // Calcolo dei bonus individuali
            int B_dec = CalcolaBonusDecina(numero, ruota, dataTarget);
            int B_FA = CalcolaBonusFiguraAntifigura(numero, ruota, dataTarget);
            int B_pol = CalcolaBonusPolarizzazione(numero, ruota, dataTarget);
            int B_rit = CalcolaBonusRitardo(numero, ruota, dataTarget);
            int B_arm = CalcolaBonusArmonia(numero, ruota, dataTarget);
            int B_diff = CalcolaBonusDifferenzaRitardi(numero, ruota, dataTarget);
            int B_9 = CalcolaBonusAutoAttrazione9(numero, ruota, dataTarget);
            int B_ir = CalcolaBonusInterRuota(numero, ruota, dataTarget);
            int B_temp = CalcolaBonusTemporale(numero, ruota, dataTarget);
            int B_seq = CalcolaBonusSequenza(numero, ruota, dataTarget);
            int B_fisica = CalcolaBonusFisicaCompleto(numero, ruota, dataTarget);

            // Registra regole attivate
            if (B_dec > 0) regoleAttivate.Add("Decina");
            if (B_FA > 0) regoleAttivate.Add("FiguraAntifigura");
            if (B_pol > 0) regoleAttivate.Add("Polarizzazione");
            if (B_rit > 0) regoleAttivate.Add("Ritardo");
            if (B_arm > 0) regoleAttivate.Add("Armonia");
            if (B_diff > 0) regoleAttivate.Add("DifferenzaRitardi");
            if (B_9 > 0) regoleAttivate.Add("AutoAttrazione9");
            if (B_ir > 0) regoleAttivate.Add("InterRuota");
            if (B_temp > 0) regoleAttivate.Add("Temporale");
            if (B_seq > 0) regoleAttivate.Add("Sequenza");
            if (B_fisica > 0) regoleAttivate.Add("Fisica");

            // CALCOLO MOLTIPLICATIVO
            double bonus_moltiplicativo =
                (B_dec / 10.0 + 1) *
                (B_FA / 10.0 + 1) *
                (B_pol / 10.0 + 1) *
                (B_rit / 10.0 + 1) *
                (B_arm / 10.0 + 1) *
                (B_diff / 10.0 + 1) *
                (B_9 / 10.0 + 1) *
                (B_ir / 10.0 + 1) *
                (B_temp / 10.0 + 1) *
                (B_seq / 10.0 + 1) *
                (B_fisica / 10.0 + 1) * 10;

            return Tuple.Create((int)Math.Round(bonus_moltiplicativo), regoleAttivate);
        }

        // ========== IMPLEMENTAZIONI DEI METODI BONUS ==========

        private int CalcolaBonusDecina(int numero, string ruota, DateTime dataTarget)
        {
            int B_dec = 0;
            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 2);
            var decineUltime2 = new HashSet<int>();

            foreach (var e in estrRuota)
            {
                foreach (int n in e.Numeri)
                {
                    decineUltime2.Add(Decina(n));
                }
            }

            int dNum = Decina(numero);
            if (decineUltime2.Contains(dNum))
            {
                if (estrRuota.Count > 0 && estrRuota[0].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += param.W_dec1;
                if (estrRuota.Count > 1 && estrRuota[1].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += param.W_dec2;
            }
            return B_dec;
        }

        //private int Decina(int numero)
        //{
        //    if (numero == 90) return 90;
        //    return (numero / 10) * 10;
        //}

        //private int Antifigura(int numero)
        //{
        //    int comp = 90 - numero;
        //    return Figura(comp);
        //}


        private int CalcolaBonusFiguraAntifigura(int numero, string ruota, DateTime dataTarget)
        {
            int B_FA = 0;
            int fNum = Figura(numero);
            if (fNum == 9) return 0; // Escludi figura 9

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);
            var coppieFA = new HashSet<Tuple<int, int>>
            {
                Tuple.Create(1, 8), Tuple.Create(8, 1),
                Tuple.Create(4, 5), Tuple.Create(5, 4)
            };

            for (int i = 0; i < estrRuota.Count; i++)
            {
                foreach (int n in estrRuota[i].Numeri)
                {
                    int fN = Figura(n);
                    if (fN == 9) continue;
                    var coppia = Tuple.Create(fN, fNum);
                    if (coppieFA.Contains(coppia))
                    {
                        if (i == 0) B_FA += param.W_FA;
                        else if (i == 1) B_FA += (int)(param.W_FA * 0.5);
                        else if (i == 2) B_FA += (int)(param.W_FA * 0.33);
                    }
                }
            }
            return B_FA;
        }

        private int CalcolaBonusPolarizzazione(int numero, string ruota, DateTime dataTarget)
        {
            int B_pol = 0;
            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 1);
            if (estrRuota.Count == 0) return 0;

            var ultimaCinquina = estrRuota[0].Numeri;
            var figureUltima = new HashSet<int>(ultimaCinquina.Select(Figura));
            int varieta = figureUltima.Count;
            int fNum = Figura(numero);

            if (varieta == 5) // Alta varietà → bonus figure già usate
            {
                var figureUltime3 = new HashSet<int>();
                foreach (var e in GetUltimeEstrazioni(ruota, dataTarget, 3))
                {
                    figureUltime3.UnionWith(e.Numeri.Select(Figura));
                }
                if (figureUltime3.Contains(fNum))
                    B_pol += param.W_pol;
            }
            else if (varieta <= 3) // Bassa varietà → bonus figure nuove
            {
                if (!figureUltima.Contains(fNum))
                    B_pol += param.W_pol;
            }
            return B_pol;
        }

        private int CalcolaBonusRitardo(int numero, string ruota, DateTime dataTarget)
        {
            int B_rit = 0;
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int fNum = Figura(numero);
            int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;

            if (ritardoF >= 25) B_rit += param.W_rit25;
            else if (ritardoF >= 20) B_rit += param.W_rit20;
            else if (ritardoF >= 15) B_rit += param.W_rit15;

            return B_rit;
        }

        private Dictionary<int, int> CalcolaRitardiFigure(string ruota, DateTime dataTarget)
        {
            var estrRuota = estrazioni
                .Where(e => e.Ruota == ruota && e.Data <= dataTarget)
                .OrderBy(e => e.Data)
                .ToList();

            if (estrRuota.Count == 0)
                return Enumerable.Range(1, 9).ToDictionary(f => f, f => 1000);

            var datesEstrazioni = estrRuota.Select(e => e.Data).Distinct().OrderBy(d => d).ToList();
            var dataToIndex = datesEstrazioni.Select((d, i) => new { d, i }).ToDictionary(x => x.d, x => x.i);
            int idxTarget = dataToIndex[dataTarget];

            var ultimaUscita = new Dictionary<int, DateTime>();
            for (int f = 1; f <= 9; f++)
                ultimaUscita[f] = DateTime.MinValue;

            foreach (var e in estrRuota)
            {
                foreach (int num in e.Numeri)
                {
                    int f = Figura(num);
                    if (e.Data > ultimaUscita[f])
                        ultimaUscita[f] = e.Data;
                }
            }

            var ritardi = new Dictionary<int, int>();
            for (int f = 1; f <= 9; f++)
            {
                if (ultimaUscita[f] == DateTime.MinValue)
                    ritardi[f] = idxTarget + 1;
                else
                    ritardi[f] = idxTarget - dataToIndex[ultimaUscita[f]];
            }

            return ritardi;
        }

        private int CalcolaBonusArmonia(int numero, string ruota, DateTime dataTarget)
        {
            int B_arm = 0;
            var figureUltime3 = GetUltimeEstrazioni(ruota, dataTarget, 3)
                .SelectMany(e => e.Numeri.Select(Figura));
            int fNum = Figura(numero);

            foreach (int fAltra in figureUltime3)
            {
                if (fAltra + fNum == 9 || fAltra + fNum == 10)
                {
                    B_arm += param.W_arm;
                    break;
                }
            }
            return B_arm;
        }

        private int CalcolaBonusDifferenzaRitardi(int numero, string ruota, DateTime dataTarget)
        {
            int B_diff = 0;
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int fNum = Figura(numero);
            int afNum = Antifigura(numero);

            var coppieFA = new HashSet<Tuple<int, int>>
            {
                Tuple.Create(1, 8), Tuple.Create(8, 1),
                Tuple.Create(4, 5), Tuple.Create(5, 4)
            };

            if (coppieFA.Contains(Tuple.Create(fNum, afNum)))
            {
                int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;
                int ritardoAF = ritardi.ContainsKey(afNum) ? ritardi[afNum] : 1000;
                int diff = Math.Abs(ritardoF - ritardoAF);
                if (diff > param.Soglia_diff)
                {
                    B_diff += (diff - param.Soglia_diff) * param.W_diff;
                }
            }
            return B_diff;
        }

        private int CalcolaBonusAutoAttrazione9(int numero, string ruota, DateTime dataTarget)
        {
            int B_9 = 0;
            int fNum = Figura(numero);
            if (fNum != 9) return 0;

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);
            for (int i = 0; i < estrRuota.Count; i++)
            {
                if (estrRuota[i].Numeri.Any(n => Figura(n) == 9))
                {
                    if (i == 0) B_9 += 20;
                    else if (i == 1) B_9 += 10;
                    else if (i == 2) B_9 += 5;
                    break;
                }
            }
            return B_9;
        }

        private int CalcolaBonusInterRuota(int numero, string ruota, DateTime dataTarget)
        {
            int B_ir = 0;
            foreach (string altraRuota in ruote)
            {
                if (altraRuota == ruota) continue;

                var estrAltraRuota = GetUltimeEstrazioni(altraRuota, dataTarget, 3);
                foreach (var e in estrAltraRuota)
                {
                    if (e.Numeri.Contains(numero))
                    {
                        if (CoppiaForti(ruota, altraRuota))
                            B_ir += 15;
                        else
                            B_ir += 10;
                        break;
                    }
                }
            }
            return B_ir;
        }

        private int CalcolaBonusTemporale(int numero, string ruota, DateTime dataTarget)
        {
            int B_temp = 0;
            DayOfWeek giornoTarget = dataTarget.DayOfWeek;
            int fNum = Figura(numero);

            var patternTemporali = new Dictionary<Tuple<string, DayOfWeek, int>, int>
            {
                { Tuple.Create("Roma", DayOfWeek.Tuesday, 8), 12 },
                { Tuple.Create("Napoli", DayOfWeek.Friday, 3), 12 },
                { Tuple.Create("Palermo", DayOfWeek.Saturday, 9), 12 },
                { Tuple.Create("Milano", DayOfWeek.Monday, 5), 10 },
                { Tuple.Create("Bari", DayOfWeek.Thursday, 1), 8 }
            };

            var key = Tuple.Create(ruota, giornoTarget, fNum);
            if (patternTemporali.ContainsKey(key))
            {
                B_temp += patternTemporali[key];
            }
            return B_temp;
        }

        private int CalcolaBonusSequenza(int numero, string ruota, DateTime dataTarget)
        {
            int B_seq = 0;
            var estrattiRecenti = GetUltimeEstrazioni(ruota, dataTarget, 3)
                .SelectMany(e => e.Numeri).ToList();

            if (CompletaSequenza(numero, estrattiRecenti))
            {
                B_seq += 15;
            }
            return B_seq;
        }

        private int CalcolaBonusFisicaCompleto(int numero, string ruota, DateTime dataTarget)
        {
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            var estrattiRecenti = GetUltimeEstrazioni(ruota, dataTarget, 3)
                .SelectMany(e => e.Numeri).ToList();

            // 1. GRAVITÀ
            double forzaGravitazionale = CalcolaForzaGravitazionaleTotale(numero, ritardi, ruota, dataTarget);

            // 2. ENERGIA POTENZIALE 
            double energia = CalcolaEnergiaPotenziale(numero, ritardi);

            // 3. ONDE
            double ampiezzaOnda = onda.Ampiezza(numero, dataTarget);
            double interferenza = onda.InterferenzaCostruttiva(numero, dataTarget, estrattiRecenti);

            // 4. ENTROPIA
            double entropiaPrecedente = CalcolaEntropiaUltimaCinquina(ruota, dataTarget);
            double bonusEntropia = entropiaPrecedente > 1.2 ? 15 : 0;

            // COMBINAZIONE
            double bonusFisica =
                (forzaGravitazionale * 0.3) +
                (energia * 0.4) +
                (interferenza * 20) +
                (bonusEntropia * 0.1) +
                (ampiezzaOnda * 5);

            return (int)Math.Round(Math.Min(60, bonusFisica));
        }

        // ========== METODI DI SUPPORTO ==========

        private List<Estrazione> GetUltimeEstrazioni(string ruota, DateTime dataTarget, int numeroEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numeroEstrazioni)
                .ToList();
        }

        private bool CoppiaForti(string ruotaA, string ruotaB)
        {
            var coppieForti = new HashSet<Tuple<string, string>>
            {
                Tuple.Create("Napoli", "Palermo"), Tuple.Create("Palermo", "Napoli"),
                Tuple.Create("Milano", "Torino"), Tuple.Create("Torino", "Milano"),
                Tuple.Create("Roma", "Napoli"), Tuple.Create("Napoli", "Roma"),
                Tuple.Create("Bari", "Napoli"), Tuple.Create("Napoli", "Bari")
            };
            return coppieForti.Contains(Tuple.Create(ruotaA, ruotaB));
        }

        // ... [ALTRI METODI: CalcolaForzaGravitazionaleTotale, CalcolaEnergiaPotenziale, 
        // CalcolaEntropiaUltimaCinquina, CompletaSequenza, ecc.]
    }
}