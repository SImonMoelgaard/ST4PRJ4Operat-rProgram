using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.Timer
{
    public interface ICountUpTimer
    {
        int countedTime { get; }
        
        event EventHandler TimerTick;
        
        void Start();
        void Stop();
    }
}
