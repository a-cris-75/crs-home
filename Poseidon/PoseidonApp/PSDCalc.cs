using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoseidonData.DataEntities;
using PoseidonData;
using CRS.CommonControlsLib;

namespace PoseidonApp
{
    public static class PSDCalc
    {

        public static PSD_RES_TOT_SAT GetResTotSat(float maxvalSat, float maxvalSatToY, int maxDisatOrbitToCalcSat, int distfromY, int numcalc
            , List<PSD_MATH_STAR_SYSTEM_RES> resM1M2, List<PSD_MATH_MATRIX_NUM> matrix2 
            ,int idmatrix1, int idmatrix2, DateTime dt1Begin, DateTime dt1End, DateTime dt2Begin, DateTime dt2End
            , PSD_CALCS calc)
        {
            PSD_RES_TOT_SAT res = new PSD_RES_TOT_SAT();
            try
            {
                res.DATA_EVENTO_DA_M1 = dt1Begin;
                res.DATA_EVENTO_A_M1 = dt1End;
                res.DATA_EVENTO_DA_M2 = dt2Begin;
                res.DATA_EVENTO_A_M2 = dt2End;
                res.ID_MATRIX_1 = idmatrix1;
                res.ID_MATRIX_2 = idmatrix2;
                res.ID_CALC =
                    dt1Begin.Day.ToString().PadLeft(2, '0') + "-" + dt1Begin.Month.ToString().PadLeft(2, '0') + "-" + dt1Begin.Year.ToString() + " " + 
                    dt1End.Day.ToString().PadLeft(2, '0') + "-" + dt1End.Month.ToString().PadLeft(2, '0') + "-" + dt1End.Year.ToString() +
                    " <> " +
                    dt2Begin.Day.ToString().PadLeft(2, '0') + "-" + dt2Begin.Month.ToString().PadLeft(2, '0') + "-" + dt2Begin.Year.ToString() + " " +
                    dt2End.Day.ToString().PadLeft(2, '0') + "-" + dt2End.Month.ToString().PadLeft(2, '0') + "-" + dt2End.Year.ToString();

                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2Filtered = resM1M2.Where(X => X.INDICE_SATURAZIONE_TO_DIST <= maxvalSat).ToList();

                res.TOT_STARS = resM1M2.Count();
                res.TOT_PLANETS = matrix2.Count;
                res.NUM_GRPSAT = resM1M2Filtered.Count();
                res.PERC_GRPSAT_STARS = (float)res.NUM_GRPSAT / resM1M2.Count() * 100;

                int totplanetscoinvolti = resM1M2Filtered.Sum(X => X.Get_PLANETS_TO_DIST(X.MAXDIST_TO_CALC_SATURAZIONE).Count());
                res.NUM_PLANETS_IN_GRPSAT_STARS = res.NUM_GRPSAT + totplanetscoinvolti;
                res.NUM_PLANETS_IN_GRPSAT = totplanetscoinvolti;

                List<PSD_MATH_PLANET_NUM> lstplanets = new List<PSD_MATH_PLANET_NUM>();
                foreach (PSD_MATH_STAR_SYSTEM_RES p in resM1M2Filtered)
                {
                    lstplanets.AddRange(p.Get_PLANETS_TO_DIST(p.MAXDIST_TO_CALC_SATURAZIONE));
                }
                
                res.NUM_PLANETS_IN_GRPSAT_DISTINCT = lstplanets.GroupBy(X => X.ID).Count(); 
                res.NUM_PLANETS_IN_GRPSAT_COMMON = totplanetscoinvolti - res.NUM_PLANETS_IN_GRPSAT_DISTINCT;
                res.NUM_CALC = numcalc;

                res.PERC_PLANETS_IN_GRPSAT_DISTINCT_TOT_PLANETS = (float)res.NUM_PLANETS_IN_GRPSAT_DISTINCT / res.TOT_PLANETS * 100;


                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2FilteredToY = resM1M2.Where(X => X.INDICE_SATURAZIONE_TO_DIST_TO_POSY < maxvalSatToY).ToList();
                res.NUM_GRPSAT_LESSTHANSAT_TO_POSY = resM1M2FilteredToY.Count();
                res.PERC_GRPSAT_LESSTHANSAT_TO_POSY_STARS = (float)res.NUM_GRPSAT_LESSTHANSAT_TO_POSY / resM1M2.Count() * 100;

                List<PSD_MATH_STAR_SYSTEM_RES> resM1M2FilteredToY2 = resM1M2.Where(X => X.INDICE_SATURAZIONE_TO_DIST_TO_POSY >= maxvalSatToY).ToList();
                res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY = resM1M2FilteredToY2.Count();
                res.PERC_GRPSAT_GREATERTHANSAT_TO_POSY_STARS = (float)res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY / resM1M2.Count() * 100;

                int numStarsWithPlanetAfterY = resM1M2FilteredToY2.Where(X => X.PLANETS.Where(Y => Y.DISTANCE_ORBIT <= X.MAXDIST_TO_CALC_SATURAZIONE && Y.POSY > X.POSY && Y.GROUPCOL == X.GROUPCOL).Count() > 0).Count();
                res.PERC_GRPSAT_GREATERTHANSAT_TO_POSY_WITH_PLANETS_DISTSAT_AFTERY = (float)numStarsWithPlanetAfterY / (float)res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY * 100;

                #region CALCOLO FINO A DIST di SATURAZIONE: calcola presenza pianeti da Y fino a distanza di saturazione
                int numplanetsAfterStar = 0;
                int numplanetsAfterStarInRuota = 0;
                int numplanetsAfterStarDistN = 0;
                int numplanetsAfterStarInRuotaDistN = 0;
                //foreach (PSD_MATH_STAR_SYSTEM_RES s in resM1M2FilteredToY.Where(X =>
                foreach (PSD_MATH_STAR_SYSTEM_RES s in resM1M2.Where(X => 
                                                        X.PLANETS.Where(Y => Y.DISTANCE_ORBIT <= X.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > X.POSY).Count()>0).ToList())
                {
                    numplanetsAfterStar = numplanetsAfterStar + s.PLANETS.Where(Y => 
                                                        Y.DISTANCE_ORBIT <= s.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > s.POSY).Count();
                    numplanetsAfterStarDistN = numplanetsAfterStarDistN + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= distfromY &&
                                                        Y.POSY > s.POSY).Count();

                    numplanetsAfterStarInRuota = numplanetsAfterStarInRuota + s.PLANETS.Where(Y => 
                                                        Y.DISTANCE_ORBIT <= s.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > s.POSY && Y.GROUPCOL == s.GROUPCOL).Count();
                    numplanetsAfterStarInRuotaDistN = numplanetsAfterStarInRuotaDistN + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= distfromY &&
                                                        Y.POSY > s.POSY && Y.GROUPCOL == s.GROUPCOL).Count();
                }
                
