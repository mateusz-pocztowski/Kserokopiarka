using System;

namespace Zadanie3
{
    public class Copier : BaseDevice, IPrinter, IScanner
    {
        private readonly Printer printer;
        private readonly Scanner scanner;

        public int PrintCounter;
        public int ScanCounter;

        public Copier()
        {
            printer = new Printer();
            scanner = new Scanner();
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
    }
}
