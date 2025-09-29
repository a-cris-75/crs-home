using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CRS.DBLibrary;
using PoseidonData.DataEntities;
using CRS.Library;

namespace PoseidonData
{
    public static class DbDataAccess
    {
        private static string connectionString = Parameters.ConnectionString;
        private static string providerName = Parameters.ProviderName;


        /// <summary>
        // insieme dei metodi per accesso dati tabelle db
        /// Restituiscce l'ultima estrazione importata. 
        /// ATTENZIONE: non funziona con sql server CE
        /// </summary>
        /// <returns></returns>
        public static PSD_ESTR_LOTTO GetLastEstrImported()
        {
            List<PSD_ESTR_LOTTO> estr = new List<PSD_ESTR_LOTTO>();
            //string sql = "SELECT coalesce(MAX(DATA),0) AS MAXDATE FROM PSD_ESTR_LOTTO ";
            string sql = 
                "SELECT * from  PSD_ESTR_LOTTO                              " + 
                "   where data =                                            " +
                "    (select coalesce( max(data),0) FROM PSD_ESTR_LOTTO )   " ;

            try
            {

                DbFactory conn = new DbFactory(connectionString, providerName);
                estr = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql);
                //DataRow r = conn.GetSingleRow(sql);
            }
            catch { }
            return estr.First();
        }

