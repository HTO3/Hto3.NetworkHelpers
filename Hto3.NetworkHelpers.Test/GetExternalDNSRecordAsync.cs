using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetExternalDNSRecordAsync
    {
        [TestMethod]
        public void NormalUse()
        {
            var myExternalDNS = NetworkHelpers.GetExternalDNSRecordAsync().Result;
            Assert.IsNotNull(myExternalDNS);
        }

        [TestMethod]
        public void ProvidingAnIpAddress()
        {
            var myExternalDNS = NetworkHelpers.GetExternalDNSRecordAsync(IPAddress.Parse("46.22.217.172")).Result;
            Assert.IsNotNull(myExternalDNS);
        }

        [TestMethod]
        public void ProvidingAnInternalIpAddress()
        {
            var myExternalDNS = NetworkHelpers.GetExternalDNSRecordAsync(IPAddress.Parse("127.0.0.1")).Result;
            Assert.IsNotNull(myExternalDNS);
        }
    }
}
