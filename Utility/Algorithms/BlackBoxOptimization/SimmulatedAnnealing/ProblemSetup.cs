namespace Utility.Algorithms.BlackBoxOptimization.SimmulatedAnnealing
{
    using System;

    /// <summary>
    /// Struct used to configure a specific problem to be simulated.
    /// </summary>
    /// <typeparam name="TState">The type of state that the simulation will have.</typeparam>
    /// <typeparam name="TEvaluation">The type fo evaluation each state will have.</typeparam>
    public struct ProblemSetup<TState, TEvaluation>
    {
        /// <summary>
        /// Gets or sets the function that will be evaluated to rate a state.
        /// </summary>
        public Func<TState, TEvaluation> ObjectiveFunction { get; set; }

        /// <summary>
        /// Gets or sets the function that generates a new, similar state, for a given state.
        /// </summary>
        public Func<TState, TState> NeighborFunction { get; set; }

        /// <summary>
        /// Gets or sets a function that, during every step of the simulation, will receive the current result of it and
        /// should display the status of the simulation.
        /// </summary>
        public Action<Result<TState, TEvaluation>> OutputFunction { get; set; }

        /// <summary>
        /// Gets or sets a function that, given two evaluations, return the ration between them (e.g. x/y)
        /// </summary>
        public Func<TEvaluation, TEvaluation, double> RatioFunction { get; set; }

        /// <summary>
        /// Gets or sets the function that will be called to determine the distance between two evaluations (e.g. x-y)
        /// </summary>
        public Func<TEvaluation, TEvaluation, double> DistanceFunction { get; set; }

        /// <summary>
        /// Gets or sets the initial state of the simulation.
        /// </summary>
        public TState InitialState { get; set; }
    }
}
