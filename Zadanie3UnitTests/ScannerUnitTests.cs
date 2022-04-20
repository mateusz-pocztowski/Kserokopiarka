using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Zadanie3;

namespace Zadanie3UnitTests
{
    [TestClass]
    public class ScannerUnitTests
    {
        [TestMethod]
        public void Scanner_GetState_StateOff()
        {
            Scanner scanner = new Scanner();
            scanner.PowerOff();
            Assert.AreEqual(IDevice.State.off, scanner.GetState());
        }

        [TestMethod]
        public void Scanner_GetState_StateOn()
        {
            Scanner scanner = new Scanner();
            scanner.PowerOn();
            Assert.AreEqual(IDevice.State.on, scanner.GetState());
        }

        [TestMethod]
        public void Scanner_Scan_DeviceOff()
        {
            var scanner = new Scanner();
            scanner.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                scanner.Scan(out doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void Scanner_Scan_DeviceOn()
        {
            var scanner = new Scanner();
            scanner.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                scanner.Scan(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void Scanner_ScanCounter()
        {
            var device = new Scanner();
            device.PowerOn();

            IDocument doc1;
            device.Scan(out doc1);
            IDocument doc2;
            device.Scan(out doc2);

            device.PowerOff();
            device.Scan(out doc1);
            device.PowerOn();

            // 2 skany, gdy urządzenie włączone
            Assert.AreEqual(2, device.ScanCounter);
        }

        [TestMethod]
        public void Scanner_PowerOnCounter()
        {
            var scanner = new Scanner();
            scanner.PowerOn();
            scanner.PowerOn();
            scanner.PowerOn();

            scanner.PowerOff();
            scanner.PowerOff();
            scanner.PowerOff();
            scanner.PowerOn();

            IDocument document3 = new ImageDocument("test.jpg");
            scanner.Scan(out document3);

            scanner.PowerOff();
            scanner.Scan(out document3);
            scanner.PowerOn();

            Assert.AreEqual(3, scanner.Counter);
        }
    }
}