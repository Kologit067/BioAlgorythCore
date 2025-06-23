

namespace BioAlgorithm.Data.Contract.Representatives.Data.Contract
{
    public class RepresentativesInputDao
    {
        public long RepresentativesInputId { get; set; }
        public int NumberOfSet { get; set; }
        public int Dimension { get; set; }
        public long Step { get; set; }
        public long MaxCount { get; set; }
        public string InputLen { get; set; }
        public string InputLenSort { get; set; }
        public int InputLenAvg { get; set; }
        public string InputData { get; set; }
        public string InputDataShort { get; set; }
        public string Isomorphic { get; set; }
        public string IsomorphicBipart { get; set; }
        public string IsomorphismResult { get; set; }
        public string IsomorphismBipartResult { get; set; }
        public int TypeTask { get; set; }
        public int GreedyComparison { get; set; }
    }
}