        public static DateTime GetLastDataLottoImported()
        {
            string sql = "SELECT coalesce(MAX(DATA),0) AS MAXDATE FROM PSD_ESTR_LOTTO ";
            DateTime res = DateTime.MinValue;
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                DataRow r = conn.GetSingleRow(sql);
                res = Convert.ToDateTime(r["MAXDATE"]);
            }
            catch { }
            return res;
        }

        public static DateTime GetLastDataEstrImported()
        {
            string sql = "SELECT coalesce(MAX(DATA),0) AS MAXDATE FROM PSD_ESTR_ESTRAZIONI ";
            DateTime res = DateTime.MinValue;
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                DataRow r = conn.GetSingleRow(sql);
                res = Convert.ToDateTime(r["MAXDATE"]);
            }
            catch { }
            return res;
        }

        public static DateTime GetLastDataR1Imported()
        {
            string sql = "SELECT coalesce(MAX(DATA_EVENTO),0) AS MAXDATE FROM PSD_REG_R1 ";
            DateTime res = DateTime.MinValue;
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                DataRow r = conn.GetSingleRow(sql);
                res = Convert.ToDateTime(r["MAXDATE"]);
            }
            catch { }
            return res;
        }

        public static int GetNumEstrazioni(DateTime dt1, DateTime dt2)
        {
            int res = 0;
            try
            {
                string sql = " SELECT count(*) FROM PSD_ESTR_LOTTO WHERE DATA BETWEEN %u AND %u";
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = Convert.ToInt32(conn.GetExecuteScalar(sql, dt1, dt2));
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Restituisce la data della prossima estrazione, in avanti o indietro a seconda che deltanumestr sia positivo oppure negativo. 
        /// Non è necessario che la data corrisponda ad una data di estrazione.
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="deltanumestr"></param>
        /// <returns></returns>
        public static DateTime GetDataEstrazioneNext(DateTime dtFrom, int deltanumestr)
        {
            int num = 0;
            DateTime res = DateTime.MinValue;
            try
            {
                // l'estrazione potrebbe non eseistere nella data esatta
                string sql = "SELECT * FROM PSD_ESTR_LOTTO WHERE DATA >= %u and DATA <=%u ";
                DbFactory conn = new DbFactory(connectionString, providerName);

                List<PSD_ESTR_LOTTO> e = new List<PSD_ESTR_LOTTO>();
                if(deltanumestr>0)
                    e = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql + " order by DATA" , dtFrom, dtFrom.AddDays(7));
                else
                    e = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql + "order by DATA desc", dtFrom.AddDays(-7), dtFrom);

                if (e.Count > 0)
                {
                    num = e.First().Numero;
                    sql = "SELECT * FROM PSD_ESTR_LOTTO WHERE NUMERO = %d";
                    e = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql, num + deltanumestr);
                    if (e.Count > 0) res = Convert.ToDateTime(e.First().Data);
                }
               
            }
            catch { }
            return res;
        }
        /// <summary>
        /// Restituisce l'estrazione solo se esiste nella data passata come parametro
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public static PSD_ESTR_LOTTO GetEstrazione(DateTime dt1)
        {
            List<PSD_ESTR_LOTTO> lst = new List<PSD_ESTR_LOTTO>();
            PSD_ESTR_LOTTO res = new PSD_ESTR_LOTTO();
            try
            {
                string sql = " SELECT * FROM PSD_ESTR_LOTTO WHERE DATA = %u";
                DbFactory conn = new DbFactory(connectionString, providerName);
                lst = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql, dt1);
                if (lst.Count > 0) res = lst.First();
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Restituisce una lista di estrazioni da a
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static List<PSD_ESTR_LOTTO> GetEstrazioni(DateTime dt1, DateTime dt2)
        {
            List<PSD_ESTR_LOTTO> res = new List<PSD_ESTR_LOTTO>();
            try
            {
                string sql = " SELECT * FROM PSD_ESTR_LOTTO WHERE DATA BETWEEN %u AND %u";
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql, dt1, dt2);
                int idx = 1;
                foreach (PSD_ESTR_LOTTO e in res)
                {
                    e.Numero = idx;
                    idx++;
                }
            }
            catch { }
            return res; 
        }

        public static List<PSD_ESTR_ESTRAZIONI> GetEstrazioniSuRuote(DateTime dt1, DateTime dt2)
        {
            List<PSD_ESTR_ESTRAZIONI> res = new List<PSD_ESTR_ESTRAZIONI>();
            try
            {
                string sql = " SELECT * FROM PSD_ESTR_ESTRAZIONI WHERE DATA BETWEEN %u AND %u ORDER BY NUMERO,RUOTA ";
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.ExecuteSql<PSD_ESTR_ESTRAZIONI>(sql, dt1, dt2);

                int idx = 1;
                List<int> resdistinct = res.Select(X => X.IDEstrazione).Distinct().ToList().OrderBy(X=>X).ToList();
                foreach (int r in resdistinct)
                {
                    foreach(PSD_ESTR_ESTRAZIONI e in res.Where(X=>X.IDEstrazione == r).ToList())
                        e.Numero = idx;
                    idx++;
                }
            }
            catch { }
            return res;
        }
        /// <summary>
        ///  Restituisce le n estrazioni dalla data dtBegin, in avanti o indietro. 
        ///  Non è necessario che in dtBegin esista un'estrazione.
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="numEstrPrec"></param>
        /// <returns></returns>
        public static List<PSD_ESTR_LOTTO> GetEstrazioniDelta(DateTime dtBegin, int deltanumestr)
        {
            List<PSD_ESTR_LOTTO> res = new List<PSD_ESTR_LOTTO>();
            try
            {

                // l'estrazione potrebbe non eseistede nella data esatta
                string sql = " SELECT * FROM PSD_ESTR_LOTTO WHERE DATA >= %u and DATA <=%u ";
                DbFactory conn = new DbFactory(connectionString, providerName);

                List<PSD_ESTR_LOTTO> e = new List<PSD_ESTR_LOTTO>();
                if (deltanumestr > 0)
                    e = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql + " order by DATA", dtBegin, dtBegin.AddDays(7));
                else
                    e = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql + "order by DATA desc", dtBegin.AddDays(-7), dtBegin);

                if (e.Count > 0)
                {
                    int num = e.First().Numero;
                    sql = " SELECT * FROM PSD_ESTR_LOTTO WHERE NUMERO >= %d and NUMERO<=%d";
                    if (deltanumestr > 0) res = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql, num, num + deltanumestr);
                    else res = conn.ExecuteSql<PSD_ESTR_LOTTO>(sql, num - deltanumestr, num);
                }

            }
            catch { }
            return res;
        }

        /// <summary>
        /// Restituisce la lista dei successi della R1 in base ai filtri passati. 
        /// Successivamente è possibile raggruppare i risultati PER DETERMINARE LE OCCORRENZE
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="ruote"></param>
        /// <param name="paramsR1"></param>
        /// <returns></returns>
        public static List<PSD_ESTR_SUCCESSI> GetResR1(DateTime dt1, DateTime dt2, List<string> ruote, PSD_PARAMETRI_R1 paramsR1)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();

            string where = "";
            if (ruote.Count < 11 && ruote.Count > 0)
                where = where + " AND " + DbUtils.PrepareSqlINClause<string>("r.RUOTA", ruote);
            if (paramsR1.COLPI.Count > 0 && paramsR1.COLPI.Count < 3)
                where = where + " AND " + DbUtils.PrepareSqlINClause<int>("r.NUM_ESTRAZIONE_SUCC", paramsR1.COLPI);
            if (paramsR1.TIPO_FREQUENZA.Count > 0 && paramsR1.TIPO_FREQUENZA.Count < 4)
                where = where + " AND " + DbUtils.PrepareSqlINClause<int>("r.TIPO_FREQUENZA", paramsR1.TIPO_FREQUENZA);
            if (paramsR1.POS_DEC_R12.Count > 0)
                where = where + " AND " + DbUtils.PrepareSqlINClause<string>("r.POSDECR1R2", paramsR1.POS_DEC_R12);
            if (paramsR1.ACCOPPIATE_DEC.Count > 0)
                where = where + " AND " + DbUtils.PrepareSqlINClause<string>("r.ACCOPIATA", paramsR1.ACCOPPIATE_DEC);
            try
            {
                string sql = "SELECT r.*,e.*                                " +
                            //"   ,count(*) as OCCORRENZE                     " +
                            " FROM psd_estr_estrazioni e                    " +
                            " JOIN psd_reg_r1 r ON r.data_evento = e.Data   " +
                            //"  AND(r.numeroa = e.N1 OR r.numeroa = e.N2 OR  " +
                            //"      r.numeroa = e.N3 OR r.numeroa = e.N4 OR  " +
                            //"      r.numeroa = e.N5)                        " +
                            "  AND r.ruota = e.ruota                        " +
                            " WHERE e.data BETWEEN @dt AND @dt              " + 
                            where;
                     
                    /*
                    where e.data > getDate() - 30000
                    --and e.ruota = 'BA'
                    group by r.data_evento,r.ruota
                    ; ";*/
                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_ESTR_SUCCESSI>(sql, dt1, dt2);
                
            }
            catch { }
            return lstres;
        }

        public static List<PSD_ESTR_SUCCESSI> GetResR1NoDbTable(DateTime dt1, DateTime dt2, List<string> ruote, PSD_PARAMETRI_R1 paramsR1)
        {
            List<PSD_ESTR_SUCCESSI> res = new List<PSD_ESTR_SUCCESSI>();
            List<PSD_ESTR_ESTRAZIONI> estr = GetEstrazioniSuRuote(dt1, dt2);
            foreach (string r in ruote)
            {
                List<PSD_ESTR_SUCCESSI> resR = GetResR1Ruota(estr, r, paramsR1.ACCOPPIATE_DEC.Where(X => X.Contains(r)).First(), paramsR1);
                res.AddRange(resR);
            }

            res = res.OrderBy(X => X.Data).ThenBy(Y => Y.Ruota).ToList();

            return res;
        }

        public static List<PSD_ESTR_SUCCESSI> GetResR1Ruota(List<PSD_ESTR_ESTRAZIONI> estr, string ruota, string accopiataDec, PSD_PARAMETRI_R1 paramsR1)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();
            string R1 = accopiataDec.Substring(0, 2);
            string R2 = accopiataDec.Substring(3, 2);
            List<PSD_ESTR_ESTRAZIONI> estrR1 = estr.Where(X => X.Ruota == R1).ToList();
            List<PSD_ESTR_ESTRAZIONI> estrR2 = estr.Where(X => X.Ruota == R2).ToList();
            List<PSD_ESTR_ESTRAZIONI> estrR = estr.Where(X => X.Ruota == ruota).ToList();
            lstres = AddPrevisioniR1(ruota, paramsR1, R1, R2, estrR1, estrR2, estrR, true);

            return lstres;
        }

        public static List<PSD_ESTR_SUCCESSI> GetPrevR1Ruota(List<PSD_ESTR_ESTRAZIONI> estr, int numEstrSegnale, int numSegnale, int idSegnale,string ruota, string accopiataDec, PSD_PARAMETRI_R1 paramsR1)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();
            string R1 = accopiataDec.Substring(0, 2);
            string R2 = accopiataDec.Substring(3, 2);
            List<PSD_ESTR_ESTRAZIONI> estrR1 = estr.Where(X => X.Ruota == R1 && X.Numero <= numEstrSegnale && X.Numero >= numEstrSegnale - paramsR1.COLPI.Count()).ToList();
            List<PSD_ESTR_ESTRAZIONI> estrR2 = estr.Where(X => X.Ruota == R2 && X.Numero <= numEstrSegnale && X.Numero >= numEstrSegnale - paramsR1.COLPI.Count()).ToList();
            List<PSD_ESTR_ESTRAZIONI> estrR = estr.Where(X => X.Ruota == ruota && X.Numero <= numEstrSegnale && X.Numero >= numEstrSegnale - paramsR1.COLPI.Count()).ToList();
            lstres = AddPrevisioniR1(ruota, paramsR1, R1, R2, estrR1, estrR2, estrR, false);
            //PSD_ESTR_SEGNALI segn = AddSegnaleR1(ruota, numSegnale, paramsR1, estr.Where(X => X.Ruota == ruota && X.Numero <= numEstrSegnale).FirstOrDefault(), idSegnale, 0 )
            return lstres;
        }

        /// <summary>
        /// Per ogni estrazione calcola i numeri da giocare negli N colpi successivi
        /// </summary>
        /// <param name="ruota"></param>
        /// <param name="paramsR1"></param>
        /// <param name="lstres"></param>
        /// <param name="R1"></param>
        /// <param name="R2"></param>
        /// <param name="estrR1"></param>
        /// <param name="estrR2"></param>
        /// <param name="estrR"></param>
        private static List<PSD_ESTR_SUCCESSI> AddPrevisioniR1(string ruota, PSD_PARAMETRI_R1 paramsR1
            , string R1, string R2
            , List<PSD_ESTR_ESTRAZIONI> estrR1, List<PSD_ESTR_ESTRAZIONI> estrR2, List<PSD_ESTR_ESTRAZIONI> estrR
            , bool addOnlyIfSucc)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();
            // PER OGNI RUOTA, per ogni estrazione
            foreach (PSD_ESTR_ESTRAZIONI eR1 in estrR1)
            {
                PSD_ESTR_ESTRAZIONI eR2 = estrR2.Where(X => X.IDEstrazione == eR1.IDEstrazione).FirstOrDefault();
                if (eR2 != null && eR1.IDEstrazione > 0)
                {
                    int nD, nI;
                    GetPrevDIR1(ruota, paramsR1, R1, R2, eR1, eR2, out nD, out nI);

                    foreach (int colpo in paramsR1.COLPI)
                    {
                        AddItemDIR1(ruota, lstres, estrR, eR1, nD, nI, colpo, addOnlyIfSucc);
                    }
                }
            }
            return lstres;
        }

        public static PSD_ESTR_SEGNALI AddSegnaleR1(string ruota, int numSegnale
            , DateTime dataSegnale
            , PSD_ESTR_ESTRAZIONI estrSegnale
            , int idSegnale,int idCalcolo,List<PSD_ESTR_SUCCESSI> listaPrevisioni
            //, float parValSatSoglia, int parDistOrbitSat
            )
        {
            PSD_ESTR_SEGNALI res = new PSD_ESTR_SEGNALI();

            //List<Tuple<string, object>> lstParametri = new List<Tuple<string, object>>();
            //lstParametri.Add(new Tuple<string, object>("SOGLIA SATURAZIONE", parValSatSoglia));
            //lstParametri.Add(new Tuple<string, object>("DIST SATURAZIONE", parDistOrbitSat));
            res.Data = dataSegnale;
            if (estrSegnale != null)
            {
                res.Data = estrSegnale.Data;
                res.N1 = estrSegnale.N1;
                res.N2 = estrSegnale.N2;
                res.N3 = estrSegnale.N3;
                res.N4 = estrSegnale.N4;
                res.N5 = estrSegnale.N5;
            }
            res.IDCalcolo = idCalcolo;
            res.IDSegnale = idSegnale;
            //res.ListaParametri = lstParametri;
            res.ListaPrevisioni = listaPrevisioni;
            res.Numero = estrSegnale.Numero;        
            res.NumeroSegnaleA = numSegnale;
            res.Ruota = ruota;
            return res;
        }

       

        private static void GetPrevDIR1(string ruota, PSD_PARAMETRI_R1 paramsR1
            , string R1, string R2 
            , PSD_ESTR_ESTRAZIONI eR1, PSD_ESTR_ESTRAZIONI eR2
            , out int nD, out int nI)
        {
            int posdec1 = Convert.ToInt32(paramsR1.POS_DEC_R12.First().Substring(0, 1));
            int posdec2 = Convert.ToInt32(paramsR1.POS_DEC_R12.First().Substring(1, 1));
            string d1 = ""; string d2 = "";
            switch (posdec1)
            {
                case 1: d1 = Convert.ToString(eR1.N1).PadLeft(2, '0').Substring(0, 1); break;
                case 2: d1 = Convert.ToString(eR1.N2).PadLeft(2, '0').Substring(0, 1); break;
                case 3: d1 = Convert.ToString(eR1.N3).PadLeft(2, '0').Substring(0, 1); break;
                case 4: d1 = Convert.ToString(eR1.N4).PadLeft(2, '0').Substring(0, 1); break;
                case 5: d1 = Convert.ToString(eR1.N5).PadLeft(2, '0').Substring(0, 1); break;
            }
            switch (posdec2)
            {
                case 1: d2 = Convert.ToString(eR2.N1).PadLeft(2, '0').Substring(0, 1); break;
                case 2: d2 = Convert.ToString(eR2.N2).PadLeft(2, '0').Substring(0, 1); break;
                case 3: d2 = Convert.ToString(eR2.N3).PadLeft(2, '0').Substring(0, 1); break;
                case 4: d2 = Convert.ToString(eR2.N4).PadLeft(2, '0').Substring(0, 1); break;
                case 5: d2 = Convert.ToString(eR2.N5).PadLeft(2, '0').Substring(0, 1); break;
            }
            nD = 0;
            nI = 0;
            if (paramsR1.TIPO_FREQUENZA.Contains(1) && ruota == R1)
                nD = Convert.ToInt32(d1 + d2);
            if (paramsR1.TIPO_FREQUENZA.Contains(2) && ruota == R2)
                nD = Convert.ToInt32(d1 + d2);
            if (paramsR1.TIPO_FREQUENZA.Contains(3) && ruota == R2)
                nI = Convert.ToInt32(d2 + d1);
            if (paramsR1.TIPO_FREQUENZA.Contains(4) && ruota == R1)
                nI = Convert.ToInt32(d2 + d1);
        }

        private static void AddItemDIR1(string ruota, List<PSD_ESTR_SUCCESSI> lstres, 
            List<PSD_ESTR_ESTRAZIONI> estrR, PSD_ESTR_ESTRAZIONI eR1, 
            int nD, int nI, int colpo,
            bool addOnlyIfSucc)
        {
            //List<PSD_ESTR_SUCCESSI> res = new List<PSD_ESTR_SUCCESSI>();
            PSD_ESTR_ESTRAZIONI e = null;
            if(addOnlyIfSucc)
                e = estrR.Where(x => x.Numero == eR1.Numero + colpo 
                                                 && ((nD > 0) && (x.N1 == nD || x.N2 == nD || x.N3 == nD || x.N4 == nD || x.N5 == nD) ||
                                                  (nI > 0) && (x.N1 == nI || x.N2 == nI || x.N3 == nI || x.N4 == nI || x.N5 == nI))
                                                 ).FirstOrDefault();//;ToList();
            else
                e = estrR.Where(x => x.Numero == eR1.Numero + colpo
                                                 //&& ((nD > 0) && (x.N1 == nD || x.N2 == nD || x.N3 == nD || x.N4 == nD || x.N5 == nD) ||
                                                 // (nI > 0) && (x.N1 == nI || x.N2 == nI || x.N3 == nI || x.N4 == nI || x.N5 == nI))
                                                 ).FirstOrDefault();
            if (e != null && e.IDEstrazione > 0)
            //if (le.Count > 0)
            {
                //foreach (PSD_ESTR_ESTRAZIONI e in le)
                {
                    PSD_ESTR_SUCCESSI s = new PSD_ESTR_SUCCESSI();
                    s.IDEstrazione = e.IDEstrazione;
                    s.Numero = e.Numero;
                    s.N1 = e.N1; s.N2 = e.N2; s.N3 = e.N3; s.N4 = e.N4; s.N5 = e.N5;
                    s.Ruota = ruota;
                    s.Data = e.Data;
                    s.TipoPrevisione = 1;
                    s.TipoPrevisioneDesc = "R1";
                    
                    int numA = 0;
                    if (nD > 0)
                    {
                        PSD_ESTR_SUCCESSI s1 = new PSD_ESTR_SUCCESSI();
                        Utils.CopyObjectToObject(s, s1);
                        if (nD == e.N1) { numA = e.N1; s1.Posizione = 1; }
                        else if (nD == e.N2) { numA = e.N2; s1.Posizione = 2; }
                        else if (nD == e.N3) { numA = e.N3; s1.Posizione = 3; }
                        else if (nD == e.N4) { numA = e.N4; s1.Posizione = 4; }
                        else if (nD == e.N5) { numA = e.N5; s1.Posizione = 5; }
                        s1.IsSuccesso = false;
                        if (numA > 0 || !addOnlyIfSucc)
                        {
                            s1.NumeroA = nD;
                            if(numA>0) s1.IsSuccesso = true;
                            PSD_ESTR_SUCCESSI ee = lstres.Where(X => X.Ruota == ruota && X.IDEstrazione == s.IDEstrazione && X.NumeroA == s1.NumeroA).FirstOrDefault();
                            if (ee != null)
                                ee.Occorrenze = ee.Occorrenze + 1;
                            else {
                                s1.Occorrenze = 1;
                                lstres.Add(s1);
                            }
                        }
                    }

                    if (nI > 0)
                    {
                        numA = 0;
                        PSD_ESTR_SUCCESSI s1 = new PSD_ESTR_SUCCESSI();
                        Utils.CopyObjectToObject(s, s1);
                        if (nI == e.N1) { numA = e.N1; s1.Posizione = 1; }
                        else if (nI == e.N2) { numA = e.N2; s1.Posizione = 2; }
                        else if (nI == e.N3) { numA = e.N3; s1.Posizione = 3; }
                        else if (nI == e.N4) { numA = e.N4; s1.Posizione = 4; }
                        else if (nI == e.N5) { numA = e.N5; s1.Posizione = 5; }
                        s1.IsSuccesso = false;
                        // aggiungo se voglio inserire un successo o solo la previsione
                        if (numA > 0 || !addOnlyIfSucc)
                        {
                            s1.NumeroA = nI;
                            if (numA > 0) s1.IsSuccesso = true;
                            PSD_ESTR_SUCCESSI ee = lstres.Where(X => X.Ruota == ruota && X.IDEstrazione == s.IDEstrazione && X.NumeroA == s1.NumeroA).FirstOrDefault();
                            if (ee != null)
                                ee.Occorrenze = ee.Occorrenze + 1;
                            else
                            {
                                s1.Occorrenze = 1;
                                lstres.Add(s1);
                            }
                        }                     
                    }
                }
            }
        }

        private static void AddPrevR1(string ruota, List<PSD_ESTR_SUCCESSI> lstres, List<PSD_ESTR_ESTRAZIONI> estrR, PSD_ESTR_ESTRAZIONI eR1, int nD, int nI, int colpo)
        {
            PSD_ESTR_ESTRAZIONI e = estrR.Where(x => x.Numero == eR1.Numero + colpo).FirstOrDefault();
            if (e != null && e.IDEstrazione > 0)
            //if (le.Count > 0)
            {
                //foreach (PSD_ESTR_ESTRAZIONI e in le)
                {
                    PSD_ESTR_SUCCESSI s = new PSD_ESTR_SUCCESSI();
                    s.IDEstrazione = e.IDEstrazione;
                    s.Numero = e.Numero;
                    s.N1 = e.N1; s.N2 = e.N2; s.N3 = e.N3; s.N4 = e.N4; s.N5 = e.N5;

                    s.Ruota = ruota;
                    s.Data = e.Data;
                    s.TipoPrevisione = 1;
                    s.TipoPrevisioneDesc = "R1";

                    if (nD > 0)
                    {
                        PSD_ESTR_SUCCESSI s1 = new PSD_ESTR_SUCCESSI();
                        Utils.CopyObjectToObject(s, s1);
                        s1.NumeroA = nD;
                        PSD_ESTR_SUCCESSI ee = lstres.Where(X => X.Ruota == ruota && X.IDEstrazione == s.IDEstrazione && X.NumeroA == s1.NumeroA).FirstOrDefault();
                        if (ee != null)
                            ee.Occorrenze = ee.Occorrenze + 1;
                        else
                        {
                            s1.Occorrenze = 1;
                            lstres.Add(s1);
                        }
                    }

                    if (nI > 0)
                    {
                        PSD_ESTR_SUCCESSI s1 = new PSD_ESTR_SUCCESSI();
                        Utils.CopyObjectToObject(s, s1);
                        s1.NumeroA = nI;
                        PSD_ESTR_SUCCESSI ee = lstres.Where(X => X.Ruota == ruota && X.IDEstrazione == s.IDEstrazione && X.NumeroA == s1.NumeroA).FirstOrDefault();
                        if (ee != null)
                            ee.Occorrenze = ee.Occorrenze + 1;
                        else
                        {
                            s1.Occorrenze = 1;
                            lstres.Add(s1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce i risultati del calcolo per raggruppamento voluto IN BASE A DATA ESTRAZIONE E RUOTA.
        /// Serve per capire quante volte (OCCORRENZE_GROUPBY) una tipologia di calcolo ha funzionato in una estrazione.
        /// Successivamente, dalla lista restituita, si possono sommare le occorrenze totali (per ruota, per periodo..) 
        /// usando la struttura PSD_RES_TOT
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="ruote"></param>
        /// <param name="paramsR1"></param>
        /// <param name="groupbyNum"></param>
        /// <param name="groupbyPosDec"></param>
        /// <param name="groupbyColpo"></param>
        /// <param name="groupbyTipoFreq"></param>
        /// <param name="groupbyPosEstr"></param>
        /// <returns></returns>
        public static List<PSD_RES> GetResR1_GROUPBY(DateTime dt1, DateTime dt2, List<string> ruote, PSD_PARAMETRI_R1 paramsR1, bool groupbyData, bool groupbyNum, bool groupbyPosDec = false, bool groupbyColpo = false, bool groupbyTipoFreq = false, bool groupbyPosEstr = false)
        {
            List<PSD_RES> lstres = new List<PSD_RES>();

            string where = "";
            if (ruote.Count < 11 && ruote.Count > 0)
                where = where + " AND " + DbUtils.PrepareSqlINClause<string>("r.RUOTA", ruote);
            if (paramsR1.COLPI.Count> 0 && paramsR1.COLPI.Count < 3)
                where = where + " AND " + DbUtils.PrepareSqlINClause<int>("r.NUM_ESTRAZIONE_SUCC", paramsR1.COLPI);
            if (paramsR1.TIPO_FREQUENZA.Count > 0 && paramsR1.TIPO_FREQUENZA.Count < 4)
                where = where + " AND " + DbUtils.PrepareSqlINClause<int>("r.TIPO_FREQUENZA", paramsR1.TIPO_FREQUENZA);
            if (paramsR1.POS_DEC_R12.Count > 0)
                where = where + " AND " + DbUtils.PrepareSqlINClause<string>("r.POSDECR1R2", paramsR1.POS_DEC_R12);

            string sel = "r.RUOTA";
            string grpby = " GROUP BY r.RUOTA";

            if (groupbyData)
            {
                sel = sel + ", r.DATA_EVENTO, E.IDESTRAZIONE AS ID_ESTRAZIONE";
                grpby = grpby + ",r.DATA_EVENTO, E.IDESTRAZIONE";
            }
            if (groupbyNum)
            {
                sel = sel + ",r.NUMEROA AS NUM_ESTRATTO";
                grpby = grpby + ",r.NUMEROA";
            }
            if (groupbyPosDec)
            {
                sel = sel + ",r.POSDECR1R2 AS FIELD1_STR";
                grpby = grpby + ",r.POSDECR1R2";
            }            
            if (groupbyTipoFreq)
            {
                sel = sel + ",r.TIPO_FREQUENZA AS FIELD1_INT";
                grpby = grpby + ",r.TIPO_FREQUENZA";
            }
            if (groupbyColpo)
            {
                sel = sel + ",r.NUM_ESTRAZIONE_SUCC AS FIELD2_INT";
                grpby = grpby + ",r.NUM_ESTRAZIONE_SUCC";
            }
            if (groupbyPosEstr)
            {
                sel = sel + ",r.POSIZIONE AS FIELD3_INT";
                grpby = grpby + ",r.POSIZIONE";
            }
            try
            {
                // le OCCORRENZE_GROUPBY si riferiscono alla tipologia di groupby:
                //  - normalmente raggruppo per posdecr1r2, cioè per regola in cui recupero le previsioni
                string sql = "SELECT                                        " +
                           sel +
                            "   ,count(*) AS OCCORRENZE_GROUPBY             " +
                            " FROM psd_estr_estrazioni e                    " +
                            " JOIN psd_reg_r1 r ON r.data_evento = e.Data   " +
                            "  AND r.ruota = e.ruota                        " +
                            " WHERE e.data BETWEEN @dt AND @dt              " +
                            where +
                            grpby;

                /*
                where e.data > getDate() - 30000
                --and e.ruota = 'BA'
                group by r.data_evento,r.ruota
                ; ";*/
                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_RES>(sql, dt1, dt2);
                //var lst = lstres.GroupBy(X => new { X.DATA_EVENTO, X.RUOTA, X.NUM_ESTRATTO}).ToList();
                lstres = lstres.DistinctBy(X => new { X.DATA_EVENTO, X.RUOTA, X.NUM_ESTRATTO }).ToList();
               
                //table1.GroupBy(x => x.Text).Select(x => x.FirstOrDefault());
            }
            catch { }
            return lstres;
        }

        /// <summary>
        /// Restituisce la lista dei successi dei numeri scelti per ogni ruota. 
        /// Ogni ruota può avere numeri scelti diversi l'una dall'altra.
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="ruoteNumscelti"></param>
        /// <returns></returns>
        public static List<PSD_ESTR_SUCCESSI> GetResNumScelti(DateTime dt1, DateTime dt2, Dictionary<string, List<int>> ruoteNumscelti)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();
            string where = " AND "+ GetSqlWhereNumScelti(ruoteNumscelti);
            try
            {
                string sql = "SELECT e.*                                    " +
                            " FROM psd_estr_estrazioni e                    " +
                            " WHERE e.data BETWEEN @dt AND @dt              " +
                            where + 
                            " ORDER BY e.numero,e.ruota                     ";

                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_ESTR_SUCCESSI>(sql, dt1, dt2);

                foreach (PSD_ESTR_SUCCESSI s in lstres)
                {
                    s.Occorrenze = 1;
                    if (ruoteNumscelti[s.Ruota].Where(X => X == s.N1).Count() > 0)
                    {
                        s.NumeroA = s.N1;
                        s.Posizione = 1;
                    }
                    if (ruoteNumscelti[s.Ruota].Where(X => X == s.N2).Count() > 0)
                    {
                        s.NumeroA = s.N2;
                        s.Posizione = 2;
                    }
                    if (ruoteNumscelti[s.Ruota].Where(X => X == s.N3).Count() > 0)
                    {
                        s.NumeroA = s.N3;
                        s.Posizione = 3;
                    }
                    if (ruoteNumscelti[s.Ruota].Where(X => X == s.N4).Count() > 0)
                    {
                        s.NumeroA = s.N4;
                        s.Posizione = 4;
                    }
                    if (ruoteNumscelti[s.Ruota].Where(X => X == s.N5).Count() > 0)
                    {
                        s.NumeroA = s.N5;
                        s.Posizione = 5;
                    }
                }
            }
            catch { }
            return lstres;
        }

        private static List<PSD_ESTR_SUCCESSI> GetResNumScelti2(DateTime dt1, DateTime dt2, Dictionary<string, List<int>> ruoteNumscelti)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();

            string where = " AND " + GetSqlWhereNumScelti(ruoteNumscelti, "E");

            string sel = "r.RUOTA";
            string grpby = " GROUP BY r.RUOTA";

            sel = sel + ", E.DATA, E.IDESTRAZIONE AS IDEstrazione";
            grpby = grpby + ",E.DATA, E.IDESTRAZIONE";

            try
            {
                // le OCCORRENZE_GROUPBY si riferiscono alla tipologia di groupby:
                //  - normalmente raggruppo per posdecr1r2, cioè per regola in cui recupero le previsioni
                string sql = "SELECT                                        " +
                           sel +
                            "   ,count(*) AS Occorrenze                     " +
                            " FROM psd_estr_estrazioni e                    " +
                            " WHERE e.data BETWEEN @dt AND @dt              " +
                            where +
                            grpby;

                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_ESTR_SUCCESSI>(sql, dt1, dt2);

            }
            catch { }
            return lstres;
        }

        private static string GetSqlWhereNumScelti(Dictionary<string, List<int>> ruoteNumscelti, string idtblEstr = "E")
        {
            string where = "(";
            //(e.ruota = 'BA' and (e.N1 in (1,2,3,4,5) or e.N2 in (1,2,3,4,5) or e.N3 in (1,2,3,4,5) or e.N4 in (1,2,3,4,5) or e.N5 in (1,2,3,4,5))) 
            foreach (KeyValuePair<string, List<int>> entry in ruoteNumscelti)
            {
                where = where + "(" + idtblEstr + ".RUOTA = '" + entry.Key + "' AND ";
                where = where + "(" +
                    DbUtils.PrepareSqlINClause<int>(idtblEstr + ".N1", entry.Value) + " or " +
                    DbUtils.PrepareSqlINClause<int>(idtblEstr + ".N2", entry.Value) + " or " +
                    DbUtils.PrepareSqlINClause<int>(idtblEstr + ".N3", entry.Value) + " or " +
                    DbUtils.PrepareSqlINClause<int>(idtblEstr + ".N4", entry.Value) + " or " +
                    DbUtils.PrepareSqlINClause<int>(idtblEstr + ".N5", entry.Value) + ")"
                    ;
                where = where + ") or ";
            }
            where = where.Substring(0, where.Length - 4) + ")";
            return where;
        }

        /// <summary>
        /// Restituisce la lista dei successi dei numeri scelti per tutte le ruote. 
        /// Tutte le ruote hanno gli stessi numeri scelti: serve per semplificare e velocizzare il calcolo. 
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="ruoteNumscelti"></param>
        /// <returns></returns>
        public static List<PSD_ESTR_SUCCESSI> GetResNumSceltiAllRuote(DateTime dt1, DateTime dt2, List<int> numscelti)
        {
            List<PSD_ESTR_SUCCESSI> lstres = new List<PSD_ESTR_SUCCESSI>();

            string where =  " AND (" +
                DbUtils.PrepareSqlINClause<int>("e.N1", numscelti) + " or " +
                DbUtils.PrepareSqlINClause<int>("e.N2", numscelti) + " or " +
                DbUtils.PrepareSqlINClause<int>("e.N3", numscelti) + " or " +
                DbUtils.PrepareSqlINClause<int>("e.N4", numscelti) + " or " +
                DbUtils.PrepareSqlINClause<int>("e.N5", numscelti) + ")"
                ;
            where = where + ")";
            
            try
            {
                string sql = "SELECT e.*                                    " +
                            " FROM psd_estr_estrazioni e                    " +
                            " WHERE e.data BETWEEN @dt AND @dt              " +
                            where;

                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_ESTR_SUCCESSI>(sql, dt1, dt2);

                foreach (PSD_ESTR_SUCCESSI s in lstres)
                {
                    if (numscelti.Where(X => X == s.N1).Count() > 0)
                    {
                        s.NumeroA = s.N1;
                        s.Posizione = 1;
                    }
                    if (numscelti.Where(X => X == s.N2).Count() > 0)
                    {
                        s.NumeroA = s.N2;
                        s.Posizione = 2;
                    }
                    if (numscelti.Where(X => X == s.N3).Count() > 0)
                    {
                        s.NumeroA = s.N3;
                        s.Posizione = 3;
                    }
                    if (numscelti.Where(X => X == s.N4).Count() > 0)
                    {
                        s.NumeroA = s.N4;
                        s.Posizione = 4;
                    }
                    if (numscelti.Where(X => X == s.N5).Count() > 0)
                    {
                        s.NumeroA = s.N5;
                        s.Posizione = 5;
                    }
                }
            }
            catch { }
            return lstres;
        }

        public static List<PSD_RES> GetResNumScelti_GROUPBY(DateTime dt1, DateTime dt2, Dictionary<string, List<int>> ruoteNumscelti, bool groupbyData)
        {
            List<PSD_RES> lstres = new List<PSD_RES>();

            //List<PSD_ESTR_SUCCESSI> lstResNumScelti = GetResNumScelti(dt1, dt2, ruoteNumscelti);

            string where = " AND " + GetSqlWhereNumScelti(ruoteNumscelti, "E");
           
            string sel = "r.RUOTA";
            string grpby = " GROUP BY r.RUOTA";

            if (groupbyData)
            {
                sel = sel + ", E.DATA, E.IDESTRAZIONE AS ID_ESTRAZIONE";
                grpby = grpby + ",E.DATA, E.IDESTRAZIONE";
            }

            try
            {
                // le OCCORRENZE_GROUPBY si riferiscono alla tipologia di groupby:
                //  - normalmente raggruppo per posdecr1r2, cioè per regola in cui recupero le previsioni
                string sql = "SELECT                                        " +
                           sel +
                            "   ,count(*) AS OCCORRENZE_GROUPBY             " +
                            " FROM psd_estr_estrazioni e                    " +
                            " WHERE e.data BETWEEN @dt AND @dt              " +
                            where +
                            grpby;

                DbFactory conn = new DbFactory(connectionString, providerName);
                lstres = conn.ExecuteSql<PSD_RES>(sql, dt1, dt2);
               
              
            }
            catch { }
            return lstres;
        }

        public static PSD_CALCS GetCalcByParams(PSD_CALCS calc)
        {
            List< PSD_CALCS> lstres = new List<PSD_CALCS>();
            PSD_CALCS res = new PSD_CALCS();
            string sql = "SELECT * FROM PSD_CALCS WHERE DATE_BEGIN = %u AND DATE_END = %u AND DESC_PARAMS_CALC = %s AND DESC_PARAMS_SEGN=%s ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            lstres = conn.ExecuteSql<PSD_CALCS>(sql, calc.DATE_BEGIN, calc.DATE_END, calc.DESC_PARAMS_CALC, calc.DESC_PARAMS_SEGN);
            if (lstres.Count > 0)
                res = lstres.First();
            return res;
        }

        public static List<PSD_CALCS> GetCalcs()
        {
            List<PSD_CALCS> lstres = new List<PSD_CALCS>();
            string sql = "SELECT * FROM PSD_CALCS ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            lstres = conn.ExecuteSql<PSD_CALCS>(sql);

            foreach (PSD_CALCS c in lstres)
            {
                sql = "SELECT * FROM PSD_ESTR_SEGNALI WHERE IDCalcolo=%d";
               c.LST_SEGNALI.AddRange( conn.ExecuteSql<PSD_ESTR_SEGNALI>(sql, c.IDCalcolo));
            }

            return lstres;
        }

        public static List<PSD_ESTR_SEGNALI> GetSegnali(int idcalcolo)
        {
            List<PSD_ESTR_SEGNALI> lstres = new List<PSD_ESTR_SEGNALI>();
            string sql = "SELECT * FROM PSD_ESTR_SEGNALI WHERE IDCALCOLO=%d ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            lstres = conn.ExecuteSql<PSD_ESTR_SEGNALI>(sql, idcalcolo);

            return lstres;
        }
    }
}
