using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetExternalDNSRecordAsync
    {
        [TestMethod]
        public void NormalUse()
        {
            var myExternalDNS = NetworkHelpers.GetExternalDNSRecordAsync().Result;
            Assert.IsNotNull(myExternalDNS);
        }
    }
}
