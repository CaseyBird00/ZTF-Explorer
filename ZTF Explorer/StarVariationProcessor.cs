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
                Console.WriteLine("Comparing RA " + star.Ra + "DECLI" + star.Decl);
                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("Matches found:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"Found Star: {reader["vsx_id"]}, RA: {reader["RA"]}, Dec: {reader["DECLI"]}");
                    }
                }
                else
                {
                    Console.WriteLine("No matches found.");
                    AddCandidateToDatabase(star);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            
            
        }
        public static void AddCandidateToDatabase(Star star)
        {
            var conn = SQL.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO Candidates (ZTF_ID, RA, DECLI, Status)
VALUES (@ztfId, @ra, @decl, @status)";
            cmd.Parameters.AddWithValue("@ztfId", star.ObjID);
            cmd.Parameters.AddWithValue("@ra", star.Ra);
            cmd.Parameters.AddWithValue("@decl", star.Decl);
            cmd.Parameters.AddWithValue("@status", "New");
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Candidate star {star.ObjID} added to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding candidate star {star.ObjID} to database: {ex.Message}");
            }
        } 



        }
}
    