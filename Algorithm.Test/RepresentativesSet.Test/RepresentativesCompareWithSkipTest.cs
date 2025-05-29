using BaseLibrary;
using BaseLibrary.Helpers;
using RepresentativesSet;
using StatisticsStorage.Accumulators;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;


namespace RepresentativesSetTest
{
    [TestClass]
    public class RepresentativesCompareWithSkipTest
    {
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareTestCase1()
        {
            // arrange
            int сardinality = 4;
            int length = 4;
            long number = BruteForceRepresentativesBinaryNumbers.GetNumberLeafOfTriangleTree(length, сardinality);
            int step = (int)(number/100);
            EnumerateIntegerTrangleRepresentativesCompare enumeration = new EnumerateIntegerTrangleRepresentativesCompare(сardinality, length, step);
            // act
            enumeration.Execute();
            // assert
            File.WriteAllLines("selected.txt",enumeration.Selected);

        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareTestCase2()
        {
            // arrange
            int сardinality = 5;
            int length = 6;
            long number = BruteForceRepresentativesBinaryNumbers.GetNumberLeafOfTriangleTree(length, сardinality);
            int step = (int)(number / 100);
            EnumerateIntegerTrangleRepresentativesCompare enumeration = new EnumerateIntegerTrangleRepresentativesCompare(сardinality, length, step);
            // act
            enumeration.Execute();
            // assert
            File.WriteAllLines("selected.txt", enumeration.Selected);
//            File.WriteAllLines("result.txt", enumeration.Result);

        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareCombination()
        {
            // arrange
            int n = 43;
            int m = 12;

            // act
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            long[,] matrix = Combinatorics.CreateCombinationMatrix(n, m);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            long ticks = stopWatch.ElapsedTicks;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Restart();
            long[,] matrixByBigNumber = Combinatorics.CreateCombinationMatrixByBigInteger(n, m);
            stopWatch.Stop();
            TimeSpan tsByBigNumber = stopWatch.Elapsed;
            long ticksByBigNumber = stopWatch.ElapsedTicks;
            string elapsedTimeByBigNumber = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsByBigNumber.Hours, tsByBigNumber.Minutes, tsByBigNumber.Seconds, tsByBigNumber.Milliseconds / 10);

            stopWatch.Restart();
            long[,] matrixByRec = Combinatorics.CreateCombinationMatrixByRec(n, m);
            stopWatch.Stop();
            TimeSpan tsByRec = stopWatch.Elapsed;
            long ticksByRec = stopWatch.ElapsedTicks;
            string elapsedTimeByRec = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsByRec.Hours, tsByRec.Minutes, tsByRec.Seconds, tsByRec.Milliseconds / 10);

            stopWatch.Restart();
            long[,] matrixRed = Combinatorics.CreateCombinationReductionMatrix(n, m);
            stopWatch.Stop();
            TimeSpan tsRed = stopWatch.Elapsed;
            long ticksRed = stopWatch.ElapsedTicks;
            string elapsedTimeRed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsRed.Hours, tsRed.Minutes, tsRed.Seconds, tsRed.Milliseconds / 10);

            //stopWatch.Restart();
            //long[,] matrixRec = Combinatorics.CreateCombinationRecMatrix(n, m);
            //stopWatch.Stop();
            //TimeSpan tsRec = stopWatch.Elapsed;
            //long ticksRec = stopWatch.ElapsedTicks;
            //string elapsedTimeRec = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsRec.Hours, tsRec.Minutes, tsRec.Seconds, tsRec.Milliseconds / 10);


