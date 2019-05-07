using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepController.Domain
{
    public class ClosedEyesDetector
    {
        public int ClosedEyesMinThreshold => 30;

        public int NextWeight => 2;

        public int PreviousWeight => 1;

        public int FloatingAverage { get; protected set; }

        public int Average { get; protected set; }

        public bool IsClosed(IEnumerable<EEGEntry> batch)
        {
            var signalO1A1 = batch.Select(it => it.SignalO1A1);
            Average = signalO1A1.Sum(it => it) / signalO1A1.Count();
            FloatingAverage = (PreviousWeight * FloatingAverage + NextWeight * Average)
                / (NextWeight + PreviousWeight);

            // var antialiasing = Math.Abs(FloatingAverage - average);
            return signalO1A1
                .AsParallel()
                .Any(it => Math.Abs(it - FloatingAverage) >= ClosedEyesMinThreshold);
        }
    }
}
