namespace SleepController.Domain
{
    public readonly struct EEGEntry
    {
        public short SignalO1A1 { get; }

        public EEGEntry(short signalO1A1)
            : this()
        {
            SignalO1A1 = signalO1A1;
        }
    }
}
