using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public class BikeSession
    {
        private double timeSinceStart;
        private int timeAmountOfCycles;
        private int lastKnownTime;

        private float metersTravelled;
        private int distanceAmountOfCycles;
        private int lastKnownDistance;

        private double speed;
        private long hearthBeats;

        private int voltage;

        public BikeSession()
        {
            this.timeSinceStart = 0;
            this.timeAmountOfCycles = 0;
            this.lastKnownTime = 0;

            this.metersTravelled = 0;
            this.distanceAmountOfCycles = 0;
            this.lastKnownDistance = 0;

            this.speed = 0;
            this.hearthBeats = 0;

            this.voltage = 0;
        }

        public void addTime(int time)
        {
            //is time smaller than the last recieved time?
                //yes? we have entered a new cycle
                    //timeSinceStart = (amount of cycles(1) * 255) + time        

                //no? contine adding the time.
            if (time < lastKnownTime)
            {
                timeAmountOfCycles++;
            }

            timeSinceStart = (timeAmountOfCycles * 255) + time;
            lastKnownTime = time;
        }

        public void addMetersTravelled(int metersTravelled)
        {
            if(metersTravelled < lastKnownDistance)
            {
                distanceAmountOfCycles++;
            }

            this.metersTravelled = (distanceAmountOfCycles * 255) + metersTravelled;
            lastKnownDistance = metersTravelled;
        }

        public void SetSpeed(double speed)
        {
            this.speed = speed;
        }

        public void SetHearthBeats(long hearthBeats)
        {
            this.hearthBeats = hearthBeats;
        }

        public long GetHearthBeats()
        {
            return this.hearthBeats;
        }

        public double GetTimeSinceStart()
        {
            return this.timeSinceStart * 0.25;
        }

        public float GetMetersTravelled()
        {
            return this.metersTravelled;
        }

        public double GetSpeed()
        {
            return this.speed;
        }

        public int GetVoltage()
        {
            return this.voltage;
        }

        public void SetVoltage(int voltage)
        {
            this.voltage = voltage;
        }
    }
}
