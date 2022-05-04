using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation
{
    public class WeatherData
    {
        public int YearID;
        public int MonthID;
        public double ExpectedTemp;
        public double Deviation;
        public double Variance;
        public int Days;

        public static WeatherData FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            WeatherData weatherData = new WeatherData();

            weatherData.YearID = Convert.ToInt32(values[0]);
            weatherData.MonthID = Convert.ToInt32(values[1]);
            weatherData.ExpectedTemp = double.Parse(values[2], CultureInfo.InvariantCulture);
            weatherData.Deviation = double.Parse(values[3], CultureInfo.InvariantCulture);
            weatherData.Variance = weatherData.Deviation * weatherData.Deviation;
            weatherData.Days = DateTime.DaysInMonth(weatherData.YearID, weatherData.MonthID);

            return weatherData;
        }
    }
}
