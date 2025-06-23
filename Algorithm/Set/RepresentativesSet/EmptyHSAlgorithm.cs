using BaseContract.Interfaces;
using BaseLibrary.Helpers;
using StatisticsStorage.Accumulators;

namespace RepresentativesSet
{
    public class EmptyHSAlgorithm : IHittingSetAlgorithm
    {
        protected int _fSize;
        protected int _fLimit;
        protected List<List<int>> listOfSet;
        protected long[] listOfSetAsNumber;
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
        //--------------------------------------------------------------------------------------
        public string SolutionAsString
        {
            get
            {
                return "";
            }
        }
        public List<string> OptimalSets
        {
            get
            {
                return null;
            }
        }
        public string AlgorithmName { get; }
        public IRepresentativesStatisticAccumulator StatisticAccumulator { get; set; }
        public EmptyHSAlgorithm(int pLength) 
        {
            StatisticAccumulator = new FakeRepresentativesStatisticAccumulator();
            AlgorithmName = "Empty";
        }
        public void Execute(int[][] pListOfSet)
        {
            this.listOfSet = pListOfSet.Select(l => l.ToList()).ToList();
            listOfSetAsNumber = listOfSet.Select(s => BruteForceRepresentativesBinaryNumbers.ElementNumbersToLongAsBinaryVector(s.ToArray())).ToArray();
            int numberOfElemnts = pListOfSet.SelectMany(l => l.Select(i => i)).Max() + 1;
            _inputData = listOfSet.AsString(); // (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSet));
            _inputDataShort = (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSetAsNumber));
            StatisticAccumulator.CreateStatistics(listOfSet.Select(l => l.ToArray()).ToArray(), _inputDataShort, AlgorithmName);
            StatisticAccumulator.SaveStatisticData(0, 0, DateTime.Now, false, string.Empty, new List<string> (), 0);
        }
        public int CurrentMinimum
        {
            get
            {
                return 0;
            }
        }
    }
}
