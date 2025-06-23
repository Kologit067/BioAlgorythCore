

using BaseLibrary.Helpers;
using RepresentativesSet.Greedy;
using StatisticsStorage.Accumulators;

namespace RepresentativesSet.Test
{
 
    //--------------------------------------------------------------------------------------
    // class EnumerateRepresentativesWithStepTestBase
    //--------------------------------------------------------------------------------------
    public class EnumerateRepresentativesWithStepTestBase
    {
        protected int _fLimit;
        protected int _fSize;
        protected int _fCardinality;
        protected int[] _fCurrentSet;
        protected bool _isSave;
        //--------------------------------------------------------------------------------------
        protected int _wrongResultCount = 0;
        public int WrongResultCount
        {
            get
            {
                return _wrongResultCount;
            }
        }
        //--------------------------------------------------------------------------------------
        protected int _wrongResultImpCount = 0;
        public int WrongResultImpCount
        {
            get
            {
                return _wrongResultImpCount;
            }
        }
        //--------------------------------------------------------------------------------------
        protected int _wrongResultImpRDCount = 0;
        public int WrongResultImpRDCount
        {
            get
            {
                return _wrongResultImpRDCount;
            }
        }
        //--------------------------------------------------------------------------------------
        protected int _gapCount = 0;
        public int GapCount
        {
            get
            {
                return _gapCount;
            }
        }
        //--------------------------------------------------------------------------------------
        protected int _oneCount = 0;
        public int OneCount
        {
            get
            {
                return _oneCount;
            }
        }
        //--------------------------------------------------------------------------------------
        protected int[][] GetAndTestListOfSet()
        {
            int[][] listOfSet = _fCurrentSet.Select(t => BruteForceRepresentativesBinaryNumbers.GetAsElementNumbers(t, _fCardinality).ToArray()).ToArray();
            int count = listOfSet.SelectMany(l => l).Distinct().Count();
            int max = listOfSet.SelectMany(l => l).Max();
            string listAsString = listOfSet.AsString();
            if (max + 1 != count)
            {
                _gapCount++;
                return null;
            }
            bool isAnyOne = listOfSet.Any(l => l.Count() == 1);
            if (isAnyOne)
            {
                _oneCount++;
                return null;
            }
            return listOfSet;
        }
        //--------------------------------------------------------------------------------------
        public virtual string ShowString
        {
            get
            {
                if (_fCurrentSet != null && _fCurrentSet.Length > 0)
                    return string.Join(",", _fCurrentSet.Select(i => i));
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