                res.NUM_PLANETS_FROM_POSY_TO_DISTSAT = numplanetsAfterStar;
                res.NUM_PLANETS_FROM_POSY_TO_DISTSAT_GROUPCOLUMN = numplanetsAfterStarInRuota;              
                if (res.TOT_STARS > 0)
                {
                    res.PERC_PLANETS_FROM_POSY_TO_DISTSAT_STARS = (float)numplanetsAfterStar / res.TOT_STARS * 100;
                    res.PERC_PLANETS_FROM_POSY_TO_DISTSAT_STARS_GROUPCOLUMN = (float)numplanetsAfterStarInRuota / res.TOT_STARS * 100;
                }

                res.NUM_PLANETS_FROM_POSY_TO_DIST = numplanetsAfterStarDistN;
                res.NUM_PLANETS_FROM_POSY_TO_DIST_GROUPCOLUMN = numplanetsAfterStarInRuotaDistN;
                if (res.TOT_STARS > 0)
                {
                    res.PERC_PLANETS_FROM_POSY_TO_DIST_STARS = (float)numplanetsAfterStarDistN / res.TOT_STARS * 100;
                    res.PERC_PLANETS_FROM_POSY_TO_DIST_STARS_GROUPCOLUMN = (float)numplanetsAfterStarInRuotaDistN / res.TOT_STARS * 100;
                }
                #endregion

                #region CALCOLO FINO A DIST N: calcola presenza pianerti da Y fino a un determinata distanza orbitale
                int numplanetsAfterStar2 = 0;
                int numplanetsAfterStarDistN2 = 0;
                int numplanetsAfterStarInRuota2 = 0;
                int numplanetsAfterStarInRuotaDistN2 = 0;
                foreach (PSD_MATH_STAR_SYSTEM_RES s in resM1M2FilteredToY2.Where(X =>
                                                        X.PLANETS.Where(Y => Y.DISTANCE_ORBIT <= X.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > X.POSY).Count() > 0).ToList())
                {
                    numplanetsAfterStar2 = numplanetsAfterStar2 + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= s.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > s.POSY).Count();
                    numplanetsAfterStarDistN2 = numplanetsAfterStarDistN2 + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= distfromY &&
                                                        Y.POSY > s.POSY).Count();

