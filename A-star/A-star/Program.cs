using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace A_star
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectedGraph<Vector, A_star.NodeData<Vector>, double> graph = new DirectedGraph<Vector, A_star.NodeData<Vector>, double>();
            //graph.AddNode(new Vector(1, 2), "This is 1-2");
            //graph.AddNode(new Vector(2, 3), "This is 2-3");
            //graph.AddEdge(graph.GetNode(new Vector(1,2)), graph.GetNode(new Vector(2, 3)), 1.2);
            //graph.AddEdge(graph.GetNode(new Vector(2, 3)), graph.GetNode(new Vector(1, 2)), 2.3);
            //Console.Write(graph.ToString());

            GraphicsUnit gu = GraphicsUnit.Pixel;
            Vector start = new Vector(0, 0);
            Vector goal = new Vector(0, 0);

            string file = Directory.GetCurrentDirectory() + "\\test";

            Bitmap map = new Bitmap(file + ".png");
            int width = (int)map.GetBounds(ref gu).Width;
            int height = (int)map.GetBounds(ref gu).Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = map.GetPixel(x, y);

                    if (c == Color.FromArgb(0,255,0)) // start
                    {
                        start = new Vector(x, y);
                        graph.AddNode(start, new A_star.NodeData<Vector>() { IntermediateCost = 0 });
                    }
                    else if (c == Color.FromArgb(255,0,0)) // goal
                    {
                        goal = new Vector(x, y);
                        graph.AddNode(goal, new A_star.NodeData<Vector>() { IntermediateCost = 0 });
                    }
                    else if (c == Color.FromArgb(255,255,255)) //blank
                    {
                        graph.AddNode(new Vector(x, y), new A_star.NodeData<Vector>() { IntermediateCost = 0 });
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector c = new Vector(x, y);
                    DirectedGraphNode<Vector, A_star.NodeData<Vector>> n = graph.GetNode(c);

                    if (n == null)
                        continue;

                    var p = graph.GetNode(new Vector(x, y));
                    if (p != null)
                        graph.AddEdge(n, p, 1);

                    // x 
                    if (y + 1 < height)
                    {
                        var m = graph.GetNode(new Vector(x, y + 1));
                        if (m != null)
                            graph.AddEdge(n, m, 1);
                    }

                    if (y - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x, y - 1));
                        if (m != null)
                            graph.AddEdge(n, m, 1);
                    }

                    // x + 1
                    if (x + 1 < width)
                    {
                        var m = graph.GetNode(new Vector(x + 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, 1);

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, 1);
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, 1);
                        }
                    }

                    // x - 1
                    if (x - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x - 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, 1);

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, 1);
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, 1);
                        }
                    }
                }
            }

            DirectedGraphNode<Vector, A_star.NodeData<Vector>>[] path = A_star.FindPath<Vector>(graph.GetNode(start), graph.GetNode(goal), graph, h);

            foreach (DirectedGraphNode<Vector, A_star.NodeData<Vector>> n in path)
            {
                if (n.UniqueIdentifier.x != start.x && n.UniqueIdentifier.y != start.y &&
                    n.UniqueIdentifier.x != goal.x && n.UniqueIdentifier.y != goal.y)
                {
                    map.SetPixel((int)n.UniqueIdentifier.x, (int)n.UniqueIdentifier.y, Color.FromArgb(0, 0, 255));
                }
            }

            map.Save(file + "2.png");

            Console.ReadKey();
        }

        static double h(DirectedGraphNode<Vector, A_star.NodeData<Vector>> n, DirectedGraphNode<Vector, A_star.NodeData<Vector>> goal)
        {
            return 1;
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

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}
