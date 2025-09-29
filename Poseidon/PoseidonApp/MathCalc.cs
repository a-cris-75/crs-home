using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoseidonData.DataEntities;
using System.Windows.Forms;

namespace PoseidonApp
{
    public static class MathCalc
    {
        /// <summary>
        /// Date due matrici MxN una risultante dell'altra restituisce per ogni numero della prima, la mappa delle distanze 
        /// di ogni elemento della seconda.
        /// Le distanze sono l'insieme delle due coordinate X e Y interi, che indicano la posizione relativa di ogni punto da quello di partenza.
        /// Per convenzione, per tradurre due numeri in uno solo rappresentante la distanza tra due punti, considero ques'ultima come la somma di X e Y
        /// </summary>
        private static PSD_MATH_STAR_SYSTEM_RES GetStarSystemNum(int num,int posx, int posy, DateTime datanum, string groupnamerow, string groupnamecol, string starsystemname, List<PSD_MATH_MATRIX_NUM> matrix
            , int maxdistCalcSaturazione, int maxdistCalcSaturazione2, int maxdistCalcSaturazione3)
        {
            PSD_MATH_STAR_SYSTEM_RES starsystem = new PSD_MATH_STAR_SYSTEM_RES();

            starsystem.STARNUM = num;
            // posizioni nella matrice M1 delle stelle
            starsystem.POSX = posx;
            starsystem.POSY = posy;
            starsystem.GROUPROW = groupnamerow;
            starsystem.GROUPCOL = groupnamecol;
            starsystem.STAR_SYSTEM_MATRIX_NAME = starsystemname;
            starsystem.DATENUM = datanum;
            starsystem.MAXDIST_TO_CALC_SATURAZIONE = maxdistCalcSaturazione;
            starsystem.MAXDIST_TO_CALC_SATURAZIONE_2 = maxdistCalcSaturazione2;
            starsystem.MAXDIST_TO_CALC_SATURAZIONE_3 = maxdistCalcSaturazione3;

            /*int posingroupcolumn = posx % 5;
            if (posingroupcolumn == 0) posingroupcolumn = 5;
            starsystem.ID = groupnamecol + posingroupcolumn.ToString(); + "-" + p.POSY.ToString().PadLeft(3, '0');
            */
            starsystem.PLANETS = new List<PSD_MATH_PLANET_NUM>();

            int idx = 0;
            foreach (PSD_MATH_PLANET_NUM t in matrix)
            {
                PSD_MATH_PLANET_NUM p = new PSD_MATH_PLANET_NUM();
                int distX = Math.Abs(posx - t.POSX);
                int distY = Math.Abs(posy - t.POSY);
                int distorbit = (distY > distX) ? distY : distX;
                //starsystem.DISTANCES.Add(distX + distY);
                // esprime la distanza dell'orbita: in pratica la massima distanza di una delle due coordinate
                //starsystem.DISTANCES_ORBIT.Add(distorbit);
                p.DATENUM = t.DATENUM;
                p.DISTANCE = distX + distY;
                p.DISTANCE_ORBIT = distorbit;
                p.GROUPCOL = t.GROUPCOL;
                p.GROUPROW = t.GROUPROW;
                p.NUM = t.NUM;
                p.POSITION_ORBIT = t.POSITION_ORBIT;
                p.POSX = t.POSX;
                p.POSY = t.POSY;
                // serve per potermi ricondurre alla stella a cui è associato il pianeta
                p.STAR_NUM = new PSD_MATH_MATRIX_NUM();
                p.STAR_NUM.POSX = posx;
                p.STAR_NUM.POSY = posy;
                p.STAR_NUM.GROUPROW = groupnamerow;
                p.STAR_NUM.GROUPCOL = groupnamecol;
                p.STAR_NUM.STAR_SYSTEM_MATRIX_NAME = starsystemname;
                p.STAR_NUM.DATENUM = datanum;
                p.STAR_NUM.NUM = num;
                int posingroupcolumn = posx % 5;
                if (posingroupcolumn == 0) posingroupcolumn = 5;
                p.STAR_NUM.POSGROUPCOL = groupnamecol + posingroupcolumn.ToString();
                

                posingroupcolumn = t.POSX % 5;
                if (posingroupcolumn == 0) posingroupcolumn = 5;
                p.POSGROUPCOL = t.GROUPCOL + posingroupcolumn.ToString();
                // ID: pos X + grop col + pos in group (es:BA1010 => estratto a Ba in pos 1 alla 10ma riga della matrice)
                p.ID = p.POSGROUPCOL + "-" + p.POSY.ToString().PadLeft(3, '0');

                p.VAL_SAT = (float)1 / (distorbit + 1);

                starsystem.PLANETS.Add(p);
                idx++;
            }

            return starsystem;
        }

        /// <summary>
        /// Data una matrice di partenza che comprende N numeri che rappresetano le stelle (stars), deduco in base alla matrice 2 tutti i sitemi solari.
        /// Quindi alla fine ho N sistemi solari a partite da ogni star num della matrice 1; per ogni starnum ho n pianeti appartenenti alla matrice 2.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
       /* public static List<PSD_MATH_STAR_SYSTEM_GRID> GetStarSystems(List<PSD_MATH_PLANET_NUM> matrix1, List<PSD_MATH_PLANET_NUM> matrix2, string namestarsystemM1)
        {
            List<PSD_MATH_STAR_SYSTEM_GRID> res = new List<PSD_MATH_STAR_SYSTEM_GRID>();
            foreach (PSD_MATH_PLANET_NUM t in matrix1)
            {
                PSD_MATH_STAR_SYSTEM_GRID starsystem = GetStarSystemNum(t.NUM, t.POSX, t.POSY,t.DATANUM, t.GROUPROW,t.GROUPCOL, namestarsystemM1, matrix2);
                res.Add(starsystem);
            }

            return res;
        }*/

