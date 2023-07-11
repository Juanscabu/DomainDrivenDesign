using EcommerceProject.Application.DTO;
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
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountsApplication _discountsApplication;

        public DiscountsController(IDiscountsApplication discountsApplication)
        {
            _discountsApplication = discountsApplication;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DiscountDto discountDto)
        {
            if (discountDto == null)
                return BadRequest();

            var response = await _discountsApplication.Create(discountDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

         [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDto discountDto)
        {
            var discountDtoExists = await _discountsApplication.Get(id);
            if (discountDtoExists.Data == null)
                return NotFound(discountDtoExists);
            if (discountDtoExists == null)
                return BadRequest();

            var response = await _discountsApplication.Update(discountDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _discountsApplication.Delete(id);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _discountsApplication.Get(id);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _discountsApplication.GetAll();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
