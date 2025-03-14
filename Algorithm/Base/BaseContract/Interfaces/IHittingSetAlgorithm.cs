
namespace BaseContract.Interfaces
{
    public interface IHittingSetAlgorithm
    {
        string AlgorithmName { get; }
        IRepresentativesStatisticAccumulator StatisticAccumulator { get; set; }
        void Execute(int[][] pListOfSet);
    }
}
