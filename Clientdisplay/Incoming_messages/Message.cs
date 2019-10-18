using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Incoming_messages
{
    public abstract class Message
    {
        protected byte[] data;
        protected BikeSession bikeSession;

        public Message(byte[] data, BikeSession bikeSession)
        {
            this.data = data;
            this.bikeSession = bikeSession;
        }


        public abstract void ParseValues();
    }
}
