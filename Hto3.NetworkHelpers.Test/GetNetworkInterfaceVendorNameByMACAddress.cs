using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hto3.NetworkHelpers.Test
{
    [TestClass]
    public class GetNetworkInterfaceVendorNameByMACAddress
    {
        [TestMethod]
        public void Vendor_Found()
        {
            //Prepare
            const String MAC_ADDRESS = "40-8D-5C-4D-EC-A6";
            const String EXPECTED_VENDOR = "GIGA-BYTE TECHNOLOGY CO.,LTD.";

            //Act
            var result = NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(MAC_ADDRESS);

            //Assert
            Assert.AreEqual(EXPECTED_VENDOR, result);
        }

        [TestMethod]
        public void Vendor_Not_Found()
        {
            //Prepare
            const String MAC_ADDRESS = "VV-VV-VV-4D-EC-A6";

            //Act
            var result = NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(MAC_ADDRESS);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invalid_Len_MAC_Address()
        {
            //Prepare
            const String MAC_ADDRESS = "sdfg sdfgsdfgsdfgdsaf sdf asdf ";

            //Act
            NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(MAC_ADDRESS);

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Empty_MAC_Address()
        {
            //Prepare
            const String MAC_ADDRESS = "";

            //Act
            NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(MAC_ADDRESS);

            //Assert
            Assert.Fail();
        }
    }
}
