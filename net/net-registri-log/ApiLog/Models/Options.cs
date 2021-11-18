using System.Collections.Generic;

namespace net_registri_log.ApiLog.Models
{
    public class Options
    {
        public bool Enable { get; set; }
        public bool TrackRequestBody { get; set; }
        public bool TrackResponseBody { get; set; }
        public List<string> IgnorePath { get; set; } = new List<string>();
    }
}
