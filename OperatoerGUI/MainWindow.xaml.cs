using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
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
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;
using OperatoerLibrary.Timer;
using CountDownTimer = OperatoerLibrary.Timer.CountDownTimer;


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
        

        private string timeElapsed;
        private string timeRemaining;
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

        private double UpperGatingValue = 0.5, LowerGatingValue = 0.4;
        private double baseLine = 0;
        private DTO_GatingValues gatingValues;

         BlockingCollection<BreathingValuesDataContainer> _datacollection = new BlockingCollection<BreathingValuesDataContainer>();
         public event PropertyChangedEventHandler PropertyChanged;
         
        
         private Controller cr;
         private ICountDownTimer countDownTimer = new CountDownTimer();
         private ICountUpTimer countUpTimer = new CountUpTimer();
         
         private double value;
         
         private int count;
         
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

            countDownTimer.Expired += new EventHandler(OnTimerExpired);
            countDownTimer.TimerTick += new EventHandler(OnTimerTick);
            countUpTimer.TimerTick+=new EventHandler(TimeLasted);
            
            cr.RunProducer();

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



        

        private void Read()
        {
            
            while (IsReading)
            {
                
                    try
                    {
                        BreathingValuesDataContainer data = _breathingData.Take();
                        
                        Console.ReadLine();
                        value = data.BreathingSample;
                   

                        //Metode der kaldes for at få data fra køen
                        
                            double dataPoint = cr.AdjustBaseLine(value);

                            DTO_Send = new DTO_Measurement(dataPoint, LowerGatingValue, UpperGatingValue, DateTime.Now);
                            countDownTimer.Start(dataPoint, LowerGatingValue, UpperGatingValue);
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
                                TimeRemaining_TB.Text = Convert.ToString(timeRemaining);
                                TimeElapsed_TB.Text = Convert.ToString(timeElapsed);

                                if (AutoBaseLineWarning_L.Visibility == Visibility.Visible|| ManualBaseLineWarning_L.Visibility == Visibility.Visible || LimitValueWarning_Label.Visibility == Visibility.Visible)
                                {
                                    count++;
                                    if (count>=150)
                                    {
                                        AutoBaseLineWarning_L.Visibility = Visibility.Hidden;
                                        ManualBaseLineWarning_L.Visibility = Visibility.Hidden;
                                        LimitValueWarning_Label.Visibility = Visibility.Hidden;
                                        count = 0;
                                    } 
                                }

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
            
            UpperGatingValue = UpperGatingValue - baseLine;
            LowerGatingValue = LowerGatingValue - baseLine;

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
            countUpTimer.Start();
            Stop_B.Visibility = Visibility.Visible;
            Start_B.Visibility = Visibility.Hidden;
            Start_B_gray.Visibility = Visibility.Visible;
            Stop_B_gray.Visibility = Visibility.Hidden;
            
        }

        private void Stop_B_Click_1(object sender, RoutedEventArgs e)
        {
            IsReading = false;
            countUpTimer.Stop();
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
            PatientGUI_cb.Text = "";
        }

        private void AdjustBaseLine_B_Click(object sender, RoutedEventArgs e)
        {
            baseLine = cr.AdjustBaseLine();
            
            
            var formattedBaseLine = baseLine.ToString("0.000", CultureInfo.CreateSpecificCulture("da-DK"));
            formattedBaseLine = formattedBaseLine.Replace(",", ".");
            ManualBaseLine_TB.Text = formattedBaseLine;
            CurrentBaseline_TB.Text = formattedBaseLine;
            AutoBaseLineWarning_L.Text = "Success";
            AutoBaseLineWarning_L.Visibility = Visibility.Visible;

            // AdjustGatingValues();
        }
        
        private void Adjust_Limit_B_Click(object sender, RoutedEventArgs e)
        {
            if (UpperLimit_TB.Text != "Upper limit" && LowerLimit_TB.Text != "Lower limit")
            {
                string result = cr.SaveGatingArea(Convert.ToDouble(LowerLimit_TB.Text),
                    Convert.ToDouble(UpperLimit_TB.Text));
                if (result == "Success")
                {
                    var sampleUpper = decimal.Parse(UpperLimit_TB.Text, CultureInfo.InvariantCulture);
                    var sampleLower = decimal.Parse(LowerLimit_TB.Text, CultureInfo.InvariantCulture);
                    
                    UpperGatingValue = Convert.ToDouble(sampleUpper);
                    LowerGatingValue = Convert.ToDouble(sampleLower);
                    //AdjustGatingValues();
                    LimitValueWarning_Label.Text = result;
                    LimitValueWarning_Label.Foreground = Brushes.LawnGreen;
                    LimitValueWarning_Label.Visibility = Visibility.Visible;
                    CurrentGatingValues_TB.Text = Convert.ToString(LowerGatingValue + " - " + UpperGatingValue);
                    
                }
                else
                {
                    try
                    {
                        gatingValues = cr.GetGatingValue();
                        UpperLimit_TB.Text = Convert.ToString(gatingValues.UpperGatingValue);
                        LowerLimit_TB.Text = Convert.ToString(gatingValues.LowerGatingValue);
                        LimitValueWarning_Label.Text = result;
                        LimitValueWarning_Label.Visibility = Visibility.Visible;
                        LimitValueWarning_Label.Foreground = Brushes.Red;
                    }
                    catch (InvalidOperationException)
                    {
                        
                        throw;
                    }
                }
            }
            else
            {
                LimitValueWarning_Label.Text = "Enter Values";
                LimitValueWarning_Label.Visibility = Visibility.Visible;
                LimitValueWarning_Label.Foreground = Brushes.Red;
            }
        }

        private void Close_B_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void AdjustBaseLinemanual_B_Click(object sender, RoutedEventArgs e)
        {
            var sample = decimal.Parse(ManualBaseLine_TB.Text, CultureInfo.InvariantCulture);
            baseLine = Convert.ToDouble(sample);
            cr.SaveBaseLineValue(baseLine);
            var formattedBaseLine = baseLine.ToString("0.000");
            CurrentBaseline_TB.Text = formattedBaseLine;
            ManualBaseLineWarning_L.Text = "Success";
            ManualBaseLineWarning_L.Visibility = Visibility.Visible;
        }

        private void TimeToTreat_B_Click(object sender, RoutedEventArgs e)
        {
            
            int time = Convert.ToInt32(TimeToTreat_TB.Text);
            int minutes = time / 60;
            int seconds = time % 60;
            string timer ="";
            
            if (minutes<=9)
            {
                timer += string.Join("","0");
            }
            timer  += string.Join("",minutes);
            timer += string.Join("",":");
            if (seconds<=9)
            {
                timer += string.Join("","0");
            }
            timer += string.Join("",seconds);



            TimeRemaining_TB.Text = Convert.ToString(timer);
            countDownTimer.SetTime(time);
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

        public void OnTimerExpired(object sender, EventArgs e)
        {
            IsReading = false;
            countUpTimer.Stop();
            Start_B.Visibility = Visibility.Visible;
            Stop_B.Visibility = Visibility.Hidden;
            Stop_B_gray.Visibility = Visibility.Visible;
            Start_B_gray.Visibility = Visibility.Hidden;
        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            timeRemaining = "";
            double time = countDownTimer.RemainingTime;
            int timeInt = Convert.ToInt32(time);
            int minutes = timeInt / 60;
            double seconds = time % 60;
         

            
            if (minutes<=9)
            {
                timeRemaining += string.Join("","0");
            }
            timeRemaining += string.Join("",minutes);
            timeRemaining += string.Join("",":");
            if (seconds<=9)
            {
                
                timeRemaining += string.Join("","0");
            }
            //string.Format("{0:N3}", seconds);
            //seconds.ToString("N3");
            var formattedSeconds = seconds.ToString("0.000");
            timeRemaining += string.Join("",formattedSeconds);



        }

        private void TimeToTreat_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(TimeToTreat_TB, "Seconds");
        }

        private void TimeToTreat_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(TimeToTreat_TB, "Seconds");
        }

        private void ManualBaseLine_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(ManualBaseLine_TB, "Input Baseline");
        }

        private void ManualBaseLine_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            GotLostFocus(ManualBaseLine_TB, "Input Baseline");
        }

        
        public void TimeLasted(object sender, EventArgs e)
        {
            timeElapsed = "";
            int time = countUpTimer.countedTime;
            int minutes = time / 60;
            int seconds = time % 60;

            
            if (minutes<=9)
            {
                timeElapsed += string.Join("","0");
            }
            timeElapsed += string.Join("",minutes);
            timeElapsed += string.Join("",":");
            if (seconds<=9)
            {
                timeElapsed += string.Join("","0");
            }
            timeElapsed += string.Join("",seconds);


            
        }

    }


}
