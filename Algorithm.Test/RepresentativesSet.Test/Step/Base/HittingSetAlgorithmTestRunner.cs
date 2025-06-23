
using BaseContract.Interfaces;
using BioAlgorithm.Services.HittingSet;

namespace RepresentativesSet.Test.Step.Base
{
    //--------------------------------------------------------------------------------------
    // class HittingSetAlgorithmTestRunner
    //--------------------------------------------------------------------------------------
    public class HittingSetAlgorithmTestRunner : HittingSetAlgorithmRunner
    {
        //--------------------------------------------------------------------------------------
        public HittingSetAlgorithmTestRunner(List<IHittingSetAlgorithm> hittingSetAlgorithms, int pCardinality, int pLength, int pMinimumValue = 1, int pForwardAdditive = 1) :
            base(hittingSetAlgorithms, pCardinality, pLength, 1000, pMinimumValue, pForwardAdditive, true)
        {

        }
        //--------------------------------------------------------------------------------------
        protected override void AssertAction()
        {
            if (hittingSetAlgorithms[0].OptimalSets != null)
            {
                for (int i = 1; i < hittingSetAlgorithms.Count; i++)
                {
                    Assert.AreEqual(hittingSetAlgorithms[0].OptimalSets.Count, hittingSetAlgorithms[i].OptimalSets.Count, "Wrong number rows in result");
                    for (int j = 0; j < hittingSetAlgorithms[0].OptimalSets.Count; j++)
                    {
                        Assert.AreEqual(hittingSetAlgorithms[0].OptimalSets[i], hittingSetAlgorithms[i].OptimalSets[j], $"Wrong string in position {i} - {hittingSetAlgorithms[0].OptimalSets[j]}. Expected - {hittingSetAlgorithms[i].OptimalSets[j]}");
                    }
                }
            }

        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
