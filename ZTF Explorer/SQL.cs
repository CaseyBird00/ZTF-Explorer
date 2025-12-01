using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ZTF_Explorer
{
    public class SQL
    {
        public static void Connection()
        {
            var server = new SqlConnectionStringBuilder
            {
                DataSource = "ztf-explorer-server.database.windows.net",
                UserID = "ztfexploreradmin",
                Password = "ZtfExplorer123!",
                InitialCatalog = "ZTF-Explorer-DB"
            };
        }
    }
}
