
using BioAlgorithmViewModel.Interfaces;

namespace BioAlgorithmViewModel.Representatives.Messages
{
    public enum DeleteAlgorithmInputTypeEnum { Algorithm, Input, AlgorithmInput }
    public class DeleteAlgorithmInputMessage
    {
        public DeleteAlgorithmInputTypeEnum DeleteAlgorithmInputType {  get; set; }
        public IInputAlgorithmViewModel InputAlgorithmViewModel { get; set; }
        public string Algorithm { get; set; }
        public int NumberOfSet { get; set; }
        public int Dimension { get; set; }
        public long MaxCount { get; set; }
    }
}
