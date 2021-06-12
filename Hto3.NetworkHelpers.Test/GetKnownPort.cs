using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetKnownPort
    {
        [TestMethod]
        public void PortFound()
        {
            //Prepare
            const Int32 PORT = 1433;
            const System.Net.Sockets.ProtocolType PROTOCOL = System.Net.Sockets.ProtocolType.Tcp;

            //Act
            var result = NetworkHelpers.GetKnownPort(PORT, PROTOCOL);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(PORT, result.Port);
            Assert.AreEqual(PROTOCOL, result.Protocol);
        }

        [TestMethod]
        public void PortNotFound()
        {
            //Prepare
            const Int32 PORT = 954;
            const System.Net.Sockets.ProtocolType PROTOCOL = System.Net.Sockets.ProtocolType.Udp;

            //Act
            var result = NetworkHelpers.GetKnownPort(PORT, PROTOCOL);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PortFoundButUnassigned()
        {
            //Prepare
            const Int32 PORT = 4;
            const System.Net.Sockets.ProtocolType PROTOCOL = System.Net.Sockets.ProtocolType.Udp;

            //Act
            var result = NetworkHelpers.GetKnownPort(PORT, PROTOCOL);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongProtocol()
        {
            //Prepare
            const Int32 PORT = 111;
            const System.Net.Sockets.ProtocolType PROTOCOL = System.Net.Sockets.ProtocolType.Icmp;

            //Act
            var result = NetworkHelpers.GetKnownPort(PORT, PROTOCOL);

            //Assert
            Assert.Fail();
        }
    }
}
