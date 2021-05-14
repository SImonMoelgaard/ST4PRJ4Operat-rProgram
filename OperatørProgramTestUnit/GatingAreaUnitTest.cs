using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using OperatoerLibrary.DTO;
using OperatoerLibrary.Filters;

namespace OperatoerProgramTestUnit
{
    [TestFixture]
    public class GatingAreaUnitTest
    {
        private IGatingArea _uut;
        private DTO_GatingValues gatingvalues;
        private DTO_GatingValues getgatingvalues;
        [SetUp]
        public void Setup()
        {
            _uut = new GatingArea();
        }

        [Test]
        public void SaveGatingValuesCorrect()
        {
            Assert.That(_uut.SaveGatingArea(1,2).Equals("Success"));
        }

        
        [TestCase(2, -2)]
        public void SaveGatingValuesLowerHigherThanHigher(double lower, double higher)
        {
            Assert.That(_uut.SaveGatingArea(lower,higher).Equals("Lower value cannot be higher than higher value"));
        }
        [TestCase(-2, -2)]
        public void SaveGatingValuesISunder0(double lower, double higher)
        {
            Assert.That(_uut.SaveGatingArea(lower,higher).Equals("Value cannot be 0"));
        }

        [TestCase(0.2,0.1)]
        [TestCase(2,1)]
        [TestCase(100,97)]
        public void GetSavedValues(double upper, double lower)
        {
            gatingvalues = new DTO_GatingValues(upper,lower);
            _uut.SaveGatingArea(lower, upper);


            getgatingvalues = new DTO_GatingValues(upper,lower);

            getgatingvalues = _uut.GetGatingValue();

            Assert.AreEqual(getgatingvalues.LowerGatingValue, gatingvalues.LowerGatingValue);
            Assert.AreEqual(getgatingvalues.UpperGatingValue, gatingvalues.UpperGatingValue);

        }




    }
}