            // assert
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    Assert.AreEqual(matrix[i, j], matrixByBigNumber[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixByBigNumber[i, j]}");
                    Assert.AreEqual(matrix[i, j], matrixByRec[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixByRec[i, j]}");
                    Assert.AreEqual(matrix[i, j], matrixRed[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixRed[i, j]}");
                    //                    Assert.AreEqual(matrix[i, j], matrixRec[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixRec[i, j]}");
                }
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareBigIntegerCombination()
        {
            // arrange
            int n = 2000;
            int m = 20;

            // act
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BigInteger[,] matrix = Combinatorics.CreateCombinationBigIntegerMatrix(n, m);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            long ticks = stopWatch.ElapsedTicks;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            stopWatch.Restart();
            BigInteger[,] matrixByRec = Combinatorics.CreateCombinationBigIntegerMatrixByRec(n, m);
            stopWatch.Stop();
            TimeSpan tsByRec = stopWatch.Elapsed;
            long ticksByRec = stopWatch.ElapsedTicks;
            string elapsedTimeByRec = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsByRec.Hours, tsByRec.Minutes, tsByRec.Seconds, tsByRec.Milliseconds / 10);

            stopWatch.Restart();
            BigInteger[,] matrixRed = Combinatorics.CreateCombinationReductionBigIntegerMatrix(n, m);
            stopWatch.Stop();
            TimeSpan tsRed = stopWatch.Elapsed;
            long ticksRed = stopWatch.ElapsedTicks;
            string elapsedTimeRed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", tsRed.Hours, tsRed.Minutes, tsRed.Seconds, tsRed.Milliseconds / 10);

            // assert
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    Assert.AreEqual(matrix[i, j], matrixByRec[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixByRec[i, j]}");
                    Assert.AreEqual(matrix[i, j], matrixRed[i, j], $"i={i + 1}, j={j + 1} {matrix[i, j]} {matrixRed[i, j]}");
                }
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareMaxCombination()
        {
            // arrange

            // act
            long longResult43_12 = 0;
            try
            {
                longResult43_12 = Combinatorics.Combination(43, 12);
                //                long longResult44_12 = Combinatorics.Combination(44, 12);
                long longResult57_11 = Combinatorics.Combination(57, 11);
                long longResult58_11 = Combinatorics.Combination(58, 11);
//                long longResult59_11 = Combinatorics.Combination(59, 11);
//                long longResult60_11 = Combinatorics.Combination(60, 11);
                //                long longResult37_13 = Combinatorics.Combination(37, 13);
                //               long longResult36_13 = Combinatorics.Combination(36, 13);
                long longResult35_13 = Combinatorics.Combination(35, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }

            long longRedResult43_12 = 0;
            long longRedResult35_13 = 0;
            long longRedResult40_13 = 0;
            long longRedResult50_12 = 0;
            long longRedResult60_11 = 0;
            try
            {
                longRedResult43_12 = Combinatorics.CombinationReduction(43, 12);
                //long longRedResult57_11 = Combinatorics.CombinationReduction(57, 11);
                //long longRedResult58_11 = Combinatorics.CombinationReduction(58, 11);
                //longRedResult35_13 = Combinatorics.CombinationReduction(35, 13);
                longRedResult50_12 = Combinatorics.CombinationReduction(50, 12);
                longRedResult60_11 = Combinatorics.CombinationReduction(60, 11);
                longRedResult40_13 = Combinatorics.CombinationReduction(40, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            //long longRecResult43_12 = 0;
            long longRecResult50_12 = 0;
            long longRecResult60_11 = 0;
            long longRecResult40_13 = 0;
            try
            {
                //longRecResult43_12 = Combinatorics.CombinationRec(43, 12);

                longRecResult50_12 = Combinatorics.CombinationRec(50, 12);
                //                long longResult59_11 = Combinatorics.Combination(59, 11);
                longRecResult60_11 = Combinatorics.CombinationRec(60, 11);
                longRecResult40_13 = Combinatorics.CombinationRec(40, 13);
                //               long longResult36_13 = Combinatorics.Combination(36, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Assert.AreEqual(longResult43_12, longRedResult43_12, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult50_12, longRedResult50_12, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult60_11, longRedResult60_11, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult40_13, longRedResult40_13, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareMaxBigIntegerCombination()
        {
            // arrange

            // act
            BigInteger longResult43_12 = 0;
            try
            {
                longResult43_12 = Combinatorics.Combination(43, 12);
                BigInteger longResult57_11 = Combinatorics.BigIntegerCombination(57, 11);
                BigInteger longResult58_11 = Combinatorics.BigIntegerCombination(58, 11);
                BigInteger longResult35_13 = Combinatorics.BigIntegerCombination(35, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            BigInteger longRedResult43_12 = 0;
            BigInteger longRedResult40_13 = 0;
            BigInteger longRedResult50_12 = 0;
            BigInteger longRedResult60_11 = 0;
            try
            {
                longRedResult43_12 = Combinatorics.BigIntegerCombinationReduction(43, 12);
                longRedResult50_12 = Combinatorics.BigIntegerCombinationReduction(50, 12);
                longRedResult60_11 = Combinatorics.BigIntegerCombinationReduction(60, 11);
                longRedResult40_13 = Combinatorics.BigIntegerCombinationReduction(40, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            BigInteger longRecResult50_12 = 0;
            BigInteger longRecResult60_11 = 0;
            BigInteger longRecResult40_13 = 0;
            try
            {

                longRecResult50_12 = Combinatorics.BigIntegerCombinationRec(50, 12);
                longRecResult60_11 = Combinatorics.BigIntegerCombinationRec(60, 11);
                longRecResult40_13 = Combinatorics.BigIntegerCombinationRec(40, 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Assert.AreEqual(longResult43_12, longRedResult43_12, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult50_12, longRedResult50_12, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult60_11, longRedResult60_11, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
            Assert.AreEqual(longRecResult40_13, longRedResult40_13, $"long - {longResult43_12}, longResult - {longRedResult43_12}");
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void CompareSkipTestCase1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            // arrange
            int limit = 43;
            int length = 11;
            long number = Combinatorics.Combination(limit, length);
            int step = (int)(number / 100000);
            //step = 678;
            EnumerateIntegerTrangleForSkipCalculation enumeration = new EnumerateIntegerTrangleForSkipCalculation(limit, length, step);
            // act
            enumeration.Execute();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            long ticks = stopWatch.ElapsedTicks;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // assert
            File.WriteAllLines("selected.txt", enumeration.Selected);

        }
    }

    //--------------------------------------------------------------------------------------
    // class EnumerateIntegerTrangleRepresentativesCompare
    //--------------------------------------------------------------------------------------
    public class EnumerateIntegerTrangleRepresentativesCompare : EnumerateIntegerTrangle
    {
        private int _fCardinality;
        private int _step;
        private int _counter = 0;
        private List<string> _result = new List<string>();
        private List<string> _selected = new List<string>();
        private RepresentativesStatisticAccumulator _statisticAccumulator;
        public List<string> Result
        { 
            get { return _result; } 
        }
        public List<string> Selected
        {
            get { return _selected; }
        }
        //--------------------------------------------------------------------------------------
        public EnumerateIntegerTrangleRepresentativesCompare(int pCardinality, int pLength, int step, int pMinimumValue = 1, int pForwardAdditive = 1)
            : base((1 << pCardinality) - 1, pLength, pMinimumValue, pForwardAdditive)
        {
            _fBreakElement = 0;
            _fCardinality = pCardinality;
            _step= step;
            _result = new List<string>();   
            _selected = new List<string>();
        }
        //--------------------------------------------------------------------------------------
        protected override bool MakeAction()
        {
            if (_fCurrentPosition == _fSize - 1)
            {
                _counter++;
                string strRepresenttion = string.Join(",", _fCurrentSet);
                if (_counter == _step)
                {
                    _counter = 0;
                    _selected.Add(strRepresenttion);
                }
                _result.Add(strRepresenttion);
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        protected override bool IsCompleteCondition()
        {
            return base.IsCompleteCondition();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostAction()
        {
        }
        //--------------------------------------------------------------------------------------

    }
    //--------------------------------------------------------------------------------------
    // class EnumerateIntegerTrangleForSkipCalculation
    //--------------------------------------------------------------------------------------
    public class EnumerateIntegerTrangleForSkipCalculation : EnumerateIntegerTrangle
    {
        private int _step;
        private long _counter = 0;
        private int _stepCounter = 0;
        private List<string> _result = new List<string>();
        private List<string> _selected = new List<string>();
        public List<string> Result
        {
            get { return _result; }
        }
        public List<string> Selected
        {
            get { return _selected; }
        }
        //--------------------------------------------------------------------------------------
        public EnumerateIntegerTrangleForSkipCalculation(int pLimit, int pLength, int step, int pMinimumValue = 1, int pForwardAdditive = 1)
            : base(pLimit, pLength, pMinimumValue, pForwardAdditive)
        {
            _fBreakElement = 0;
            _step = step;
            _result = new List<string>();
            _selected = new List<string>();
            Combinatorics.SetCombinationMatrix(pLimit,pLength);
            Combinatorics.SetCombinationBigIntegerMatrix(pLimit, pLength);
        }
        //--------------------------------------------------------------------------------------
        protected override bool MakeAction()
        {
            if (_fCurrentPosition == _fSize - 1)
            {
                _counter++;
                _stepCounter++;
                if (_stepCounter == _step)
                {
                    _stepCounter = 0;
                    int[] skipList = Combinatorics.SkipEnumeration(_fLimit, _fSize, _counter);
                    Combinatorics simpleleCombinatorics = new Combinatorics("SkipEnumerationBigInteger", "By Matrix", _fLimit, _fSize);
                    int[] skipListBigInteger = simpleleCombinatorics.SkipEnumerationBigInteger(_fLimit, _fSize,new BigInteger( _counter), null, null);
                    Combinatorics noRecCombinatorics = new Combinatorics("SkipEnumerationNoRecBigInteger", "By Matrix", _fLimit, _fSize);
                    int[] skipListNoRecBigInteger = noRecCombinatorics.SkipEnumerationBigInteger(_fLimit, _fSize, new BigInteger(_counter), null, null);
                    string strRepresenttion = string.Join(",", _fCurrentSet);
                    string strSkipList = string.Join(",", skipList);
                    string strSkipListBigInteger = string.Join(",", skipListBigInteger);
                    string strSkipListNoRecBigInteger = string.Join(",", skipListNoRecBigInteger);
                    _selected.Add(strRepresenttion);
                    _result.Add(strRepresenttion);
                    Assert.AreEqual(strRepresenttion, strSkipList);
                    Assert.AreEqual(strRepresenttion, strSkipListBigInteger);
                    Assert.AreEqual(strRepresenttion, strSkipListNoRecBigInteger);
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        protected override bool IsCompleteCondition()
        {
            return base.IsCompleteCondition();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostAction()
        {
        }
        //--------------------------------------------------------------------------------------

    }
}
