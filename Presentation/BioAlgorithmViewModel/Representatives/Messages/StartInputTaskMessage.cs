

namespace BioAlgorithmViewModel.Representatives.Messages
{
    public enum KindOfInputTaskEnum { Isomorphism, IsomorphismByPart, DefineTypeTask }
    public class StartInputTaskMessage
    {
        public KindOfInputTaskEnum KindOfInputTask { get; set; }
        public int? NumberOfSet { get; set; }
        public int? Dimension { get; set; }
        public long? Step { get; set; }
    }
}
