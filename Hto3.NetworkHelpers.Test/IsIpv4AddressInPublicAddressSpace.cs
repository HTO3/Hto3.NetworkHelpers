﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class IsIpv4AddressInPublicAddressSpace
    {
        [TestMethod]
        public void OnPrivateSpace()
        {
            //Prepare
            IPAddress PRIVATE_IP_ADDRESS = IPAddress.Parse("192.168.1.1");

            //Act
            var result = NetworkHelpers.IsIpv4AddressInPublicAddressSpace(PRIVATE_IP_ADDRESS);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OnPublicSpace()
        {
            //Prepare
            IPAddress PUBLIC_IP_ADDRESS = IPAddress.Parse("200.87.14.111");

            //Act
            var result = NetworkHelpers.IsIpv4AddressInPublicAddressSpace(PUBLIC_IP_ADDRESS);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
