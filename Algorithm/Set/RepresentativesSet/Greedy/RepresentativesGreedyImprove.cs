using System.Diagnostics;
using BaseContract.Interfaces;

namespace RepresentativesSet.Greedy
{
    public class RepresentativesGreedyImprove : RepresentativesGreedy, IHittingSetAlgorithm
    {
        public RepresentativesGreedyImprove() : base()
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
                int maxCount = elements.Max(e => e.Count);
                // all elements with weight = max count
                List<(List<int> e, int i)> maxList = elements.Select((e, i) => (e, i)).Where(s => s.e.Count() == maxCount).ToList();
                (List<int> e, int i) max = maxList.First();
                StatisticAccumulator.IterationCountInc();
                // if > 1 take NOT random - Min sum elements in all corresponding sets
                if (maxList.Count > 1)
                {
                    StatisticAccumulator.IterationCountInc();
                    max = maxList.OrderBy(m => m.e.Sum(k => listOfSet[k].Count())).First();
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
        }
    }

}
