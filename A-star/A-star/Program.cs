using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using GraphNode = Pathfinding.AStar<string, string>.Node;
using GraphEdge = Pathfinding.AStar<string, string>.EdgeData;
using MapNode = Pathfinding.AStar<Pathfinding.Vector, string>.Node;
using MapEdge = Pathfinding.AStar<Pathfinding.Vector, string>.EdgeData;

namespace Pathfinding
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphTest();
            //MapTest();
        }

        static void GraphTest()
        {
            Graphs.DirectedGraph<string, string> graph = new Graphs.DirectedGraph<string, string>();

            //Nodes
            graph.AddNode(new GraphNode("A"));
            graph.AddNode(new GraphNode("B"));
            graph.AddNode(new GraphNode("C"));
            graph.AddNode(new GraphNode("D"));
            graph.AddNode(new GraphNode("E"));
            graph.AddNode(new GraphNode("F"));

            //Edges
            graph.AddEdge(graph.GetNode("A"), graph.GetNode("B"), new GraphEdge("A -> B", 10));
            graph.AddEdge(graph.GetNode("A"), graph.GetNode("C"), new GraphEdge("A -> C", 2));
            graph.AddEdge(graph.GetNode("B"), graph.GetNode("E"), new GraphEdge("B -> E", 4));
            graph.AddEdge(graph.GetNode("B"), graph.GetNode("F"), new GraphEdge("B -> F", 1));
            graph.AddEdge(graph.GetNode("C"), graph.GetNode("D"), new GraphEdge("C -> D", 5));
            graph.AddEdge(graph.GetNode("D"), graph.GetNode("E"), new GraphEdge("D -> E", 6));
            graph.AddEdge(graph.GetNode("F"), graph.GetNode("A"), new GraphEdge("F -> A", 0));
            graph.AddEdge(graph.GetNode("F"), graph.GetNode("E"), new GraphEdge("F -> E", 4));

            GraphNode[] path = AStar<string, string>.FindPath((GraphNode)graph.GetNode("A"), (GraphNode)graph.GetNode("E"), graph, hGraphTest);

            foreach (GraphNode n in path)
            {
                Console.WriteLine(n);
            }

            Console.ReadKey();
        }

        static double hGraphTest(GraphNode n, GraphNode goal)
        {
            return Math.Ceiling(n.IntermediateCost / 3);
        }

        static double hMapTest(MapNode n, MapNode goal)
        {
            return n.data.DistanceTo(goal.data);
        }

        static void MapTest()
        {
            Graphs.DirectedGraph<Vector, string> graph = new Graphs.DirectedGraph<Vector, string>();

            GraphicsUnit gu = GraphicsUnit.Pixel;
            Vector start = new Vector(0, 0);
            Vector goal = new Vector(0, 0);

            string file = Directory.GetCurrentDirectory() + "\\test";

            Bitmap map = new Bitmap(file + ".png");
            int width = (int)map.GetBounds(ref gu).Width;
            int height = (int)map.GetBounds(ref gu).Height;

            //Build nodes
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = map.GetPixel(x, y);

                    if (c == Color.FromArgb(0, 255, 0)) // start
                    {
                        start = new Vector(x, y);
                        graph.AddNode(new MapNode(start));
                    }
                    else if (c == Color.FromArgb(255, 0, 0)) // goal
                    {
                        goal = new Vector(x, y);
                        graph.AddNode(new MapNode(goal));
                    }
                    else if (c == Color.FromArgb(255, 255, 255)) //blank
                    {
                        graph.AddNode(new MapNode(new Vector(x, y)));
                    }
                }

                Console.WriteLine($"N - x: {x}");
            }

            //Build edges
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector c = new Vector(x, y);
                    MapNode n = (MapNode)graph.GetNode(c);

                    if (n == null)
                        continue;

                    // x 
                    if (y + 1 < height)
                    {
                        var m = graph.GetNode(new Vector(x, y + 1));
                        if (m != null)
                            graph.AddEdge(n, m, new MapEdge(n + "->" + m, 1));
                    }

                    if (y - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x, y - 1));
                        if (m != null)
                            graph.AddEdge(n, m, new MapEdge(n + "->" + m, 1));
                    }

                    // x + 1
                    if (x + 1 < width)
                    {
                        var m = graph.GetNode(new Vector(x + 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, new MapEdge(n + "->" + m, 1));

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, new MapEdge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, new MapEdge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }
                    }

                    // x - 1
                    if (x - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x - 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, new MapEdge(n + "->" + m, 1));

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, new MapEdge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, new MapEdge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }
                    }
                }

                Console.WriteLine($"E - x: {x}");
            }

            MapNode[] path = AStar<Vector, string>.FindPath((MapNode)graph.GetNode(start), (MapNode)graph.GetNode(goal), graph, hMapTest);

            //Color path
            foreach (MapNode n in path)
            {
                if (!(n.data.x == start.x && n.data.y == start.y) &&
                    !(n.data.x == goal.x && n.data.y == goal.y))
                {
                    map.SetPixel((int)n.data.x, (int)n.data.y, Color.FromArgb(0, 0, 255));
                }
            }

            map.Save(file + "2.png");
            Console.WriteLine("DonnoDK");
            Console.ReadKey();
        }
    }


    struct Vector
    {
        public float x;
        public float y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public double DistanceTo(Vector v)
        {
            return Math.Sqrt(Math.Pow(x - v.x, 2) + Math.Pow(y - v.y, 2));
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        public override bool Equals(object obj)
        {
            Vector v = (Vector)obj;

            return v.x == x && v.y == y;
        }

    }
}
