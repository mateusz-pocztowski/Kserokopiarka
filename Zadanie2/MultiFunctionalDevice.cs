using System;

namespace Zadanie2
{
    public class MultiFunctionalDevice : BaseDevice, IPrinter, IScanner, IFax
    {
        public int PrintCounter;
        public int ScanCounter;
        public int SendFaxCounter;

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = new TextDocument("");

            if (GetState() != IDevice.State.on) return;

            ScanCounter++;
            switch (formatType)
            {
                case IDocument.FormatType.PDF:
                    document = new PDFDocument($"PDFScan{ScanCounter}.pdf");
                    break;
                case IDocument.FormatType.TXT:
                    document = new TextDocument($"TextScan{ScanCounter}.txt");
                    break;
                case IDocument.FormatType.JPG:
                    document = new ImageDocument($"ImageScan{ScanCounter}.jpg");
                    break;
            }
            DateTime now = DateTime.Now;
            Console.Write($"{now} Scan: {document.GetFileName()}\n");
        }

        public void Print(in IDocument document)
        {
            if (GetState() != IDevice.State.on) return;

            PrintCounter++;
            DateTime now = DateTime.Now;
            Console.Write($"{now} Print: {document.GetFileName()}\n");
        }

        public void ScanAndPrint()
        {
            if (GetState() != IDevice.State.on) return;

            IDocument dokument;
            Scan(out dokument);
            Print(dokument);
        }

        public void SendFax(string fax)
        {
            if (GetState() != IDevice.State.on) return;

            SendFaxCounter++;
            DateTime now = DateTime.Now;
            Console.Write($"{now} Send FAX: {fax}\n");
        }
    }
}
