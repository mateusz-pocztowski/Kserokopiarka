using System;

namespace Zadanie3
{
    class Printer : BaseDevice, IPrinter
    {
        public int PrintCounter;
        public void Print(in IDocument document)
        {
            if (GetState() != IDevice.State.on) return;

            PrintCounter++;
            DateTime now = DateTime.Now;
            Console.Write($"{now} Print: {document.GetFileName()}\n");
        }
    }
}
