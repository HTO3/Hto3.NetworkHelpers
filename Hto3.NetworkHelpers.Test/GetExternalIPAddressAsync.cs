using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetExternalIPAddressAsync
    {
        [TestMethod]
        public void NormalUse()
        {
            var myExternalIP = NetworkHelpers.GetExternalIPAddressAsync().Result;
            Assert.IsNotNull(myExternalIP);
        }
    }
}
