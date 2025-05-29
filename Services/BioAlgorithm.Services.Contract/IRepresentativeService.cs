using System.Threading.Tasks;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using Representatives.Data.Contract;

namespace BioAlgorithm.Services.Contract
{
    public interface IRepresentativeService
    {
        Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync();
        Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string order);
        Task<string> TestIsomorphismAsync(string algorithmName, int dimension, int numberOfSet, long step, bool isBipart = false);
        Task<string> DefineTaskTypeAsync(string algorithmName, int dimension, int numberOfSet, long step);
        Task ExecuteAlgorithmAsync(string algorithm, string algorithmDetail, int dimension, int numberOfSet);
        Task ExecuteAlgorithmStepAsync(string algorithm, string algorithmDetail, string CalculationStep, string CombinationType,
            int dimension, int numberOfSet, long maxCount);
        Task<List<RepresentativesInput>> GetRepresentativeInputsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string SelectedInputDataSort);
   }
}
