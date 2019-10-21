using Clientdisplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientdisplay.Incoming_messages
{
    class HearthDataMessage : Message
    {
        public HearthDataMessage(byte[] data, BikeSession bikeSession) : base(data, bikeSession)
        {
            this.ParseValues();
        }

        public override void ParseValues()
        {
            String stringData = BitConverter.ToString(data);
            if (stringData.Substring(0, 2).Equals("16"))
            {
                this.bikeSession.SetHearthBeats(Convert.ToInt32(stringData.Substring(3, 2), 16));
            }
        }
    }
}
