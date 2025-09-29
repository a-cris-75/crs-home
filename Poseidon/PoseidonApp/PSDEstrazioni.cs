using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using PoseidonData.DataEntities;
using PoseidonData;
using CRS.CommonControlsLib;

namespace PoseidonApp
{
    public static class PSDEstrazioni
    {
        public static List<PSD_ESTR_ESTRAZIONI> ReadFileEstr(string filename, int filetype)
        {
            List<PSD_ESTR_ESTRAZIONI> res = new List<PSD_ESTR_ESTRAZIONI>();
            try
            {
                List<string> file = File.ReadAllLines(filename).ToList<string>();
                // file con data ruota e 5 estratti
                if (filetype == COSTANTS.TYPE_FILE_ESTRAZIONI_2_DATA_RUOTA_ESTR)
                {
                    foreach (string s in file)
                    {
                        string stmp = s.Substring(0, s.IndexOf('\t'));
                        string[] lst = s.Split(new Char[] { ' ', ',', ';','\t' }, StringSplitOptions.RemoveEmptyEntries);

                        PSD_ESTR_ESTRAZIONI e = new PSD_ESTR_ESTRAZIONI();

                        string pattern = "yyyy/MM/dd";

                        DateTime dt;
                        if (DateTime.TryParseExact(lst[0], pattern, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                            e.Data = dt;
                        else
                        {
                            ABSMessageBox.Show("Formato data non corretto!", "ERR");
                            break;
                        }

                        e.Ruota = lst[1];
                        if (COSTANTS.RUOTA_NAZIONALE.Contains(e.Ruota))
                            e.Ruota = "NZ";
                        e.N1 = Convert.ToInt32(lst[2]);
                        e.N2 = Convert.ToInt32(lst[3]);
                        e.N3 = Convert.ToInt32(lst[4]);
                        e.N4 = Convert.ToInt32(lst[5]);
                        e.N5 = Convert.ToInt32(lst[6]);
                        res.Add(e);
                    }

                    res = RecalcNumEstr(res);
                }

                // file con data, numero estrazione,e 55 numeri estratti (per 10+1 ruote) senza identificativo di ruota
                if (filetype == COSTANTS.TYPE_FILE_ESTRAZIONI_1_TUTTE_LE_RUOTE)
                {
                    foreach (string s in file)
                    {
                        string stmp = s.Substring(0, s.IndexOf(' '));
                        string[] lst = s.Split(new Char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                        PSD_ESTR_ESTRAZIONI e = new PSD_ESTR_ESTRAZIONI();
                        e.Data = Convert.ToDateTime(lst[0]);
                        e.IDEstrazione = Convert.ToInt32(lst[1]);
                        e.Numero = e.IDEstrazione;
                        for (int i = 1; i <= 55; i++)
                        {
                            int r = (int)((i-1) / 5) + 1;
                            e.Ruota = COSTANTS.RUOTE[r];
                            e.N1 = Convert.ToInt32(lst[2]);
                            e.N2 = Convert.ToInt32(lst[3]);
                            e.N3 = Convert.ToInt32(lst[4]);
                            e.N4 = Convert.ToInt32(lst[5]);
                            e.N5 = Convert.ToInt32(lst[6]);
                            res.Add(e);
                        }
                    }
                }      
            }
            catch { }
            return res;
        }

        public static List<PSD_ESTR_ESTRAZIONI> RecalcNumEstr(List<PSD_ESTR_ESTRAZIONI> estr)
        {
            try
            {
                estr = estr.OrderBy(X => X.Data).ToList();
                PSD_ESTR_ESTRAZIONI[] aestr = estr.ToArray<PSD_ESTR_ESTRAZIONI>();
                int idx = 0;
                int num = 1;
                foreach (PSD_ESTR_ESTRAZIONI e in estr)
                {
                    if (idx>0 && e.Data != aestr[idx-1].Data)
                        num++;

                    e.IDEstrazione = num;
                    e.Numero = num;
                    idx++;
                }

            }
            catch { }

            return estr;
        }

        public static void ImportDB(List<PSD_ESTR_ESTRAZIONI> estr)
        {           
            try
            {
                DateTime lastdt = DbDataAccess.GetLastDataEstrImported();
                List<PSD_ESTR_ESTRAZIONI> lstestr = estr.Where(X => X.Data > lastdt).ToList();

                foreach (PSD_ESTR_ESTRAZIONI e in lstestr)
                {
                    DbDataUpdate.InsertEstrazioniRuota(e, false);
                    DbDataUpdate.InsertGemelli(e, false);
                }

                lastdt = DbDataAccess.GetLastDataLottoImported();
                lstestr = estr.Where(X => X.Data > lastdt).ToList();
                List<PSD_ESTR_LOTTO> lotto = EstrToLotto(lstestr);
                foreach (PSD_ESTR_LOTTO l in lotto)
                    DbDataUpdate.InsertEstrazioni(l, false);

            }
            catch { }
            //return res;
        }

        private static List<PSD_ESTR_LOTTO> EstrToLotto(List<PSD_ESTR_ESTRAZIONI> estr)
        {
            List<PSD_ESTR_LOTTO> res = new List<PSD_ESTR_LOTTO>();
            DateTime dtprev = DateTime.MinValue;
            //bool isfirst = true;
            PSD_ESTR_LOTTO l = new PSD_ESTR_LOTTO();

            estr = estr.OrderBy(X => X.Data).ToList();
            PSD_ESTR_ESTRAZIONI[] aestr = estr.ToArray<PSD_ESTR_ESTRAZIONI>();

            int idx = 0;
            foreach (PSD_ESTR_ESTRAZIONI e in estr)
            {               
                l.Data = e.Data;
                l.Numero = e.Numero;
               
                switch (e.Ruota)
                {
                    case "BA":
                        l.BA1 = e.N1;
                        l.BA2 = e.N2;
                        l.BA3 = e.N3;
                        l.BA4 = e.N4;
                        l.BA5 = e.N5;         
                        break;
                    case "CA":
                        l.CA1 = e.N1;
                        l.CA2 = e.N2;
                        l.CA3 = e.N3;
                        l.CA4 = e.N4;
                        l.CA5 = e.N5;
                        break;
                    case "FI":
                        l.FI1 = e.N1;
                        l.FI2 = e.N2;
                        l.FI3 = e.N3;
                        l.FI4 = e.N4;
                        l.FI5 = e.N5;
                        break;
                    case "GE":
                        l.GE1 = e.N1;
                        l.GE2 = e.N2;
                        l.GE3 = e.N3;
                        l.GE4 = e.N4;
                        l.GE5 = e.N5;
                        break;
                    case "MI":
                        l.MI1 = e.N1;
                        l.MI2 = e.N2;
                        l.MI3 = e.N3;
                        l.MI4 = e.N4;
                        l.MI5 = e.N5;
                        break;
                    case "NA":
                        l.NA1 = e.N1;
                        l.NA2 = e.N2;
                        l.NA3 = e.N3;
                        l.NA4 = e.N4;
                        l.NA5 = e.N5;
                        break;
                    case "PA":
                        l.PA1 = e.N1;
                        l.PA2 = e.N2;
                        l.PA3 = e.N3;
                        l.PA4 = e.N4;
                        l.PA5 = e.N5;
                        break;
                    case "RM":
                        l.RM1 = e.N1;
                        l.RM2 = e.N2;
                        l.RM3 = e.N3;
                        l.RM4 = e.N4;
                        l.RM5 = e.N5;
                        break;
                    case "TO":
                        l.TO1 = e.N1;
                        l.TO2 = e.N2;
                        l.TO3 = e.N3;
                        l.TO4 = e.N4;
                        l.TO5 = e.N5;
                        break;
                    case "VE":
                        l.VE1 = e.N1;
                        l.VE2 = e.N2;
                        l.VE3 = e.N3;
                        l.VE4 = e.N4;
                        l.VE5 = e.N5;
                        break;
                    case "NZ":
                        l.NZ1 = e.N1;
                        l.NZ2 = e.N2;
                        l.NZ3 = e.N3;
                        l.NZ4 = e.N4;
                        l.NZ5 = e.N5;
                        break;
                }
                /*
                if (e.Ruota == "VE")
                {
                    res.Add(l);
                    l = new PSD_ESTR_LOTTO();
                }*/

                if (aestr.Count() > idx + 1)
                {
                    if (aestr[idx + 1].Data != e.Data)
                    {
                        res.Add(l);
                        l = new PSD_ESTR_LOTTO();
                    }
                }
                else
                {
                    res.Add(l);
                    l = new PSD_ESTR_LOTTO();
                }

                idx ++;
            }
            return res;
        }

        private static int GetPosAccoppiata(string ruota)
        {
            int res = 2;
            if (ruota == "BA" || ruota == "FI" || ruota == "MI" || ruota == "PA" || ruota == "TO")
                res = 1;
            if (ruota == "NZ") res = 2;
            return res;
        }

        /// <summary>
        /// Inserisce nel db tutte le estrazioni vincenti date dalla regola1. 
        /// R1: metto insieme la decina di un estratto (N1..N5) della prima ruota con la decina della seconda ruota.  
        /// Determina i numeri della R1 dalle ruote adiacenti (1-2,2-3,3-4..) per tutte le decine (1-1,1-2..5-5).
        /// Calcola i numeri vincenti par al max 3 colpi.
        /// </summary>
        /// <param name="estr">Contiene tutte le estrazioni di tutte le ruote</param>
        public static void ImportR1(List<PSD_ESTR_ESTRAZIONI> estr)
        {
            PSD_ESTR_ESTRAZIONI estrSourceR1 = new PSD_ESTR_ESTRAZIONI();
            PSD_ESTR_ESTRAZIONI estrSourceR2 = new PSD_ESTR_ESTRAZIONI();
            try
            {
                DateTime lastdt = DbDataAccess.GetLastDataR1Imported();
                estr = estr.Where(X => X.Data > lastdt).ToList();

                foreach(string r in COSTANTS.RUOTE_INT.Keys)
                {
                    List<PSD_ESTR_ESTRAZIONI> estrR1 = estr.Where(X => X.Ruota == r).ToList();
                    int numr = COSTANTS.RUOTE_INT[r];
                    // arrivo fino alla ruota 11
                    if (numr <= 10)
                    {
                        // indice a partire da 0 => COSTANTS.RUOTE[numr]
                        List<PSD_ESTR_ESTRAZIONI> estrR2 = estr.Where(X => X.Ruota == COSTANTS.RUOTE[numr]).ToList();

                        for (int pos1 = 1; pos1 <= 5; pos1++)
                        {
                            for (int pos2 = 1; pos2 <= 5; pos2++)
                            {
                                int posdec = pos1 * 10 + pos2;

                                ImportR1Ruote(estrR1, estrR2, posdec, 3);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private static void ImportR1Ruote(List<PSD_ESTR_ESTRAZIONI> estrSourceR1, List<PSD_ESTR_ESTRAZIONI> estrSourceR2, int posDec, int colpi)
        {
            try
            {
                List<PSD_ESTR_ESTRAZIONI> estrSearch1 = estrSourceR1;
                List<PSD_ESTR_ESTRAZIONI> estrSearch2 = estrSourceR2;

                string ruota2 = "";
                if(estrSourceR2.Count>0)
                    ruota2 = estrSourceR2.First().Ruota;
                
                if(!string.IsNullOrEmpty(ruota2))
                {
                    foreach (PSD_ESTR_ESTRAZIONI e1 in estrSourceR1)
                    {
                        PSD_ESTR_ESTRAZIONI e2 = new PSD_ESTR_ESTRAZIONI();
                        // nel caso non ci siano dati calcolo ugualmente la R1 sulla prima ruota
                        e2.Data = e1.Data;
                        e2.Ruota = ruota2;
                        e2.IDEstrazione = e1.IDEstrazione;
                        e2.Numero = e1.Numero;
                        e2.N1 = 0;
                        e2.N2 = 0;
                        e2.N3 = 0;
                        e2.N4 = 0;
                        e2.N5 = 0;
                        // nelle prime estrazioni non ci sono tutte le ruote
                        if (estrSourceR2.Where(X => X.Data == e1.Data).Count() > 0)
                        {
                            e2 = estrSourceR2.Where(X => X.Data == e1.Data).First();
                        }
                        for (int colpo = 1; colpo <= colpi; colpo++)
                        {
                            PSD_ESTR_ESTRAZIONI eSearch = new PSD_ESTR_ESTRAZIONI();
                            if (estrSearch1.Where(X => X.Numero == e1.Numero + colpo).Count() > 0)
                            {
                                eSearch = estrSearch1.Where(X => X.Numero == e1.Numero + colpo).First();
                                // inserisce l'estratto vincente cercandolo sulla prima ruota
                                SetSolutionR1(e1, e2, posDec, eSearch);
                            }

                            if (estrSearch2.Where(X => X.Numero == e1.Numero + colpo).Count() > 0)
                            {
                                eSearch = estrSearch2.Where(X => X.Numero == e1.Numero + colpo).First();
                                // inserisce l'estratto vincente cercandolo sulla seconda ruota
                                SetSolutionR1(e1, e2, posDec, eSearch);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private static bool SetSolutionR1(PSD_ESTR_ESTRAZIONI estrSourceR1, PSD_ESTR_ESTRAZIONI estrSourceR2, int posDec, PSD_ESTR_ESTRAZIONI estrToSearch)
        {
            try
            {
                bool res1 = false;
                bool res2 = false;

                int p1 = posDec / 10;//Convert.ToInt32(pos.ToString().Substring(0,1));
                int p2 = posDec % 10;

                int nr1 = estrSourceR1.N1;
                switch (p1)
                {
                    case 2: nr1 = estrSourceR1.N2; break;
                    case 3: nr1 = estrSourceR1.N3; break;
                    case 4: nr1 = estrSourceR1.N4; break;
                    case 5: nr1 = estrSourceR1.N5; break;
                }

                int nr2 = estrSourceR2.N1;
                switch (p2)
                {
                    case 2: nr2 = estrSourceR2.N2; break;
                    case 3: nr2 = estrSourceR2.N3; break;
                    case 4: nr2 = estrSourceR2.N4; break;
                    case 5: nr2 = estrSourceR2.N5; break;
                }

                int d1 = (nr1 / 10);
                int d2 = (nr2 / 10);

                int dir = d1 * 10 + d2;
                int ind = d2 * 10 + d1;

                if (dir > 90) dir = 90;
                if (ind > 90) ind = 90;

                if (dir == ind) ind = -1;
             
                int colpo = estrToSearch.Numero - estrSourceR1.Numero;
                string acc = estrSourceR1.Ruota + "-" + estrSourceR2.Ruota;
                int tipofreq = PoseidonApp.COSTANTS.TIPO_FREQ_DIR1;
                if (dir == estrToSearch.N1 || dir == estrToSearch.N2 || dir == estrToSearch.N3 || dir == estrToSearch.N4 || dir == estrToSearch.N5)
                {
                    if(estrToSearch.Ruota == estrSourceR2.Ruota)
                        tipofreq = PoseidonApp.COSTANTS.TIPO_FREQ_DIR2;
                    res1 = DbDataUpdate.InsertR1(estrToSearch, dir, tipofreq, colpo, posDec,acc);   
                }

                tipofreq = PoseidonApp.COSTANTS.TIPO_FREQ_IND1;
                if (ind == estrToSearch.N1 || ind == estrToSearch.N2 || ind == estrToSearch.N3 || ind == estrToSearch.N4 || ind == estrToSearch.N5)
                {
                    if(estrToSearch.Ruota == estrSourceR2.Ruota)
                        tipofreq = PoseidonApp.COSTANTS.TIPO_FREQ_IND2;
                    res2 = DbDataUpdate.InsertR1(estrToSearch, ind, tipofreq, colpo, posDec, acc);
                }

                return res1 || res2;
            }
            catch 
            {
                //log.Error("Error UpdateEstrazioni: " + ex.Message);
                return false;
            }
        }

    }
}
