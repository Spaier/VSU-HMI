using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SleepController.Domain
{
    public class EEGParser
    {
        public async Task<Signal> Parse(StreamReader streamReader)
        {
            var eeg = new Signal()
            {
                Frequency = 200,
            };

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync().ConfigureAwait(false);

                // TODO: Extract
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line[0] is ';')
                {
                    // TODO: Parse Frequency
                    continue;
                }

                var values = line.Split(' ')
                    .Select(it => short.Parse(it))
                    .ToList();

                eeg.Data.Add(values[0]);
            }

            return eeg;
        }
    }
}
