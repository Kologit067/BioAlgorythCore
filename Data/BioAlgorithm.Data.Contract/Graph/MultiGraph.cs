using BaseLibrary.Helpers;


namespace GraphLib
{
    //--------------------------------------------------------------------------------------
    // class MultiGraph
    //--------------------------------------------------------------------------------------
    public class MultiGraph
    {
        public List<MultiVertex> Vertices { get; private set; }
        public List<MultiEdge> Edges { get; private set; }
        public MultiGraph(string graphAsString) 
        {
            Edges = new List<MultiEdge>();
            int[][] sets = CollectionPresentation.StringToArray(graphAsString);
            Vertices = sets.SelectMany(l => l).Distinct().OrderBy(l => l).Select(l => new MultiVertex(l)).ToList();
            int count = Vertices.Select(l => l.Ind).Count();
            int max = Vertices.Select(l => l.Ind).Max();
            if (count != max+1)
            {
                throw new ArgumentException("Gap in element list");
            }
            foreach (int[] set in sets)
            {
                MultiEdge edge = new MultiEdge(set.OrderBy(i => i));
                Edges.Add(edge);
                foreach (int i in set)
                {
                    Vertices[i].Edges.Add(edge);
                }
            }
        }

        //--------------------------------------------------------------------------------------
        public int DefineType()
        {
            int graphType = 0;
            if (Vertices.Any(v => v.Edges.Count < 2))
                graphType |= 1;
            if (Vertices.Any(v => v.Edges.Count >= Edges.Count - 1))
                graphType |= 2;
            if (Edges.Any(e => e.VertexSet.Count < 2))
                graphType |= 4;
            if (Edges.Any(e => e.VertexSet.Count == Vertices.Count))
                graphType |= 8;
            var components = GetComponents();
            if (components.Count > 1)
                graphType |= 16;
            return graphType;
        }

        //--------------------------------------------------------------------------------------
        public List<List<int>> GetComponents()
        {
            List<List<int>> components = new List<List<int>>();
            List<int> currentVertexSet = Vertices.Select((v, ind) => ind).ToList();
            while (currentVertexSet.Count > 0)
            {
                List<int> component = new List<int>();
                Queue<int> vertexQueue = new Queue<int>();
                vertexQueue.Enqueue(currentVertexSet[0]);
                component.Add(0);
                while (vertexQueue.Count > 0)
                {
                    int curVertex = vertexQueue.Dequeue();  
                    foreach(var edge in Vertices[curVertex].Edges)
                        foreach(var v in edge.VertexSet)
                        {
                            if (!component.Contains(v) )
                            {
                                component.Add(v);
                                vertexQueue.Enqueue(v);
                            }
                        }
                }
                components.Add(component);

                foreach (var item in component)
                {
                    currentVertexSet.Remove(item);
                }
            }
            return components;
        }
        //--------------------------------------------------------------------------------------
        public string ShowEdges
        {
            get
            {
                return string.Join(';', Edges.Select(e => $" ({string.Join(',', e.VertexSet)}) "));
            }
        }
        //--------------------------------------------------------------------------------------
        public string ShowVertices
        {
            get
            {
                return string.Join(';', Vertices.Select(v => $" ({string.Join(',', v.Edges.Select(e => Edges.IndexOf(e)))}) "));
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
