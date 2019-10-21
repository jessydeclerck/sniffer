using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace sniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                WriteStdout(new SnifferInformation("No interfaces found! Make sure WinPcap is installed."));
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                if (device.Description != null)
                    WriteStdout(new InterfaceInfo((i + 1).ToString(), device.Name, device.Description));
                else
                    WriteStdout(new SnifferInformation("(No description available)"));
            }

            int deviceIndex = 0;
            do
            {
                WriteStdout(new StdinRequest("Enter the interface number (1-" + allDevices.Count + "):"));
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > allDevices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);

            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[deviceIndex - 1];

            // Open the device
            using (PacketCommunicator communicator =
                selectedDevice.Open(2000,                                  // portion of the packet to capture
                                                                           // 65536 guarantees that the whole packet will be captured on all the link layers
                                    PacketDeviceOpenAttributes.NoCaptureLocal | PacketDeviceOpenAttributes.NoCaptureRemote, // promiscuous mode
                                    100))                                  // read timeout
            {
                // Check the link layer. We support only Ethernet for simplicity.
                if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                {
                    WriteStdout(new SnifferInformation("This program works only on Ethernet networks."));
                    return;
                }

                // Compile the filter
                using (BerkeleyPacketFilter filter = communicator.CreateFilter("port 443"))
                {
                    // Set the filter
                    communicator.SetFilter(filter);
                }

                WriteStdout(new Info("Listening on " + selectedDevice.Description + "..."));

                communicator.SetKernelBufferSize(10000000);
                communicator.SetKernelMinimumBytesToCopy(50);
                // start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }

        }
        // Callback function invoked by libpcap for every incoming packet
        private static void PacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            TcpDatagram tcp = ip.Tcp;

            // print srcport and tcp payload in json format
            if (tcp.PayloadLength > 0)
            {
                WriteStdout(new SnifferMsg(tcp.SourcePort.ToString(), tcp.Payload.ToHexadecimalString()));
            }
        }

        private static void WriteStdout(StdoutMsg msg)
        {
            msg.type = msg.GetType().ToString();
            Console.WriteLine(JsonConvert.SerializeObject(msg));
        }

    }
}
