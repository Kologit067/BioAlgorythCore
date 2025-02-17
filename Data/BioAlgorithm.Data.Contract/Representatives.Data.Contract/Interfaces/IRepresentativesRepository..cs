using Representatives.Data.Contract;

namespace BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces
{
    public interface IRepresentativesRepository
    {
        Task DeleteRepresentativeAlgorithmGroupAsync(RepresentativeAlgorithmGroupDimension selectedAlgorithmGroup);

        Task<string> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? step = null);


        Task DeleteRepresentativeAlgorithmAsync(RepresentativeAlgorithmGroup selectedAlgorithm);
        Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string algorithmGroupListSort);


        Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync();

        Task<List<RepresentativeAlgorithWithDimension>> GetRepresentativeAlgorithmWithDimensionsAsync();

        Task<List<RepresentativesPerfomance>> GetRepresentativePerformanceListAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order);


        Task<List<RepresentativesPerfomanceCompare>> GetRepresentativePerformanceCompareListAsync(RepresentativesPerfomanceCompareFilter representativesPerfomanceCompareFilter);
        Task<string> UpdateIsomorphicAsync(long representativesInputId, string inputData, bool isBipart = false);

        Task<string> CompleteUpdateIsomorphicAsync(bool isBipart = false);

        Task ClearIsomorphicAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, bool isBipart = false);
        Task<List<RepresentativesInput>> GetRepresentativeInputsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order);

    }
}
