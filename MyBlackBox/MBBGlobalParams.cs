using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace MyBlackBoxCore
{
    public static class MBBGlobalParams
    {
        private static string ApplicationPath = Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location));
        public static string DbFileNameSqlServerCE_MBB = "DBMbbCore.sdf";
        public static string DBSqlServerFilePathNameMBB = Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location)) + "\\DB\\SQLServerVersion\\myblackbox.sql";
        public static string DBMySQLFilePathNameMBB = Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location)) + "\\DB\\MySQLVersion\\myblackbox.sql";
        public static string DBSqlServerFileDataPathNameMBB = Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location)) + "\\DB\\SQLServerVersion\\myblackboxdata.sql";
        public static string DBMySQLFileDataPathNameMBB = Path.GetDirectoryName((Assembly.GetExecutingAssembly().Location)) + "\\DB\\MySQLVersion\\myblackboxdata.sql";
        
    }
}
