using System;

namespace Zadanie3
{
    class Scanner : BaseDevice, IScanner
    {
        public int ScanCounter;

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
    }
}
