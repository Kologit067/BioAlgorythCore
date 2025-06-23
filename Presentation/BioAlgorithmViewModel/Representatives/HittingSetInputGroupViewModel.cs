using System.Collections.ObjectModel;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using System.Windows.Input;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithmViewModel.Interfaces;
using Representatives.Data.Contract;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetInputGroupViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetInputGroupViewModel : HittingSetBaseViewModel, IInputAlgorithmViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<HittingSetInputGroup> hittingSetInputGroups;
        public ObservableCollection<HittingSetInputGroup> HittingSetInputGroups
        {
            get
            {
                return hittingSetInputGroups;
            }
            set
            {
                hittingSetInputGroups = value;
                OnPropertyChanged(nameof(HittingSetInputGroups));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetInputGroup selectedHittingSetInputGroup;
        public HittingSetInputGroup SelectedHittingSetInputGroup
        {
            get
            {
                return selectedHittingSetInputGroup;
            }
            set
            {
                selectedHittingSetInputGroup = value;
                OnPropertyChanged(nameof(SelectedHittingSetInputGroup));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private string hittingSetInputGroupSort;
        public string HittingSetInputGroupSort
        {
            get
            {
                return hittingSetInputGroupSort;
            }
            set
            {
                hittingSetInputGroupSort = value;
                OnPropertyChanged(nameof(HittingSetInputGroupSort));
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        private List<string> hittingSetInputGroupSortItems;
        public List<string> HittingSetInputGroupSortItems
        {
            get
            {
                return hittingSetInputGroupSortItems;
            }
            set
            {
                hittingSetInputGroupSortItems = value;
                OnPropertyChanged(nameof(HittingSetInputGroupSortItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetInputGroupViewModel(RepresentativesRepository representativesRepository) : base(representativesRepository)
        {
            HittingSetInputGroups = new ObservableCollection<HittingSetInputGroup>();
            HittingSetInputGroupSortItems = new List<string>()
            {
                "Dimension, NumberOfSet",
                "NumberOfSet, Dimension"
            };
            HittingSetInputGroupSort = HittingSetInputGroupSortItems[0];
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshHittingSetGroupListCommand;
        public ICommand RefreshHittingSetGroupListCommand
        {
            get
            {
                if (refreshHittingSetGroupListCommand == null)
                {
                    refreshHittingSetGroupListCommand = new DelegateCommand(RefreshHittingSetInputGroupListAction, CanRefreshHittingSetInputGroupListAction);
                }
                return refreshHittingSetGroupListCommand;
            }
        }
        private bool refreshHittingSetInputGroupEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshHittingSetInputGroupListAction()
        {
            ExecutionState = "Query running...";
            refreshHittingSetInputGroupEnable = false;

            HittingSetInputGroups.Clear();
            List<HittingSetInputGroup> algorithms = await representativesRepository.GetHittingSetInputGroupAsync(HittingSetInputGroupSort);
            foreach (HittingSetInputGroup a in algorithms)
                HittingSetInputGroups.Add(a);
            ExecutionState = "Query completed.";
            refreshHittingSetInputGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshHittingSetInputGroupListAction()
        {
            return refreshHittingSetInputGroupEnable;
        }
        protected override void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {
            message.Dimension = SelectedHittingSetInputGroup.Dimension;
            message.NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet;
            message.Step = SelectedHittingSetInputGroup.Step;
            message.MaxCount = SelectedHittingSetInputGroup.MaxCount;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterInGroupCommand;
        public ICommand ToFilterInGroupCommand
        {
            get
            {
                if (toFilterInGroupCommand == null)
                {
                    toFilterInGroupCommand = new DelegateCommand(ToFilterInGroupAction, CanToFilterInGroupAction);
                }
                return toFilterInGroupCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterInGroupAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "RepresentativePerformanceAsGroup"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<AlgorithmGroupToFilterByGroupMessage>(new AlgorithmGroupToFilterByGroupMessage()
            {
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(AlgorithmGroupToFilterByGroupMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterInGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterInInputCommand;
        public ICommand ToFilterInInputCommand
        {
            get
            {
                if (toFilterInInputCommand == null)
                {
                    toFilterInInputCommand = new DelegateCommand(ToFilterInInputAction, CanToFilterInInputAction);
                }
                return toFilterInInputCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterInInputAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "InputData"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<AlgorithmGroupToFilterInputDataMessage>(new AlgorithmGroupToFilterInputDataMessage()
            {
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(AlgorithmGroupToFilterInputDataMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterInInputAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand openInWindowCommand;
        public ICommand OpenInWindowCommand
        {
            get
            {
                if (openInWindowCommand == null)
                {
                    openInWindowCommand = new DelegateCommand(OpenInWindowAction, CanOpenInWindowAction);
                }
                return openInWindowCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void OpenInWindowAction()
        {
            Messenger.Default.Send<AlgorithmGroupOpenWindowMessage>(new AlgorithmGroupOpenWindowMessage()
            {
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(AlgorithmGroupOpenWindowMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanOpenInWindowAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand deleteGroupCommand;
        public ICommand DeleteGroupCommand
        {
            get
            {
                if (deleteGroupCommand == null)
                {
                    deleteGroupCommand = new DelegateCommand(DeleteGroupAction, CanDeleteGroupAction);
                }
                return deleteGroupCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void DeleteGroupAction()
        {
            Messenger.Default.Send<DeleteAlgorithmInputMessage>(new DeleteAlgorithmInputMessage()
            {
                DeleteAlgorithmInputType = DeleteAlgorithmInputTypeEnum.Input,
                InputAlgorithmViewModel = this,
                MaxCount = SelectedHittingSetInputGroup.MaxCount,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Dimension = SelectedHittingSetInputGroup.Dimension,
            }, typeof(DeleteAlgorithmInputMessage));

            //ExecutionState = "Operation running...";
            //refreshHittingSetInputGroupEnable = false;
            //string? result = await representativesRepository.DeleteHittingSetInputGroupAsync(SelectedHittingSetInputGroup);
            //if (string.IsNullOrEmpty(result))
            //{
            //    HittingSetInputGroups.Remove(SelectedHittingSetInputGroup);
            //    ExecutionState = "Operation completed";
            //}
            //{
            //    ExecutionState = $"Operation failed: {result}";
            //}
            //refreshHittingSetInputGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void DeleteSelectedItem()
        {
            HittingSetInputGroups.Remove(SelectedHittingSetInputGroup);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskInputCommand;
        public ICommand RunTaskInputCommand
        {
            get
            {
                if (runTaskInputCommand == null)
                {
                    runTaskInputCommand = new DelegateCommand(RunTaskInputAction, CanRunTaskInputAction);
                }
                return runTaskInputCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskInputAction()
        {
            Messenger.Default.Send<StartTaskMessage>(new StartTaskMessage()
            {
                Algorithm = null,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                MaxCount = SelectedHittingSetInputGroup.MaxCount,
            }, typeof(StartTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskInputAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTaskStepInputCommand;
        public ICommand RunTaskStepInputCommand
        {
            get
            {
                if (runTaskStepInputCommand == null)
                {
                    runTaskStepInputCommand = new DelegateCommand(RunTaskStepInputAction, CanRunTaskStepInputAction);
                }
                return runTaskStepInputCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTaskStepInputAction()
        {
            Messenger.Default.Send<StartTaskStepMessage>(new StartTaskStepMessage()
            {
                Algorithm = null,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(StartTaskStepMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTaskStepInputAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismCommand;
        public ICommand RunIsomorphismCommand
        {
            get
            {
                if (runIsomorphismCommand == null)
                {
                    runIsomorphismCommand = new DelegateCommand(RunIsomorphismAction, CanRunIsomorphismAction);
                }
                return runIsomorphismCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.Isomorphism,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runIsomorphismBiPartCommand;
        public ICommand RunIsomorphismBiPartCommand
        {
            get
            {
                if (runIsomorphismBiPartCommand == null)
                {
                    runIsomorphismBiPartCommand = new DelegateCommand(RunIsomorphismBiPartAction, CanRunIsomorphismBiPartAction);
                }
                return runIsomorphismBiPartCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunIsomorphismBiPartAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.IsomorphismByPart,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(StartInputTaskMessage));

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunIsomorphismBiPartAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runTypeTaskCommand;
        public ICommand RunTypeTaskCommand
        {
            get
            {
                if (runTypeTaskCommand == null)
                {
                    runTypeTaskCommand = new DelegateCommand(RunTypeTaskAction, CanRunTypeTaskAction);
                }
                return runTypeTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunTypeTaskAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineTypeTask,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(StartInputTaskMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunTypeTaskAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand runGreedyComparisonCommand;
        public ICommand RunGreedyComparisonCommand
        {
            get
            {
                if (runGreedyComparisonCommand == null)
                {
                    runGreedyComparisonCommand = new DelegateCommand(RunGreedyComparisonAction, CanRunGreedyComparisonAction);
                }
                return runGreedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void RunGreedyComparisonAction()
        {
            Messenger.Default.Send<StartInputTaskMessage>(new StartInputTaskMessage()
            {
                KindOfInputTask = KindOfInputTaskEnum.DefineGreedyComparison,
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(StartInputTaskMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRunGreedyComparisonAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toInputGeedyComparisonCommand;
        public ICommand ToInputGeedyComparisonCommand
        {
            get
            {
                if (toInputGeedyComparisonCommand == null)
                {
                    toInputGeedyComparisonCommand = new DelegateCommand(ToInputGeedyComparisonAction, CanToInputGeedyComparisonAction);
                }
                return toInputGeedyComparisonCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToInputGeedyComparisonAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "InputDataGreedyComparison"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<InputDataGreedyComparisonToFilterMessage>(new InputDataGreedyComparisonToFilterMessage()
            {
                Dimension = SelectedHittingSetInputGroup.Dimension,
                NumberOfSet = SelectedHittingSetInputGroup.NumberOfSet,
                Step = SelectedHittingSetInputGroup.Step,
                MaxCount = SelectedHittingSetInputGroup.MaxCount
            }, typeof(InputDataGreedyComparisonToFilterMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToInputGeedyComparisonAction()
        {
            return true;
        }
    }

}
