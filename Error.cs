using System;

namespace sniffer
{
    public class SnifferInformation : StdoutMsg
    {
        public SnifferInformation() { }

        public SnifferInformation(String errorDetails)
        {
            this.errorDetails = errorDetails;
        }
        public string errorDetails { get; set; }

    }
}