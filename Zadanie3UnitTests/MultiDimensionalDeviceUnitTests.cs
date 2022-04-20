using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Zadanie3;

namespace Zadanie3UnitTests
{
    [TestClass]
    public class MultiDimensionalDeviceUnitTests
    {
        [TestMethod]
        public void MultidimensionalDevice_GetState_StateOff()
        {
            var device = new MultidimensionalDevice();
            device.PowerOff();

            Assert.AreEqual(IDevice.State.off, device.GetState());
        }

        [TestMethod]
        public void MultidimensionalDevice_GetState_StateOn()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            Assert.AreEqual(IDevice.State.on, device.GetState());
        }


        [TestMethod]
        public void MultidimensionalDevice_Print_DeviceOn()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument testDoc = new PDFDocument("test.pdf");
                device.Print(in testDoc);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultidimensionalDevice_Print_DeviceOff()
        {
            var device = new MultidimensionalDevice();
            device.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                device.Print(in doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultidimensionalDevice_Scan_DeviceOff()
        {
            var device = new MultidimensionalDevice();
            device.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                device.Scan(out doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultidimensionalDevice_Scan_DeviceOn()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                device.Scan(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultidimensionalDevice_ScanAndPrint_DeviceOn()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                device.ScanAndPrint();
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        [TestMethod]
        public void MultidimensionalDevice_ScanAndPrint_DeviceOff()
        {
            var device = new MultidimensionalDevice();
            device.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                device.ScanAndPrint();
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultidimensionalDevice_PrintCounter()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            device.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            device.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            device.Print(in doc3);

            device.PowerOff();
            device.Print(in doc3);
            device.Scan(out doc1);
            device.PowerOn();

            device.ScanAndPrint();
            device.ScanAndPrint();

            // 5 wydruków, gdy urządzenie włączone
            Assert.AreEqual(5, device.PrintCounter);
        }

        [TestMethod]
        public void MultidimensionalDevice_ScanCounter()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            IDocument doc1;
            device.Scan(out doc1);
            IDocument doc2;
            device.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            device.Print(in doc3);

            device.PowerOff();
            device.Print(in doc3);
            device.Scan(out doc1);
            device.PowerOn();

            device.ScanAndPrint();
            device.ScanAndPrint();

            // 4 skany, gdy urządzenie włączone
            Assert.AreEqual(4, device.ScanCounter);
        }

        [TestMethod]
        public void MultidimensionalDevice_PowerOnCounter()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();
            device.PowerOn();
            device.PowerOn();

            IDocument doc1;
            device.Scan(out doc1);
            IDocument doc2;
            device.Scan(out doc2);

            device.PowerOff();
            device.PowerOff();
            device.PowerOff();
            device.PowerOn();

            IDocument doc3 = new ImageDocument("aaa.jpg");
            device.Print(in doc3);

            device.PowerOff();
            device.Print(in doc3);
            device.Scan(out doc1);
            device.PowerOn();

            device.ScanAndPrint();
            device.ScanAndPrint();

            // 3 włączenia
            Assert.AreEqual(3, device.Counter);
        }

        [TestMethod]
        public void MultidimensionalDevice_SendFaxCounter()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            IDocument doc1;
            device.Scan(out doc1);
            IDocument doc2;
            device.Scan(out doc2);

            device.SendFax("fax1");

            IDocument doc3 = new ImageDocument("aaa.jpg");
            device.Print(in doc3);

            device.SendFax("fax2");

            device.PowerOff();

            device.SendFax("fax3");
            device.SendFax("fax4");

            device.Print(in doc3);
            device.Scan(out doc1);
            device.PowerOn();

            device.SendFax("fax5");

            device.ScanAndPrint();
            device.ScanAndPrint();

            // 3 faxy, gdy urządzenie włączone
            Assert.AreEqual(3, device.SendFaxCounter);
        }


        [TestMethod]
        public void MultidimensionalDevice_SendFax_DeviceOn()
        {
            var device = new MultidimensionalDevice();
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
        public void MultidimensionalDevice_SendFax_DeviceOff()
        {
            var device = new MultidimensionalDevice();
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