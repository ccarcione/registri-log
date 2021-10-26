using System;

namespace net_registri_log.ApiLog.Models
{
    public class ApiObject
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        /// <summary>
        /// Body size in kb.
        /// </summary>
        public float? RequestSize { get; set; }
        public string ResponseBody { get; set; }
        /// <summary>
        /// Body size in kb.
        /// </summary>
        public float? ResponseSize { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
