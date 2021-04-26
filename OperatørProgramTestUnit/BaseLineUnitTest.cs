using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using OperatoerLibrary.Filters;

namespace OperatoerProgramTestUnit
{
    [TestFixture]
    public class BaseLineUnitTest
    {



        private IBaseLineFilter _uut;
        private Random random;
        [SetUp]
        public void Setup()
        {
            _uut = new BaselineFilter();
            
            random = new Random();

            int datapoint = 5;
            for (int i = 1; i < 5; i++)
            {

                _uut.AddToBaseLineList(datapoint);
                datapoint--;

            }
        }

        [Test]
        public void AdjustBaseLineVlaueTest()
        {
           

            double data = _uut.AdjustBaseLineValue();
            Assert.AreEqual(2, data);

        }

        [Test]
        public void BaseLineAdjustBreathingValueTest()
        {
            _uut.AdjustBaseLineValue();
            var adjustedvalue = _uut.BaseLineAdjustBreathingValue(5);
            Assert.AreEqual(3, adjustedvalue);
        }

        [Test]
        public void AddToBaseLineListRemoveAt250()
        {
            for (int i = 0; i < 255; i++)
            {
                _uut.AddToBaseLineList(random.Next(1, 5));
            }

            var data = _uut.BaseLineAdjustList.Count;
            Assert.AreEqual(250, data);
        }

    }
}
