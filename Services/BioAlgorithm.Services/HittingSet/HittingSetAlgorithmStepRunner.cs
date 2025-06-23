using BaseLibrary.Helpers;
using StatisticsStorage.Accumulators;
using StatisticsStorage.Savers;
using System.Numerics;
using RepresentativesSet.Test;
using BaseContract.Interfaces;

namespace BioAlgorithm.Services.HittingSet
{

    public class HittingSetAlgorithmStepRunner : EnumerateRepresentativesWithStepTestBase
    {
        protected List<RepresentativesStatisticAccumulator> _statisticAccumulators;
        protected readonly List<IHittingSetAlgorithm> hittingSetAlgorithms;
        protected readonly Combinatorics combinatorics;

        protected BigInteger number;
        //--------------------------------------------------------------------------------------
        protected BigInteger _maxCount;
        public BigInteger MaxCount
        {
            get
            {
                return _maxCount;
            }
        }
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
        public HittingSetAlgorithmStepRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, string calculationStep, 
            string combinationType, int pCardinality, int pLength, long maxCount, int bufferSize, int pMinimumValue = 1, 
            int pForwardAdditive = 1, bool isSave = true )
        {
            this.hittingSetAlgorithms = hittingSetAlgorithms;

            _fLimit = (1 << pCardinality) - 1;
            _fSize = pLength;
            _fCardinality = pCardinality;
            _maxCount = maxCount;
            _isSave = isSave;
            number = Combinatorics.BigIntegerCombination(_fLimit, _fSize);
            _step = BigInteger.Divide(number, _maxCount);

            this.hittingSetAlgorithms = hittingSetAlgorithms;
            _statisticAccumulators = new List<RepresentativesStatisticAccumulator> ();
            if (_isSave)
            {
                foreach (var algorithm in this.hittingSetAlgorithms)
                {
                    IRepresentativesSaver saver = null;
                    if (algorithm.AlgorithmName == "Empty")
                    {
                        saver = new RepresentativesInputSaver();
                    }
                    else
                    {
                        saver = new RepresentativesSaver();
                    }

                    var statisticAccumulator = new RepresentativesStatisticAccumulator(saver, pLength, pCardinality, (decimal)_maxCount, (decimal)_step, bufferSize);

                    algorithm.StatisticAccumulator = statisticAccumulator;
                    _statisticAccumulators.Add(statisticAccumulator);
                }
            }

            combinatorics = new Combinatorics(calculationStep, combinationType, _fLimit, _fSize);
        }
        //--------------------------------------------------------------------------------------
        public async Task<bool> ExecuteAsync()
        {
            foreach (var algorithm in this.hittingSetAlgorithms)
                await algorithm.StatisticAccumulator.DeleteAsync(algorithm.AlgorithmName, _fSize, _fCardinality, (decimal)_maxCount);
            int? startn = null;
            int? startm = null;
            for (BigInteger counter = _step; counter < number; counter += _step)
            {
                _fCurrentSet = combinatorics.SkipEnumerationBigInteger(_fLimit, _fSize, counter, startn, startm);
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
        protected virtual void ActAction(int[][] listOfSet)
        {
            foreach (var algorithm in this.hittingSetAlgorithms)
                algorithm.Execute(listOfSet);
        }
        //--------------------------------------------------------------------------------------
        protected virtual void AssertAction()
        {

        }
        //--------------------------------------------------------------------------------------
        protected void PostAction()
        {
            foreach (var algorithm in this.hittingSetAlgorithms)
                algorithm.StatisticAccumulator.SaveRemain();
        }
        //--------------------------------------------------------------------------------------
    }

}
