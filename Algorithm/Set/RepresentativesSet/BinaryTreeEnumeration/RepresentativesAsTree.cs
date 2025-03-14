using BaseContract.Interfaces;
using BaseLibrary;
using StatisticsStorage.Accumulators;

namespace RepresentativesSet
{
    //--------------------------------------------------------------------------------------
    // class RepresentativesAsTree
    //--------------------------------------------------------------------------------------
    public class RepresentativesAsTree : EnumerateReverseBinVectors, IHittingSetAlgorithm
    {
        protected int[][] listOfSet;
        protected long[] listOfSetAsNumber;
        protected int currentMinimum;
        protected List<int> _fCurrentOptimalSet;		    // текущий оптимальный набор элементов
        protected List<string> _fOptimalSets;               // 
        public int CurrentMinimum
        { 
            get 
            { 
                return currentMinimum; 
            } 
        }
        protected string _inputData;
        public string InputData
        {
            get
            {
                return _inputData;
            }
        }
        protected string _inputDataShort;
        public string InputDataShort
        {
            get
            {
                return _inputDataShort;
            }
        }
        public IRepresentativesStatisticAccumulator StatisticAccumulator { get; set; }
        public RepresentativesAsTree(int pLength) : base(pLength)
        {
            StatisticAccumulator = new FakeRepresentativesStatisticAccumulator();
        }
        //-----------------------------------------------------------------------------------
        protected override void SupplementInitial()
        {
            StatisticAccumulator.CreateStatistics(listOfSet, _inputDataShort, AlgorithmName);
        }
        //-----------------------------------------------------------------------------------
        public virtual void Execute(int[][] pListOfSet)
        {
            listOfSet = pListOfSet;
            listOfSetAsNumber = listOfSet.Select(s => BruteForceRepresentativesBinaryNumbers.ElementNumbersToLongAsBinaryVector(s)).ToArray();
            if (listOfSet.Any(s => s.Any(e => e >= _fSize)))
                throw new ArgumentException("Element of set can not be > Length.");
            _fCurrentOptimalSet = _fCurrentSet.ToList();
            currentMinimum = _fSize;
            _fOptimalSets = new List<string>();
            _inputData = (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSet));
            _inputDataShort = (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSetAsNumber));
            Execute();
        }
        //--------------------------------------------------------------------------------------
        protected bool IsIntersect()
        {
            bool isIntersect = true;
            for (int k = 0; k < listOfSet.Length; k++)
            {
                if (!listOfSet[k].Any(s => _fCurrentSet[s] > 0))
                {
                    isIntersect = false;
                    break;
                }
            }
            return isIntersect;
        }
        //--------------------------------------------------------------------------------------
        protected void UpdateOptimalResults(int candidatValue)
        {
            if (candidatValue < currentMinimum)
            {
                StatisticAccumulator.UpdateOptcountInc();
                for (int i = 0; i < _fCurrentSet.Count; i++)
                {
                    _fCurrentOptimalSet[i] = _fCurrentSet[i];
                }
                if (_fCurrentPosition < _fCurrentSet.Count - 1)
                {
                    for (int i = _fCurrentPosition + 1; i < _fCurrentSet.Count; i++)
                    {
                        _fCurrentOptimalSet[i] = 0;
                    }
                }
                currentMinimum = candidatValue;
                _fOptimalSets.Clear();
            }
            List<int> result = new List<int>();
            for (int i = 0; i < _fCurrentSet.Count; i++)
            {
                if (_fCurrentSet[i] == 1)
                    result.Add(i);
            }
            _fOptimalSets.Add(string.Join(",", result));
        }
        //--------------------------------------------------------------------------------------
        protected override void IterationAction()
        {
            StatisticAccumulator.IterationCountInc();
        }
        protected override void TerminalAction()
        {
            StatisticAccumulator.TerminalCountInc();
        }
        //-----------------------------------------------------------------------------------
        protected override void PostAction()
        {
            StatisticAccumulator.SaveStatisticData(ElapsedTicks, DurationMilliSeconds, DateTime.Now,
                IsComplete, CurrentSetAsString, _fOptimalSets, currentMinimum);
        }
        //--------------------------------------------------------------------------------------
    }
}
