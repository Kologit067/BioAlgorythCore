using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using BioAlgorithm.Data.Representatives.Data;
using Representatives.Data.Contract;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithmViewModel.Interfaces;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class RepresentativePerformanceAlgorithmViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class RepresentativePerformanceAlgorithmViewModel : ViewModelBase, IInputAlgorithmViewModel
    {
        private RepresentativesRepository representativesRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<RepresentativeAlgorithmGroup> representativeAlgorithmGroups;
        public ObservableCollection<RepresentativeAlgorithmGroup> RepresentativeAlgorithmGroups
        {
            get
            {
                return representativeAlgorithmGroups;
            }
            set
            {
                representativeAlgorithmGroups = value;
                OnPropertyChanged(nameof(RepresentativeAlgorithmGroups));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private RepresentativeAlgorithmGroup selectedAlgorithm;
        public RepresentativeAlgorithmGroup SelectedAlgorithm
        {
            get
            {
                return selectedAlgorithm;
            }
            set
            {
                selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RepresentativePerformanceAlgorithmViewModel(RepresentativesRepository representativesRepository)
        {
            this.representativesRepository = representativesRepository;
            RepresentativeAlgorithmGroups = new ObservableCollection<RepresentativeAlgorithmGroup>();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand refreshRepresentativeAlgorithmListCommand;
        public ICommand RefreshRepresentativeAlgorithmListCommand
        {
            get
            {
                if (refreshRepresentativeAlgorithmListCommand == null)
                {
                    refreshRepresentativeAlgorithmListCommand = new DelegateCommand(RefreshRepresentativeAlgorithmListAction, CanRefreshRepresentativeAlgorithmListAction);
                }
                return refreshRepresentativeAlgorithmListCommand;
            }
        }
        private bool refreshRepresentativeAlgorithmListEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private async void RefreshRepresentativeAlgorithmListAction()
        {
            ExecutionState = "Query running...";
            refreshRepresentativeAlgorithmListEnable = false;

            RepresentativeAlgorithmGroups.Clear();
            List<RepresentativeAlgorithmGroup> algorithms = await representativesRepository.GetAlgorithmsAsync();
            foreach (RepresentativeAlgorithmGroup a in algorithms)
                RepresentativeAlgorithmGroups.Add(a);
            ExecutionState = "Query completed.";
            refreshRepresentativeAlgorithmListEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanRefreshRepresentativeAlgorithmListAction()
        {
            return refreshRepresentativeAlgorithmListEnable;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toFilterCommand;
        public ICommand ToFilterCommand
        {
            get
            {
                if (toFilterCommand == null)
                {
                    toFilterCommand = new DelegateCommand(ToFilterAction, CanToFilterAction);
                }
                return toFilterCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "RepresentativePerformance"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<AlgorithmToFilterMessage>(new AlgorithmToFilterMessage() { Algorithm = SelectedAlgorithm.Algorithm}, typeof(AlgorithmToFilterMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand toAsGroupFilterCommand;
        public ICommand ToAsGroupFilterCommand
        {
            get
            {
                if (toAsGroupFilterCommand == null)
                {
                    toAsGroupFilterCommand = new DelegateCommand(ToFilterAsGroupAction, CanToFilterAsGroupAction);
                }
                return toAsGroupFilterCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ToFilterAsGroupAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "RepresentativePerformanceAsGroup"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<AlgorithmToFilterByGroupMessage>(new AlgorithmToFilterByGroupMessage() { Algorithm = SelectedAlgorithm.Algorithm }, typeof(AlgorithmToFilterByGroupMessage));
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAsGroupAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand deleteAlgorithmCommand;
        public ICommand DeleteAlgorithmCommand
        {
            get
            {
                if (deleteAlgorithmCommand == null)
                {
                    deleteAlgorithmCommand = new DelegateCommand(DeleteAlgorithmAction, CanDeleteAlgorithmAction);
                }
                return deleteAlgorithmCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void DeleteAlgorithmAction()
        {
            Messenger.Default.Send<DeleteAlgorithmInputMessage>(new DeleteAlgorithmInputMessage()
            {
                DeleteAlgorithmInputType = DeleteAlgorithmInputTypeEnum.Algorithm,
                InputAlgorithmViewModel = this,
                Algorithm = SelectedAlgorithm.Algorithm
            }, typeof(DeleteAlgorithmInputMessage));


            //ExecutionState = "Operation running...";
            //refreshRepresentativeAlgorithmListEnable = false;
            //string? result = await representativesRepository.DeleteRepresentativeAlgorithmAsync(SelectedAlgorithm);
            //if (string.IsNullOrEmpty(result))
            //{
            //    RepresentativeAlgorithmGroups.Remove(SelectedAlgorithm);
            //    ExecutionState = "Operation completed";
            //}
            //{
            //    ExecutionState = $"Operation failed: {result}";
            //}
            //refreshRepresentativeAlgorithmListEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteAlgorithmAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void DeleteSelectedItem()
        {
            RepresentativeAlgorithmGroups.Remove(SelectedAlgorithm);
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
