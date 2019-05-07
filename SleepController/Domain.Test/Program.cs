using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SleepController.Domain.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var streamReader = new StreamReader(@"C:\Users\Spaier\Projects\VSU\HMI\EegSleepController\SleepController\Data\kazakov_eeg_4channels.edf");
            var parser = new EEGParser();
            var eeg = await parser.Parse(streamReader).ConfigureAwait(false);
            var detector = new SuperDetector();
            var result = new List<(short Entry, bool IsClosed, int Average, int FloatingAverage)>();
            var batchSize = 24;
            {
                var i = 0;
                while (true)
                {
                    var batch = eeg.Data
                        .Skip(batchSize * i++)
                        .Take(batchSize);

                    if (!batch.Any())
                    {
                        break;
                    }

                    var isClosed = detector.IsClosed(batch);
                    result.AddRange(batch.Select(entry => (entry, isClosed, detector.Average, detector.FloatingAverage)));
                }
            }

            await File.WriteAllLinesAsync("results.csv", result
                .Select(it => $"{(it.IsClosed ? "1" : "0")},{it.Average},{it.FloatingAverage},{it.Entry}")
                .Prepend("IsClosed,Average,FloatingAverage,Value")
                .ToArray())
                .ConfigureAwait(false);

            await File.WriteAllLinesAsync("is_closed.txt", result
                .Select(it => $"{(it.IsClosed ? "1" : "0")}")
                .ToArray())
                .ConfigureAwait(false);

            await File.WriteAllLinesAsync("average.txt", result
                .Select(it => $"{it.Average}")
                .ToArray())
                .ConfigureAwait(false);

            await File.WriteAllLinesAsync("floating_average.txt", result
                .Select(it => $"{it.FloatingAverage}")
                .ToArray())
                .ConfigureAwait(false);

            Console.WriteLine("The End.");
            Console.ReadKey();
        }
    }
}
