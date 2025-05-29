using System.Collections.ObjectModel;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using Representatives.Data.Contract;
using System.Windows.Input;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetInputGroupViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class HittingSetInputGroupViewModel : HittingSetBaseViewModel
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
            ExecutionState = "Operation running...";
            refreshHittingSetInputGroupEnable = false;
            string? result = await representativesRepository.DeleteHittingSetInputGroupAsync(SelectedHittingSetInputGroup);
            if (string.IsNullOrEmpty(result))
            {
                HittingSetInputGroups.Remove(SelectedHittingSetInputGroup);
                ExecutionState = "Operation completed";
            }
            {
                ExecutionState = $"Operation failed: {result}";
            }
            refreshHittingSetInputGroupEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }

}
