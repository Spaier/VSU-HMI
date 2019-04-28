using System;

namespace SleepController.Domain
{
    public class EEGChannelIndex : Attribute
    {
        public EEGChannelIndex(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}
