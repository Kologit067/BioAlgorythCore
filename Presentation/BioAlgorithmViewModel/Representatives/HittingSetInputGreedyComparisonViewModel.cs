using System.Collections.ObjectModel;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using Representatives.Data.Contract;
using System.Windows.Input;
using RepresentativesSet.Greedy;

namespace BioAlgorithmViewModel.Representatives
{
    public class HittingSetInputGreedyComparisonViewModel : HittingSetBaseViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<InputDataGreedyComparison> inputDataList;
        public ObservableCollection<InputDataGreedyComparison> InputDataList
        {
            get
            {
                return inputDataList;
            }
            set
            {
                inputDataList = value;
                OnPropertyChanged(nameof(InputDataList));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private InputDataGreedyComparison selectedInputtem;
        public InputDataGreedyComparison SelectedInputItem
        {
            get
            {
                return selectedInputtem;
            }
            set
            {
                selectedInputtem = value;
                OnPropertyChanged(nameof(SelectedInputItem));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string selectedInputDataSort;
        public string SelectedInputDataSort
        {
            get
            {
                return selectedInputDataSort;
            }
            set
            {
                selectedInputDataSort = value;
                OnPropertyChanged(nameof(SelectedInputDataSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> inputDataSortItems;
        public List<string> InputDataSortItems
        {
            get
            {
                return inputDataSortItems;
            }
            set
            {
                inputDataSortItems = value;
                OnPropertyChanged(nameof(InputDataSortItems));
            }
        }
        //---------------------------------------------------------------------------------------------------------------------- 
        public List<string> TaskTypeFilterTypeSource
        {
            get => new List<string>() { "Or", "And", "Exact =", "Or/Not", "And/Not", "Exact =/Not" };
        }
        //---------------------------------------------------------------------------------------------------------------------- 
        public List<string> GreedyComparisonFilterTypeSource
        {
            get => new List<string>() { "Or", "And", "Exact =", "Or/Not", "And/Not", "Exact =/Not" };
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter1;
        public bool TaskTypeFilter1
        {
            get
            {
                return taskTypeFilter1;
            }
            set
            {
                taskTypeFilter1 = value;
                OnPropertyChanged(nameof(TaskTypeFilter1));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter2;
        public bool TaskTypeFilter2
        {
            get
            {
                return taskTypeFilter2;
            }
            set
            {
                taskTypeFilter2 = value;
                OnPropertyChanged(nameof(TaskTypeFilter2));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter4;
        public bool TaskTypeFilter4
        {
            get
            {
                return taskTypeFilter4;
            }
            set
            {
                taskTypeFilter4 = value;
                OnPropertyChanged(nameof(TaskTypeFilter4));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter8;
        public bool TaskTypeFilter8
        {
            get
            {
                return taskTypeFilter8;
            }
            set
            {
                taskTypeFilter8 = value;
                OnPropertyChanged(nameof(TaskTypeFilter8));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter16;
        public bool TaskTypeFilter16
        {
            get
            {
                return taskTypeFilter16;
            }
            set
            {
                taskTypeFilter16 = value;
                OnPropertyChanged(nameof(TaskTypeFilter16));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool taskTypeFilter32;
        public bool TaskTypeFilter32
        {
            get
            {
                return taskTypeFilter32;
            }
            set
            {
                taskTypeFilter32 = value;
                OnPropertyChanged(nameof(TaskTypeFilter32));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private bool greedySimpleFilter;
        public bool GreedySimpleFilter
        {
            get
            {
                return greedySimpleFilter;
            }
            set
            {
                greedySimpleFilter = value;
                OnPropertyChanged(nameof(GreedySimpleFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyRelationFilter;
        public bool GreedyRelationFilter
        {
            get
            {
                return greedyRelationFilter;
            }
            set
            {
                greedyRelationFilter = value;
                OnPropertyChanged(nameof(GreedyRelationFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyImproveFilter;
        public bool GreedyImproveFilter
        {
            get
            {
                return greedyImproveFilter;
            }
            set
            {
                greedyImproveFilter = value;
                OnPropertyChanged(nameof(GreedyImproveFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool greedyImproveRDFilter;
        public bool GreedyImproveRDFilter
        {
            get
            {
                return greedyImproveRDFilter;
            }
            set
            {
                greedyImproveRDFilter = value;
                OnPropertyChanged(nameof(GreedyImproveRDFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetInputGreedyComparisonViewModel(RepresentativesRepository representativesRepository) : base(representativesRepository)
        {
            InputDataList = new ObservableCollection<InputDataGreedyComparison>();
            InputDataSortItems = new List<string>()
            {
                "RepresentativesInputId, InputDataShort",
                "InputDataShort, RepresentativesInputId"
            };
            SelectedInputDataSort = InputDataSortItems[0];
            HittingSetFilter.TaskTypeFilterType = TaskTypeFilterTypeSource[0];
            HittingSetFilter.GreedyComparisonFilterType = GreedyComparisonFilterTypeSource[0];
            Messenger.Default.Register<InputDataGreedyComparisonToFilterMessage>(this, OnInputDataGreedyComparisonToFilterMessageReceived, typeof(InputDataGreedyComparisonToFilterMessage));
        }
        private bool refreshRepresentativeAlgorithmGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshRepresentativeInputListCommand;
        public ICommand RefreshRepresentativeInputListCommand
        {
            get
            {
                if (refreshRepresentativeInputListCommand == null)
                {
                    refreshRepresentativeInputListCommand = new DelegateCommand(RefreshRepresentativeInputListAction, CanRefreshRepresentativeInputListAction);
                }
                return refreshRepresentativeInputListCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshRepresentativeInputListAction()
        {
            ExecutionState = "Query running...";
            refreshRepresentativeAlgorithmGroupEnable = false;

            InputDataList.Clear();

            int taskTypeFilter = 0;
            if (TaskTypeFilter1)
                taskTypeFilter = taskTypeFilter | 1;
            if (TaskTypeFilter2)
                taskTypeFilter = taskTypeFilter | 2;
            if (TaskTypeFilter4)
                taskTypeFilter = taskTypeFilter | 4;
            if (TaskTypeFilter8)
                taskTypeFilter = taskTypeFilter | 8;
            if (TaskTypeFilter16)
                taskTypeFilter = taskTypeFilter | 16;
            if (TaskTypeFilter32)
                taskTypeFilter = taskTypeFilter | 32;

            int greedyComparisonFilter = 0;
            if (greedyImproveRDFilter)
                greedyComparisonFilter = greedyComparisonFilter | 1;
            if (greedyImproveFilter)
                greedyComparisonFilter = greedyComparisonFilter | 2;
            if (GreedyRelationFilter)
                greedyComparisonFilter = greedyComparisonFilter | 4;
            if (GreedySimpleFilter)
                greedyComparisonFilter = greedyComparisonFilter | 8;

            RepresentativesPerfomanceFilter? representativesPerfomanceFilter = HittingSetFilter.Map();
            if (representativesPerfomanceFilter != null)
            {
                representativesPerfomanceFilter.TaskTypeFilter = taskTypeFilter;
                representativesPerfomanceFilter.GreedyComparisonFilter = greedyComparisonFilter;
            }

            List<InputDataGreedyComparison> items = await representativesRepository.GetInputDataGreedyComparisonsAsync(representativesPerfomanceFilter, SelectedInputDataSort);
            foreach (InputDataGreedyComparison item in items)
                InputDataList.Add(item);

            ExecutionState = "Query completed.";
            refreshRepresentativeAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshRepresentativeInputListAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable && HittingSetFilter.Dimension != null && HittingSetFilter.NumberOfSet != null && HittingSetFilter.MaxCount != null;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool testIsomorphismEnable = true;
        private ICommand testIsomorphismCommand;
        public ICommand TestIsomorphismCommand
        {
            get
            {
                if (testIsomorphismCommand == null)
                {
                    testIsomorphismCommand = new DelegateCommand(TestIsomorphismAction, CanTestIsomorphismAction);
                }
                return testIsomorphismCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void TestIsomorphismAction()
        {
            ExecutionState = "Test calculation running...";
            testIsomorphismEnable = false;

            RepresentativeService representativeService = new RepresentativeService(representativesRepository);
            string error = await representativeService.TestIsomorphismAsync(string.Empty,
                SelectedInputItem.Dimension,
                SelectedInputItem.NumberOfSet,
                SelectedInputItem.MaxCount, false);
            ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
            testIsomorphismEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanTestIsomorphismAction()
        {
            return testIsomorphismEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand testIsomorphismBipartCommand;
        public ICommand TestIsomorphismBipartCommand
        {
            get
            {
                if (testIsomorphismBipartCommand == null)
                {
                    testIsomorphismBipartCommand = new DelegateCommand(TestIsomorphismBipartAction, CanTestIsomorphismBipartAction);
                }
                return testIsomorphismBipartCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void TestIsomorphismBipartAction()
        {
            ExecutionState = "Test calculation running...";
            testIsomorphismEnable = false;

            RepresentativeService representativeService = new RepresentativeService(representativesRepository);
            string error = await representativeService.TestIsomorphismAsync(string.Empty,
                SelectedInputItem.Dimension,
                SelectedInputItem.NumberOfSet,
                SelectedInputItem.MaxCount, true);
            ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
            testIsomorphismEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanTestIsomorphismBipartAction()
        {
            return testIsomorphismEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OnInputDataGreedyComparisonToFilterMessageReceived(InputDataGreedyComparisonToFilterMessage algorithmGroupToFilterMessage)
        {
            HittingSetFilter.Dimension = algorithmGroupToFilterMessage.Dimension;
            HittingSetFilter.NumberOfSet = algorithmGroupToFilterMessage.NumberOfSet;
            HittingSetFilter.MaxCount = algorithmGroupToFilterMessage.MaxCount;
            RefreshRepresentativeInputListAction();
        }
        protected override void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {
            message.Dimension = SelectedInputItem.Dimension;
            message.NumberOfSet = SelectedInputItem.NumberOfSet;
            message.MaxCount = SelectedInputItem.MaxCount;
        }
        protected override string GetGraphData()
        {
            return SelectedInputItem.InputData;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskCommand;
        public ICommand RunTaskCommand
        {
            get
            {
                if (runTaskCommand == null)
                {
                    runTaskCommand = new DelegateCommand(RunTaskAction, CanRunTaskAction);
                }
                return runTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskAction()
        {
            // RepresentativeAlgorithmWindow window
            Messenger.Default.Send<StartTaskMessage>(new StartTaskMessage()
            {
                Algorithm = null,
                Dimension = null,
                NumberOfSet = null,
                MaxCount = null
            }, typeof(StartTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskStepCommand;
        public ICommand RunTaskStepCommand
        {
            get
            {
                if (runTaskStepCommand == null)
                {
                    runTaskStepCommand = new DelegateCommand(RunTaskStepAction, CanRunTaskStepAction);
                }
                return runTaskStepCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskStepAction()
        {
            // RepresentativeAlgorithmWindow window
            Messenger.Default.Send<StartTaskStepMessage>(new StartTaskStepMessage()
            {
                Algorithm = null,
                Dimension = null,
                NumberOfSet = null,
                MaxCount = null
            }, typeof(StartTaskStepMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskStepAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismTaskCommand;
        public ICommand RunIsomorphismTaskCommand
        {
            get
            {
                if (runIsomorphismTaskCommand == null)
                {
                    runIsomorphismTaskCommand = new DelegateCommand(RunIsomorphismTaskAction, CanRunIsomorphismTaskAction);
                }
                return runIsomorphismTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.Isomorphism,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismBipartTaskCommand;
        public ICommand RunIsomorphismBipartTaskCommand
        {
            get
            {
                if (runIsomorphismBipartTaskCommand == null)
                {
                    runIsomorphismBipartTaskCommand = new DelegateCommand(RunIsomorphismBipartTaskAction, CanRunIsomorphismBipartTaskAction);
                }
                return runIsomorphismBipartTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismBipartTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.IsomorphismByPart,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismBipartTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runDefineTypeTaskCommand;
        public ICommand RunDefineTypeTaskCommand
        {
            get
            {
                if (runDefineTypeTaskCommand == null)
                {
                    runDefineTypeTaskCommand = new DelegateCommand(RunDefineTypeTaskAction, CanRunDefineTypeTaskAction);
                }
                return runDefineTypeTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunDefineTypeTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineTypeTask,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunDefineTypeTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runDefineGreedyComparisonCommand;
        public ICommand RunDefineGreedyComparisonCommand
        {
            get
            {
                if (runDefineGreedyComparisonCommand == null)
                {
                    runDefineGreedyComparisonCommand = new DelegateCommand(RunDefineGreedyComparisonAction, CanRunGreedyComparisonTaskAction);
                }
                return runDefineGreedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunDefineGreedyComparisonAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineGreedyComparison,
                Dimension = null,
                NumberOfSet = null,
                Step = null,
                MaxCount = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunGreedyComparisonTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }

}
