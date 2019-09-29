using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetLocalIPv4AddressToReachInternet
    {
        [TestMethod]
        public void NormalUse()
        {
            var ipAddress = NetworkHelpers.GetLocalIPv4AddressToReachInternet().Result;
            Assert.IsNotNull(ipAddress);
        }
    }
}
