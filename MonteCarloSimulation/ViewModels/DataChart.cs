using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

using MonteCarloSimulation.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MonteCarloSimulation.ViewModels
{
    public class DataChart: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ObservableCollection<ObservableValue> _observableValues;
        public ObservableCollection<ISeries> Series { get; set; }

        public List<Axis> XAxes { get; set; } = new List<Axis>
        {
            new Axis
            {
                Name = "Monate",
                NamePaint = new SolidColorPaint { Color = SKColors.Red
            },
                // Use the labels property for named or static labels
                MinLimit = 0,
                MaxLimit = 12,
                MinStep = 0.5,
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez" },
                LabelsRotation = 15,
            }
        };
        public List<Axis> YAxes { get; set; } = new List<Axis>
        {
            new Axis
            {
                Name = "Gewinn in €",
                NamePadding = new LiveChartsCore.Drawing.Padding(0, 15),

                // Use the Labeler property to give format to the axis values 
                // Now the Y axis we will display it as currency
                // LiveCharts provides some common formatters
                // in this case we are using the currency formatter.
                Labeler = Labelers.Currency 

                // you could also build your own currency formatter
                // for example:
                // Labeler = (value) => value.ToString("C") 

                // But the one that LiveCharts provides creates shorter labels when
                // the amount is in millions or trillions
            }
        };

        private string _resultString;
        public string ResultString { 
            get { return _resultString; }
            set { 
                _resultString = value;
                OnPropertyChanged("ResultString");
            }
        }

        private ObservableCollection<ReportData> _reports;
        public ObservableCollection<ReportData> Reports {
            get { return _reports; }
            set
            {
                _reports = value;
                OnPropertyChanged("Reports");
            } 
        }
        public DataChart()
        {
            _observableValues = new ObservableCollection<ObservableValue>();

            Series = new ObservableCollection<ISeries>
            {
                new ColumnSeries<ObservableValue>
                {
                    Values = _observableValues,
                    //Fill = null
                },
            };
            _resultString = "Results";
            ResultString = _resultString;

            _reports = new ObservableCollection<ReportData>();
            _reports.Add(new ReportData("Januar", 0));
            Reports = _reports;
        }

        public void AddSeries(List<double> values)
        {
            Series.Add(new ColumnSeries<double> { Values = values });
        }
        public void AddSeries()
        {
            IDictionary<string, double> monthlyResults = Handler.GetMonthlyReportVals();
            List<double> chartInput = new List<double>();
            ResultString = "";
            Reports.Clear();
            foreach(KeyValuePair<string, double> value in monthlyResults)
            {
                chartInput.Add(value.Value);
                //ResultString += value.Key + ":                      " + value.Value.ToString() + " €\n";
                ResultString += Handler.Get30CharLine(value.Key, value.Value.ToString() + "€");
                Reports.Add(new ReportData(value.Key, value.Value));
            }

            Series.Add(new ColumnSeries<double> { Values = chartInput });
        }
        public ICommand AddSeriesCommand => new Command(o => AddSeries());
        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
