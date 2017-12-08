namespace Utility.Algorithms.BlackBoxOptimization.SimmulatedAnnealing
{
    /// <summary>
    /// Struct that reports the result of a simulation
    /// </summary>
    /// <typeparam name="TState">The type of state of the simulation.</typeparam>
    /// <typeparam name="TEvaluation">The type of evaluation of each state.</typeparam>
    public struct Result<TState, TEvaluation>
    {
        /// <summary>
        /// Gets or sets the best found state.
        /// </summary>
        public TState BestState { get; set; }

        /// <summary>
        /// Gets or sets the evaluation of the best state.
        /// </summary>
        public TEvaluation BestEvaluation { get; set; }

        /// <summary>
        /// Gets or sets the final state that was generated.
        /// </summary>
        public TState FinalState { get; set; }

        /// <summary>
        /// Gets or sets the evaluation of the last generated state.
        /// </summary>
        public TEvaluation FinalEvaluation { get; set; }

        /// <summary>
        /// Gets or sets the amount of states that were generated.
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Gets or sets the temperature at which the simulation ended.
        /// </summary>
        public double FinalTemperature { get; set; }
    }
}
