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
            int batchSize = 100;
            var stars = Queue.VariableStarsQ;
            int totalStars = stars.Count;
            int processed = 0;

            using var conn = SQL.GetConnection();
            while (processed < /*totalStars*/ 50)
            {

                Console.WriteLine("adding to batch");
                var batch = stars.Skip(processed).Take(batchSize).ToList();
                var sqlBuilder = new StringBuilder("SELECT * FROM Stars WHERE ");

                var parameters = new List<MySqlConnector.MySqlParameter>();

                for (int i = 0; i < /*batch.Count*/ 20; i++)
                {
                    var CurrentStar = batch[i];
                    double tolerance = 0.00833; // 30 arcseconds for testing

                    Console.WriteLine(CurrentStar + "number " + i);

                    double raMin = CurrentStar.Ra - tolerance;
                    double raMax = CurrentStar.Ra + tolerance;
                    double decMin = CurrentStar.Decl - tolerance;
                    double decMax = CurrentStar.Decl + tolerance;

                    sqlBuilder.Append($"(RA BETWEEN @raMin{i} AND @raMax{i} AND DECLI BETWEEN @decMin{i} AND @decMax{i})");

                    if (i < batch.Count - 1)
                        sqlBuilder.Append(" OR ");

                    parameters.Add(new MySqlConnector.MySqlParameter($"@raMin{i}", raMin));
                    parameters.Add(new MySqlConnector.MySqlParameter($"@raMax{i}", raMax));
                    parameters.Add(new MySqlConnector.MySqlParameter($"@decMin{i}", decMin));
                    parameters.Add(new MySqlConnector.MySqlParameter($"@decMax{i}", decMax));
                }

                Console.WriteLine("Querying Stars");

                using var cmd = conn.CreateCommand();
                cmd.CommandTimeout = 1800;
                cmd.CommandText = sqlBuilder.ToString();
                cmd.Parameters.AddRange(parameters.ToArray());

                try
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Found Star: {reader["vsx_id"]}, RA: {reader["RA"]}, Dec: {reader["DECLI"]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                processed += batchSize;
                Console.WriteLine($"Processed {processed}/{totalStars} stars...");
            }


        }

    }
}
