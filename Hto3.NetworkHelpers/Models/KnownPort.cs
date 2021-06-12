using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Hto3.NetworkHelpers.Models
{
    /// <summary>
    /// Known port information.
    /// </summary>
    public class KnownPort
    {
        internal KnownPort(String serviceName, Int32 port, ProtocolType protocol, String description)
        {
            this.ServiceName = serviceName;
            this.Port = port;
            this.Protocol = protocol;
            this.Description = description;
        }

        /// <summary>
        /// Service name related with the port.
        /// </summary>
        public String ServiceName { get; private set; }
        /// <summary>
        /// Port number.
        /// </summary>
        public Int32 Port { get; private set; }
        /// <summary>
        /// Protocol (UDP or TCP).
        /// </summary>
        public ProtocolType Protocol { get; private set; }
        /// <summary>
        /// Service description.
        /// </summary>
        public String Description { get; private set; }
    }
}
