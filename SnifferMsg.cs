using System;

namespace sniffer
{
    public class SnifferMsg : StdoutMsg
    {
        public SnifferMsg() { }

        public SnifferMsg(String srcport, String payload)
        {
            this.srcport = srcport;
            this.payload = payload;
        }
        public string srcport { get; set; }
        public string payload { get; set; }

    }
}