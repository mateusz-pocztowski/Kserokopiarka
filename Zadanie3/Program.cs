using System;

namespace Zadanie3
{
    class Program
    {
        static void Main()
        {
            var device = new MultidimensionalDevice();
            device.PowerOn();

            IDocument testDoc = new PDFDocument("test");
            device.Print(in testDoc);
            device.Scan(out testDoc);
            device.Scan(out testDoc, formatType: IDocument.FormatType.PDF);
            device.ScanAndPrint();
            device.SendFax("test");
            IDocument doc1;
            device.Scan(out doc1, formatType: IDocument.FormatType.PDF);

            Console.WriteLine(device.PrintCounter);
        }
    }
}
