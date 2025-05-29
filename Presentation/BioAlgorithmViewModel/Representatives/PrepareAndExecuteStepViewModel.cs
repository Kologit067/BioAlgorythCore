using System.Collections.ObjectModel;
using System.Windows.Input;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithm.Services.Contract;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Representatives.Messages;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PrepareAndExecuteStepViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class PrepareAndExecuteStepViewModel : PrepareAndExecuteViewModel
    {
        protected string calculationStep;
        public string CalculationStep
        {
            get
            {
                return calculationStep;
            }
            set
            {
                calculationStep = value;
                OnPropertyChanged(nameof(CalculationStep));
            }
        }
        protected string combinationType;
        public string CombinationType
        {
            get
            {
                return combinationType;
            }
            set
            {
                combinationType = value;
                OnPropertyChanged(nameof(CombinationType));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected ObservableCollection<string> calculationStepItems;
        public ObservableCollection<string> CalculationStepItems
        {
            get
            {
                return calculationStepItems;
            }
            set
            {
                calculationStepItems = value;
                OnPropertyChanged(nameof(CalculationStepItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected ObservableCollection<string> combinationTypeItems;
        public ObservableCollection<string> CombinationTypeItems
        {
            get
            {
                return combinationTypeItems;
            }
            set
            {
                combinationTypeItems = value;
                OnPropertyChanged(nameof(CombinationTypeItems));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PrepareAndExecuteStepViewModel()
        { }
        //----------------------------------------------------------------------------------------------------------------------
        public PrepareAndExecuteStepViewModel(StartTaskStepMessage message)
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
            if (message.Step != null)
            {
                Step = message.Step;
            }
            if (message.MaxCount != null)
            {
                MaxCount = message.MaxCount;
            }
            representativesRepository = new RepresentativesRepository();
            FillAlgorithm();
            FillStepDetails();
            PropertyChanged += PrepareAndExecuteViewModel_PropertyChanged;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void FillStepDetails()
        {
            CalculationStepItems = new ObservableCollection<string>()
            {
                "SkipEnumerationBigInteger",
                "SkipEnumerationNoRecBigInteger",
                "SkipEnumerationSaveFPBigInteger",              // Not Recursive Save first position
                "SkipEnumerationSaveFPImpBigInteger"            // Not Recursive Save first position Improve 1
            };
            CombinationTypeItems = new ObservableCollection<string>()
            {
                "By Matrix",
                "By Dictionary",
                "Without Matrix"
            };
            CalculationStep = CalculationStepItems[0];
            CombinationType = CombinationTypeItems[0];
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand executeAlgorithmStepCommand;
        public ICommand ExecuteAlgorithmStepCommand
        {
            get
            {
                if (executeAlgorithmStepCommand == null)
                {
                    executeAlgorithmStepCommand = new DelegateCommand(ExecuteAlgorithmStepAction, CanExecuteAlgorithmStepAction);
                }
                return executeAlgorithmStepCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected async void ExecuteAlgorithmStepAction()
        {
            ExecutionState = "Task running...";
            ExecuteAlgorithmEnable = false;

            IRepresentativeService representativeService = new RepresentativeService(representativesRepository);
            if (Dimension.HasValue && NumberOfSet.HasValue)
                await representativeService.ExecuteAlgorithmStepAsync(Algorithm, AlgorithmDetail, CalculationStep, CombinationType, Dimension.Value, NumberOfSet.Value, MaxCount ?? 0);

            ExecutionState = "Task completed.";
            ExecuteAlgorithmEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        protected bool CanExecuteAlgorithmStepAction()
        {
            return ExecuteAlgorithmEnable && Dimension.HasValue && NumberOfSet.HasValue;
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
