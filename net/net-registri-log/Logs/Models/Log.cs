using System;

namespace net_registri_log.Logs.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
        public string UserName { get; set; }
        public string Operation { get; set; }
        public string JsonObject { get; set; }
    }
}
