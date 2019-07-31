using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DnsTestTask2
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = ".";
            var maxThreads = 10;
            var substring = "test";

            if (args.Length > 0)
            {

                path = args[0];

                for (var i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-t":
                            maxThreads = int.Parse(args[i + 1]);
                            break;
                        case "-s":
                            substring = args[i + 1].Trim('"');
                            break;
                    }
                }

            }

            var fileNames = Directory.EnumerateFiles(path);


            Parallel.ForEach(
                fileNames,
                new ParallelOptions { MaxDegreeOfParallelism = maxThreads },
                fileName => FindString(fileName, substring)
                );

        }

        private static void FindString(string fileName, string substring)
        {
            var sw = new Stopwatch();
            sw.Start();

            var file = File.ReadAllLines(fileName);
            for (var i = 0; i < file.Length; i++)
            {
                if (file[i].Contains(substring))
                {
                    PrintInfo(fileName, sw.ElapsedTicks, file[i], i);
                }
            }

            sw.Stop();
        }

        private static void PrintInfo(string fileName, long tiks, string line, long lineNumber)
        {
            var ns = 1000000000.0 * tiks / Stopwatch.Frequency;
            Console.WriteLine($"{fileName} [thread={Thread.CurrentThread.ManagedThreadId},time={Convert.ToInt64(ns)}ns, line={lineNumber}] {line}");
        }
    }
}
