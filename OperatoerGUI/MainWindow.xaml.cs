using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using OperatoerLibrary.DTO;

namespace OperatoerGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ChartValues<ChartModel> ChartValues { get; set; }

        public MainWindow()
        {
            InitializeComponent();

          

            
            Linechart();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ChartValues<LineDataModel> LinedataValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        private double xAxisMax;
        private double xAxisMin;
        private double yAxisMax;
        private double yAxisMin;
        public bool IsReading { get; set; }

        public double XAxisMax
        {
            get { return xAxisMax; }
            set
            {
                xAxisMax = value;
                propertyChanged("XAxisMax");

            }
        }
        public double XAxisMin
        {
            get { return XAxisMin; }
            set
            {
                xAxisMin = value;
                propertyChanged("XAxisMin");

            }
        }


        protected virtual void propertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string[] Labels { get; set; }

        private void Linechart()
        {

            var mapper = Mappers.Xy<ChartModel>()
                .X(model => model.time.Ticks)
                .Y(model => model.breathData);

            Charting.For<ChartModel>(mapper);


           
            ChartValues = new ChartValues<ChartModel>();

            DateTimeFormatter = value => new DateTime((long) value).ToString("mm:ss:ms");


            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            AxisUnit = TimeSpan.TicksPerSecond;





            DataContext = this;

        }

        private void SetAxisLimits(DateTime now)
        {
            xAxisMax = now.Ticks + TimeSpan.FromSeconds(0).Ticks;
            xAxisMin = now.Ticks - TimeSpan.FromSeconds(5).Ticks;

        }

        private void read()
        {
            while (IsReading)
            {


                try
                {
                    ChartValues.Add(new ChartModel
                    {
                        //time = ting i dto
                        //Breathdata = ting i DTO
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }
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
    }
}