using System;

namespace Zadanie2
{
    class Program
    {
        static void Main()
        {
            var xerox = new MultiFunctionalDevice();
            xerox.PowerOn();
            IDocument doc1 = new PDFDocument("test.pdf");
            xerox.Print(in doc1);

            IDocument doc2;
            xerox.Scan(out doc2);

            xerox.ScanAndPrint();
            xerox.SendFax("testfax");
            System.Console.WriteLine(xerox.Counter);
            System.Console.WriteLine(xerox.PrintCounter);
            System.Console.WriteLine(xerox.ScanCounter);
            System.Console.WriteLine(xerox.SendFaxCounter);
        }
    }
}
