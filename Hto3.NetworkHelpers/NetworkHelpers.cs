using Hto3.NetworkHelpers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers
{
    /// <summary>
    /// Methods to solve common network tasks
    /// </summary>
    public static class NetworkHelpers
    {
        /// <summary>
        /// Get all lan IPv4 addreesses of this machine.
        /// </summary>
        /// <returns>Coleção não materializada dos IPs da máquina</returns>
        public static IEnumerable<IPAddress> GetLocalIPv4Addresses()
        {
            return
                Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .Where(al => !al.IsIPv6Teredo)
                    .Where(al => !al.IsIPv6SiteLocal)
                    .Where(al => !al.IsIPv6Multicast)
                    .Where(al => !al.IsIPv6LinkLocal)
                    .Where(al => al.AddressFamily == AddressFamily.InterNetwork);
        }
        /// <summary>
        /// Get host name through IP address.
        /// </summary>
        /// <param name="ip">A string that contains an IP address in dotted-quad notation for IPv4 and in colon-hexadecimal notation for IPv6.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<String> GetHostNameThroughIPAddressAsync(String ip, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetHostNameThroughIPAddressAsync(IPAddress.Parse(ip));
        }
        /// <summary>
        /// Get host name through IP address.
        /// </summary>
        /// <param name="ip">Target IP address</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<String> GetHostNameThroughIPAddressAsync(IPAddress ip, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.StartNew(() => Dns.GetHostEntry(ip).HostName, cancellationToken);
        }
        /// <summary>
        /// Get external IP address of this machine.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<IPAddress> GetExternalIPAddressAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Factory.StartNew(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();                    
                    using (var client = new WebClient())
                    {
                        var responseAsString = client.DownloadString("https://icanhazip.com/");//Alternative https://api.ipify.org/
                        return IPAddress.Parse(responseAsString.Trim());
                    }
                }
                ,
                cancellationToken);
        }
        /// <summary>
        /// Returns the reverse DNS record (PTR) for your external IP.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<String> GetExternalDNSRecordAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Factory.StartNew(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var client = new WebClient())
                    {
                        var responseAsString = client.DownloadString("http://icanhazptr.com/");
                        return responseAsString.Trim();
                    }                  
                }
                ,
                cancellationToken);
        }
        /// <summary>
        /// Get the local IP address to reach the Internet.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<IPAddress> GetLocalIPv4AddressToReachInternet(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Factory.StartNew(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("8.8.8.8", 65530);
                        if (!(socket.LocalEndPoint is IPEndPoint endPoint))
                            throw new InvalidOperationException($"Error occurred casting {socket.LocalEndPoint} to IPEndPoint");

                        return endPoint.Address;
                    }
                },
                cancellationToken);
        }
        /// <summary>
        /// Validate a CIDR Ip string (i.e "10.0.0.0/24")
        /// </summary>
        /// <param name="cidrIp">An IP address with the format 0.0.0.0/0</param>
        /// <returns></returns>
        public static Boolean ValidateCIDRIP(String cidrIp)
        {
            if (String.IsNullOrWhiteSpace(cidrIp))
                return false;

            var parts = cidrIp.Split('/');
            
            if (parts.Length != 2)
                return false;

            if (!IPAddress.TryParse(parts[0], out _))
                return false;

            if (!Int32.TryParse(parts[1], out var netmaskBitCount))
                return false;

            if (netmaskBitCount < 0 || netmaskBitCount > 32)
                return false;

            return true;
        }

        /// <summary>
        /// This Function is Used to get Subnet based on NetMask (i.e 0-32).
        /// </summary>
        /// <param name="netMask">0-32</param>
        /// <returns></returns>
        public static IPAddress GetIpv4SubnetFromNetMask(Int32 netMask)
        {
            var subNetMask = default(String);
            var powResult = default(Double);
            var calSubNet = 32 - netMask;

            if (calSubNet >= 0 && calSubNet <= 8)
            {
                for (int ipower = 0; ipower < calSubNet; ipower++)
                    powResult += Math.Pow(2, ipower);
                
                var finalSubnet = 255 - (Int32)powResult;
                subNetMask = $"255.255.255.{finalSubnet}";
            }
            else if (calSubNet > 8 && calSubNet <= 16)
            {
                int secOctet = 16 - calSubNet;
                secOctet = 8 - secOctet;

                for (int ipower = 0; ipower < secOctet; ipower++)
                     powResult += Math.Pow(2, ipower);
                
                var finalSubnet = 255 - (Int32)powResult;
                subNetMask = $"255.255.{finalSubnet}.0";
            }
            else if (calSubNet > 16 && calSubNet <= 24)
            {
                int thirdOctet = 24 - calSubNet;
                thirdOctet = 8 - thirdOctet;

                for (int ipower = 0; ipower < thirdOctet; ipower++)
                    powResult += Math.Pow(2, ipower);
                
                var finalSubnet = 255 - (Int32)powResult;
                subNetMask = $"255.{finalSubnet}.0.0";
            }
            else if (calSubNet > 24 && calSubNet <= 32)
            {
                int fourthOctet = 32 - calSubNet;
                fourthOctet = 8 - fourthOctet;

                for (int ipower = 0; ipower < fourthOctet; ipower++)
                    powResult += Math.Pow(2, ipower);
                
                var finalSubnet = 255 - (Int32)powResult;
                subNetMask = $"{finalSubnet}.0.0.0";
            }
            else
                throw new InvalidOperationException($"Invalid NetMask {netMask}, must be in range 0-32.");

            return IPAddress.Parse(subNetMask);
        }
        /// <summary>
        /// Check if the provided IPv4 is in range of the CIDR Ip.
        /// </summary>
        /// <param name="checkIp"></param>
        /// <param name="cidrIp"></param>
        /// <returns></returns>
        public static Boolean IsIpv4AddressInRange(IPAddress checkIp, String cidrIp)
        {
            if (!ValidateCIDRIP(cidrIp))
                throw new ArgumentException($"cidrIp was not in the correct format:\nExpected: a.b.c.d/n\nActual: {cidrIp}", nameof(cidrIp));

            var parts = cidrIp.Split('/');
            var cidrAddress = IPAddress.Parse(parts[0]);            
            var netmaskBitCount = Int32.Parse(parts[1]);
            var checkIpBytes = BitConverter.ToInt32(checkIp.GetAddressBytes(), 0);
            var cidrIpBytes = BitConverter.ToInt32(cidrAddress.GetAddressBytes(), 0);
            var cidrMaskBytes = IPAddress.HostToNetworkOrder(-1 << (32 - netmaskBitCount));

            var ipIsInRange = (checkIpBytes & cidrMaskBytes) == (cidrIpBytes & cidrMaskBytes);

            return ipIsInRange;
        }
        /// <summary>
        /// Check if the provided IPv4 is in the private address space.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static Boolean IsIpv4AddressInPrivateAddressSpace(IPAddress ipAddress)
        {
            const String CIDR_PRIVATE_ADDRESS_BLOCK_A = "10.0.0.0/8";
            const String CIDR_PRIVATE_ADDRESS_BLOCK_B = "172.16.0.0/12";
            const String CIDR_PRIVATE_ADDRESS_BLOCK_C = "192.168.0.0/16";

            return
                IsIpv4AddressInRange(ipAddress, CIDR_PRIVATE_ADDRESS_BLOCK_A)
                ||
                IsIpv4AddressInRange(ipAddress, CIDR_PRIVATE_ADDRESS_BLOCK_B)
                ||
                IsIpv4AddressInRange(ipAddress, CIDR_PRIVATE_ADDRESS_BLOCK_C);
        }
        /// <summary>
        /// Check if the provided IPv4 is in the public address space.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static Boolean IsIpv4AddressInPublicAddressSpace(IPAddress ipAddress)
        {
            return !IsIpv4AddressInPrivateAddressSpace(ipAddress);
        }
        /// <summary>
        /// Get the network interface vendor name by MAC address. Null if not found.
        /// </summary>
        /// <param name="macAddress">Hex formated MAC Address as 00-00-00-00-00-00 or 00:00:00:00:00:00.</param>
        /// <returns></returns>
        public static String GetNetworkInterfaceVendorNameByMACAddress(String macAddress)
        {
            if (String.IsNullOrWhiteSpace(macAddress))
                throw new ArgumentNullException("The MAC address seems to be empty.");
            if (macAddress.Length != 17)
                throw new ArgumentException("The MAC address length must be 17 chars.");

            var line = default(String);
            //http://standards-oui.ieee.org/oui/oui.txt
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hto3.NetworkHelpers.Resources.oui.txt");
            using (var streamReader = new StreamReader(resourceStream, Encoding.UTF8, false, 1024))
            {
                var oui = macAddress
                    .Substring(0, 8)
                    .ToUpper()
                    .Replace(':', '-');

                do
                {
                    line = streamReader.ReadLine();
                }
                while
                (
                    line != null
                    &&
                    !line.StartsWith(oui)
                );
            }
            
            return line?.Substring(line.IndexOf("\t\t") + 2);
        }
        /// <summary>
        /// Get information about a known port. Null if not found.
        /// </summary>
        /// <param name="port">Port number.</param>
        /// <param name="protocolType">Protocol type (TCP or UDP only!).</param>
        /// <returns></returns>
        public static KnownPort GetKnownPort(Int32 port, ProtocolType protocolType)
        {
            if (protocolType != ProtocolType.Tcp && protocolType != ProtocolType.Udp)
                throw new ArgumentException("The protocol must be TCP OR UDP.");

            var line = default(String);
            //https://www.iana.org/assignments/service-names-port-numbers/service-names-port-numbers.csv
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hto3.NetworkHelpers.Resources.service-names-port-numbers.csv");
            using (var streamReader = new StreamReader(resourceStream, Encoding.UTF8, false, 1024))
            {
                var portFormat = $",{port},";
                var protFormat = $",{protocolType.ToString().ToLower()},";

                do
                {
                    line = streamReader.ReadLine();
                }
                while
                (
                    line != null
                    &&
                    !(
                        line.Contains(portFormat)
                        &&
                        line.Contains(protFormat)
                    )
                );
            }

            if (line == null)
                return null;

            var lineValues = line.Split(',');

            if (lineValues.Length < 4)
                return null;

            if (lineValues[3] == "Unassigned")
                return null;

            return new KnownPort(lineValues[0], port, protocolType, lineValues[3]);
        }
    }
}
