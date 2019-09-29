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


            var result = NetworkHelpers.GetIpv4SubnetFromNetMask(23);
            Assert.IsNotNull(result);
        }
    }
}
