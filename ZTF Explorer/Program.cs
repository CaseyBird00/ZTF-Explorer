using System;


namespace ZTF_Explorer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //TODO Create thread for parquet reader

            Console.WriteLine("Hello, World!");
            ParquetReader reader = new ParquetReader();
            reader.parquetreader();
            Start();

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