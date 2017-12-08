namespace Utility.Algorithms.BlackBoxOptimization.SimmulatedAnnealing
{
    /// <summary>
    /// Struct used to configure the simulated annealing algorithm.
    /// </summary>
    public struct SimulatedAnnealingConfig
    {
        /// <summary>
        /// Gets or sets the maximum amount of states to be accepted in a single chain.
        /// </summary>
        public int MaxAcceptedPerChain { get; set; }

        /// <summary>
        /// Gets or sets the maximum amoun of states to be generated in a single chain.
        /// </summary>
        public int MaxAttemptsPerChain { get; set; }

        /// <summary>
        /// Gets or sets the rate at which the temperature will drop in each chaim.
        /// </summary>
        public double TemperatureDropRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the temperature will change withing a chain.
        /// </summary>
        public bool ChangeTemperatureOnChain { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of chains that should be run without accepting a new state.
        /// </summary>
        public int MaxChainsWithoutChange { get; set; }
    }
}
