using System;

namespace net_registri_log.Logs.Models
{
    public class FiltriLogs
    {
        //public DateTime? DataDa { get; set; }
        //public DateTime? DataA { get; set; }
        public string Level { get; set; }
        public string UserName { get; set; }
        public string Operation { get; set; }
        public bool Desc { get; set; }
        public string OrderColumn { get; set; }
    }
}
