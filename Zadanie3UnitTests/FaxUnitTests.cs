using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Zadanie3;

namespace Zadanie3UnitTests
{
    [TestClass]
    public class FaxUnitTests
    {
        [TestMethod]
        public void Fax_GetState_StateOff()
        {
            Fax fax = new Fax();
            fax.PowerOff();
            Assert.AreEqual(IDevice.State.off, fax.GetState());
        }

        [TestMethod]
        public void Fax_GetState_StateOn()
        {
            Fax fax = new Fax();
            fax.PowerOn();
            Assert.AreEqual(IDevice.State.on, fax.GetState());
        }

        [TestMethod]
        public void Fax_PowerOnCounter()
        {
            var fax = new Fax();
            fax.PowerOn();
            fax.PowerOn();
            fax.PowerOn();

            fax.PowerOff();
            fax.PowerOff();
            fax.PowerOff();
            fax.PowerOn();

            fax.SendFax("test");

            fax.PowerOff();
            fax.SendFax("test");
            fax.PowerOn();

            Assert.AreEqual(3, fax.Counter);
        }

        [TestMethod]
        public void Fax_SendFaxCounter()
        {
            var device = new Fax();
            device.PowerOn();

            device.SendFax("fax1");
            device.SendFax("fax2");

            device.PowerOff();

            device.SendFax("fax3");
            device.SendFax("fax4");

            device.PowerOn();

            device.SendFax("fax5");

            // 3 faxy, gdy urządzenie włączone
            Assert.AreEqual(3, device.SendFaxCounter);
        }


        [TestMethod]
        public void Fax_SendFax_DeviceOn()
        {
            var device = new Fax();
            device.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                string faxNumber = "test";
                device.SendFax(faxNumber);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Send FAX"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(faxNumber));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void Fax_SendFax_DeviceOff()
        {
            var device = new Fax();
            device.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                string faxNumber = "test";
                device.SendFax(faxNumber);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Send FAX"));
                Assert.IsFalse(consoleOutput.GetOutput().Contains(faxNumber));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }
    }
}