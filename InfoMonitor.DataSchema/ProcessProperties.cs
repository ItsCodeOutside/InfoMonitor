namespace InfoMonitor.DataSchema
{
    public class ProcessProperties
    {
        public int ProcessId { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public bool Responding { get; set; }
        public long RAMBytes { get; set; }
    }
}
