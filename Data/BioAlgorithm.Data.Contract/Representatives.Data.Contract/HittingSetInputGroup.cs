
namespace BioAlgorithm.Data.Contract.Representatives.Data.Contract
{
    public class HittingSetInputGroup
    {
        public int NumberOfSet { get; set; }
        public int Dimension { get; set; }
        public long Step { get; set; }
        public long MaxCount { get; set; }
        public long TotalCount { get; set; }
        public long SumTypaTask { get; set; }
        public string TypeTaskRelation { get; set; }
        public long SumIsomorphic { get; set; }
        public long SumIsomorphicBipart { get; set; }
    }
}
