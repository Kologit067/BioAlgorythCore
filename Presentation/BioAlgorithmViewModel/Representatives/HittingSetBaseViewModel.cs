using System.Windows.Input;
using BioAlgorithm.Data.Representatives.Data;
using BioAlgorithmViewModel.Common;
using BioAlgorithmViewModel.Mappings;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;

namespace BioAlgorithmViewModel.Representatives
{
    //----------------------------------------------------------------------------------------------------------------------
    // class HittingSetBaseViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public abstract class HittingSetBaseViewModel : ViewModelBase
    {
        protected readonly RepresentativesRepository representativesRepository;
        //----------------------------------------------------------------------------------------------------------------------
        private HittingSetFilterViewModel hittingSetDataFilter;
        public HittingSetFilterViewModel HittingSetFilter
        {
            get
            {
                return hittingSetDataFilter;
            }
            set
            {
                hittingSetDataFilter = value;
                OnPropertyChanged(nameof(HittingSetFilter));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public HittingSetBaseViewModel(RepresentativesRepository representativesRepository)
        {
            this.representativesRepository = representativesRepository;
            HittingSetFilter = new HittingSetFilterViewModel();
            HittingSetFilter.Top = 1000;
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
            AlgorithmGroupToFilterMessage message = new AlgorithmGroupToFilterMessage();
            FillAlgorithmGroupToFilterMessage(message);
            Messenger.Default.Send<AlgorithmGroupToFilterMessage>(message, typeof(AlgorithmGroupToFilterMessage));
        }
        protected virtual void FillAlgorithmGroupToFilterMessage(AlgorithmGroupToFilterMessage message)
        {

        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanToFilterAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private ICommand makeGraphCommand;
        public ICommand MakeGraphCommand
        {
            get
            {
                if (makeGraphCommand == null)
                {
                    makeGraphCommand = new DelegateCommand(MakeGraphAction, CanMakeGraphAction);
                }
                return makeGraphCommand;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void MakeGraphAction()
        {
            Messenger.Default.Send<RepresentativeTabChangeMessage>(new RepresentativeTabChangeMessage()
            {
                RepresentativeTabName = "BipartiteGraph"
            }, typeof(RepresentativeTabChangeMessage));
            Messenger.Default.Send<InputDataToGraphMessage>(new InputDataToGraphMessage()
            {
                InputData = GetGraphData()
            }, typeof(InputDataToGraphMessage));
        }
        protected virtual string GetGraphData()
        {
            return string.Empty;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanMakeGraphAction()
        {
            return true;
        }
    }
    //----------------------------------------------------------------------------------------------------------------------
}
