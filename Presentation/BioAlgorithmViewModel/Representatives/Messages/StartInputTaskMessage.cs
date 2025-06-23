

namespace BioAlgorithmViewModel.Representatives.Messages
{
    public enum KindOfInputTaskEnum { Isomorphism, IsomorphismByPart, DefineTypeTask, DefineGreedyComparison }
    public class StartInputTaskMessage
    {
        public KindOfInputTaskEnum KindOfInputTask { get; set; }
        public int? NumberOfSet { get; set; }
        public int? Dimension { get; set; }
        public long? Step { get; set; }
        public long? MaxCount { get; set; }
    }
}
