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
            var ipAddresses = NetworkHelpers.GetLocalIPAddresses();

            Assert.IsNotNull(ipAddresses);
            Assert.IsTrue(ipAddresses.Any());
        }
    }
}
