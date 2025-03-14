
using RepresentativesSet.Greedy;
using RepresentativesSet;
using RepresentativesSetTest.Base;
using StatisticsStorage.Accumulators;
using StatisticsStorage.Savers;
using BaseContract.Interfaces;

namespace BioAlgorithm.Services.HittingSet
{
    //--------------------------------------------------------------------------------------
    // class HittingSetAlgorithmRunner
    //--------------------------------------------------------------------------------------
    public class HittingSetAlgorithmRunner : EnumerateRepresentativesTestBase
    {
        private RepresentativesStatisticAccumulator _statisticAccumulator;

        private readonly IHittingSetAlgorithm hittingSetAlgorithm;
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmRunner(IHittingSetAlgorithm hittingSetAlgorithm, int pCardinality, int pLength, int bufferSize, int pMinimumValue = 1, int pForwardAdditive = 1)
            : base(pCardinality, pLength, pMinimumValue, pForwardAdditive)
        {
            this.hittingSetAlgorithm = hittingSetAlgorithm;

            _statisticAccumulator = new RepresentativesStatisticAccumulator(new RepresentativesSaver(), pLength, pCardinality, 1, bufferSize);

            hittingSetAlgorithm.StatisticAccumulator = _statisticAccumulator;
        }
        //--------------------------------------------------------------------------------------
        public async Task ExecuteAsync()
        {
            await _statisticAccumulator.DeleteAsync(hittingSetAlgorithm.AlgorithmName, _fSize , _fCardinality, 1);
            Execute();
        }
        //--------------------------------------------------------------------------------------
        protected override void ActAction(int[][] listOfSet)
        {
            hittingSetAlgorithm.Execute(listOfSet);

        }
        //--------------------------------------------------------------------------------------
        protected override void AssertAction()
        {
//            _statisticAccumulator.RemoveLastStatistic();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostAction()
        {
            _statisticAccumulator.SaveRemain();
        }
        //--------------------------------------------------------------------------------------
    }

}
