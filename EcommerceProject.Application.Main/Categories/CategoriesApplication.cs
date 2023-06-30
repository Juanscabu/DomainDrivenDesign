using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Feature.Customers;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Transversal.Common;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace EcommerceProject.Application.Feature.Categories
{
    public class CategoriesApplication : ICategoriesApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger<CustomersApplication> _logger;
        private readonly IDistributedCache _distributedCache;

        public CategoriesApplication(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger<CustomersApplication> logger
            , IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<Response<IEnumerable<CategoryDto>>> GetAll()
        {
            var response = new Response<IEnumerable<CategoryDto>>();
            var cacheKey = "categoriesList";
            try
            {
                var redisCategories = await _distributedCache.GetAsync(cacheKey);

                if (redisCategories != null)
                {
                    response.Data = JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(redisCategories);
                }
                else
                {
                    var categories = await _unitOfWork.Categories.GetAll();
                    response.Data = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                    if (response.Data != null)
                    {
                        var serializedCategories = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response.Data));
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(8))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                        await _distributedCache.SetAsync(cacheKey, serializedCategories, options);
                    }
                }
                if (response.Data != null)
                {
                    response.IsSuccess = true;
                    response.Message = "Succesfull Categories Retrieval";
                    _logger.LogInformation("Succesfull Categories Retrieval");
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                _logger.LogError(e.Message);
            }
            return response;
        }
    }
}
