using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
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
        public static IEnumerable<IPAddress> GetLocalIPAddresses()
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
        public static Task<IPAddress> GetExternalIPv4AddressAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Factory.StartNew(() =>
                {
                    var client = new WebClient();
                    var servicos = new[] { "http://ipinfo.io/ip", "http://ipecho.net/plain", "https://api.ipify.org/", "http://bot.whatismyipaddress.com/", "http://ipv4.icanhazip.com/" };

                    foreach (var servico in servicos)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        try
                        {
                            var responseAsString = client.DownloadString(servico);
                            var match = Regex.Match(responseAsString, @"(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");

                            if (!match.Success)
                                continue;
                            else
                                return IPAddress.Parse(match.Value);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    throw new InvalidOperationException("Não foi possível obter o endereço de IP externo. Verifique a sua conexão de internet.");
                }
                ,
                cancellationToken);
        }
        /// <summary>
        /// Get the local ip address to reach the Internet.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<IPAddress> GetLocalIPv4AddressToReachInternet(CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Factory.StartNew(() =>
                {
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

        //    static Result<IPAddress> GetLocalIpAddressWithoutInternet(string localNetworkCidrIp)
        //    {
        //        var localIps = GetLocalIPv4AddressList();
        //        if (localIps.Count == 1)
        //        {
        //            return Result.Ok(localIps[0]);
        //        }

        //        foreach (var ip in localIps)
        //        {
        //            var checkIp = ip.IsInRange(localNetworkCidrIp);
        //            if (!checkIp.Success) continue;
        //            if (!checkIp.Value) continue;

        //            return Result.Ok(ip);
        //        }

        //        return Result.Fail<IPAddress>("Unable to determine local IP address");
        //    }

        //    static List<IPAddress> GetLocalIPv4AddressList()
        //    {
        //        var localIps = new List<IPAddress>();
        //        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        //        {
        //            var ips =
        //                nic.GetIPProperties().UnicastAddresses
        //                    .Select(uni => uni.Address)
        //                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();

        //            localIps.AddRange(ips);
        //        }

        //        return localIps;
        //    }

        //    public static Result<List<IPAddress>> ParseIPv4Addresses(string input)
        //    {
        //        const string ipV4Pattern =
        //            @"(?:(?:1\d\d|2[0-5][0-5]|2[0-4]\d|0?[1-9]\d|0?0?\d)\.){3}(?:1\d\d|2[0-5][0-5]|2[0-4]\d|0?[1-9]\d|0?0?\d)";

        //        if (string.IsNullOrEmpty(input))
        //        {
        //            return Result.Fail<List<IPAddress>>("Input string cannot be null");
        //        }

        //        var ips = new List<IPAddress>();
        //        try
        //        {
        //            var regex = new Regex(ipV4Pattern);
        //            foreach (Match match in regex.Matches(input))
        //            {
        //                var parse = ParseSingleIPv4Address(match.Value);
        //                if (parse.Failure)
        //                {
        //                    return Result.Fail<List<IPAddress>>(parse.Error);
        //                }

        //                ips.Add(parse.Value);
        //            }
        //        }
        //        catch (RegexMatchTimeoutException ex)
        //        {
        //            return Result.Fail<List<IPAddress>>($"{ex.Message} ({ex.GetType()}) raised in method IpAddressHelper.ParseIPv4Addresses");
        //        }

        //        return ips.Count > 0
        //            ? Result.Ok(ips)
        //            : Result.Fail<List<IPAddress>>("Input string did not contain any valid IPv4 addresses");
        //    }

        /// <summary>
        /// Validate a CIDR Ip string (i.e "10.0.0.0/24")
        /// </summary>
        /// <param name="cidrIp">An IP address with the format 0.0.0.0/0</param>
        /// <returns></returns>
        public static Boolean ValidateCIDRIp(String cidrIp)
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

    //    // true if ipAddress falls inside the range defined by cidrIp, example:
    //    // bool result = IsInCidrRange("192.168.2.3", "192.168.2.0/24"); // result = true
    //    public static Result<bool> IpAddressIsInRange(string checkIp, string cidrIp)
    //    {
    //        if (string.IsNullOrEmpty(checkIp))
    //        {
    //            throw new ArgumentException("Input string must not be null", checkIp);
    //        }

    //        var parseIp = ParseIPv4Addresses(checkIp);
    //        if (parseIp.Failure)
    //        {
    //            return Result.Fail<bool>($"Unable to parse IP address from input string {checkIp}");
    //        }

    //        return IpAddressIsInRange(parseIp.Value[0], cidrIp);
    //    }

    //    public static Result<bool> IpAddressIsInRange(IPAddress checkIp, string cidrIp)
    //    {
    //        var cidrIpNull = $"CIDR IP address was null or empty string, {cidrIp}";
    //        var cidrIpParseError = $"Unable to parse IP address from input string {cidrIp}";
    //        var cidrIpSplitError = $"cidrIp was not in the correct format:\nExpected: a.b.c.d/n\nActual: {cidrIp}";
    //        var cidrMaskParseError1 = $"Unable to parse netmask bit count from {cidrIp}";
    //        const string cidrMaskParseError2 = "Netmask bit count value is invalid, must be in range 0-32";

    //        if (string.IsNullOrEmpty(cidrIp))
    //        {
    //            return Result.Fail<bool>(cidrIpNull);
    //        }

    //        var parseIp = ParseIPv4Addresses(cidrIp);
    //        if (parseIp.Failure)
    //        {
    //            return Result.Fail<bool>(cidrIpParseError);
    //        }

    //        var cidrAddress = parseIp.Value[0];

    //        var parts = cidrIp.Split('/');
    //        if (parts.Length != 2)
    //        {
    //            return Result.Fail<bool>(cidrIpSplitError);
    //        }

    //        if (!Int32.TryParse(parts[1], out var netmaskBitCount))
    //        {
    //            return Result.Fail<bool>(cidrMaskParseError1);
    //        }

    //        if (0 > netmaskBitCount || netmaskBitCount > 32)
    //        {
    //            return Result.Fail<bool>(cidrMaskParseError2);
    //        }

    //        var checkIpBytes = BitConverter.ToInt32(checkIp.GetAddressBytes(), 0);
    //        var cidrIpBytes = BitConverter.ToInt32(cidrAddress.GetAddressBytes(), 0);
    //        var cidrMaskBytes = IPAddress.HostToNetworkOrder(-1 << (32 - netmaskBitCount));

    //        var ipIsInRange = (checkIpBytes & cidrMaskBytes) == (cidrIpBytes & cidrMaskBytes);

    //        return Result.Ok(ipIsInRange);
    //    }

    //    public static Result<bool> IpAddressIsInPrivateAddressSpace(string ipAddress)
    //    {
    //        var parseIp = ParseIPv4Addresses(ipAddress);
    //        if (parseIp.Failure)
    //        {
    //            return Result.Fail<bool>($"Unable to parse IP address from {ipAddress}");
    //        }

    //        var ipIsInPrivateRange = IpAddressIsInPrivateAddressSpace(parseIp.Value[0]);

    //        return Result.Ok(ipIsInPrivateRange);
    //    }

    //    public static bool IpAddressIsInPrivateAddressSpace(IPAddress ipAddress)
    //    {
    //        var inPrivateBlockA = IpAddressIsInRange(ipAddress, CidrPrivateAddressBlockA).Value;
    //        var inPrivateBlockB = IpAddressIsInRange(ipAddress, CidrPrivateAddressBlockB).Value;
    //        var inPrivateBlockC = IpAddressIsInRange(ipAddress, CidrPrivateAddressBlockC).Value;

    //        return inPrivateBlockA || inPrivateBlockB || inPrivateBlockC;
    //    }

    //    public static AddressType GetAddressType(IPAddress ipAddress)
    //    {
    //        return IpAddressIsInPrivateAddressSpace(ipAddress)
    //            ? AddressType.Private
    //            : AddressType.Public;
    //    }

    //    public static Result<string> GetCidrIp()
    //    {
    //        var platform = Environment.OSVersion.Platform.ToString();

    //        var ipAddressInfoList = platform.Contains("win", StringComparison.OrdinalIgnoreCase)
    //            ? GetWindowsUnicastAddressInfoList()
    //            : GetUnixUnicastAddressInfoList();

    //        if (ipAddressInfoList.Count == 0)
    //        {
    //            const string error =
    //                "No IPv4 addresses are assocaited with any network adapters " +
    //                "on this machine, unable to determine CIDR IP";

    //            return Result.Fail<string>(error);
    //        }

    //        if (ipAddressInfoList.Count > 1)
    //        {
    //            return Result.Fail<string>("More than one ethernet adapters found, unable to determine CIDR IP");
    //        }

    //        return platform.Contains("win", StringComparison.OrdinalIgnoreCase)
    //            ? GetCidrIpFromWindowsIpAddressInformation(ipAddressInfoList[0])
    //            : GetCidrIpFromUnixIpAddressInformation(ipAddressInfoList[0]);
    //    }

    //    static List<UnicastIPAddressInformation> GetUnixUnicastAddressInfoList()
    //    {
    //        var ethernetAdapters = NetworkInterface.GetAllNetworkInterfaces().Select(nic => nic)
    //            .Where(nic => nic.Name.StartsWith("en", StringComparison.Ordinal)).ToList();

    //        var ipV4List = new List<UnicastIPAddressInformation>();
    //        foreach (var nic in ethernetAdapters)
    //        {
    //            var ips =
    //                nic.GetIPProperties().UnicastAddresses
    //                    .Select(ipInfo => ipInfo)
    //                    .Where(ipInfo => ipInfo.Address.AddressFamily == AddressFamily.InterNetwork).ToList();

    //            ipV4List.AddRange(ips);
    //        }

    //        return ipV4List;
    //    }

    //    static List<UnicastIPAddressInformation> GetWindowsUnicastAddressInfoList()
    //    {
    //        var ethernetAdapters = NetworkInterface.GetAllNetworkInterfaces().Select(nic => nic)
    //            .Where(nic => nic.Name.Contains("ethernet", StringComparison.OrdinalIgnoreCase)
    //                          || nic.Description.Contains("ethernet", StringComparison.OrdinalIgnoreCase)).ToList();

    //        var ipV4List = new List<UnicastIPAddressInformation>();
    //        foreach (var nic in ethernetAdapters)
    //        {
    //            var ips =
    //                nic.GetIPProperties().UnicastAddresses
    //                    .Select(ipInfo => ipInfo)
    //                    .Where(ipInfo => ipInfo.Address.AddressFamily == AddressFamily.InterNetwork).ToList();

    //            ipV4List.AddRange(ips);
    //        }

    //        return ipV4List;
    //    }

    //    static Result<string> GetCidrIpFromWindowsIpAddressInformation(UnicastIPAddressInformation ipInfo)
    //    {
    //        var ipAddress = ipInfo.Address;
    //        var networkBitCount = ipInfo.PrefixLength;

    //        return Result.Ok(GetCidrIpFromIpAddressAndNetworkBitCount(ipAddress, networkBitCount));
    //    }

    //    static Result<string> GetCidrIpFromUnixIpAddressInformation(UnicastIPAddressInformation ipInfo)
    //    {
    //        var getNetworkBitCount = GetNetworkBitCountFromSubnetMask(ipInfo.IPv4Mask);
    //        if (getNetworkBitCount.Failure)
    //        {
    //            return Result.Fail<string>("Unable to determine CIDR IP from available network adapter information");
    //        }

    //        var networkBitCount = getNetworkBitCount.Value;

    //        return Result.Ok(GetCidrIpFromIpAddressAndNetworkBitCount(ipInfo.Address, networkBitCount));
    //    }

    //    static Result<int> GetNetworkBitCountFromSubnetMask(IPAddress subnetMask)
    //    {
    //        var binaryArray = ConvertIpAddressToBinary(subnetMask, false).ToCharArray();

    //        if (binaryArray.Length == 0 || binaryArray.Length != 32)
    //        {
    //            return Result.Fail<int>("Binary string was not in the expected format.");
    //        }

    //        if (binaryArray[0] != '1')
    //        {
    //            return Result.Fail<int>("Binary string was not in the expected format.");
    //        }

    //        var onesCount = 0;
    //        var zerosCount = 0;
    //        var firstZeroEncountered = false;

    //        foreach (var bit in binaryArray)
    //        {
    //            if (!firstZeroEncountered)
    //            {
    //                if (bit == '1')
    //                {
    //                    onesCount++;
    //                }

    //                if (bit == '0')
    //                {
    //                    firstZeroEncountered = true;
    //                    zerosCount++;
    //                }
    //            }
    //            else
    //            {
    //                if (bit == '1')
    //                {
    //                    break;
    //                }

    //                if (bit == '0')
    //                {
    //                    zerosCount++;
    //                }
    //            }
    //        }

    //        var totalBits = onesCount + zerosCount;
    //        if (totalBits != 32)
    //        {
    //            return Result.Fail<int>("Binary string was not in the expected format.");
    //        }

    //        return Result.Ok(onesCount);
    //    }

    //    static string GetCidrIpFromIpAddressAndNetworkBitCount(IPAddress address, int networkBitCount)
    //    {
    //        var ipAddressBytes = address.GetAddressBytes();
    //        var networkIdBytes = new byte[4];

    //        foreach (var i in Enumerable.Range(0, ipAddressBytes.Length))
    //        {
    //            var byteArray = Convert.ToString(ipAddressBytes[i], 2).PadLeft(8, '0').ToCharArray();
    //            foreach (var j in Enumerable.Range(0, byteArray.Length))
    //            {
    //                var bitNumber = i * 8 + j + 1;
    //                if (bitNumber > networkBitCount)
    //                {
    //                    byteArray[j] = '0';
    //                }
    //            }

    //            var byteString = new string(byteArray);
    //            networkIdBytes[i] = Convert.ToByte(byteString, 2);
    //        }

    //        var networkId = new IPAddress(networkIdBytes);
    //        return $"{networkId}/{networkBitCount}";
    //    }

    //    public static Result<string> ConvertIpAddressToBinary(string ip, bool separateBytes)
    //    {
    //        var parseResult = ParseIPv4Addresses(ip);
    //        if (parseResult.Failure)
    //        {
    //            return Result.Fail<string>(parseResult.Error);
    //        }

    //        var binary = ConvertIpAddressToBinary(parseResult.Value[0], separateBytes);

    //        return Result.Ok(binary);
    //    }

    //    public static string ConvertIpAddressToBinary(IPAddress ip, bool separateBytes)
    //    {
    //        var bytes = ip.GetAddressBytes();
    //        var s = string.Empty;

    //        foreach (var i in Enumerable.Range(0, bytes.Length))
    //        {
    //            var oneByte = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
    //            s += oneByte;

    //            if (!separateBytes) continue;

    //            if (!i.IsLastIteration(bytes.Length))
    //            {
    //                s += " - ";
    //            }
    //        }

    //        return s;
    //    }

    }
}
