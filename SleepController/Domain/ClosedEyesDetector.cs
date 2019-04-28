using System;
using System.Collections.Generic;
using System.Linq;

namespace SleepController.Domain
{
    public class ClosedEyesDetector
    {
        public int ClosedEyesThreshold => 30;

        public bool IsClosed(IEnumerable<EEGEntry> batch)
        {
            var ebola = batch.Select(it => it.SignalO1A1).ToList();
            var average = ebola.Sum(it => Math.Abs(it)) / batch.Count();

            return average >= ClosedEyesThreshold;
        }
    }
}
