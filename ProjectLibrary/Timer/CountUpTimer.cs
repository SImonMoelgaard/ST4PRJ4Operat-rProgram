using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.Timer
{
    public class CountUpTimer : ICountUpTimer
    {
        public int countedTime { get; private set; }
        public event EventHandler TimerTick;


        private System.Timers.Timer timer;

        public CountUpTimer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimerEvent;
            timer.Interval = 1000; 
            timer.AutoReset = true;  
        }
        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }
        private void OnTimerEvent(object sender, System.Timers.ElapsedEventArgs args)
        {
            
            countedTime++; 

            TimerTick?.Invoke(this, EventArgs.Empty);

            
        }
    }
}
