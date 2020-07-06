using System.Collections.Generic;
using AwsomenautsCardGame.Maps;

namespace ExternalCode.AStar
{
    /// <summary>
    /// Contains the code for A* Pathfinding.
    /// </summary>
    public static class AStar
    {
		/// <summary>
		/// Resets the CardSocket in the list.
		/// </summary>
		/// <param name="nodes">Resets the nodes to be used again.</param>
		public static void ResetNodes(List<CardSocket> nodes)
        {
            foreach (CardSocket node in nodes)
            {
                node.CurrentCost = 0;
                node.EstimatedCost = 0;
                node.Parent = null;
                node.State = NodeState.UNCONSIDERED;
            }
        }

        /// <summary>
        /// Generates the Path from an (solved) node graph, before it gets reset.
        /// </summary>
        /// <param name="target">The node where we want to go.</param>
        /// <returns>The Path to the target node.</returns>
        public static List<CardSocket> GeneratePath(CardSocket target)
        {
            List<CardSocket> ret = new List<CardSocket>();
            CardSocket current = target;
            while (current != null)
            {
                ret.Add(current);
                current = current.Parent;
            }

            ret.Reverse();
            return ret;
        }

        /// <summary>
        /// Computes the path from => to.
        /// </summary>
        /// <param name="from">Start node.</param>
        /// <param name="to">end node.</param>
        /// <returns>Path from start to end.</returns>
        public static List<CardSocket> Compute(CardSocket from, CardSocket to)
        {
            List<CardSocket> done = new List<CardSocket>();

            // A priority queue that will sort our nodes based on the total cost estimate
            PriorityQueue<CardSocket> open = new PriorityQueue<CardSocket>();
            foreach (CardSocket node in from.ConnectedNodes)
            {
                // Add connecting nodes if traversable
                if (node.Traversable)
                {
                    // Calculate the Costs
                    node.CurrentCost = from.CurrentCost + from.DistanceTo(node) * node.TraversalCostMultiplier;
                    node.EstimatedCost = from.CurrentCost + node.DistanceTo(to);

                    // Enqueue
                    open.Enqueue(node);
                }
            }

            while (true)
            {
                // End Condition( Path not found )
                if (open.Count == 0)
                {
                    ResetNodes(done);
                    ResetNodes(open.GetData());
                    return new List<CardSocket>();
                }

				// Selecting next Element from queue
				CardSocket current = open.Dequeue();

                // Add it to the done list
                done.Add(current);

                current.State = NodeState.CLOSED;

                // EndCondition( Path was found )
                if (current == to)
                {
                    List<CardSocket> ret = GeneratePath(to); // Create the Path

                    // Reset all Nodes that were used.
                    ResetNodes(done);
                    ResetNodes(open.GetData());
                    return ret;
                }
                else
                {
                    AddOrUpdateConnected(current, to, open);
                }
            }
        }

        private static void AddOrUpdateConnected(CardSocket current, CardSocket to, PriorityQueue<CardSocket> queue)
        {
            foreach (CardSocket connected in current.ConnectedNodes)
            {
                if (!connected.Traversable ||
                    connected.State == NodeState.CLOSED)
                {
                    continue; // Do ignore already checked and not traversable nodes.
                }

                // Adds a previously not "seen" node into the Queue
                if (connected.State == NodeState.UNCONSIDERED)
                {
                    connected.Parent = current;
                    connected.CurrentCost = current.CurrentCost + current.DistanceTo(connected) * connected.TraversalCostMultiplier;
                    connected.EstimatedCost = connected.CurrentCost + connected.DistanceTo(to);
                    connected.State = NodeState.OPEN;
                    queue.Enqueue(connected);
                }
                else if (current != connected)
                {
                    // Updating the cost of the node if the current way is cheaper than the previous
                    double newCCost = current.CurrentCost + current.DistanceTo(connected);
                    double newTCost = newCCost + current.EstimatedCost;
                    if (newTCost < connected.TotalCost)
                    {
                        connected.Parent = current;
                        connected.CurrentCost = newCCost;
                    }
                }
                else
                {
                    // Codacy made me do it.
                    throw new PathfindingException("Detected the same node twice. Confusion how this could ever happen");
                }
            }
        }
    }
}
