using System;
using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<LineDataModel>()
                .X(model => model.time.Ticks)
                .Y(model => model.Rawdata);

            LinedataValues = new ChartValues<LineDataModel>();
            var collection = new SeriesCollection();
            var LinechartSeriesCollection = new LineSeries();


            DateTimeFormatter = value => new DateTime((long) value).ToString("mm:ss:ms");

            DataContext = this;
        }

        public ChartValues<LineDataModel> LinedataValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
    }
}