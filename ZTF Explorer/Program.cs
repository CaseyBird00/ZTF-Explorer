using System;
using System.ComponentModel.Design;


namespace ZTF_Explorer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //TODO Create thread for parquet reader

            
            Console.WriteLine("Enter SQL DB");

            string input = Console.ReadLine();

            Console.WriteLine("Intializing SQL...");
            try { 
            
                SQL.LoadSQL(Convert.ToInt32(input));
                using var connection = SQL.GetConnection();

                // Open the connection
                //connection.Open();
                Console.WriteLine("SQL Initialized.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect: " + ex.Message);
            }

            Console.WriteLine("type \"Read\" to start reading files or \"Process\" to start processing stars");
            input = Console.ReadLine();
            if (input == "Read" || input == "read")
            {
                ParquetReader reader = new ParquetReader();
                reader.Parquetreader();
            }
            else if (input == "Process" || input == "process")
            {
                Start();
            }
            else if (input == "Compare" || input == "compare")
            {
                Compare();
            }
            else if (input == "Log" || input == "log")
            {
                Export();
            }
            else

            {
                Console.WriteLine("Invalid input");
            }

            //TODO create thread for data processing
        }
        public static void Start()
        {

            foreach (var star in Queue.StarsQ)
            {
                StarProcessing.StartProcess(star);
            }
            Main(Array.Empty<string>());

        }

        public static void Compare()
        {
            foreach (var star in Queue.VariableStarsQ)
            {
                StarVariationProcessor.MatchStars(star);
            }
            Main(Array.Empty<string>());
        }

        public static void Export()
        {
            try
            {
                foreach (var star in Queue.VariableStarsQ)
                {
                    Console.WriteLine("Candidate Star: " + star.ObjID + "RA " + star.Ra + "DECL " + star.Decl);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Failed " + Queue.Candidates);
            }
            
                Main(Array.Empty<string>());
        }
    }
}