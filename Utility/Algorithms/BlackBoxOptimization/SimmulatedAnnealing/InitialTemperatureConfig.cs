namespace Utility.Algorithms.BlackBoxOptimization.SimmulatedAnnealing
{
    /// <summary>
    /// Struct for specifying the temperature configuration for a simulation.
    /// </summary>
    public struct InitialTemperatureConfig
    {
        /// <summary>
        /// Gets or sets the initial value of the temperature.
        /// </summary>
        public double Seed { get; set; }

        /// <summary>
        /// Gets or sets the ratio at which the temperature will grow during setup.
        /// </summary>
        public double GrowthRate { get; set; }

        /// <summary>
        /// Gets the minimum rate at which temperature should, initially, accepts any state.
        /// </summary>
        public decimal MinAcceptanceRate { get; set; }
    }
}
