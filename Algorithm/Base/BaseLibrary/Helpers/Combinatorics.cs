using System.Numerics;

namespace BaseLibrary.Helpers
{
    //--------------------------------------------------------------------------------------
    // class Combinatorics
    //--------------------------------------------------------------------------------------
    public class Combinatorics
    {
        //--------------------------------------------------------------------------------------
        public static long[,]? CombinationMatrix;
        public static BigInteger[,]? BigIntCombinationMatrix;
        public static Dictionary<(int,int), BigInteger> BigIntCombinationDictionary = new Dictionary<(int, int), BigInteger>();
        private static BigInteger[,,,]? CountForPositionMatrix;
        public static Dictionary<(int, int, int, int), BigInteger> CountForPositionDictionary = new Dictionary<(int, int, int, int), BigInteger>();
        public Func<int, int, BigInteger, int?, int?, int[]> SkipEnumerationBigInteger;
        private Func<int, int, BigInteger> GetBigIntegerCombination;
        private Func<int, int, int, int, BigInteger> GetCountForPositionSaveFPBigInteger;

        public Combinatorics(string calculationStep, string combinationType, int limit, int size)
        {
            SkipEnumerationBigInteger = calculationStep switch
            {
                "SkipEnumerationBigInteger" => SkipEnumerationBigIntegerSimple,
                "SkipEnumerationNoRecBigInteger" => SkipEnumerationNoRecBigInteger,
                "SkipEnumerationSaveFPBigInteger" => SkipEnumerationSaveFPBigInteger,              // Not Recursive Save first position
                "SkipEnumerationSaveFPImpBigInteger" => SkipEnumerationSaveFPImpBigInteger            // Not Recursive Save first position Improve 1
            };
            GetBigIntegerCombination = combinationType switch
            {
                "By Matrix" => CombinationByMatrixBigInteger,
                "By Dictionary" => CombinationByDictionaryBigInteger,
                "Without Matrix" => BigIntegerCombination            
            };
            GetCountForPositionSaveFPBigInteger = combinationType switch
            {
                "By Matrix" => GetCountForPositionSaveFPBigIntegerByMatrix,
                "By Dictionary" => GetCountForPositionSaveFPBigIntegerByDictionary,
                "Without Matrix" => CalculateCountForPositionSaveFPBigInteger
            };
            if (combinationType == "By Matrix")
            {
                Combinatorics.SetCombinationBigIntegerMatrix(limit, size);
                Combinatorics.CreateCountForPositionMatrix(limit, size);
            }

        }
        //--------------------------------------------------------------------------------------
        // Matrix
        //--------------------------------------------------------------------------------------
        public static void SetCombinationMatrix(int n, int m)
        {
            CombinationMatrix = CreateCombinationMatrixByBigInteger(n, m);
        }
        //--------------------------------------------------------------------------------------
        public static long[,] CreateCombinationMatrix(int n, int m)
        {
            long[,] matrix = new long[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = Combination(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static long[,] CreateCombinationMatrixByBigInteger(int n, int m)
        {
            long[,] matrix = new long[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = (long)BigIntegerCombination(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static long[,] CreateCombinationRecMatrix(int n, int m)
        {
            long[,] matrix = new long[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = CombinationRec(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static long[,] CreateCombinationReductionMatrix(int n, int m)
        {
            long[,] matrix = new long[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = CombinationReduction(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static long[,] CreateCombinationMatrixByRec(int n, int k)
        {
            long[,] matrix = new long[n, k];
            matrix[0, 0] = 1;
            for (int i = 1; i < k; i++)
            {
                matrix[0, i] = 0;
            }
            for (int i = 1; i < n; i++)
            {
                matrix[i, 0] = i + 1;
            }
            for (int i = 1; i < n; i++)
                for (int j = 1; j < k; j++)
                    matrix[i, j] = matrix[i - 1, j - 1] + matrix[i - 1, j];

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger[,] CreateCombinationBigIntegerMatrix(int n, int m)
        {
            BigInteger[,] matrix = new BigInteger[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = BigIntegerCombination(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger[,] CreateCombinationRecBigIntegerMatrix(int n, int m)
        {
            BigInteger[,] matrix = new BigInteger[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = BigIntegerCombinationRec(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger[,] CreateCombinationReductionBigIntegerMatrix(int n, int m)
        {
            BigInteger[,] matrix = new BigInteger[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = BigIntegerCombinationReduction(i + 1, j + 1);

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger[,] CreateCombinationBigIntegerMatrixByRec(int n, int k)
        {
            BigInteger[,] matrix = new BigInteger[n, k];
            matrix[0, 0] = 1;
            for (int i = 1; i < k; i++)
            {
                matrix[0, i] = 0;
            }
            for (int i = 1; i < n; i++)
            {
                matrix[i, 0] = i + 1;
            }
            for (int i = 1; i < n; i++)
                for (int j = 1; j < k; j++)
                    matrix[i, j] = matrix[i - 1, j - 1] + matrix[i - 1, j];

            return matrix;
        }
        //--------------------------------------------------------------------------------------
        // Combination
        //--------------------------------------------------------------------------------------
        public static long CombinationRec(int n, int k)
        {
            if (k > n)
                return 0;
            if ( k == n )
                return 1;
            if (k == 1 || k == n - 1)
                return n;
            if (k > n - k)
                k = n - k;
            return CombinationRec(n - 1, k - 1) + CombinationRec(n - 1, k);
        }
        //--------------------------------------------------------------------------------------
        public static long Combination(int n, int k)
        {
            checked
            {
                if (k > n)
                    return 0;
                if (k > n - k)
                    k = n - k;
                long numerator = 1;
                long denominator = 1;
                for (int i = n - k + 1; i <= n; i++)
                    numerator *= i;
                for (int i = 1; i <= k; i++)
                    denominator *= i;

                return numerator / denominator;
            }
        }
        //--------------------------------------------------------------------------------------
        public static long CombinationReduction(int n, int k)
        {
            checked
            {
                if (k > n)
                    return 0;
                if (k > n - k)
                    k = n - k;
                long numerator = 1;
                long denominator = 1;
                List<int> denList = Enumerable.Range(1, k).ToList();
                for (int i = n - k + 1; i <= n; i++)
                {
                    int m = i;
                    for (int j = denList.Count - 1; j >= 0; j--)
                    {
                        if (m % denList[j] == 0)
                        {
                            m /= denList[j];
                            denList.RemoveAt(j);
                        }
                        if (m == 1)
                            break;
                    }
                    numerator *= m;
                }
                    
                for (int i = 0; i < denList.Count; i++)
                    denominator *= denList[i];

                return numerator / denominator;
            }
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger BigIntegerCombination(int n, int k)
        {
            if (k > n)
                return 0;
            if (k > n - k)
                k = n - k;
            BigInteger numerator = new BigInteger(1);
            BigInteger denominator = new BigInteger(1);
            for (int i = n - k + 1; i <= n; i++)
                numerator = BigInteger.Multiply(numerator, i);
            for (int i = 1; i <= k; i++)
                denominator = BigInteger.Multiply(denominator, i);

            return BigInteger.Divide(numerator, denominator);
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger BigIntegerCombinationRec(int n, int k)
        {
            if (k > n)
                return 0;
            if (k == n)
                return 1;
            if (k == 1 || k == n - 1)
                return n;
            if (k > n - k)
                k = n - k;
            return BigIntegerCombinationRec(n - 1, k - 1) + BigIntegerCombinationRec(n - 1, k);
        }
        //--------------------------------------------------------------------------------------
        public static BigInteger BigIntegerCombinationReduction(int n, int k)
        {
            checked
            {
                if (k > n)
                    return 0;
                if (k > n - k)
                    k = n - k;
                BigInteger numerator = 1;
                BigInteger denominator = 1;
                List<int> denList = Enumerable.Range(1, k).ToList();
                for (int i = n - k + 1; i <= n; i++)
                {
                    int m = i;
                    for (int j = denList.Count - 1; j >= 0; j--)
                    {
                        if (m % denList[j] == 0)
                        {
                            m /= denList[j];
                            denList.RemoveAt(j);
                        }
                        if (m == 1)
                            break;
                    }
                    numerator *= m;
                }

                for (int i = 0; i < denList.Count; i++)
                    denominator *= denList[i];

                return numerator / denominator;
            }
        }
        //--------------------------------------------------------------------------------------
        // Long
        //--------------------------------------------------------------------------------------
        public static long CalculateStep(int n, int k, int maxNumber)
        {
            BigInteger combinatio = BigIntegerCombination(n, k);

            var result = BigInteger.Divide(combinatio, maxNumber);
            return (long)result;
        }
        //--------------------------------------------------------------------------------------
        public static int[] SkipEnumeration(int n, int m, long number)
        {
            int[] result = new int[m];
            long rest = number;
            int curn = n;
            int curm = m;
            int previ = 1;
            int prevj = 1;
            int lastJ = 0;
            while (rest > 0)
            {
                (int i, int j, long countForCurrentIndex) = GetFirstPosition(curn, curm, rest);
                int ii = previ;
                for (int jj = prevj; jj < prevj - 1 + j; jj++)
                {
                    result[jj - 1] = ii++;
                }
                lastJ = prevj - 2 + j;
                result[prevj - 2 + j] = previ + i - 1;
                previ += i;
                prevj += j;
                curm -= j;
                curn -= i;
                rest -= countForCurrentIndex;
            }
            lastJ++;
            while (lastJ < m)
            {
                result[lastJ] = LastIndexInCountMatrix(n, m, lastJ + 1);
                lastJ++;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        public static (int, int, long) GetFirstPosition(int n, int m, long number)
        {
            if (number == 1)
                return (m, m, 1);
            int j = m;
            int jlast = m;
            int ilast = n;
            long countLast = 1;
            while (j > 0)
            {
                int lastIndex = LastIndexInCountMatrix(n, m, j);
                long countForLastIndex = GetCountForPosition(n, m, lastIndex, j);
                if (countForLastIndex == number)
                    return (lastIndex, j, countForLastIndex);
                if (countForLastIndex > number)
                {
                    int i = j + 1;
                    while (i <= n)
                    {
                        long countForCurrentIndex = GetCountForPosition(n, m, i, j);
                        if (countForCurrentIndex == number)
                            return (i, j, countForCurrentIndex);
                        if (countForCurrentIndex > number)
                            return (i, j, countLast);
                        jlast = j;
                        ilast = i;
                        countLast = countForCurrentIndex;
                        i++;
                    }
                    throw new Exception("Logical error GetFirstPosition");

                }
                else
                {
                    countLast = countForLastIndex;
                }
                --j;
            }
            return (0, 0, 0L);
        }
        //--------------------------------------------------------------------------------------
        private static long GetCountForPosition(int n, int m, int i, int j)
        {
            if (j > m)
                return 0;
            else if (i < j)
                return 0;
            else if (j == m)
                return i - j + 1;
            else if (i == j)
            {
                return GetCountForPosition(n, m, LastIndexInCountMatrix(n, m, j + 1), j + 1);
            }
            else
            {
                long prevCount = GetCountForPosition(n, m, i - 1, j);
                long combi = CombinationByMatrix(n - i, m - j);
                return prevCount + combi;
            }

        }
        //--------------------------------------------------------------------------------------
        private static long CombinationByMatrix(int n, int k)
        {
            return CombinationMatrix[n - 1, k - 1];
        }
        //--------------------------------------------------------------------------------------
        private static int LastIndexInCountMatrix(int n, int m, int j)
        {
            return n - (m - j);
        }
        //--------------------------------------------------------------------------------------
        // BigInteger
        //--------------------------------------------------------------------------------------
        private int[] SkipEnumerationBigIntegerSimple(int n, int m, BigInteger number, int? curnStart = null, int? curmStart = null)
        {
            int[] result = new int[m];
            BigInteger rest = number;
            int curn = n;
            int curm = m;
            int previ = 1;
            int prevj = 1;
            int lastJ = 0;
            while (rest > 0)
            {
                (int i, int j, BigInteger countForCurrentIndex) = GetFirstPositionBigInteger(curn, curm, rest);
                int ii = previ;
                for (int jj = prevj; jj < prevj - 1 + j; jj++)
                {
                    result[jj - 1] = ii++;
                }
                lastJ = prevj - 2 + j;
                result[prevj - 2 + j] = previ + i - 1;
                previ += i;
                prevj += j;
                curm -= j;
                curn -= i;
                rest -= countForCurrentIndex;
            }
            lastJ++;
            while (lastJ < m)
            {
                result[lastJ] = LastIndexInCountMatrix(n, m, lastJ + 1);
                lastJ++;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        private (int, int, BigInteger) GetFirstPositionBigInteger(int n, int m, BigInteger number)
        {
            if (number == 1)
                return (m, m, 1);
            int j = m;
            int jlast = m;
            int ilast = n;
            BigInteger countLast = 1;
            while (j > 0)
            {
                int lastIndex = LastIndexInCountMatrix(n, m, j);
                BigInteger countForLastIndex = GetCountForPositionBigInteger(n, m, lastIndex, j);
                if (countForLastIndex == number)
                    return (lastIndex, j, countForLastIndex);
                if (countForLastIndex > number)
                {
                    int i = j + 1;
                    while (i <= n)
                    {
                        BigInteger countForCurrentIndex = GetCountForPositionBigInteger(n, m, i, j);
                        if (countForCurrentIndex == number)
                            return (i, j, countForCurrentIndex);
                        if (countForCurrentIndex > number)
                            return (i, j, countLast);
                        jlast = j;
                        ilast = i;
                        countLast = countForCurrentIndex;
                        i++;
                    }
                    throw new Exception("Logical error GetFirstPosition");

                }
                else
                {
                    countLast = countForLastIndex;
                }
                --j;
            }
            return (0, 0, 0L);
        }
        //--------------------------------------------------------------------------------------
        public static void SetCombinationBigIntegerMatrix(int n, int m)
        {
            BigIntCombinationMatrix = CreateCombinationBigIntegerMatrix(n, m);
        }
        //--------------------------------------------------------------------------------------
        public static void CreateCountForPositionMatrix(int n, int m)
        { 
            CountForPositionMatrix = new BigInteger[n + 1, m + 1, n + 1, m + 1];
            for (int i = 0; i <= n; i++)
                for (int j = 0; j <= m; j++)
                    for (int k = 0; k <= n; k++)
                        for (int l = 0; l <= m; l++)
                            CountForPositionMatrix[i, j, k, l] = -1;
        }
        //--------------------------------------------------------------------------------------
        public BigInteger GetCountForPositionBigInteger(int n, int m, int i, int j)
        {
            if (j > m)
                return 0;
            else if (i < j)
                return 0;
            else if (j == m)
                return i - j + 1;
            else if (i == j)
            {
                return GetCountForPositionBigInteger(n, m, LastIndexInCountMatrix(n, m, j + 1), j + 1);
            }
            else
            {
                BigInteger prevCount = GetCountForPositionBigInteger(n, m, i - 1, j);
                BigInteger combi = GetBigIntegerCombination(n - i, m - j);
                return prevCount + combi;
            }

        }
        //--------------------------------------------------------------------------------------
        private static BigInteger CombinationByMatrixBigInteger(int n, int k)
        {
            return BigIntegerCombination(n, k);
        }
        //--------------------------------------------------------------------------------------
        private static BigInteger CombinationByDictionaryBigInteger(int n, int k)
        {
            return BigIntCombinationMatrix[n - 1, k - 1];
        }
        //--------------------------------------------------------------------------------------
        // BigInteger Inproved - Not Recursive
        //--------------------------------------------------------------------------------------
        private int[] SkipEnumerationNoRecBigInteger(int n, int m, BigInteger number, int? curnStart = null, int? curmStart = null)
        {
            int[] result = new int[m];
            BigInteger rest = number;
            int curn = n;
            int curm = m;
            int previ = 1;
            int prevj = 1;
            int lastJ = 0;
            while (rest > 0)
            {
                (int i, int j, BigInteger countForCurrentIndex) = GetFirstPositionNoRecBigInteger(curn, curm, rest);
                int ii = previ;
                for (int jj = prevj; jj < prevj - 1 + j; jj++)
                {
                    result[jj - 1] = ii++;
                }
                lastJ = prevj - 2 + j;
                result[prevj - 2 + j] = previ + i - 1;
                previ += i;
                prevj += j;
                curm -= j;
                curn -= i;
                rest -= countForCurrentIndex;
            }
            lastJ++;
            while (lastJ < m)
            {
                result[lastJ] = LastIndexInCountMatrix(n, m, lastJ + 1);
                lastJ++;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        private (int, int, BigInteger) GetFirstPositionNoRecBigInteger(int n, int m, BigInteger number)
        {
            if (number == 1)
                return (m, m, 1);
            int j = m;
            int jlast = m;
            int ilast = n;
            BigInteger countLast = 1;
            while (j > 0)
            {
                int lastIndex = LastIndexInCountMatrix(n, m, j);
                BigInteger countForLastIndex = GetCountForPositionNoRecBigInteger(n, m, lastIndex, j);
                if (countForLastIndex == number)
                    return (lastIndex, j, countForLastIndex);
                if (countForLastIndex > number)
                {
                    int i = j + 1;
                    while (i <= n)
                    {
                        BigInteger countForCurrentIndex = GetCountForPositionNoRecBigInteger(n, m, i, j);
                        if (countForCurrentIndex == number)
                            return (i, j, countForCurrentIndex);
                        if (countForCurrentIndex > number)
                            return (i, j, countLast);
                        jlast = j;
                        ilast = i;
                        countLast = countForCurrentIndex;
                        i++;
                    }
                    throw new Exception("Logical error GetFirstPosition");

                }
                else
                {
                    countLast = countForLastIndex;
                }
                --j;
            }
            return (0, 0, 0L);
        }
        //--------------------------------------------------------------------------------------
        public BigInteger GetCountForPositionNoRecBigInteger(int n, int m, int istart, int jstart)
        {
            if (jstart > m)
                return 0;
            else if (istart < jstart)
                return 0;
            else if (jstart == m)
                return istart - jstart + 1;
            else
            {
                BigInteger result = n - m + 1;
                for (int j = m - 1; j >= jstart; j--)
                {
                    int iLimit = j == jstart ? istart : LastIndexInCountMatrix(n, m, j);
                    for (int i = j+1; i <= iLimit; i++)
                    {
                        result = BigInteger.Add( result, GetBigIntegerCombination(n-i, m-j));
                    }
                }
                return result;
            }
        }
        //--------------------------------------------------------------------------------------
        // BigInteger Inproved - Not Recursive Save first position
        //--------------------------------------------------------------------------------------
        private int[] SkipEnumerationSaveFPBigInteger(int n, int m, BigInteger number, int? curnStart = null, int? curmStart = null)
        {
            int[] result = new int[m];
            BigInteger rest = number;
            int curn = n;
            int curm = m;
            int previ = 1;
            int prevj = 1;
            int lastJ = 0;
            while (rest > 0)
            {
                (int i, int j, BigInteger countForCurrentIndex) = GetFirstPositionSaveFPBigInteger(curn, curm, rest);
                int ii = previ;
                for (int jj = prevj; jj < prevj - 1 + j; jj++)
                {
                    result[jj - 1] = ii++;
                }
                lastJ = prevj - 2 + j;
                result[prevj - 2 + j] = previ + i - 1;
                previ += i;
                prevj += j;
                curm -= j;
                curn -= i;
                rest -= countForCurrentIndex;
            }
            lastJ++;
            while (lastJ < m)
            {
                result[lastJ] = LastIndexInCountMatrix(n, m, lastJ + 1);
                lastJ++;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        private  (int, int, BigInteger) GetFirstPositionSaveFPBigInteger(int n, int m, BigInteger number)
        {
            if (number == 1)
                return (m, m, 1);
            int j = m;
            int jlast = m;
            int ilast = n;
            BigInteger countLast = 1;
            while (j > 0)
            {
                int lastIndex = LastIndexInCountMatrix(n, m, j);
                BigInteger countForLastIndex = GetCountForPositionSaveFPBigInteger(n, m, lastIndex, j);
                if (countForLastIndex == number)
                    return (lastIndex, j, countForLastIndex);
                if (countForLastIndex > number)
                {
                    int i = j + 1;
                    while (i <= n)
                    {
                        BigInteger countForCurrentIndex = GetCountForPositionSaveFPBigInteger(n, m, i, j);
                        if (countForCurrentIndex == number)
                            return (i, j, countForCurrentIndex);
                        if (countForCurrentIndex > number)
                            return (i, j, countLast);
                        jlast = j;
                        ilast = i;
                        countLast = countForCurrentIndex;
                        i++;
                    }
                    throw new Exception("Logical error GetFirstPosition");

                }
                else
                {
                    countLast = countForLastIndex;
                }
                --j;
            }
            return (0, 0, 0L);
        }
        //--------------------------------------------------------------------------------------
        private BigInteger GetCountForPositionSaveFPBigIntegerByMatrix(int n, int m, int istart, int jstart)
        {
            if (CountForPositionMatrix[n,m,istart, jstart] != -1)
                return CountForPositionMatrix[n,m,istart, jstart];
            BigInteger result = CalculateCountForPositionSaveFPBigInteger(n, m, istart, jstart);
            CountForPositionMatrix[n, m, istart, jstart] = result;
            return result;
        }
        //--------------------------------------------------------------------------------------
        private BigInteger GetCountForPositionSaveFPBigIntegerByDictionary(int n, int m, int istart, int jstart)
        {
            BigInteger result;
            if (CountForPositionDictionary.TryGetValue((n,m,istart,jstart), out result))
                return result;
            result = CalculateCountForPositionSaveFPBigInteger(n, m, istart, jstart);
            CountForPositionDictionary.Add((n, m, istart, jstart), result);
            return result;
        }
        //--------------------------------------------------------------------------------------
        private BigInteger CalculateCountForPositionSaveFPBigInteger(int n, int m, int istart, int jstart)
        {
            BigInteger result = 0;
            if (jstart > m)
                result = 0;
            else if (istart < jstart)
                result = 0;
            else if (jstart == m)
                result = istart - jstart + 1;
            else
            {
                result = n - m + 1;
                for (int j = m - 1; j >= jstart; j--)
                {
                    int iLimit = j == jstart ? istart : LastIndexInCountMatrix(n, m, j);
                    for (int i = j + 1; i <= iLimit; i++)
                    {
                        result = BigInteger.Add(result, GetBigIntegerCombination(n - i, m - j));
                    }
                }
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        // BigInteger Inproved - Not Recursive Save first position Improve 1
        //--------------------------------------------------------------------------------------
        private int[] SkipEnumerationSaveFPImpBigInteger(int n, int m, BigInteger number, int? curnStart = null, int? curmStart = null)
        {
            int[] result = new int[m];
            BigInteger rest = number;
            int curn = n;
            int curm = m;
            int previ = 1;
            int prevj = 1;
            int lastJ = 0;
            while (rest > 0)
            {
                (int i, int j, BigInteger countForCurrentIndex) = GetFirstPositionSaveFPBigImpInteger(curn, curm, rest, m == curm ? curmStart : null);
                int ii = previ;
                for (int jj = prevj; jj < prevj - 1 + j; jj++)
                {
                    result[jj - 1] = ii++;
                }
                lastJ = prevj - 2 + j;
                result[prevj - 2 + j] = previ + i - 1;
                previ += i;
                prevj += j;
                curm -= j;
                curn -= i;
                rest -= countForCurrentIndex;
            }
            lastJ++;
            while (lastJ < m)
            {
                result[lastJ] = LastIndexInCountMatrix(n, m, lastJ + 1);
                lastJ++;
            }
            return result;
        }
        //--------------------------------------------------------------------------------------
        private (int, int, BigInteger) GetFirstPositionSaveFPBigImpInteger(int n, int m, BigInteger number, int? curmStart = null)
        {
            if (number == 1)
                return (m, m, 1);
            int j = curmStart ?? m;
            int jlast = m;
            int ilast = n;
            BigInteger countLast = 0;
            while (j > 0)
            {
                int lastIndex = LastIndexInCountMatrix(n, m, j);
                BigInteger countForLastIndex = GetCountForPositionSaveFPBigInteger(n, m, lastIndex, j);
                if (countForLastIndex == number)
                    return (lastIndex, j, countForLastIndex);
                if (countForLastIndex > number)
                {
                    int i = j + 1;
                    while (i <= n)
                    {
                        BigInteger countForCurrentIndex = GetCountForPositionSaveFPBigInteger(n, m, i, j);
                        if (countForCurrentIndex == number)
                            return (i, j, countForCurrentIndex);
                        if (countForCurrentIndex > number)
                        {
                            if (countLast == 0)
                            {
                                lastIndex = LastIndexInCountMatrix(n, m, j+1);
                                countLast = GetCountForPositionSaveFPBigInteger(n, m, lastIndex, j + 1);
                            }
                            return (i, j, countLast);
                        }
                        jlast = j;
                        ilast = i;
                        countLast = countForCurrentIndex;
                        i++;
                    }
                    throw new Exception("Logical error GetFirstPosition");

                }
                else
                {
                    countLast = countForLastIndex;
                }
                --j;
            }
            return (0, 0, 0L);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
