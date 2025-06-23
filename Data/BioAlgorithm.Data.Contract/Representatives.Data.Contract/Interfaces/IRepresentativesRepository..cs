using Representatives.Data.Contract;

namespace BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces
{
    public interface IRepresentativesRepository
    {
        Task<string> DeleteRepresentativeAlgorithmGroupAsync(RepresentativeAlgorithmGroupDimension selectedAlgorithmGroup);

        Task<string> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? step = null);


        Task<string> DeleteRepresentativeAlgorithmAsync(RepresentativeAlgorithmGroup selectedAlgorithm);
        Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string algorithmGroupListSort);


        Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync();

        Task<List<RepresentativeAlgorithWithDimension>> GetRepresentativeAlgorithmWithDimensionsAsync();

        Task<List<RepresentativesPerfomance>> GetRepresentativePerformanceListAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order);


        Task<List<RepresentativesPerfomanceCompare>> GetRepresentativePerformanceCompareListAsync(RepresentativesPerfomanceCompareFilter representativesPerfomanceCompareFilter);
        Task<string> UpdateIsomorphicAsync(long representativesInputId, string inputData, string result, bool isBipart = false);
        Task<string> UpdateTypeTaskAsync(long representativesInputId, int typeTask);

        Task<string> CompleteUpdateIsomorphicAsync(bool isBipart = false);
        Task<string> CompleteUpdateTypeTaskAsync();

        Task ClearIsomorphicAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, bool isBipart = false);
        Task<List<RepresentativesInputDao>> GetRepresentativeInputsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order);

        Task ClearTaskTypeAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter);
        Task<List<(string Algorithname, int Count)>> GetGeedyAlgorithmGroupDimensionsAsync(int dimension, int numberOfSet, long maxCount);
        Task SetGeedyComparisonAsync(int dimension, int numberOfSet, long maxCount);
        Task<List<InputDataGreedyComparison>> GetInputDataGreedyComparisonsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order);
    }
}
