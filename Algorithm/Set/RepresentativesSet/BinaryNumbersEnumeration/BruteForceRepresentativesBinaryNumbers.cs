using System.Diagnostics;
using BaseContract.Interfaces;
using StatisticsStorage.Accumulators;

namespace RepresentativesSet
{
    //--------------------------------------------------------------------------------------
    // class BruteForceRepresentativesBinaryNumders
    //--------------------------------------------------------------------------------------
    public class BruteForceRepresentativesBinaryNumbers : IHittingSetAlgorithm
    {
        int[][] jaggedArray2 = {
    new int[] { 1, 3, 5, 7, 9 },
    new int[] { 0, 2, 4, 6 },
    new int[] { 11, 12 }
};

        protected List<string> _fOptimalSets;               // 
        private int maxNumber;
        private long[] listOfSetAsBinary;
        public IRepresentativesStatisticAccumulator StatisticAccumulator { get; set; }
        //--------------------------------------------------------------------------------------
        public string SolutionAsString
        {
            get
            {
                return (_fOptimalSets?.Count ?? 0) > 0 ? _fOptimalSets[0] : "";
            }
        }
        public BruteForceRepresentativesBinaryNumbers()
        {
            StatisticAccumulator = new FakeRepresentativesStatisticAccumulator();
            _fOptimalSets = new List<string>();
        }
        public virtual void Execute(int[][] pListOfSet)
        {
            ExecuteByBinary(pListOfSet);
        }
        //--------------------------------------------------------------------------------------
        // pListOfSubSet - subsets is presented as list of numbers of element of Set that included into subset
        public List<int> ExecuteByBinary(int[][] pListOfSubSet)
        {
            Prepare(pListOfSubSet);
            return ExecuteByLongAsBinaryVector(listOfSetAsBinary, maxNumber);
        }
        //--------------------------------------------------------------------------------------
        public List<int> ExecuteByBinaryVer2(int[][] pListOfSubSet)
        {
            Prepare(pListOfSubSet);

            return ExecuteByLongAsBinaryVectorVer2(listOfSetAsBinary, maxNumber);
        }
        //--------------------------------------------------------------------------------------
        private void Prepare(int[][] pListOfSubSet)
        {
            _fOptimalSets.Clear();
            maxNumber = pListOfSubSet.Max(s => s.Max()) + 1;

            listOfSetAsBinary = pListOfSubSet.Select(s => BruteForceRepresentativesBinaryNumbers.ElementNumbersToLongAsBinaryVector(s)).ToArray();
            string inputDataShort = (Newtonsoft.Json.JsonConvert.SerializeObject(listOfSetAsBinary));
            StatisticAccumulator.CreateStatistics(pListOfSubSet.Select(l => l.ToArray()).ToArray(), inputDataShort, AlgorithmName);

        }
        //--------------------------------------------------------------------------------------
        private List<int> ExecuteByLongAsBinaryVector(long[] listOfSetAsBinary, int maxNumber)
        {
            return ExecuteByLongAsBinaryVectorGeneric(listOfSetAsBinary, maxNumber, (l, i) =>
            {
                bool isIntersect = true;
                for (int k = 0; k < listOfSetAsBinary.Length; k++)
                {
                    if ((listOfSetAsBinary[k] & i) == 0)
                    {
                        isIntersect = false;
                        break;
                    }
                }
                return isIntersect;
            });
        }
        //--------------------------------------------------------------------------------------
        private List<int> ExecuteByLongAsBinaryVectorVer2(long[] listOfSetAsBinary, int maxNumber)
        {
            return ExecuteByLongAsBinaryVectorGeneric(listOfSetAsBinary, maxNumber, (l, i) => l.All(s => (s & i) != 0));
        }
        //--------------------------------------------------------------------------------------
        private List<int> ExecuteByLongAsBinaryVectorGeneric(long[] listOfSetAsBinary, int maxNumber, Func<long[], int, bool> IsIntersect)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            long limit = 1 << maxNumber;
            long currentMinimumSet = limit - 1;
            int currentMinimum = maxNumber;
            for (int i = 0; i < limit; i++)
            {
                StatisticAccumulator.IterationCountInc();
                bool isIntersect = IsIntersect(listOfSetAsBinary, i);
                if (isIntersect)
                {
                    int candidatValue = DefineSumOfBit(i, limit, currentMinimum);
                    if (candidatValue <= currentMinimum)
                    {
                        StatisticAccumulator.IterationCountInc();
                        if (candidatValue < currentMinimum)
                        {
                            StatisticAccumulator.IterationCountInc();
                            StatisticAccumulator.UpdateOptcountInc();
                            currentMinimum = candidatValue;
                            currentMinimumSet = i;
                            _fOptimalSets.Clear();
                        }
                        _fOptimalSets.Add(string.Join(",", GetAsElementNumbers(i, maxNumber)));
                    }
                }
            }
            stopwatch.Stop();
            long elapsedTicks = stopwatch.ElapsedTicks;
            long durationMilliSeconds = stopwatch.ElapsedMilliseconds; List<int> result = GetAsElementNumbers(currentMinimumSet, maxNumber);
            string SolutionAsString = string.Join(",", result);
            StatisticAccumulator.SaveStatisticData(elapsedTicks, durationMilliSeconds, DateTime.Now,
                false, SolutionAsString, new List<string> { SolutionAsString }, result.Count);
            return result;
        }
        //--------------------------------------------------------------------------------------
        // long type representation of binary vector --> array of number psition with value 1(true) (kind of subset representaion)
        public static List<int> GetAsElementNumbers(long currentMinimumSet, int maxNumber)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < maxNumber; i++)
            {
                if (((1 << i) & currentMinimumSet) != 0)
                    result.Add(i);
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        // array of number psition with value 1(true) (kind of subset representaion) --> long type representation of binary vector
        public static long ElementNumbersToLongAsBinaryVector(int[] numberElements)
        {
            long result = 0;
            foreach (int pos in numberElements)
                result |= 1L << pos;
            return result;
        }
        //--------------------------------------------------------------------------------------
        public static int DefineSumOfBit(int numberAsSet, long limit, int curMin)
        {
            int sum = 0;
            int pos = 1;
            while (pos <= limit && sum <= curMin)
            {
                if ((numberAsSet & pos) != 0)
                    sum++;
                pos <<= 1;
            }
            return sum;
        }
        //--------------------------------------------------------------------------------------
        public static long GetNumberLeafOfTriangleTree(int length, int cardinality)
        {
            int n = 1 << cardinality;
            int k = length;
            int n_k = n - k;
            long result = 1;
            for (int i = n - 1; i >= n_k; i--)
            {
                result *= i;
            }
            for (int i = 2; i <= k; i++)
            {
                result /= i;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        public static long DefineSumOfBitVer2(long numberAsSet)
        {
            long result = (numberAsSet & 0x5555555555555555) + ((numberAsSet >> 1) & 0x5555555555555555);
            result = (result & 0x3333333333333333) + ((result >> 2) & 0x3333333333333333);
            result = (result & 0x0F0F0F0F0F0F0F0F) + ((result >> 4) & 0x0F0F0F0F0F0F0F0F);
            result = (result & 0x00FF00FF00FF00FF) + ((result >> 8) & 0x00FF00FF00FF00FF);
            result = (result & 0x0000FFFF0000FFFF) + ((result >> 16) & 0x0000FFFF0000FFFF);
            result = (result & 0x00000000FFFFFFFF) + ((result >> 32) & 0x00000000FFFFFFFF);

            return result;
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
        public virtual string AlgorithmName
        {
            get
            {
                return GetType().Name;
            }
        }
        //--------------------------------------------------------------------------------------
        public int CurrentMinimum
        {
            get
            {
                return _fOptimalSets.Count;
            }
        }
    }
    //--------------------------------------------------------------------------------------
    public class BruteForceRepresentativesBinaryNumbersVer2 : BruteForceRepresentativesBinaryNumbers
    {
        //--------------------------------------------------------------------------------------
        public override void Execute(int[][] pListOfSet)
        {
            ExecuteByBinaryVer2(pListOfSet);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
