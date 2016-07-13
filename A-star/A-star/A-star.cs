using System;
using System.Collections.Generic;
using Heap;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_star
{
    class A_star
    {
        static public Node<NID, NodeData<NID>>[] FindPath<NID>(Node<NID, NodeData<NID>> start, Node<NID, NodeData<NID>> goal, DirectedGraph<NID, NodeData<NID>, double> graph, Func<Node<NID, NodeData<NID>>, Node<NID, NodeData<NID>>, double> heuristic)
        {
            List<Node<NID, NodeData<NID>>> visited = new List<Node<NID, NodeData<NID>>>();
            Heap.Heap frontier = new Heap.Heap(new List<Node<NID, NodeData<NID>>>(), Heap.Heap.HeapProperty.MinHeap);

            Node<NID, NodeData<NID>> s = (Node<NID, NodeData<NID>>)start;
            s.Data.IntermediateCost = 0;
            s.Data.Parent = null;
            frontier.HeapInsert(s);

            while (frontier.Count > 0)
            {
                Node<NID, NodeData<NID>> current = (Node<NID, NodeData<NID>>)frontier.HeapExtractRoot();

                if (current == goal)
                {
                    return ReconstructPath(current);
                }

                visited.Add(current);
                foreach (var edgeTo in graph.GetEdges(current))
                {
                    Node<NID, NodeData<NID>> to = (Node<NID, NodeData<NID>>)edgeTo.Key;

                    if (visited.Contains(to))
                    {
                        continue;
                    }

                    to.Data.IntermediateCost = edgeTo.Value.Data + heuristic(to, goal);
                    to.Data.Parent = current;
                    if (!frontier.Contains(to))
                    {
                        frontier.HeapInsert(to);
                    }
                }
            }

            return null;
        }

        static private Node<NID, NodeData<NID>>[] ReconstructPath<NID>(Node<NID, NodeData<NID>> end)
        {
            Stack<Node<NID, NodeData<NID>>> path = new Stack<Node<NID, NodeData<NID>>>();

            Node<NID, NodeData<NID>> current = end;
            while (current.Data.Parent != null)
            {
                path.Push(current);
                current = current.Data.Parent;
            }

            return path.ToArray();
        }

        public class NodeData<NID>
        {
            public Node<NID, NodeData<NID>> Parent;
            public double IntermediateCost;
        }

        public class Node<NID, NData> : DirectedGraphNode<NID, NData>, HeapNode
        {
            public Node(NID uniqueIdentifier, NData data) : base(uniqueIdentifier, data)
            {

            }

            public double key
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "Node: " + UniqueIdentifier.ToString() + "|" + Data.ToString();
            }
        }
    }
}
