namespace Utility.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    /// Static class that provides an implementation of Dijkstra's path-finding algorithm.
    /// </summary>
    public static class Dijkstra
    {
        /// <summary>
        /// Class representing a graph which will be used for searches.
        /// </summary>
        public class Graph
        {
            internal Dictionary<int, Dictionary<int, int>> Vertices = new Dictionary<int, Dictionary<int, int>>();

            /// <summary>
            /// Adds a vertex to the graph.
            /// </summary>
            /// <param name="id">The unique ID that represents the vertex.</param>
            /// <param name="edges">The edges that go out of the vertex. The key is the target node, and the value is
            /// the cost of the edge.</param>
            public void AddVertex(int id, Dictionary<int, int> edges)
            {
                Vertices[id] = edges;
            }
        }

        /// <summary>
        /// Searches for the shortest path between two vertices.
        /// </summary>
        /// <param name="graph">The graph that will be searched on.</param>
        /// <param name="start">The ID of the start vertex.</param>
        /// <param name="finish">The ID of the target vertex.</param>
        /// <returns>An array containing, in order, the path that has to be followed, not including the source, or null
        /// if there is no valid path.</returns>
        public static int[] ShortestPath(Graph graph, int start, int finish)
        {
            var previous = new Dictionary<int, int>();
            var distances = new Dictionary<int, int>();
            var nodes = new SortedSet<int>(new FuncComparer<int>((a,b) => distances[a] - distances[b]));
            List<int> path = null;

            foreach (var vertex in graph.Vertices)
            {
                if (vertex.Key == start)
                    distances[vertex.Key] = 0;
                else
                    distances[vertex.Key] = int.MaxValue;

                nodes.Add(vertex.Key);
            }

            while (nodes.Count > 0)
            {
                var smallest = nodes.Min;
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<int>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }
                    break;
                }

                if (distances[smallest] == int.MaxValue)
                    break;

                foreach (var neighbor in graph.Vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        if (nodes.Contains(neighbor.Key))
                        {
                            nodes.Remove(neighbor.Key);
                            nodes.Add(neighbor.Key);
                        }
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path?.ToArray();
        }

        /// <summary>
        /// Calculates the cost of following a certain path.
        /// </summary>
        /// <param name="graph">The graph in which the path takes place.</param>
        /// <param name="start">The start vertex of the path.</param>
        /// <param name="path">The vertices, in order, which have to be visited.</param>
        /// <returns>The total cost of following the path.</returns>
        public static int CostOfPath(Graph graph, int start, int[] path)
        {
            var total = 0;
            var curr = start;
            foreach (var node in path)
            {
                total += graph.Vertices[curr][node];
                curr = node;
            }
            return total;
        }
    }
}
