using System;
using System.Collections.Generic;
using System.Linq;

namespace SleepController.Domain
{
    public class ClosedEyesDetector
    {
        public int ClosedEyesMinThreshold => 20;

        public int ClosedEyesMaxThreshold => 45;

        public bool IsClosed(IEnumerable<EEGEntry> batch)
        {
            var average = batch.Select(it => it.SignalO1A1)
                .Sum(it => Math.Abs(it)) / batch.Count();

            return average >= ClosedEyesMinThreshold && average <= ClosedEyesMaxThreshold;
        }
    }
}
