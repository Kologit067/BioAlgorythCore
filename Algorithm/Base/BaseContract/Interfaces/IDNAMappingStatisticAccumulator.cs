using BaseLibrary.Objects;

namespace BaseContract.Interfaces
{
    public interface IDNAMappingStatisticAccumulator
    {
        void IterationCountInc();
        void TerminalCountInc();
        void UpdateOptcountInc();
        void ElemenationCountInc();
        void CreateStatistics(string inputData, string algorithm, AlgorithmParameters algorithmParameters);
        void SaveStatisticData(string outputPresentation, long duration, long durationMilliSeconds, DateTime dateComplete,
            bool isComplete, string lastRoute, string optimalRoute, List<List<int>> listOfSolution);
        string Delete(string algorithm);
    }
}
