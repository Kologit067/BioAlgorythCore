using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioAlgorithmViewModel.Common;

namespace BioAlgorithmViewModel.Representatives
{
    public class CaseDefinitionViewModel : ViewModelBase
    {
        private string algorithm;
        public string Algorithm
        {
            get
            {
                return algorithm;
            }
            set
            {
                algorithm = value;
                OnPropertyChanged(nameof(Algorithm));
            }
        }
        private int? numberOfSet;
        public int? NumberOfSet
        {
            get
            {
                return numberOfSet;
            }
            set
            {
                numberOfSet = value;
                OnPropertyChanged(nameof(NumberOfSet));
            }
        }
        private int? dimension;
        public int? Dimension
        {
            get
            {
                return dimension;
            }
            set
            {
                dimension = value;
                OnPropertyChanged(nameof(Dimension));
            }
        }
        private long? step;
        public long? Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
                OnPropertyChanged(nameof(Step));
            }
        }
    }
}
