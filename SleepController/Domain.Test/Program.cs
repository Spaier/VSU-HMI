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
            var detector = new ClosedEyesDetector();
            var result = new List<(IEnumerable<EEGEntry> Batch, bool IsClosed)>();
            var batchSize = 24;
            {
                var i = 0;
                while (true)
                {
                    var batch = eeg.Data
                        .Skip(batchSize * i++)
                        .Take(batchSize)
                        .ToList();

                    if (batch.Count() is 0)
                    {
                        break;
                    }

                    var isClosed = detector.IsClosed(batch);
                    result.Add((batch, isClosed));
                }
            }
            var isClosedArray = result
                .SelectMany(it => it.Batch.Select(batch => it.IsClosed))
                .ToList();

            await File.WriteAllLinesAsync("results.txt", isClosedArray
                .Select(it => it ? "1" : "0")
                .ToArray())
                .ConfigureAwait(false);

            Console.WriteLine("The End.");
            Console.ReadKey();
        }
    }
}
