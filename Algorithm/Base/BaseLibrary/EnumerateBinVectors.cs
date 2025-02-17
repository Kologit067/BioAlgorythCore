namespace BaseLibrary
{
    public class EnumerateBinVectors(int pLength) : EnumerateIntegerFullSet(1, pLength, 0)
    {
    }
    public class EnumerateReverseBinVectors(int pLength) : EnumerateIntegerFullSet(1, pLength, 0)
    {

        //--------------------------------------------------------------------------------------
        protected override int FirstElement(int pPosition)
        {
            return _fLimit;
        }
        //--------------------------------------------------------------------------------------
        protected override bool NextElement(int pPosition)
        {
            if (_fCurrentSet[pPosition] <= _fMinimumValue)
                return false;
            _fCurrentSet[pPosition]--;
            return true;
        }
    }
}
