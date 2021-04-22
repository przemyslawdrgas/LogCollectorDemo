using LogCollectorDemo.Core.Enums;

namespace LogCollectorDemo.Core.Models
{
    public class Log
    {
        public string Id { get; set; }
        public LogState State { get; set; }
        public long Timestamp { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
    }
}
