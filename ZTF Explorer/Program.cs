using System;


namespace ZTF_Explorer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ParquetReader reader = new ParquetReader();
            reader.parquetreader();
        }
    }
}