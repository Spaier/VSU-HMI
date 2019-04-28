namespace SleepController.Domain
{
    public readonly struct EEGData
    {
        [EEGChannel(4, "O1A1")]
        public short SignalO1A1 { get; }

        [EEGChannel(10, "A1A2")]
        public short SignalA1A2 { get; }

        [EEGChannel(15, "O2A2")]
        public short SignalO2A2 { get; }

        [EEGChannel(21, "OzA2")]
        public short SignalOzA2 { get; }
    }
}
