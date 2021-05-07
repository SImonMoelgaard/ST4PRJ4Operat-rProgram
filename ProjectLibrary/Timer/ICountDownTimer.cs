using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.Timer
{
    public interface ICountDownTimer
    {
        double RemainingTime { get; }
        
        event EventHandler Expired;
        event EventHandler TimerTick;
        
        public void SetTime(double time);
        public void Start(double dataPoint, double lowerGating, double higherGating);
        void Stop();
    }
}
