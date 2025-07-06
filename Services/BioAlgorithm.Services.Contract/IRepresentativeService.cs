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
        Task ExecuteAlgorithmAsync(List<(string algorithm, string algorithmDetail)> algorithms, int dimension, int numberOfSet, bool isInputForce);
        Task ExecuteAlgorithmStepAsync(List<(string algorithm, string algorithmDetail)> algorithms, string CalculationStep, string CombinationType,
            int dimension, int numberOfSet, long maxCount, bool isInputForce);
        Task<List<RepresentativesInputDao>> GetRepresentativeInputsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string SelectedInputDataSort);
   }
}
