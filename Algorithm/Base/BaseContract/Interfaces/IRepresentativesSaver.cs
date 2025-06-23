
using StatisticsStorage.Accumulators.Objects;

namespace BaseContract.Interfaces
{
    public interface IRepresentativesSaver
    {
        string Save(List<RepresentativesPerfomance> representativesPerfomances);
        Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? maxCount = null);
   }
}
