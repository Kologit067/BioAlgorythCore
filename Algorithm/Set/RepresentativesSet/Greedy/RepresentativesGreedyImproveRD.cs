using System.Diagnostics;
using BaseContract.Interfaces;

namespace RepresentativesSet.Greedy
{
    public class RepresentativesGreedyImproveRD : RepresentativesGreedy, IHittingSetAlgorithm
    {
        public RepresentativesGreedyImproveRD() : base()
        {
        }
        public override void Execute(int[][] pListOfSet)
        {
            base.Execute(pListOfSet);

            stopwatch = new Stopwatch();
            stopwatch.Start();
            StatisticAccumulator.CreateStatistics(listOfSet.Select(l => l.ToArray()).ToArray(), _inputDataShort, AlgorithmName);
            while (listOfSet.Where(s => s.Count() > 0).Count() > 0)
            {
                // all elements with weight = max count
                int maxCount = elements.Max(e => e.Count);
                List<(List<int> e, int i)> maxList = elements.Select((e, i) => (e, i)).Where(s => s.e.Count() == maxCount).ToList();
                (List<int> e, int i) max = maxList.First();
                StatisticAccumulator.IterationCountInc();
                // if > 1 take NOT random - Min sum elements in all corresponding sets
                if (maxList.Count > 1)
                {
                    StatisticAccumulator.IterationCountInc();
                    max = maxList.OrderBy(m => RelationCountDistinct(listOfSet, m.i)).Last();
                }

                Solution.Add(max.i);
                var deletedSets = max.e.ToList();
                for (int i = 0; i < listOfSet.Count; i++)
                {
                    StatisticAccumulator.IterationCountInc();
                    if (deletedSets.Contains(i))
                        listOfSet[i].Clear();
                }
                for (int i = 0; i < elements.Length; i++)
                {
                    StatisticAccumulator.IterationCountInc();
                    deletedSets.ForEach(d => elements[i].Remove(d));
                }
            }
            StatisticAccumulator.UpdateOptcountInc();
            stopwatch.Stop();
            _fElapsedTicks = stopwatch.ElapsedTicks;
            _fDurationMilliSeconds = stopwatch.ElapsedMilliseconds;
            StatisticAccumulator.SaveStatisticData(ElapsedTicks, DurationMilliSeconds, DateTime.Now,
                false, SolutionAsString, new List<string> { SolutionAsString }, Solution.Count);
            Solution = Solution.OrderBy(s => s).ToList();
        }
    }
}
