using System;

namespace SleepController.Domain
{
    public class EEGChannelAttribute : Attribute
    {
        public EEGChannelAttribute(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; set; }

        public string Name { get; set; }
    }
}
