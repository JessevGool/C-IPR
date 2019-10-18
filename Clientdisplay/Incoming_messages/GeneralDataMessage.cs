using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Incoming_messages
{
    public class GeneralDataMessage : Message
    {
        public GeneralDataMessage(byte[] data, BikeSession bikeSession) : base(data, bikeSession)
        {
            this.ParseValues();
        }


        public override void ParseValues()
        {
            Decimal timeSinceStart = Decimal.Parse(data[6].ToString());
            bikeSession.addTime((int)timeSinceStart);

            Decimal metersTravelled = Decimal.Parse(data[7].ToString());
            bikeSession.addMetersTravelled((int)metersTravelled);

            Byte speed = (byte)((((byte)data[8]) << 8) | ((byte)data[9]));
            bikeSession.SetSpeed(speed);
        }
    }
}
