using System;

namespace Zadanie3
{
    public class Fax : BaseDevice, IFax
    {
        public int SendFaxCounter;

        public void SendFax(string fax)
        {
            if (GetState() != IDevice.State.on) return;

            SendFaxCounter++;
            DateTime now = DateTime.Now;
            Console.Write($"{now} Send FAX: {fax}\n");
        }
    }
}
