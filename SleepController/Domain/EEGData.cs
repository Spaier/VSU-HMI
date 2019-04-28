namespace SleepController.Domain
{
    public readonly struct EEGData
    {
        public short Signal01A1 { get; }

        public short Signal0zA2 { get; }

        public short Signal02A2 { get; }

        public short SignalA1A2 { get; }

        public short SignalExt { get; }
    }
}
