namespace Utility.Algorithms.BlackBoxOptimization.SimmulatedAnnealing
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Static class that provides access to simmulated annealing procedures.
    /// </summary>
    public static class SimulatedAnnealing
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Minimizes a given objective function through simmulated annealing.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvaluation">The type of the evaluation of a state.</typeparam>
        /// <param name="setup">Setup configuration for the problem to solve.</param>
        /// <param name="config">Configuration of the simulation.</param>
        /// <param name="tempConfig">The configuration for getting the initial temperature.</param>
        /// <returns>The result of the simulation.</returns>
        public static Result<TState, TEvaluation> Minimize<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup,
            SimulatedAnnealingConfig config, InitialTemperatureConfig tempConfig) where TEvaluation : IComparable
        {
            return Optimize(setup, config, false, null, tempConfig);
        }

        /// <summary>
        /// Maximizes a given objective function through simmulated annealing.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvaluation">The type of the evaluation of a state.</typeparam>
        /// <param name="setup">Setup configuration for the problem to solve.</param>
        /// <param name="config">Configuration of the simulation.</param>
        /// <param name="tempConfig">The configuration for getting the initial temperature.</param>
        /// <returns>The result of the simulation.</returns>
        public static Result<TState, TEvaluation> Maximize<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup,
            SimulatedAnnealingConfig config, InitialTemperatureConfig tempConfig) where TEvaluation : IComparable
        {
            return Optimize(setup, config, true, null, tempConfig);
        }

        /// <summary>
        /// Minimizes a given objective function through simmulated annealing.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvaluation">The type of the evaluation of a state.</typeparam>
        /// <param name="setup">Setup configuration for the problem to solve.</param>
        /// <param name="config">Configuration of the simulation.</param>
        /// <param name="initialTemperature">The initial temperature for the simulation.</param>
        /// <returns>The result of the simulation.</returns>
        public static Result<TState, TEvaluation> Minimize<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup,
            SimulatedAnnealingConfig config, double initialTemperature) where TEvaluation : IComparable
        {
            return Optimize(setup, config, false, initialTemperature, new InitialTemperatureConfig());
        }

        /// <summary>
        /// Maximizes a given objective function through simmulated annealing.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvaluation">The type of the evaluation of a state.</typeparam>
        /// <param name="setup">Setup configuration for the problem to solve.</param>
        /// <param name="config">Configuration of the simulation.</param>
        /// <param name="initialTemperature">The initial temperature for the simulation.</param>
        /// <returns>The result of the simulation.</returns>
        public static Result<TState, TEvaluation> Maximize<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup,
            SimulatedAnnealingConfig config, double initialTemperature) where TEvaluation : IComparable
        {
            return Optimize(setup, config, true, initialTemperature, new InitialTemperatureConfig());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Result<TState, TEvaluation> Optimize<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup, SimulatedAnnealingConfig config, bool maximize, double? initialTemperature,
            InitialTemperatureConfig tempConfig) where TEvaluation : IComparable
        {
            var bestState = setup.InitialState;
            var bestEvaluation = setup.ObjectiveFunction(setup.InitialState);
            var attempts = 1;
            var chainsWithoutChange = 0;
            var currentState = setup.InitialState;

            var temperature = initialTemperature ?? GetInitialTemperature(setup, config, tempConfig, maximize);

            while (chainsWithoutChange < config.MaxChainsWithoutChange)
            {
                var currBest = bestState;
                currentState = MarkovChain(setup, config, currentState, maximize, ref temperature, ref bestState, ref bestEvaluation, ref attempts);

                if (Equals(currBest, bestState)) chainsWithoutChange++;
                else chainsWithoutChange = 0;

                temperature *= config.TemperatureDropRate;
            }

            return new Result<TState, TEvaluation>
            {
                BestState = bestState,
                BestEvaluation = bestEvaluation,
                FinalState = currentState,
                FinalEvaluation = setup.ObjectiveFunction(currentState),
                FinalTemperature = temperature,
                Attempts = attempts
            };
        }

        private static double GetInitialTemperature<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup, 
            SimulatedAnnealingConfig config, InitialTemperatureConfig tempConfig, bool maximize) 
            where TEvaluation : IComparable
        {
            var temperature = tempConfig.Seed;
            var state = setup.InitialState;
            while (true)
            {
                state = MarkovChain(setup, config, state, maximize, temperature, out decimal acceptRate);
                if (acceptRate >= tempConfig.MinAcceptanceRate)
                    break;
                temperature *= tempConfig.GrowthRate;
            }
            return 0;
        }

        private static TState MarkovChain<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup, 
            SimulatedAnnealingConfig config, TState initialState, bool maximize, ref double temperature, 
            ref TState bestState, ref TEvaluation bestEvaluation, ref int totalAttempts) where TEvaluation : IComparable
        {
            var attempts = 1;
            var successfuAttempts = 0;
            var currentEvaluation = setup.ObjectiveFunction(initialState);
            var lastAcceptedEvaluation = currentEvaluation;
            var lastAcceptedState = initialState;

            while (attempts <= config.MaxAttemptsPerChain && successfuAttempts <= config.MaxAcceptedPerChain)
            {
                var neighbor = setup.NeighborFunction(lastAcceptedState);

                var evaluation = setup.ObjectiveFunction(neighbor);
                attempts++;
                totalAttempts++;

                setup.OutputFunction?.Invoke(new Result<TState, TEvaluation>
                {
                    BestState = bestState,
                    BestEvaluation = bestEvaluation,
                    FinalState = neighbor,
                    FinalEvaluation = evaluation,
                    Attempts = totalAttempts,
                    FinalTemperature = temperature
                });

                if (!IsAccepted(lastAcceptedEvaluation, evaluation, temperature, maximize,
                    setup.DistanceFunction)) continue;

                if (config.ChangeTemperatureOnChain)
                    temperature *= maximize
                        ? setup.RatioFunction(lastAcceptedEvaluation, evaluation)
                        : setup.RatioFunction(evaluation, lastAcceptedEvaluation);
                lastAcceptedState = neighbor;
                lastAcceptedEvaluation = evaluation;
                successfuAttempts++;

                UpdateBest(neighbor, evaluation, ref bestState, ref bestEvaluation, maximize);
            }

            return lastAcceptedState;
        }

        private static TState MarkovChain<TState, TEvaluation>(ProblemSetup<TState, TEvaluation> setup, 
            SimulatedAnnealingConfig config, TState initialState, bool maximize, double temperature, 
            out decimal acceptRate) where TEvaluation : IComparable
        {
            var attempts = 1;
            var successfuAttempts = 0;
            var currentEvaluation = setup.ObjectiveFunction(initialState);
            var lastAcceptedEvaluation = currentEvaluation;
            var lastAcceptedState = initialState;

            while (attempts <= config.MaxAttemptsPerChain && successfuAttempts <= config.MaxAcceptedPerChain)
            {
                var neighbor = setup.NeighborFunction(lastAcceptedState);

                var evaluation = setup.ObjectiveFunction(neighbor);
                attempts++;

                if (!IsAccepted(lastAcceptedEvaluation, evaluation, temperature, maximize,
                    setup.DistanceFunction)) continue;

                if (config.ChangeTemperatureOnChain)
                    temperature *= maximize
                        ? setup.RatioFunction(lastAcceptedEvaluation, evaluation)
                        : setup.RatioFunction(evaluation, lastAcceptedEvaluation);
                lastAcceptedState = neighbor;
                lastAcceptedEvaluation = evaluation;
                successfuAttempts++;
            }

            acceptRate = successfuAttempts / (decimal) attempts;
            return lastAcceptedState;
        }

        private static void UpdateBest<TState, TEvaluation>(TState currentState, TEvaluation currentEvaluation, 
            ref TState bestState, ref TEvaluation bestEvaluation, bool maximize) where TEvaluation : IComparable
        {
            var comparison = currentEvaluation.CompareTo(bestEvaluation);
            if (maximize)
            {
                if (comparison >= 0)
                {
                    bestState = currentState;
                    bestEvaluation = currentEvaluation;
                }
            }
            else
            {
                if (comparison < 0)
                {
                    bestState = currentState;
                    bestEvaluation = currentEvaluation;
                }
            }
        }

        private static bool IsAccepted<TEvaluation>(TEvaluation best, TEvaluation current, double temperatue, 
            bool maximize, Func<TEvaluation, TEvaluation, double> distanceFunction) where TEvaluation : IComparable
        {
            var comparison = current.CompareTo(best);
            if (maximize && comparison >= 0) return true;
            if (!maximize && comparison <= 0) return true;

            return Random.NextDouble() < Math.Exp(-Math.Abs(distanceFunction(best, current)) / temperatue);
        }
    }
}
