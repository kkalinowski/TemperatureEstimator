namespace TemperatureEstimator.EstimationEngines
{
    public class EstimationResult
    {
        public double Value { get; set; }
        public Estimator Estimator { get; set; }
        public double Error { get; set; }
        public string EstimatorInfo { get; set; }
        public string EstimatorName { get; set; }

        public static EstimationResult Create(double value, IEstimationEngine engine)
        {
            return new EstimationResult
            {
                Value = value,
                Estimator = engine.Estimator,
                EstimatorName = engine.EstimatorName,
                EstimatorInfo = engine.EstimatorInfo
            };
        }
    }
}