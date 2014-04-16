using System.Collections.Generic;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    public interface IEstimationEngine
    {
        double Estimate(IEnumerable<IDateValue> dateValues);
    }
}