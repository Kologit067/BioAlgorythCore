using BioAlgorithmViewModel.BipartiteGraphModel;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using BioAlgorithm.Data.Representatives.Data;
using Representatives.Data;
using System;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class RepresentativesViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class RepresentativesViewModel : ViewModelBase
    {
        private RepresentativesRepository representativesRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativePerformanceViewModel representativePerformance;
        public RepresentativePerformanceViewModel RepresentativePerformance
        {
            get
            {
                return representativePerformance;
            }
            set
            {
                representativePerformance = value;
                OnPropertyChanged(nameof(RepresentativePerformance));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativePerformanceAlgorithmViewModel representativePerformanceAlgorithm;
        public RepresentativePerformanceAlgorithmViewModel RepresentativePerformanceAlgorithm
        {
            get
            {
                return representativePerformanceAlgorithm;
            }
            set
            {
                representativePerformanceAlgorithm = value;
                OnPropertyChanged(nameof(RepresentativePerformanceAlgorithm));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativePerformanceGroupViewModel representativePerformanceGroup;
        public RepresentativePerformanceGroupViewModel RepresentativePerformanceGroup
        {
            get
            {
                return representativePerformanceGroup;
            }
            set
            {
                representativePerformanceGroup = value;
                OnPropertyChanged(nameof(RepresentativePerformanceGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputGroupViewModel hittingSetInputGroup;
        public HittingSetInputGroupViewModel HittingSetInputGroup
        {
            get
            {
                return hittingSetInputGroup;
            }
            set
            {
                hittingSetInputGroup = value;
                OnPropertyChanged(nameof(HittingSetInputGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativePerformanceAlgorithmCompareViewModel representativePerformanceAlgorithmCompare;
        public RepresentativePerformanceAlgorithmCompareViewModel RepresentativePerformanceAlgorithmCompare
        {
            get
            {
                return representativePerformanceAlgorithmCompare;
            }
            set
            {
                representativePerformanceAlgorithmCompare = value;
                OnPropertyChanged(nameof(RepresentativePerformanceAlgorithmCompare));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativePerformanceAlgorithmWithGroupViewModel representativePerformanceAlgorithmWithGroup;
        public RepresentativePerformanceAlgorithmWithGroupViewModel RepresentativePerformanceAlgorithmWithGroup
        {
            get
            {
                return representativePerformanceAlgorithmWithGroup;
            }
            set
            {
                representativePerformanceAlgorithmWithGroup = value;
                OnPropertyChanged(nameof(RepresentativePerformanceAlgorithmWithGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputDataViewModel hittingSetInput;
        public HittingSetInputDataViewModel HittingSetInput
        {
            get
            {
                return hittingSetInput;
            }
            set
            {
                hittingSetInput = value;
                OnPropertyChanged(nameof(HittingSetInput));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputGreedyComparisonViewModel hittingSetInputGreedyComparison;
        public HittingSetInputGreedyComparisonViewModel HittingSetInputGreedyComparison
        {
            get
            {
                return hittingSetInputGreedyComparison;
            }
            set
            {
                hittingSetInputGreedyComparison = value;
                OnPropertyChanged(nameof(HittingSetInputGreedyComparison));
            }
        }
        
        //----------------------------------------------------------------------------------------------------------------------
        private BipartiteGraphViewModel bipartiteGraph;
        public BipartiteGraphViewModel BipartiteGraph
        {
            get
            {
                return bipartiteGraph;
            }
            set
            {
                bipartiteGraph = value;
                OnPropertyChanged(nameof(BipartiteGraph));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private int selectedTab;
        public int SelectedTab
        {
            get
            {
                return selectedTab;
            }
            set
            {
                selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RepresentativesViewModel()
        {
            representativesRepository = new RepresentativesRepository();
            RepresentativePerformance = new RepresentativePerformanceViewModel(representativesRepository);
            RepresentativePerformanceAlgorithm = new RepresentativePerformanceAlgorithmViewModel(representativesRepository);
            RepresentativePerformanceGroup = new RepresentativePerformanceGroupViewModel(representativesRepository);
            HittingSetInputGroup = new HittingSetInputGroupViewModel(representativesRepository);
            RepresentativePerformanceAlgorithmCompare = new RepresentativePerformanceAlgorithmCompareViewModel(representativesRepository);
            RepresentativePerformanceAlgorithmWithGroup = new RepresentativePerformanceAlgorithmWithGroupViewModel(representativesRepository);
            HittingSetInput = new HittingSetInputDataViewModel(representativesRepository);
            HittingSetInputGreedyComparison = new HittingSetInputGreedyComparisonViewModel(representativesRepository);
            BipartiteGraph = new BipartiteGraphViewModel();
            Messenger.Default.Register<RepresentativeTabChangeMessage>(this, OnAlgorithmGroupToFilterMessageReceived, typeof(RepresentativeTabChangeMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmGroupToFilterMessageReceived(RepresentativeTabChangeMessage message)
        {
            SelectedTab = message.RepresentativeTabName switch
            {
                "RepresentativePerformance" => 5,
                "BipartiteGraph" => 8,
                "RepresentativePerformanceAsGroup" => 7,
                "InputData" => 3,
                "InputDataGreedyComparison" => 4,
                _ => 0
            };
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
