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
                ProcessResult processResult = new ProcessResult("../data/", arguments);
                processResult.GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
