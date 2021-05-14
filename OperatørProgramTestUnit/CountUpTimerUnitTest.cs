using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OperatoerLibrary.Timer;

namespace OperatoerProgramTestUnit
{
    [TestFixture]
    public class CountUpTimerUnitTest
    {
        private ICountUpTimer _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new CountUpTimer();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void Start(int waitSeconds)
        {
            _uut.Start();
            Thread.Sleep(waitSeconds*1010);
            Assert.AreEqual(waitSeconds, _uut.countedTime);
        }

        [Test]
        public void Stop()
        {
            _uut.Start();
            Thread.Sleep(3030);
            _uut.Stop();
            Thread.Sleep(1000);
            Assert.AreEqual(3, _uut.countedTime);
        }

    }
}
