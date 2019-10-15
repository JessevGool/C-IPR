using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avans.TI.BLE;

namespace IPRRepo
{
    class Data
    {
        public int getHeartrate(BLESubscriptionValueChangedEventArgs e)
        {
            int heartRate = -1;
            String stringData = BitConverter.ToString(e.Data);
            if (stringData.Substring(0, 2).Equals("16"))
            {
                heartRate = Convert.ToInt32(stringData.Substring(3, 2), 16);
            }

            return heartRate;
        }
    }
}
