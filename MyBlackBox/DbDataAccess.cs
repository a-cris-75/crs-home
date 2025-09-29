using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRS.Library;
using MyBlackBoxCore.DataEntities;
using System.Data;
using CRS.DBLibrary;

namespace MyBlackBoxCore
{
    public static class DbDataAccess
    {
        private static string connectionString;
        private static string providerName;

        public static void Init(string connstring, string provider)
        {
            connectionString = connstring;
            providerName = provider;
        }

        public static int GetSizeFieldTextMsg()
        {
            try
            {
                int res = 0;
                DbFactory conn = new DbFactory(connectionString, providerName);

                DataTable t = conn.GetSchemaInfo("note" );
                DataColumn c = t.Columns["Text"];
                res = c.MaxLength;
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetSizeFieldTextMsg: " + ex.Message, "");
                return -1;
            }
        }

        public static List<DbNote> GetNotesOR(List<string> keys)
        {
            try
            {
                string w = "";
                foreach(string s in keys){
                    w= w + " or k.name=@s ";
                }
                if(string.IsNullOrEmpty(w)==false) 
                    w=w.Substring(4);

                List<DbNote> res = new List<DbNote>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNote>(
                            " select distinct n.* from note n "+
                            " left outer join note_key k on k.idnote=n.id " +
                            " where " + w);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNotes: " + ex.Message,"");
                return null;
            }
        }

