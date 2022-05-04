using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation.Model
{
    public class ReportData
    {
        private string monthName;

        public string MonthName
        {
            get { return monthName; }
            set { monthName = value; }
        }

        private double estimatedProfit;

        public double EstimatedProfit
        {
            get { return estimatedProfit; }
            set { estimatedProfit = value; }
        }

        
        public ReportData(string month, double profit)
        {
            MonthName = month;
            EstimatedProfit = profit;
        }

    }
}
