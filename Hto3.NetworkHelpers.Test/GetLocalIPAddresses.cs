using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetLocalIPAddresses
    {
        [TestMethod]
        public void NormalUse()
        {
            var ipAddresses = NetworkHelpers.GetLocalIPv4Addresses();

            Assert.IsNotNull(ipAddresses);
            Assert.IsTrue(ipAddresses.Any());
        }
    }
}
