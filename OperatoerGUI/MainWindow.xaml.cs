using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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

        private DTO_Measurement DTO_Send;

        private double UpperGatingValue = -18, LowerGatingValue = -18.3;
        private double UpperGatingValueAdjusted = 0, LowerGatingValueAdjusted = 0;
        private double baseLine = 0;

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
        
            cr.OpenPorts();


            


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
        // Indlæser bare filen
           
            while (IsReading)
            {
                




                try
                {
                    BreathingValuesDataContainer data  = _breathingData.Take();
                    value = data.BreathingSample;
                    
                    
                    //Metode der kaldes for at få data fra køen
                
                    
                    foreach (var VARIABLE in value)
                    {

                        double dataPoint = cr.AdjustBaseLine(VARIABLE);

                        DTO_Send = new DTO_Measurement(dataPoint, LowerGatingValueAdjusted, UpperGatingValueAdjusted, DateTime.Now);
                        //Husk at ændre til rigtige gating værdier
                        cr.SendMeasurement(DTO_Send);



                        ChartValues.Add(new MeasurementModel
                        {
                            //time = ting i dto
                            //Breathdata = ting i DTO

                            Time = DateTime.Now,
                            RawData = dataPoint




                        });

                        SetAxisLimits(DateTime.Now);


                        if (ChartValues.Count > 500)
                        {
                            ChartValues.RemoveAt(0);
                        }


                        this.Dispatcher.Invoke(() =>
                        {
                            Clock_TB.Text = Convert.ToString(DateTime.Now);
                            Showdata_TB.Text = Convert.ToString(dataPoint);


                        });
                        Thread.Sleep(40);
                    }

                   

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                



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

        private void Adjust_b_Click(object sender, RoutedEventArgs e)
        {
           baseLine = cr.AdjustBaseLine();
           AdjustGatingValues();

        }

        private void Connect_B_Click(object sender, RoutedEventArgs e)
        {
            var selected = PatientGUI_CB.Text;
            int guiType = 1;
            if (selected == "Standard") { guiType = 1; }
            else if (selected == "Jul") { guiType = 2;}
            



            cr.SendGUIInfo(guiType);
        }

        private void Stop_b_Click(object sender, RoutedEventArgs e)
        {
            IsReading = false;
            if (!IsReading) Task.Factory.StartNew(Read);
        }

        private void AdjustGatingValues()
        {
            UpperGatingValueAdjusted = UpperGatingValue - baseLine;
            LowerGatingValueAdjusted = LowerGatingValue - baseLine;
        }


        private void GatingValueConfirm_b_Click(object sender, RoutedEventArgs e)
        {

            if (Gatingvalueupper_TB.Text != null && GatingValueLower_TB.Text != null)
            {
                UpperGatingValue = Convert.ToDouble(Gatingvalueupper_TB.Text);
                LowerGatingValue = Convert.ToDouble(GatingValueLower_TB.Text);
                AdjustGatingValues();

            }







        }
    }
}