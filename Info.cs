using System;

namespace sniffer
{
    public class Info : StdoutMsg
    {
        public Info() { }

        public Info(String info)
        {
            this.info = info;
        }
        public string info { get; set; }

    }
}