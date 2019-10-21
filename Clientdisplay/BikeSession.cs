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

        private float MetersTravelled;
        private int DistanceAmountOfCycles;
        private int LastKnownDistance;

        private double Speed;
        private long HearthBeats;

        private int Voltage;

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

        public void SetSpeed(double speed)
        {
            this.Speed = speed;
        }

        public void SetHearthBeats(long hearthBeats)
        {
            this.HearthBeats = hearthBeats;
        }

        public long GetHearthBeats()
        {
            return this.HearthBeats;
        }

        public double GetTimeSinceStart()
        {
            return this.TimeSinceStart * 0.25;
        }

        public float GetMetersTravelled()
        {
            return this.MetersTravelled;
        }

        public double GetSpeed()
        {
            return this.Speed;
        }

        public int GetVoltage()
        {
            return this.Voltage;
        }

        public void SetVoltage(int voltage)
        {
            this.Voltage = voltage;
        }
    }
}
