using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class ValidateCIDRIp
    {
        [TestMethod]
        public void Ok()
        {
            //Prepare
            const String VALID_CIDR_IP = "10.0.0.0/24";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(VALID_CIDR_IP);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Fail1()
        {
            //Prepare
            const String WRONG_CIDR_IP = "24";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail2()
        {
            //Prepare
            const String WRONG_CIDR_IP = "    ";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail3()
        {
            //Prepare
            const String WRONG_CIDR_IP = null;

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail4()
        {
            //Prepare
            const String WRONG_CIDR_IP = "192.168.1.1";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail5()
        {
            //Prepare
            const String WRONG_CIDR_IP = "192.168.1.1/";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail6()
        {
            //Prepare
            const String WRONG_CIDR_IP = "/";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Fail7()
        {
            //Prepare
            const String WRONG_CIDR_IP = "192.168.1.1/64";

            //Act
            var result = NetworkHelpers.ValidateCIDRIP(WRONG_CIDR_IP);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
