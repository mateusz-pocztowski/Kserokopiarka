using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Zadanie3;

namespace Zadanie3UnitTests
{
    [TestClass]
    public class PrinterUnitTests
    {
        [TestMethod]
        public void Printer_GetState_StateOff()
        {
            Printer printer = new Printer();
            printer.PowerOff();
            Assert.AreEqual(IDevice.State.off, printer.GetState());
        }

        [TestMethod]
        public void Printer_GetState_StateOn()
        {
            Printer printer = new Printer();
            printer.PowerOn();
            Assert.AreEqual(IDevice.State.on, printer.GetState());
        }

        [TestMethod]
        public void Printer_Print_DeviceOn()
        {
            Printer device = new Printer();
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
        public void MultiFunctionalDevice_Print_DeviceOff()
        {
            Printer device = new Printer();
            device.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument testDoc = new PDFDocument("test.pdf");
                device.Print(in testDoc);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void Printer_PrintCounter()
        {
            Printer printer = new Printer();
            printer.PowerOn();

            IDocument doc1 = new PDFDocument("test1.pdf");
            printer.Print(in doc1);
            IDocument doc2 = new TextDocument("test2.txt");
            printer.Print(in doc2);
            IDocument doc3 = new ImageDocument("test3.jpg");
            printer.Print(in doc3);

            printer.PowerOff();
            printer.Print(in doc3);
            printer.PowerOn();

            printer.Print(in doc3);
            printer.Print(in doc3);

            Assert.AreEqual(5, printer.PrintCounter);
        }

        [TestMethod]
        public void Printer_PowerOnCounter()
        {
            var printer = new Printer();
            printer.PowerOn();
            printer.PowerOn();
            printer.PowerOn();

            printer.PowerOff();
            printer.PowerOff();
            printer.PowerOff();
            printer.PowerOn();

            IDocument document3 = new ImageDocument("test.jpg");
            printer.Print(in document3);

            printer.PowerOff();
            printer.Print(in document3);
            printer.PowerOn();

            Assert.AreEqual(3, printer.Counter);
        }
    }
}