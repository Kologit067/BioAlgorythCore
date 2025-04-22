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
        private ObservableCollection<RepresentativesInput> inputDataList;
        public ObservableCollection<RepresentativesInput> InputDataList
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
        private RepresentativesInput selectedInputtem;
        public RepresentativesInput SelectedInputItem
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
        public HittingSetInputDataViewModel(RepresentativesRepository representativesRepository) : base(representativesRepository)
        {
            InputDataList = new ObservableCollection<RepresentativesInput>();
            InputDataSortItems = new List<string>()
            {
                "Dimension, NumberOfSet, Step, InputDataShort",
                "Dimension, NumberOfSet, Step, RepresentativesInputId",
                "NumberOfSet, Dimension, Step, InputDataShort",
                "NumberOfSet, Dimension, Step, RepresentativesInputId"
            };
            SelectedInputDataSort = InputDataSortItems[0];
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
            RepresentativesPerfomanceFilter? representativesPerfomanceFilterDto = HittingSetFilter.Map();
            List<RepresentativesInput> items = await representativesRepository.GetRepresentativeInputsAsync(representativesPerfomanceFilterDto, SelectedInputDataSort);
            foreach (RepresentativesInput item in items)
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
                SelectedInputItem.Step, false);
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
                SelectedInputItem.Step, true);
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
            HittingSetFilter.Step = algorithmGroupToFilterMessage.Step;
            RefreshRepresentativeInputListAction();
        }
        protected override void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {
            message.Dimension = SelectedInputItem.Dimension;
            message.NumberOfSet = SelectedInputItem.NumberOfSet;
            message.Step = SelectedInputItem.Step;
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
                Step = null
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
                Step = null
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
                Step = null
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunDefineTypeTaskAction()
        {
            return refreshRepresentativeAlgorithmGroupEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
