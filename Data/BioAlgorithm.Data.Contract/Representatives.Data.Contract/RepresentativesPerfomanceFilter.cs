
namespace Representatives.Data.Contract
{
    public class RepresentativesPerfomanceFilter
    {
        public int? Top { get; set; }
        public string Algorithm { get; set; }
        public int? NumberOfSet { get; set; }
        public int? Dimension { get; set; }
        public long? Step { get; set; }
        public long? MaxCount { get; set; }
        public string InputLen { get; set; }
        public string InputLenSort { get; set; }
        public long? NumberOfIterationFrom { get; set; }
        public long? NumberOfIterationTo { get; set; }
        public long? DurationFrom { get; set; }
        public long? DurationTo { get; set; }
        public bool? IsComplete { get; set; }
        public int? CountTerminalFrom { get; set; }
        public int? CountTerminalTo { get; set; }
        public int? BestValue { get; set; }
        public int TaskTypeFilter { get; set; }
        public string TaskTypeFilterType { get; set; }
        public int GreedyComparisonFilter { get; set; }
        public string GreedyComparisonFilterType { get; set; }
        public string PairComparison { get; set; }
        public string SelectedGreedy { get; set; }

        public string GetStringKey()
        {
            string key = "I";
            if (Top.HasValue)
                key += $"Top-{Top.Value.ToString()}";
            if (!string.IsNullOrEmpty(Algorithm))
                key += $"Algorithm-{Algorithm}";
            if (NumberOfSet.HasValue)
                key += $"NumberOfSet-{NumberOfSet.Value.ToString()}";
            if (Dimension.HasValue)
                key += $"Dimension-{Dimension.Value.ToString()}";
            if (Step.HasValue)
                key += $"Step-{Step.Value.ToString()}";
            if (MaxCount.HasValue)
                key += $"MaxCount-{MaxCount.Value.ToString()}";
            if (!string.IsNullOrEmpty(InputLen))
                key += $"InputLen-{InputLen}";
            if (!string.IsNullOrEmpty(InputLenSort))
                key += $"InputLenSort-{InputLenSort}";
            if (NumberOfIterationFrom.HasValue)
                key += $"NumberOfIterationFrom-{NumberOfIterationFrom.Value.ToString()}";
            if (NumberOfIterationTo.HasValue)
                key += $"NumberOfIterationTo-{NumberOfIterationTo.Value.ToString()}";
            if (DurationFrom.HasValue)
                key += $"DurationFrom-{DurationFrom.Value.ToString()}";
            if (DurationTo.HasValue)
                key += $"DurationTo-{DurationTo.Value.ToString()}";
            if (IsComplete.HasValue)
                key += $"IsComplete-{IsComplete.Value.ToString()}";
            if (CountTerminalFrom.HasValue)
                key += $"CountTerminalFrom-{CountTerminalFrom.Value.ToString()}";
            if (CountTerminalTo.HasValue)
                key += $"CountTerminalTo-{CountTerminalTo.Value.ToString()}";
            if (BestValue.HasValue)
                key += $"BestValue-{BestValue.Value.ToString()}";
            key += $"TaskTypeFilter-{TaskTypeFilter.ToString()}";
            if (!string.IsNullOrEmpty(TaskTypeFilterType))
                key += $"TaskTypeFilterType-{TaskTypeFilterType}";
            if (!string.IsNullOrEmpty(GreedyComparisonFilterType))
                key += $"GreedyComparisonFilterType-{GreedyComparisonFilterType}";
            return key;
        }
    }
}
