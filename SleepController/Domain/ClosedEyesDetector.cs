using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepController.Domain
{
    public class ClosedEyesDetector
    {
        public int ClosedEyesMinThreshold => 40;

        public int NextWeight => 2;

        public int PreviousWeight => 1;

        public int FloatingAverage { get; protected set; }

        public bool IsClosed(IEnumerable<EEGEntry> batch, out int average)
        {
            var signalO1A1 = batch.Select(it => it.SignalO1A1);
            average = signalO1A1.Sum(it => it) / signalO1A1.Count();

            FloatingAverage = (PreviousWeight * FloatingAverage + NextWeight * average)
                / (NextWeight + PreviousWeight);

            // var antialiasing = Math.Abs(FloatingAverage - average);
            return signalO1A1
                .AsParallel()
                .Any(it => Math.Abs(it - FloatingAverage) >= ClosedEyesMinThreshold);
        }
    }
}
