using System.Collections.Generic;
using System.Linq;
using lib12.Collections;
using lib12.DependencyInjection;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    [Singleton]
    public class WeightedMeanEngine : IEstimationEngine
    {
        public Estimator Estimator { get { return Estimator.WeightedMean; } }
        public string EstimatorName { get { return "Weighted mean"; } }
        public string EstimatorInfo { get { return "https://en.wikipedia.org/wiki/Weighted_arithmetic_mean"; } }

        public EstimationResult Estimate(IEnumerable<IDateValue> dateValues)
        {
            var last5 = dateValues.TakeLast(5).Select(x => x.Value).ToArray();
            var result = (40 * last5[4] + 25 * last5[3] + 15 * last5[2] + 12 * last5[1] + 8 * last5[0]) / 100;
            return EstimationResult.Create(result, this);
        }
    }
}