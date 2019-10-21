using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientdisplay
{
    public class BikeSession
    {
        private double TimeSinceStart;
        private int TimeAmountOfCycles;
        private int LastKnownTime;
        private int DistanceAmountOfCycles;
        private int LastKnownDistance;

        public float MetersTravelled { get; set; }
        public double Speed { get; set; }
        public long HearthBeats { get; set; }

        public int Voltage { get; set; }

        public BikeSession()
        {
            this.TimeSinceStart = 0;
            this.TimeAmountOfCycles = 0;
            this.LastKnownTime = 0;

            this.MetersTravelled = 0;
            this.DistanceAmountOfCycles = 0;
            this.LastKnownDistance = 0;

            this.Speed = 0;
            this.HearthBeats = 0;

            this.Voltage = 0;
        }

        public void addTime(int time)
        {
            //is time smaller than the last recieved time?
            //yes? we have entered a new cycle
            //timeSinceStart = (amount of cycles(1) * 255) + time        

            //no? contine adding the time.
            if (time < LastKnownTime)
            {
                TimeAmountOfCycles++;
            }

            TimeSinceStart = (TimeAmountOfCycles * 255) + time;
            LastKnownTime = time;
        }

        public void addMetersTravelled(int metersTravelled)
        {
            if (metersTravelled < LastKnownDistance)
            {
                DistanceAmountOfCycles++;
            }

            this.MetersTravelled = (DistanceAmountOfCycles * 255) + metersTravelled;
            LastKnownDistance = metersTravelled;
        }

        public double GetTimeSinceStart()
        {
            return this.TimeSinceStart * 0.25;
        }
    }
}