        public static List<PSD_MATH_STAR_SYSTEM_RES> GetStarSystems(List<PSD_MATH_MATRIX_NUM> matrix1, List<PSD_MATH_MATRIX_NUM> matrix2, string namestarsystemM1, int maxdistCalcSaturazione, int maxdistCalcSaturazione2, int maxdistCalcSaturazione3)
        {
            List<PSD_MATH_STAR_SYSTEM_RES> res = new List<PSD_MATH_STAR_SYSTEM_RES>();
            foreach (PSD_MATH_MATRIX_NUM t in matrix1)
            {
                PSD_MATH_STAR_SYSTEM_RES starsystem = GetStarSystemNum(t.NUM, t.POSX, t.POSY, t.DATENUM, t.GROUPROW, t.GROUPCOL, namestarsystemM1, matrix2, maxdistCalcSaturazione, maxdistCalcSaturazione2, maxdistCalcSaturazione3);
                res.Add(starsystem);
            }

            return res;
        }

        public static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppie(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, out List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstCoppieEqDist)
        {
            // ALGORITIMO: 
            //  1) determino la minima distanza (mindistorbit1) per cui siano rappresentati tutti i numeri della matrice risultante
            //      - ciò significa che entro mindistorbit1 sono presenti tutti i numeri della matrice risultante
            //      - cioè non esite un numero distante da tutti gli altri più di mindistorbit1: in questo caso non riesco a formare tutte le coppie
            //  2) formo le coppie semplici, formate da un numero della matrice di origine che ne ha solo uno nella matrice risultante sovrapposto
            //  3) per ogni distanza orbitale, formo le coppie con la minima distanza: 
            //      a. dalla distanza più piccola, prendo i numeri non presenti alla stessa distanza orbitale da altri numeri della matrice di origine
            //      b. per i rimanenti passo al punto 4)
            //  4) ...

            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWork;
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res1 = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res2 = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            ;
            res1 = CalcolaCoppieDist0(lstStarSystem, out lstStarSystemWork);
            res2 = CalcolaCoppieMinDist1(lstStarSystemWork, out lstStarSystemWork, out lstCoppieEqDist);
            res1.AddRange(res2);
            return res1;
        }

        public static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppie2(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem)
        {
            // ALGORITIMO: 
            //  1) determino la minima distanza (mindistorbit1) per cui siano rappresentati tutti i numeri della matrice risultante
            //      - ciò significa che entro mindistorbit1 sono presenti tutti i numeri della matrice risultante
            //      - cioè non esite un numero distante da tutti gli altri più di mindistorbit1: in questo caso non riesco a formare tutte le coppie
            //  2) formo le coppie semplici, formate da un numero della matrice di origine che ne ha solo uno nella matrice risultante sovrapposto
            //  3) per ogni distanza orbitale, formo le coppie con la minima distanza: 
            //      a. dalla distanza più piccola, prendo i numeri non presenti alla stessa distanza orbitale da altri numeri della matrice di origine
            //      b. per i rimanenti passo al punto 4)
            //  4) ...

            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWork;
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res1 = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res2 = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            int numcalc = 0;
            res1 = CalcolaCoppieDist0(lstStarSystem, out lstStarSystemWork);
            res2 = CalcolaCoppieMinDist2(lstStarSystemWork, res1, numcalc);
            res1.AddRange(res2);
            return res1;
        }

