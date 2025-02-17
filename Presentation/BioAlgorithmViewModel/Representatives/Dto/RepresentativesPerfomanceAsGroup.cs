using BioAlgorithmViewModel.Representatives.Dto;

namespace BioAlgorithm.RepresentativesModel
{
    public class RepresentativesPerfomanceAsGroup
    {
        public string ColumnGroupName { get; set; }
        public string ColumnGroupValue { get; set; }
        public List<RepresentativesPerfomanceDto> RepresentativesPerfomanceList { get; set; }
    }
}
