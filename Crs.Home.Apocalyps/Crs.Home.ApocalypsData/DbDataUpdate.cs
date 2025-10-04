   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Crs.Home.ApocalypsData.DataEntities;
using Crs.Base.DBaseLibrary;

namespace Crs.Home.ApocalypsData
{
    public static class DbDataUpdate
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
                            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string providerName = Parameters.ProviderName;
        private static string connectionString = Parameters.GetConnStringProvider_fromJson("LOCALDB", out providerName, out string formatdateDB);


        public static void Init(string connstring, string PROVIDER)
        {
            connectionString = connstring;
            providerName = PROVIDER;
        }

        // insieme dei metodi per modifica dati tabelle db
        public static bool InsertEstrazioniRuota(ESTRAZIONI estr, bool checkIfExists)
        {
            try
            {
                string sql = "";
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                //-- viene passato nel caso della sincronizzazione del db
                int id = estr.IDEstrazione;

                if (estr.IDEstrazione <= 0)
                {
                    id = conn.GetMaxFieldVal("ESTRAZIONI", "NUMERO") + 1;
                }

                bool toins = true;
                if (checkIfExists)
                {
                    sql = "SELECT * FROM ESTRAZIONI WHERE RUOTA = %s AND DATA = %u";
                    if (conn.ExecuteSql(sql, estr.Ruota, estr.Data).Rows.Count > 0)
                    {
                        toins = false;
                    }
                }

                if (toins)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();
                        //
                        sql = "INSERT INTO ESTRAZIONI               " +
                            "(IDESTRAZIONE,NUMERO,RUOTA,DATA,N1,N2,N3,N4,N5)    " +
                            "VALUES (%d,%d,%s,%u,%d,%d,%d,%d,%d)                ";
                        res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, id, id, estr.Ruota, estr.Data, estr.N1, estr.N2, estr.N3, estr.N4, estr.N5);


                        if (res1)
                            transaction.Commit();
                        else transaction.Rollback();
                    }
                }

                return res1;
            }
            catch (Exception ex)
            {
                log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }

        public static bool InsertEstrazioniRuota(Estrazione estr, bool checkIfExists)
        {
            try
            {
                string sql = "";
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                //-- viene passato nel caso della sincronizzazione del db
                
                bool toins = true;
                if (checkIfExists)
                {
                    sql = "SELECT * FROM ESTRAZIONI WHERE RUOTA = %s AND DATA = %u";
                    if (conn.ExecuteSql(sql, estr.Ruota, estr.Data).Rows.Count > 0)
                    {
                        toins = false;
                    }
                }

                if (toins)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();
                        //
                        sql = "INSERT INTO ESTRAZIONI               " +
                            "(IDESTRAZIONE,NUMERO,RUOTA,DATA,N1,N2,N3,N4,N5)    " +
                            "VALUES (%d,%d,%s,%u,%d,%d,%d,%d,%d)                ";
                        res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, estr.SeqAnno, estr.SeqAnno, estr.Ruota, estr.Data
                            , estr.Numeri[0], estr.Numeri[1], estr.Numeri[2], estr.Numeri[3], estr.Numeri[4]);


                        if (res1)
                            transaction.Commit();
                        else transaction.Rollback();
                    }
                }

                return res1;
            }
            catch (Exception ex)
            {
                log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }

        public static bool InsertEstrazioni(LOTTO estr, bool checkIfExists)
        {
            try
            {
                string sql = "";
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;
                //-- viene passato nel caso della sincronizzazione del db
                int id = estr.Numero;

                if (estr.Numero <= 0)
                {
                    id = conn.GetMaxFieldVal("LOTTO", "NUMERO") + 1;
                }

                bool toins = true;
                if (checkIfExists)
                {
                    sql = "SELECT * FROM LOTTO WHERE DATA = %u";
                    if (conn.ExecuteSql(sql, estr.Data).Rows.Count > 0)
                    {
                        toins = false;
                    }
                }

                if (toins)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();
                        //
                        sql = "INSERT INTO LOTTO                       " +
                            "(IDESTRAZIONE,NUMERO,DATA                          " +
                            "   ,BA1,BA2,BA3,BA4,BA5                            " +
                            "   ,CA1,CA2,CA3,CA4,CA5                            " +
                            "   ,FI1,FI2,FI3,FI4,FI5                            " +
                            "   ,GE1,GE2,GE3,GE4,GE5                            " +
                            "   ,MI1,MI2,MI3,MI4,MI5                            " +
                            "   ,NA1,NA2,NA3,NA4,NA5                            " +
                            "   ,PA1,PA2,PA3,PA4,PA5                            " +
                            "   ,RM1,RM2,RM3,RM4,RM5                            " +
                            "   ,TO1,TO2,TO3,TO4,TO5                            " +
                            "   ,VE1,VE2,VE3,VE4,VE5                            " +
                            "   ,NZ1,NZ2,NZ3,NZ4,NZ5                            " +
                            ")                                                  " +
                            "VALUES (%d,%d,%u                                   " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            "   ,%d,%d,%d,%d,%d                                 " +
                            ")                ";
                        res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction,sql, id, id, estr.Data,
                            estr.BA1, estr.BA2, estr.BA3, estr.BA4, estr.BA5,
                            estr.CA1, estr.CA2, estr.CA3, estr.CA4, estr.CA5,
                            estr.FI1, estr.FI2, estr.FI3, estr.FI4, estr.FI5,
                            estr.GE1, estr.GE2, estr.GE3, estr.GE4, estr.GE5,
                            estr.MI1, estr.MI2, estr.MI3, estr.MI4, estr.MI5,
                            estr.NA1, estr.NA2, estr.NA3, estr.NA4, estr.NA5,
                            estr.PA1, estr.PA2, estr.PA3, estr.PA4, estr.PA5,
                            estr.RM1, estr.RM2, estr.RM3, estr.RM4, estr.RM5,
                            estr.TO1, estr.TO2, estr.TO3, estr.TO4, estr.TO5,
                            estr.VE1, estr.VE2, estr.VE3, estr.VE4, estr.VE5,
                            estr.NZ1, estr.NZ2, estr.NZ3, estr.NZ4, estr.NZ5
                            );


                        if (res1)
                            transaction.Commit();
                        else transaction.Rollback();
                    }
                }

                return res1;
            }
            catch (Exception ex)
            {
                log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }

        public static bool InsertGemelli(ESTRAZIONI estr, bool checkIfExists)
        {
            try
            {
                string sql = "";
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;

                List<int> gem = new List<int>();

                if (estr.N1 > 10 && estr.N1.ToString().Substring(0, 1) == estr.N1.ToString().Substring(1))
                    gem.Add(estr.N1);
                if (estr.N2 > 10 && estr.N2.ToString().Substring(0, 1) == estr.N2.ToString().Substring(1))
                    gem.Add(estr.N2);
                if (estr.N3 > 10 && estr.N3.ToString().Substring(0, 1) == estr.N3.ToString().Substring(1))
                    gem.Add(estr.N3);
                if (estr.N4 > 10 && estr.N4.ToString().Substring(0, 1) == estr.N4.ToString().Substring(1))
                    gem.Add(estr.N4);
                if (estr.N5 > 10 && estr.N5.ToString().Substring(0, 1) == estr.N5.ToString().Substring(1))
                    gem.Add(estr.N5);

                bool toins = true;
                if (checkIfExists)
                {
                    sql = "SELECT * FROM GEMELLI WHERE DATA = %u AND RUOTA=%s";
                    if (conn.ExecuteSql(sql, estr.Data, estr.Ruota).Rows.Count > 0)
                    {
                        toins = false;
                    }
                }

                if (toins)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();
                        //
                        sql = "INSERT INTO GEMELLI     " +
                            "(RUOTA,GEMELLO,DATA)               " +
                            "VALUES (%s,%d,%u)                  ";

                        foreach (int g in gem)
                        {
                            res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, estr.Ruota, g, estr.Data);
                            if (res1)
                                transaction.Commit();
                            else transaction.Rollback();
                        }
                    }
                }

                return res1;
            }
            catch (Exception ex)
            {
                log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }

        public static bool InsertGemelli(Estrazione estr, bool checkIfExists)
        {
            try
            {
                string sql = "";
                DbFactory conn = new DbFactory(connectionString, providerName);

                bool res1 = false;

                List<int> gem = new List<int>();

                int N1 = estr.Numeri[0];
                int N2 = estr.Numeri[1];            
                int N3 = estr.Numeri[2];
                int N4 = estr.Numeri[3];
                int N5 = estr.Numeri[4];

                if (N1 > 10 && N1.ToString().Substring(0, 1) == N1.ToString().Substring(1))
                    gem.Add(N1);
                if (N2 > 10 && N2.ToString().Substring(0, 1) == N2.ToString().Substring(1))
                    gem.Add(N2);
                if (N3 > 10 && N3.ToString().Substring(0, 1) == N3.ToString().Substring(1))
                    gem.Add(N3);
                if (N4 > 10 && N4.ToString().Substring(0, 1) == N4.ToString().Substring(1))
                    gem.Add(N4);
                if (N5 > 10 && N5.ToString().Substring(0, 1) == N5.ToString().Substring(1))
                    gem.Add(N5);

                bool toins = true;
                if (checkIfExists)
                {
                    sql = "SELECT * FROM GEMELLI WHERE DATA = %u AND RUOTA=%s";
                    if (conn.ExecuteSql(sql, estr.Data, estr.Ruota).Rows.Count > 0)
                    {
                        toins = false;
                    }
                }

                if (toins)
                {
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();
                        //
                        sql = "INSERT INTO GEMELLI     " +
                            "(RUOTA,GEMELLO,DATA)               " +
                            "VALUES (%s,%d,%u)                  ";

                        foreach (int g in gem)
                        {
                            res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, estr.Ruota, g, estr.Data);
                            if (res1)
                                transaction.Commit();
                            else transaction.Rollback();
                        }
                    }
                }

                return res1;
            }
            catch (Exception ex)
            {
                log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }


        public static void ImportDB(List<ESTRAZIONI> estr)
        {
            try
            {
                DateTime lastdt = DbDataAccess.GetLastDataEstrImported();
                List<ESTRAZIONI> lstestr = estr.Where(X => X.Data > lastdt).ToList();

                foreach (ESTRAZIONI e in lstestr)
                {
                    DbDataUpdate.InsertEstrazioniRuota(e, false);
                    DbDataUpdate.InsertGemelli(e, false);
                }

                lastdt = DbDataAccess.GetLastDataLottoImported();
                lstestr = estr.Where(X => X.Data > lastdt).ToList();
                List<LOTTO> lotto = DbDataAccess.EstrToLotto(lstestr);
                foreach (LOTTO l in lotto)
                    DbDataUpdate.InsertEstrazioni(l, false);

            }
            catch { }
            //return res;
        }

        public static bool ImportDB(List<Estrazione> estr)
        {
            bool res = true;
            try
            {
                DateTime lastdt = DbDataAccess.GetLastDataEstrImported();
                List<Estrazione> lstestr = estr.Where(X => X.Data > lastdt).ToList();

                foreach (Estrazione e in lstestr)
                {
                    bool re =DbDataUpdate.InsertEstrazioniRuota(e, false);
                    bool rg = DbDataUpdate.InsertGemelli(e, false);

                    if(!re || !rg) res = false;
                }

                bool resl = true;
                lastdt = DbDataAccess.GetLastDataLottoImported();
                lstestr = estr.Where(X => X.Data > lastdt).ToList();
                List<LOTTO> lotto = DbDataAccess.EstrToLotto(lstestr);
                foreach (LOTTO l in lotto)
                {
                    bool r = DbDataUpdate.InsertEstrazioni(l, false);
                    if(!r) resl = false;
                }

                res = res && resl;

            }
            catch { res = false; }
            return res;
        }


        public static bool InsertR1(ESTRAZIONI estrToSearch, int num1, int tipoFreq, int colpo, int posDecR1R2, string acc)
        {
            DbFactory conn = new DbFactory(connectionString, providerName);
            bool res1 = false;
            try
            {
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    //
                    string sql = "INSERT INTO PSD_REG_R1               " +
                        "(DATA_EVENTO,RUOTA,NUMEROA             " +
                        "   ,TIPO_FREQUENZA,NUM_ESTRAZIONE_SUCC " +
                        "   ,ACCOPPIATA,POSIZIONE,POSDECR1R2 )  " +
                        "VALUES (%u,%s,%d,%d,%d,%s,%d,%s)       ";

                    /*int acc = 1;
                    switch(estrToSearch.Ruota){
                        case "FI": case "GE": acc = 2; break;
                        case "MI": case "NA": acc = 3; break;
                        case "PA": case "RM": acc = 4; break;
                        case "TO": case "VE": acc = 5; break;
                    }*/

                    int pos = 1;
                    if (num1 == estrToSearch.N2) pos = 2;
                    if (num1 == estrToSearch.N3) pos = 3;
                    if (num1 == estrToSearch.N4) pos = 4;
                    if (num1 == estrToSearch.N5) pos = 5;

                    res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, estrToSearch.Data, estrToSearch.Ruota, num1, tipoFreq, colpo, acc, pos, posDecR1R2);
                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
            }
            catch { }

            return res1;
        }

        public static int InsertCalcInDB(
            PSD_CALCS calcs, out bool exists)
        //DateTime dt1, DateTime dt2, string strres, List<Tuple<string, object>> paramsCalc, List<Tuple<string, object>> paramsSegn)
        {
            List<Tuple<string, object>> lstParametri = new List<Tuple<string, object>>();
            string sql =
                "INSERT INTO PSD_CALCS          " +
                "(DESC_RES, DESC_PARAMS_CALC,   " +
                " DESC_PARAMS_SEGN, DATE_BEGIN, DATE_END) " +
                " VALUES (%s,%s,%s,%u,%u)    ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            exists = false;
            int idcalc = 0;
            try
            {
                idcalc = DbDataAccess.GetCalcByParams(calcs).IDCalcolo;
                if (idcalc == 0)
                {
                    exists = false;
                    using (DbConnection dbconn = conn.GetConnection())
                    {
                        dbconn.Open();
                        DbTransaction transaction = dbconn.BeginTransaction();

                        string p1 = string.Join(", ", calcs.LST_PARAMETRI_CALCOLO.Select(X => X.Item1 + "=" + X.Item2.ToString()).ToList());
                        string p2 = string.Join(", ", calcs.LST_PARAMETRI_SEGNALE.Select(X => X.Item1 + "=" + X.Item2.ToString()).ToList());

                        bool res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, calcs.DESC_RES, p1, p2, calcs.DATE_BEGIN, calcs.DATE_END);
                        if (res1)
                            transaction.Commit();
                        else transaction.Rollback();

                        sql =
                            "SELECT * FROM PSD_CALCS           " +
                            "WHERE DESC_PARAMS_CALC=%s AND DESC_PARAMS_SEGN=%s AND DATE_BEGIN =%u AND DATE_END = %u ";
                        List<PSD_CALCS> c = conn.ExecuteSql<PSD_CALCS>(sql, p1, p2, calcs.DATE_BEGIN, calcs.DATE_END);
                        if (c.Count > 0)
                            idcalc = c.First().IDCalcolo;
                    }
                }
                else exists = true;
            }
            catch { }

            return idcalc;
        }

        public static bool UpdateResCalcInDB(
            PSD_CALCS calcs)
        //DateTime dt1, DateTime dt2, string strres, List<Tuple<string, object>> paramsCalc, List<Tuple<string, object>> paramsSegn)
        {
            List<Tuple<string, object>> lstParametri = new List<Tuple<string, object>>();
            string sql =
                "UPDATE PSD_CALCS SET       " +
                "   DESC_RES=%s             " + 
                "   ,DATE_END=%u            " +
                "WHERE IDCALCOLO = %d       ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            bool res1 = false;

            try
            {
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, calcs.DESC_RES, calcs.DATE_END, calcs.IDCalcolo);
                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
            }
            catch { }

            return res1;
        }

        public static bool InsertSegnale(PSD_ESTR_SEGNALI segn)
        {
            bool res = false;
            string sql =
                "INSERT INTO PSD_ESTR_SEGNALI                       " +
                "(NumeroSegnaleA, NumeroSegnaleB,NumeroSegnaleC     " +
                " IDCalcolo,IDEstrazione)                           " +
                " VALUES (%d,%d,%d,%d,%d)                           ";
            DbFactory conn = new DbFactory(connectionString, providerName);

            try
            {
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res = conn.ExecNonQueryWithTransaction(dbconn, transaction, sql, segn.NumeroSegnaleA, segn.NumeroSegnaleB, segn.NumeroSegnaleC, segn.IDCalcolo, segn.IDEstrazione);
                    if (res)
                        transaction.Commit();
                    else transaction.Rollback();
                }
            }
            catch { }

            return res;
        }

        public static bool DeleteCalcInDB(
           PSD_CALCS calcs)
        {
            List<Tuple<string, object>> lstParametri = new List<Tuple<string, object>>();
            string sql =
                "DELETE FROM PSD_CALCS      " +
                "WHERE IDCALCOLO = %d       ";
            DbFactory conn = new DbFactory(connectionString, providerName);
            bool res1 = false;

            try
            {
                using (DbConnection dbconn = conn.GetConnection())
                {
                    dbconn.Open();
                    DbTransaction transaction = dbconn.BeginTransaction();
                    res1 = conn.ExecNonQueryWithTransaction(dbconn, transaction,sql, calcs.IDCalcolo);
                    if (res1)
                        transaction.Commit();
                    else transaction.Rollback();
                }
            }
            catch { }

            return res1;
        }

    }
}
