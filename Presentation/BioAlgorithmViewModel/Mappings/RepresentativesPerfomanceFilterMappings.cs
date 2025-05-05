using BioAlgorithmViewModel.Representatives;
using Representatives.Data.Contract;

namespace BioAlgorithmViewModel.Mappings
{
    public static class RepresentativesPerfomanceFilterMappings
    {
        public static RepresentativesPerfomanceFilter? Map(this HittingSetFilterViewModel input)
        {
            return input == null
            ? null
                : new RepresentativesPerfomanceFilter()
                {
                    Top = input.Top,
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    Step = input.Step,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    NumberOfIterationFrom = input.NumberOfIterationFrom,
                    NumberOfIterationTo = input.NumberOfIterationTo,
                    DurationFrom = input.DurationFrom,
                    DurationTo = input.DurationTo,
                    IsComplete = input.IsComplete,
                    CountTerminalFrom = input.CountTerminalFrom,
                    CountTerminalTo = input.CountTerminalTo,
                    BestValue = input.BestValue,
                    TaskTypeFilterType = input.TaskTypeFilterType
                };
        }

        public static IEnumerable<RepresentativesPerfomanceFilter> Map(this IEnumerable<HittingSetFilterViewModel> input) => input?.Select(i => i.Map()).ToList();

        public static HittingSetFilterViewModel? ToEntity(this RepresentativesPerfomanceFilter input)
        {
            return input == null
                ? null
                : new HittingSetFilterViewModel()
                {
                    Top = input.Top,
                    Algorithm = input.Algorithm,
                    NumberOfSet = input.NumberOfSet,
                    Dimension = input.Dimension,
                    InputLen = input.InputLen,
                    InputLenSort = input.InputLenSort,
                    NumberOfIterationFrom = input.NumberOfIterationFrom,
                    NumberOfIterationTo = input.NumberOfIterationTo,
                    DurationFrom = input.DurationFrom,
                    DurationTo = input.DurationTo,
                    IsComplete = input.IsComplete,
                    CountTerminalFrom = input.CountTerminalFrom,
                    CountTerminalTo = input.CountTerminalTo,
                    BestValue = input.BestValue
                };
        }
        public static IEnumerable<HittingSetFilterViewModel> ToEntity(this IEnumerable<RepresentativesPerfomanceFilter> input) => input?.Select(i => i.ToEntity()).ToList();

    }

}
