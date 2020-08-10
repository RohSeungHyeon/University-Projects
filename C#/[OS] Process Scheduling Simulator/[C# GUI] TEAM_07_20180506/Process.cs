using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM_07
{
    class Process
    {
        private int ArrivalTime = 0;
        private int BurstTime = 0;
        private int RemainingBurstTime = 0;
        private int WaitingTime = 0;
        private int TurnaroundTime = 0;
        private int TimeQuantum = 0;
        private double NormalizedTT = 0;
        private double ResponseRatio = 0;
        public int i = 0;

        public void setArrivalTime(int _ArrivalTime)
        {
            ArrivalTime = _ArrivalTime;
        }

        public void setBurstTime(int _BurstTime)
        {
            BurstTime = _BurstTime;
            RemainingBurstTime = BurstTime;
        }

        public void setRemainingBurstTime()
        {
            RemainingBurstTime--;
        }

        public void setWaitingTime()
        {
            WaitingTime++;
        }

        public void setTurnaroundTime()
        {
            TurnaroundTime = WaitingTime + BurstTime;
        }

        public void setTimeQuantum(int _TimeQuantum)
        {
            TimeQuantum = _TimeQuantum;
        }

        public void passTimeQuantum()
        {
            TimeQuantum--;
        }

        public void setNormalizedTT()
        {
            NormalizedTT = (double)TurnaroundTime / (double)BurstTime;
        }

        public void setResponseRatio()
        {
            ResponseRatio = (double)(WaitingTime + BurstTime) / (double)(BurstTime);
        }

        public int getArrivalTime()
        {
            return ArrivalTime;
        }

        public int getBurstTime()
        {
            return BurstTime;
        }

        public int getWaitingTime()
        {
            return WaitingTime;
        }

        public int getTurnaroundTime()
        {
            return TurnaroundTime;
        }

        public int getTimeQuantum()
        {
            return TimeQuantum;
        }

        public double getNormilizedTT()
        {
            return NormalizedTT;
        }

        public int getRemainingBurstTime()
        {
            return RemainingBurstTime;
        }

        public double getResponseRatio()
        {
            return ResponseRatio;
        }
    }
}
