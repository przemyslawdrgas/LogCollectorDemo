namespace LogCollectorDemo.Core.Entities
{
    public class EventEntity : IEntity
    {
        public string Id { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
        public bool Alert { get; set; }

        public EventEntity(string id, int duration, string type, string host, bool alert)
        {
            Id = id;
            Duration = duration;
            Type = type;
            Host = host;
            Alert = alert;
        }
    }
}
