using Representatives.Data.Contract;

namespace BioAlgorithm.Services.Contract
{
    public interface IRepresentativeService
    {
        Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync();
        Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string order);
        Task<string> TestIsomorphismAsync(string algorithmName, int dimension, int numberOfSet, long step);
    }
}
