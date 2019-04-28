using System.Collections.Generic;

namespace SleepController.Domain
{
    public class EEG
    {
        public int Frequency { get; set; }

        public List<EEGEntry> Data { get; set; } = new List<EEGEntry>();
    }
}
