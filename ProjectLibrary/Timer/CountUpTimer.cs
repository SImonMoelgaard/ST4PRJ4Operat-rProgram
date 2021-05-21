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
        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            timer.Enabled = true;
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Enabled = false;
        }
        /// <summary>
        /// Timer event. Counts up in increments of 1 second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimerEvent(object sender, System.Timers.ElapsedEventArgs args)
        {
            
            countedTime++; 

            TimerTick?.Invoke(this, EventArgs.Empty);

            
        }
    }
}