        /// <summary>
        /// Restituisce info sul documento gestito dall'utente. E' utile per sapere se l'utente ha già creato
        /// una nota con questo documento, in altre parole se il documento è già stato versionato per l'utente.
        /// Presuppongo che un utente gestisca un solo file con un determinato nome.
        /// In altre parole il nome del file (anche se presente in più cartelle) è univoco per l'utente.
        /// </summary>
        /// <param name="docname"></param>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public static DbNoteDoc GetDocInfo(string docname, int iduser/*, int idnote*/)
        {
            try
            {
                // ATTENZIONE: condivido una nota, quindi tutti i documenti di una nota
                // se intendo condividere solo un documento devo creare una nota apposita
                List<DbNoteDoc> reslst = new List<DbNoteDoc>();
                DbNoteDoc res = new DbNoteDoc();
                DbFactory conn = new DbFactory(connectionString, providerName);
                reslst = conn.GetListData<DbNoteDoc>(
                            " select d.*, n.IdUser as IDUserNoteOwner,u.Name as UserNoteOwner   " +
                            " from note_doc d                                                   " +                          
                            " join note n on n.id=d.idnote and n.iduser=d.iduser                " +
                            " left outer join note_sharing s on s.idnote=n.id                   " +
                            "   and  n.iduser=s.iduser                                          " +
                            " left outer join userapp u on u.id=n.idUser                        " +
                            " where d.docname=@s and (n.iduser=@n or s.idusersharing = @n)      " +
                            //"   and d.datetimeLastMod > "+
                            " order by d.version desc                                           "
                            ,docname, iduser, iduser);

                if (reslst.Count > 0)
                    res = reslst.First();

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetDocInfo: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Restituisce info sul documento gestito dall'utente. Il documento in questo caso è riferito alla nota 
        /// quindi il nome del file non è necessariamete univoco per l'utente. In altre parole file con lo stesso nome ma 
        /// presenti in cartelle diverse sono considerati diversi.
        /// </summary>
        /// <param name="docname"></param>
        /// <param name="iduser"></param>
        /// <param name="idnote"></param>
        /// <returns></returns>
        public static DbNoteDoc GetDocInfo(string docname, int iduser, Int64 idnote)
        {
            try
            {
                // ATTENZIONE: condivido una nota, quindi tutti i documenti di una nota
                // se intendo condividere solo un documento devo creare una nota apposita
                List<DbNoteDoc> reslst = new List<DbNoteDoc>();
                DbNoteDoc res = new DbNoteDoc();
                DbFactory conn = new DbFactory(connectionString, providerName);
                reslst = conn.GetListData<DbNoteDoc>(
                            " select d.*, n.IdUser as IDUserNoteOwner,u.Name as UserNoteOwner   " +
                            " from note_doc d                                                   " +
                            " join note n on n.id=d.idnote and n.iduser=d.iduser                " +
                            " left outer join note_sharing s on s.idnote=n.id                   " +
                            "  and n.iduser=s.iduser                                            " +
                            " left outer join userapp u on u.id=n.idUser                        " +
                            " where d.docname=@s and (n.iduser=@n or s.idusersharing = @n)      " +
                            "   and n.id=@n "+
                            " order by d.version desc                                           "
                            , docname, iduser, iduser, idnote);

                if (reslst.Count > 0)
                    res = reslst.First();

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetDocInfo: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Restituisce la lista dei documenti gestiti dall'utente (di cui è proprietario o che condivide con altri)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public static List<DbNoteDoc> GetDocsUser(int iduser)
        {
            try
            {
                // ATTENZIONE: condivido una nota, quindi tutti i documenti di una nota
                // se intendo condividere solo un documento devo creare una nota apposita
                List<DbNoteDoc> res = new List<DbNoteDoc>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNoteDoc>(
                            " select d.* from note_doc d                            " +
                            " join note n on n.id=d.idnote                          " +
                            "  and n.iduser=d.iduser                                " +
                            " left outer join note_sharing s on s.idnote=n.id       " +
                            " where (n.iduser=@n or s.idusersharing = @n)           " +
                            " order by d.version desc                               "
                            , iduser, iduser);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetDocsUser: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Restituisce la lista dei documenti o file associati alla nota dello user
        /// </summary>
        /// <param name="idnote"></param>
        /// <returns></returns>
        public static List<DbNoteDoc> GetDocsNote(long idnote, int iduser)
        {
            try
            {
                List<DbNoteDoc> res = new List<DbNoteDoc>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNoteDoc>(
                            " select distinct d.* from note_doc d                   " +
                            " join note n on n.id=d.idnote                          " +
                            "  and n.iduser=d.iduser                                " +
                            " left outer join note_sharing s on s.idnote=n.id       " +
                            "  and s.iduser=n.iduser                                " +
                            " where n.id=@n                                         " +
                            "   and n.iduser=@n                                     " +
                            " order by d.version desc                               "
                            , idnote, iduser);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetDocsUser: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Restituisce la lista dei documenti o file associati alla nota
        /// </summary>
        /// <param name="idnote"></param>
        /// <returns></returns>
        public static List<DbNoteDoc> GetDocsFromNotes(int iduser, DateTime dateFrom, DateTime dateTo, int rate, string simbolRate, string title, bool typeFindAtLeastOne, bool onlyFavorites)
        {
            try
            {
                string clause = "";
 
                if (string.IsNullOrEmpty(title) == false)
                {

                    {
                        //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                        //List<string> l = title.Split(delimiterChars).ToList();
                        List<string> l = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string s in l)
                        {
                            if (!string.IsNullOrEmpty(s) && !typeFindAtLeastOne)
                                clause = clause + " and n.title LIKE '%" + s + "%' ";
                            if (!string.IsNullOrEmpty(s) && typeFindAtLeastOne)
                                clause = clause + " or n.title LIKE '%" + s + "%' ";
                        }
                        if (clause.Length > 0)
                        {
                            clause = " and (" + clause.Substring(4) + ")";
                        }
                    }
                }

                string favClause = "";
                if (onlyFavorites)
                    favClause = " AND n.isfavorite=1 ";

                if (string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateTo <= DateTime.MinValue)
                    dateTo = DateTime.Now;

                if (dateFrom <= DateTime.MinValue)
                    dateFrom = dateTo.AddYears(-20);

                List<DbNoteDoc> res = new List<DbNoteDoc>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNoteDoc>(
                            " select d.* from note_doc d                            " +
                            " join note n on n.id=d.idnote                          " +
                            "  and n.iduser=d.iduser                                " +
                            //" select distinct n.* from note n " +
                            " where n.iduser=@n and n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by d.docname "
                            , iduser, dateFrom, dateTo, rate);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetDocsFromNotes: " + ex.Message, "");
                return null;
            }
        }

        public static int GetNextIDNote(int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                int res = conn.GetMaxFieldValInKey("note", "id", new string[] { "iduser" }, new object[] { iduser }) + 1;//new string[] { "idjob" }, new object[] { idjobDef }

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNextIDNote: " + ex.Message, "");
                return 0;
            }
        }

        public static int GetNextIDNote(string user)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                int iduser = -1;
                object r = conn.GetExecuteScalar("select id from userapp where name=@s", user);
                if (r != null)
                    iduser = Convert.ToInt32(r);
                int res = conn.GetMaxFieldValInKey("note", "id", new string[] { "iduser" }, new object[] { iduser }) + 1;//new string[] { "idjob" }, new object[] { idjobDef }

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNextIDNote: " + ex.Message, "");
                return 0;
            }
        }

        public static int GetIDUser(string username)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                int res = -1;
                object r = conn.GetExecuteScalar("select id from userapp where name=@s", username);
                if(r!=null && Convert.ToInt32(r)>0)
                    res = Convert.ToInt32(r);

                if (username == "root")
                    res = 0;
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetIDUser: " + ex.Message, "");
                return 0;
            }
        }

        public static string GetUser(int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                string res = "";
                object r = conn.GetExecuteScalar("select name from userapp where id=@n", iduser);
                if (r != null && string.IsNullOrEmpty(Convert.ToString(r))==false)
                    res = Convert.ToString(r);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetUser: " + ex.Message, "");
                return null;
            }
        }

        public static MBBUserInfo GetUserInfo(int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                MBBUserInfo res = new MBBUserInfo();
                List<MBBUserInfo> lst = conn.GetListData<MBBUserInfo>("select name from userapp where id=@n", iduser);
                if (lst.Count > 0)
                    res = lst.First();

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetUserInfo: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetUsers(string group, int iduserexclude)
        {
            try
            {
                string sql = "select distinct name from userapp where id>0 ";
                if(iduserexclude>0)
                    sql = "select distinct name from userapp where id>0 and id<>" + Convert.ToString(iduserexclude);
                if (string.IsNullOrEmpty(group) == false)
                {
                    sql = "select distinct username from user_group where groupname= '" + group + "'";
                    if(iduserexclude>0)
                        sql = "select distinct username from user_group where groupname= '" + group + "' and iduser<>" + Convert.ToString(iduserexclude);
                }
                DbFactory conn = new DbFactory(connectionString, providerName);
                List<string> r = conn.GetListString(true, sql);               
                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetUsers: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetSharedUsers(Int64 idnote, int iduser)
        {
            try
            {
                string sql = "select usernamesharing from note_sharing where idnote=@n and iduser=@n";
                DbFactory conn = new DbFactory(connectionString, providerName);
                List<string> r = conn.GetListString(true, sql, idnote,iduser);
                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetSharedUsers: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetGroups()
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                //List<string> r = conn.GetListString(true, "select distinct workgroup from userapp ");
                List<string> r = conn.GetListString(true, "select distinct groupname from user_group ");

                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetGroups: " + ex.Message, "");
                return null;
            }
        }

        /// <summary>
        /// Utile per recuperare info sulla nota conosciuta
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="idnote"></param>
        /// <returns></returns>
        public static DbNote GetNote(int iduser, Int64 idnote)
        {
            try
            {
                DbNote res = new DbNote();
                DbFactory conn = new DbFactory(connectionString, providerName);
                List<DbNote> l = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where n.iduser=@n and n.id=@n "
                            , iduser, idnote);
                if(l.Count>0)
                    res = l.FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNote: " + ex.Message, "");
                return null;
            }
        }

        public static DbNote GetNoteAndDocs(int iduser, Int64 idnote, bool mergenotessameparent = false)
        {
            DbNote res = new DbNote();
            try
            {              
                DbFactory conn = new DbFactory(connectionString, providerName);
                List<DbNote> l = conn.GetListData<DbNote>(
                            " select * from note n              " +
                            " where n.iduser=@n and n.id=@n     " 
                            , iduser, idnote);

                List<DbNote> lchild = conn.GetListData<DbNote>(
                            " select * from note N                              " +
                            " where n.iduser=@n and n.IDNOTEPARENT=@n           " +
                            " AND N.IDNOTEPARENT>0 and N.IDNOTEPARENT<>N.ID     " 
                            , iduser, idnote);

                if (lchild.Count > 0)
                    l.AddRange(lchild);

                if (mergenotessameparent)
                    l = MBBCommon.MergeNotesParent(l);

                if (l.Count > 0)
                    res = l.FirstOrDefault();

                if (res.ID > 0)
                {
                    res.Docs = GetDocsNote(res.ID, iduser);
                    res.SharedUsers = GetSharedUsers(res.ID, iduser);
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNote: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetLastsNotes(int iduser, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<DbNote> res = new List<DbNote>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where n.iduser=@n and n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " order by n.datetimeInserted desc, n.id desc "
                            ,iduser,dateFrom,dateTo);

                if (res.Count == 0)
                {
                    res = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where n.id >= (select coalesce(max(id),0) from note nn where nn.iduser=@n) - 50 " +
                            " and n.iduser=@n "+
                            " order by n.datetimeInserted, n.id "
                            , iduser, iduser);
                }
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetLastsNotes: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetNotes(int iduser, DateTime dateFrom, DateTime dateTo, int rate, string simbolRate, string title, bool typeFindAtLeastOne, bool onlyFavorites)
        {
            try
            {
                string clause = "";
                //if (string.IsNullOrEmpty(title) == false)
                //    clause = "and title LIKE '%"+title+"%' ";
                if (string.IsNullOrEmpty(title) == false)
                {
                    //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    //List<string> l = title.Split(delimiterChars).ToList();
                    List<string> l = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string s in l)
                    {
                        if (!string.IsNullOrEmpty(s) && !typeFindAtLeastOne)
                            clause = clause + " and n.title LIKE '%" + s + "%' ";
                        if (!string.IsNullOrEmpty(s) && typeFindAtLeastOne)
                            clause = clause + " or n.title LIKE '%" + s + "%' ";
                    }
                    
                }

                if (clause.Length > 0)               
                    clause = " and (" + clause.Substring(4) + ")";
                

                string favClause = "";
                if (onlyFavorites)
                    favClause = " AND n.isfavorite=1 ";

                if (string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateTo <= DateTime.MinValue)
                    dateTo = DateTime.Now;

                if (dateFrom <= DateTime.MinValue)
                    dateFrom = dateTo.AddYears(-20);

                List<DbNote> res = new List<DbNote>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where n.iduser=@n and n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " and n.rate"+ simbolRate + "@n "+ clause + favClause +
                            " order by n.datetimeInserted, n.id "
                            , iduser, dateFrom, dateTo, rate);
 
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNotes: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetNotesAndDocs(int iduser, DateTime dateFrom, DateTime dateTo, int rate, string simbolRate, string title, bool typeFindAtLeastOne, bool onlyFavorites)
        {
            try
            {
                string clause = "";
                
                if (string.IsNullOrEmpty(title) == false)
                {
                    //char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    //List<string> l = title.Split(delimiterChars).ToList();
                    //string[] sep = MBBCommon.GetSeparationCharTitleParam();
                    List<string> l = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string s in l)
                    {
                        if (!string.IsNullOrEmpty(s) && !typeFindAtLeastOne)
                            clause = clause + " and n.title LIKE '%" + s + "%' ";
                        if (!string.IsNullOrEmpty(s) && typeFindAtLeastOne)
                            clause = clause + " or n.title LIKE '%" + s + "%' ";
                    }
                }


                if (string.IsNullOrEmpty(clause) == false) clause = "AND (" + clause.Substring(4) + ")";

                string favClause = "";
                if (onlyFavorites)
                    favClause =  " AND n.isfavorite=1 ";

                if(string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateTo <= DateTime.MinValue)
                    dateTo = DateTime.Now;

                if (dateFrom <= DateTime.MinValue)
                    dateFrom = dateTo.AddYears(-20);

                string usr = "";
                if (iduser > 0)
                    usr = " n.iduser=" + iduser.ToString() + " and ";

                List<DbNote> notes = new List<DbNote>();
                List<DbNoteDoc> docs = new List<DbNoteDoc>();
                List<DbNoteSharing> sharedusers = new List<DbNoteSharing>();
                //List<string> sharedusers = new List<string>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                notes = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where " + usr + " n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by n.datetimeInserted, n.id "
                            , /*iduser,*/ dateFrom, dateTo, rate);

                docs = conn.GetListData<DbNoteDoc>(
                            " select distinct d.*,u.Name as UserNoteOwner                           " +
                            " from note n                                                           " +
                            " left outer join note_doc d on d.idnote=n.id and d.iduser=n.iduser     " +
                            " left outer join userapp u on u.id=n.idUser                            " +
                            " where " + usr + " n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by d.idnote, d.docname "
                            , dateFrom, dateTo, rate);

                //sharedusers = conn.GetListString(true,
                sharedusers = conn.GetListData<DbNoteSharing>(
                            " select distinct s.* from note n " +
                            " join note_sharing s on s.idnote=n.id and s.iduser=n.iduser " +
                            " where " + usr + " n.datetimeinserted>=@dt and n.datetimeinserted<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by s.idnote, s.usernamesharing "
                            , dateFrom, dateTo, rate);

                List<DbNoteDoc> ld = new List<DbNoteDoc>();
                
                foreach (DbNote n in notes)
                {
                    n.Docs = new List<DbNoteDoc>();
                    if (docs.Count > 0)
                        n.Docs = docs.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList();
                    n.SharedUsers = new List<string>();
                    List<DbNoteSharing> nsu = new List<DbNoteSharing>();
                    if (sharedusers.Count > 0)
                    {
                        nsu = sharedusers.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList();
                        foreach (DbNoteSharing s in nsu)
                            n.SharedUsers.Add(s.UserNameSharing);
                    }
                    //res.Add(n);
                }

                //CRSLog.WriteLog("GetNotesAndDocs: " + ex.Message, "");

                return notes;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Exception in GetNotesAndDocs: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetNotesAndDocsFilterModified(int iduser, DateTime dateFromLastMod, DateTime dateToLastMod, int rate, string simbolRate, string title, bool typeFindAtLeastOne, bool onlyFavorites)
        {
            try
            {
                string clause = "";

                if (string.IsNullOrEmpty(title) == false)
                {
                    List<string> l = title.Split(MBBCommon.GetSeparationCharTitleParam(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string s in l)
                    {
                        if (!string.IsNullOrEmpty(s) && !typeFindAtLeastOne)
                            clause = clause + " and n.title LIKE '%" + s + "%' ";
                        if (!string.IsNullOrEmpty(s) && typeFindAtLeastOne)
                            clause = clause + " or n.title LIKE '%" + s + "%' ";
                    }
                }


                if (string.IsNullOrEmpty(clause) == false) clause = "AND (" + clause.Substring(4) + ")";

                string favClause = "";
                if (onlyFavorites)
                    favClause = " AND n.isfavorite=1 ";

                if (string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateToLastMod <= DateTime.MinValue)
                    dateToLastMod = DateTime.Now;

                if (dateFromLastMod <= DateTime.MinValue)
                    dateFromLastMod = dateToLastMod.AddYears(-20);

                string usr = "";
                if (iduser > 0)
                    usr = " n.iduser=" + iduser.ToString() + " and ";

                List<DbNote> notes = new List<DbNote>();
                List<DbNoteDoc> docs = new List<DbNoteDoc>();
                List<DbNoteSharing> sharedusers = new List<DbNoteSharing>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                notes = conn.GetListData<DbNote>(
                            " select n.* from note n " +
                            " where " + usr + " n.datetimeLastMod>=@dt and n.datetimeLastMod<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by n.datetimeLastMod, n.id "
                            , dateFromLastMod, dateToLastMod, rate);

                docs = conn.GetListData<DbNoteDoc>(
                            " select distinct d.*,u.Name as UserNoteOwner                           " +
                            " from note n                                                           " +
                            " left outer join note_doc d on d.idnote=n.id and d.iduser=n.iduser     " +
                            " left outer join userapp u on u.id=n.idUser                            " +
                            " where " + usr + " n.datetimeLastMod>=@dt and n.datetimeLastMod<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by d.idnote, d.docname "
                            , dateFromLastMod, dateToLastMod, rate);

                sharedusers = conn.GetListData<DbNoteSharing>(
                            " select distinct s.* from note n " +
                            " join note_sharing s on s.idnote=n.id and s.iduser=n.iduser " +
                            " where " + usr + " n.datetimeLastMod>=@dt and n.datetimeLastMod<=@dt " +
                            " and n.rate" + simbolRate + "@n " + clause + favClause +
                            " order by s.idnote, s.usernamesharing "
                            , dateFromLastMod, dateToLastMod, rate);

                List<DbNoteDoc> ld = new List<DbNoteDoc>();

                foreach (DbNote n in notes)
                {
                    n.Docs = new List<DbNoteDoc>();
                    if (docs.Count > 0)
                        n.Docs = docs.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList();
                    n.SharedUsers = new List<string>();
                    List<DbNoteSharing> nsu = new List<DbNoteSharing>();
                    if (sharedusers.Count > 0)
                    {
                        nsu = sharedusers.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList();
                        foreach (DbNoteSharing s in nsu)
                            n.SharedUsers.Add(s.UserNameSharing);
                    }

                }
                return notes;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("Exception in GetNotesAndDocs: " + ex.Message, "");
                return null;
            }
        }


        public static List<DbNoteSharing> GetNotesAndDocsForSharing( string usersharing, DateTime dateFrom, DateTime dateTo, int rate, string simbolRate, string title, bool onlyToRead)
        {
            try
            {
                string clause = "";
                if (string.IsNullOrEmpty(title) == false)
                    clause = "and n.title LIKE '%" + title + "%' ";
                if (onlyToRead)
                    clause = " and ns.istoread=1 ";

                if (string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateTo <= DateTime.MinValue)
                    dateTo = DateTime.Now;

                if (dateFrom <= DateTime.MinValue)
                    dateFrom = dateTo.AddYears(-20);

                List<DbNoteDoc> docs = new List<DbNoteDoc>();
                List<DbNoteSharing> sharedparentnotes = new List<DbNoteSharing>();

                List<DbNoteSharing> sharednoteswithcurrentuser = new List<DbNoteSharing>();
                List<DbNoteSharing> sharednotesfromcurrentuser = new List<DbNoteSharing>();
                List<DbNote> childrennotes = new List<DbNote>();
                DbFactory conn = new DbFactory(connectionString, providerName);

                int iduser = GetIDUser(usersharing);
                docs = conn.GetListData<DbNoteDoc>(
                            " select distinct d.* from note n                                   " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join note_doc d on d.idnote=n.id and d.iduser=n.iduser " +
                            " where ns.usernamesharing=@s                                       " +
                            " and n.rate" + simbolRate + "@n " + clause +
                            " order by d.idnote, d.docname                                      "
                            , usersharing,rate);
                // note condivise con l'utente attuale: è il ricevente delle note
                sharednoteswithcurrentuser = conn.GetListData<DbNoteSharing>(
                            " select distinct ns.*,n.*, u.name as UserOwner from note n         " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join userapp u on u.id=n.iduser                        " +
                            " where ns.usernamesharing=@s                                       "+
                            " and n.rate" + simbolRate + "@n " + clause +
                            " and (n.idnoteparent is null or n.idnoteparent=0)                  "+
                            " order by n.datetimeinserted,n.id,n.iduser                         "
                            , usersharing, rate);
                // note generate dall'utente attuale che vuole condividere con un altro utente
                sharednotesfromcurrentuser = conn.GetListData<DbNoteSharing>(
                            " select distinct ns.IDNote,ns.IsToRead,n.*, u.name as UserOwner    " +
                            "   from note n                                                     " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join userapp u on u.id=n.iduser                        " +
                            " where n.iduser=@n                                                 " +
                            " and n.rate" + simbolRate + "@n " + clause +
                            " and (n.idnoteparent is null or n.idnoteparent=0)                  " +
                            " order by n.datetimeinserted,n.id,n.iduser                         "
                            , iduser, rate);

                List<DbNoteDoc> ld = new List<DbNoteDoc>();

                foreach (DbNoteSharing n in sharednoteswithcurrentuser)
                {
                    sharedparentnotes.Add(n);
                }

                foreach (DbNoteSharing n in sharednotesfromcurrentuser)
                {
                    sharedparentnotes.Add(n);
                }
                
                // per ogni nota condivisa per l'utente che legge attualmente (condivisore, non owner)
                // recupera le risposte dell'owner e dell'utente attuale
                foreach (DbNoteSharing n in sharedparentnotes)
                {
                    n.Docs = new List<DbNoteDoc>();
                    if (docs.Count > 0)
                        n.Docs = docs.Where(X => X.IDNote == n.IDNote && X.IDUser == n.IDUser).ToList();

                    childrennotes = conn.GetListData<DbNote>(
                            " select n.*  from note n                       " +
                            //" join userapp u on u.id=n.iduser               " +
                            //" join note_sharing ns on ns.IDNote = n.ID and ns.idusersharing=n.iduser " +
                            //" where n.iduser=@n "+
                            " where n.rate " + simbolRate + "@n " + 
                            " and n.idnoteparent=@n and n.usernameparent=@s " +
                            //" and n.iduser=@n                               " +  
                            " order by n.datetimeinserted "
                            , rate, n.IDNote,n.UserOwner/*,n.IDUser*/);

                    string txt = n.Text;
                    int i = 1;
                    foreach (DbNote c in childrennotes)
                    {
                        txt = txt + "\n\n" + "-" + i.ToString() + "- " + GetUser(c.IDUser) + " [" + c.DateTimeInserted.ToShortDateString() + " " + c.DateTimeInserted.ToShortTimeString() + "]" +
                            "\n"+ c.Text;
                        i++;
                    }

                    n.Text = txt;
                }
                return sharedparentnotes;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNotesAndDocsForSharing: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetNotesAndDocsShared(string usersharing, DateTime dateFrom, DateTime dateTo, int rate, string simbolRate, string title, bool onlyToRead)
        {
            try
            {
                string clause = "";
                if (string.IsNullOrEmpty(title) == false)
                    clause = "and n.title LIKE '%" + title + "%' ";
                if (onlyToRead)
                    clause = " and ns.istoread=1 ";

                if (string.IsNullOrEmpty(simbolRate))
                    simbolRate = ">=";

                if (dateTo <= DateTime.MinValue)
                    dateTo = DateTime.Now;

                if (dateFrom <= DateTime.MinValue)
                    dateFrom = dateTo.AddYears(-20);

                List<DbNoteDoc> docs = new List<DbNoteDoc>();
                List<DbNote> sharedparentnotes = new List<DbNote>();

                List<DbNote> sharednoteswithcurrentuser = new List<DbNote>();
                List<DbNote> sharednotesfromcurrentuser = new List<DbNote>();
                List<DbNote> childrennotes = new List<DbNote>();
                DbFactory conn = new DbFactory(connectionString, providerName);

                int iduser = GetIDUser(usersharing);
                docs = conn.GetListData<DbNoteDoc>(
                            " select distinct d.* from note n                                   " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join note_doc d on d.idnote=n.id and d.iduser=n.iduser " +
                            " where ns.usernamesharing=@s                                       " +
                            " and n.rate" + simbolRate + "@n " + clause +
                            " order by d.idnote, d.docname                                      "
                            , usersharing, rate);
                // note condivise con l'utente attuale: è il ricevente delle note
                sharednoteswithcurrentuser = conn.GetListData<DbNote>(
                            " select distinct ns.*,n.*, u.name as UserOwner from note n         " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join userapp u on u.id=n.iduser                        " +
                            " where ns.usernamesharing=@s                                       " +
                            " and n.rate" + simbolRate + "@n " + clause +
                            " and (n.idnoteparent is null or n.idnoteparent=0)                  " +
                            " order by n.datetimeinserted,n.id,n.iduser                         "
                            , usersharing, rate);
                // note generate dall'utente attuale che vuole condividere con un altro utente
                sharednotesfromcurrentuser = conn.GetListData<DbNote>(
                            " select distinct ns.IDNote,ns.IsToRead,n.*, u.name as UserOwner    " +
                            "   from note n                                                     " +
                            " join note_sharing ns on ns.IDNote = n.ID and ns.iduser=n.iduser   " +
                            " left outer join userapp u on u.id=n.iduser                        " +
                            " where n.iduser=@n                                                 " +
                            " and n.rate" + simbolRate + "@n " + clause +
                            " and (n.idnoteparent is null or n.idnoteparent=0)                  " +
                            " order by n.datetimeinserted,n.id,n.iduser                         "
                            , iduser, rate);

                List<DbNoteDoc> ld = new List<DbNoteDoc>();

                foreach (DbNote n in sharednoteswithcurrentuser)
                {
                    sharedparentnotes.Add(n);
                }

                foreach (DbNote n in sharednotesfromcurrentuser)
                {
                    sharedparentnotes.Add(n);
                }

                // per ogni nota condivisa per l'utente che legge attualmente (condivisore, non owner)
                // recupera le risposte dell'owner e dell'utente attuale
                foreach (DbNote n in sharedparentnotes)
                {
                    n.Docs = new List<DbNoteDoc>();
                    if (docs.Count > 0)
                        n.Docs = docs.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList();

                    childrennotes = conn.GetListData<DbNote>(
                            " select n.*  from note n           " +
                        //" join userapp u on u.id=n.iduser               " +
                        //" join note_sharing ns on ns.IDNote = n.ID and ns.idusersharing=n.iduser " +
                        //" where n.iduser=@n "+
                            " where n.rate " + simbolRate + "@n " +
                            " and n.idnoteparent=@n             " +
                            " and n.usernameparent=@s           " +
                        //" and n.iduserparent=@n                               " +  
                            " order by n.datetimeinserted "
                            , rate, n.ID, /*n.UserOwner*/n.UserNameParent);

                    string txt = n.Text;
                    int i = 1;
                    foreach (DbNote c in childrennotes)
                    {
                        txt = txt + "\n\n" + "-" + i.ToString() + "- " + GetUser(c.IDUser) + " [" + c.DateTimeInserted.ToShortDateString() + " " + c.DateTimeInserted.ToShortTimeString() + "]" +
                            "\n" + c.Text;
                        i++;
                    }

                    n.Text = txt;
                }
                return sharedparentnotes;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetNotesAndDocsForSharing: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetTitles(int iduser)
        {
            try
            {
                List<string> res = new List<string>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListString(true,
                            " select distinct n.title from note n " +
                            " where n.iduser=@n " +
                            " order by n.title "
                            , iduser);
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetTitles: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetFavoritesTitles(int iduser)
        {
            try
            {
                List<string> res = new List<string>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListString(true,
                            " select distinct n.title from note n " +
                            " where n.iduser=@n     " +
                            " and n.isfavorite=1    " +
                            " order by n.title      "
                            , iduser);
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetFavoritesTitles: " + ex.Message, "");
                return null;
            }
        }

        public static List<string> GetLanguages()
        {
            try
            {
                List<string> res = new List<string>();
                DbFactory conn = new DbFactory(connectionString, providerName);
                
                res = conn.GetListString(true,
                           "select name from parameter where groupparam=@s order by name", "language");

                //CRSParameter p = new CRSParameter(conn);
                //DataTable prm = p.GetParamList("language");
                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetLanguages: " + ex.Message, "");
                return null;
            }
        }

        public static bool ExistsNote(Int64 idnote, int iduser)
        {
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                int c = conn.GetRecordCount(
                            " select count(*) from note n " +
                            " where n.iduser=@n and n.id=@n "
                            , iduser, idnote);
                if (c > 0) 
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("ExistsNote: " + ex.Message, "");
                return false;
            }
        }

        /// <summary>
        /// iduser deve essere > 0, gli altri parametri possono essere null o minDate
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<DbActivity> GetActivities(int iduser, DateTime? dt1, DateTime? dt2, List<string> status, int typefilter_0Ins1Mod,bool onlywithnotes)
        {
            List<DbActivity> res = new List<DbActivity>();
            List<DbActivity> reslog = new List<DbActivity>();
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                string wst = "";
                if (status != null && status.Count > 0)
                    wst = " and n.status in('" + string.Join("','", status) + "')";//" and status=%s ";

                //string wem = " and n.idactivity > 0  ";
                //if (alsoempty) wem = "";
                string wdt = "";

                if (dt1 != null && dt1 > DateTime.MinValue && dt2 != null && dt2 > DateTime.MinValue)
                {
                    wdt = " and n.dateTimeInserted >= @dt and n.dateTimeInserted <= @dt ";
                    if (typefilter_0Ins1Mod == 1)                   
                        wdt = " and n.dateTimeLastMod >= @dt and n.dateTimeLastMod <= @dt ";
                    
                    wdt = DbUtils.PrepareSqlStatement(DbConst.PROVIDER_SQLSERVER_CE_35, wdt, dt1, dt2);

                    string wdt2 = " and L.StartActivityLog >= @dt and L.StartActivityLog <= @dt";
                    wdt2 = DbUtils.PrepareSqlStatement(DbConst.PROVIDER_SQLSERVER_CE_35, wdt2, dt1, dt2);
                    // prendo anche le attività modificate di recente (cioè nell'intervallo passato come parametro)
                    // le attività recenti le identifico attraverso activity_log 
                    string sqllog =
                       "select distinct n.IDACTIVITY,N.TOTMINPREVIEW,N.STARTACTIVITY,N.TITLEACTIVITY    " +
                       "   ,TEXTACTIVITY,N.IDUSER,N.STATUS,N.TYPEACTIVITY,N.DATETIMEINSERTED            " +
                       "   ,N.DATETIMELASTMOD                                                           " +      
                       "   ,COALESCE(max(L.StartActivityLog), GETDATE())  as LastStartLogActivity       " +
                       " from activity n                                                                " +
                       " JOIN ACTIVITY_LOG L                                                            " +
                       "   ON L.IDACTIVITY = N.IDACTIVITY                                               " +
                       " where n.iduser = @n                                                            " +
                       wst + wdt2 +
                       " GROUP BY                                                                       " +
                       "     N.IDACTIVITY,N.TOTMINPREVIEW,N.STARTACTIVITY,N.TITLEACTIVITY,TEXTACTIVITY  " +
                       "    ,N.IDUSER,N.STATUS,N.TYPEACTIVITY,N.DATETIMEINSERTED,N.DATETIMELASTMOD      ";

                    reslog = conn.GetListData<DbActivity>(sqllog, iduser);
                    if(reslog.Count>0)                   
                        reslog = reslog.Where(X => X.LastStartLogActivity >= dt1 && X.LastStartLogActivity <= dt2).ToList();
                    
                }
                string jwn = "";
                if (onlywithnotes) jwn = " JOIN ACTIVITY_NOTE A ON A.IDACTIVITY = n.IDACTIVITY ";
                // prendo solo le ultime activities inserite
                res = conn.GetListData<DbActivity>(
                            " select distinct n.* from activity n   " +
                            jwn +
                            " where n.iduser=@n                     " +
                            wst + wdt
                            , iduser);

                if (reslog.Count() > 0)               
                    res.AddRange(reslog.Where(X => res.Where(Y => Y.IDActivity == X.IDActivity).Count() == 0).ToList());
                
                if (typefilter_0Ins1Mod == 0)
                    res = res.OrderByDescending(X => X.DateTimeInserted).ThenByDescending(Y=>Y.LastStartLogActivity).ToList();
                else
                    res = res.OrderByDescending(X => X.LastStartLogActivity).ThenByDescending(Y => Y.DateTimeInserted).ToList();

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivities: " + ex.Message, "");
                return null;
            }
        }

        public static DbActivity GetActivity(long idact)
        {
            List<DbActivity> res = new List<DbActivity>();
            DbActivity r = new DbActivity();
            try
            {
                // ci dovrebbe essere anche l'id user ma non lo considero per le activities (tanto l'id è costruito su long filetime)
                // e comunque è da vedere sel'attività è da associare ad un utente o se in realtà è generica
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbActivity>(
                            " select * from activity n      " +
                            " where n.idactivity=@n         " 
                           , idact);

                if (res.Count() > 0)
                    r = res.First();

                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivities: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbActivityLog> GetActivityLogs(long idact)
        {
            List<DbActivityLog> res = new List<DbActivityLog>();
            DbActivityLog r = new DbActivityLog();
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbActivityLog>(
                            " select * from activity_log n                      " +
                            " join activity a on a.idactivity = n.idactivity    " +
                            " where n.idactivity=@n                             " +
                            " ORDER BY N.startactivitylog                       "
                           , idact);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivityLogs: " + ex.Message, "");
                return null;
            }
        }

        public static DbActivity GetActivity(string title)
        {
            List<DbActivity> res = new List<DbActivity>();
            DbActivity r = new DbActivity();
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbActivity>(
                            " select * from activity n      " +
                            " where n.TitleActivity=@s      "
                           , title);

                if (res.Count() > 0)
                    r = res.First();

                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivity: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbActivityLog> GetActivityLogs(DateTime fromDate, DateTime toDate, bool onlyNotEmptyActivities)
        {
            List<DbActivityLog> res = new List<DbActivityLog>();
            DbActivityLog r = new DbActivityLog();
            try
            {
                string wh = "";
                if (onlyNotEmptyActivities)
                    wh = " and a.idactivity>0 ";
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbActivityLog>(
                            " select * from activity_log n                                  " +
                            " join activity a on a.idactivity = n.idactivity                " +           
                            " where n.startactivitylog>=@dt and n.startactivitylog<=@dt     " +
                            " ORDER BY N.startactivitylog                                   " +
                            wh
                           , fromDate, toDate);

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivityLogs with params date: " + ex.Message, "");
                return null;
            }
        }

        public static DbActivityNote GetActivityNote(long idact, long idnote)
        {
            List<DbActivityNote> res = new List<DbActivityNote>();
            DbActivityNote r = new DbActivityNote();
            try
            {

                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbActivityNote>(
                            " select * from activity_note n         " +
                            " where n.idactivity=@n and n.idnote=@n "
                           , idact, idnote);

                if (res.Count() > 0)
                    r = res.First();

                return r;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivityNote: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetActivityNotes(long idact)
        {
            List<DbNote> res = new List<DbNote>();
            List<DbNoteDoc> resdoc = new List<DbNoteDoc>();
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNote>(
                    " select * from note n                              " +                
                    " join activity_note an on an.idnote = n.id         " +
                    " join activity a on a.idactivity = an.idactivity   " +
                    "  and a.iduser = n.iduser                          " +   
                    " where a.idactivity=@n                             "
                    , idact);

                resdoc = conn.GetListData<DbNoteDoc>(
                    "  select * from activity_note an                   " +
                    "  join activity a on a.idactivity = an.idactivity  " +
                    "  join note_doc d on d.idnote = an.idnote          " +
                    "   and d.iduser = a.iduser                         " +    
                    "  where a.idactivity = @n                          "
                    , idact);

                if (resdoc.Count > 0)
                {
                    foreach(DbNote n in res)
                    {
                        n.Docs = new List<DbNoteDoc>();
                        n.Docs.AddRange(resdoc.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList());
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetActivityNotes: " + ex.Message, "");
                return null;
            }
        }

        public static List<DbNote> GetEmptyActivitiesNotes(DateTime? dt1, DateTime? dt2)
        {
            List<DbNote> res = new List<DbNote>();
            List<DbNoteDoc> resdoc = new List<DbNoteDoc>();

            string wdt = "";
            if (dt1 != null && dt1 > DateTime.MinValue && dt2 != null && dt2 > DateTime.MinValue)
            {
                wdt = " and n.DateTimeLastMod>=%u and n.DateTimeLastMod<=%u ";
                wdt = DbUtils.PrepareSqlStatement(DbConst.PROVIDER_SQLSERVER_CE_35, wdt, dt1, dt2);
            }
            try
            {
                DbFactory conn = new DbFactory(connectionString, providerName);
                res = conn.GetListData<DbNote>(
                    " select *,-1 as IDActivity from note n                 " +
                    " left outer join activity_note an on an.idnote = n.id  " +
                    //" join activity a on a.idactivity = an.idactivity   " +
                    //"  and a.iduser = n.iduser                          " +
                    " where an.idactivity is null                           " +
                    wdt);

                resdoc = conn.GetListData<DbNoteDoc>(
                    "  select * from note n                                 " +
                     " left outer join activity_note an on an.idnote = n.id " +
                    "  join note_doc d on d.idnote = n.id                   " +
                    "   and d.iduser = n.iduser                             " +
                    "  where an.idactivity is null                          " +
                    wdt);

                if (resdoc.Count > 0)
                {
                    foreach (DbNote n in res)
                    {
                        n.Docs = new List<DbNoteDoc>();
                        n.Docs.AddRange(resdoc.Where(X => X.IDNote == n.ID && X.IDUser == n.IDUser).ToList());
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                CRSLog.WriteLog("GetEmptyActivitiesNotes: " + ex.Message, "");
                return null;
            }
        }
    }
}
