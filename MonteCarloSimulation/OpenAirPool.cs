using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation
{
    public class OpenAirPool
    {
        /// <summary>
        /// Liste in denen MonteCarlo-Objekte gespeichert werden.
        /// Im Programmkontext soll jeder zu simulierende Monat über ein MonteCarlo-Objekt mit entsprechenden Daten verfügen.
        /// </summary>
        public List<MonteCarlo> Seasons = new List<MonteCarlo>();
        public int PotentialCustomers { get; set; }
        public double EntranceReg { get; set; }
        public double EntranceSpec { get; set; }
        public double EntranceRegProb { get; set; }
        public decimal MonthlyCosts { get; set; }
        public decimal DailyCosts { get; set; }


        public OpenAirPool(double entranceRegProb, double entranceReg, double entranceSpec, int potCustomers, decimal monthlyCosts, decimal dailyCosts, params MonteCarlo[] monteCarlos)
        {
            EntranceRegProb = entranceRegProb;
            EntranceReg = entranceReg;
            EntranceSpec = entranceSpec;
            PotentialCustomers = potCustomers;
            MonthlyCosts = monthlyCosts;
            DailyCosts = dailyCosts;
            for (int i = 0; i < monteCarlos.Length; i++)
            {
                Seasons.Add(monteCarlos[i]);
            }
        }
        public OpenAirPool(double entranceRegProb, double entranceReg, double entranceSpec, int potCustomers, decimal monthlyCosts, decimal dailyCosts)
        {
            EntranceRegProb = entranceRegProb;
            EntranceReg = entranceReg;
            EntranceSpec = entranceSpec;
            PotentialCustomers = potCustomers;
            MonthlyCosts = monthlyCosts;
            DailyCosts = dailyCosts;
        }

        /// <summary>
        /// Gewinnfunktion. Erhält Kundenanzahl, Eintrittspreis und Kosten.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="entranceCosts"></param>
        /// <param name="costsPerTimeUnit"></param>
        /// <returns></returns>
        public double GetProfit(int customer, double entranceCosts, decimal costsPerTimeUnit)
        {
            return customer * entranceCosts - (double)costsPerTimeUnit;
        }
        
        /// <summary>
        /// Umsatzfunktion. Diese Funktion ist eine Überladung der eigentlichen Gewinnfunktion,
        /// allerdings werden die Kosten hier noch nicht abgezogen, wodurch sie faktisch zur Erlösfunktion wird.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="entranceCosts"></param>
        /// <returns></returns>
        public double GetProfit(int customer, double entranceCosts)
        {
            return customer * entranceCosts;
        }

        /// <summary>
        /// Noch nicht fertig implementiert.
        /// siehe GetExpectedProfitPerMonth() nur für Tage statt Monate
        /// </summary>
        /// <param name="experiments"></param>
        /// <param name="elementCount"></param>
        /// <param name="monteCarloIndex"></param>
        /// <returns></returns>
        public double[] GetExpectedProfitPerDay(long experiments, int elementCount, int monteCarloIndex)
        {
            double expectedCustomers = 0.0;
            double[] elementsRND = new double[elementCount];
            double sum;
            double customerFactor = 0; //Kundenfaktor für Treffer
            double customerFactorSum = 0;
            double compareTemp;
            Random rnd = new Random();

            double[] expectedProfit = new double[elementCount];

            for (int i = 1; i < experiments; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    elementsRND[j] = Seasons[monteCarloIndex].GetGaussianValue();

                    //hier auswertung pro tag
                    if (elementsRND[j] < 12)
                    {
                        customerFactor = 0;
                    }
                    else if (elementsRND[j] >= 12 && elementsRND[j] < 20)
                    {
                        customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (0.4 - 0.1) + 0.1;
                    }
                    else if (elementsRND[j] >= 20 && elementsRND[j] > 30)
                    {
                        customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (1.3 - 0.7) + 0.7; ;
                    }
                    else if (elementsRND[j] >= 30)
                    {
                        customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (1.5 - 0.7) + 0.7;
                    }
                    
                    int expectedCustomersPerExp = (int)Math.Abs(PotentialCustomers * customerFactor);
                    if (expectedCustomersPerExp > 0)
                    {
                        customerFactorSum += customerFactor;
                        expectedCustomers += expectedCustomersPerExp;
                        expectedProfit[j] += GetExpectedProfitPerCustomers(expectedCustomersPerExp, EntranceRegProb, monteCarloIndex, DailyCosts);
                    }
                }               
            }
            for(int i = 0; i < elementCount; i++)
            {
                expectedProfit[i] = expectedProfit[i] / experiments;
            }
            return expectedProfit;
        }

        /// <summary>
        /// Simuliert die durchschnittlichen Monatstemperaturen und ermittelt basierend darauf den monatlichen Gewinn.
        /// </summary>
        /// <param name="experiments"></param>
        /// <param name="elementCount"></param>
        /// <param name="monteCarloIndex"></param>
        /// <returns></returns>
        public double GetExpectedProfitPerMonth(long experiments, int elementCount, int monteCarloIndex)
        {
            double expectedCustomers = 0.0;
            double[] elementsRND = new double[elementCount];
            double sum;
            double customerFactor = 0; //Kundenfaktor für Treffer
            double compareTemp;
            Random rnd = new Random();

            double expectedProfit = 0;

            for (int i = 1; i < experiments; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    elementsRND[j] = Seasons[monteCarloIndex].GetGaussianValue();
                }

                sum = Seasons[monteCarloIndex].GetArraySum(elementsRND);

                //Auswertung pro Monat
                compareTemp = Math.Abs((sum - Seasons[monteCarloIndex].IdealValue) / elementCount);

                if (compareTemp < 12)
                {
                    customerFactor = 0;
                }
                else if (compareTemp >= 12 && compareTemp < 20)
                {
                    customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (0.7 - 0.1) + 0.1;
                }
                else if (compareTemp >= 20 && compareTemp > 30)
                {
                    customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (1.3 - 0.7) + 0.7;
                }
                else if (compareTemp >= 30)
                {
                    customerFactor = Seasons[monteCarloIndex].rnd.NextDouble() * (1.5 - 0.7) + 0.7;
                }
                int expectedCustomersPerExp = (int)Math.Abs(PotentialCustomers * customerFactor);
                if (expectedCustomersPerExp > 0)
                {
                    expectedProfit += GetExpectedProfitPerCustomers(expectedCustomersPerExp, EntranceRegProb, monteCarloIndex, MonthlyCosts);
                }
                
                expectedCustomers += expectedCustomersPerExp;
            }
            return expectedProfit / experiments;
        }

        /// <summary>
        /// Ermittelt den Eintrittspreis für die übergeben Kundenanzahl
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="probability"> beschreibt die Wahrscheinlichkeit mit der ein Kunde ein reguläres Ticket kauft</param>
        /// <param name="monteCarloIndex"></param>
        /// <param name="costsPerTimeUnit"></param>
        /// <returns></returns>
        public double GetExpectedProfitPerCustomers(double customers, double probability, int monteCarloIndex, decimal costsPerTimeUnit)
        {
            double returnValue = 0;
            long countReg = 0;
            long countSpec = 0;
            if(monteCarloIndex < Seasons.Count && monteCarloIndex >= 0)
            {
                
                for(int i = 0; i < customers; i++)
                {
                    bool isRegularTicket = Seasons[monteCarloIndex].rnd.NextDouble() <= probability;

                    if (isRegularTicket)
                    {
                        countReg++;
                    }
                    else
                    {
                        countSpec++;
                    }
                }
                // Kosten werden hier abgezogen, da sonst von jedem Kundenkauf die monatl. Kosten abgezogen werden würden.
                returnValue = GetProfit((int)countReg, EntranceReg) + GetProfit((int)countSpec, EntranceSpec) -(double)costsPerTimeUnit;
            }
            return returnValue;
        }
    }
}
