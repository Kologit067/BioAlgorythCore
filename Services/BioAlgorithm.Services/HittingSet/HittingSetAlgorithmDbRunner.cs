using BaseContract.Interfaces;
using BaseLibrary.Helpers;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using StatisticsStorage.Accumulators;
using StatisticsStorage.Savers;

namespace BioAlgorithm.Services.HittingSet
{
    public class HittingSetAlgorithmDbRunner 
    {
        protected List<RepresentativesStatisticAccumulator> _statisticAccumulators;
        protected readonly List<IHittingSetAlgorithm> hittingSetAlgorithms;
        protected readonly List<InputDataSlim> _inputDataList;
//        protected int _fLimit;
        protected int _fSize;
        protected int _fCardinality;
//        protected int[] _fCurrentSet;
        protected decimal _maxCount;
        protected bool _isSave;
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmDbRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, List<InputDataSlim> inputDataList, int pCardinality, int pLength, decimal maxCount,
            int bufferSize, int pMinimumValue = 1, int pForwardAdditive = 1, bool isSave = true)
        {
            this.hittingSetAlgorithms = hittingSetAlgorithms;

 //           _fLimit = (1 << pCardinality) - 1;
            _fSize = pLength;
            _fCardinality = pCardinality;
            _maxCount = maxCount;
            _isSave = isSave;
            _inputDataList = inputDataList;

            this.hittingSetAlgorithms = hittingSetAlgorithms;
            _statisticAccumulators = new List<RepresentativesStatisticAccumulator>();
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

                    var statisticAccumulator = new RepresentativesStatisticAccumulator(saver, pLength, pCardinality, (decimal)_maxCount, (decimal)1, bufferSize);

                    algorithm.StatisticAccumulator = statisticAccumulator;
                    _statisticAccumulators.Add(statisticAccumulator);
                }
            }

        }
        //--------------------------------------------------------------------------------------
        public async Task<bool> ExecuteAsync()
        {
            foreach (var algorithm in this.hittingSetAlgorithms)
                await algorithm.StatisticAccumulator.DeleteAsync(algorithm.AlgorithmName, _fSize, _fCardinality, (decimal)_maxCount);
            foreach (InputDataSlim inputData in _inputDataList)
            {
                int[][] listOfSet = CollectionPresentation.StringToArray(inputData.InputData);
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
