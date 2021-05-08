namespace InfoMonitor.DataSchema
{
    public class ComputerProperties
    {
        public string MachineName { get; set; }
        public ulong TotalRAMMegabytes { get; set; }
        public uint TotalLogicalCPUCores { get; set; }
        public uint AverageCurrentClockSpeed { get; set; }

    }
}
