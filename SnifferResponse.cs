using System;

namespace sniffer
{
    public class SnifferResponse
    {
        public SnifferResponse() { }

        public SnifferResponse(String srcport, String payload)
        {
            this.srcport = srcport;
            this.payload = payload;
        }
        public string srcport { get; set; }
        public string payload { get; set; }

    }
}