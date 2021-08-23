using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class ErrorLog : GuidEntitiy
    {
        public DateTime timestamp { get; set; }
        public string level { get; set; }
        public string stacktrace { get; set; }
        public string message { get; set; }
        public string AppName { get; set; }
    }
}
