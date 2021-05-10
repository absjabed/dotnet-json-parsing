using System;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Arguments arguments = new Arguments(args);
                ProcessJsonFiles processJson = new ProcessJsonFiles("../data/", arguments);
                processJson.GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
