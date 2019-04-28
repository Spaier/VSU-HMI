namespace SleepController.Domain
{
    public readonly struct EEGEntry
    {
        [EEGChannel(4, "O1A1")]
        public short SignalO1A1 { get; }

        [EEGChannel(10, "A1A2")]
        public short SignalA1A2 { get; }

        [EEGChannel(15, "O2A2")]
        public short SignalO2A2 { get; }

        [EEGChannel(21, "OzA2")]
        public short SignalOzA2 { get; }

        public EEGEntry(short signalO1A1, short signalA1A2, short signalO2A2, short signalOzA2)
            : this()
        {
            SignalO1A1 = signalO1A1;
            SignalA1A2 = signalA1A2;
            SignalO2A2 = signalO2A2;
            SignalOzA2 = signalOzA2;
        }
    }
}
