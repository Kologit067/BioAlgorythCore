using BaseContract.Interfaces;
using BioAlgorithm.Services.HittingSet;

namespace RepresentativesSet.Test.Step.Base
{
    //--------------------------------------------------------------------------------------
    // class HittingSetAlgorithmGreedyStepTestRunner
    //--------------------------------------------------------------------------------------
    public class HittingSetAlgorithmGreedyStepTestRunner : HittingSetAlgorithmStepRunner
    {
        public HittingSetAlgorithmGreedyStepTestRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, string calculationStep,
    string combinationType, int pCardinality, int pLength, long maxCount, int bufferSize, int pMinimumValue = 1,
    int pForwardAdditive = 1, bool isSave = true) : base (hittingSetAlgorithms, calculationStep,
    combinationType, pCardinality, pLength, maxCount, bufferSize, pMinimumValue, pForwardAdditive, isSave)
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
