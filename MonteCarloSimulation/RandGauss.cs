using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarloSimulation
{
    public class RandGauss : Random
    {
        private double u1;
        private double u2;
        private double temp1;
        private double temp2;

        public RandGauss() : base()
        {

        }

        public RandGauss(int seed) : base(seed)
        {

        }

        public double RandomGauss(double mu = 0, double sigma = 1)
        {
            if (sigma <= 0)
            {
                throw new ArgumentOutOfRangeException("sigma", "Must be greater than zero.");
            }

            u1 = base.NextDouble();
            u2 = base.NextDouble();
            temp1 = Math.Sqrt(-2 * Math.Log(u1));
            temp2 = 2 * Math.PI * u2;

            return mu + sigma * (temp1 * Math.Cos(temp2));
        }
    }
}
