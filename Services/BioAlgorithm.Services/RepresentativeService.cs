using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces;
using BioAlgorithm.Services.Contract;
using GraphLib;
using IsomorphismGraph;
using Representatives.Data.Contract;

namespace RepresentativeServices
{
    public class RepresentativeService : IRepresentativeService
    {
        private readonly IRepresentativesRepository representativesRepository;
        public RepresentativeService(IRepresentativesRepository representativesRepository) 
        { 
            this.representativesRepository = representativesRepository;
        }
        public async Task<string> TestIsomorphismAsync(string algorithmName, int dimension, int numberOfSet, long step)
        {
            string error = string.Empty;
            try
            {
                RepresentativesPerfomanceFilter representativesPerfomanceFilterDto = new RepresentativesPerfomanceFilter
                {
                    Algorithm = algorithmName,
                    Dimension = dimension,
                    NumberOfSet = numberOfSet,
                    Step = step
                };
                await representativesRepository.ClearIsomorphicAsync(representativesPerfomanceFilterDto);
                List<RepresentativesInput> items = await representativesRepository.GetRepresentativeInputsAsync(representativesPerfomanceFilterDto, "InputDataShort");
                items.ForEach(item =>
                {
                    item.Isomorphic = null;
                });
                for (int i = 0; i < items.Count; i++)
                {
                    RepresentativesInput itemOut = items[i];
                    if (itemOut.Isomorphic == null)
                    {
                        for (int j = i; j < items.Count; j++)
                        {
                            RepresentativesInput itemIn = items[j];
                            if (itemIn.Isomorphic == null)
                            {
                                bool result = true;
                                if (i != j)
                                {
                                    MultiGraph graph1 = new MultiGraph(itemOut.InputData);
                                    MultiGraph graph2 = new MultiGraph(itemIn.InputData);
                                    IsomorphismMultiGraph algorithm = new IsomorphismMultiGraph(graph1, graph2);
                                    result = algorithm.IsIsomorphic();
                                    if (result)
                                    {
                                        itemIn.Isomorphic = itemOut.InputData;
                                        error = await representativesRepository.UpdateIsomorphicAsync(itemIn.RepresentativesInputId, itemOut.InputData, false);
                                    }
                                }
                                else
                                    error = await representativesRepository.UpdateIsomorphicAsync(itemIn.RepresentativesInputId, itemIn.InputData, false);
                            }
                            if (!string.IsNullOrEmpty(error))
                            {
                                return error;
                            }
                        }
                    }
                }
                error = await representativesRepository.CompleteUpdateIsomorphicAsync(false);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return error;
        }

        public async Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync()
        {
            return await representativesRepository.GetAlgorithmsAsync();
        }
        public async Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string order)
        {
            return await representativesRepository.GetRepresentativeAlgorithmGroupDimensionsAsync(order);
        }
    }
}
