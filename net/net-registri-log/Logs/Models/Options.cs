namespace net_registri_log.Logs.Models
{
    public class Options
    {
        public bool Enable { get; set; }
        public bool SerilogDebuggingSelfLogEnable { get; set; }
        public string SerilogSelfFileName { get; set; }
    }
}
