using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using OperatoerLibrary;
using OperatoerLibrary.DTO;
using OperatoerLibrary.ProducerConsumer;



namespace OperatoerGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Chart Axis
        /// </summary>
        private double axisMax;
        private double axisMin;
        //private double yAxisMax;
        //private double yAxisMin;

        public Func<double, string> DateTimeFormatter { get; set; }

        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        public ChartValues<MeasurementModel> ChartValues { get; set; }
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData = new BlockingCollection<BreathingValuesDataContainer>();
        public bool IsReading { get; set; }

       

         //BlockingCollection<BreathingValuesDataContainer> _datacollection = new BlockingCollection<BreathingValuesDataContainer>();
         
         
        
         public event PropertyChangedEventHandler PropertyChanged;
         
         

         private List<DTO_Measurement> data;
         private Controller cr;
      
        
        public MainWindow()
        {
            InitializeComponent();
        //IProducer producer = new Producer(_datacollection);

        //     producer = new Producer(_datacollection);
        //    producer.GetOneBreathingValue();

        //    BreathingValuesDataContainer container = _datacollection.Take();
        
            cr = new Controller(_breathingData);
            
        var mapper = Mappers.Xy<MeasurementModel>()
            .X(model => model.Time.Ticks)
            .Y(model => model.RawData);

        Charting.For<MeasurementModel>(mapper);


           
        ChartValues = new ChartValues<MeasurementModel>();
           

        DateTimeFormatter = value => new DateTime((long) value).ToString("hh:mm:ss:ms");


        AxisStep = TimeSpan.FromSeconds(1).Ticks;

        AxisUnit = TimeSpan.TicksPerSecond;

        SetAxisLimits(DateTime.Now);

        IsReading = false;
        

        DataContext = this;


     
        }

        
        
        public double AxisMax
        {
            get { return axisMax; }
            set
            {
                axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }

        /// <summary>
        /// Y Axis Minimum
        /// </summary>
        public double AxisMin
        {
            get { return axisMin; }
            set
            {
                axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }



        private List<double> value;
       
        private void Read()
        {
           cr.loaddata();

           
            while (IsReading)
            {


                try
                {
                    BreathingValuesDataContainer data  = _breathingData.Take();
                    value = data.BreathingSample;
                    
                    
                    //Metode der kaldes for at få data fra køen
                
                    
                    foreach (var VARIABLE in value)
                    {
                        ChartValues.Add(new MeasurementModel
                        {
                            //time = ting i dto
                            //Breathdata = ting i DTO

                            Time = DateTime.Now,
                            RawData = VARIABLE




                        });

                        SetAxisLimits(DateTime.Now);


                        if (ChartValues.Count > 50000)
                        {
                            ChartValues.RemoveAt(0);
                        }


                        this.Dispatcher.Invoke(() =>
                        {
                            ID_TB.Text = Convert.ToString(DateTime.Now);

                        });
                        Thread.Sleep(5);
                    }

                   

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                data = new List<DTO_Measurement>();
                //data = cr.getdata();
                
               
                    //foreach (var VARIABLE in data)
                    //{
                    //    ChartValues.Add(new MeasurementModel
                    //    {
                    //        //time = ting i dto
                    //        //Breathdata = ting i DTO

                    //        Time = VARIABLE.Time,
                    //        RawData = VARIABLE.MeasurementData




                    //    });


                    //    SetAxisLimits(VARIABLE.Time);

                    //    if (ChartValues.Count > 500)
                    //    {
                    //        ChartValues.RemoveAt(0);
                    //    }


                    //    this.Dispatcher.Invoke(() =>
                    //    {
                    //        ID_TB.Text = Convert.ToString(VARIABLE.Time);

                    //    });
                    //    Thread.Sleep(33);

                 
                

               

            }
        }


        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(0).Ticks; // lets force the axis to be 0 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(10).Ticks; // and 4 seconds behind

        }



        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

       

       

      

        private void testknap_Click(object sender, RoutedEventArgs e)
        {
            

                //var top = Canvas.GetTop(DENHER);
                //if (top<50)
                //{
                //    Canvas.SetTop(DENHER, Canvas.GetTop(DENHER)+5);
                //}
                //else
                //{
                //    Canvas.SetTop(DENHER, Canvas.GetTop(DENHER)-5);
                //}

        }

        private void ScaleUp_b_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ScaleDown_b_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Start_b_Click(object sender, RoutedEventArgs e)
        {
            IsReading = !IsReading;
            if (IsReading) Task.Factory.StartNew(Read);
        }

        private void MeasurementChart_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void MeasurementChart_UpdaterTick(object sender)
        {

        }

        private void MeasurementChart_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}