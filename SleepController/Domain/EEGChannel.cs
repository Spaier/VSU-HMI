using System;

namespace SleepController.Domain
{
    public class EEGChannel : Attribute
    {
        public EEGChannel(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public int Value { get; set; }

        public string Name { get; set; }
    }
}
