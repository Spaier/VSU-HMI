namespace SleepController.Domain
{
    public readonly struct EEGData
    {
        [EEGChannelIndex(4)]
        public short SignalO1A1 { get; }

        [EEGChannelIndex(10)]
        public short SignalA1A2 { get; }

        [EEGChannelIndex(15)]
        public short SignalO2A2 { get; }

        [EEGChannelIndex(21)]
        public short SignalOzA2 { get; }
    }
}
