using System.Collections.Generic;

namespace SleepController.Domain
{
    public class Signal
    {
        public int Frequency { get; set; }

        public List<short> Data { get; set; } = new List<short>();
    }
}
