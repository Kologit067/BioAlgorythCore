using BaseContract.Interfaces;
using BaseLibrary;
using StatisticsStorage.Accumulators;

namespace RepresentativesSet.BinaryTreeEnumeration
{
    //--------------------------------------------------------------------------------------
    public class BruteForceRepresentativesAsTreeDirect : EnumerateBinVectors, IHittingSetAlgorithm
    {
        private int[][] listOfSet;
        private int currentMinimum;
        protected long[] listOfSetAsNumber;
        protected List<int> _fCurrentOptimalSet;		    // текущий оптимальный набор элементов
        protected List<string> _fOptimalSets;		        // 
        public IRepresentativesStatisticAccumulator StatisticAccumulator { get; set; }
        protected string _inputDataShort;
        public string InputDataShort
        {
            get
            {
                return _inputDataShort;
            }
        }
        //--------------------------------------------------------------------------------------
        public BruteForceRepresentativesAsTreeDirect(int pLength)  : base(pLength)
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
            _inputDataShort = (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSetAsNumber));

            Execute();
        }
        //--------------------------------------------------------------------------------------
        protected override bool MakeAction()
        {
            if (_fCurrentPosition == _fSize - 1)
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
                if (isIntersect)
                {
                    int candidatValue = _fCurrentSet.Sum();
                    if (candidatValue <= currentMinimum)
                    {
                        if (candidatValue < currentMinimum)
                        {
                            for (int i = 0; i < _fCurrentSet.Count; i++)
                            {
                                _fCurrentOptimalSet[i] = _fCurrentSet[i];
                            }
                            currentMinimum = candidatValue;
                            _fOptimalSets.Clear();
                        }
                        List<int> result = new List<int>();
                        for (int i = 0; i < _fCurrentSet.Count; i++)
                        {
                            if (_fCurrentSet[i] != 0)
                                result.Add(i);
                        }
                        _fOptimalSets.Add(string.Join(",", result));
                        StatisticAccumulator.UpdateOptcountInc();
                    }
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        public List<int> Result
        {
            get
            {
                List<int> result = new List<int>();
                for (int i = 0; i < _fCurrentOptimalSet.Count; i++)
                {
                    if (_fCurrentOptimalSet[i] != 0)
                        result.Add(i);
                }
                return result;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<string> OptimalSets
        {
            get
            {
                return _fOptimalSets;
            }
            set
            {
                _fOptimalSets = value;
            }
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
    }
    //--------------------------------------------------------------------------------------
}
