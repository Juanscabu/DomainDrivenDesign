using EcommerceProject.Application.Interface.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EcommerceProject.Service.WebApi.Controllers.v2
{
    [Authorize]
    [EnableRateLimiting("fixedWindow")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesApplication _categoriesApplication;

        public CategoriesController(ICategoriesApplication categoriesApplication)
        {
            _categoriesApplication = categoriesApplication;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoriesApplication.GetAll();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }
    }
}
