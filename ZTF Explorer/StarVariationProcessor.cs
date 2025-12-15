using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MySqlConnector;

namespace ZTF_Explorer
{
    public class StarVariationProcessor
    {
        public static void MatchStars(Star star)
        {
            double tolerance = 0.00833; // 1 arcsecond in degrees

            double raMin = star.Ra - tolerance;
            double raMax = star.Ra + tolerance;

            double decMin = star.Decl - tolerance;
            double decMax = star.Decl + tolerance;


            var conn = SQL.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 30 * 60;
            cmd.CommandText = @"
    SELECT *
    FROM Stars
    WHERE RA BETWEEN @raMin AND @raMax
      AND DECLI BETWEEN @decMin AND @decMax";

            cmd.Parameters.AddWithValue("@raMin", raMin);
            cmd.Parameters.AddWithValue("@raMax", raMax);
            cmd.Parameters.AddWithValue("@decMin", decMin);
            cmd.Parameters.AddWithValue("@decMax", decMax);

            try
            {
                Console.WriteLine("Comparing RA " + star.Ra +"DECLI" + star.Decl);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"Found Star: {reader["vsx_id"]}, RA: {reader["RA"]}, Dec: {reader["DECLI"]}");
                }

            } catch(Exception ex) { 
             Console.WriteLine(ex.Message);
            
            }
            
            
        }

    }
}
