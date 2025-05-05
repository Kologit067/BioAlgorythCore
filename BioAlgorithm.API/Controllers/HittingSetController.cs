using BioAlgorithm.Services.Contract;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Representatives.Data.Contract;
using WebApi.OutputCache.V2;

namespace BioAlgorythm.API.Controllers
{
//    [EnableCors("AllowAngular")]
    public class HittingSetController : Controller
    {
        private readonly IRepresentativeService _representativeService;
        private readonly IMemoryCache _memoryCache;
        public HittingSetController(IRepresentativeService representativeService, IMemoryCache cache) 
        { 
            _representativeService = representativeService;
            _memoryCache = cache;
        }

        // GET: api/HittingSet/Algorithms
        [HttpGet]
        [Route("/api/HittingSet/Algorithms")]
        [CacheOutput]
        public async Task<ActionResult> Algorithms()
        {
            if (!_memoryCache.TryGetValue("Algorithms", out List<RepresentativeAlgorithmGroup>? result))
            { 
                result = await _representativeService.GetAlgorithmsAsync();
                _memoryCache.Set("Algorithms", result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            }
            return Json(result);
        }
        // GET: api/HittingSet/Algorithms
        [HttpGet]
        [Route("/api/HittingSet/GroupAlgorithms/{order}")]
        [CacheOutput]
        public async Task<ActionResult> GroupAlgorithms(string order)
        {
            string cacheKey = $"GroupAlgorithms-{order}";
            if (!_memoryCache.TryGetValue(cacheKey, out List<RepresentativeAlgorithmGroupDimension>? result))
            {
                try
                {
                    result = await _representativeService.GetRepresentativeAlgorithmGroupDimensionsAsync(order);
                    _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
             return Json(result);
        }

    }
}
 