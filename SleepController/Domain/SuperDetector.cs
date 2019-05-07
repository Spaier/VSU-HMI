using System;
using System.Collections.Generic;
using System.Linq;

namespace SleepController.Domain
{
    public class SuperDetector
    {
        public int Threshold { get; set; }

        public int NextWeight => 2;

        public int PreviousWeight => 1;

        public int FloatingAverage { get; protected set; }

        public int Average { get; protected set; }

        public bool Detect(IEnumerable<short> signal)
        {
            Average = signal.Sum(it => it) / signal.Count();
            FloatingAverage = (PreviousWeight * FloatingAverage + NextWeight * Average)
                / (NextWeight + PreviousWeight);

            return signal
                .AsParallel()
                .Any(it => Math.Abs(it - FloatingAverage) >= Threshold);
        }
    }
}