        private static void CalcolaCatena(List<PSD_MATH_MATRIX_NUM> matrix1, List<PSD_MATH_MATRIX_NUM> matrix2, int maxdistSat)
        {
            List<PSD_MATH_STAR_SYSTEM_RES> starSystem12 = GetStarSystems(matrix1, matrix2, "M1 => M2", maxdistSat,0,0);
            List<PSD_MATH_STAR_SYSTEM_RES> starSystem21 = GetStarSystems(matrix2, matrix1, "M2 => M1", maxdistSat,0,0);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstcoppimindist = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            foreach (PSD_MATH_STAR_SYSTEM_RES s1 in starSystem12)
            {
                //CalcolaCatenaR(s1, starSystem12, starSystem21, ref lstcoppimindist);
                int mindistP = s1.PLANETS.Min(x => x.DISTANCE_ORBIT);
                List<PSD_MATH_PLANET_NUM> lstmindistplanets_fromStar = s1.PLANETS.Where(x => x.DISTANCE_ORBIT == mindistP).ToList();

                // se c'è un solo pianeta vicino alla minima distanza dalla stella, e se lo stesso pianeta non è più vicino ad un'altra stella allora accoppia
                if (lstmindistplanets_fromStar.Count == 1)
                {
                    PSD_MATH_PLANET_NUM p1 = lstmindistplanets_fromStar.First();
                    PSD_MATH_STAR_SYSTEM_RES planettoStar = starSystem21.Where(X => X.STARNUM == p1.NUM && X.POSX == p1.POSX && X.POSY == p1.POSY).First();
                    int mindistS = planettoStar.PLANETS.Min(x => x.DISTANCE_ORBIT);
                    if (mindistS > mindistP)
                    { 
                        PSD_MATH_STAR_SYSTEM_MIN_DIST_RES coppia = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                        coppia.PLANET = p1;
                        coppia.IS_COPPIA_UNIVOCA = true;
                        coppia.STARNUM = SetStarNum(s1);
                        coppia.DISTANCE_ORBIT = p1.DISTANCE_ORBIT;
                        lstcoppimindist.Add(coppia);
                    }
                }
                // se ci sono più pianeti alla min dist, scegli quelli più opportuni => quelli che non hanno altre stelle più vicine
                //  - li metto tutti
                else
                {
                    int idx = 0;
                    foreach (PSD_MATH_PLANET_NUM p1 in lstmindistplanets_fromStar)
                    {
                        // trovo la lista dei pianeti associata al pianeta che è stella in M2=> M1 
                        PSD_MATH_STAR_SYSTEM_RES planettoStar = starSystem21.Where(X => X.STARNUM == p1.NUM && X.POSX == p1.POSX && X.POSY == p1.POSY).First();
                        // elimino dai pianeti la stella iniziale
                        planettoStar.PLANETS = planettoStar.PLANETS.Where(X => X.NUM != s1.STARNUM && X.POSX != s1.POSX && X.POSY != s1.POSY).ToList();
                        // trovo la minima distanza delle stelle da pianeta
                        int mindistS = planettoStar.PLANETS.Min(x => x.DISTANCE_ORBIT);

                        if (mindistS > mindistP)
                        {
                            idx++;
                            PSD_MATH_STAR_SYSTEM_MIN_DIST_RES coppia = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                            coppia.PLANET = p1;
                            coppia.IS_COPPIA_UNIVOCA = false;
                            coppia.STARNUM = SetStarNum(s1);
                            coppia.DISTANCE_ORBIT = p1.DISTANCE_ORBIT;
                            lstcoppimindist.Add(coppia);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// VERSIONE RICORSIVA
        /// Per ogni stella trova i pianeti più vicini.
        /// Ricorsivamante per ogni pianeta più vicino trova le stelle più vicine, escludendo la stella iniziale 
        /// </summary>
        private static void CalcolaCatena2(List<PSD_MATH_MATRIX_NUM> matrix1, List<PSD_MATH_MATRIX_NUM> matrix2, int maxdistSat)
        {
            List<PSD_MATH_STAR_SYSTEM_RES> starSystem12 = GetStarSystems(matrix1, matrix2, "M1 => M2", maxdistSat,0,0);
            List<PSD_MATH_STAR_SYSTEM_RES> starSystem21 = GetStarSystems(matrix2, matrix1, "M2 => M1", maxdistSat,0,0);
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstcoppimindist = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            foreach (PSD_MATH_STAR_SYSTEM_RES s1 in starSystem12)
            {
                int mindistAct = s1.PLANETS.Min(x => x.DISTANCE_ORBIT);
                int mindistNew;
                CalcolaCatenaR(s1, starSystem12, starSystem21, ref lstcoppimindist, mindistAct, out mindistNew);
          
            }
        }

        // TO TEST 
        private static bool CalcolaCatenaR(PSD_MATH_STAR_SYSTEM_RES s1, List<PSD_MATH_STAR_SYSTEM_RES> starSystem12, List<PSD_MATH_STAR_SYSTEM_RES> starSystem21, ref List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstcoppimindist, int mindistAct, out int mindistNew)
        {
            bool iscomplete = false;
            int mindistP = s1.PLANETS.Min(x => x.DISTANCE_ORBIT);

            mindistNew = mindistP;
            // 1) trovo la lista dei pianeti associati alla stella s1 con la minima distanza dalla stella
            List<PSD_MATH_PLANET_NUM> lstmindistplanets_fromStar = s1.PLANETS.Where(x => x.DISTANCE_ORBIT == mindistP).ToList();
            // 2) per ogni pianeta alla minima distanza vedo se questo stesso è più vicino ad un'altra stella (diversa da quella iniziale): 
            //      2.a - se no accoppia
            //      2.b - se si prosegui la ricerca ricorsivamente
            foreach (PSD_MATH_PLANET_NUM p1 in lstmindistplanets_fromStar)
            {
                // per ogni pianeta posizionato alla minima distanza dalla stella iniziale s1, trovo la lista dei pianeti/stella associata al pianeta che è stella in M2=> M1 
                PSD_MATH_STAR_SYSTEM_RES planettoStar = starSystem21.Where(X => X.STARNUM == p1.NUM && X.POSX == p1.POSX && X.POSY == p1.POSY).First();
                // elimino dai pianeti la stella iniziale
                planettoStar.PLANETS = planettoStar.PLANETS.Where(X => X.NUM != s1.STARNUM && X.POSX != s1.POSX && X.POSY != s1.POSY).ToList();

                int mindistS = planettoStar.PLANETS.Min(x => x.DISTANCE_ORBIT);
                // trovo la lista dei pianeti/stelle associati alla stella/pianeta p1 con la minima distanza dalla stella/pianeta
                List<PSD_MATH_PLANET_NUM> lstmindiststars_fromPlanet = planettoStar.PLANETS.Where(x => x.DISTANCE_ORBIT == mindistS).ToList();
                mindistNew = mindistS;

                //if(lstcoppimindist.Select(X=>X.STARNUM).Distinct().ToList().Count >= starSystem12.Count)
                // dato che vengono aggiunte solo le coppie non presenti, quando ho raggiunto un numero di coppie uguale al numero di elementi delle matrici
                // ho finito
                // BISOGNA VEDERE SE PRENDENDO LE COPPIE CON AL MINIMA DISTANZA RIESCO AD ACCOPPIARE tutti gli elementi delle matrici
                if (lstcoppimindist.Count >= starSystem12.Count)
                {
                    iscomplete = true;
                }
                else
                {
                    // se non esiste già la coppia star-planet o planet-star aggiungila
                    if ((lstcoppimindist.Where(x => x.STARNUM.NUM == s1.STARNUM && x.STARNUM.POSX == s1.POSX && x.STARNUM.POSY == s1.POSY).Count() == 0) &&
                        (lstcoppimindist.Where(x => x.STARNUM.NUM == p1.NUM && x.STARNUM.POSX == p1.POSX && x.STARNUM.POSY == p1.POSY).Count() == 0))
                    {
                        PSD_MATH_STAR_SYSTEM_MIN_DIST_RES coppia = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                        coppia.PLANET = p1;
                        coppia.IS_COPPIA_UNIVOCA = false;
                        coppia.STARNUM = SetStarNum(s1);
                        coppia.DISTANCE_ORBIT = p1.DISTANCE_ORBIT;
                        lstcoppimindist.Add(coppia);
                        // devo eliminare l'elemento aggiunto dalle matrici
                    }
                    else
                    {
                        /*
                        if (lstcoppimindist.Where(x => x.STARNUM.NUM == s1.STARNUM && x.STARNUM.POSX == s1.POSX && x.STARNUM.POSY == s1.POSY).Count() > 0)
                            lstcoppimindist.Where(x => x.STARNUM.NUM == s1.STARNUM && x.STARNUM.POSX == s1.POSX && x.STARNUM.POSY == s1.POSY).First().DISTANCE_ORBIT++;
                        else
                        if (lstcoppimindist.Where(x => x.STARNUM.NUM == p1.NUM && x.STARNUM.POSX == p1.POSX && x.STARNUM.POSY == p1.POSY).Count() > 0)
                            lstcoppimindist.Where(x => x.STARNUM.NUM == p1.NUM && x.STARNUM.POSX == p1.POSX && x.STARNUM.POSY == p1.POSY).First().DISTANCE_ORBIT++;
                        */
                    }
                }

                PSD_MATH_STAR_SYSTEM_RES p1star = new PSD_MATH_STAR_SYSTEM_RES();
                p1star.DATENUM = p1.DATENUM;
                p1star.GROUPCOL = p1.GROUPCOL;
                p1star.GROUPROW = p1.GROUPROW;
                p1star.POSX = p1.POSX;
                p1star.POSY = p1.POSY;
                p1star.GROUPCOL = p1.GROUPCOL;
                p1star.STARNUM = p1.NUM;
                p1star.STAR_SYSTEM_MATRIX_NAME = planettoStar.STAR_SYSTEM_MATRIX_NAME;

                if (!iscomplete)
                    iscomplete = CalcolaCatenaR(p1star, starSystem21, starSystem12, ref lstcoppimindist, mindistAct, out mindistNew);
            }
            
           
            return iscomplete;
        }

        /// <summary>
        /// Fornisce le soluzioni per i possibili accoppiamenti fra una matrice ed un'altra. 
        /// La matrice di origine corrisponde alle posizioni degli eventi positivi (es regola 1) in un intervallo temporale solitamente di 35 estrazioni.
        /// La matrice risultante è la matrice dell'intervallo temporale successivo
        /// </summary>
        /// <param name="lstStarSystem"></param>
        /// <returns></returns>
        private static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppieMinDist1(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, out List<PSD_MATH_STAR_SYSTEM_RES> lstStarnumNoCalc, out List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstCoppieMigliorIdxDistMin)
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            // contiene la lista delle coppie con STAR - PLANET la cui distanza è migliore da quella esaminata
            // ogni volta che incontro una coppia migliore incremento il valore INDEX_COPPIA_MIGLIOR_DISTANZA di tale coppia, per poter successivamente avere un criterio di scelta
            lstCoppieMigliorIdxDistMin = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            // considero i numeri della matrice risultante distanti al max maxdistorbit dai numeri della matrice di origine
            // per diminuire i calcoli
            int idx;
            
            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWork = lstStarSystem.ToList();
            //  3)
            idx = 0;
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                // a. scorro tutti i pianeti satellite di starM1 (num della matrice risultante M2) e vedo quali distano meno o uguale dagli altri elementi star di M1 alla distanza fra star e planet
                //      a.1: devo trovare in ogni lista PLANETS di M1 tutte le presenze del pianeta satellite con distanza <= dist analizzata
                //      a.2: se non esistono aggiungo la coppia ai risultati
                //      a.3: se esistono, proseguo col prossimo numero accoppiato a star nella lista PLANETS e assegno un valore alle coppie migliori trovate (INDEX_COPPIA_MIGLIOR_DISTANZA) 

                foreach (PSD_MATH_PLANET_NUM planet in starM1.PLANETS)
                {
                    // a.1 prendo tutte gli elementi di M1 le cui liste di pianeti contengono planet e hanno una distanza orbitale inferiore
                    List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWithPlanet = lstStarSystemWork.Where(X =>
                        X.PLANETS.Where(Y => Y.NUM == planet.NUM &&
                            Y.GROUPCOL == planet.GROUPCOL &&
                            Y.GROUPROW == planet.GROUPROW &&
                            Y.DISTANCE_ORBIT <= planet.DISTANCE_ORBIT).Count() > 0).ToList();
                    // se non ce ne sono, allora ho trovato una coppia con la minima distanza
                    if (lstStarSystemWithPlanet.Count == 0)
                    { 
                        PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                        cp.STARNUM = SetStarNum(starM1);
                        cp.PLANET = planet;
                        cp.IS_COPPIA_UNIVOCA = true;
                        //cp.NUM_COPPIE_NON_UNIVOCHE = 0;
                        res.Add(cp);
                        // rimuovo dalla lista il numero accoppiato
                        lstStarSystemWork.RemoveAll(X => X.POSX == starM1.POSX && X.POSY == starM1.POSY);
                        // rimuovo il pianeta satellite accoppiato da ogni lista di pianeti satellite
                        foreach (PSD_MATH_STAR_SYSTEM_RES starM1work in lstStarSystemWork)
                        {
                            starM1work.PLANETS.RemoveAll(X => X.NUM == planet.NUM && X.POSX == planet.POSX && X.POSY == planet.POSY);
                        }
                    }
                    // altrimenti continuo a cercare
                    else {
                        // elimino il pianeta in esame da tutte le liste in cui il pianeta in esame dista più del valore trovato => 
                        //      - dato che ho trovate altre coppie STAR-PLANET più vicine
                        //      - cioè per lo stesso pianeta c'è una stella più vicina

                        //lstStarSystemWithPlanet = 
                        lstStarSystemWork.Where(X =>
                            X.PLANETS.Where(Y => Y.NUM == planet.NUM &&
                                Y.GROUPCOL == planet.GROUPCOL &&
                                Y.GROUPROW == planet.GROUPROW &&
                                Y.DISTANCE_ORBIT > planet.DISTANCE_ORBIT).Count() > 0).ToList()
                            .Select(Z=>Z.PLANETS.RemoveAll(Y=>Y.NUM == planet.NUM && Y.POSX == planet.POSX && Y.POSY == planet.POSY && Y.DISTANCE_ORBIT>planet.DISTANCE_ORBIT));
                        // copio gli elementi ancora in gioco, cioè le coppie la cui distanza è <= a quella della STAR e PLANET attuali
                        // lstStarSystemWithPlanet contiene la lista di stelle col pianeta in gioco la cui distanza dalla stessla è inferiore a qualla analizzata
                        //  - in sostanza contiene le stelle col pianeta ancora in gioco dato che non è ancora statoa ccoppiato ma ha una distanza inferiore ma ancora da valutare rispetto alle altre
                        foreach (PSD_MATH_STAR_SYSTEM_RES pp in lstStarSystemWithPlanet)
                        {
                           
                            //cp.NUM_COPPIE_NON_UNIVOCHE = lstStarSystemWithPlanet.Count;
                            if (lstCoppieMigliorIdxDistMin.Where(X => X.STARNUM.NUM == pp.STARNUM && X.PLANET.POSX == planet.POSX && X.PLANET.POSY == planet.POSY).Count() == 0)
                            {
                                PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                                cp.STARNUM = SetStarNum(pp);
                                cp.PLANET = planet;
                                cp.IS_COPPIA_UNIVOCA = false;
                                cp.DISTANCE_ORBIT = planet.DISTANCE_ORBIT;
                                lstCoppieMigliorIdxDistMin.Add(cp);
                            }
                            else
                            {
                                //int cnt = lstCoppieMigliorIdxDistMin.Where(X => X.STARNUM.NUM == pp.STARNUM && X.PLANET.POSX == planet.POSX && X.PLANET.POSY == planet.POSY).First().INDEX_COPPIA_MIGLIOR_DISTANZA + 1;
                                //lstCoppieMigliorIdxDistMin.Where(X => X.STARNUM.NUM == pp.STARNUM && X.PLANET.POSX == planet.POSX && X.PLANET.POSY == planet.POSY).First().INDEX_COPPIA_MIGLIOR_DISTANZA = idx;
                            }
                        }
                    }

                }
                idx++;
            }
            //lstCoppieMigliorIdxDistMin = lstCoppieMigliorIdxDistMin.OrderBy(X => X.PLANET.NUM).ThenBy(X => X.PLANET.POSX).ThenBy(X => X.PLANET.POSY).ThenBy(X => X.PLANET.INDEX_COPPIA_MIGLIOR_DISTANZA).ToList();
            lstStarnumNoCalc = lstStarSystemWork.ToList();

            return res;
        }

        /// <summary>
        /// DA TESTARE
        /// Come CalcolaCoppieMinDist1, con la diferenza che riesegue l'algoritmo finchè è possibile, cioè finchè trova in maniera inequivocabile una coppia.
        /// Quando una coppia è trovata viene tolta dalla lista. Finisce quando ha finito di scorrere tutta la lista delle stelle.
        /// Otterrò un alista di accoppiamenti (res) ed una lista di non accoppiamenti (lstStarSystem=>lstStarSystemWork)
        /// </summary>
        /// <param name="lstStarSystem"></param>
        /// <param name="lstStarnumNoCalc"></param>
        /// <param name="lstCoppieMigliorIdxDistMin"></param>
        /// <returns></returns>
        private static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppieMinDist2(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res, int numCalcolo)
        {
            int idx;
            // ordino la lista in base alla distanza minima dai pianeti del suo sistema solare:
            //  - considero quindi prima le stella con pianeti molto vicini
            //  - se ce n'è più di una accoppio la stella con un numero minore di pianeti alla stessa distanza
            lstStarSystem = lstStarSystem.OrderBy(x => x.PLANETS.Min(y => y.DISTANCE_ORBIT)).ToList();

            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWork = lstStarSystem.ToList();
            //  3)
            idx = 0;
            bool coppiatrovata = false;
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                // a. scorro tutti i pianeti satellite di starM1 (num della matrice risultante M2) e vedo quali distano meno o uguale dagli altri elementi star di M1 alla distanza fra star e planet
                //      a.1: devo trovare in ogni lista PLANETS di M1 tutte le presenze del pianeta satellite con distanza <= dist analizzata
                //      a.2: se non esistono aggiungo la coppia ai risultati
                //      a.3: se esistono, proseguo col prossimo numero accoppiato a star nella lista PLANETS e assegno un valore alle coppie migliori trovate (INDEX_COPPIA_MIGLIOR_DISTANZA) 

                List<PSD_MATH_PLANET_NUM> planetsord = starM1.PLANETS.OrderBy(x => x.DISTANCE_ORBIT).ToList();

                foreach (PSD_MATH_PLANET_NUM planet in planetsord)
                {
                    // a.1 prendo tutte gli elementi di M1 le cui liste di pianeti contengono planet e hanno una distanza orbitale inferiore
                    //  - SUPERFLUO: la lista è già ordinata in questo senso quindi non ci possono essere stelle più vicine al pianeta in esame
                    /*
                     List<PSD_MATH_STAR_SYSTEM> lstStarSystemWithPlanet = lstStarSystemWork.Where(X =>
                        X.PLANETS.Where(Y => Y.NUM == planet.NUM &&
                            Y.GROUPCOL == planet.GROUPCOL &&
                            Y.GROUPROW == planet.GROUPROW &&
                            Y.DISTANCE_ORBIT < planet.DISTANCE_ORBIT).Count() > 0).ToList();
                     */

                    int mindist = planet.DISTANCE_ORBIT;
                    int numplanetsmindist = starM1.PLANETS.Where(x => x.DISTANCE_ORBIT == mindist).Count();

                    // a.1 prendo tutte gli elementi di M1 le cui liste di pianeti contengono planet e hanno una distanza orbitale uguale
                    //  - in altre parole prendo in considerazione le altre stelle che hanno la stessa distanza da planet
                    List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWithPlanetEqDist = lstStarSystemWork.Where(X =>
                        X.PLANETS.Where(Y => Y.NUM == planet.NUM &&
                            Y.GROUPCOL == planet.GROUPCOL &&
                            Y.GROUPROW == planet.GROUPROW &&
                            Y.DISTANCE_ORBIT == planet.DISTANCE_ORBIT).Count() > 0
                            // nella lista non devo riconteggiare la stella iniziale
                            && (X.POSX != starM1.POSX || X.POSY != starM1.POSY)).ToList();

                    // se non ce ne sono, allora ho trovato una coppia con la minima distanza
                    // se ci sono ambiguità cioè pianeti con min dist =, allora comunque scelgo questo accoppiamento
                    // così determino comunque una scelta fra un ventaglio di scelte simili
                    if (lstStarSystemWithPlanetEqDist.Count == 0)
                    {
                        PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                        cp.STARNUM = SetStarNum(starM1);
                        cp.PLANET = planet;

                        // lista delle stelle la cui distanza dal pianeta in esame è uguale alla distanza corrente, esclusa ovviamente la stella in esame 
                        /*int numcoppiesimili = lstStarSystemWork.Where(X =>
                           X.PLANETS.Where(Y => Y.NUM == planet.NUM &&
                               Y.GROUPCOL == planet.GROUPCOL &&
                               Y.GROUPROW == planet.GROUPROW &&
                               Y.DISTANCE_ORBIT == planet.DISTANCE_ORBIT).Count() > 0
                               && (X.POSX != starM1.POSX || X.POSY != starM1.POSY)).Count();*/
                        cp.IS_COPPIA_UNIVOCA = true; // numcoppiesimili == 0;
                        cp.NUM_CALC = numCalcolo + 1;
                        numCalcolo++;
                        res.Add(cp);
                        // rimuovo dalla lista il numero accoppiato
                        lstStarSystemWork.RemoveAll(X => X.POSX == starM1.POSX && X.POSY == starM1.POSY);
                        // rimuovo il pianeta satellite accoppiato da ogni lista di pianeti satellite
                        foreach (PSD_MATH_STAR_SYSTEM_RES starM1work in lstStarSystemWork)
                        {
                            starM1work.PLANETS.RemoveAll(X => X.NUM == planet.NUM && X.POSX == planet.POSX && X.POSY == planet.POSY);
                        }
                        coppiatrovata = true;
                        break;
                    }
                    // altrimenti continuo a cercare
                    else
                    {
                        // a.2 dalla lista risultante prendo la coppia stella / pianeta la cui distanza è uguale a quella analizzata
                        //     e la cui stella ha un numero inferiore di pianeti vicini a quella distanza
                        lstStarSystemWithPlanetEqDist = lstStarSystemWithPlanetEqDist.OrderBy(x => x.PLANETS.Where(y => y.DISTANCE_ORBIT == mindist).Count()).ToList();

                        if (lstStarSystemWithPlanetEqDist.Count > 0)
                        {
                            PSD_MATH_STAR_SYSTEM_RES starM1_res = lstStarSystemWithPlanetEqDist.First();
                            PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                            cp.STARNUM = SetStarNum(starM1_res);
                            cp.PLANET = planet;
                            cp.IS_COPPIA_UNIVOCA = false;
                            cp.NUM_CALC = numCalcolo + 1;
                            numCalcolo++;
                            res.Add(cp);
                            // rimuovo dalla lista il numero accoppiato
                            lstStarSystemWork.RemoveAll(X => X.POSX == starM1_res.POSX && X.POSY == starM1_res.POSY);
                            // rimuovo il pianeta satellite accoppiato da ogni lista di pianeti satellite
                            foreach (PSD_MATH_STAR_SYSTEM_RES starM1work in lstStarSystemWork)
                            {
                                starM1work.PLANETS.RemoveAll(X => X.NUM == planet.NUM && X.POSX == planet.POSX && X.POSY == planet.POSY);
                            }
                            coppiatrovata = true;
                            break;

                        }
                    }
                }
                idx++;
            }

            if (coppiatrovata)
                res = CalcolaCoppieMinDist2(lstStarSystemWork, res, numCalcolo);

            return res;
        }

        /// <summary>
        /// Determina la lista dei pianeti satellite a distanza max passata come parametro dalle stelle della matrice 1
        /// L'intento è capire se entro un certo limite è possibile trovare TUTTI i satelliti ad una certa distanza massima dalle stelle.
        /// Oppure se ci sono pianeti esclusi dal possibile accoppiamento dalle stelle. 
        /// In altre parole se esistono delle stelle isolate rispetto ai possibili satelliti
        /// </summary>
        /// <param name="lstStarSystem"></param>
        /// <returns></returns>
        public static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppieUnivocheMinDistParam(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, int mindistMin, int mindistMax)
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();

            // lista di stelle i cui pianeti satellite distano al max mindist
            //int numcalcolo = 0;
            //int idx;
            for(int mindist = mindistMin; mindist<= mindistMax; mindist++)
            {
                CalcolaCoppieUnivocheAtDist(lstStarSystem, res, mindist);
            }
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                starM1.NUM_PLANETS_MIN_DIST_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID).Count();
                starM1.EXISTS_COPPIA_UNIVOCA_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID && !X.IS_COPPIA_UNIVOCA).Count() == 0;
            }
            return res;
        }

        // calcola le coppe univoche stelle pianeti, la cui distanza è mindist
        private static void CalcolaCoppieUnivocheAtDist(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res, int mindist)
        {
            // lista delle stelle aventi pianeti alla distanza stabilita
            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemAtDist = lstStarSystem.Where(x => x.PLANETS.Where(y => y.DISTANCE_ORBIT == mindist).Count() > 0).ToList();
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystemAtDist)
            {
                GetCoppiaUnivocaAtDist(res, mindist, starM1, lstStarSystem, true);
            }
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                starM1.NUM_PLANETS_MIN_DIST_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID).Count();
                starM1.EXISTS_COPPIA_UNIVOCA_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID && !X.IS_COPPIA_UNIVOCA).Count() == 0;
            }
        }

        /// <summary>
        /// Calcola le coppe univoche stelle pianeti, la cui distanza è la minima per ogni stella dal suo pianeta più vicino
        /// </summary>
        /// <param name="lstStarSystem"></param>
        /// <returns></returns>
        public static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppieUnivocheAtMinDist(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem)
        {
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();

            // lista delle stelle aventi pianeti alla distanza stabilita
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                GetCoppiaUnivocaAtDist(res, starM1.MIN_DIST_PLANETS, starM1, lstStarSystem, true);
            }
            // in base ai risultati stabilisco se la stella è associata ad un solo pianeta in maniera univoca
            foreach (PSD_MATH_STAR_SYSTEM_RES starM1 in lstStarSystem)
            {
                starM1.NUM_PLANETS_MIN_DIST_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID).Count();
                starM1.EXISTS_COPPIA_UNIVOCA_AFTER_CALC = res.Where(X => X.STARNUM.ID == starM1.ID && !X.IS_COPPIA_UNIVOCA).Count()==0;
            }
            return res;
        }

        private static void GetCoppiaUnivocaAtDist(List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res, int mindist, PSD_MATH_STAR_SYSTEM_RES starM1, List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, bool findIfMultiuplePlanetsMinDist)
        {
            // VINCOLO DI UNICITA':
            // - ci deve essere solo un pianeta alla distanza stabilita
            // - la stella non deve essere già stata accoppiata ad altri pianeti 
            //   oppure se è stata accoppiata, la distanza deve essere superiore a quella attuale
            // - il pianeta non deve essere più vicino ad un altra stella
            int numplanetsok = starM1.PLANETS.Where(X => X.DISTANCE_ORBIT == mindist).Count();
            if (numplanetsok == 1 || (numplanetsok > 1 && findIfMultiuplePlanetsMinDist))
            {            
                foreach (PSD_MATH_PLANET_NUM planet in starM1.PLANETS.Where(X => X.DISTANCE_ORBIT == mindist).ToList())
                {
                    bool isunivoca = true;
                    //PSD_MATH_PLANET_NUM planet = starM1.PLANETS.Where(X => X.DISTANCE_ORBIT == mindist).First();
                    // se non c'è la coppia (con la stella in esame) la aggiunge fra le univoche
                    if (res.Where(X => X.STARNUM.ID == starM1.ID).Count() == 0)
                    {
                        // se il pianeta è associato ad un'altra stella ma ha una distanza maggiore toglilo dalle soluzioni
                        res.RemoveAll(X => X.PLANET.ID == planet.ID && X.DISTANCE_ORBIT > mindist);
                        // devo anche controllare che il pianeta non sia già "impegnato" con una distanza inferiore con un'altra stella
                        List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstsameplanets = res.Where(X => X.PLANET.ID == planet.ID && X.STARNUM.ID != starM1.ID).ToList();

                        //if (s.DISTANCE_ORBIT > mindist)
                        foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES p in lstsameplanets)
                        {                      
                            if (p.DISTANCE_ORBIT == mindist)
                            {
                                res.Where(X => X.PLANET.ID == planet.ID && X.STARNUM.ID == p.STARNUM.ID).First().IS_COPPIA_UNIVOCA = false;
                                isunivoca = false;
                            }

                        }
                        SetResult(res, mindist, isunivoca, starM1, planet);
                    }
                    else
                    {
                        // controlla che la coppia presente abbia la distanza inferiore a quella dell'attuale
                        List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstsamestars = res.Where(X => X.STARNUM.ID == starM1.ID && X.PLANET.ID != planet.ID).ToList();
                        res.RemoveAll(X => X.STARNUM.ID == starM1.ID && X.DISTANCE_ORBIT > mindist);
                        //if (s.DISTANCE_ORBIT > mindist)
                        foreach (PSD_MATH_STAR_SYSTEM_MIN_DIST_RES s in lstsamestars)
                        {
                            if (s.DISTANCE_ORBIT == mindist)
                            {
                                res.Where(X => X.STARNUM.ID == starM1.ID && X.PLANET.ID == s.PLANET.ID).First().IS_COPPIA_UNIVOCA = false;
                                isunivoca = false;
                            }
                        }
                        SetResult(res, mindist, isunivoca, starM1, planet);
                    }
                }
            }
            /*
            if(findIfMultiuplePlanetsMinDist && numplanetsok > 1)
            {
                // devo aggiungere la coppia solo se c'è un solo pianeta fra quelli alla min dist, che non ha altre stelle più vicine

                foreach (PSD_MATH_PLANET_NUM planet in starM1.PLANETS.Where(X => X.DISTANCE_ORBIT == mindist).ToList())
                {
                    // cnt: numero di stelle diverse dalla stella attuale più vicine al pianeta analizzato
                    int cnt = lstStarSystem.Where(X => X.PLANETS.Where(Y => Y.ID == planet.ID && Y.DISTANCE_ORBIT < mindist && Y.STAR_NUM.ID != starM1.ID).Count()>0).Count();

                    if (cnt == 0)
                    {
                        // PROBABILMENTE QUESTI RAGIONAMENTI SONO SUPERFLUI  dato che se il pianeta non è più vicino ad altre stelle va bene
                        //però potrebbe essere che è stato inserito per un'altra stella precedentementse
                        // se non c'è la coppia la aggiunge fra le univoche
                        if (res.Where(X => X.STARNUM.ID == starM1.ID).Count() == 0)
                        {
                            // per scrupolo elimino dalla lista un altra coppia con stella diversa e pianeta uguale
                            res.RemoveAll(X => X.PLANET.ID == planet.ID);
                            SetResult(res, mindist, true, starM1, planet);                           
                        }
                        else
                        {
                            // controlla che la coppia presente abbia la distanza inferiore a quella dell'attuale
                            PSD_MATH_STAR_SYSTEM_MIN_DIST_RES s = res.Where(X => X.STARNUM.ID == starM1.ID).First();
                            if (s.DISTANCE_ORBIT > mindist)
                            {
                                res.RemoveAll(X => X.STARNUM.ID == starM1.ID);
                                SetResult(res, mindist, true, starM1, planet);
                            }
                        }
                    }
                }
            }*/
           
        }

        private static void SetResult(List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res, int mindist, bool isunivoca, PSD_MATH_STAR_SYSTEM_RES starM1, PSD_MATH_PLANET_NUM planet)
        {
            PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
            starM1.MAXDIST_TO_CALC_SATURAZIONE = mindist;
            cp.STARNUM = SetStarNum(starM1);
            cp.PLANET = planet;
            cp.DISTANCE_ORBIT = mindist;
            cp.STARNUM.POSGROUPCOL = starM1.POSGROUPCOL;
            cp.STARNUM.POSX = starM1.POSX;
            cp.STARNUM.POSY = starM1.POSY;
            // idx rappresenta il numero di soluzione per ogni stella
            //cp.INDEX_COPPIA_MIGLIOR_DISTANZA = idx;
            // numcalcolo rappresenta il numero di soluzioni totali
            //cp.NUM_CALCOLO = numcalcolo;
            //idx++;
            //numcalcolo++;
            cp.IS_COPPIA_UNIVOCA = isunivoca;
            res.Add(cp);
        }

        /// <summary>
        /// Formo le coppie semplici, formate da un numero della matrice di origine che ne ha solo uno nella matrice risultante sovrapposto
        /// </summary>
        /// <param name="lstStarSystem"></param>
        /// <param name="lstStarSystemNew"></param>
        /// <returns></returns>
        private static List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> CalcolaCoppieDist0(List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem, out List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemNew)
        {
            int idx = 0;
            List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> res = new List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES>();
            List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystemWork = lstStarSystemWork = lstStarSystem.ToList();
            foreach (PSD_MATH_STAR_SYSTEM_RES itmM1 in lstStarSystem)
            {
                // se esite può essercene solo 1 
                if (itmM1.PLANETS.Where(X => X.DISTANCE_ORBIT == 0).Count() == 1)
                {
                    PSD_MATH_MATRIX_NUM star = SetStarNum(itmM1);
                    PSD_MATH_STAR_SYSTEM_MIN_DIST_RES cp = new PSD_MATH_STAR_SYSTEM_MIN_DIST_RES();
                    cp.STARNUM = star;
                    cp.PLANET = itmM1.PLANETS.Where(X => X.DISTANCE_ORBIT == 0).First();
                    cp.IS_COPPIA_UNIVOCA = true;
                    res.Add(cp);

                    // elimina dello star sistem di lavoro la stelle accoppiata col pianeta a distanza 0
                    lstStarSystemWork.RemoveAll(X => X.STARNUM == star.NUM && X.POSX == star.POSX && X.POSY == star.POSY);
                    // per ogni elemento dello star sistem di lavoro elimina dalla lista dei pianeti quelli accoppiati
                    foreach (PSD_MATH_STAR_SYSTEM_RES itmW in lstStarSystemWork)
                    {                      
                        itmW.PLANETS.RemoveAll(X => X.NUM == cp.PLANET.NUM && X.POSX == cp.PLANET.POSX && X.POSY == cp.PLANET.POSY);
                    }
                }
                idx++;
            }

            lstStarSystemNew = lstStarSystemWork;
            return res;
        }

        private static PSD_MATH_MATRIX_NUM SetStarNum(PSD_MATH_STAR_SYSTEM_RES itmM1)
        {
            PSD_MATH_MATRIX_NUM star = new PSD_MATH_MATRIX_NUM();
            star.ID = itmM1.ID;
            star.NUM = itmM1.STARNUM;
            star.POSX = itmM1.POSX;
            star.POSY = itmM1.POSY;
            star.DATENUM = itmM1.DATENUM;
            star.VAL_SAT = itmM1.Get_INDICE_SATURAZIONE_TO_DIST(itmM1.MAXDIST_TO_CALC_SATURAZIONE);
            star.GROUPCOL = itmM1.GROUPCOL;
            star.GROUPROW = itmM1.GROUPROW;
            star.STAR_SYSTEM_MATRIX_NAME = itmM1.STAR_SYSTEM_MATRIX_NAME;
            return star;
        }

        

        /// <summary>
        /// Determina le stelle che non hanno trovato accoppiamento con i pianeti dell matrice 2. 
        /// Succede se il parametro di minima distanza non soddisfa la minima distanza effettiva fra una stella e tutti i suoi pianeti satellite
        /// </summary>
        /// <param name="lstcoppietrovate"></param>
        /// <param name="lstStarSystem"></param>
        /// <returns></returns>
        private static List<PSD_MATH_STAR_SYSTEM_RES> GetCoppieNonTrovate(List<PSD_MATH_STAR_SYSTEM_MIN_DIST_RES> lstcoppietrovate, List<PSD_MATH_STAR_SYSTEM_RES> lstStarSystem)
        {
            List<PSD_MATH_STAR_SYSTEM_RES> res = new List<PSD_MATH_STAR_SYSTEM_RES>();

            res = lstStarSystem.Where(x => !(lstcoppietrovate.Exists(y => y.STARNUM.POSX == x.POSX && y.STARNUM.POSY == x.POSY))).ToList();

            foreach(PSD_MATH_STAR_SYSTEM_RES r in res)
            {
                // elimino i pianeti che hanno trovato accoppiamento con almeno una stella

            }
            return res;
        }
    }
}
