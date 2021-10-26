using System;

namespace net_registri_log.ApiLog.Models
{
    public class FiltriApiLog
    {
        //public DateTime? DataDa { get; set; }
        //public DateTime? DataA { get; set; }
        public string UserId { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public bool Desc { get; set; }
        public string OrderColumn { get; set; }
    }
}
