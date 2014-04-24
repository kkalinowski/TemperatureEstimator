using lib12.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    [Singleton]
    public class TrendEngine : IEstimationEngine
    {
        private const int SampleLength = 14;
        private const double NextWeightCoefficent = 0.7;

        public Estimator Estimator { get { return Estimator.Trend; } }
        public string EstimatorName { get { return "Trend estimation"; } }
        public string EstimatorInfo { get { return "https://en.wikipedia.org/wiki/Trend_estimation"; } }

        public EstimationResult Estimate(IEnumerable<IDateValue> dateValues)
        {
            var data = dateValues.Select(x => x.Value).Reverse().ToArray();
            var weight = 1.0;
            var weightSum = 0.0;
            var trend = 0.0;

            for (var i = 0; i < SampleLength; i++)
            {
                trend += weight * (data[i] - data[i + 1]);
                weightSum += weight;
                weight *= NextWeightCoefficent;                
            }

            trend /= weightSum;
            return EstimationResult.Create(data[0] + trend, this);
        }
    }
}