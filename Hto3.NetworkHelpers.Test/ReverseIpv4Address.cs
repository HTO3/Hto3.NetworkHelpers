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
    public class ReverseIpv4Address
    {
        [TestMethod]
        public void NormalUse()
        {
            //Arrange
            var expected = IPAddress.Parse("1.0.168.192");
            var input = IPAddress.Parse("192.168.0.1");

            //Act
            var actual = NetworkHelpers.ReverseIpv4Address(input);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
