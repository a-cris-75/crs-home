
using Crs.Home.ApocalypsData.DataEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    //public class Estrazione
    //{
    //    public DateTime Data { get; set; }
    //    public string Ruota { get; set; }
    //    public List<int> Numeri { get; set; }

    //    public Estrazione(DateTime data, string ruota, List<int> numeri)
    //    {
    //        Data = data;
    //        Ruota = ruota;
    //        Numeri = numeri;
    //    }
    //}

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
        //public int W_auto_att9 { get; set; } = 1;
        //public int W_inter_ruota { get; set; } = 1;
        //public int W_Temporale { get; set; } = 1;
        //public int W_Sequenza { get; set; } = 1;
        //public int W_Fisica { get; set; } = 1;

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

    public class ModelloLotto
    {
        protected ParametriModello param;
        private List<Estrazione> estrazioni;
        private HashSet<Tuple<int, int>> coppieFA;
        private string[] ruote = {
            "Bari", "Cagliari", "Firenze", "Genova", "Milano",
            "Napoli", "Palermo", "Roma", "Torino", "Venezia", "Nazionale"
        };

        public ModelloLotto(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
        {
            this.estrazioni = estrazioniStoriche;
            this.param = parametri ?? new ParametriModello();
            //this.performanceTracker = new PerformanceTracker();
            //this.estrazioniDaUltimaCalibrazione = 0;
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

        private int Antifigura(int numero)
        {
            int comp = 90 - numero;
            return Figura(comp);
        }

        private int Decina(int numero)
        {
            if (numero == 90) return 90;
            return (numero / 10) * 10;
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

        // Modifica CalcolaBonus per restituire anche regole attivate
        //public Tuple<int, List<string>> CalcolaBonusConTracking(int numero, string ruota, DateTime dataTarget)
        //{
        //    var regoleAttivate = new List<string>();
        //    int bonus = 0;

        //    // ... calcolo bonus esistente ...
        //    // DOPO ogni bonus, aggiungi la regola alla lista se attiva

        //    var estrRuota = estrazioni
        //        .Where(e => e.Ruota == ruota && e.Data < dataTarget)
        //        .OrderByDescending(e => e.Data)
        //        .Take(3)
        //        .ToList();

        //    int fNum = Figura(numero);
        //    int afNum = Antifigura(numero);
        //    int dNum = Decina(numero);

        //    // Calcolo dei bonus individuali
        //    int B_dec = 0, B_FA = 0, B_pol = 0, B_rit = 0, B_arm = 0, B_diff = 0, B_9 = 0;

        //    // 1. Bonus decina
        //    var decineUltime2 = new HashSet<int>();
        //    foreach (var e in estrRuota.Take(2))
        //    {
        //        foreach (int n in e.Numeri)
        //        {
        //            decineUltime2.Add(Decina(n));
        //        }
        //    }
            

        //    if (decineUltime2.Contains(dNum))
        //    {
        //        if (estrRuota.Count > 0 && estrRuota[0].Numeri.Any(n => Decina(n) == dNum))
        //            B_dec += param.W_dec1;
        //        if (estrRuota.Count > 1 && estrRuota[1].Numeri.Any(n => Decina(n) == dNum))
        //            B_dec += param.W_dec2;

        //        regoleAttivate.Add("Bonus decina");
        //    }

        //    // 2. Bonus figura-antifigura (solo 1-8 e 4-5) - ESCLUDI figura 9
        //    if (fNum != 9)
        //    {
        //        for (int i = 0; i < estrRuota.Count; i++)
        //        {
        //            foreach (int n in estrRuota[i].Numeri)
        //            {
        //                int fN = Figura(n);
        //                if (fN == 9) continue;
        //                var coppia = Tuple.Create(fN, fNum);
        //                if (coppieFA.Contains(coppia))
        //                {
        //                    if (i == 0) B_FA += param.W_FA;
        //                    else if (i == 1) B_FA += (int)(param.W_FA * 0.5);
        //                    else if (i == 2) B_FA += (int)(param.W_FA * 0.33);
        //                }
        //            }
        //        }
        //        if(B_FA>0)
        //            regoleAttivate.Add("Bonus figura-antifigura");
        //    }

        //    // 3. Bonus polarizzazione
        //    if (estrRuota.Count > 0)
        //    {
        //        var ultimaCinquina = estrRuota[0].Numeri;
        //        var figureUltima = new HashSet<int>(ultimaCinquina.Select(Figura));
        //        int varieta = figureUltima.Count;

        //        if (varieta == 5)
        //        {
        //            var figureUltime3 = new HashSet<int>();
        //            foreach (var e in estrRuota)
        //            {
        //                foreach (int n in e.Numeri)
        //                {
        //                    figureUltime3.Add(Figura(n));
        //                }
        //            }
        //            if (figureUltime3.Contains(fNum))
        //                B_pol += param.W_pol;
        //        }
        //        else if (varieta <= 3)
        //        {
        //            if (!figureUltima.Contains(fNum))
        //                B_pol += param.W_pol;
        //        }

        //        regoleAttivate.Add("Bonus polarizzazione");
        //    }

        //    // 4. Bonus ritardo
        //    var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
        //    int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;
        //    if (ritardoF >= 25) B_rit += param.W_rit25;
        //    else if (ritardoF >= 20) B_rit += param.W_rit20;
        //    else if (ritardoF >= 15) B_rit += param.W_rit15;

        //    if(B_rit>0) 
        //        regoleAttivate.Add("Bonus ritardo");

        //    // 5. Bonus armonia
        //    var figureUltime4 = new List<int>();
        //    foreach (var e in estrRuota)
        //    {
        //        figureUltime4.AddRange(e.Numeri.Select(Figura));
        //    }
        //    foreach (int fAltra in figureUltime4)
        //    {
        //        if (fAltra + fNum == 9 || fAltra + fNum == 10)
        //        {
        //            B_arm += param.W_arm;
        //            break;
        //        }
        //    }
        //    if (B_arm > 0)
        //        regoleAttivate.Add("Bonus armonia");

        //    // 6. Bonus differenza ritardi
        //    if (coppieFA.Contains(Tuple.Create(fNum, afNum)))
        //    {
        //        int ritardoAF = ritardi.ContainsKey(afNum) ? ritardi[afNum] : 1000;
        //        int diff = Math.Abs(ritardoF - ritardoAF);
        //        if (diff > param.Soglia_diff)
        //        {
        //            B_diff += (diff - param.Soglia_diff) * param.W_diff;
        //            regoleAttivate.Add("Bonus differenza ritardi");
        //        }         
        //    }

        //    // 7. Bonus auto-attrazione figura 9
        //    if (fNum == 9)
        //    {
        //        for (int i = 0; i < estrRuota.Count; i++)
        //        {
        //            if (estrRuota[i].Numeri.Any(n => Figura(n) == 9))
        //            {
        //                if (i == 0) B_9 += 20;
        //                else if (i == 1) B_9 += 10;
        //                else if (i == 2) B_9 += 5;
        //                break;
        //            }
        //        }
        //    }

        //    // 8. Bonus inter-ruota
        //    int B_ir = 0;
        //    foreach (string altraRuota in ruote)
        //    {
        //        if (altraRuota == ruota) continue;

        //        // Cerca se il numero è uscito su altraRuota nelle ultime 3 estrazioni
        //        var estrAltraRuota = estrazioni
        //            .Where(e => e.Ruota == altraRuota && e.Data < dataTarget)
        //            .OrderByDescending(e => e.Data)
        //            .Take(3)
        //            .ToList();

        //        foreach (var e in estrAltraRuota)
        //        {
        //            if (e.Numeri.Contains(numero))
        //            {
        //                // Assegna bonus in base alla forza della coppia
        //                if (CoppiaForti(ruota, altraRuota, true))
        //                    B_ir += 15;
        //                else
        //                    B_ir += 10;
        //                break;
        //            }
        //        }
        //    }

        //    // 9. Bonus pattern temporali
        //    int B_temp = 0;

        //    DayOfWeek giornoTarget = dataTarget.DayOfWeek;
        //    string ruotaTarget = ruota;

        //    // Mappa dei pattern significativi (ruota, giorno, figura) → bonus
        //    var patternTemporali = new Dictionary<Tuple<string, DayOfWeek, int>, int>
        //    {
        //        { Tuple.Create("Roma", DayOfWeek.Tuesday, 8), 12 },
        //        { Tuple.Create("Napoli", DayOfWeek.Friday, 3), 12 },
        //        { Tuple.Create("Palermo", DayOfWeek.Saturday, 9), 12 },
        //        { Tuple.Create("Milano", DayOfWeek.Monday, 5), 10 },
        //        { Tuple.Create("Bari", DayOfWeek.Thursday, 1), 8 }
        //    };

        //    var key = Tuple.Create(ruotaTarget, giornoTarget, fNum);
        //    if (patternTemporali.ContainsKey(key))
        //    {
        //        B_temp += patternTemporali[key];
        //    }

        //    // 10. Bonus completamento sequenza
        //    int B_seq = 0;
        //    var estrattiRecenti = estrRuota.SelectMany(e => e.Numeri).ToList();
        //    if (CompletaSequenza(numero, estrattiRecenti))
        //    {
        //        B_seq += 15;
        //    }

        //    // 11. Bonus Fisica (Gravità + Entropia)
        //    int B_fisica = CalcolaBonusFisica(numero, ruota, dataTarget);

        //    double bonus_moltiplicativo =
        //        (B_dec / 10.0 + 1) *
        //        (B_FA / 10.0 + 1) *
        //        (B_pol / 10.0 + 1) *
        //        (B_rit / 10.0 + 1) *
        //        (B_arm / 10.0 + 1) *
        //        (B_diff / 10.0 + 1) *
        //        (B_9 / 10.0 + 1) *
        //        (B_ir / 10.0 + 1) *
        //        (B_temp / 10.0 + 1) *
        //        (B_seq / 10.0 + 1) *
        //        (B_fisica / 10.0 + 1) *
        //        10;

        //    return Tuple.Create(bonus, regoleAttivate);
        //}


        public int CalcolaBonus(int numero, string ruota, DateTime dataTarget, List<string>? regoleAttivate = null)
        {
            int bonus = 0;
            var estrRuota = estrazioni
                .Where(e => e.Ruota == ruota && e.Data < dataTarget)
                .OrderByDescending(e => e.Data)
                .Take(3)
                .ToList();

            int fNum = Figura(numero);
            int afNum = Antifigura(numero);
            int dNum = Decina(numero);

            // Calcolo dei bonus individuali
            int B_dec = 0, B_FA = 0, B_pol = 0, B_rit = 0, B_arm = 0, B_diff = 0, B_9 = 0;

            // 1. Bonus decina
            var decineUltime2 = new HashSet<int>();
            foreach (var e in estrRuota.Take(2))
            {
                foreach (int n in e.Numeri)
                {
                    decineUltime2.Add(Decina(n));
                }
            }

            if (decineUltime2.Contains(dNum))
            {
                if (estrRuota.Count > 0 && estrRuota[0].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += param.W_dec1;
                if (estrRuota.Count > 1 && estrRuota[1].Numeri.Any(n => Decina(n) == dNum))
                    B_dec += param.W_dec2;
            }

            // 2. Bonus figura-antifigura (solo 1-8 e 4-5) - ESCLUDI figura 9
            if (fNum != 9)
            {
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
            }

            // 3. Bonus polarizzazione
            if (estrRuota.Count > 0)
            {
                var ultimaCinquina = estrRuota[0].Numeri;
                var figureUltima = new HashSet<int>(ultimaCinquina.Select(Figura));
                int varieta = figureUltima.Count;

                if (varieta == 5)
                {
                    var figureUltime3 = new HashSet<int>();
                    foreach (var e in estrRuota)
                    {
                        foreach (int n in e.Numeri)
                        {
                            figureUltime3.Add(Figura(n));
                        }
                    }
                    if (figureUltime3.Contains(fNum))
                        B_pol += param.W_pol;
                }
                else if (varieta <= 3)
                {
                    if (!figureUltima.Contains(fNum))
                        B_pol += param.W_pol;
                }
            }

            // 4. Bonus ritardo
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            int ritardoF = ritardi.ContainsKey(fNum) ? ritardi[fNum] : 1000;
            if (ritardoF >= 25) B_rit += param.W_rit25;
            else if (ritardoF >= 20) B_rit += param.W_rit20;
            else if (ritardoF >= 15) B_rit += param.W_rit15;

            // 5. Bonus armonia
            var figureUltime4 = new List<int>();
            foreach (var e in estrRuota)
            {
                figureUltime4.AddRange(e.Numeri.Select(Figura));
            }
            foreach (int fAltra in figureUltime4)
            {
                if (fAltra + fNum == 9 || fAltra + fNum == 10)
                {
                    B_arm += param.W_arm;
                    break;
                }
            }

            // 6. Bonus differenza ritardi
            if (coppieFA.Contains(Tuple.Create(fNum, afNum)))
            {
                int ritardoAF = ritardi.ContainsKey(afNum) ? ritardi[afNum] : 1000;
                int diff = Math.Abs(ritardoF - ritardoAF);
                if (diff > param.Soglia_diff)
                {
                    B_diff += (diff - param.Soglia_diff) * param.W_diff;
                }
            }

            // 7. Bonus auto-attrazione figura 9
            if (fNum == 9)
            {
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
            }

            // 8. Bonus inter-ruota
            int B_ir = 0;
            foreach (string altraRuota in ruote)
            {
                if (altraRuota == ruota) continue;

                // Cerca se il numero è uscito su altraRuota nelle ultime 3 estrazioni
                var estrAltraRuota = estrazioni
                    .Where(e => e.Ruota == altraRuota && e.Data < dataTarget)
                    .OrderByDescending(e => e.Data)
                    .Take(3)
                    .ToList();

                foreach (var e in estrAltraRuota)
                {
                    if (e.Numeri.Contains(numero))
                    {
                        // Assegna bonus in base alla forza della coppia
                        if (CoppiaForti(ruota, altraRuota, true))
                            B_ir += 15;
                        else
                            B_ir += 10;
                        break;
                    }
                }
            }

            // 9. Bonus pattern temporali
            int B_temp = 0;

            DayOfWeek giornoTarget = dataTarget.DayOfWeek;
            string ruotaTarget = ruota;

            // Mappa dei pattern significativi (ruota, giorno, figura) → bonus
            var patternTemporali = new Dictionary<Tuple<string, DayOfWeek, int>, int>
            {
                { Tuple.Create("Roma", DayOfWeek.Tuesday, 8), 12 },
                { Tuple.Create("Napoli", DayOfWeek.Friday, 3), 12 },
                { Tuple.Create("Palermo", DayOfWeek.Saturday, 9), 12 },
                { Tuple.Create("Milano", DayOfWeek.Monday, 5), 10 },
                { Tuple.Create("Bari", DayOfWeek.Thursday, 1), 8 }
            };

            var key = Tuple.Create(ruotaTarget, giornoTarget, fNum);
            if (patternTemporali.ContainsKey(key))
            {
                B_temp += patternTemporali[key];
            }

            // 10. Bonus completamento sequenza
            int B_seq = 0;
            var estrattiRecenti = estrRuota.SelectMany(e => e.Numeri).ToList();
            if (CompletaSequenza(numero, estrattiRecenti))
            {
                B_seq += 15;
            }

            // 11. Bonus Fisica (Gravità + Entropia)
            //int B_fisica = CalcolaBonusFisica(numero, ruota, dataTarget);
            int B_fisica = CalcolaBonusFisicaCompleto(numero, ruota, dataTarget);


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
                (B_fisica / 10.0 + 1) *
                10;

            //return Tuple.Create(bonus, regoleAttivate);
            return (int)Math.Round(bonus_moltiplicativo);
        }

        public Tuple<int, List<string>> CalcolaBonusConRegole(int numero, string ruota, DateTime dataTarget)
        {
            var regoleAttivate = new List<string>();
            int B_dec = 0, B_FA = 0, B_pol = 0, B_rit = 0, B_arm = 0, B_diff = 0, B_9 = 0, B_ir = 0, B_temp = 0, B_seq = 0;

            // [CODICE ESISTENTE - ma ora registra quando attiva una regola]

            // 1. Bonus decina
            if (B_dec > 0) regoleAttivate.Add("Decina");

            // 2. Bonus figura-antifigura
            if (B_FA > 0) regoleAttivate.Add("FiguraAntifigura");

            // 3. Bonus polarizzazione
            if (B_pol > 0) regoleAttivate.Add("Polarizzazione");

            // 4. Bonus ritardo
            if (B_rit > 0) regoleAttivate.Add("Ritardo");

            // 5. Bonus armonia
            if (B_arm > 0) regoleAttivate.Add("Armonia");

            // 6. Bonus differenza ritardi
            if (B_diff > 0) regoleAttivate.Add("DifferenzaRitardi");

            // 7. Bonus auto-attrazione figura 9
            if (B_9 > 0) regoleAttivate.Add("AutoAttrazione9");

            // 8. Bonus inter-ruota
            if (B_ir > 0) regoleAttivate.Add("InterRuota");

            // 9. Bonus temporale
            if (B_temp > 0) regoleAttivate.Add("Temporale");

            // 10. Bonus sequenza
            if (B_seq > 0) regoleAttivate.Add("Sequenza");

            // Calcolo moltiplicativo
            double bonus_moltiplicativo =
                (B_dec / 10.0 + 1) * (B_FA / 10.0 + 1) * (B_pol / 10.0 + 1) *
                (B_rit / 10.0 + 1) * (B_arm / 10.0 + 1) * (B_diff / 10.0 + 1) *
                (B_9 / 10.0 + 1) * (B_ir / 10.0 + 1) * (B_temp / 10.0 + 1) *
                (B_seq / 10.0 + 1) * 10;

            return Tuple.Create((int)Math.Round(bonus_moltiplicativo), regoleAttivate);
        }

        private int CalcolaBonusFisica_deprecated(int numero, string ruota, DateTime dataTarget)
        {
            // 1. Componente gravitazionale
            var ritardi = CalcolaRitardiFigure(ruota, dataTarget);
            double forzaTotale = CalcolaForzaGravitazionaleTotale(numero, ritardi, ruota, dataTarget);

            // 2. Componente entropica - PARAMETRI OTTIMIZZATI
            double entropiaPrecedente = CalcolaEntropiaUltimaCinquina(ruota, dataTarget);
            double bonusEntropia = entropiaPrecedente > 0.8 ? 20 : 0; // Soglia 0.8, Peso 20

            // 3. Combinazione
            double bonusFisica = (forzaTotale / 5.0) + bonusEntropia;

            return Math.Max(0, (int)Math.Round(bonusFisica));
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
            double bonusFisica =
                (forzaGravitazionale * 0.3) +      // 30% gravità
                (energia * 0.4) +                  // 40% energia  
                (interferenza * 20) +              // 20% interferenza
                (bonusEntropia * 0.1) +            // 10% entropia
                (ampiezzaOnda * 5);                // bonus base onda

            return (int)Math.Round(Math.Min(60, bonusFisica));
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

        private double CalcolaForzaGravitazionaleTotale(int numero, Dictionary<int, int> ritardi, string ruota, DateTime dataTarget)
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

        private double CalcolaEntropiaUltimaCinquina(string ruota, DateTime dataTarget)
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

        private bool CompletaSequenza(int numero, List<int> estrattiPrecedenti)
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
        
        private bool CoppiaForti(string ruotaA, string ruotaB, bool onlyStrong)
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
        
        public List<int> Previsione(string ruota, DateTime dataTarget)
        {
            var scores = new List<Tuple<int, int>>();
            for (int num = 1; num <= 90; num++)
            {
                int bonus = CalcolaBonus(num, ruota, dataTarget);
                scores.Add(Tuple.Create(num, bonus));
            }

            return scores
                .Where(x => x.Item2 >= param.Soglia)
                .Select(x => x.Item1)
                .OrderBy(x => x)
                .ToList();
        }
        
        public Dictionary<int, int> GetPunteggiCompleti(string ruota, DateTime dataTarget)
        {
            var scores = new Dictionary<int, int>();
            for (int num = 1; num <= 90; num++)
            {
                scores[num] = CalcolaBonus(num, ruota, dataTarget);
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

    }


    public class ModelloAdattivo2 : ModelloLotto
    {
        private Dictionary<string, RegolaPerformance> performanceRegole;
        private int estrazioniDaUltimoAggiornamento = 0;
        private const int INTERVALLO_AGGIORNAMENTO = 30;
        private Queue<Tuple<string, DateTime, string, int, bool>> codaPerformance;

        public ModelloAdattivo2(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
            : base(estrazioniStoriche, parametri)
        {
            InizializzaPerformanceRegole();
            codaPerformance = new Queue<Tuple<string, DateTime, string, int, bool>>();
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
            { "Sequenza", new RegolaPerformance { NomeRegola = "Sequenza", UltimoReset = DateTime.Now } }
        };
        }

        public List<int> PrevisioneConTracking(string ruota, DateTime dataTarget, List<int> numeriUsciti = null)
        {
            var scores = new List<Tuple<int, int, List<string>>>();

            for (int num = 1; num <= 90; num++)
            {
                var result = CalcolaBonusConRegole(num, ruota, dataTarget);
                //var result = CalcolaBonus(num, ruota, dataTarget);
                scores.Add(Tuple.Create(num, result.Item1, result.Item2));
            }

            var consigliati = scores.Where(x => x.Item2 >= param.Soglia).Select(x => x.Item1).ToList();

            // Registra performance dopo l'estrazione (se abbiamo i risultati reali)
            if (numeriUsciti != null)
            {
                RegistraPerformance(ruota, dataTarget, scores, numeriUsciti);
            }

            return consigliati.OrderBy(x => x).ToList();
        }

        private void RegistraPerformance(string ruota, DateTime data, List<Tuple<int, int, List<string>>> scores, List<int> numeriUsciti)
        {
            foreach (var score in scores)
            {
                int numero = score.Item1;
                bool uscito = numeriUsciti.Contains(numero);
                var regoleAttivate = score.Item3;

                foreach (string regola in regoleAttivate)
                {
                    if (performanceRegole.ContainsKey(regola))
                    {
                        var perf = performanceRegole[regola];
                        if (uscito)
                            perf.AttivazioniSuccesso++;
                        else
                            perf.AttivazioniFallite++;
                    }
                }
            }

            estrazioniDaUltimoAggiornamento++;
            if (estrazioniDaUltimoAggiornamento >= INTERVALLO_AGGIORNAMENTO)
            {
                AggiornaPesi();
                estrazioniDaUltimoAggiornamento = 0;
            }
        }

        private void AggiornaPesi()
        {
            Console.WriteLine("=== AGGIORNAMENTO PESI MODELLO ===");

            double vantaggioMedio = performanceRegole.Values
                .Where(p => (p.AttivazioniSuccesso + p.AttivazioniFallite) > 10)
                .Average(p => p.VantaggioCorrente);

            foreach (var kvp in performanceRegole)
            {
                var perf = kvp.Value;
                int totaleAttivazioni = perf.AttivazioniSuccesso + perf.AttivazioniFallite;

                if (totaleAttivazioni < 5) continue; // Troppo pochi dati

                double rapporto = perf.VantaggioCorrente / vantaggioMedio;
                rapporto = Math.Max(0.5, Math.Min(2.0, rapporto)); // Limita oscillazioni

                double nuovoPeso = OttieniPesoCorrente(kvp.Key) * rapporto;
                AggiornaParametro(kvp.Key, nuovoPeso);

                Console.WriteLine($"{kvp.Key}: Vantaggio {perf.VantaggioCorrente:F2}x, Peso {nuovoPeso:F1} (x{rapporto:F2})");

                // Reset contatori
                //perf.AttivazioniSuccesso = 0;
                //perf.AttivazioniFallite = 0;
                //perf.UltimoReset = DateTime.Now;
                perf.Reset();
            }
        }

        private double OttieniPesoCorrente(string regola)
        {
            switch (regola)
            {
                case "Decina": return param.W_dec1; // Usiamo W_dec1 come riferimento
                case "FiguraAntifigura": return param.W_FA;
                case "Polarizzazione": return param.W_pol;
                case "Ritardo": return param.W_rit15;
                case "Armonia": return param.W_arm;
                case "DifferenzaRitardi": return param.W_diff;
                case "AutoAttrazione9": return 20; // Peso fisso per B_9
                case "InterRuota": return 15; // Peso fisso per B_ir
                case "Temporale": return 12; // Peso fisso per B_temp
                case "Sequenza": return 15; // Peso fisso per B_seq
                default: return 10;
            }
        }

        private void AggiornaParametro(string regola, double nuovoPeso)
        {
            int pesoInt = (int)Math.Round(nuovoPeso);

            switch (regola)
            {
                case "Decina":
                    param.W_dec1 = Math.Max(5, Math.Min(40, pesoInt));
                    param.W_dec2 = param.W_dec1 / 2;
                    break;
                case "FiguraAntifigura":
                    param.W_FA = Math.Max(10, Math.Min(50, pesoInt));
                    break;
                case "Polarizzazione":
                    param.W_pol = Math.Max(2, Math.Min(15, pesoInt));
                    break;
                case "Ritardo":
                    param.W_rit15 = Math.Max(5, Math.Min(30, pesoInt));
                    param.W_rit20 = (int)(param.W_rit15 * 1.67);
                    param.W_rit25 = (int)(param.W_rit15 * 2.33);
                    break;
                case "Armonia":
                    param.W_arm = Math.Max(2, Math.Min(10, pesoInt));
                    break;
                case "DifferenzaRitardi":
                    param.W_diff = Math.Max(1, Math.Min(5, pesoInt));
                    break;
                    // Per regole con pesi fissi, aggiorniamo i valori fissi
            }
        }


    }


    public class ModelloAdattivo : ModelloLotto
    {
        private PerformanceTracker performanceTracker;
        private int estrazioniDaUltimaCalibrazione;
        private const int INTERVALLO_CALIBRAZIONE = 30;

        public ModelloAdattivo(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null)
            : base(estrazioniStoriche, parametri)
        {
            this.performanceTracker = new PerformanceTracker();
            this.estrazioniDaUltimaCalibrazione = 0;
        }

        // Metodo specifico per adattività
        public List<int> PrevisioneConAdattivita(string ruota, DateTime dataTarget, List<int> numeriUsciti = null)
        {
            var risultati = new List<Tuple<int, int, List<string>>>();

            for (int num = 1; num <= 90; num++)
            {
                List<string> regoleAttivate = new List<string>();
                int bonus = CalcolaBonus(num, ruota, dataTarget, regoleAttivate);
                risultati.Add(Tuple.Create(num, bonus, regoleAttivate));
            }

            var consigliati = risultati.Where(x => x.Item2 >= param.Soglia).Select(x => x.Item1).ToList();

            if (numeriUsciti != null)
            {
                AggiornaPerformance(risultati, numeriUsciti);
            }

            return consigliati.OrderBy(x => x).ToList();
        }

        private void AggiornaPerformance(List<Tuple<int, int, List<string>>> risultati, List<int> numeriUsciti)
        {
            foreach (var risultato in risultati)
            {
                bool uscito = numeriUsciti.Contains(risultato.Item1);
                foreach (string regola in risultato.Item3)
                {
                    performanceTracker.RegistraPerformance(regola, uscito);
                }
            }

            estrazioniDaUltimaCalibrazione++;
            if (estrazioniDaUltimaCalibrazione >= INTERVALLO_CALIBRAZIONE)
            {
                CalibraPesi();
                estrazioniDaUltimaCalibrazione = 0;
            }
        }

        private void CalibraPesi()
        {
            var aggiornamenti = performanceTracker.CalcolaAggiornamentiPesi();

            foreach (var agg in aggiornamenti)
            {
                double nuovoPeso = OttieniPesoCorrente(agg.Key) * agg.Value;
                ApplicaAggiornamento(agg.Key, nuovoPeso);
            }

            Console.WriteLine("=== PESI AGGIORNATI ===");
            foreach (var agg in aggiornamenti)
            {
                Console.WriteLine($"{agg.Key}: x{agg.Value:F2}");
            }
        }

        // METODI MANCANTI - AGGIUNGI QUESTI:
        private double OttieniPesoCorrente(string regola)
        {
            switch (regola)
            {
                case "Decina": return param.W_dec1;
                case "FiguraAntifigura": return param.W_FA;
                case "Polarizzazione": return param.W_pol;
                case "Ritardo": return param.W_rit15;
                case "Armonia": return param.W_arm;
                case "DifferenzaRitardi": return param.W_diff;
                case "AutoAttrazione9": return 20;
                case "InterRuota": return 15;
                case "Temporale": return 12;
                case "Sequenza": return 15;
                case "Fisica": return 15;
                default: return 10;
            }
        }

        private void ApplicaAggiornamento(string regola, double nuovoPeso)
        {
            int pesoInt = (int)Math.Round(nuovoPeso);

            switch (regola)
            {
                case "Decina":
                    param.W_dec1 = Math.Max(5, Math.Min(40, pesoInt));
                    param.W_dec2 = param.W_dec1 / 2;
                    break;
                case "FiguraAntifigura":
                    param.W_FA = Math.Max(10, Math.Min(50, pesoInt));
                    break;
                case "Polarizzazione":
                    param.W_pol = Math.Max(2, Math.Min(15, pesoInt));
                    break;
                case "Ritardo":
                    param.W_rit15 = Math.Max(5, Math.Min(30, pesoInt));
                    param.W_rit20 = (int)(param.W_rit15 * 1.67);
                    param.W_rit25 = (int)(param.W_rit15 * 2.33);
                    break;
                // ... altri casi ...
                case "Armonia":
                    param.W_arm = Math.Max(2, Math.Min(10, pesoInt));
                    break;
                case "DifferenzaRitardi":
                    param.W_diff = Math.Max(1, Math.Min(5, pesoInt));
                    break;
                    // Per regole con pesi fissi, aggiorniamo i valori fissi
            }
        }
    }


    public class StatoQuantistico
    {
        public Dictionary<int, double> Ampiezze { get; set; } // numero -> ampiezza probabilità
        public int Collassa() => Ampiezze.OrderByDescending(x => x.Value).First().Key;

        // NUOVO METODO: evoluzione temporale dello stato
        public void Evolvi(DateTime nuovoTempo)
        {
            foreach (var num in Ampiezze.Keys.ToList())
            {
                double nuovaAmpiezza = new OndaNumerica().Ampiezza(num, nuovoTempo);
                Ampiezze[num] = (Ampiezze[num] + nuovaAmpiezza) / 2.0; // Media con evoluzione
            }
        }
    }

    public class OndaNumerica
    {

        public double Ampiezza(int numero, DateTime data)
        {
            // Implementazione più sofisticata con multiple frequenze
            double faseBase = FunzioneOnda(numero, data); // Riutilizza versione originale

            // Aggiungi armoniche superiori
            double secondaArmonica = Math.Sin(2 * (data.DayOfYear / 365.0) * 2 * Math.PI * ModelloLotto. Figura(numero));
            double terzaArmonica = Math.Sin(3 * (data.DayOfYear / 365.0) * 2 * Math.PI * (90 - numero) / 90.0);

            return (faseBase + 0.5 * secondaArmonica + 0.2 * terzaArmonica) / 1.7;
        }

        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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

    // Caricatore CSV (invariato)
    public static class CaricatoreEstrazioni
    {
        public static List<Estrazione> CaricaDaCSV(string filePath)
        {
            var estrazioni = new List<Estrazione>();
            var culture = CultureInfo.GetCultureInfo("it-IT");

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(';');
                    if (parts.Length < 3) continue;

                    if (DateTime.TryParseExact(parts[0], "dd/MM/yyyy", culture, DateTimeStyles.None, out DateTime data))
                    {
                        string ruota = parts[1];
                        var numeri = parts[2].Split(',').Select(int.Parse).ToList();
                        estrazioni.Add(new Estrazione(data, ruota, numeri));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore caricamento CSV: {ex.Message}");
            }

            return estrazioni;
        }
    }

    public class RegolaPerformance
    {
        public string NomeRegola { get; set; }
        public int AttivazioniSuccesso { get; set; }
        public int AttivazioniFallite { get; set; }
        public double VantaggioCorrente
        {
            get
            {
                double totale = AttivazioniSuccesso + AttivazioniFallite;
                if (totale == 0) return 1.0;
                double successRate = AttivazioniSuccesso / totale;
                return successRate / 0.0556; // Vantaggio vs caso casuale
            }
        }
        public DateTime UltimoReset { get; set; }
        public int TotaleAttivazioni { get { return AttivazioniSuccesso + AttivazioniFallite; } }

        public void Reset() 
        {
            AttivazioniSuccesso = 0;
            AttivazioniFallite = 0;
            
            UltimoReset = DateTime.Now;
            //TotaleAttivazioni = 0;
        }
    }


    //public class ModelloAdattivo : ModelloLotto
    //{
    //    private Dictionary<string, RegolaPerformance> performanceRegole;
    //    private int estrazioniDaUltimoAggiornamento = 0;
    //    private const int INTERVALLO_AGGIORNAMENTO = 30; // ogni 30 estrazioni

    //    public ModelloAdattivo(List<Estrazione> estrazioniStoriche, ParametriModello parametri = null) : base(estrazioniStoriche, parametri)
    //    {
    //    }

    //    public void RegistraPerformance(string ruota, DateTime data, int numero, bool uscito, List<string> regoleAttivate)
    //    {
    //        foreach (string regola in regoleAttivate)
    //        {
    //            var perf = performanceRegole[regola];
    //            if (uscito)
    //                perf.AttivazioniSuccesso++;
    //            else
    //                perf.AttivazioniFallite++;
    //        }

    //        estrazioniDaUltimoAggiornamento++;
    //        if (estrazioniDaUltimoAggiornamento >= INTERVALLO_AGGIORNAMENTO)
    //        {
    //            AggiornaPesi();
    //            estrazioniDaUltimoAggiornamento = 0;
    //        }
    //    }

    //    private void AggiornaPesi()
    //    {
    //        foreach (var perf in performanceRegole.Values)
    //        {
    //            double nuovoPeso = CalcolaNuovoPeso(perf.VantaggioCorrente);
    //            AggiornaParametro(perf.NomeRegola, nuovoPeso);
    //        }
    //    }

    //    private double CalcolaNuovoPeso(double vantaggioRegola)
    //    {
    //        double vantaggioMedio = performanceRegole.Values.Average(p => p.VantaggioCorrente);
    //        double rapporto = vantaggioRegola / vantaggioMedio;

    //        // Limita l'aggiustamento tra 0.7x e 1.3x per stabilità
    //        rapporto = Math.Max(0.7, Math.Min(1.3, rapporto));

    //        return parametriCorrenti * rapporto;
    //    }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            var estrazioni = CaricatoreEstrazioni.CaricaDaCSV("estrazioni.csv");

            if (estrazioni.Count == 0)
            {
                Console.WriteLine("Nessuna estrazione caricata. Verifica il file.");
                return;
            }

            var modello = new ModelloLotto(estrazioni);
            DateTime dataPrevisione = new DateTime(2009, 6, 13);

            foreach (string ruota in new[] { "Roma", "Bari", "Napoli" }) // Test su 3 ruote
            {
                var consigli = modello.Previsione(ruota, dataPrevisione);
                var punteggi = modello.GetPunteggiCompleti(ruota, dataPrevisione);

                Console.WriteLine($"{ruota}: {consigli.Count} numeri");
                Console.WriteLine($"  Numeri: {string.Join(", ", consigli)}");
                Console.WriteLine($"  Punteggi: {string.Join(", ", punteggi.Where(x => x.Value >= 40).OrderByDescending(x => x.Value).Select(x => $"{x.Key}({x.Value})"))}");
                Console.WriteLine();
            }

            var modelloAdattivo = new ModelloAdattivo(estrazioni);
            dataPrevisione = new DateTime(2009, 6, 13);

            foreach (string ruota in new[] { "Roma", "Bari", "Napoli" }) // Test su 3 ruote
            {
                var risultatiReali = new List<int> { 7, 38, 39, 65, 90 };
                var consigli = modelloAdattivo.PrevisioneConAdattivita(ruota, dataPrevisione, risultatiReali);
                var punteggi = modelloAdattivo.GetPunteggiCompleti(ruota, dataPrevisione);

                Console.WriteLine($"{ruota}: {consigli.Count} numeri");
                Console.WriteLine($"  Numeri: {string.Join(", ", consigli)}");
                Console.WriteLine($"  Punteggi: {string.Join(", ", punteggi.Where(x => x.Value >= 40).OrderByDescending(x => x.Value).Select(x => $"{x.Key}({x.Value})"))}");
                Console.WriteLine();
            }
        }
    }
}
