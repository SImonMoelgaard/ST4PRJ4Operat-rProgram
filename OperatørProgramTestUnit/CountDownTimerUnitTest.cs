using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using OperatoerLibrary;
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;
using OperatoerLibrary.Timer;

namespace OperatoerProgramTestUnit
{
    [TestFixture]
    public class CountDownTimerUnitTest
    {
        private ICountDownTimer _uut;
        

        [SetUp]
        public void Setup()
        {
            _uut = new CountDownTimer();
        }

        [TestCase(5)]
        [TestCase(500)]
        [TestCase(1000)]
        public void SetTime(int seconds)
        {
            _uut.SetTime(seconds);
            Assert.AreEqual(seconds, _uut.RemainingTime);
        }

        [TestCase(5,1,10)]
        public void Start(double dataPoint, double lowerGating, double higherGating)
        {
            _uut.SetTime(500);

            for (int i = 0; i < 15; i++)
            {
                _uut.Start(dataPoint, lowerGating, higherGating);
                Thread.Sleep(40);
            }
            _uut.Start(15, lowerGating, higherGating);

            
            Assert.That(_uut.RemainingTime, Is.EqualTo(499.85).Within(0.1)); // pass
            
        }

        



    }
}
