using System;

namespace net_registri_log.AuditLog.Models
{
    public class FiltriAuditLog
    {
        //public DateTime? DataDa { get; set; }
        //public DateTime? DataA { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public bool Desc { get; set; }
        public string OrderColumn { get; set; }
    }
}
