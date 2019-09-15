using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetIPAddresses
    {
        [TestMethod]
        public void NormalUse()
        {
            var ipAddresses = NetworkHelpers.GetIPAddresses();
            Assert.IsNotNull(ipAddresses);
            Assert.IsTrue(ipAddresses.Any());
        }
    }
}
