![logo](https://raw.githubusercontent.com/HTO3/Hto3.NetworkHelpers/master/nuget-logo-small.png)

Hto3.NetworkHelpers
========================================

[![License](https://img.shields.io/github/license/HTO3/Hto3.NetworkHelpers)](https://github.com/HTO3/Hto3.NetworkHelpers/blob/master/LICENSE)
[![Hto3.NetworkHelpers](https://img.shields.io/nuget/v/Hto3.NetworkHelpers.svg)](https://www.nuget.org/packages/Hto3.NetworkHelpers/)
[![Downloads](https://img.shields.io/nuget/dt/Hto3.NetworkHelpers)](https://www.nuget.org/stats/packages/Hto3.NetworkHelpers?groupby=Version)
[![Build Status](https://travis-ci.org/HTO3/Hto3.NetworkHelpers.svg?branch=master)](https://travis-ci.org/HTO3/Hto3.NetworkHelpers)
[![codecov](https://codecov.io/gh/HTO3/Hto3.NetworkHelpers/branch/master/graph/badge.svg)](https://codecov.io/gh/HTO3/Hto3.NetworkHelpers)

Features
--------
Network helper methods.

### GetLocalIPv4Addresses

Get all lan IPv4 addreesses of this machine.

```csharp
IEnumerable<IPAddress> ipAddresses = NetworkHelpers.GetLocalIPv4Addresses();
```

### GetHostNameThroughIPAddressAsync

Get host name through IP address.

```csharp
String hostname = await NetworkHelpers.GetHostNameThroughIPAddressAsync("66.171.248.178");
```

### GetExternalIPAddressAsync

Get external IP address of this machine.

```csharp
IPAddress ipAddress = await NetworkHelpers.GetExternalIPAddressAsync();
```

### GetExternalDNSRecordAsync

Returns the reverse DNS record (PTR) for your external IP.

```csharp
String dnsRecord await NetworkHelpers.GetExternalDNSRecordAsync();
//something like a1799.dscb.akamai.net
```

### GetLocalIPv4AddressToReachInternet

Get the local ip address to reach the Internet.

```csharp
IPAddress ipAddress = NetworkHelpers.GetLocalIPv4AddressToReachInternet()
```

### ValidateCIDRIP

Validate a CIDR Ip string (i.e "10.0.0.0/24")

```csharp
const String WRONG_CIDR_IP = "192.168.1.1/";
Boolean result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);
//result is false
```
```csharp
const String VALID_CIDR_IP = "10.0.0.0/24";
Boolean result = NetworkHelpers.ValidateCIDRIP(VALID_CIDR_IP);
//result is true
```

### GetIpv4SubnetFromNetMask

This Function is Used to get Subnet based on NetMask (i.e 0-32).

```csharp
IPAddress result = NetworkHelpers.GetIpv4SubnetFromNetMask(23);
//Result is "255.255.254.0"
```

### IsIpv4AddressInRange

Check if the provided Ip is in range of the CIDR Ip.

```csharp
IPAddress IP = IPAddress.Parse("192.168.1.1");
String CIDR_IP_16 = "192.168.0.0/16";
Boolean result = NetworkHelpers.IsIpv4AddressInRange(IP, CIDR_IP_16);
//result is true
```
```csharp
IPAddress IP = IPAddress.Parse("10.0.0.1");
String CIDR_IP_24 = "192.168.0.0/24";
Boolean result = NetworkHelpers.IsIpv4AddressInRange(IP, CIDR_IP_24);
//result is false
```

### IsIpv4AddressInPrivateAddressSpace

Check if the provided Ip is in the private address space.

```csharp
IPAddress PRIVATE_IP_ADDRESS = IPAddress.Parse("192.168.1.1");
Boolean result = NetworkHelpers.IsIpv4AddressInPrivateAddressSpace(PRIVATE_IP_ADDRESS);
//result is true
```
```csharp
IPAddress PUBLIC_IP_ADDRESS = IPAddress.Parse("200.87.14.111");
Boolean result = NetworkHelpers.IsIpv4AddressInPrivateAddressSpace(PUBLIC_IP_ADDRESS);
//result is false
```

### IsIpv4AddressInPublicAddressSpace

Check if the provided Ip is in the public address space.

```csharp
IPAddress PRIVATE_IP_ADDRESS = IPAddress.Parse("192.168.1.1");
Boolean result = NetworkHelpers.IsIpv4AddressInPublicAddressSpace(PRIVATE_IP_ADDRESS);
//result is false
```
```csharp
IPAddress PUBLIC_IP_ADDRESS = IPAddress.Parse("200.87.14.111");
Boolean result = NetworkHelpers.IsIpv4AddressInPublicAddressSpace(PUBLIC_IP_ADDRESS);
//result is true
```

### GetNetworkInterfaceVendorNameByMACAddress

Get the network interface vendor name by MAC address. Null if not found.

```csharp
String result = NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress("40-8D-5C-4D-EC-A6");
//result is "GIGA-BYTE TECHNOLOGY CO.,LTD."
```

### GetKnownPort

Get information about a known port. Null if not found.

```csharp
var result = NetworkHelpers.GetKnownPort(1433, ProtocolType.Tcp);
//result is
// * ServiceName: "ms-sql-s."
// * Port: 1433
// * Protocol: ProtocolType.Tcp
// * Description: "Microsoft-SQL-Server"
```