                    numplanetsAfterStarInRuota2 = numplanetsAfterStarInRuota2 + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= s.MAXDIST_TO_CALC_SATURAZIONE &&
                                                        Y.POSY > s.POSY && Y.GROUPCOL == s.GROUPCOL).Count();
                    numplanetsAfterStarInRuotaDistN2 = numplanetsAfterStarInRuotaDistN2 + s.PLANETS.Where(Y =>
                                                        Y.DISTANCE_ORBIT <= distfromY &&
                                                        Y.POSY > s.POSY && Y.GROUPCOL == s.GROUPCOL).Count();
                }

                res.NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT = numplanetsAfterStar2;
                res.NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_GROUPCOLUMN = numplanetsAfterStarInRuota2;
                if (res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY > 0)
                {
                    res.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_STARS = (float)numplanetsAfterStar2 / res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY * 100;
                    res.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DISTSAT_STARS_GROUPCOLUMN = (float)numplanetsAfterStarInRuota2 / res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY * 100;
                }

                res.NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST = numplanetsAfterStarDistN2;
                res.NUM_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_GROUPCOLUMN = numplanetsAfterStarInRuotaDistN2;
                if (res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY > 0)
                {
                    res.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_STARS = (float)numplanetsAfterStarDistN2 / res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY * 100;
                    res.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST_STARS_GROUPCOLUMN = (float)numplanetsAfterStarInRuotaDistN2 / res.NUM_GRPSAT_GREATERTHANSAT_TO_POSY * 100;
                }

                //res.PERC_PLANETS_GREATERTHANSAT_FROM_POSY_TO_DIST
                #endregion

                // aggiungo i segnali al calcolo
                // dai segnali poi posso calcolare le previsioni
                // ha senso calcolare i segnali su parametri di R1 diversi: per esempio sun pos decine diverso 
                // in modo da poter incrociare i risultati derivanti da una base di dati diverso 
                if (calc!=null)
                {
                    calc.LST_RES_TOT.Add(res);
                    foreach (PSD_MATH_STAR_SYSTEM_RES r in resM1M2FilteredToY2)
                    {
                        calc.LST_SEGNALI.Add( DbDataAccess.AddSegnaleR1(r.GROUPCOL, r.STARNUM, r.DATENUM, null, 0, calc.IDCalcolo, null));
                    }
                }
            }
            catch { }
            return res;
        }

        public static List<PSD_RES_TOT> GetResTotGroupByNumEstr(int GROUPBY_NUM_ESTR, List<PSD_ESTR_SUCCESSI> LST_SUCCESSI, List<PSD_ESTR_LOTTO> LST_ESTRAZIONI)
        {
            List<PSD_RES_TOT> restot = new List<PSD_RES_TOT>();

            try
            {
               
                List<Tuple<DateTime, int>> lstEstr = new List<Tuple<DateTime, int>>();
                lstEstr.AddRange(LST_ESTRAZIONI.Select(X => new Tuple<DateTime, int>(X.Data, X.IDEstrazione)));
                int idestr1 = lstEstr.First().Item2;
                int totestr = lstEstr.Count();

                int numint = totestr / GROUPBY_NUM_ESTR;
                int[] occprecruote = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                int numestrprec = 0;
                int numoccprec = 0;
                
                PSD_RES_TOT resparzPREC = new PSD_RES_TOT();

                for (int idxintervallo = 0; idxintervallo < numint; idxintervallo++)
                {
                    List<PSD_ESTR_SUCCESSI> LST_SUCC_INTERVAL =
                        LST_SUCCESSI.Where(X => X.IDEstrazione >= idestr1 && X.IDEstrazione < idestr1 + GROUPBY_NUM_ESTR).ToList();

                    List<PSD_ESTR_LOTTO> LST_ESTR_INTERVAL =
                        LST_ESTRAZIONI.Where(X => X.IDEstrazione >= idestr1 && X.IDEstrazione < idestr1 + GROUPBY_NUM_ESTR).ToList();

                    PSD_RES_TOT resparz = GetResultsInterval(LST_SUCC_INTERVAL, LST_ESTR_INTERVAL, idxintervallo, occprecruote, ref numoccprec, ref numestrprec, ref resparzPREC);
                    restot.Add(resparz);
                    idestr1 = LST_ESTR_INTERVAL.Last().IDEstrazione + 1;
                }
            }
            catch { }

            return restot;
        }

        public static List<PSD_RES_TOT> GetResTotGroupByOcc(int GROUPBY_OCCORRENZE, List<PSD_ESTR_SUCCESSI> LST_SUCCESSI, List<PSD_ESTR_LOTTO> LST_ESTRAZIONI)
        {
            List<PSD_RES_TOT> restot = new List<PSD_RES_TOT>();

            try
            {
                List<Tuple<DateTime, int>> lstEstr = new List<Tuple<DateTime, int>>();
                lstEstr.AddRange(LST_ESTRAZIONI.Select(X => new Tuple<DateTime, int>(X.Data, X.IDEstrazione)));
                int idestr1 = lstEstr.First().Item2;
                //int totestr = lstEstr.Count();
                //int numint = totestr / GROUPBY_OCCORRENZE;
                int[] occprecruote = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int numestrprec = 0;
                int numoccprec = 0;
                bool isend = false;
                int idx = 0;
                int idxintervallo = 1;
                PSD_RES_TOT resparzPREC = new PSD_RES_TOT();
                while (!isend)
                {
                    List<PSD_ESTR_SUCCESSI> LST_SUCC_INTERVAL = LST_SUCCESSI.GetRange(idx, GROUPBY_OCCORRENZE);
                    int lastidestrinterval = LST_SUCC_INTERVAL.Last().IDEstrazione;
                    List<PSD_ESTR_LOTTO> LST_ESTR_INTERVAL =
                        LST_ESTRAZIONI.Where(X => X.IDEstrazione >= idestr1 && X.IDEstrazione <= lastidestrinterval).ToList();

                    idx = idx + LST_SUCC_INTERVAL.Count;
                    if (idx >= LST_SUCCESSI.Count - 1) isend = true;

                    PSD_RES_TOT resparz = GetResultsInterval(LST_SUCC_INTERVAL, LST_ESTR_INTERVAL, idxintervallo, occprecruote, ref numoccprec, ref numestrprec, ref resparzPREC);
                    restot.Add(resparz);
                    idestr1 = LST_ESTR_INTERVAL.Last().IDEstrazione + 1;
                    idxintervallo++;
                }
            }
            catch { }

            return restot;
        }

        private static PSD_RES_TOT GetResultsInterval(
             List<PSD_ESTR_SUCCESSI> LST_SUCC_INTERVAL
            , List<PSD_ESTR_LOTTO> LST_ESTR_INTERVAL
            , int idxintervallo
            , int[] occprecruote
            , ref int numoccprec
            , ref int numestrprec
            , ref PSD_RES_TOT resparzPREC)
        {
            //List<PSD_ESTR_SUCCESSI> restmp = LST_SUCCESSI.GetRange(idx, GROUPBY_OCCORRENZE);
            //idx = idx + restmp.Count;
            //if (idx >= LST_SUCCESSI.Count - 1) isend = true;

            //int[] occprecruote = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int i = 0;
            PSD_RES_TOT resparz = new PSD_RES_TOT();
            foreach (string ruota in COSTANTS.RUOTE)
            {
                PSD_RES_RUOTA r = new PSD_RES_RUOTA();
                r.OCCORRENZE = LST_SUCC_INTERVAL.Where(X => X.Ruota == ruota).Count();
                r.RUOTA = ruota;
                r.VARIAZIONE = (float)((float)(r.OCCORRENZE - occprecruote[i]) * 100) / r.OCCORRENZE;
                occprecruote[i] = r.OCCORRENZE;
                i++;

                if (ruota == "BA") { resparz.BA = r; resparz.OCC_BA = r.OCCORRENZE; }
                if (ruota == "CA") { resparz.CA = r; resparz.OCC_CA = r.OCCORRENZE; }
                if (ruota == "FI") { resparz.FI = r; resparz.OCC_FI = r.OCCORRENZE; }
                if (ruota == "GE") { resparz.GE = r; resparz.OCC_GE = r.OCCORRENZE; }
                if (ruota == "MI") { resparz.MI = r; resparz.OCC_MI = r.OCCORRENZE; }
                if (ruota == "NA") { resparz.NA = r; resparz.OCC_NA = r.OCCORRENZE; }
                if (ruota == "PA") { resparz.PA = r; resparz.OCC_PA = r.OCCORRENZE; }
                if (ruota == "RM") { resparz.RM = r; resparz.OCC_RM = r.OCCORRENZE; }
                if (ruota == "TO") { resparz.TO = r; resparz.OCC_TO = r.OCCORRENZE; }
                if (ruota == "VE") { resparz.VE = r; resparz.OCC_VE = r.OCCORRENZE; }
                if (ruota == "NZ") { resparz.NZ = r; resparz.OCC_NZ = r.OCCORRENZE; }
            }

            resparz.DATA_EVENTO_DA = LST_ESTR_INTERVAL.First().Data;
            resparz.DATA_EVENTO_A = LST_ESTR_INTERVAL.Last().Data;
            resparz.ID_ESTR_DA = LST_ESTR_INTERVAL.First().IDEstrazione;
            resparz.ID_ESTR_A = LST_ESTR_INTERVAL.Last().IDEstrazione;
            resparz.NUM_ESTRAZIONI = resparz.ID_ESTR_A - resparz.ID_ESTR_DA;
            //resparz.NUM_ESTRAZIONI_TOT = resparz.NUM_ESTRAZIONI + numestrtot;
            // media estrazioni per avere tot successi dall'inizio del calcolo
            //resparz.NUM_ESTRAZIONI_MEDIA_INTERVALLO_TOT = (float)resparz.NUM_ESTRAZIONI_TOT / numintervallo;
            //resparz.OCCORRENZE_TOT = LST_SUCC_INTERVAL.Count() + occTOT;
            resparz.OCCORRENZE = LST_SUCC_INTERVAL.Count();// == this.GROUPBY_OCCORRENZE
                                                // media occorrenze per avere 
                                                //resparz.OCCORRENZE_MEDIA_INTERVALLO_TOT = (float)(resparz.OCCORRENZE_TOT / resparz.NUM_ESTRAZIONI_TOT);
                                                // media occorrenze per estrazione
            resparz.OCCORRENZE_MEDIA_ESTRAZIONE = (float)resparz.OCCORRENZE / resparz.NUM_ESTRAZIONI;

            resparz.VARIAZIONE_OCC_TOPREC = 0;
            if (numoccprec > 1 && resparz.OCCORRENZE>0)
            {
                resparz.VARIAZIONE_OCC_TOPREC = (float)(resparz.OCCORRENZE - numoccprec) * 100 / resparz.OCCORRENZE;
            }

            resparz.VARIAZIONE_NUMESTR_TOPREC = 0;
            if (resparzPREC.NUM_ESTRAZIONI > 0)
            {
                resparz.VARIAZIONE_NUMESTR_TOPREC = (float)((float)(resparz.NUM_ESTRAZIONI - numestrprec) * 100) / resparz.NUM_ESTRAZIONI;
            }
            //List<PSD_ESTR_LOTTO> lst_estr_parz = LST_ESTR_INTERVAL.Where(X => X.IDEstrazione >= resparz.ID_ESTR_DA && X.IDEstrazione <= resparz.ID_ESTR_A).ToList();
            //resparz.NUM_ESTRATTI = GetNumEstratti(lst_estr_parz);
            resparz.NUM_ESTRATTI = GetNumEstratti(LST_ESTR_INTERVAL);
            resparz.PERC_OCCORRENZE = (float)resparz.OCCORRENZE * 100 / resparz.NUM_ESTRATTI;

            int sumx = LST_SUCC_INTERVAL.Sum(X => X.Posizione + (5 * (COSTANTS.RUOTE_INT[X.Ruota]) - 1));
            int idestr1 = LST_ESTR_INTERVAL.First().IDEstrazione;
            int sumy = LST_SUCC_INTERVAL.Sum(X => X.IDEstrazione - idestr1 + 1);

            if (LST_SUCC_INTERVAL.Count() > 0)
            {
                resparz.BARICENTRO_X = (float)sumx / LST_SUCC_INTERVAL.Count();
                resparz.BARICENTRO_Y = (float)sumy / LST_SUCC_INTERVAL.Count();

                string dirbx = "°";
                string dirby = "°";
                if (resparzPREC.NUM_ESTRAZIONI > 0)
                {
                    // teorema di pitagora: redice[(X2-X1)*(X2-X1) + (Y2-Y1)*(Y2-Y1)]
                    resparz.BAR_BARPREC_LEN_ABS = (float)Math.Sqrt((float)Math.Pow(resparz.BARICENTRO_X - resparzPREC.BARICENTRO_X, 2) + (float)Math.Pow(resparz.BARICENTRO_Y - resparzPREC.BARICENTRO_Y, 2));

                    if (resparz.BARICENTRO_X > resparzPREC.BARICENTRO_X) dirbx = ">";
                    else
                    if (resparz.BARICENTRO_X < resparzPREC.BARICENTRO_X)
                        dirbx = "<";

                    if (resparz.BARICENTRO_Y > resparzPREC.BARICENTRO_Y) dirby = "V";
                    else
                    if (resparz.BARICENTRO_Y < resparzPREC.BARICENTRO_Y)
                        dirby = "^";

                    resparz.DELTA_X_BAR_PREC = resparz.BARICENTRO_X - resparzPREC.BARICENTRO_X;
                    resparz.DELTA_Y_BAR_PREC = resparz.BARICENTRO_Y - resparzPREC.BARICENTRO_Y;
                }
                resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_X = dirbx;
                resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_Y = dirby;

                int numestr = resparz.ID_ESTR_A - resparz.ID_ESTR_DA;
                float barYMatrix = 0;
                float barXMatrix = 0;

                resparz.BAR_BARMATRIX_LEN_ABS = GetLenBaricentroXY(numestr, resparz.NUM_ESTRATTI, resparz.BARICENTRO_X, resparz.BARICENTRO_Y, out barXMatrix, out barYMatrix);
                resparz.BARICENTRO_X_MATRIX = barXMatrix;
                resparz.BARICENTRO_Y_MATRIX = barYMatrix;
                resparz.DELTA_X_BAR_MATRIX = resparz.BARICENTRO_X - resparz.BARICENTRO_X_MATRIX;
                resparz.DELTA_Y_BAR_MATRIX = resparz.BARICENTRO_Y - resparz.BARICENTRO_Y_MATRIX;

                dirbx = "°";
                dirby = "°";
                if (resparz.BARICENTRO_X > resparz.BARICENTRO_X_MATRIX) dirbx = ">";
                else
                if (resparz.BARICENTRO_X < resparz.BARICENTRO_X_MATRIX)
                    dirbx = "<";

                if (resparz.BARICENTRO_Y > resparz.BARICENTRO_Y_MATRIX) dirby = "V";
                else
                if (resparz.BARICENTRO_Y < resparz.BARICENTRO_Y_MATRIX)
                    dirby = "^";
                resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_X_DA_MATRIX = dirbx;
                resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_Y_DA_MATRIX = dirby;

            }
            resparz.IDX = idxintervallo;
            //restot.Add(resparz);

            //idestr1 = LST_SUCC_INTERVAL.Last().IDEstrazione + 1;

            resparzPREC = resparz;
            //occTOT = occTOT + resparz.OCCORRENZE;
            numestrprec = resparz.NUM_ESTRAZIONI;
            numoccprec = resparz.OCCORRENZE;
            //numestrtot = numestrtot + resparz.NUM_ESTRAZIONI;
            //numintervallo++;

            return resparz;
        }


        public static List<PSD_RES_TOT> GetResTotGroupByNumEstr_OLD(int GROUPBY_NUM_ESTR, List<PSD_ESTR_SUCCESSI> LST_SUCCESSI, List<PSD_ESTR_LOTTO> LST_ESTRAZIONI)
        {
            List<PSD_RES_TOT> restot = new List<PSD_RES_TOT>();

            try
            {

                List<Tuple<DateTime, int>> lstEstr = new List<Tuple<DateTime, int>>();
                lstEstr.AddRange(LST_ESTRAZIONI.Select(X => new Tuple<DateTime, int>(X.Data, X.IDEstrazione)));
                int idestr1 = lstEstr.First().Item2;
                int totestr = lstEstr.Count();

                int numint = totestr / GROUPBY_NUM_ESTR;
                int[] occprecruote = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                int occprec = 0;
                int occTOT = 0;
                int idx = 1;

                PSD_RES_TOT resparzPREC = new PSD_RES_TOT();
                for (int interval = 0; interval < numint; interval++)
                {

                    List<PSD_ESTR_SUCCESSI> restmp =
                        LST_SUCCESSI.Where(X => X.IDEstrazione >= idestr1 && X.IDEstrazione <= idestr1 + GROUPBY_NUM_ESTR).ToList();

                    int i = 0;
                    PSD_RES_TOT resparz = new PSD_RES_TOT();
                    foreach (string ruota in COSTANTS.RUOTE)
                    {
                        PSD_RES_RUOTA r = new PSD_RES_RUOTA();
                        r.OCCORRENZE = restmp.Where(X => X.Ruota == ruota).Count();
                        r.RUOTA = ruota;
                        r.VARIAZIONE = (float)((float)(r.OCCORRENZE - occprecruote[i]) * 100) / r.OCCORRENZE;
                        occprecruote[i] = r.OCCORRENZE;
                        i++;

                        if (ruota == "BA") { resparz.BA = r; resparz.OCC_BA = r.OCCORRENZE; }
                        if (ruota == "CA") { resparz.CA = r; resparz.OCC_CA = r.OCCORRENZE; }
                        if (ruota == "FI") { resparz.FI = r; resparz.OCC_FI = r.OCCORRENZE; }
                        if (ruota == "GE") { resparz.GE = r; resparz.OCC_GE = r.OCCORRENZE; }
                        if (ruota == "MI") { resparz.MI = r; resparz.OCC_MI = r.OCCORRENZE; }
                        if (ruota == "NA") { resparz.NA = r; resparz.OCC_NA = r.OCCORRENZE; }
                        if (ruota == "PA") { resparz.PA = r; resparz.OCC_PA = r.OCCORRENZE; }
                        if (ruota == "RM") { resparz.RM = r; resparz.OCC_RM = r.OCCORRENZE; }
                        if (ruota == "TO") { resparz.TO = r; resparz.OCC_TO = r.OCCORRENZE; }
                        if (ruota == "VE") { resparz.VE = r; resparz.OCC_VE = r.OCCORRENZE; }
                        if (ruota == "NZ") { resparz.NZ = r; resparz.OCC_NZ = r.OCCORRENZE; }
                    }

                    resparz.DATA_EVENTO_DA = lstEstr.Where(X => X.Item2 == idestr1).First().Item1;
                    resparz.DATA_EVENTO_A = lstEstr.Where(X => X.Item2 == idestr1 + GROUPBY_NUM_ESTR).First().Item1;
                    resparz.ID_ESTR_DA = idestr1;
                    resparz.ID_ESTR_A = idestr1 + GROUPBY_NUM_ESTR;
                    resparz.NUM_ESTRAZIONI = GROUPBY_NUM_ESTR;
                    //resparz.NUM_ESTRAZIONI_TOT = GROUPBY_NUM_ESTR * (interval + 1);
                    //resparz.OCCORRENZE_TOT = restmp.Count() + occTOT;
                    resparz.OCCORRENZE = restmp.Count();
                    // media di occorrenze per intervallo
                    //resparz.OCCORRENZE_MEDIA_INTERVALLO_TOT = (float)resparz.OCCORRENZE_TOT / (interval + 1);
                    resparz.OCCORRENZE_MEDIA_ESTRAZIONE = (float)resparz.OCCORRENZE / resparz.NUM_ESTRAZIONI;

                    resparz.VARIAZIONE_OCC_TOPREC = 0;
                    //resparz.VARIAZIONE_TOMEDIA = 0;
                    if (idx > 1)
                    {
                        resparz.VARIAZIONE_OCC_TOPREC = (float)(resparz.OCCORRENZE - occprec) * 100 / resparz.OCCORRENZE;//(float)(resparz.OCCORRENZE - occprec);
                        //resparz.VARIAZIONE_TOMEDIA = (float)(resparz.OCCORRENZE_MEDIA_INTERVALLO_TOT - resparz.OCCORRENZE) * 100 / resparz.OCCORRENZE_MEDIA_INTERVALLO_TOT;
                    }
                    List<PSD_ESTR_LOTTO> lst_estr_parz = LST_ESTRAZIONI.Where(X => X.IDEstrazione >= idestr1 && X.IDEstrazione <= idestr1 + GROUPBY_NUM_ESTR).ToList();
                    resparz.NUM_ESTRATTI = GetNumEstratti(lst_estr_parz);
                    resparz.PERC_OCCORRENZE = (float)resparz.OCCORRENZE * 100 / resparz.NUM_ESTRATTI;

                    int sumx = restmp.Sum(X => X.Posizione + (5 * (COSTANTS.RUOTE_INT[X.Ruota] - 1)));
                    int sumy = restmp.Sum(X => X.IDEstrazione - idestr1 + 1);

                    if (restmp.Count() > 0)
                    {
                        resparz.BARICENTRO_X = (float)sumx / restmp.Count();
                        resparz.BARICENTRO_Y = (float)sumy / restmp.Count();
                        // teorema di pitagora: redice[(X2-X1)*(X2-X1) + (Y2-Y1)*(Y2-Y1)]

                        string dirbx = "°";
                        string dirby = "°";
                        resparz.BAR_BARPREC_LEN_ABS = 0;
                        if (resparzPREC.NUM_ESTRAZIONI > 0)
                        {
                            resparz.BAR_BARPREC_LEN_ABS = (float)Math.Sqrt((float)Math.Pow(resparz.BARICENTRO_X - resparzPREC.BARICENTRO_X, 2) + (float)Math.Pow(resparz.BARICENTRO_Y - resparzPREC.BARICENTRO_Y, 2));

                            if (resparz.BARICENTRO_X > resparzPREC.BARICENTRO_X) dirbx = ">";
                            else
                            if (resparz.BARICENTRO_X < resparzPREC.BARICENTRO_X)
                                dirbx = "<";

                            if (resparz.BARICENTRO_Y > resparzPREC.BARICENTRO_Y) dirby = "V";
                            else
                            if (resparz.BARICENTRO_Y < resparzPREC.BARICENTRO_Y)
                                dirby = "^";

                            resparz.DELTA_X_BAR_PREC = resparz.BARICENTRO_X - resparzPREC.BARICENTRO_X;
                            resparz.DELTA_Y_BAR_PREC = resparz.BARICENTRO_Y - resparzPREC.BARICENTRO_Y;
                        }
                        resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_X = dirbx;
                        resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_Y = dirby;

                        float barYMatrix = 0; //(float)GROUPBY_NUM_ESTR / 2;
                        float barXMatrix = 0; //(float)((float)resparz.NUM_ESTRATTI / GROUPBY_NUM_ESTR)/2;

                        resparz.BAR_BARMATRIX_LEN_ABS = GetLenBaricentroXY(GROUPBY_NUM_ESTR, resparz.NUM_ESTRATTI, resparz.BARICENTRO_X, resparz.BARICENTRO_Y, out barXMatrix, out barYMatrix);
                        resparz.BARICENTRO_X_MATRIX = barXMatrix;
                        resparz.BARICENTRO_Y_MATRIX = barYMatrix;
                        resparz.DELTA_X_BAR_MATRIX = resparz.BARICENTRO_X - resparz.BARICENTRO_X_MATRIX;
                        resparz.DELTA_Y_BAR_MATRIX = resparz.BARICENTRO_Y - resparz.BARICENTRO_Y_MATRIX;

                        //resparz.BAR_BARMATRIX_LEN_ABS = (float)Math.Sqrt((float)Math.Pow(resparz.BARICENTRO_X - barXMatrix, 2) + (float)Math.Pow(resparz.BARICENTRO_Y - barYMatrix, 2));

                        dirbx = "°";
                        dirby = "°";
                        if (resparz.BARICENTRO_X > resparz.BARICENTRO_X_MATRIX) dirbx = ">";
                        else
                            if (resparz.BARICENTRO_X < resparz.BARICENTRO_X_MATRIX)
                            dirbx = "<";

                        if (resparz.BARICENTRO_Y > resparz.BARICENTRO_Y_MATRIX) dirby = "V";
                        else
                        if (resparz.BARICENTRO_Y < resparz.BARICENTRO_Y_MATRIX)
                            dirby = "^";
                        resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_X_DA_MATRIX = dirbx;
                        resparz.DIREZIONE_SPOSTAMENTO_BARICENTRO_Y_DA_MATRIX = dirby;

                    }
                    idestr1 = idestr1 + GROUPBY_NUM_ESTR + 1;

                    resparz.IDX = idx;

                    restot.Add(resparz);

                    occTOT = occTOT + resparz.OCCORRENZE;
                    occprec = resparz.OCCORRENZE;

                    resparzPREC = resparz;
                    idx++;
                }
            }
            catch { }

            return restot;
        }

        private static float GetLenBaricentroXY(int numestrazioni, int totestratti, float bar1X, float bar1Y, out float barX, out float barY)
        {
            barX = 0;
            barY = 0;
            int numX = totestratti / numestrazioni;
            for (int i = 1; i <= numX; i++)
                barX = barX + i;

            for (int i = 1; i <= numestrazioni; i++)
                barY = barY + i;

            barX = (float)barX / numX;
            barY = (float)barY / numestrazioni;
            return (float)Math.Sqrt((float)Math.Pow(bar1X - barX, 2) + (float)Math.Pow(bar1Y - barY, 2)); ;
        }

        private static int GetNumEstratti(List<PSD_ESTR_LOTTO> lstestr)
        {
            int res = 0;// lstestr.Count * 5 * 11;
            foreach(PSD_ESTR_LOTTO e in lstestr)
            {
                if (e.BA1 > 0) res = res + 5;
                if (e.CA1 > 0) res = res + 5;
                if (e.FI1 > 0) res = res + 5;
                if (e.GE1 > 0) res = res + 5;
                if (e.MI1 > 0) res = res + 5;
                if (e.NA1 > 0) res = res + 5;
                if (e.PA1 > 0) res = res + 5;
                if (e.RM1 > 0) res = res + 5;
                if (e.TO1 > 0) res = res + 5;
                if (e.VE1 > 0) res = res + 5;
                if (e.NZ1 > 0) res = res + 5;
            }
            return res;

        }
       
        public static void GetMatrix(DateTime dtM1Start, DateTime dtM1End, DateTime dtM2Start, DateTime dtM2End, bool parificaOccorrenzeMatrici, List<PSD_ESTR_SUCCESSI> LST_SUCCESSI, out List<PSD_MATH_MATRIX_NUM> matrix1_lstSTAR, out List<PSD_MATH_MATRIX_NUM> matrix2_lstPLANETS)
        {
            matrix1_lstSTAR = new List<PSD_MATH_MATRIX_NUM>();
            matrix2_lstPLANETS = new List<PSD_MATH_MATRIX_NUM>();

            List<PSD_ESTR_SUCCESSI> lstsucc1 = LST_SUCCESSI.Where(X => X.Data >= dtM1Start && X.Data <= dtM1End).ToList();           
            List<PSD_ESTR_SUCCESSI> lstsucc2 = LST_SUCCESSI.Where(X => X.Data >= dtM2Start && X.Data <= dtM2End).ToList();

            if (parificaOccorrenzeMatrici)
            {
                int maxocc = lstsucc1.Count;
                if (maxocc > lstsucc2.Count) maxocc = lstsucc2.Count;
                lstsucc1.RemoveRange(maxocc, lstsucc1.Count - maxocc);
                lstsucc2.RemoveRange(maxocc, lstsucc2.Count - maxocc);
            }
            
            lstsucc1 = lstsucc1.OrderBy(X => X.IDEstrazione).ThenBy(X => X.Ruota).ToList();
            int idestr1 = lstsucc1.First().IDEstrazione;
            FillMatrix(matrix1_lstSTAR, matrix2_lstPLANETS, lstsucc1, lstsucc2);
          
        }

        // non usata, vca bene eventualmente se si desidera usare idx estrazione anzichè le date
        public static void GetMatrix(int idxM1Start, int idxM2Start, int numoccorrenze, List<PSD_ESTR_SUCCESSI> LST_SUCCESSI, out List<PSD_MATH_MATRIX_NUM> matrix1_lstSTAR, out List<PSD_MATH_MATRIX_NUM> matrix2_lstPLANETS)
        {
            matrix1_lstSTAR = new List<PSD_MATH_MATRIX_NUM>();
            matrix2_lstPLANETS = new List<PSD_MATH_MATRIX_NUM>();

            List<PSD_ESTR_SUCCESSI> lstsucc1 = LST_SUCCESSI.Where(X => X.IDEstrazione >= idxM1Start).ToList();
            lstsucc1.RemoveRange(numoccorrenze, lstsucc1.Count - numoccorrenze);
            List<PSD_ESTR_SUCCESSI> lstsucc2 = LST_SUCCESSI.Where(X => X.IDEstrazione >= idxM2Start).ToList();
            lstsucc2.RemoveRange(numoccorrenze, lstsucc2.Count - numoccorrenze);
            FillMatrix(matrix1_lstSTAR, matrix2_lstPLANETS, lstsucc1, lstsucc2);
        }

        private static void FillMatrix(List<PSD_MATH_MATRIX_NUM> matrix1_lstSTAR, List<PSD_MATH_MATRIX_NUM> matrix2_lstPLANETS, List<PSD_ESTR_SUCCESSI> lstsucc1, List<PSD_ESTR_SUCCESSI> lstsucc2)
        {
            lstsucc1 = lstsucc1.OrderBy(X => X.IDEstrazione).ThenBy(X => X.Ruota).ToList();
            int idestr1 = lstsucc1.First().IDEstrazione;

            foreach (PSD_ESTR_SUCCESSI s in lstsucc1)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATENUM = s.Data;
                n.GROUPCOL = s.Ruota;
                n.GROUPROW = s.IDEstrazione.ToString();
                n.NUM = s.NumeroA;
                n.POSX = s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.Ruota + s.Posizione.ToString();
                // ID: pos X + grop col + pos in group (es:BA1010 => estratto a Ba in pos 1 alla 10ma riga della matrice)
                n.ID = n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix1_lstSTAR.Add(n);
            }
            idestr1 = lstsucc2.First().IDEstrazione;
            foreach (PSD_ESTR_SUCCESSI s in lstsucc2)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATENUM = s.Data;
                n.GROUPCOL = s.Ruota;
                n.GROUPROW = s.IDEstrazione.ToString();
                n.NUM = s.NumeroA;
                n.POSX = s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.Ruota + s.Posizione.ToString();
                n.ID = n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix2_lstPLANETS.Add(n);
            }

            //return lstsucc1;
        }

        private static void GetMatrixFromRes(List<PSD_MATH_MATRIX_NUM> matrix1_lstSTAR, List<PSD_MATH_MATRIX_NUM> matrix2_lstPLANETS, List<PSD_MATH_STAR_SYSTEM_RES> lstsucc1, List<PSD_MATH_STAR_SYSTEM_RES> lstsucc2)
        {
            lstsucc1 = lstsucc1.OrderBy(X => X.POSY).ThenBy(X => X.POSX).ToList();
            int idestr1 = lstsucc1.First().POSY;

            foreach (PSD_MATH_STAR_SYSTEM_RES s in lstsucc1)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATENUM = s.DATENUM;
                n.GROUPCOL = s.GROUPCOL;// Convert.ToString( s.POSY / 5) ;
                n.GROUPROW = s.GROUPROW;
                n.NUM = s.STARNUM;
                n.POSX = s.POSX;//s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.POSY;//s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.POSGROUPCOL;//s.Ruota + s.Posizione.ToString();
                // ID: pos X + grop col + pos in group (es:BA1010 => estratto a Ba in pos 1 alla 10ma riga della matrice)
                n.ID = s.ID;//n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix1_lstSTAR.Add(n);
            }
            idestr1 = lstsucc2.First().POSY;
            foreach (PSD_MATH_STAR_SYSTEM_RES s in lstsucc2)
            {
                PSD_MATH_PLANET_NUM n = new PSD_MATH_PLANET_NUM();
                n.DATENUM = s.DATENUM;
                n.GROUPCOL = s.GROUPCOL;// Convert.ToString( s.POSY / 5) ;
                n.GROUPROW = s.GROUPROW;
                n.NUM = s.STARNUM;
                n.POSX = s.POSX;//s.Posizione + (5 * (COSTANTS.RUOTE_INT[s.Ruota] - 1));
                n.POSY = s.POSY;//s.IDEstrazione - idestr1;
                n.POSGROUPCOL = s.POSGROUPCOL;//s.Ruota + s.Posizione.ToString();
                n.ID = s.ID;//n.POSGROUPCOL + "-" + n.POSY.ToString().PadLeft(3, '0');
                matrix2_lstPLANETS.Add(n);
            }

            //return lstsucc1;
        }

    }
}
