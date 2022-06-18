using MonteCarloSimulation.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MonteCarloSimulation.Model;
using Microsoft.Win32;

namespace MonteCarloSimulation
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MonteCarlo MonteCarloObj { get; set; }
        public OpenAirPool Pool { get; set; }

        public int Experiments { get; set; }

        public double SunnyMark { get; set; }

        public int Customers { get; set; }
        public double TicketReg { get; set; }
        public double TicketSpec { get; set; }
        public double ProbTicketReg { get; set; }

        public decimal MonthlyCosts { get; set; }

        private List<WeatherData> values = new List<WeatherData>();


        OpenFileDialog openFile = new OpenFileDialog();
        public MainWindow()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            openFile.InitialDirectory = "c:\\";
            openFile.Filter = "csv files (*.csv)|*.csv";

            InitializeComponent();

            Customers = 0;
            TicketReg = 0;
            TicketSpec = 0;
            ProbTicketReg = 0;
            MonthlyCosts = 0;

            Pool = new OpenAirPool(ProbTicketReg, TicketReg, TicketSpec, Customers, MonthlyCosts, 0);

            SetParamProperties();

            if (File.Exists(csvPathBox.Text))
            {
                Handler.SetSeasonContext(csvPathBox.Text);
            }
            else
            {
                resultLabel.Content = "Warning: CSV-Path does not exist";
                resultLabel.Background = Brushes.Yellow;
            }
        }

        private void SetParamProperties()
        {
            if (!string.IsNullOrEmpty(textBoxEntranceReg.Text))
            {
                Handler.TicketReg = double.Parse(textBoxEntranceReg.Text, CultureInfo.InvariantCulture);             

                resultLabel.Content = labelEntranceReg.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
                return;
            }

            if (!string.IsNullOrEmpty(textBoxEntranceSpec.Text))
            {
                Handler.TicketSpec = double.Parse(textBoxEntranceSpec.Text, CultureInfo.InvariantCulture);

                resultLabel.Content = labelEntranceSpec.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
                return;
            }

            if (!string.IsNullOrEmpty(textBoxProbReg.Text))
            {
                Handler.ProbTicketReg = double.Parse(textBoxProbReg.Text, CultureInfo.InvariantCulture);

                resultLabel.Content = labelProbReg.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
                return;
            }

            if (!string.IsNullOrEmpty(textBoxMonthlyCosts.Text))
            {
                Handler.MonthlyCosts = decimal.Parse(textBoxMonthlyCosts.Text, CultureInfo.InvariantCulture);
                Handler.DailyyCosts = Handler.MonthlyCosts * 12 / 365.25M;

                resultLabel.Content = labelMonthlyCosts.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
                return;
            }

            if (!string.IsNullOrEmpty(textBoxPotentialCustomers.Text))
            {
                Handler.Customers = int.Parse(textBoxPotentialCustomers.Text, CultureInfo.InvariantCulture);
               
                resultLabel.Content = labelPotentialCustomers.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
                return;
            }

            if (!string.IsNullOrEmpty(textBoxExperiments.Text))
            {
                Handler.Experiments = int.Parse(textBoxExperiments.Text, CultureInfo.InvariantCulture);

                resultLabel.Content = labelExperiments.Content + " set";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = "Warning: Parameter not set";
                resultLabel.Background = Brushes.Yellow;
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            SetParamProperties();
        }

        private void buttonLoadCsvPath_Click(object sender, RoutedEventArgs e)
        {
            Pool.Seasons.Clear();

            string loadPath;
            if (openFile.ShowDialog() == true)
            {
                loadPath = openFile.FileName;

                csvPathBox.Text = loadPath;
            }


            if (File.Exists(csvPathBox.Text))
            {
                //values = File.ReadAllLines(csvPathBox.Text)
                //             .Skip(1)
                //             .Select(v => WeatherData.FromCsv(v))
                //             .ToList();
                resultLabel.Content = "Success: CSV Data read";
                resultLabel.Background = Brushes.Green;
                Handler.SetSeasonContext(csvPathBox.Text);
            }
            else
            {
                resultLabel.Content = "Warning: CSV-Path does not exist";
                resultLabel.Background = Brushes.Yellow;
            }

            
        }
    }
}
