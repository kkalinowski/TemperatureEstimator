using System.Collections.Generic;
using ABMath.ModelFramework.Data;
using ABMath.ModelFramework.Models;
using lib12.Collections;
using lib12.DependencyInjection;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    [Singleton]
    public class ArmaEngine : IEstimationEngine
    {
        private readonly ARMAModel armaModel;

        public ArmaEngine()
        {
            armaModel = new ARMAModel(4, 3);
        }

        public double Estimate(IEnumerable<IDateValue> dateValues)
        {
            var series = new TimeSeries();
            dateValues.ForEach(x => series.Add(x.Date, x.Value, true));

            armaModel.SetInput(0, series, null);
            armaModel.FitByMLE(10, 10, 10, null);
            armaModel.ComputeResidualsAndOutputs();
            
            var result = armaModel.GetOutput(3) as TimeSeries;
            return result[0];
        }
    }
}