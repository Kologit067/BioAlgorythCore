
using BaseLibrary.Helpers;
using RepresentativesSet.Greedy;
using RepresentativesSet;
using StatisticsStorage.Accumulators;
using StatisticsStorage.Savers;
using System.Numerics;
using RepresentativesSet.Test;
using BaseContract.Interfaces;

namespace BioAlgorithm.Services.HittingSet
{

    public class HittingSetAlgorithmStepRunner : EnumerateRepresentativesWithStepTestBase
    {
        private RepresentativesStatisticAccumulator _statisticAccumulator;
        private readonly IHittingSetAlgorithm hittingSetAlgorithm;

        protected BigInteger _maxCount;
        protected BigInteger number;
        //--------------------------------------------------------------------------------------
        protected BigInteger _step;
        public BigInteger Step
        {
            get
            {
                return _step;
            }
        }
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmStepRunner(IHittingSetAlgorithm hittingSetAlgorithm, int pCardinality, int pLength, long maxCount, int bufferSize, int pMinimumValue = 1, int pForwardAdditive = 1)
        {
            this.hittingSetAlgorithm = hittingSetAlgorithm;

            _fLimit = (1 << pCardinality) - 1;
            _fSize = pLength;
            _fCardinality = pCardinality;
            _maxCount = maxCount;
            number = Combinatorics.BigIntegerCombination(_fLimit, _fSize);
            _step = BigInteger.Divide(number, _maxCount);

            _statisticAccumulator = new RepresentativesStatisticAccumulator(new RepresentativesSaver(), pLength, pCardinality, (decimal)_step, bufferSize);

            hittingSetAlgorithm.StatisticAccumulator = _statisticAccumulator;
            Combinatorics.SetCombinationBigIntegerMatrix(_fLimit, _fSize);
            Combinatorics.CreateCountForPositionMatrix(_fLimit, _fSize);
        }
        //--------------------------------------------------------------------------------------
        public async Task<bool> ExecuteAsync()
        {
            await _statisticAccumulator.DeleteAsync(hittingSetAlgorithm.AlgorithmName, _fSize, _fCardinality, (decimal)_step);
            int? startn = null;
            int? startm = null;
            for (BigInteger counter = _step; counter < number; counter += _step)
            {
                _fCurrentSet = Combinatorics.SkipEnumerationSaveFPImpBigInteger(_fLimit, _fSize, counter, startn, startm);
                (startn, startm) = _fCurrentSet.Select((f, ind) => (f, ind)).FirstOrDefault(a => a.f > a.ind + 1);
                if (startm.HasValue)
                    startm += 1;
                int[][] listOfSet = GetAndTestListOfSet();
                if (listOfSet != null)
                {
                    // act
                    ActAction(listOfSet);
                    // assert
                    AssertAction();
                }
            }
            PostAction();
            return false;
        }
        //--------------------------------------------------------------------------------------
        protected void ActAction(int[][] listOfSet)
        {
            hittingSetAlgorithm.Execute(listOfSet);
        }
        //--------------------------------------------------------------------------------------
        protected void AssertAction()
        {

        }
        //--------------------------------------------------------------------------------------
        protected void PostAction()
        {
            _statisticAccumulator.SaveRemain();
        }
        //--------------------------------------------------------------------------------------
    }

}
