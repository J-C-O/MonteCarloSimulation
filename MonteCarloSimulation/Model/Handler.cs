using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation.Model
{
    public static class Handler
    {
        public static OpenAirPool Pool = new OpenAirPool(0, 0, 0, 0, 0, 0);

        public static int Experiments { get; set; }
        public static int Customers {
            get { return Pool.PotentialCustomers; }
            set { Pool.PotentialCustomers = value; } 
        }
        public static double TicketReg {
            get { return Pool.EntranceReg; }
            set { Pool.EntranceReg = value; }
        }
        public static double TicketSpec {
            get { return Pool.EntranceSpec; }
            set { Pool.EntranceSpec = value; }
        }
        public static double ProbTicketReg {
            get { return Pool.EntranceRegProb; }
            set { Pool.EntranceRegProb = value; }
        }
        public static decimal MonthlyCosts {
            get { return Pool.MonthlyCosts; }
            set { Pool.MonthlyCosts = value; }
        }
        public static decimal DailyyCosts {
            get { return Pool.DailyCosts; }
            set { Pool.DailyCosts = value; }
        }

        private static List<WeatherData> values = new List<WeatherData>();

        public static string Get30CharLine(string firstEntry, string secondEntry)
        {
            if (!firstEntry.Contains(":"))
            {
                firstEntry += ":";
            }
            string returnString = new string('.', 15);
            int length1 = firstEntry.Length;
            int length2 = secondEntry.Length;
            int buff = 20 - length1;

            

            returnString = firstEntry + new string('_', buff) + secondEntry + "\n";
            //returnString = firstEntry + new string('_', buff) + '\n';
            int len = returnString.Length;
            return returnString;
        }

        public static List<string> GetValuesLabel()
        {
            List<string> returnList = new List<string>();
            string month = "";
            DateTime date;
            for(int i = 0; i < values.Count; i++)
            {
                date = new DateTime(2020, values[i].MonthID, 1);
                month = date.ToString("MMMM");
                returnList.Add(month);
            }
            return returnList;
        }
        public static List<int> GetMonthIDs()
        {
            List<int> returnList = new List<int>();
            for (int i = 0; i < values.Count; i++)
            {
                returnList.Add(values[i].MonthID);
            }
            return returnList;
        }
        public static void SetSeasonContext(string csvPath)
        {
 
                values = File.ReadAllLines(csvPath)
                             .Skip(1)
                             .Select(v => WeatherData.FromCsv(v))
                             .ToList();

            // clear season data
            Pool.Seasons.Clear();
            foreach (WeatherData weatherData in values)
            {
                Pool.Seasons.Add(new MonteCarlo(weatherData.Deviation, weatherData.ExpectedTemp, weatherData.Variance, weatherData.ExpectedTemp * weatherData.Days));
            }
        }
        private static double GetSimProfit(int index)
        {
            return Math.Round(Pool.GetExpectedProfitPerMonth(Experiments, values[index].Days, index), 2);
        }

        public static IDictionary<string, double> GetMonthlyReportVals()
        {
            DateTime date;
            string month = "";
            double profitTemp = 0;
            double[] estimatedProfit = new double[values.Count];

            IDictionary<string, double> returnList = new Dictionary<string, double>();

            for (int i = 0; i < values.Count; i++)
            {
                date = new DateTime(2020, values[i].MonthID, 1);
                month = date.ToString("MMMM");
                profitTemp = GetSimProfit(i);

                estimatedProfit[i] = profitTemp;

                returnList.Add(new KeyValuePair<string, double>(month, profitTemp));
            }

            return returnList;
        }
    }
}
