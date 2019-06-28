﻿using System;
using ImageProcessing.Distributions.Abstract;

namespace ImageProcessing.Distributions.Two___Parameter_Distributions
{
    class WeibullDistribution : IDistribution
    {

        public WeibullDistribution(double lambda, double k)
        {
            this.lambda = lambda;
            this.k      = k;
        }

        private double lambda { get; set; }
        private double k      { get; set; }

        public string Name { get; } = "Weibull Distribution";

        public double GetMean()
        {
            throw new NotImplementedException();
        }
   
        public double GetVariance()
        {
            throw new NotImplementedException();
        }

        public double Quantile(double p) => lambda * Math.Pow(-Math.Log(1 - p), 1.0 / k);
   
    }
}
