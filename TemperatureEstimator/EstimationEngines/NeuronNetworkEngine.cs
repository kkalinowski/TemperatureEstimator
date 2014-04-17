using AForge.Neuro;
using AForge.Neuro.Learning;
using lib12.Core;
using lib12.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.EstimationEngines
{
    [Singleton]
    public class NeuronNetworkEngine : IEstimationEngine
    {
        private const int Iterations = 1000;
        private const double LearningRate = 0.1;
        private const double Momentum = 0;
        private const double SigmoidAlphaValue = 2.0;
        private const int LayerWidth = 5;

        public Estimator Estimator { get { return Estimator.NeuronNetwork; } }

        public EstimationResult Estimate(IEnumerable<IDateValue> dateValues)
        {
            var data = dateValues.ToArray();
            var samplesCount = data.Length - LayerWidth;
            var factor = 1.7 / data.Length;
            var yMin = data.Min(x => x.Value);

            var input = new double[samplesCount][];
            var output = new double[samplesCount][];

            for (var i = 0; i < samplesCount; i++)
            {
                input[i] = new double[LayerWidth];
                output[i] = new double[1];

                for (var j = 0; j < LayerWidth; j++)
                    input[i][j] = (data[i + j].Value - yMin) * factor - 0.85;

                output[i][0] = (data[i + LayerWidth].Value - yMin) * factor - 0.85;
            }

            var network = new ActivationNetwork(
                new BipolarSigmoidFunction(SigmoidAlphaValue),
                LayerWidth, LayerWidth * 2, 1);

            var teacher = new BackPropagationLearning(network)
            {
                LearningRate = LearningRate,
                Momentum = Momentum
            };

            var solutionSize = data.Length - LayerWidth;
            var solution = new double[solutionSize, 2];
            var networkInput = new double[LayerWidth];

            for (var j = 0; j < solutionSize; j++)
                solution[j, 0] = j + LayerWidth;

            TimesLoop.Do(Iterations, () =>
            {
                teacher.RunEpoch(input, output);

                for (int i = 0, n = data.Length - LayerWidth; i < n; i++)
                {
                    for (var j = 0; j < LayerWidth; j++)
                        networkInput[j] = (data[i + j].Value - yMin) * factor - 0.85;

                    solution[i, 1] = (network.Compute(networkInput)[0] + 0.85) / factor + yMin;
                }
            });

            return EstimationResult.Create(solution[0, 1], this);
        }
    }
}