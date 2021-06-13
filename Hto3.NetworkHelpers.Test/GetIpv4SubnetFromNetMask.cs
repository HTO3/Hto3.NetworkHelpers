using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetIpv4SubnetFromNetMask
    {
        [DataTestMethod]
        [DataRow(8)]
        [DataRow(16)]
        [DataRow(23)]
        [DataRow(24)]
        [DataRow(32)]
        public void NormalUse(Int32 netMask)
        {
            //Act
            var result = NetworkHelpers.GetIpv4SubnetFromNetMask(netMask);

            //Assert
            switch (netMask)
            {
                case 8: Assert.AreEqual("255.0.0.0", result.ToString()); break;
                case 16: Assert.AreEqual("255.255.0.0", result.ToString()); break;
                case 23: Assert.AreEqual("255.255.254.0", result.ToString()); break;
                case 24: Assert.AreEqual("255.255.255.0", result.ToString()); break;
                case 32: Assert.AreEqual("255.255.255.255", result.ToString()); break;
                default: Assert.Fail(); break;
            }            
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void InvalidNetMaskNegative()
        {
            //Arrange
            const Int32 WRONG_NET_MASK = -1;

            //Act
            var result = NetworkHelpers.GetIpv4SubnetFromNetMask(WRONG_NET_MASK);

            //Assert
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void InvalidNetMaskHighThan32()
        {
            //Arrange
            const Int32 WRONG_NET_MASK = 33;

            //Act
            var result = NetworkHelpers.GetIpv4SubnetFromNetMask(WRONG_NET_MASK);

            //Assert
            Assert.Fail();
        }
    }
}
