using BioAlgorithmModel.BipartiteGraphModel;
using BaseLibrary;

namespace IsomorphismGraph
{
    //--------------------------------------------------------------------------------------
    // class IsomorphismBipartGraph
    //--------------------------------------------------------------------------------------
    public class IsomorphismBipartGraph : EnumerateSetOnPosition<int, int>
    {
        protected int _fSize;
        private BipartiteGraph _graph1;
        private BipartiteGraph _graph2;
        List<List<int>> leftReferences;
        List<List<int>> rightReferences;
        private bool isSatisfied = true;
        private readonly int leftCount;
        //--------------------------------------------------------------------------------------
        public IsomorphismBipartGraph(BipartiteGraph graph1, BipartiteGraph graph2) : base(graph1.LeftSet.Count + graph1.RightSet.Count)
        {
            _fBreakElement = -1;
            _fSize = graph1.LeftSet.Count + graph1.RightSet.Count;
            _graph1 = graph1;
            _graph2 = graph2;
            leftCount = graph1.LeftSet.Count;
            if (graph1.LeftSet.Count != graph2.LeftSet.Count)
                return;
            if (graph1.RightSet.Count != graph2.RightSet.Count)
                return;
            //Dictionary<int, List<int>> weightLeftGroup1 = _graph1.LeftSet.GroupBy(v => v.AdjacentVertices.Count).OrderBy(g => g.Key).
            //    ToDictionary(g => g.Key, g => g.OrderBy(v => v.Number).Select(v => v.Number).ToList());
            //Dictionary<int, List<int>> weightLeftGroup2 = _graph2.LeftSet.GroupBy(v => v.AdjacentVertices.Count).OrderBy(g => g.Key).
            //    ToDictionary(g => g.Key, g => g.OrderBy(v => v.Number).Select(v => v.Number).ToList());

            //Dictionary<int, List<int>> weightRihtGroup1 = _graph1.RightSet.GroupBy(v => v.AdjacentVertices.Count).OrderBy(g => g.Key).
            //    ToDictionary(g => g.Key, g => g.OrderBy(v => v.Number).Select(v => v.Number).ToList());
            //Dictionary<int, List<int>> weightRightGroup2 = _graph2.RightSet.GroupBy(v => v.AdjacentVertices.Count).OrderBy(g => g.Key).
            //    ToDictionary(g => g.Key, g => g.OrderBy(v => v.Number).Select(v => v.Number).ToList());

            Dictionary<int, List<int>> weightLeftGroup1 = GroupVertexByCount(_graph1.LeftSet);
            Dictionary<int, List<int>> weightLeftGroup2 = GroupVertexByCount(_graph2.LeftSet);

            Dictionary<int, List<int>> weightRightGroup1 = GroupVertexByCount(_graph1.RightSet);
            Dictionary<int, List<int>> weightRightGroup2 = GroupVertexByCount(_graph2.RightSet);

            List<int> keysLeft1 = weightLeftGroup1.Keys.ToList();
            List<int> keysLeft2 = weightLeftGroup2.Keys.ToList();
            List<int> keysRight1 = weightRightGroup1.Keys.ToList();
            List<int> keysRight2 = weightRightGroup2.Keys.ToList();

            if (keysLeft1.Count != keysLeft2.Count)
                return;
            if (keysRight1.Count != keysRight2.Count)
                return;

            foreach (int key in keysLeft1)
            {
                if (!weightLeftGroup2.ContainsKey(key))
                    return;
                if (weightLeftGroup1[key].Count != weightLeftGroup2[key].Count)
                    return;
            }
            foreach (int key in keysRight1)
            {
                if (!weightRightGroup2.ContainsKey(key))
                    return;
                if (weightRightGroup1[key].Count != weightRightGroup2[key].Count)
                    return;
            }

            leftReferences = _graph1.LeftSet.Select(v => new List<int>()).ToList();
            for (int i = 0; i < leftReferences.Count; i++)
            {
                int weight = _graph1.LeftSet[i].AdjacentVertices.Count;
                leftReferences[i].AddRange(weightLeftGroup2[weight]);
            }
            rightReferences = _graph1.RightSet.Select(v => new List<int>()).ToList();
            for (int i = 0; i < rightReferences.Count; i++)
            {
                int weight = _graph1.RightSet[i].AdjacentVertices.Count;
                rightReferences[i].AddRange(weightRightGroup2[weight]);
            }

            Parallel.For(0, _fCurrentSet.Count, i => _fCurrentSet[i] = -1);

        }
        //--------------------------------------------------------------------------------------
        private Dictionary<int, List<int>> GroupVertexByCount(List<BipartiteVertex> vertexList) => vertexList.GroupBy(v => v.AdjacentVertices.Count).OrderBy(g => g.Key).
                ToDictionary(g => g.Key, g => g.OrderBy(v => v.Number).Select(v => v.Number).ToList());
        
