using System;
using System.Linq;

namespace DirectoryCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath, mask;
            var testMode = false;
            var parser = new Parser();

            var argPath = parser.GetArgumentValue(args, "-p"); // path
            if (argPath != null)
            {
                basePath = (string)argPath;
            }
            else
            {
                Console.WriteLine("The parameter \"path\" (-p) has not been passed, program is stopping.");
                return;
            }
            var argMask = parser.GetArgumentValue(args, "-m"); // mask
            if (argMask != null)
            {
                mask = (string)argMask;
            }
            else
            {
                Console.WriteLine("The parameter \"mask\" (-m) has not been passed, program is stopping.");
                return;
            }
            var argSearchDay = parser.GetArgumentValue(args, "-s"); // searched day
            var searchDay = argSearchDay != null ? int.Parse(argSearchDay) : 1;

            var argDaysBack = parser.GetArgumentValue(args, "-d"); // days back
            var daysBack = argDaysBack != null ? int.Parse(argDaysBack) : 30;

            testMode = parser.ArgumentExists(args, "-t");

            try
            {
                Cleaner cleaner = new Cleaner(basePath, mask, searchDay, daysBack, testMode);
                var files = cleaner.CollectFiles();
                if (files.Any())
                {
                    cleaner.Clean(files); // remove files that don't fit the search 
                }

            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"No permissions to access the catalog: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The unexpected error has occured: {ex.Message}");
            }
        }
    }
}