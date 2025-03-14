using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using BioAlgorithm.Data.Representatives.Data;
using Representatives.Data.Contract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BioAlgorithmViewModel.Representatives
{
    public class RepresentativePerformanceViewModel : HittingSetBaseViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<RepresentativesPerfomance> representativePerformanceList;
        public ObservableCollection<RepresentativesPerfomance> RepresentativePerformanceList
        {
            get
            {
                return representativePerformanceList;
            }
            set
            {
                representativePerformanceList = value;
                OnPropertyChanged(nameof(RepresentativePerformanceList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativesPerfomance selectrdRepresentativeItem;
        public RepresentativesPerfomance SelectrdRepresentativeItem
        {
            get
            {
                return selectrdRepresentativeItem;
            }
            set
            {
                selectrdRepresentativeItem = value;
                OnPropertyChanged(nameof(SelectrdRepresentativeItem));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedRepresentativePerformanceSort;
        public string SelectedRepresentativePerformanceSort
        {
            get
            {
                return selectedRepresentativePerformanceSort;
            }
            set
            {
                selectedRepresentativePerformanceSort = value;
                OnPropertyChanged(nameof(SelectedRepresentativePerformanceSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> representativePerformanceSortItems;
        public List<string> RepresentativePerformanceSortItems
        {
            get
            {
                return representativePerformanceSortItems;
            }
            set
            {
                representativePerformanceSortItems = value;
                OnPropertyChanged(nameof(RepresentativePerformanceSortItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetFilterViewModel representativesPerfomanceFilter;
        public HittingSetFilterViewModel RepresentativesPerfomanceFilter
        {
            get
            {
                return representativesPerfomanceFilter;
            }
            set
            {
                representativesPerfomanceFilter = value;
                OnPropertyChanged(nameof(RepresentativesPerfomanceFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RepresentativePerformanceViewModel(RepresentativesRepository representativesRepository) : base(representativesRepository)
        {
            RepresentativePerformanceList = new ObservableCollection<RepresentativesPerfomance>();
            RepresentativePerformanceSortItems = new List<string>()
            {
                "Algorithm, Dimension, NumberOfSet, Step, InputDataShort",
                "Algorithm, Dimension, NumberOfSet, Step, RepresentativesPerfomanceId",
                "Algorithm, NumberOfSet, NumberOfSet, Step, InputDataShort",
                "Algorithm, NumberOfSet, NumberOfSet, Step, RepresentativesPerfomanceId",
                "Dimension, NumberOfSet, Algorithm, Step, InputDataShort",
                "Dimension, NumberOfSet, Algorithm, Step, RepresentativesPerfomanceId",
                "NumberOfSet, Dimension, Algorithm, Step, InputDataShort",
                "NumberOfSet, Dimension, Algorithm, Step, RepresentativesPerfomanceId"
            };
            SelectedRepresentativePerformanceSort = RepresentativePerformanceSortItems[0];
//            this.representativesRepository = representativesRepository;
            RepresentativesPerfomanceFilter = new HittingSetFilterViewModel();
            Messenger.Default.Register<AlgorithmToFilterMessage>(this, OnAlgorithmToFilterMessageReceived, typeof(AlgorithmToFilterMessage));
            Messenger.Default.Register<AlgorithmGroupToFilterMessage>(this, OnAlgorithmGroupToFilterMessageReceived, typeof(AlgorithmGroupToFilterMessage));
            RepresentativesPerfomanceFilter.Top = 1000;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RepresentativePerformanceViewModel(RepresentativesRepository representativesRepository, AlgorithmGroupOpenWindowMessage message)
            : this(representativesRepository)
        {
            RepresentativesPerfomanceFilter = new HittingSetFilterViewModel();
            RepresentativesPerfomanceFilter.Algorithm = message.Algorithm;
            RepresentativesPerfomanceFilter.Dimension = message.Dimension;
            RepresentativesPerfomanceFilter.NumberOfSet = message.NumberOfSet;
            RepresentativesPerfomanceFilter.MaxCount = message.Step;
            RepresentativesPerfomanceFilter.Top = 1000;
        }
        private bool refreshRepresentativeAlgorithmGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshRepresentativePerformanceListCommand;
        public ICommand RefreshRepresentativePerformanceListCommand
        {
            get
            {
                if (refreshRepresentativePerformanceListCommand == null)
                {
                    refreshRepresentativePerformanceListCommand = new DelegateCommand(RefreshRepresentativePerformanceListAction, CanRefreshRepresentativePerformanceListAction);
                }
                return refreshRepresentativePerformanceListCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshRepresentativePerformanceListAction()
        {
            ExecutionState = "Query running...";
            refreshRepresentativeAlgorithmGroupEnable = false;

            RepresentativePerformanceList.Clear();
            RepresentativesPerfomanceFilter? representativesPerfomanceFilterDto = RepresentativesPerfomanceFilter.Map();
            List<RepresentativesPerfomance> items = await representativesRepository.GetRepresentativePerformanceListAsync(representativesPerfomanceFilterDto, SelectedRepresentativePerformanceSort);
            foreach (RepresentativesPerfomance item in items)
                RepresentativePerformanceList.Add(item);

            ExecutionState = "Query completed.";
            refreshRepresentativeAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshRepresentativePerformanceListAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        ////----------------------------------------------------------------------------------------------------------------------
        //private ICommand makeGraphCommand;
        //public ICommand MakeGraphCommand
        //{
        //    get
        //    {
        //        if (makeGraphCommand == null)
        //        {
        //            makeGraphCommand = new DelegateCommand(MakeGraphAction, CanMakeGraphAction);
        //        }
        //        return makeGraphCommand;
        //    }
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private void MakeGraphAction()
        //{
        //    Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
        //    {
        //        RepresentativeTabName = "BipartiteGraph"
        //    }, typeof(RepresentativeTabChangeMessage));
        //    Messenger.Default.Send<InputDataToGraphMessage>(new InputDataToGraphMessage()
        //    {
        //        InputData = SelectrdRepresentativeItem.InputData
        //    }, typeof(InputDataToGraphMessage));
        //}
        ////----------------------------------------------------------------------------------------------------------------------
        //private bool CanMakeGraphAction()
        //{
        //    return true;
        //}
        protected override string GetGraphData()
        {
            return SelectrdRepresentativeItem.InputData;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmToFilterMessageReceived(AlgorithmToFilterMessage algorithmToFilterMessage)
        {
            RepresentativesPerfomanceFilter.Algorithm = algorithmToFilterMessage.Algorithm;
            RepresentativesPerfomanceFilter.Dimension = null;
            RepresentativesPerfomanceFilter.NumberOfSet = null;
            RepresentativesPerfomanceFilter.MaxCount = null;
            RefreshRepresentativePerformanceListAction();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnAlgorithmGroupToFilterMessageReceived(AlgorithmGroupToFilterMessage algorithmGroupToFilterMessage)
        {
            RepresentativesPerfomanceFilter.Algorithm = algorithmGroupToFilterMessage.Algorithm;
            RepresentativesPerfomanceFilter.Dimension = algorithmGroupToFilterMessage.Dimension;
            RepresentativesPerfomanceFilter.NumberOfSet = algorithmGroupToFilterMessage.NumberOfSet;
            RepresentativesPerfomanceFilter.MaxCount = algorithmGroupToFilterMessage.Step;
            RefreshRepresentativePerformanceListAction();
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
