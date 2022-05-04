using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation
{
    public class MonteCarlo
    {
        /// <summary>
        /// gleichverteilte Toleranz
        /// </summary>
        public double Tolerance { get; set; }

        /// <summary>
        /// Nennwert d. Elemente
        /// </summary>
        public double NomValue { get; set; }

        /// <summary>
        /// Schwellwert der nicht überschritten werden darf (Abweichung)
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// Idealmaß (Erwartungswert)
        /// </summary>
        public double IdealValue { get; set; }

        /// <summary>
        /// Eigenschaft für Zufahlszahl
        /// </summary>
        public RandGauss Gauss { get; set; }

        public Random rnd = new Random();
        public MonteCarlo(double tolerance, double nomValue, double variance, double idealValue)
        {
            Tolerance = tolerance;
            NomValue = nomValue;
            Variance = variance;
            IdealValue = idealValue;

            Gauss = new RandGauss();
        }

        public double GetArraySum(double[] array)
        {
            double returnSum = 0;

            for (int i = 0; i < array.Length; i++)
            {
                returnSum += array[i];
            }

            return returnSum;
        }

        public double GetGaussianValue()
        {
            return NomValue - Tolerance + NomValue * Tolerance * (1 - rnd.NextDouble());
        }

        /// <summary>
        /// Grundlage für GetExpectedProfitPerMonth().
        /// NOT USED
        /// </summary>
        /// <param name="experiments"></param>
        /// <param name="elementCount"></param>
        /// <returns></returns>
        public double GetProbabilityExpectationsExceeded(long experiments, int elementCount)
        {
            double returnProbability;
            double[] elementsRND = new double[elementCount];
            double sum;
            long hit = 0; //Variable für Treffer

            double compareSum;

            

            for (int i = 1; i < experiments; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    elementsRND[j] = GetGaussianValue();

                    //elementsRND[j] = Gauss.RandomGauss(IdealValue, Variance);
                }

                sum = GetArraySum(elementsRND);
                compareSum = Math.Abs((sum - IdealValue) / elementCount);
                if (compareSum > Variance)
                {
                    hit++;
                }
            }
            returnProbability = (double)hit / experiments;
            return returnProbability;
        }

        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="experiments"></param>
        /// <param name="elementCount"></param>
        /// <param name="sunnyMark"></param>
        /// <returns></returns>
        public double GetProbabilitySunnyDays(long experiments, int elementCount, double sunnyMark)
        {
            double returnValue = 0.0;
            double[] elementsRND = new double[elementCount];
            double sum;
            long hit = 0; //Variable für Treffer
            double compareSum;
            Random rnd = new Random();

            for (int i = 1; i < experiments; i++)
            {
                for (int j = 0; j < elementCount; j++)
                {
                    elementsRND[j] = NomValue - (Tolerance) + (NomValue * Tolerance) * (1 - rnd.NextDouble());

                    //elementsRND[j] = Gauss.RandomGauss(IdealValue, Variance);
                }

                sum = GetArraySum(elementsRND);
                compareSum = Math.Abs((sum - IdealValue) / elementCount);
                if (compareSum > sunnyMark)
                {
                    hit++;
                }
            }
            returnValue = (double)hit / experiments;

            return returnValue;
        }

    }
}
