using System;


namespace ZTF_Explorer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //TODO Create thread for parquet reader
            
            Console.WriteLine("type \"Read\" to start reading files or \"Process\" to start processing stars");
            string input = Console.ReadLine();
            if(input == "Read")
            {
                ParquetReader reader = new ParquetReader();
                reader.parquetreader();
            }
            else if(input == "Process")
            {
                Start();
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
        }
    
    }
}