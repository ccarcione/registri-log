using System;
using System.ComponentModel.DataAnnotations;

namespace net_registri_log.ApiLog.Models
{
    public class ApiObject
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        [MaxLength(8)]
        public string Method { get; set; }
        [MaxLength(2083)]
        public string Url { get; set; }
        [MaxLength(1024)]
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
