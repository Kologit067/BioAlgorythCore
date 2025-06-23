using BaseContract.Interfaces;
using BioAlgorithm.Services.HittingSet;

namespace RepresentativesSet.Test.Step.Base
{
    //--------------------------------------------------------------------------------------
    // class HittingSetAlgorithmGreedyTestRunner
    //--------------------------------------------------------------------------------------
    public class HittingSetAlgorithmGreedyTestRunner : HittingSetAlgorithmRunner
    {
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmGreedyTestRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, int pCardinality, int pLength, int pMinimumValue = 1, int pForwardAdditive = 1) : 
            base(hittingSetAlgorithms, pCardinality, pLength, 1000, pMinimumValue, pForwardAdditive, true)
        {

        }
        //--------------------------------------------------------------------------------------
        protected override void AssertAction()
        {
            for (int i = 1; i < hittingSetAlgorithms.Count; i++)
            {
                if (hittingSetAlgorithms[0].CurrentMinimum == hittingSetAlgorithms[i].CurrentMinimum)
                {
                    if (hittingSetAlgorithms[0].OptimalSets != null)
                    {
                        string solutionAsString = hittingSetAlgorithms[i].SolutionAsString;
                        Assert.IsTrue(hittingSetAlgorithms[0].OptimalSets.Any(o => o == solutionAsString));
                    }
                }
                else
                {
                    _wrongResultCount++;
                }

            }

        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
