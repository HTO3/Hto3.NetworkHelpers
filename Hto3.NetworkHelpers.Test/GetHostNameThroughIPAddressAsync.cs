using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetHostNameThroughIPAddressAsync
    {
        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            const String EXPECTED_HOST_NAME = "api1.whatismyipaddress.com";

            //Act
            var hostname = NetworkHelpers.GetHostNameThroughIPAddressAsync("66.171.248.178").Result;

            //Assert
            Assert.AreEqual(EXPECTED_HOST_NAME, hostname);
        }
    }
}
