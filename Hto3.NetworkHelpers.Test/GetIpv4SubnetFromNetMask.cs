using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetIpv4SubnetFromNetMask
    {
        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            const String EXPECTED_IP = "255.255.254.0";

            //Act
            var result = NetworkHelpers.GetIpv4SubnetFromNetMask(23);

            //Assert
            Assert.AreEqual(EXPECTED_IP, result.ToString());
        }
    }
}
