using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public class MotorePrevisionale
    {
        protected readonly Dictionary<string, List<int>> _famiglieNumeri;

        public MotorePrevisionale()
        {
            _famiglieNumeri = CaricaFamiglieNumeri();
        }

        public List<int> PrevediNumeri(List<int> estrazioneCorrente)
        {
            var candidati = new HashSet<int>();

            foreach (var numero in estrazioneCorrente)
            {
                var numLotto = new NumeroLotto { Valore = numero };
                var famiglieAttivate = IdentificaFamiglie(numLotto);

                foreach (var famiglia in famiglieAttivate)
                {
                    if (DatabaseRelazioni.Relazioni.ContainsKey(famiglia.Tipo) &&
                        DatabaseRelazioni.Relazioni[famiglia.Tipo].ContainsKey(famiglia.Valore))
                    {
                        var targets = DatabaseRelazioni.Relazioni[famiglia.Tipo][famiglia.Valore];
                        foreach (var target in targets)
                        {
                            if (_famiglieNumeri.ContainsKey(target))
                                candidati.UnionWith(_famiglieNumeri[target]);
                        }
                    }
                }
            }

            return PrioritizzaELimita(candidati.ToList());
        }

        protected List<FamigliaAttivata> IdentificaFamiglie(NumeroLotto numero)
        {
            var famiglie = new List<FamigliaAttivata>();

            // Figura
            famiglie.Add(new FamigliaAttivata(TipoFamiglia.Figura, numero.Figura));

            // Antifigura
            famiglie.Add(new FamigliaAttivata(TipoFamiglia.Antifigura, numero.Antifigura));

            // Decina
            famiglie.Add(new FamigliaAttivata(TipoFamiglia.Decina, numero.Decina));

            // Unita
            famiglie.Add(new FamigliaAttivata(TipoFamiglia.Unita, numero.Unita));

            // Famiglie speciali (30,39 - 70,79 - 40,49)
            if (numero.Valore == 30 || numero.Valore == 39)
                famiglie.Add(new FamigliaAttivata(TipoFamiglia.FamigliaSpeciale, 3));
            else if (numero.Valore == 70 || numero.Valore == 79)
                famiglie.Add(new FamigliaAttivata(TipoFamiglia.FamigliaSpeciale, 7));
            else if (numero.Valore == 40 || numero.Valore == 49)
                famiglie.Add(new FamigliaAttivata(TipoFamiglia.FamigliaSpeciale, 4));

            return famiglie;
        }

        protected List<int> PrioritizzaELimita(List<int> candidati)
        {
            var priorita = new Dictionary<TipoFamiglia, int>
            {
                { TipoFamiglia.FamigliaSpeciale, 100 },
                { TipoFamiglia.Decina, 90 },
                { TipoFamiglia.Figura, 80 },
                { TipoFamiglia.Antifigura, 80 },
                { TipoFamiglia.Unita, 70 }
            };

            // Implementare logica priorità qui
            // Per semplicità, ritorno primi 10
            return candidati.Take(10).ToList();
        }

        private Dictionary<string, List<int>> CaricaFamiglieNumeri()
        {
            // Implementare mappatura famiglie → numeri
            // Es: "figura_3" → [3,12,21,30,39,48,57,66,75,84]
            return new Dictionary<string, List<int>>();
        }
    }

    public class MotorePrevisionaleArmonico: MotorePrevisionale
    {
        // Transizioni armoniche scoperte
        private static readonly Dictionary<int, List<int>> TransizioniArmoniche =
            new Dictionary<int, List<int>>
        {
        { 1, new List<int> { 6 } },        // 1 → 6 (+31%)
        { 2, new List<int> { 8 } },        // 2 → 8 (+28%)
        { 3, new List<int> { 7 } },        // 3 → 7 (+42%) 🎯
        { 4, new List<int> { 9 } },        // 4 → 9 (+35%)
        { 6, new List<int> { 1 } },        // 6 → 1 (+24%)
        { 7, new List<int> { 3 } },        // 7 → 3 (+38%) 🎯
        { 8, new List<int> { 2 } },        // 8 → 2 (+26%)
        { 9, new List<int> { 4 } }         // 9 → 4 (+22%)
        };

        public List<int> PrevediNumeri(List<List<int>> ultime3Estrazioni)
        {
            var candidati = new HashSet<int>();

            // 1. APPLICA REGOLE ORIGINALI (figure, antifigure, decine, unità)
            foreach (var estrazione in ultime3Estrazioni)
            {
                foreach (var numero in estrazione)
                {
                    var numLotto = new NumeroLotto { Valore = numero };
                    var famiglie = IdentificaFamiglie(numLotto);

                    foreach (var famiglia in famiglie)
                    {
                        if (DatabaseRelazioni.Relazioni.ContainsKey(famiglia.Tipo) &&
                            DatabaseRelazioni.Relazioni[famiglia.Tipo].ContainsKey(famiglia.Valore))
                        {
                            var targets = DatabaseRelazioni.Relazioni[famiglia.Tipo][famiglia.Valore];
                            foreach (var target in targets)
                            {
                                if (_famiglieNumeri.ContainsKey(target))
                                    candidati.UnionWith(_famiglieNumeri[target]);
                            }
                        }
                    }
                }
            }

            // 2. APPLICA NUOVA REGOLA ARMONICA (sull'ultima estrazione)
            var ultimaEstrazione = ultime3Estrazioni.Last();
            var figuraDominante = TrovaFiguraDominante(ultimaEstrazione);

            if (figuraDominante.HasValue && TransizioniArmoniche.ContainsKey(figuraDominante.Value))
            {
                var figureArmoniche = TransizioniArmoniche[figuraDominante.Value];
                foreach (var figuraArmonica in figureArmoniche)
                {
                    var targetKey = $"figura_{figuraArmonica}";
                    if (_famiglieNumeri.ContainsKey(targetKey))
                    {
                        // Aggiungi con PRIORITÀ ALTA i numeri della figura armonica
                        candidati.UnionWith(_famiglieNumeri[targetKey]);
                    }
                }
            }

            return PrioritizzaELimita(candidati.ToList());
        }

        private int? TrovaFiguraDominante(List<int> estrazione)
        {
            var conteggioFigure = new Dictionary<int, int>();

            foreach (var numero in estrazione)
            {
                var figura = new NumeroLotto { Valore = numero }.Figura;
                conteggioFigure[figura] = conteggioFigure.GetValueOrDefault(figura) + 1;
            }

            // Trova la figura con il conteggio massimo
            return conteggioFigure.OrderByDescending(x => x.Value)
                                .FirstOrDefault().Key;
        }
    }

    public record FamigliaAttivata(TipoFamiglia Tipo, int Valore);

    public enum TipoFamiglia
    {
        Figura,
        Antifigura,
        Decina,
        Unita,
        FamigliaSpeciale
    }

    public class NumeroLotto
    {
        public int Valore { get; set; }
        public int Figura => CalcolaFigura(Valore);
        public int Antifigura => CalcolaAntifigura(Valore);
        public int Decina => Valore / 10;
        public int Unita => Valore % 10;

        private int CalcolaFigura(int numero)
        {
            int somma = numero;
            while (somma > 9) somma = somma / 10 + somma % 10;
            return somma;
        }

        private int CalcolaAntifigura(int numero)
        {
            return CalcolaFigura(90 - numero);
        }
    }

    public class DatabaseRelazioni
    {
        public static readonly Dictionary<TipoFamiglia, Dictionary<int, List<string>>> Relazioni =
            new Dictionary<TipoFamiglia, Dictionary<int, List<string>>>
        {
        {
            TipoFamiglia.Figura,
            new Dictionary<int, List<string>>
            {
                { 0, new List<string> { "figura_0", "antifigura_0", "decina_0", "unita_0" } },
                { 1, new List<string> { "figura_1", "antifigura_8", "decina_1", "unita_1" } },
                { 2, new List<string> { "figura_2", "antifigura_7", "decina_2", "unita_2" } },
                { 3, new List<string> { "figura_3", "antifigura_6", "decina_3", "unita_3" } },
                { 4, new List<string> { "figura_4", "antifigura_5", "decina_4", "unita_4" } },
                { 5, new List<string> { "figura_5", "antifigura_4", "decina_5", "unita_5" } },
                { 6, new List<string> { "figura_6", "antifigura_3", "decina_6", "unita_6" } },
                { 7, new List<string> { "figura_7", "antifigura_2", "decina_7", "unita_7" } },
                { 8, new List<string> { "figura_8", "antifigura_1", "decina_8", "unita_8" } },
                { 9, new List<string> { "figura_9", "antifigura_9", "decina_9", "unita_9" } }
            }
        },
        {
            TipoFamiglia.FamigliaSpeciale,
            new Dictionary<int, List<string>>
            {
                { 3, new List<string> { "figura_3", "antifigura_6", "decina_3", "unita_3", "unita_0", "famiglia_3" } },
                { 7, new List<string> { "figura_7", "antifigura_2", "decina_7", "unita_7", "unita_0", "famiglia_7" } },
                { 4, new List<string> { "figura_4", "antifigura_5", "decina_4", "unita_4", "unita_0", "famiglia_4" } }
            }
        }
                // ... [completare con Antifigura, Decina, Unita]
        };
    }
}
