using BioAlgorithm.Data.Representatives.Data;
using System.Windows.Input;
using BioAlgorithmViewModel.Common;
using System.Collections.ObjectModel;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithm.Services.HittingSet;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PrepareAndExecuteViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class PrepareAndExecuteViewModel : CaseDefinitionViewModel
    {
        protected readonly RepresentativesRepository representativesRepository;
        private string algorithmDetail;
        public string AlgorithmDetail
        {
            get
            {
                return algorithmDetail;
            }
            set
            {
                algorithmDetail = value;
                OnPropertyChanged(nameof(AlgorithmDetail));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<string> algorithmItems;
        public ObservableCollection<string> AlgorithmItems
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
        //----------------------------------------------------------------------------------------------------------------------
        private ObservableCollection<string> algorithmDetailItems;
        public ObservableCollection<string> AlgorithmDetailItems
        {
            get
            {
                return algorithmDetailItems;
            }
            set
            {
                algorithmDetailItems = value;
                OnPropertyChanged(nameof(AlgorithmDetailItems));
            }
        }
        private bool algorithmDetailReadOnly;
        public bool AlgorithmDetailReadOnly
        {
            get
            {
                return algorithmDetailReadOnly;
            }
            set
            {
                algorithmDetailReadOnly = value;
                OnPropertyChanged(nameof(AlgorithmDetailReadOnly));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PrepareAndExecuteViewModel(StartTaskMessage message)
        {
            if (message.Algorithm != null && (algorithmItems?.Contains(message.Algorithm) ?? false))
            {
                Algorithm = message.Algorithm;
            }
            if (message.NumberOfSet != null)
            {
                NumberOfSet = message.NumberOfSet;
            }
            if (message.Dimension != null)
            {
                Dimension = message.Dimension;
            }
            if (message.MaxCount != null)
            {
                Step = message.MaxCount;
            }
            representativesRepository = new RepresentativesRepository();
            AlgorithmItems = new ObservableCollection<string>()
            {
                "BruteForceRepresentativesBinaryNumbers",
                "BruteForceRepresentativesBinaryNumbersVer2",
                "BruteForceRepresentativesAsTree",
                "BruteForceRepresentativesAsTreeDirect",
                "RepresentativesBranchAndBound",
                "RepresentativesBranchAndBoundByValue",
                "RepresentativesBranchAndBoundFirst",
                "RepresentativesGreedySimple",
                "RepresentativesGreedyImprove",
                "RepresentativesGreedyRelation",
                "RepresentativesGreedyImproveRD",
                "RepresentativesTriangleBranchAndBound",
                "RepresentativesTriangle",
                "RepresentativesTriangleStrategy"
            };
            AlgorithmDetailItems = new ObservableCollection<string>()
            {
                "SelectElementSimpleStrategy",
                "SelectElementRelationStrategy",
                "SelectElementImproveStrategy",
                "SelectElementImproveRDStrategy"
            };
            Algorithm = AlgorithmItems[0];
            AlgorithmDetail = AlgorithmDetailItems[0];
            AlgorithmDetailReadOnly = true;

            PropertyChanged += PrepareAndExecuteViewModel_PropertyChanged;
        }

        private void PrepareAndExecuteViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Algorithm))
            {
                if (e.PropertyName == "RepresentativesTriangleStrategy")
                {
                    AlgorithmDetailReadOnly = false;
                }
                else
                {
                    AlgorithmDetailReadOnly = true;
                }
            }
        }

        private bool ExecuteAlgorithmEnable = true;
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
        private async void ExecuteAlgorithmAction()
        {
            ExecutionState = "Task running...";
            ExecuteAlgorithmEnable = false;

            RepresentativeService representativeService = new RepresentativeService(representativesRepository);
            if (Dimension.HasValue && NumberOfSet.HasValue)
                await representativeService.ExecuteAlgorithmAsync(Algorithm, AlgorithmDetail, Dimension.Value, NumberOfSet.Value, Step ?? 1);

            ExecutionState = "Task completed.";
            ExecuteAlgorithmEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanExecuteAlgorithmAction()
        {
            return ExecuteAlgorithmEnable && Dimension.HasValue && NumberOfSet.HasValue;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
