using System.Collections.Generic;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    public interface IEstimationEngine
    {
        EstimationResult Estimate(IEnumerable<IDateValue> dateValues);
        Estimator Estimator { get; }
        string EstimatorName { get; }
        string EstimatorInfo { get; }
    }
}