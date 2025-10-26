using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public class ParametriModello
    {
        public int W_dec1 { get; set; } = 20;
        public int W_dec2 { get; set; } = 10;
        public int W_FA { get; set; } = 30;
        public int W_pol { get; set; } = 5;
        public int W_rit15 { get; set; } = 15;
        public int W_rit20 { get; set; } = 25;
        public int W_rit25 { get; set; } = 35;
        public int W_arm { get; set; } = 5;
        public int W_diff { get; set; } = 1;
        // questi sono pesi fissi che non necessitano di aggiornamento

        public int Soglia_diff { get; set; } = 10;
        public int Soglia { get; set; } = 50;

        // Metodi per aggiornamento dinamico
        public void AggiornaDaPerformance(Dictionary<string, double> aggiornamenti)
        {
            foreach (var agg in aggiornamenti)
            {
                switch (agg.Key)
                {
                    case "Decina":
                        W_dec1 = LimitaIntervallo(W_dec1 * agg.Value, 10, 40);
                        W_dec2 = W_dec1 / 2;
                        break;
                    case "FiguraAntifigura":
                        W_FA = LimitaIntervallo(W_FA * agg.Value, 15, 50);
                        break;
                    case "Fisica":
                        // La fisica ha parametri fissi, ma possiamo aggiungere un moltiplicatore globale
                        break;
                        // ... altri casi ...
                }
            }
        }

        private int LimitaIntervallo(double valore, int min, int max)
        {
            return Math.Max(min, Math.Min(max, (int)Math.Round(valore)));
        }
    }

    public class ParametriModelloFibo
    {
        // ✅ CONFIGURAZIONE FIBO_BASE
        public int W_dec1 { get; set; } = 8;    // Fibonacci: 8
        public int W_dec2 { get; set; } = 4;    // Metà di W_dec1
        public int W_FA { get; set; } = 13;     // Fibonacci: 13
        public int W_pol { get; set; } = 3;     // Ridotto
        public int W_rit15 { get; set; } = 8;   // Scala Fibonacci
        public int W_rit20 { get; set; } = 13;  // Fibonacci: 13
        public int W_rit25 { get; set; } = 21;  // Fibonacci: 21
        public int W_arm { get; set; } = 3;     // Ridotto
        public int W_diff { get; set; } = 1;    // Mantenuto

        // ✅ SOGLIA FIBO_BASE
        public int Soglia { get; set; } = 65;
        public int Soglia_diff { get; set; } = 10;
    }

    public class ModelloLotto
    {
        protected ParametriModello param;
        protected List<Estrazione> estrazioni;
        protected HashSet<Tuple<int, int>> coppieFA;
        protected string[] ruote = {
            "Bari", "Cagliari", "Firenze", "Genova", "Milano",
            "Napoli", "Palermo", "Roma", "Torino", "Venezia", "Nazionale"
        };

        public ModelloLotto(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
        {
            this.estrazioni = estrazioniStoriche;
            this.param = parametri ?? new ParametriModello();
            coppieFA = new HashSet<Tuple<int, int>>
            {
                Tuple.Create(1, 8), Tuple.Create(8, 1),
                Tuple.Create(4, 5), Tuple.Create(5, 4)
            };
        }

        // Funzioni base
        public static int Figura(int numero)
        {
            int n = numero;
            while (n > 9)
            {
                n = n.ToString().Sum(c => int.Parse(c.ToString()));
            }
            return n;
        }

        protected int Antifigura(int numero)
        {
            int comp = 90 - numero;
            return Figura(comp);
        }

        public static int Decina(int numero)
        {
            if (numero == 90) return 90;
            return (numero / 10) * 10;
        }

        public Tuple<int, List<string>> CalcolaBonusConRegole(int numero, string ruota, DateTime dataTarget)
        {
            var regoleAttivate = new List<string>();

            // Calcolo dei bonus individuali
            int B_dec = CalcolaBonusDecina(numero, ruota, dataTarget);
            int B_FA = CalcolaBonusFiguraAntifigura(numero, ruota, dataTarget);//, out string debug);
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

        public int CalcolaBonusDecina2(int numero, string ruota, DateTime dataTarget)
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

        public int CalcolaBonusDecina(int numero, string ruota, DateTime dataTarget)
        {
            int dNum = Decina(numero);
            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 1); // SOLO 1 estrazione!

            if (estrRuota.Count > 0 && estrRuota[0].Numeri.Any(n => Decina(n) == dNum))
                return param.W_dec1; // SOLO 20, non 30!

            return 0;
        }

        public int CalcolaBonusFiguraAntifiguraDebug(int numero, string ruota, DateTime dataTarget, out string debug)
        {
            int B_FA = 0;
            int fNum = Figura(numero);
            debug = string.Empty;
            if (fNum == 9) return 0; // Escludi figura 9

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);

            debug = $"=== DEBUG FA per {numero} (figura {fNum}) ===";
            for (int i = 0; i < estrRuota.Count; i++)
            {
                debug += $"\nEstrazione {i} giorni fa:";
                foreach (int n in estrRuota[i].Numeri)
                {
                    int fN = Figura(n);
                    if (fN == 9) continue;
                    var coppia = Tuple.Create(fN, fNum);
                    if (coppieFA.Contains(coppia))
                    {
                        debug += $"\n  Trovata coppia FA: {fN}-{fNum} dal numero {n}";
                        int bonus = 0;
                        if (i == 0) bonus += param.W_FA;
                        else if (i == 1) bonus += (int)(param.W_FA * 0.5);
                        else if (i == 2) bonus += (int)(param.W_FA * 0.33);

                        B_FA += bonus;
                        debug += $"\n  Bonus aggiunto: {bonus}, Totale: {B_FA}";
                        break;
                    }
                }
            }
            return B_FA;
        }

        public int CalcolaBonusFiguraAntifigura(int numero, string ruota, DateTime dataTarget)
        {
            int fNum = Figura(numero);
            if (fNum == 9) return 0;

            var estrRuota = GetUltimeEstrazioni(ruota, dataTarget, 3);

            // Cerca la PRIMA coppia FA (più recente)
            for (int i = 0; i < estrRuota.Count; i++)
            {
                foreach (int n in estrRuota[i].Numeri)
                {
                    int fN = Figura(n);
                    if (fN == 9) continue;
                    var coppia = Tuple.Create(fN, fNum);
                    if (coppieFA.Contains(coppia))
                    {
                        // Trovata coppia - applica bonus e ESCI
                        if (i == 0) return param.W_FA;
                        else if (i == 1) return (int)(param.W_FA * 0.5);
                        else if (i == 2) return (int)(param.W_FA * 0.33);
                    }
                }
            }

            return 0; // Nessuna coppia trovata
        }

        public int CalcolaBonusPolarizzazione(int numero, string ruota, DateTime dataTarget)
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

        public int CalcolaBonusRitardo(int numero, string ruota, DateTime dataTarget)
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

        public Dictionary<int, int> CalcolaRitardiFigure(string ruota, DateTime dataTarget)
        {
            var estrRuota = estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)  // ⚠️ NOTA: < dataTarget (NON <=)
                .OrderBy(e => e.Data)
                .ToList();

            if (estrRuota.Count == 0)
                return Enumerable.Range(1, 9).ToDictionary(f => f, f => 1000);

            var datesEstrazioni = estrRuota.Select(e => e.Data).Distinct().OrderBy(d => d).ToList();
            var dataToIndex = datesEstrazioni.Select((d, i) => new { d, i }).ToDictionary(x => x.d, x => x.i);

            // ⚠️ NOTA: idxTarget è l'INDICE dell'ultima estrazione PRIMA di dataTarget
            DateTime ultimaDataPrimaTarget = datesEstrazioni.Last();
            int idxTarget = dataToIndex[ultimaDataPrimaTarget];

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
                    ritardi[f] = idxTarget + 1;  // Mai uscita
                else
                    ritardi[f] = idxTarget - dataToIndex[ultimaUscita[f]];
            }

            return ritardi;
        }

        public int CalcolaBonusArmonia(int numero, string ruota, DateTime dataTarget)
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

        public int CalcolaBonusDifferenzaRitardi(int numero, string ruota, DateTime dataTarget)
        {
            int B_diff = 0;
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int fNum = Figura(numero);
            int afNum = Antifigura(numero);

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

        public int CalcolaBonusAutoAttrazione9(int numero, string ruota, DateTime dataTarget)
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

        public int CalcolaBonusInterRuota(int numero, string ruota, DateTime dataTarget)
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
                        if (CoppiaForti(ruota, altraRuota, true))
                            B_ir += 15;
                        else
                            B_ir += 10;
                        break;
                    }
                }
            }
            return B_ir;
        }

        public int CalcolaBonusTemporale(int numero, string ruota, DateTime dataTarget)
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

        public int CalcolaBonusSequenza(int numero, string ruota, DateTime dataTarget)
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

        // ========== METODI DI SUPPORTO ==========

        public List<Estrazione> GetUltimeEstrazioni(string ruota, DateTime dataTarget, int numeroEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numeroEstrazioni)
                .ToList();
        }

        public int CalcolaBonusFisicaCompleto(int numero, string ruota, DateTime dataTarget)
        {
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);

            var estrattiRecenti = GetUltimiEstratti(ruota, dataTarget, 3);

            // 1. GRAVITÀ (attrazione tra numeri)
            double forzaGravitazionale = CalcolaForzaGravitazionaleTotale(numero, ritardi, ruota, dataTarget);

            // 2. ENERGIA POTENZIALE (pressione del ritardo)
            double energia = CalcolaEnergiaPotenziale(numero, ritardi);

            // 3.ONDA (risonanza temporale e interferenza)
            var onda = new OndaNumerica();
            double ampiezzaOnda = onda.Ampiezza(numero, dataTarget);
            double interferenza = onda.InterferenzaCostruttiva(numero, dataTarget, estrattiRecenti);

            // 4. ENTROPIA (tendenza all'ordine)
            double entropiaPrecedente = CalcolaEntropiaUltimaCinquina(ruota, dataTarget);
            double bonusEntropia = entropiaPrecedente > 1.2 ? 15 : 0;

            // COMBINAZIONE PONDERATA
            //double bonusFisica =
            //    (forzaGravitazionale * 0.3) +      // 30% gravità
            //    (energia * 0.4) +                  // 40% energia  
            //    (interferenza * 20) +              // 20% interferenza
            //    (bonusEntropia * 0.1) +            // 10% entropia
            //    (ampiezzaOnda * 5);                // bonus base onda

            // MIA FISICA (parametri diversi):
            //double bonusFisica =
            //    (forzaGravitazionale * 0.1) +      // 10% gravità (non 30%)
            //    (energia * 0.2) +                  // 20% energia (non 40%)  
            //    (interferenza * 10) +              // 10% interferenza (non 20%)
            //    (bonusEntropia * 0.05) +           // 5% entropia (non 10%)
            //    (ampiezzaOnda * 2);                // bonus base onda (non 5)

            double bonusFisica =
                (forzaGravitazionale * 0.2) +      // 20% gravità
                (energia * 0.3) +                  // 30% energia  
                (interferenza * 15) +              // 15% interferenza
                (bonusEntropia * 0.08) +           // 8% entropia
                (ampiezzaOnda * 3);                // bonus base onda

            return (int)Math.Round(Math.Min(50, bonusFisica)); // Limite 50

            //return (int)Math.Round(Math.Min(60, bonusFisica));
        }

        private List<int> GetUltimiEstratti(string ruota, DateTime dataTarget, int numeroEstrazioni)
        {
            return estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(numeroEstrazioni)
                .SelectMany(e => e.Numeri)
                .ToList();
        }

        private double ForzaGravitazionale(int num1, int num2, Dictionary<int, int> ritardi)
        {
            int figura1 = Figura(num1);
            int figura2 = Figura(num2);

            int ritardo1 = ritardi.ContainsKey(figura1) ? ritardi[figura1] : 0;
            int ritardo2 = ritardi.ContainsKey(figura2) ? ritardi[figura2] : 0;

            double distanza = Math.Abs(num1 - num2);
            if (distanza == 0) distanza = 0.5;

            double G = 50.0; // OTTIMIZZATO
            return G * (ritardo1 * ritardo2) / (distanza * distanza);
        }

        public double CalcolaForzaGravitazionaleTotale(int numero, Dictionary<int, int> ritardi, string ruota, DateTime dataTarget)
        {
            double forzaTotale = 0;
            //var estrattiRecenti = estrazioni
            //    .Where(e => e.Ruota == ruota && e.Data < dataTarget)
            //    .OrderByDescending(e => e.Data)
            //    .Take(2)
            //    .SelectMany(e => e.Numeri)
            //    .ToList();

            var estrattiRecenti = GetUltimiEstratti(ruota, dataTarget, 2);

            foreach (int altroNum in estrattiRecenti)
            {
                forzaTotale += ForzaGravitazionale(numero, altroNum, ritardi);
            }

            return forzaTotale;
        }

        protected double CalcolaEntropiaUltimaCinquina(string ruota, DateTime dataTarget)
        {
            var ultimaCinquina = estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Select(e => e.Numeri)
                .FirstOrDefault();

            if (ultimaCinquina == null || ultimaCinquina.Count != 5) return 0;

            var figure = ultimaCinquina.Select(Figura).ToList();
            var distribuzione = figure.GroupBy(f => f).ToDictionary(g => g.Key, g => g.Count());

            double entropia = 0;
            foreach (var count in distribuzione.Values)
            {
                double p = count / 5.0;
                entropia -= p * Math.Log(p);
            }
            return entropia;
        }

        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="ritardo"></param>
        /// <returns></returns>
        public double EnergiaPotenziale(int numero, int ritardo)
        {
            // Energia cresce esponenzialmente con il ritardo
            return Math.Pow(1.5, ritardo / 10.0);
        }

        /// <summary>
        /// (1 + ln(1 + ritardo/T)): crescita logaritmica iniziale
        /// exp(ritardo/τ) : crescita esponenziale per ritardi molto lunghi
        /// Combina i vantaggi di entrambi i comportamenti
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="ritardi"></param>
        /// <returns></returns>
        public double CalcolaEnergiaPotenziale(int numero, Dictionary<int, int> ritardi)
        {
            int figura = Figura(numero);
            int ritardo = ritardi.ContainsKey(figura) ? ritardi[figura] : 0;

            // Formula: E = E0 × (1 + ln(1 + ritardo/T)) × exp(ritardo/τ)
            // Dove:
            // E0 = energia base
            // T = tempo caratteristico (estrazioni)
            // τ = costante decadimento

            double E0 = 10.0;  // Energia base
            double T = 5.0;    // Scala logaritmica
            double τ = 25.0;   // Decadimento esponenziale

            if (ritardo == 0) return 0;

            double energia = E0 *
                            (1 + Math.Log(1 + ritardo / T)) *
                            Math.Exp(ritardo / τ);

            return energia;
        }

        protected bool CompletaSequenza(int numero, List<int> estrattiPrecedenti)
        {
            if (estrattiPrecedenti.Count < 3) return false;

            // Prendi gli ultimi 3 numeri estratti sulla stessa ruota
            var ultimi3 = estrattiPrecedenti.Take(3).OrderBy(x => x).ToList();

            // Verifica se il numero completa una progressione aritmetica
            for (int i = 0; i < ultimi3.Count - 1; i++)
            {
                for (int j = i + 1; j < ultimi3.Count; j++)
                {
                    int diff = ultimi3[j] - ultimi3[i];

                    // Progressione in avanti: numero = ultimo + diff
                    if (numero == ultimi3.Last() + diff)
                        return true;

                    // Progressione all'indietro: numero = primo - diff
                    if (numero == ultimi3.First() - diff)
                        return true;

                    // Progressione interna: numero completa una sequenza tra i esistenti
                    if (diff > 1)
                    {
                        for (int k = ultimi3[i] + diff; k < ultimi3[j]; k += diff)
                        {
                            if (numero == k)
                                return true;
                        }
                    }
                }
            }

            // Verifica sequenze di figure
            var figureUltimi3 = ultimi3.Select(Figura).ToList();
            int figuraNumero = Figura(numero);

            // Se le figure degli ultimi 3 formano una progressione, verifica se il numero la completa
            if (figureUltimi3.Count == 3)
            {
                int diffFig1 = figureUltimi3[1] - figureUltimi3[0];
                int diffFig2 = figureUltimi3[2] - figureUltimi3[1];

                if (diffFig1 == diffFig2 && diffFig1 != 0)
                {
                    // Progressione di figure: verifica se il numero completa
                    if (figuraNumero == figureUltimi3.Last() + diffFig1)
                        return true;
                    if (figuraNumero == figureUltimi3.First() - diffFig1)
                        return true;
                }
            }

            return false;
        }

        protected bool CoppiaForti(string ruotaA, string ruotaB, bool onlyStrong)
        {
            // Coppie con vantaggio > 1.3x dai nostri test
            var coppieForti = new HashSet<Tuple<string, string>>
            {
                Tuple.Create("Napoli", "Palermo"),
                Tuple.Create("Palermo", "Napoli"),
                Tuple.Create("Milano", "Torino"),
                Tuple.Create("Torino", "Milano"),
                Tuple.Create("Roma", "Napoli"),
                Tuple.Create("Napoli", "Roma"),
                Tuple.Create("Bari", "Napoli"),
                Tuple.Create("Napoli", "Bari"),
                // queto ha un vantaggio di 1.24x secondo i test
                Tuple.Create("Firenze", "Roma"),
                Tuple.Create("Roma", "Firenze")
            };

            if (onlyStrong)
            {
                coppieForti = new HashSet<Tuple<string, string>>
            {
                Tuple.Create("Napoli", "Palermo"),
                Tuple.Create("Palermo", "Napoli"),
                Tuple.Create("Milano", "Torino"),
                Tuple.Create("Torino", "Milano"),
                Tuple.Create("Roma", "Napoli"),
                Tuple.Create("Napoli", "Roma"),
                Tuple.Create("Bari", "Napoli"),
                Tuple.Create("Napoli", "Bari"),
            };
            }

            return coppieForti.Contains(Tuple.Create(ruotaA, ruotaB));
        }

        public Dictionary<int, Tuple<int, List<string>>> GetPunteggiCompleti(string ruota, DateTime dataTarget)
        {
            var scores = new Dictionary<int, Tuple<int, List<string>>>();
            for (int num = 1; num <= 90; num++)
            {
                //scores[num] = CalcolaBonus(num, ruota, dataTarget);
                scores[num] = CalcolaBonusConRegole(num, ruota, dataTarget);
            }
            return scores;
        }


        // to do
        // Dopo aver calcolato tutti i bonus, usiamo lo stato quantistico come "tie-breaker"
        //public List<int> PrevisioneConQuantistica(string ruota, DateTime dataTarget)
        //{
        //    var scores = CalcolaBonus(ruota, dataTarget);
        //    var candidati = scores.Where(x => x.Punteggio >= 45).ToList(); // Soglia più bassa

        //    // Creo stato quantistico con i candidati
        //    var stato = new StatoQuantistico();
        //    foreach (var cand in candidati)
        //    {
        //        stato.Ampiezze[cand.Numero] = cand.Punteggio / 100.0;
        //    }

        //    // Collasso seleziona i migliori
        //    var selezionati = new List<int>();
        //    for (int i = 0; i < Math.Min(5, candidati.Count); i++)
        //    {
        //        int numero = stato.Collassa();
        //        selezionati.Add(numero);
        //        stato.Ampiezze.Remove(numero); // Rimuovi per prossimo collasso
        //    }

        //    return selezionati;
        //}

        //public List<int> Previsione(string ruota, DateTime dataTarget)
        //{
        //    var scores = new List<Tuple<int, int>>();
        //    for (int num = 1; num <= 90; num++)
        //    {
        //        int bonus = CalcolaBonus(num, ruota, dataTarget);
        //        scores.Add(Tuple.Create(num, bonus));
        //    }

        //    return scores
        //        .Where(x => x.Item2 >= param.Soglia)
        //        //.Where(x => x.Item2 >= soglia)
        //        .Select(x => x.Item1)
        //        .OrderBy(x => x)
        //        .ToList();
        //}

    }

    public class OndaNumerica
    {
        public double Ampiezza(int numero, DateTime data)
        {
            // Implementazione più sofisticata con multiple frequenze
            double faseBase = FunzioneOnda(numero, data); // Riutilizza versione originale

            // Aggiungi armoniche superiori
            double secondaArmonica = Math.Sin(2 * (data.DayOfYear / 365.0) * 2 * Math.PI * ModelloLotto.Figura(numero));
            double terzaArmonica = Math.Sin(3 * (data.DayOfYear / 365.0) * 2 * Math.PI * (90 - numero) / 90.0);

            return (faseBase + 0.5 * secondaArmonica + 0.2 * terzaArmonica) / 1.7;
        }

        public double FunzioneOnda(int numero, DateTime data)
        {
            // Fase dipende da data e figura
            double fase = (data.DayOfYear / 365.0) * 2 * Math.PI * ModelloLotto.Figura(numero);
            return Math.Sin(fase);
        }

        public double InterferenzaCostruttiva(int numero, DateTime data, List<int> numeriRecenti)
        {
            // Calcola quanto il numero "interferisce costruttivamente" con gli ultimi estratti
            double interferenzaTotale = 0;

            foreach (int altroNum in numeriRecenti)
            {
                double diffFase = Math.Abs(Ampiezza(numero, data) - Ampiezza(altroNum, data));
                // Interferenza costruttiva quando le fasi sono simili
                interferenzaTotale += 1.0 / (1.0 + diffFase * 10.0);
            }

            return interferenzaTotale / Math.Max(1, numeriRecenti.Count);
        }
    }

}
