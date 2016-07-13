using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_star
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="NID">Node identifier - A unique identifier for each node.</typeparam>
    /// <typeparam name="NData">Node data - Data to be stored on each node.</typeparam>
    /// <typeparam name="EData">Edge data - Data to be stored on each edge.</typeparam>
    class DirectedGraph<NID, NData, EData>
    {
        Dictionary<NID, DirectedGraphNode<NID, NData>> nodes = new Dictionary<NID, DirectedGraphNode<NID, NData>>();
        Dictionary<DirectedGraphNode<NID, NData>, Dictionary<DirectedGraphNode<NID, NData>, EData>> edges = new Dictionary<DirectedGraphNode<NID, NData>, Dictionary<DirectedGraphNode<NID, NData>, EData>>();

        public DirectedGraphNode<NID, NData> GetNode(NID id)
        {
            if (nodes.ContainsKey(id))
            {
                return nodes[id];
            }

            throw new ArgumentException("Node does not exist");
        }

        public EData GetEdge(DirectedGraphNode<NID, NData> from, DirectedGraphNode<NID, NData> to)
        {
            if (edges.ContainsKey(from) && edges[from].ContainsKey(to))
            {
                return edges[from][to];
            }

            throw new ArgumentException("Edge does not exist");
        }

        public Dictionary<DirectedGraphNode<NID, NData>, EData> GetEdges(DirectedGraphNode<NID, NData> from)
        {
            if (edges.ContainsKey(from))
            {
                return edges[from];
            }

            return null;
        }

        public void AddNode(NID id, NData data)
        {
            if (!nodes.ContainsKey(id))
            {
                nodes.Add(id, new DirectedGraphNode<NID, NData>(id, data));
            }
        }

        public void AddEdge(DirectedGraphNode<NID, NData> from, DirectedGraphNode<NID, NData> to, EData data)
        {
            if (!edges.ContainsKey(from))
            {
                edges.Add(from, new Dictionary<DirectedGraphNode<NID, NData>, EData>());
            }

            if (!edges[from].ContainsKey(to))
            {
                edges[from].Add(to, data);
            }
        }

        public override string ToString()
        {
            string s = "Printing nodes:\n";

            foreach (NID k in nodes.Keys)
            {
                s += "Key=" + k.ToString() + ", Value=" + nodes[k].ToString() + "\n";
            }

            s += "\n--------------------------------------------\nPrinting edges:\n";
            foreach (DirectedGraphNode<NID, NData> from in edges.Keys)
            {
                s += "from " + from.ToString() + ":\n";

                foreach (DirectedGraphNode<NID, NData> to in edges[from].Keys)
                {
                    s += "to: Key=" + to.ToString() + ", Value=" + edges[from][to] + "\n";
                }

                s += "\n";
            }

            return s;
        }
    }

    abstract class DirectedGraphNode<NID, NData>
    {
        public NID UniqueIdentifier;
        public NData Data;

        public DirectedGraphNode(NID uniqueIdentifier, NData data)
        {
            this.UniqueIdentifier = uniqueIdentifier;
            this.Data = data;
        }

        public override string ToString()
        {
            return "Node: " + UniqueIdentifier.ToString() + "|" + Data.ToString();
        }
    }
}
