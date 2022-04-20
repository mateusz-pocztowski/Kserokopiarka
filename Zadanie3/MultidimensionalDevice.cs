using System;

namespace Zadanie3
{
    public class MultidimensionalDevice : BaseDevice, IPrinter, IScanner, IFax
    {
        private readonly Printer printer;
        private readonly Scanner scanner;
        private readonly Fax fax;

        public int PrintCounter => printer.PrintCounter;
        public int ScanCounter => scanner.ScanCounter;
        public int SendFaxCounter => fax.SendFaxCounter;

        public MultidimensionalDevice()
        {
            printer = new Printer();
            scanner = new Scanner();
            fax = new Fax();
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            scanner.Scan(out document);
        }

        public void Print(in IDocument document)
        {
            printer.Print(document);
        }

        public void ScanAndPrint()
        {
            if (GetState() != IDevice.State.on) return;

            IDocument document;
            Scan(out document);
            Print(document);
        }

        public void SendFax(string fax)
        {
            this.fax.SendFax(fax);
        }


        public override void PowerOff()
        {
            if (GetState() == IDevice.State.off) return;

            state = IDevice.State.off;
            printer.state = IDevice.State.off;
            scanner.state = IDevice.State.off;
            fax.state = IDevice.State.off;
            Console.WriteLine("... Device is off !");
        }

        public override void PowerOn()
        {
            if (GetState() == IDevice.State.on) return;

            state = IDevice.State.on;
            printer.state = IDevice.State.on;
            scanner.state = IDevice.State.on;
            fax.state = IDevice.State.on;
            Console.WriteLine("Device is on ...");
            Counter++;
        }
    }
}
