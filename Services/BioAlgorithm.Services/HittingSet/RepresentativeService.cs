using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces;
using BioAlgorithm.Services.Contract;
using GraphLib;
using IsomorphismGraph;
using Representatives.Data.Contract;
using BioAlgorithmModel.BipartiteGraphModel;
using BaseContract.Interfaces;
using RepresentativesSet.BranchAndBound;
using RepresentativesSet;
using RepresentativesSet.BinaryTreeEnumeration;
using RepresentativesSet.Greedy;
using RepresentativesSet.TriangleEnumeration;
using RepresentativesSet.TriangleEnumeration.SelectElement;

namespace BioAlgorithm.Services.HittingSet
{
    public class RepresentativeService : IRepresentativeService
    {
        private readonly IRepresentativesRepository representativesRepository;
        public RepresentativeService(IRepresentativesRepository representativesRepository)
        {
            this.representativesRepository = representativesRepository;
        }
        public async Task<string> TestIsomorphismAsync(string algorithmName, int dimension, int numberOfSet, long step, bool isBipart = false)
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
                await representativesRepository.ClearIsomorphicAsync(representativesPerfomanceFilterDto, isBipart);
                List<RepresentativesInput> items = await representativesRepository.GetRepresentativeInputsAsync(representativesPerfomanceFilterDto, "InputDataShort");
                if (!isBipart)
                {
                    items.ForEach(item =>
                    {
                        item.Isomorphic = null;
                        item.IsomorphismResult = null;
                    });
                }
                else
                {
                    items.ForEach(item =>
                    {
                        item.Isomorphic = null;
                        item.IsomorphismBipartResult = null;
                    });
                }
                for (int i = 0; i < items.Count; i++)
                {
                    RepresentativesInput itemOut = items[i];
                    MultiGraph graph1 = new MultiGraph(itemOut.InputData);
                    BipartiteGraph bipartiteGraph1 = new BipartiteGraph(itemOut.InputData);
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
                                    MultiGraph graph2 = new MultiGraph(itemIn.InputData);
                                    BipartiteGraph bipartiteGraph2 = new BipartiteGraph(itemIn.InputData);
                                    if (!isBipart)
                                    {
                                        IsomorphismMultiGraph algorithm = new IsomorphismMultiGraph(graph1, graph2);
                                        result = algorithm.IsIsomorphic();
                                        if (result)
                                        {
                                            itemIn.Isomorphic = itemOut.InputData;
                                            error = await representativesRepository.UpdateIsomorphicAsync(itemIn.RepresentativesInputId, itemOut.InputData, algorithm.ShowFullString, false);
                                        }
                                    }
                                    else
                                    {
                                        IsomorphismBipartGraph algorithm = new IsomorphismBipartGraph(bipartiteGraph1, bipartiteGraph2);
                                        result = algorithm.IsIsomorphic();
                                        if (result)
                                        {
                                            itemIn.Isomorphic = itemOut.InputData;
                                            error = await representativesRepository.UpdateIsomorphicAsync(itemIn.RepresentativesInputId, itemOut.InputData, algorithm.ShowFullString, true);
                                        }
                                    }
                                }
                                else
                                    error = await representativesRepository.UpdateIsomorphicAsync(itemIn.RepresentativesInputId, itemIn.InputData, "", false);
                            }
                            if (!string.IsNullOrEmpty(error))
                            {
                                return error;
                            }
                        }
                    }
                }
                error = await representativesRepository.CompleteUpdateIsomorphicAsync(isBipart);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return error;
        }

        public async Task<string> DefineTaskTypeAsync(string algorithmName, int dimension, int numberOfSet, long step)
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
                await representativesRepository.ClearTaskTypeAsync(representativesPerfomanceFilterDto);
                List<RepresentativesInput> items = await representativesRepository.GetRepresentativeInputsAsync(representativesPerfomanceFilterDto, "InputDataShort");
                items.ForEach(item =>
                {
                    item.TypeTask = 0;
                });
                for (int i = 0; i < items.Count; i++)
                {
                    RepresentativesInput itemOut = items[i];
                    MultiGraph graph = new MultiGraph(itemOut.InputData);
//                    BipartiteGraph bipartiteGraph1 = new BipartiteGraph(itemOut.InputData);
                    int taskType = graph.DefineType();
                    items[i].TypeTask = taskType;
                    error = await representativesRepository.UpdateTypeTaskAsync(items[i].RepresentativesInputId, items[i].TypeTask);
                     if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                }
                error = await representativesRepository.CompleteUpdateTypeTaskAsync();
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

        public async Task ExecuteAlgorithmAsync(string algorithm, string algorithmDetail, int dimension, int numberOfSet, long maxCount)
        {
            int сardinality = dimension;
            int length = numberOfSet;
            IHittingSetAlgorithm hittingSetAlgorithm = new {algorithm, algorithmDetail } switch
            {
                { algorithm: "BruteForceRepresentativesBinaryNumbers", algorithmDetail: _ }  => new BruteForceRepresentativesBinaryNumbers(),
                { algorithm: "BruteForceRepresentativesBinaryNumbersVer2", algorithmDetail: _ } => new BruteForceRepresentativesBinaryNumbersVer2(),
                { algorithm: "BruteForceRepresentativesAsTree", algorithmDetail: _ } => new BruteForceRepresentativesAsTree(dimension),
                { algorithm: "BruteForceRepresentativesAsTreeDirect", algorithmDetail: _ } => new BruteForceRepresentativesAsTreeDirect(dimension),
                { algorithm: "RepresentativesBranchAndBound", algorithmDetail: _ } => new RepresentativesBranchAndBound(dimension),
                { algorithm: "RepresentativesBranchAndBoundByValue", algorithmDetail: _ } => new RepresentativesBranchAndBoundByValue(dimension),
                { algorithm: "RepresentativesBranchAndBoundFirst", algorithmDetail: _ } => new RepresentativesBranchAndBoundFirst(dimension),
                { algorithm: "RepresentativesGreedySimple", algorithmDetail: _ } => new RepresentativesGreedySimple(),
                { algorithm: "RepresentativesGreedyImprove", algorithmDetail: _ } => new RepresentativesGreedyImprove(),
                { algorithm: "RepresentativesGreedyRelation", algorithmDetail: _ } => new RepresentativesGreedyRelation(),
                { algorithm: "RepresentativesGreedyImproveRD", algorithmDetail: _ } => new RepresentativesGreedyImproveRD(),
                { algorithm: "RepresentativesTriangleBranchAndBound", algorithmDetail: _ } => new RepresentativesTriangleBranchAndBound(dimension),
                { algorithm: "RepresentativesTriangle", algorithmDetail: _ } => new RepresentativesTriangle(dimension),
                { algorithm: "RepresentativesTriangleStrategy", algorithmDetail: "SelectElementSimpleStrategy" } => new RepresentativesTriangleStrategy(dimension, new SelectElementSimpleStrategy()),
                { algorithm: "RepresentativesTriangleStrategy", algorithmDetail: "SelectElementRelationStrategy" } => new RepresentativesTriangleStrategy(dimension, new SelectElementRelationStrategy()),
                { algorithm: "RepresentativesTriangleStrategy", algorithmDetail: "SelectElementImproveStrategy" } => new RepresentativesTriangleStrategy(dimension, new SelectElementImproveStrategy()),
                { algorithm: "RepresentativesTriangleStrategy", algorithmDetail: "SelectElementImproveRDStrategy" } => new RepresentativesTriangleStrategy(dimension, new SelectElementImproveRDStrategy()),
};
            if (maxCount == 1)
            {
                HittingSetAlgorithmRunner enumeration = new HittingSetAlgorithmRunner(hittingSetAlgorithm, сardinality, length, 1000);
                await enumeration.ExecuteAsync();
            }
            else
            {
                HittingSetAlgorithmStepRunner enumerationStep = new HittingSetAlgorithmStepRunner(hittingSetAlgorithm, сardinality, length, maxCount, 1000);
                await enumerationStep.ExecuteAsync();
            }
        }
    }
}
