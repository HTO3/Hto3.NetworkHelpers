using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetExternalIPv4AddressAsync
    {
        [TestMethod]
        public void NormalUse()
        {
            var myExternalIP = NetworkHelpers.GetExternalIPv4AddressAsync().Result;
            Assert.IsNotNull(myExternalIP);
        }
    }
}
