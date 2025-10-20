using Crs.Home.ApocalypsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crs.Home.ApocalypsData
{
    public class ModelloQuantisticoCompleto : ModelloQuantistico
    {
        private Dictionary<string, List<int>> _famiglieNumeri;
        private List<(IOperatoreArmonico operatore, double peso)> _operatoriConPesi; // ✅ CORRETTA DICHIARAZIONE
        public ModelloQuantisticoCompleto()
        {
            _famiglieNumeri = CaricaFamiglieNumeri();
            _operatoriConPesi = new List<(IOperatoreArmonico, double)> // ✅ INIZIALIZZAZIONE CORRETTA
                                        {
                                            (new OperatoreCluster(), 0.3),
                                            (new OperatoreTransizioniArmoniche(), 0.4),
                                            (new OperatoreRiequilibrio(), 0.2),
                                            (new OperatoreMultiLivello(), 0.1)
                                        };
        }

        public List<int> PrevediNumeri(List<List<int>> ultime3Estrazioni)
        {
            // 🌊 1. CALCOLA ONDA PASSAO (dalle ultime 3 estrazioni)
            var ondaPassato = CalcolaOndaPassato(ultime3Estrazioni);

            // 🔮 2. CALCOLA ONDA FUTURO IPOTETICA (trasformazione armonica)
            var ondaFuturo = CalcolaOndaFuturoIpotetica(ondaPassato);

            // 💥 3. INTERFERENZA QUANTISTICA
            var ondaInterferenza = InterferenzaCostruttiva(ondaPassato, ondaFuturo);

            // 🎯 4. COLLASSO DELLA FUNZIONE D'ONDA
            return CollassoPerPicco(ondaInterferenza, 10);
        }


        //private OndaArmonica CalcolaOndaPassato(List<List<int>> estrazioni)
        //{
        //    var onda = new OndaArmonica();

        //    // Pesi basati sulla nostra analisi statistica
        //    var operatori = new List<(IOperatoreArmonico op, double peso)>
        //{
        //    (new OperatoreCluster(), 0.30),
        //    (new OperatoreTransizioniArmoniche(), 0.40),
        //    (new OperatoreRiequilibrio(), 0.20),
        //    (new OperatoreMultiLivello(), 0.10)
        //};

        //    foreach (var (op, peso) in operatori)
        //    {
        //        var energiaOp = new OndaArmonica();
        //        op.Applica(energiaOp, estrazioni);

        //        // Somma pesata
        //        for (int i = 0; i < 90; i++)
        //        {
        //            onda.Energia[i] += energiaOp.Energia[i] * peso;
        //        }
        //    }

        //    onda.Normalizza();
        //    return onda;
        //}

        private OndaArmonica CalcolaOndaPassato(List<List<int>> estrazioni)
        {
            var onda = new OndaArmonica();

            foreach (var (op, peso) in _operatoriConPesi)
            {
                var operatoreClone = op.Clone();
                var energiaOp = new OndaArmonica();

                // ✅ CORRETTO: Aggiunto il terzo parametro _famiglieNumeri
                operatoreClone.Applica(energiaOp, estrazioni, _famiglieNumeri);

                for (int i = 0; i < 90; i++)
                {
                    onda.Energia[i] += energiaOp.Energia[i] * peso;
                }
            }

            onda.Normalizza();
            return onda;
        }

        private OndaArmonica CalcolaOndaFuturoIpotetica_(OndaArmonica ondaPassato)
        {
            var ondaFuturo = new OndaArmonica();

            // 🎵 1. SPECCHIATURA ARMONICA (68% - pattern più forte)
            var picchiPassato = TrovaPicchi(ondaPassato, soglia: 0.5);
            foreach (var picco in picchiPassato)
            {
                var figura = LottoMath.CalcolaFigura(picco.Numero);
                var antifigura = 10 - figura;

                var numeriAntifigura = _famiglieNumeri[$"figura_{antifigura}"];
                foreach (var num in numeriAntifigura)
                {
                    ondaFuturo.Energia[num - 1] += picco.Energia * 0.68;
                }
            }

            // 🔄 2. COMPLEMENTARIETÀ DECIMALE (72%)
            foreach (var picco in picchiPassato)
            {
                var unita = picco.Numero % 10;

                // Tutti i numeri con stessa unità
                for (int num = unita; num <= 90; num += 10)
                {
                    if (num >= 1 && num <= 90)
                    {
                        ondaFuturo.Energia[num - 1] += picco.Energia * 0.72;
                    }
                }
            }

            // 🏛️ 3. NUMERI ARMONICI UNIVERSALI
            var numeriArmonici = new[] { 30, 70, 49, 81, 22, 55, 66, 88, 11, 44, 90 };
            foreach (var num in numeriArmonici)
            {
                ondaFuturo.Energia[num - 1] += 0.3;
            }

            // 🌊 4. RINFORZO TRANSIZIONI FORTI
            foreach (var picco in picchiPassato)
            {
                var figura = LottoMath.CalcolaFigura(picco.Numero);

                // Transizioni forti scoperte: 3↔7, 4↔9, etc.
                var transizioni = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 6 } }, { 2, new List<int> { 8 } },
                { 3, new List<int> { 7 } }, { 4, new List<int> { 9 } },
                { 6, new List<int> { 1 } }, { 7, new List<int> { 3 } },
                { 8, new List<int> { 2 } }, { 9, new List<int> { 4 } }
            };

                if (transizioni.ContainsKey(figura))
                {
                    foreach (var figuraTarget in transizioni[figura])
                    {
                        var numeriTransizione = _famiglieNumeri[$"figura_{figuraTarget}"];
                        foreach (var num in numeriTransizione.Take(3)) // Solo 3 più probabili
                        {
                            ondaFuturo.Energia[num - 1] += picco.Energia * 0.5;
                        }
                    }
                }
            }

            ondaFuturo.Normalizza();
            return ondaFuturo;
        }

        private OndaArmonica CalcolaOndaFuturoIpotetica(OndaArmonica ondaPassato)
        {
            var ondaFuturo = new OndaArmonica();

            // 🎵 1. SPECCHIATURA ARMONICA (68% - pattern più forte)
            var picchiPassato = TrovaPicchi(ondaPassato, soglia: 0.5);
            foreach (var picco in picchiPassato)
            {
                var figura = LottoMath.CalcolaFigura(picco.Numero); // ✅ Usa LottoMath
                var antifigura = 10 - figura;

                var key = $"figura_{antifigura}";
                if (_famiglieNumeri.ContainsKey(key)) // ✅ Controlla esistenza
                {
                    var numeriAntifigura = _famiglieNumeri[key];
                    foreach (var num in numeriAntifigura)
                    {
                        ondaFuturo.Energia[num - 1] += picco.Energia * 0.68;
                    }
                }
            }

            // 🔄 2. COMPLEMENTARIETÀ DECIMALE (72%)
            foreach (var picco in picchiPassato)
            {
                var unita = picco.Numero % 10;

                // Tutti i numeri con stessa unità
                for (int num = unita; num <= 90; num += 10)
                {
                    if (num >= 1 && num <= 90)
                    {
                        ondaFuturo.Energia[num - 1] += picco.Energia * 0.72;
                    }
                }
            }

            // 🏛️ 3. NUMERI ARMONICI UNIVERSALI
            var numeriArmonici = new[] { 30, 70, 49, 81, 22, 55, 66, 88, 11, 44, 90 };
            foreach (var num in numeriArmonici)
            {
                ondaFuturo.Energia[num - 1] += 0.3;
            }

            // 🌊 4. RINFORZO TRANSIZIONI FORTI
            foreach (var picco in picchiPassato)
            {
                var figura = LottoMath.CalcolaFigura(picco.Numero); // ✅ Usa LottoMath

                // Transizioni forti scoperte: 3↔7, 4↔9, etc.
                var transizioni = new Dictionary<int, List<int>>
                {
                    { 1, new List<int> { 6 } }, { 2, new List<int> { 8 } },
                    { 3, new List<int> { 7 } }, { 4, new List<int> { 9 } },
                    { 6, new List<int> { 1 } }, { 7, new List<int> { 3 } },
                    { 8, new List<int> { 2 } }, { 9, new List<int> { 4 } }
                };

                if (transizioni.ContainsKey(figura))
                {
                    foreach (var figuraTarget in transizioni[figura])
                    {
                        var key = $"figura_{figuraTarget}";
                        if (_famiglieNumeri.ContainsKey(key)) // ✅ Controlla esistenza
                        {
                            var numeriTransizione = _famiglieNumeri[key];
                            foreach (var num in numeriTransizione.Take(3))
                            {
                                ondaFuturo.Energia[num - 1] += picco.Energia * 0.5;
                            }
                        }
                    }
                }
            }

            ondaFuturo.Normalizza();
            return ondaFuturo;
        }

        private OndaArmonica InterferenzaCostruttiva(OndaArmonica onda1, OndaArmonica onda2)
        {
            var interferenza = new OndaArmonica();

            // 💥 PRODOTTO delle energie (interferenza costruttiva)
            for (int i = 0; i < 90; i++)
            {
                interferenza.Energia[i] = onda1.Energia[i] * onda2.Energia[i];
            }

            interferenza.Normalizza();
            return interferenza;
        }

        private List<int> CollassoPerPicco(OndaArmonica onda, int quantiNumeri)
        {
            var numeriOrdinati = onda.Energia
                .Select((energia, index) => new { Numero = index + 1, Energia = energia })
                .OrderByDescending(x => x.Energia)
                .Select(x => x.Numero)
                .Take(quantiNumeri)
                .ToList();

            return numeriOrdinati;
        }

        private List<PiccoEnergia> TrovaPicchi(OndaArmonica onda, double soglia)
        {
            var picchi = new List<PiccoEnergia>();
            for (int i = 0; i < 90; i++)
            {
                if (onda.Energia[i] >= soglia)
                {
                    picchi.Add(new PiccoEnergia { Numero = i + 1, Energia = onda.Energia[i] });
                }
            }
            return picchi.OrderByDescending(p => p.Energia).ToList();
        }


    }

    public class OperatoreCluster : IOperatoreArmonico
    {
       
        public void Applica(OndaArmonica onda, List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri)
        {
            var numeriCluster = TrovaNumeriInCluster(estrazioni, famiglieNumeri);
            foreach (var num in numeriCluster)
            {
                onda.Energia[num - 1] += 1.0;
            }
        }

        private List<int> TrovaNumeriInCluster(List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri)
        {
            var candidati = new HashSet<int>();

            // Cluster per figura
            var figureInCluster = TrovaFigureInCluster(estrazioni);
            foreach (var figura in figureInCluster)
            {
                var key = $"figura_{figura}";
                if (famiglieNumeri.ContainsKey(key))
                    candidati.UnionWith(famiglieNumeri[key]);
            }

            // Cluster per decina
            var decineInCluster = TrovaDecineInCluster(estrazioni);
            foreach (var decina in decineInCluster)
            {
                var key = $"decina_{decina}";
                if (famiglieNumeri.ContainsKey(key))
                    candidati.UnionWith(famiglieNumeri[key]);
            }

            return candidati.ToList();
        }

        private List<int> TrovaFigureInCluster(List<List<int>> estrazioni)
        {
            var figureCount = new Dictionary<int, int>();

            foreach (var estrazione in estrazioni)
            {
                foreach (var numero in estrazione)
                {
                    var figura = LottoMath.CalcolaFigura(numero);
                    figureCount[figura] = figureCount.GetValueOrDefault(figura) + 1;
                }
            }

            return figureCount.Where(kv => kv.Value >= 2).Select(kv => kv.Key).ToList();
        }

        private List<int> TrovaDecineInCluster(List<List<int>> estrazioni)
        {
            var decineCount = new Dictionary<int, int>();

            foreach (var estrazione in estrazioni)
            {
                foreach (var numero in estrazione)
                {
                    var decina = numero / 10;
                    decineCount[decina] = decineCount.GetValueOrDefault(decina) + 1;
                }
            }

            return decineCount.Where(kv => kv.Value >= 2).Select(kv => kv.Key).ToList();
        }

        public IOperatoreArmonico Clone() => new OperatoreCluster();
    }



    public class OperatoreTransizioniArmoniche : IOperatoreArmonico
    {
        public void Applica(OndaArmonica onda, List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri)
        {
            var ultimaEstrazione = estrazioni.Last();
            var figureDominanti = TrovaFigureDominanti(ultimaEstrazione);

            foreach (var figura in figureDominanti)
            {
                var numeriTransizione = GetNumeriTransizioneArmonica(figura, famiglieNumeri);
                foreach (var num in numeriTransizione)
                {
                    onda.Energia[num - 1] += 0.8;
                }
            }
        }

        private List<int> TrovaFigureDominanti(List<int> estrazione)
        {
            var figureCount = new Dictionary<int, int>();

            foreach (var numero in estrazione)
            {
                var figura = LottoMath.CalcolaFigura(numero);
                figureCount[figura] = figureCount.GetValueOrDefault(figura) + 1;
            }

            return figureCount.Where(kv => kv.Value >= 2).Select(kv => kv.Key).ToList();
        }

        private List<int> GetNumeriTransizioneArmonica(int figura, Dictionary<string, List<int>> famiglieNumeri)
        {
            var transizioni = new Dictionary<int, List<int>>
        {
            { 1, new List<int> { 6 } }, { 2, new List<int> { 8 } },
            { 3, new List<int> { 7 } }, { 4, new List<int> { 9 } },
            { 6, new List<int> { 1 } }, { 7, new List<int> { 3 } },
            { 8, new List<int> { 2 } }, { 9, new List<int> { 4 } }
        };

            var candidati = new List<int>();
            if (transizioni.ContainsKey(figura))
            {
                foreach (var figuraTarget in transizioni[figura])
                {
                    var key = $"figura_{figuraTarget}";
                    if (famiglieNumeri.ContainsKey(key))
                    {
                        var numeriFigura = famiglieNumeri[key];
                        candidati.AddRange(numeriFigura.Take(3));
                    }
                }
            }

            return candidati;
        }

        public IOperatoreArmonico Clone() => new OperatoreTransizioniArmoniche();
    }

    public class OperatoreRiequilibrio : IOperatoreArmonico
    {
        public void Applica(OndaArmonica onda, List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri)
        {
            var figureInRiequilibrio = TrovaFigureInRiequilibrio(estrazioni);
            foreach (var figura in figureInRiequilibrio)
            {
                var numeriRiequilibrio = GetNumeriRiequilibrio(figura, famiglieNumeri);
                foreach (var num in numeriRiequilibrio)
                {
                    onda.Energia[num - 1] += 0.6;
                }
            }
        }

        private List<int> TrovaFigureInRiequilibrio(List<List<int>> estrazioni)
        {
            var figureInRiequilibrio = new List<int>();
            var ultimeFigure = new HashSet<int>();

            foreach (var estrazione in estrazioni)
            {
                foreach (var numero in estrazione)
                {
                    ultimeFigure.Add(LottoMath.CalcolaFigura(numero));
                }
            }

            for (int figura = 1; figura <= 9; figura++)
            {
                if (!ultimeFigure.Contains(figura))
                {
                    figureInRiequilibrio.Add(figura);
                }
            }

            return figureInRiequilibrio;
        }

        private List<int> GetNumeriRiequilibrio(int figura, Dictionary<string, List<int>> famiglieNumeri)
        {
            var key = $"figura_{figura}";
            if (!famiglieNumeri.ContainsKey(key))
                return new List<int>();

            var numeriFigura = famiglieNumeri[key];
            return numeriFigura
                .Where(n => (n % 10) <= 3)
                .Take(3)
                .ToList();
        }

        public IOperatoreArmonico Clone() => new OperatoreRiequilibrio();
    }


    // 📦 CLASSI DI SUPPORTO
    public class OndaArmonica
    {
        public double[] Energia { get; set; }

        public OndaArmonica()
        {
            Energia = new double[90];
            for (int i = 0; i < 90; i++)
                Energia[i] = 0.0;
        }

        public void Normalizza()
        {
            var max = Energia.Max();
            if (max > 0)
            {
                for (int i = 0; i < 90; i++)
                    Energia[i] /= max;
            }
        }
    }

    public struct PiccoEnergia
    {
        public int Numero { get; set; }
        public double Energia { get; set; }
    }

    public class OperatoreMultiLivello : IOperatoreArmonico
    {
        public void Applica(OndaArmonica onda, List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri)
        {
            var numeriArmonici = GetNumeriArmoniciMultiLivello();
            foreach (var num in numeriArmonici)
            {
                onda.Energia[num - 1] += 0.4;
            }
        }

        private List<int> GetNumeriArmoniciMultiLivello()
        {
            return new List<int>
        {
            30, 70, 49, 81, 22, 55, 66, 88, 11, 44, 90
        };
        }

        public IOperatoreArmonico Clone() => new OperatoreMultiLivello();
    }

    public interface IOperatoreArmonico
    {
        void Applica(OndaArmonica onda, List<List<int>> estrazioni, Dictionary<string, List<int>> famiglieNumeri);
        IOperatoreArmonico Clone();
    }

    public static class LottoMath
    {
        public static int CalcolaFigura(int numero)
        {
            if (numero < 1 || numero > 90)
                return 0;

            int somma = numero;
            while (somma > 9)
            {
                somma = somma / 10 + somma % 10;
            }
            return somma;
        }
    }

}


