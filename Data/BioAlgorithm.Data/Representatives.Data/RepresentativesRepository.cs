
using System.Configuration;
using System.Data;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract;
using BioAlgorithm.Data.Contract.Representatives.Data.Contract.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Representatives.Data.Contract;

namespace BioAlgorithm.Data.Representatives.Data
{
    //----------------------------------------------------------------------------------------------------------------------
    // class RepresentativesRepository
    //----------------------------------------------------------------------------------------------------------------------
    public class RepresentativesRepository : IRepresentativesRepository
    {
        private readonly string _connectionString;
        public RepresentativesRepository(string? connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _connectionString = ConfigurationManager.ConnectionStrings["BioAlgorithm"].ConnectionString;
            }
            else
            {
                _connectionString = connectionString;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteRepresentativeAlgorithmGroupAsync(RepresentativeAlgorithmGroupDimension selectedAlgorithmGroup)
        {
            return await DeleteAsync(selectedAlgorithmGroup.Algorithm, selectedAlgorithmGroup.NumberOfSet, selectedAlgorithmGroup.Dimension, selectedAlgorithmGroup.MaxCount);
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteHittingSetInputGroupAsync(HittingSetInputGroup selectedAlgorithmGroup)
        {
            return await DeleteInputAsync(selectedAlgorithmGroup.NumberOfSet, selectedAlgorithmGroup.Dimension, selectedAlgorithmGroup.MaxCount);
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? maxCount = null)
        {
            string? error = null;


            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                SqlCommand addCommand = new SqlCommand("dbo.deleteRepresentativesPerfomance", connection);
                addCommand.CommandType = CommandType.StoredProcedure;
                addCommand.CommandTimeout = 300;
                SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@Dimension", dimension);
                tvpParam2.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam3 = addCommand.Parameters.AddWithValue("@Algorithm", algorithm);
                tvpParam3.SqlDbType = SqlDbType.VarChar;
                SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@NumberOfSet", numberOfSet);
                tvpParam.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam4 = addCommand.Parameters.AddWithValue("@MaxCount", maxCount);
                tvpParam4.SqlDbType = SqlDbType.BigInt;

                await addCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return error;

        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteInputAsync(int? numberOfSet = null, int? dimension = null, decimal? maxCount = null)
        {
            string? error = null;


            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            try
            {
                SqlCommand addCommand = new SqlCommand("dbo.deleteRepresentativesInput", connection);
                addCommand.CommandType = CommandType.StoredProcedure;
                addCommand.CommandTimeout = 300;
                SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@Dimension", dimension);
                tvpParam2.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@NumberOfSet", numberOfSet);
                tvpParam.SqlDbType = SqlDbType.Int;
                SqlParameter tvpParam4 = addCommand.Parameters.AddWithValue("@MaxCount", maxCount);
                tvpParam4.SqlDbType = SqlDbType.BigInt;

                await addCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            finally
            {
                connection.Close();
            }
            return error;

        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string?> DeleteRepresentativeAlgorithmAsync(RepresentativeAlgorithmGroup selectedAlgorithm)
        {
            return await DeleteAsync(selectedAlgorithm.Algorithm);
        }
        public async Task<List<RepresentativeAlgorithmGroupDimension>> GetRepresentativeAlgorithmGroupDimensionsAsync(string algorithmGroupListSort)
        {
            try
            {
                List<RepresentativeAlgorithmGroupDimension> algorithmGroups = new List<RepresentativeAlgorithmGroupDimension>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"SELECT [Algorithm], [NumberOfSet], [Dimension], [MaxCount], COUNT(*) as TotalCount, 
       SUM([NumberOfIteration]) as NumberOfIteration, SUM([Duration]) as TotalDuration,
	   SUM([Duration])/COUNT(*) as AverageDuration
FROM [dbo].[RepresentativesPerfomance] AS rp
INNER JOIN [dbo].[RepresentativesInput] AS ri
ON (rp.RepresentativesInputId = ri.RepresentativesInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension], [MaxCount]";
                    if (!string.IsNullOrEmpty(algorithmGroupListSort))
                        sql += $@"ORDER BY {algorithmGroupListSort}
";
                    algorithmGroups = (await db.QueryAsync<RepresentativeAlgorithmGroupDimension>(sql, commandTimeout: 180)).ToList();
                }
                return algorithmGroups;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<RepresentativeAlgorithmGroupDimension>();
            }
        }

        public async Task<List<(string Algorithname,int Count)>> GetGeedyAlgorithmGroupDimensionsAsync(int dimension, int numberOfSet, long maxCount)
        {
            try
            {
                List<(string Algorithname, int Count)> algorithmGroups = new List<(string Algorithname, int Count)>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"SELECT [Algorithm],  COUNT(*) as TotalCount 
FROM [dbo].[RepresentativesPerfomance] AS rp
INNER JOIN [dbo].[RepresentativesInput] AS ri
ON (rp.RepresentativesInputId = ri.RepresentativesInputId)
WHERE [NumberOfSet] = {numberOfSet} AND [Dimension] = {dimension} AND [MaxCount] = {maxCount} AND [Algorithm] IN ('RepresentativesGreedyImprove','RepresentativesGreedyImproveRD','RepresentativesGreedyRelation','RepresentativesGreedySimple','RepresentativesBranchAndBoundByValue')
GROUP BY [Algorithm] 
";
                    algorithmGroups = (await db.QueryAsync<(string Algorithname, int Count)>(sql, commandTimeout: 180)).ToList();
                }
                return algorithmGroups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<RepresentativeAlgorithmGroupDimension>();
            }
        }


        public async Task SetGeedyComparisonAsync(int dimension, int numberOfSet, long maxCount)
        {
            try
            {
                
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"
UPDATE ri 
  SET ri.GreedyComparison = 
	 CASE
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue = rgd.BestValue
				THEN 0
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue <> rgd.BestValue
				THEN 1
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 2
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 3
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 4
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 5
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 6
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 7
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 8
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 9
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 10
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 11
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 12
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 13
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 14
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 15
	END 
  FROM [BioAlgorithm].[dbo].[RepresentativesInput] AS ri
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as r
  ON r.RepresentativesInputId = ri.RepresentativesInputId AND r.Algorithm = 'RepresentativesBranchAndBoundByValue' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet} AND ri.MaxCount = {maxCount} 
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgs
  ON rgs.RepresentativesInputId = ri.RepresentativesInputId AND rgs.Algorithm = 'RepresentativesGreedySimple' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgr
  ON rgr.RepresentativesInputId = ri.RepresentativesInputId AND rgr.Algorithm = 'RepresentativesGreedyRelation' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgi
  ON rgi.RepresentativesInputId = ri.RepresentativesInputId AND rgi.Algorithm = 'RepresentativesGreedyImprove' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgd
  ON rgd.RepresentativesInputId = ri.RepresentativesInputId AND rgd.Algorithm = 'RepresentativesGreedyImproveRD' AND ri.Dimension = {dimension} AND ri.NumberOfSet = {numberOfSet}  AND ri.MaxCount = {maxCount}
";
                    await db.ExecuteAsync(sql, commandTimeout: 180);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
                //return new List<RepresentativeAlgorithmGroupDimension>();
            }
        }

        public async Task<List<HittingSetInputGroup>> GetHittingSetInputGroupAsync(string algorithmGroupListSort)
        {
            try
            {
                List<HittingSetInputGroup> groups = new List<HittingSetInputGroup>();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = $@"WITH CTE 
AS
(
SELECT [NumberOfSet], [Dimension], [MaxCount], COUNT(*) as TotalCount, SUM (IIF([TypeTask] > 0, 1, 0)) AS SumTypaTask,
SUM (IIF([Isomorphic] IS NOT NULL, 1, 0)) AS SumIsomorphic, SUM (IIF(IsomorphicBipart IS NOT NULL, 1, 0)) AS SumIsomorphicBipart
FROM [dbo].[RepresentativesInput] AS ri
GROUP BY [NumberOfSet], [Dimension], [MaxCount]
)
SELECT [NumberOfSet], [Dimension], [MaxCount], TotalCount, SumTypaTask, SUBSTRING(CAST(CAST(SumTypaTask AS NUMERIC(19,4))/TotalCount AS VARCHAR),1,5) AS TypeTaskRelation, SumIsomorphic, SumIsomorphicBipart
FROM CTE ";
                    if (!string.IsNullOrEmpty(algorithmGroupListSort))
                        sql += $@"ORDER BY {algorithmGroupListSort}
";
                    groups = (await db.QueryAsync<HittingSetInputGroup>(sql, commandTimeout: 180)).ToList();
                }
                return groups;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<RepresentativeAlgorithWithDimension>> GetRepresentativeAlgorithmWithDimensionsAsync()
        {
            List<RepresentativeAlgorithWithDimension> algorithmWithDimensions = new List<RepresentativeAlgorithWithDimension>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT [Algorithm], [NumberOfSet], [Dimension], [MaxCount]
FROM [BioAlgorithm].[dbo].[RepresentativesPerfomance] AS rp
INNER JOIN [dbo].[RepresentativesInput] AS ri
ON (rp.RepresentativesInputId = ri.RepresentativesInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension], [MaxCount]
";
                algorithmWithDimensions = ( await db.QueryAsync<RepresentativeAlgorithWithDimension>(query)).ToList();
            }
            return algorithmWithDimensions;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<RepresentativeAlgorithmGroup>> GetAlgorithmsAsync()
        {
            List<RepresentativeAlgorithmGroup> algorithmGroups = new List<RepresentativeAlgorithmGroup>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"WITH CTE AS
(
SELECT [Algorithm], [NumberOfSet], [Dimension], COUNT(*) as cnt, SUM([NumberOfIteration]) as SumNumberOfIteration, SUM([Duration]) as SumDuration
FROM [BioAlgorithm].[dbo].[RepresentativesPerfomance] AS rp
INNER JOIN [dbo].[RepresentativesInput] AS ri
ON (rp.RepresentativesInputId = ri.RepresentativesInputId)
GROUP BY [Algorithm], [NumberOfSet], [Dimension]
)
SELECT [Algorithm], 
	SUM(cnt) AS TotalCount, 
	COUNT(*) as CountByDimension, 
	SUM(SumNumberOfIteration) as NumberOfIteration, 
	SUM(SumDuration) as TotalDuration,
	SUM(SumDuration)/SUM(cnt) AS AverageDuration
FROM CTE
GROUP BY [Algorithm]";
                algorithmGroups = ( await db.QueryAsync<RepresentativeAlgorithmGroup>(sql,commandTimeout: 180)).ToList();
            }
            return algorithmGroups;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<RepresentativesPerfomance>> GetRepresentativePerformanceListAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order)
        {
            string top = "";
            if (representativesPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({representativesPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.Algorithm))
            {
                whereList.Add($"[Algorithm] = '{representativesPerfomanceFilter.Algorithm}'");
            }
            if (representativesPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {representativesPerfomanceFilter.NumberOfSet}");
            }
            if (representativesPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {representativesPerfomanceFilter.Dimension}");
            }
            if (representativesPerfomanceFilter.Step.HasValue && representativesPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {representativesPerfomanceFilter.Step}");
            }
            if (representativesPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {representativesPerfomanceFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{representativesPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{representativesPerfomanceFilter.InputLenSort}'");
            }
            if (representativesPerfomanceFilter.NumberOfIterationFrom.HasValue)
            {
                whereList.Add($"[NumberOfIteration] >= {representativesPerfomanceFilter.NumberOfIterationFrom}");
            }
            if (representativesPerfomanceFilter.NumberOfIterationTo.HasValue)
            {
                whereList.Add($"[NumberOfIteration] <= {representativesPerfomanceFilter.NumberOfIterationTo}");
            }
            if (representativesPerfomanceFilter.DurationFrom.HasValue)
            {
                whereList.Add($"[Duration] >= {representativesPerfomanceFilter.DurationFrom}");
            }
            if (representativesPerfomanceFilter.DurationTo.HasValue)
            {
                whereList.Add($"[Duration] <= {representativesPerfomanceFilter.DurationTo}");
            }
            if (representativesPerfomanceFilter.CountTerminalFrom.HasValue)
            {
                whereList.Add($"[CountTerminal] >= {representativesPerfomanceFilter.CountTerminalFrom}");
            }
            if (representativesPerfomanceFilter.CountTerminalTo.HasValue)
            {
                whereList.Add($"[CountTerminal] <= {representativesPerfomanceFilter.CountTerminalTo}");
            }
            if (representativesPerfomanceFilter.BestValue.HasValue)
            {
                whereList.Add($"[BestValue] = {representativesPerfomanceFilter.BestValue}");
            }
            if (representativesPerfomanceFilter.IsComplete.HasValue)
            {
                whereList.Add($"[IsComplete] = {representativesPerfomanceFilter.IsComplete}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<RepresentativesPerfomance> representativesPerfomances = new List<RepresentativesPerfomance>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT {top} [RepresentativesPerfomanceId]
      ,[NumberOfSet],[Dimension],[Step],MaxCount,[InputLen],[InputLenSort]
      ,[InputLenAvg],[InputData],[InputDataShort],[Algorithm],[NumberOfIteration]
	  ,[Duration],[DurationMilliSeconds],[DateComplete],[IsComplete]
      ,[LastRoute],[OptimalRoute],[CountTerminal],[BestValue]
      ,[UpdateOptcount],[ElemenationCount]
FROM [BioAlgorithm].[dbo].[RepresentativesPerfomance] AS rp
INNER JOIN [dbo].[RepresentativesInput] AS ri
ON (rp.RepresentativesInputId = ri.RepresentativesInputId)
{where}
ORDER BY {order}";
                representativesPerfomances = ( await db.QueryAsync<RepresentativesPerfomance>(query, commandTimeout: 180)).ToList();
            }
            return representativesPerfomances;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<RepresentativesInputDao>> GetRepresentativeInputsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order)
        {
            string top = "";
            if (representativesPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({representativesPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            if (representativesPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {representativesPerfomanceFilter.NumberOfSet}");
            }
            if (representativesPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {representativesPerfomanceFilter.Dimension}");
            }
            if (representativesPerfomanceFilter.Step.HasValue && representativesPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {representativesPerfomanceFilter.Step}");
            }
            if (representativesPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {representativesPerfomanceFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{representativesPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{representativesPerfomanceFilter.InputLenSort}'");
            }

            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "Or")
            {
                whereList.Add($"([TypeTask] & {representativesPerfomanceFilter.TaskTypeFilter}) != 0");
            }
            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "And")
            {
                whereList.Add($"([TypeTask] & {representativesPerfomanceFilter.TaskTypeFilter}) = {representativesPerfomanceFilter.TaskTypeFilter}");
            }
            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "Exact =")
            {
                whereList.Add($"([TypeTask] = {representativesPerfomanceFilter.TaskTypeFilter})");
            }

            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<RepresentativesInputDao> representativesInputs = new List<RepresentativesInputDao>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"SELECT {top} RepresentativesInputId, [NumberOfSet],[Dimension],[Step],MaxCount,[InputLen],[InputLenSort]
      ,[InputLenAvg],[InputData],[InputDataShort],Isomorphic,IsomorphicBipart, IsomorphismResult, IsomorphismBipartResult, TypeTask, GreedyComparison
FROM [dbo].[RepresentativesInput] AS ri
{where}
ORDER BY {order}";
                representativesInputs = (await db.QueryAsync<RepresentativesInputDao>(query, commandTimeout: 180)).ToList();
            }
            updateIsomorphicDict.Clear();
            return representativesInputs;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<InputDataGreedyComparison>> GetInputDataGreedyComparisonsAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, string order)
        {
            string top = "";
            if (representativesPerfomanceFilter.Top.HasValue)
            {
                top = $"TOP ({representativesPerfomanceFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();
            whereList.Add($"[NumberOfSet] = {representativesPerfomanceFilter.NumberOfSet}");
            whereList.Add($"[Dimension] = {representativesPerfomanceFilter.Dimension}");
            whereList.Add($"[MaxCount] = {representativesPerfomanceFilter.MaxCount}");
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLen))
            {
                whereList.Add($"[InputLen] = '{representativesPerfomanceFilter.InputLen}'");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceFilter.InputLenSort))
            {
                whereList.Add($"[InputLenSort] = '{representativesPerfomanceFilter.InputLenSort}'");
            }

            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "Or")
            {
                whereList.Add($"([TypeTask] & {representativesPerfomanceFilter.TaskTypeFilter}) != 0");
            }
            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "And")
            {
                whereList.Add($"([TypeTask] & {representativesPerfomanceFilter.TaskTypeFilter}) = {representativesPerfomanceFilter.TaskTypeFilter}");
            }
            if (representativesPerfomanceFilter.TaskTypeFilter != 0 && representativesPerfomanceFilter.TaskTypeFilterType == "Exact =")
            {
                whereList.Add($"([TypeTask] = {representativesPerfomanceFilter.TaskTypeFilter})");
            }

            if (representativesPerfomanceFilter.GreedyComparisonFilter != 0 && representativesPerfomanceFilter.GreedyComparisonFilterType == "Or")
            {
                whereList.Add($"([GreedyComparison] & {representativesPerfomanceFilter.GreedyComparisonFilter}) != 0");
            }
            if (representativesPerfomanceFilter.GreedyComparisonFilter != 0 && representativesPerfomanceFilter.GreedyComparisonFilterType == "And")
            {
                whereList.Add($"([GreedyComparison] & {representativesPerfomanceFilter.GreedyComparisonFilter}) = {representativesPerfomanceFilter.GreedyComparisonFilter}");
            }
            if (representativesPerfomanceFilter.GreedyComparisonFilter != 0 && representativesPerfomanceFilter.GreedyComparisonFilterType == "Exact =")
            {
                whereList.Add($"([GreedyComparison] = {representativesPerfomanceFilter.GreedyComparisonFilter})");
            }

            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<InputDataGreedyComparison> representativesInputs = new List<InputDataGreedyComparison>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = $@"
  SELECT ri.RepresentativesInputId, 
     r.[Algorithm], r.BestValue, 
	 CASE
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue = rgd.BestValue
				THEN 0
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND  r.BestValue <> rgd.BestValue
				THEN 1
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 2
		WHEN r.BestValue = rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 3
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 4
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 5
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 6
		WHEN r.BestValue = rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 7
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 8
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 9
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 10
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue = rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 11
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 12
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue = rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 13
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue = rgd.BestValue
				THEN 14
		WHEN r.BestValue <> rgs.BestValue AND r.BestValue <> rgr.BestValue AND r.BestValue <> rgi.BestValue AND r.BestValue <> rgd.BestValue
				THEN 15
	END AS GreedyComparisonCalc, GreedyComparison,
	rgs.[Algorithm] as AlgorithmSimple, rgs.BestValue as BestValueSimple, 
	rgr.[Algorithm] as AlgorithmRelation, rgr.BestValue as BestValueRelation, 
	rgi.[Algorithm] as AlgorithmImprove, rgi.BestValue as BestValueImprove, 
	rgd.[Algorithm] as AlgorithmImproveRD, rgd.BestValue as BestValueImproveRD, 
	TypeTask,InputData, [NumberOfSet], [Dimension], MAXCOUNT
  FROM [BioAlgorithm].[dbo].[RepresentativesInput] AS ri
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as r 
  ON r.RepresentativesInputId = ri.RepresentativesInputId AND r.Algorithm = 'RepresentativesBranchAndBoundByValue' AND ri.Dimension = {representativesPerfomanceFilter.Dimension} AND ri.NumberOfSet = {representativesPerfomanceFilter.NumberOfSet} AND ri.MaxCount = {representativesPerfomanceFilter.MaxCount} 
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgs
  ON rgs.RepresentativesInputId = ri.RepresentativesInputId AND rgs.Algorithm = 'RepresentativesGreedySimple' AND ri.Dimension = {representativesPerfomanceFilter.Dimension} AND ri.NumberOfSet = {representativesPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {representativesPerfomanceFilter.MaxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgr
  ON rgr.RepresentativesInputId = ri.RepresentativesInputId AND rgr.Algorithm = 'RepresentativesGreedyRelation' AND ri.Dimension = {representativesPerfomanceFilter.Dimension} AND ri.NumberOfSet = {representativesPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {representativesPerfomanceFilter.MaxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgi
  ON rgi.RepresentativesInputId = ri.RepresentativesInputId AND rgi.Algorithm = 'RepresentativesGreedyImprove' AND ri.Dimension = {representativesPerfomanceFilter.Dimension} AND ri.NumberOfSet = {representativesPerfomanceFilter.NumberOfSet}  AND ri.MaxCount = {representativesPerfomanceFilter.MaxCount}
  INNER JOIN [BioAlgorithm].[dbo].[RepresentativesPerfomance] as rgd
  ON rgd.RepresentativesInputId = ri.RepresentativesInputId AND rgd.Algorithm = 'RepresentativesGreedyImproveRD' AND ri.Dimension = {representativesPerfomanceFilter.Dimension} AND ri.NumberOfSet = {representativesPerfomanceFilter.NumberOfSet} AND ri.MaxCount = {representativesPerfomanceFilter.MaxCount}
{where}
ORDER BY {order}";
                representativesInputs = (await db.QueryAsync<InputDataGreedyComparison>(query, commandTimeout: 180)).ToList();
            }
            return representativesInputs;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<List<RepresentativesPerfomanceCompare>> GetRepresentativePerformanceCompareListAsync(RepresentativesPerfomanceCompareFilter representativesPerfomanceCompareFilter)
        {
            string top = "";
            if (representativesPerfomanceCompareFilter.Top.HasValue)
            {
                top = $"TOP ({representativesPerfomanceCompareFilter.Top})";
            }
            string where = "";
            List<string> whereList = new List<string>();

            if (representativesPerfomanceCompareFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"ra.[NumberOfSet] = {representativesPerfomanceCompareFilter.NumberOfSet}");
            }
            if (representativesPerfomanceCompareFilter.Dimension.HasValue)
            {
                whereList.Add($"ra.[Dimension] = {representativesPerfomanceCompareFilter.Dimension}");
            }
            if (representativesPerfomanceCompareFilter.Step.HasValue)
            {
                whereList.Add($"ra.[Step] = {representativesPerfomanceCompareFilter.Step}");
            }
            if (representativesPerfomanceCompareFilter.MaxCount.HasValue)
            {
                whereList.Add($"ra.[MaxCount] = {representativesPerfomanceCompareFilter.MaxCount}");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceCompareFilter.BestValueCompare) && representativesPerfomanceCompareFilter.BestValueCompare != "N/A")
            {
                whereList.Add($"(ra.BestValue {representativesPerfomanceCompareFilter.BestValueCompare.Replace("1", "").Replace("2", "")} rb.BestValue OR ra.BestValue IS NULL OR rb.BestValue IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceCompareFilter.NumberIterationCompare) && representativesPerfomanceCompareFilter.NumberIterationCompare != "N/A")
            {
                whereList.Add($"(ra.[NumberOfIteration] {representativesPerfomanceCompareFilter.NumberIterationCompare.Replace("1", "").Replace("2", "")} rb.[NumberOfIteration] OR ra.[NumberOfIteration] IS NULL OR rb.[NumberOfIteration] IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceCompareFilter.DurationCompare) && representativesPerfomanceCompareFilter.DurationCompare != "N/A")
            {
                whereList.Add($"(ra.Duration {representativesPerfomanceCompareFilter.DurationCompare.Replace("1", "").Replace("2", "")} rb.Duration OR ra.Duration IS NULL OR rb.Duration IS NULL )");
            }
            if (!string.IsNullOrWhiteSpace(representativesPerfomanceCompareFilter.ElemenationCountCompare) && representativesPerfomanceCompareFilter.ElemenationCountCompare != "N/A")
            {
                whereList.Add($"(ra.ElemenationCount {representativesPerfomanceCompareFilter.ElemenationCountCompare.Replace("1", "").Replace("2", "")} rb.ElemenationCount OR ra.ElemenationCount IS NULL OR rb.ElemenationCount IS NULL )");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            List<RepresentativesPerfomanceCompare> representativesPerfomancesCompare = new List<RepresentativesPerfomanceCompare>();
            string query = $@"WITH ra AS
(
SELECT ria.[NumberOfSet], ria.[Dimension], ria.[InputData], ria.[InputDataShort], ria.Step, ria.MaxCount,
       rpa.Algorithm, rpa.BestValue, rpa.OptimalRoute, rpa.[NumberOfIteration],rpa.[Duration], rpa.ElemenationCount
FROM [BioAlgorithm].[dbo].[RepresentativesPerfomance] AS rpa
INNER JOIN [dbo].[RepresentativesInput] AS ria
ON (rpa.RepresentativesInputId = ria.RepresentativesInputId)
),
rb AS
(
SELECT rib.[NumberOfSet], rib.[Dimension], rib.[InputData], rib.[InputDataShort], rib.Step, rib.MaxCount,
       rpb.Algorithm, rpb.BestValue, rpb.OptimalRoute, rpb.[NumberOfIteration], rpb.[Duration], rpb.ElemenationCount
FROM [BioAlgorithm].[dbo].[RepresentativesPerfomance] AS rpb
INNER JOIN [dbo].[RepresentativesInput] AS rib
ON (rpb.RepresentativesInputId = rib.RepresentativesInputId)
)
SELECT ra.[NumberOfSet], ra.[Dimension], ra.[InputData], ra.[InputDataShort], ra.Step, ra.MaxCount,
       ra.Algorithm as Algorithm1, rb.Algorithm as Algorithm2, 
       ra.BestValue as BestValue1, rb.BestValue as BestValue2, 
	   ra.OptimalRoute as OptimalRoute1, rb.OptimalRoute as OptimalRoute2,
	   ra.[NumberOfIteration] as NumberOfIteration1, rb.[NumberOfIteration] as NumberOfIteration2,
       ra.[Duration] as Duration1, rb.[Duration] as Duration2,
       ra.ElemenationCount as ElemenationCount1, rb.ElemenationCount as ElemenationCount2
FROM ra INNER JOIN rb
ON (ra.InputData = rb.InputData AND ra.[NumberOfSet] = rb.[NumberOfSet] AND ra.[Dimension] = rb.[Dimension] AND
ra.[Algorithm] = '{representativesPerfomanceCompareFilter.Algorithm1}' AND rb.[Algorithm] = '{representativesPerfomanceCompareFilter.Algorithm2}') 
{where}
";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                representativesPerfomancesCompare = (await db.QueryAsync<RepresentativesPerfomanceCompare>(query)).ToList();
            }
            return representativesPerfomancesCompare;
        }
        private int updateIsomorphicBufferSize = 1000;
        private Dictionary<long, (string,string)> updateIsomorphicDict = new Dictionary<long, (string,string)>();
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> UpdateIsomorphicAsync(long representativesInputId, string inputData, string result, bool isBipart = false)
        {
            string error = string.Empty;
            updateIsomorphicDict.Add(representativesInputId, (inputData, result));

            if (updateIsomorphicDict.Count >= updateIsomorphicBufferSize)
            {
                error = await SaveUpdateIsomorphicAsync(updateIsomorphicDict, isBipart);
                updateIsomorphicDict.Clear();
            }
            return error;
        }

        private int updateTaskTypeBufferSize = 1000;
        private Dictionary<long, int> updateTaskTypeDict = new Dictionary<long, int>();
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> UpdateTypeTaskAsync(long representativesInputId, int typeTask)
        {
            string error = string.Empty;
            updateTaskTypeDict.Add(representativesInputId, typeTask);

            if (updateTaskTypeDict.Count >= updateTaskTypeBufferSize)
            {
                error = await SaveUpdateTypeTaskAsync(updateTaskTypeDict);
                updateTaskTypeDict.Clear();
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> CompleteUpdateIsomorphicAsync(bool isBipart = false)
        {
            string error = await SaveUpdateIsomorphicAsync(updateIsomorphicDict, isBipart);
            updateIsomorphicDict.Clear();
            return error;
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task<string> CompleteUpdateTypeTaskAsync()
        {
            string error = await SaveUpdateTypeTaskAsync(updateTaskTypeDict);
            updateIsomorphicDict.Clear();
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private async Task<string> SaveUpdateIsomorphicAsync(Dictionary<long, (string, string)> updateIsomorphicDict, bool isBipart = false)
        {
            string error = string.Empty;
            try
            {

                DataTable isonorphicTable = new DataTable();
                isonorphicTable.Columns.Add("Id", System.Type.GetType("System.Int64"));
                isonorphicTable.Columns.Add("InputData", System.Type.GetType("System.String"));
                isonorphicTable.Columns.Add("Result", System.Type.GetType("System.String"));


                foreach (var ui in updateIsomorphicDict)
                {
                    isonorphicTable.Rows.Add(ui.Key, ui.Value.Item1, ui.Value.Item2);
                }

                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("dbo.spUpdateIsomorphic", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@UpdateIsomorphics", isonorphicTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.UpdateIsomorphicType";
                    SqlParameter tvpParam2 = addCommand.Parameters.AddWithValue("@IsBipart", isBipart);
                    tvpParam2.SqlDbType = SqlDbType.Bit;
                    await addCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }


        //----------------------------------------------------------------------------------------------------------------------
        private async Task<string> SaveUpdateTypeTaskAsync(Dictionary<long, int> updateTypeTaskDict)
        {
            string error = string.Empty;
            try
            {

                DataTable typeTaskTable = new DataTable();
                typeTaskTable.Columns.Add("Id", System.Type.GetType("System.Int64"));
                typeTaskTable.Columns.Add("TypeTask", System.Type.GetType("System.Int32"));


                foreach (var ui in updateTypeTaskDict)
                {
                    typeTaskTable.Rows.Add(ui.Key, ui.Value);
                }

                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                try
                {
                    SqlCommand addCommand = new SqlCommand("dbo.spUpdateTypeTask", connection);
                    addCommand.CommandType = CommandType.StoredProcedure;
                    addCommand.CommandTimeout = 300;
                    SqlParameter tvpParam = addCommand.Parameters.AddWithValue("@UpdateTypeTask", typeTaskTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.UpdateTypeTaskType";
                    await addCommand.ExecuteNonQueryAsync();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return error;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public async Task ClearIsomorphicAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter, bool isBipart = false)
        {
            string where = "";
            List<string> whereList = new List<string>();
            if (representativesPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {representativesPerfomanceFilter.NumberOfSet}");
            }
            if (representativesPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {representativesPerfomanceFilter.Dimension}");
            }
            if (representativesPerfomanceFilter.Step.HasValue && representativesPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {representativesPerfomanceFilter.Step}");
            }
            if (representativesPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {representativesPerfomanceFilter.MaxCount}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = string.Empty;
                if (!isBipart)
                {
                    query = $@"UPDATE [dbo].[RepresentativesInput]
SET [Isomorphic] = NULL, [IsomorphismResult] = NULL
{where}
";
                }
                else
                {
                    query = $@"UPDATE [dbo].[RepresentativesInput]
SET [IsomorphicBipart] = NULL, [IsomorphismBipartResult] = NULL
{where}
";
                }
                await db.ExecuteAsync(query);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public async Task ClearTaskTypeAsync(RepresentativesPerfomanceFilter representativesPerfomanceFilter)
        {
            string where = "";
            List<string> whereList = new List<string>();
            if (representativesPerfomanceFilter.NumberOfSet.HasValue)
            {
                whereList.Add($"[NumberOfSet] = {representativesPerfomanceFilter.NumberOfSet}");
            }
            if (representativesPerfomanceFilter.Dimension.HasValue)
            {
                whereList.Add($"[Dimension] = {representativesPerfomanceFilter.Dimension}");
            }
            if (representativesPerfomanceFilter.Step.HasValue && representativesPerfomanceFilter.Step.Value > 0)
            {
                whereList.Add($"[Step] = {representativesPerfomanceFilter.Step}");
            }
            if (representativesPerfomanceFilter.MaxCount.HasValue)
            {
                whereList.Add($"[MaxCount] = {representativesPerfomanceFilter.MaxCount}");
            }
            if (whereList.Count > 0)
            {
                where = "WHERE " + string.Join(" AND ", whereList);
            }
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = string.Empty;
                query = $@"UPDATE [dbo].[RepresentativesInput]
SET [TypeTask] = NULL
{where}
";
                await db.ExecuteAsync(query);
            }
            //----------------------------------------------------------------------------------------------------------------------
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