        public bool IsIsomorphic()
        {
            if (leftReferences == null || rightReferences == null)
                return false;
            Execute();
            return isSatisfied;

        }

        //--------------------------------------------------------------------------------------
        protected override void AddAction(int p)
        {
            if (_fCurrentPosition >= leftCount)
            {
                int vertexIndex2 = GetCorrespondingVertex(_fCurrentPosition);
                List<int> adjacentVertices = _graph1.RightSet[_fCurrentPosition - leftCount].AdjacentVertices;
                foreach (int vertexAdjIndexInSet1 in adjacentVertices)
                {
                    if (vertexAdjIndexInSet1 >= _fCurrentPosition)
                    {
                        throw new InvalidOperationException("Logical Error. Adjacent Vertex in right set must be from left set");
                    }
                    int vertexAdjIndex2 = GetCorrespondingVertex(vertexAdjIndexInSet1);
                    if (!_graph2.RightSet[vertexIndex2].AdjacentVertices.Contains(vertexAdjIndex2))
                    {
                        isSatisfied = false;
                        return;
                    }
                }
                isSatisfied = true;
            }
        }

        //--------------------------------------------------------------------------------------
        protected override void BackAction()
        {

        }

        //--------------------------------------------------------------------------------------
        protected override int FirstElement(int pPosition)
        {
            int element = FindNextFreeElement(0, pPosition);
            if (element < 0)
                throw new Exception("FirstElement logic error");
            return element;
        }

        //--------------------------------------------------------------------------------------
        protected override bool NextElement(int pPosition)
        {
            int element = FindNextFreeElement(_fCurrentSet[pPosition] + 1, pPosition);
            if (element < 0)
                return false;
            _fCurrentSet[pPosition] = element;
            string show = ShowFullString;
            return true;
        }
        //--------------------------------------------------------------------------------------
        protected int FindNextFreeElement(int start, int pPosition)
        {
            List<List<int>> references;
            int startInSet = 0;
            if (pPosition < leftCount)
            {
                references = leftReferences;
            }
            else
            {
                references = rightReferences;
                startInSet = leftCount;
            }
            for (int i = start; i < references[pPosition - startInSet].Count; i++)
            {
                bool isIncluded = false;
                for (int j = startInSet; j < pPosition; j++)
                {
                    if (references[j - startInSet][_fCurrentSet[j]] == references[pPosition - startInSet][i])
                    {
                        isIncluded = true;
                        break;
                    }
                }
                if (!isIncluded)
                {
                    isSatisfied = true;
                    return i;
                }
                //                return references[pPosition][i];
            }
            return -1;
        }
        //--------------------------------------------------------------------------------------
        protected override void ForwardAction()
        {

        }

        //--------------------------------------------------------------------------------------
        protected override int InitialElement()
        {
            return FirstElement(0);
        }

        //--------------------------------------------------------------------------------------
        protected override bool IsCompleteCondition()
        {
            IterationAction();
            if (_fCurrentPosition >= _fSize - 1 || !isSatisfied)
            {
                TerminalAction();
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------------
        protected override bool MakeAction()
        {
            if (_fCurrentPosition == _fSize - 1 && isSatisfied)
            {
                return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        protected override void PostAction()
        {

        }

        //--------------------------------------------------------------------------------------
        protected override void RemoveAction(int p)
        {

        }

        //--------------------------------------------------------------------------------------
        protected override void SupplementInitial()
        {

        }
        private int GetCorrespondingVertex(int ind)
        {
            int vertexIndexInSet2 = _fCurrentSet[ind];
            if (ind < leftCount)
                return leftReferences[ind][vertexIndexInSet2];
            else
                return rightReferences[ind - leftCount][vertexIndexInSet2];
        }
        //--------------------------------------------------------------------------------------
        public override string? ShowFullString
        {
            get
            {
                if (leftReferences == null || rightReferences == null)
                    return null;
                if (_fCurrentSet != null && _fCurrentSet.Count > 0)
                {
                    string leftPart = string.Join(",", _fCurrentSet.Select((i, ind) => (i ,ind)).Where(o => o.ind < leftCount)
                        .Select( s => s.i >= 0 ? leftReferences[s.ind][s.i].ToString() : "_")
                    );
                    string rightPart = string.Join(",", _fCurrentSet.Select((i, ind) => (i, ind)).Where(o => o.ind >= leftCount)
                        .Select(s => s.i >= 0 ? rightReferences[s.ind - leftCount][s.i].ToString() : "_")
                    );
                    return $"Left: {leftPart}. Right: {rightPart}";
                }
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------

}
