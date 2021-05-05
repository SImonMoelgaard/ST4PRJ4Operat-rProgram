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
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OperatoerLibrary;
using OperatoerLibrary.DTO;
using OperatoerLibrary.ProducerConsumer;



namespace OperatoerGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        /// <summary>
        /// Chart Axis
        /// </summary>
        private double axisMax;
        private double axisMin;
        //private double yAxisMax;
        //private double yAxisMin;

        //public double axisMax { get; set; }
        //public double axisMin { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }

        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        public ChartValues<MeasurementModel> ChartValues { get; set; }
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData = new BlockingCollection<BreathingValuesDataContainer>();
        public bool IsReading { get; set; }

        private DTO_Measurement DTO_Send;

        private double UpperGatingValue = -18, LowerGatingValue = -18.3;
        private double UpperGatingValueAdjusted = -18, LowerGatingValueAdjusted = -18.3;
        private double baseLine = 0;
        private DTO_GatingValues gatingValues;

         BlockingCollection<BreathingValuesDataContainer> _datacollection = new BlockingCollection<BreathingValuesDataContainer>();
         
         
        
         public event PropertyChangedEventHandler PropertyChangedtest;
         
         
         
        

         private List<DTO_Measurement> data;
         private Controller cr;
         private double _trend;
         public SeriesCollection LastHourSeries { get; set; }
         private double _lastLecture;

         
         public MainWindow ()
         {
             InitializeComponent();
             
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



        private double value;

        private void Read()
        {
            cr.RunProducer();
            while (IsReading)
            {
                
                    try
                    {
                        BreathingValuesDataContainer data = _breathingData.Take();
                        
                        Console.ReadLine();
                        value = data.BreathingSample;
                   

                        //Metode der kaldes for at få data fra køen
                        
                            double dataPoint = cr.AdjustBaseLine(value);

                            DTO_Send = new DTO_Measurement(dataPoint, LowerGatingValueAdjusted, UpperGatingValueAdjusted, DateTime.Now);
                            //Husk at ændre til rigtige gating værdier
                            cr.SendMeasurement(DTO_Send);



                            ChartValues.Add(new MeasurementModel
                            {
                                

                                Time = DateTime.Now,
                                RawData = dataPoint




                            });

                            SetAxisLimits(DateTime.Now);


                            if (ChartValues.Count > 150)
                            {
                                ChartValues.RemoveAt(0);
                            }


                            this.Dispatcher.Invoke(() =>
                            {
                            


                            });
                        
                        

                   


                    }
                    catch (InvalidOperationException)
                    {
                    
                    }
                
                

            }
        }


        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(0).Ticks; // lets force the axis to be 0 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(6).Ticks; // and 4 seconds behind

        }



        







        
       

        
       

       
        /// <summary>
        /// Takes the string from the combobox and returns the correspondent number to the controller class.
        /// </summary>
        private void AdjustGatingValues()
        {
            
            UpperGatingValueAdjusted = UpperGatingValue - baseLine;
            LowerGatingValueAdjusted = LowerGatingValue - baseLine;

        }


        /// <summary>
        /// Takes the two inputted numbers and adjusts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void Start_B_Click_1(object sender, RoutedEventArgs e)
        {
            IsReading = true;
            if (IsReading) Task.Factory.StartNew(Read);

            Stop_B.Visibility = Visibility.Visible;
            Start_B.Visibility = Visibility.Hidden;
            Start_B_gray.Visibility = Visibility.Visible;
            Stop_B_gray.Visibility = Visibility.Hidden;
            
        }

        private void Stop_B_Click_1(object sender, RoutedEventArgs e)
        {
            IsReading = false;
            
            Start_B.Visibility = Visibility.Visible;
            Stop_B.Visibility = Visibility.Hidden;
            Stop_B_gray.Visibility = Visibility.Visible;
            Start_B_gray.Visibility = Visibility.Hidden;
        }

        private void UpperLimit_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(UpperLimit_TB, "Upper limit");
        }

        private void UpperLimit_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(UpperLimit_TB, "Upper limit");
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private void LowerLimit_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(LowerLimit_TB, "Lower limit");
        }

        private void LowerLimit_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(LowerLimit_TB, "Lower limit");
        }

        private void Select_B_Click(object sender, RoutedEventArgs e)
        {
            var selected = PatientGUI_cb.Text;
            int guiType = 1;
            if (selected == "Standard") { guiType = 1; }
            else if (selected == "Christmas") { guiType = 2;}
            



            cr.SendGUIInfo(guiType);

            PatientInterface_L.Visibility = Visibility.Visible;

        }

        private void AdjustBaseLine_B_Click(object sender, RoutedEventArgs e)
        {
            baseLine = cr.AdjustBaseLine();
            CurrentBaseline_TB.Text = Convert.ToString(baseLine);
            AdjustGatingValues();
        }
        
        private void Adjust_Limit_B_Click(object sender, RoutedEventArgs e)
        {
            if (UpperLimit_TB.Text != null && LowerLimit_TB.Text != null)
            {
                string result = cr.SaveGatingArea(Convert.ToDouble(LowerLimit_TB.Text),
                    Convert.ToDouble(UpperLimit_TB.Text));
                if (result == "Succes")
                {
                    UpperGatingValue = Convert.ToDouble(UpperLimit_TB.Text);
                    LowerGatingValue = Convert.ToDouble(LowerLimit_TB.Text);
                    AdjustGatingValues();
                    LimitValueWarning_Label.Text = result;
                    LimitValueWarning_Label.Visibility = Visibility.Visible;
                }
                else
                {
                    gatingValues = cr.GetGatingValue();
                    UpperLimit_TB.Text = Convert.ToString(gatingValues.UpperGatingValue);
                    LowerLimit_TB.Text = Convert.ToString(gatingValues.LowerGatingValue);
                    LimitValueWarning_Label.Text = result;
                    LimitValueWarning_Label.Visibility = Visibility.Visible;
                    
                }


                
                
            }
        }

        private void Close_B_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public void GotLostFocus(TextBox a, string text)
        {
            if (a.Text==text)
            {
                a.Text = "";
            }
            else if (a.Text == string.Empty)
            {
                a.Text = text;
            }

        }

    }


}
