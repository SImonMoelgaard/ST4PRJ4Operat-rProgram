using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.ProducerConsumer
{
    public interface IProducer
    {
        public void Run();

        public void GetOneBreathingValue();
    }
}
