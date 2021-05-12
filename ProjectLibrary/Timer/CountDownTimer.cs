using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.Timer
{
    public class CountDownTimer : ICountDownTimer
    {
        public double RemainingTime { get; private set; }
       
        private int filter = 0;
        public event EventHandler Expired;
        public event EventHandler TimerTick;
       

        private System.Timers.Timer timer;
       

        public CountDownTimer()
        {
            


            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimerEvent;
            timer.Interval = 25; 
            timer.AutoReset = true;  
        }

        
        public void SetTime(double time)
        {
            RemainingTime = time;
        }

        public void Start(double dataPoint, double lowerGating, double higherGating)
        {
            if (dataPoint>=lowerGating && dataPoint <= higherGating)
            {
                filter++;
                
            }
            else
            {
                filter = 0;
                
            }

            if (filter >=12)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
            
        }

        private void Expire()
        {
            timer.Enabled = false;
            Expired?.Invoke(this, System.EventArgs.Empty);
        }

        private void OnTimerEvent(object sender, System.Timers.ElapsedEventArgs args)
        {
            
            RemainingTime -= 0.025; 

            TimerTick?.Invoke(this, EventArgs.Empty);

            if (RemainingTime <= 0)
            {
                Expire();
            }
        }
        
    }
}
