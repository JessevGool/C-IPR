using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Models
{
        public class BikeValue
        {
            public double TimeSinceStart { get; set; }
            public float MetersTravelled { get; set; }
            public double Speed { get; set; }
            public long HearthBeats { get; set; }
            public int Voltage { get; set; }

            public BikeValue() : this(0, 0, 0, 0, 0)
            {

            }

            public BikeValue(double timeSinceStart, float metersTravelled, double speed, long hearthBeats, int voltage)
            {
                this.TimeSinceStart = timeSinceStart;
                this.MetersTravelled = metersTravelled;
                this.Speed = speed;
                this.HearthBeats = hearthBeats;
                this.Voltage = voltage;
            }
        }
}
