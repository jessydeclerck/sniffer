using System;

namespace sniffer
{
    public class InterfaceInfo : StdoutMsg
    {
        public InterfaceInfo() { }

        public InterfaceInfo(String number, String name, String description)
        {
            this.number = number;
            this.name = name;
            this.description = description;
        }
        public string number { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}