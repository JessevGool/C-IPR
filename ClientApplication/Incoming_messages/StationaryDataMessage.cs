using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Incoming_messages
{
    class StationaryDataMessage : Message
    {
        public StationaryDataMessage(byte[] data, BikeSession bikeSession) : base(data, bikeSession)
        {
            this.ParseValues();
        }


        public override void ParseValues()
        {
            //TODO: implement the following values into the GUI
            Byte powerWattsV1 = (byte)((((byte)data[7]) << 8) | ((byte)data[8]));
            bikeSession.SetVoltage((int)powerWattsV1);


            Byte bitShiftedByte2 = (byte)((((byte)data[9]) << 8) | ((byte)data[10])); //deze moet nog worden ingesteld
            //Console.WriteLine("power in Watts V1: " + (int)powerWattsV1);
            Console.WriteLine("power in Watts V1: " + (int)bitShiftedByte2); //aanpassen!
        }
    }
}
