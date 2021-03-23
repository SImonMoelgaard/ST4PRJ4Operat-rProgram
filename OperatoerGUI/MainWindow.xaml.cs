using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Configurations;
using OperatoerLibrary.DTO;

namespace OperatoerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChartValues<LineDataModel> LinedataValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            var mapper = Mappers.Xy<LineDataModel>()
                .X(model => model.time.Ticks)
                .Y(model => model.Rawdata);

            LinedataValues = new ChartValues<LineDataModel>();
            SeriesCollection collection = new SeriesCollection();
            LineSeries LinechartSeriesCollection = new LineSeries();
            


            DateTimeFormatter = value => new DateTime((long) value).ToString("mm:ss:ms");

            DataContext = this;
        }
    }
}
