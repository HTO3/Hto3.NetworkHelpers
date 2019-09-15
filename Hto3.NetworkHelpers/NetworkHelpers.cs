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
        public static IEnumerable<IPAddress> GetIPAddresses()
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
                    var client = new WebClient();
                    var servicos = new[] { "http://ipinfo.io/ip", "http://ipecho.net/plain", "https://api.ipify.org/", "http://bot.whatismyipaddress.com/" };

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
    }
}
