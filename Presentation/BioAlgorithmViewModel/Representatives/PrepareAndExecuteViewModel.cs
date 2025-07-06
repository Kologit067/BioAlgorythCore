using BioAlgorithm.Data.Representatives.Data;
using System.Windows.Input;
using BioAlgorithmViewModel.Common;
using System.Collections.ObjectModel;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithm.Core.Representatives;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PrepareAndExecuteViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class PrepareAndExecuteViewModel : CaseDefinitionViewModel
    {
        protected RepresentativesRepository representativesRepository;
        //----------------------------------------------------------------------------------------------------------------------
        protected ObservableCollection<AlgorithmDetailViewModel> algorithmItems;
        public ObservableCollection<AlgorithmDetailViewModel> AlgorithmItems
        {
            get
            {
                return algorithmItems;
            }
            set
            {
                algorithmItems = value;
                OnPropertyChanged(nameof(AlgorithmItems));
            }
        }
        protected bool isInputForce;
        public bool IsInputForce
        {
            get
            {
                return isInputForce;
            }
            set
            {
                isInputForce = value;
                OnPropertyChanged(nameof(IsInputForce));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PrepareAndExecuteViewModel()
        {

        }
        public PrepareAndExecuteViewModel(StartTaskMessage message)
        {
            if (message.Algorithm != null && algorithmItems != null)
            {
                AlgorithmDetailViewModel? messageAlgorithm = algorithmItems.FirstOrDefault( a => a.Algorithm == message.Algorithm);
                if (messageAlgorithm != null)
                {
                    messageAlgorithm.IsSelected = true;
                }
            }
            if (message.NumberOfSet != null)
            {
                NumberOfSet = message.NumberOfSet;
            }
            if (message.Dimension != null)
            {
                Dimension = message.Dimension;
            }
            representativesRepository = new RepresentativesRepository();
            FillAlgorithm();
            PropertyChanged += PrepareAndExecuteViewModel_PropertyChanged;
        }

        protected void FillAlgorithm()
        {

            AlgorithmItems = new ObservableCollection<AlgorithmDetailViewModel>()
            {
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceRepresentativesBinaryNumbers" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceRepresentativesBinaryNumbersVer2" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceRepresentativesAsTree" },
                new AlgorithmDetailViewModel() {Algorithm = "BruteForceRepresentativesAsTreeDirect" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesBranchAndBound" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesBranchAndBoundByValue" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesBranchAndBoundFirst" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesGreedySimple" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesGreedyImprove" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesGreedyRelation" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesGreedyImproveRD" },
                new AlgorithmDetailViewModel() {Algorithm = "AllGreedy" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangleBranchAndBound" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangle" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangleStrategy", AlgorithmDetail = "SelectElementSimpleStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangleStrategy", AlgorithmDetail = "SelectElementRelationStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangleStrategy", AlgorithmDetail = "SelectElementImproveStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "RepresentativesTriangleStrategy", AlgorithmDetail = "SelectElementImproveRDStrategy" },
                new AlgorithmDetailViewModel() {Algorithm = "Empty" }
            };
            
        }
        protected void PrepareAndExecuteViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Algorithm))
            {
                
            }
        }

        protected bool ExecuteAlgorithmEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand executeAlgorithmCommand;
        public ICommand ExecuteAlgorithmCommand
        {
            get
            {
                if (executeAlgorithmCommand == null)
                {
                    executeAlgorithmCommand = new DelegateCommand(ExecuteAlgorithmAction, CanExecuteAlgorithmAction);
                }
                return executeAlgorithmCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected async void ExecuteAlgorithmAction()
        {
            ExecutionState = "Task running...";
            ExecuteAlgorithmEnable = false;

            RepresentativeService representativeService = new RepresentativeService(representativesRepository);
            if (Dimension.HasValue && NumberOfSet.HasValue)
            {
                var selectedAlgoriths = AlgorithmItems.Where(a => a.IsSelected).Select(a => (a.Algorithm, a.AlgorithmDetail)).ToList();
                if (selectedAlgoriths.Count > 0)
                    await representativeService.ExecuteAlgorithmAsync(selectedAlgoriths, Dimension.Value, NumberOfSet.Value, IsInputForce);
                else
                {
                    ExecutionState = "Algoriths are not selected.";
                    ExecuteAlgorithmEnable = true;
                    return;
                }
            }

            ExecutionState = "Task completed.";
            ExecuteAlgorithmEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected bool CanExecuteAlgorithmAction()
        {
            return ExecuteAlgorithmEnable && Dimension.HasValue && NumberOfSet.HasValue;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
