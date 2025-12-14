using Azure.Core.GeoJson;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTF_Explorer
{
    public class SQL
    {
        private static string? IP;
        private static string? UserID;
        private static string? Password;
        private static string? Database;


        public static void LoadSQL()
        {
            string FilePath = "C:\\Users\\Casey\\Documents\\GitHub\\ZTF-Explorer\\ZTF Explorer\\SQL.dat";

            var file = File.ReadAllLines(FilePath);

            IP = file[0];
            UserID = file[1];
            Password = file[2];
            Database = file[3];
            //Console.WriteLine(IP + UserID + Password + Database);


        }
        public static MySqlConnection GetConnection()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = IP,
                Port = 3306,
                UserID = UserID,
                Password = Password,
                Database = Database,
                ConnectionTimeout = 5 // seconds
            };

            var conn = new MySqlConnection(builder.ConnectionString);
            conn.Open(); // must open it before using
            return conn;
        }
    }
}
