using System;

namespace sniffer
{
    public class StdinRequest : StdoutMsg
    {
        public StdinRequest() { }

        public StdinRequest(String msg)
        {
            this.msg = msg;
        }
        public string msg { get; set; }
    }
}