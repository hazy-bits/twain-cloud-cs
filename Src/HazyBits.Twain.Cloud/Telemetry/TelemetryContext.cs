using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HazyBits.Twain.Cloud.Telemetry
{
    public class TelemetryContext
    {
        public string TypeContext { get; set; }

        public int ThreadId { get; set; }

        public Dictionary<string, string> Props { get; set; }

        [JsonIgnore]
        public Type Type { get; set; }
    }
}
