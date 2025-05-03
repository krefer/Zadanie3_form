using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogStandardizer.Data
{
    public class LogEntry
    {
        public string OriginalDate { get; set; }
        public string Time { get; set; }
        public string LogLevel { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
    }
}
