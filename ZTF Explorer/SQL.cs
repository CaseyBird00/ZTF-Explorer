using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.GeoJson;
using Microsoft.Data.SqlClient;

namespace ZTF_Explorer
{
    public class SQL
    {
        string IP { get; set; }
        string UserID { get; set; }
        string Password { get; set; }

        string Database { get; set; }
        void LoadSQL()
        {
            string FilePath = "C:\\Users\\Casey\\Documents\\GitHub\\ZTF-Explorer\\ZTF Explorer\\SQL.dat";

            var file = File.ReadAllLines(FilePath);

            IP = file[0];
            UserID = file[1];
            Password = file[2];
            Database = file[3];

        }
        public void Connection()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = IP,           // IP or server name
                UserID = UserID,           // SQL username
                Password = Password,       // SQL password
                InitialCatalog = Database  // Database name
            };
        }

    }
}
