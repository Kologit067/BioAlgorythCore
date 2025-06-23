using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Utility;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithm.Data.Representatives.Data;
using Representatives.Data.Contract;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithm.Services.HittingSet;

namespace BioAlgorithmViewModel.Representatives
{
    public class HittingSetInputDataViewModel : HittingSetBaseViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<RepresentativesInputDao> inputDataList;
        public ObservableCollection<RepresentativesInputDao> InputDataList
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
        private RepresentativesInputDao selectedInputtem;
        public RepresentativesInputDao SelectedInputItem
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
        //private string taskTypeFilterType;
        //public string TaskTypeFilterType
        //{
        //    get
        //    {
        //        return taskTypeFilterType;
        //    }
        //    set
        //    {
        //        taskTypeFilterType = value;
        //        OnPropertyChanged(nameof(TaskTypeFilterType));
        //    }
        //}
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
        public HittingSetInputDataViewModel(RepresentativesRepository representativesRepository) : base(representativesRepository)
        {
            InputDataList = new ObservableCollection<RepresentativesInputDao>();
            InputDataSortItems = new List<string>()
            {
                "Dimension, NumberOfSet, Step, InputDataShort",
                "Dimension, NumberOfSet, Step, RepresentativesInputId",
                "NumberOfSet, Dimension, Step, InputDataShort",
                "NumberOfSet, Dimension, Step, RepresentativesInputId"
            };
            SelectedInputDataSort = InputDataSortItems[0];
            HittingSetFilter.TaskTypeFilterType = TaskTypeFilterTypeSource[0];
            Messenger.Default.Register<AlgorithmGroupToFilterInputDataMessage>(this, OnAlgorithmGroupToFilterInputDataMessageReceived, typeof(AlgorithmGroupToFilterInputDataMessage));
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
            RepresentativesPerfomanceFilter? representativesPerfomanceFilterDto = HittingSetFilter.Map();
            if (representativesPerfomanceFilterDto != null)
                representativesPerfomanceFilterDto.TaskTypeFilter = taskTypeFilter;
            List<RepresentativesInputDao> items = await representativesRepository.GetRepresentativeInputsAsync(representativesPerfomanceFilterDto, SelectedInputDataSort);
            foreach (RepresentativesInputDao item in items)
                InputDataList.Add(item);

            ExecutionState = "Query completed.";
            refreshRepresentativeAlgorithmGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshRepresentativeInputListAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
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
        private void OnAlgorithmGroupToFilterInputDataMessageReceived(AlgorithmGroupToFilterInputDataMessage algorithmGroupToFilterMessage)
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
            message.Step = SelectedInputItem.Step;
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
