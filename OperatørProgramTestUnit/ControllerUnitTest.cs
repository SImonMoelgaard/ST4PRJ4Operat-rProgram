using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using OperatoerLibrary;
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerProgramTestUnit
{
    [TestFixture]
    public class ControllerUnitTest
    {
        private Controller _uut;
        private IBaseLineFilter fakeBaseLineFilter;
        private IAltitudeSensor fakeProducer;
        private IUDPSender fakeUdpSender;
        private BlockingCollection<BreathingValuesDataContainer> data;
        private IUDPSender udpsender;
        [SetUp]
        public void Setup()
        {
            udpsender = new UDPSender();
            fakeBaseLineFilter = Substitute.For<IBaseLineFilter>();
            
            fakeUdpSender = Substitute.For<UDPSender>();
            data = new BlockingCollection<BreathingValuesDataContainer>();
            fakeProducer = Substitute.For<AltitudeSensor>(data);
            _uut = new Controller(data);



        }

        [Test]
        public void OpenPortsTest()
        {
            _uut.OpenPorts();
           fakeUdpSender.Received().OpenSendPorts();
        }
        //[Test]
        //public void LoadDataTest()
        //{
        //    _uut.LoadData();
        //    //fakeProducer.Received().Run();
        //    Assert.Pass();
        //}

        [Test]
        public void AdjustBaseLineTest()
        {
            _uut.AdjustBaseLine();
           // fakeBaseLineFilter.Received().BaseLineAdjustBreathingValue(1);
        }

        
        //[Test]
        //public void SendMeasurementTest()
        //{
        //    fakeUdpSender.OpenSendPorts();
        //    DTO_Measurement testDTO = new DTO_Measurement(1,2,3,DateTime.Today);
        //    _uut.SendMeasurement(testDTO);
        //   // fakeUdpSender.Received().SendMeasurementData(testDTO);
        //   Assert.Pass();
        //}
    }
}
