using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SleepController.Domain
{
    public class SignalParser
    {
        public async Task<Signal> Parse(StreamReader streamReader, int index)
        {
            var signal = new Signal()
            {
                Frequency = -1,
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

                var values = line.Split(' ', '\t')
                    .Select(it => short.Parse(it))
                    .ToList();

                signal.Data.Add(values[index]);
            }

            return signal;
        }
    }
}
