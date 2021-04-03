using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class IsIpv4AddressInRange
    {
        [TestMethod]
        public void OnRange()
        {
            //Prepare
            IPAddress IP = IPAddress.Parse("192.168.1.1");
            String CIDR_IP = "192.168.0.0/16";

            //Act
            var result = NetworkHelpers.IsIpv4AddressInRange(IP, CIDR_IP);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotOnRange()
        {
            //Prepare
            IPAddress IP = IPAddress.Parse("10.0.0.1");
            String CIDR_IP = "192.168.0.0/24";

            //Act
            var result = NetworkHelpers.IsIpv4AddressInRange(IP, CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
