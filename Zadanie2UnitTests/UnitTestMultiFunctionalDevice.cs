using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zadanie2;
using System;
using System.IO;

namespace Zadanie1UnitTests
{

    public class ConsoleRedirectionToStringWriter : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleRedirectionToStringWriter()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOutput()
        {
            return stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }


    [TestClass]
    public class UnitTestMultiFunctionalDevice
    {
        // weryfikacja, czy urz�dzenie wy��cza si� poprawnie
        [TestMethod]
        public void MultiFunctionalDevice_GetState_StateOff()
        {
            var device = new MultiFunctionalDevice();
            device.PowerOff();

            Assert.AreEqual(IDevice.State.off, device.GetState());
        }

        // weryfikacja, czy urz�dzenie w��cza si� poprawnie
        [TestMethod]
        public void MultiFunctionalDevice_GetState_StateOn()
        {
            var device = new MultiFunctionalDevice();
            device.PowerOn();

            Assert.AreEqual(IDevice.State.on, device.GetState());
        }


        // weryfikacja, czy po wywo�aniu metody `Print` i w��czonym urz�dzeniu w napisie pojawia si� s�owo `Print`
        [TestMethod]
        public void MultiFunctionalDevice_Print_DeviceOn()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy po wywo�aniu metody `Print` i wy��czonym urz�dzeniu w napisie NIE pojawia si� s�owo `Print`
        [TestMethod]
        public void MultiFunctionalDevice_Print_DeviceOff()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy po wywo�aniu metody `Scan` i wy��czonym urz�dzeniu w napisie NIE pojawia si� s�owo `Scan`
        [TestMethod]
        public void MultiFunctionalDevice_Scan_DeviceOff()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy po wywo�aniu metody `Scan` i w��czonym urz�dzeniu w napisie pojawia si� s�owo `Scan`
        [TestMethod]
        public void MultiFunctionalDevice_Scan_DeviceOn()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy wywo�anie metody `Scan` z parametrem okre�laj�cym format dokumentu
        // zawiera odpowiednie rozszerzenie (`.jpg`, `.txt`, `.pdf`)
        [TestMethod]
        public void MultiFunctionalDevice_Scan_FormatTypeDocument()
        {
            var device = new MultiFunctionalDevice();
            device.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                device.Scan(out doc1, formatType: IDocument.FormatType.JPG);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".jpg"));

                device.Scan(out doc1, formatType: IDocument.FormatType.TXT);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".txt"));

                device.Scan(out doc1, formatType: IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        // weryfikacja, czy po wywo�aniu metody `ScanAndPrint` i w��czonym urz�dzeniu w napisie pojawiaj� si� s�owa `Print`
        // oraz `Scan`
        [TestMethod]
        public void MultiFunctionalDevice_ScanAndPrint_DeviceOn()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy po wywo�aniu metody `ScanAndPrint` i wy��czonym urz�dzeniu w napisie NIE pojawia si� s�owo `Print`
        // ani s�owo `Scan`
        [TestMethod]
        public void MultiFunctionalDevice_ScanAndPrint_DeviceOff()
        {
            var device = new MultiFunctionalDevice();
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
        public void MultiFunctionalDevice_PrintCounter()
        {
            var device = new MultiFunctionalDevice();
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

            // 5 wydruk�w, gdy urz�dzenie w��czone
            Assert.AreEqual(5, device.PrintCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_ScanCounter()
        {
            var device = new MultiFunctionalDevice();
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

            // 4 skany, gdy urz�dzenie w��czone
            Assert.AreEqual(4, device.ScanCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_PowerOnCounter()
        {
            var device = new MultiFunctionalDevice();
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

            // 3 w��czenia
            Assert.AreEqual(3, device.Counter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_SendFaxCounter()
        {
            var device = new MultiFunctionalDevice();
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

            // 3 faxy, gdy urz�dzenie w��czone
            Assert.AreEqual(3, device.SendFaxCounter);
        }


        // weryfikacja, czy po wywo�aniu metody `SendFax` i w��czonym urz�dzeniu w napisie pojawiaj� si� s�owa `Send FAX` z numerem FAXu
        [TestMethod]
        public void MultiFunctionalDevice_SendFax_DeviceOn()
        {
            var device = new MultiFunctionalDevice();
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

        // weryfikacja, czy po wywo�aniu metody `SendFax` i wy��czonym urz�dzeniu w napisie NIE pojawia si� s�owo `Send FAX` z numerem FAXu
        [TestMethod]
        public void MultiFunctionalDevice_SendFax_DeviceOff()
        {
            var device = new MultiFunctionalDevice();
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