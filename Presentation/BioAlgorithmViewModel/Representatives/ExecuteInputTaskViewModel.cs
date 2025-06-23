using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithm.Services.HittingSet;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Representatives.Messages;
using System.Windows.Input;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PrepareAndExecuteViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class ExecuteInputTaskViewModel : CaseDefinitionViewModel
    {
        protected readonly RepresentativesRepository representativesRepository;
        private readonly KindOfInputTaskEnum KindOfInputTask;
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ExecuteInputTaskViewModel(StartInputTaskMessage message)
        {
            KindOfInputTask = message.KindOfInputTask;
            if (message.NumberOfSet != null)
            {
                NumberOfSet = message.NumberOfSet;
            }
            if (message.Dimension != null)
            {
                Dimension = message.Dimension;
            }
            //if (message.Step != null)
            //{
            //    Step = message.Step;
            //}
            if (message.MaxCount != null)
            {
                MaxCount = message.MaxCount;
            }
            else
            {
                MaxCount = 0;
            }
            representativesRepository = new RepresentativesRepository();

            Title = message.KindOfInputTask switch
            {
                KindOfInputTaskEnum.Isomorphism => "Define Isomorphism of Graph",
                KindOfInputTaskEnum.IsomorphismByPart => "Define Isomorphism of Bipartite Graph",
                KindOfInputTaskEnum.DefineTypeTask => "Define Type of Task",
                KindOfInputTaskEnum.DefineGreedyComparison => "Define GreedyComparison",
            };
        }

 
        private bool ExecuteTaskEnable = true;
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand executeTaskCommand;
        public ICommand ExecuteTaskCommand
        {
            get
            {
                if (executeTaskCommand == null)
                {
                    executeTaskCommand = new DelegateCommand(ExecuteTaskAction, CanTaskAlgorithmAction);
                }
                return executeTaskCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async void ExecuteTaskAction()
        {
            ExecutionState = "Task running...";
            ExecuteTaskEnable = false;
            string error;

            RepresentativeService representativeService = new RepresentativeService(representativesRepository);
            if (Dimension.HasValue && NumberOfSet.HasValue)
            {
                switch(KindOfInputTask)
                {
                    case KindOfInputTaskEnum.Isomorphism:
                        error = await representativeService.TestIsomorphismAsync(string.Empty,
                            Dimension.Value,
                            NumberOfSet.Value,
                            MaxCount ?? 0, false);
                        ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
                        break;
                    case KindOfInputTaskEnum.IsomorphismByPart:
                        error = await representativeService.TestIsomorphismAsync(string.Empty,
                            Dimension.Value,
                            NumberOfSet.Value,
                            MaxCount ?? 0, true);
                        ExecutionState = !string.IsNullOrEmpty(error) ? $"Test calculation failed: {error}" : "Test calculation completed";
                        break;
                    case KindOfInputTaskEnum.DefineTypeTask:
                        error = await representativeService.DefineTaskTypeAsync(string.Empty,
                            Dimension.Value,
                            NumberOfSet.Value,
                            MaxCount ?? 0);
                        ExecutionState = !string.IsNullOrEmpty(error) ? $"Task failed: {error}" : "Task completed";
                        break;
                    case KindOfInputTaskEnum.DefineGreedyComparison:
                        error = await representativeService.DefineGreedyComparisonAsync(string.Empty,
                            Dimension.Value,
                            NumberOfSet.Value,
                            MaxCount ?? 0);
                        ExecutionState = !string.IsNullOrEmpty(error) ? $"Task failed: {error}" : "Task completed";
                        break;
                }
            }

            ExecuteTaskEnable = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanTaskAlgorithmAction()
        {
            return ExecuteTaskEnable && Dimension.HasValue && NumberOfSet.HasValue;
        }
        //----------------------------------------------------------------------------------------------------------------------

    }
}
