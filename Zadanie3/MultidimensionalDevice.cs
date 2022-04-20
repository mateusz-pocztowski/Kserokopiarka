using System;

namespace Zadanie3
{
    public class MultidimensionalDevice : BaseDevice, IPrinter, IScanner, IFax
    {
        private readonly Printer printer;
        private readonly Scanner scanner;
        private readonly Fax fax;

        public int PrintCounter;
        public int ScanCounter;
        public int SendFaxCounter;

        public MultidimensionalDevice()
        {
            printer = new Printer();
            scanner = new Scanner();
            fax = new Fax();
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            scanner.Scan(out document);
            ScanCounter = scanner.ScanCounter;
        }

        public void Print(in IDocument document)
        {
            printer.Print(document);
            PrintCounter = printer.PrintCounter;
        }

        public void ScanAndPrint()
        {
            if (GetState() != IDevice.State.on) return;

            IDocument document;
            Scan(out document);
            Print(document);
            ScanCounter = scanner.ScanCounter;
            PrintCounter = printer.PrintCounter;
        }

        public void SendFax(string fax)
        {
            this.fax.SendFax(fax);
            SendFaxCounter = this.fax.SendFaxCounter;
        }
    }
}
