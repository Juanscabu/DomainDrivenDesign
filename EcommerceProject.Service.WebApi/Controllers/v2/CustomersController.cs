using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Service.WebApi.Controllers.v2
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CustomersController : Controller
    {
        private readonly ICustomersApplication _customerApplication;

        public CustomersController(ICustomersApplication customerApplication)
        {
            _customerApplication = customerApplication;
        }

        #region sync methods

        [HttpPost("Insert")]
        public IActionResult Insert([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest();

            var response = _customerApplication.Insert(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpPut("Update/{customerId}")]
        public IActionResult Update(string customerId, [FromBody] CustomerDto customersDto)
        {
            var customerDto = _customerApplication.Get(customerId);
            if (customerDto.Data == null)
                return NotFound(customerDto.Message);
            if (customerDto == null)
                return BadRequest();

            var response = _customerApplication.Update(customersDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpDelete("Delete/{customerId}")]
        public IActionResult Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = _customerApplication.Delete(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("Get/{customerId}")]
        public IActionResult Get(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = _customerApplication.Get(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = _customerApplication.GetAll();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAllWithPagination")]
        public IActionResult GetAllWithPagination([FromQuery] int pageNumber, int pageSize)
        {
            var response = _customerApplication.GetAllWithPagination(pageNumber, pageSize);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        #endregion

        #region async methods
        [HttpPost("InsertAsync")]
        public async Task<IActionResult> InsertAsync([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest();

            var response = await _customerApplication.InsertAsync(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpPut("UpdateAsync/{customerId}")]
        public async Task<IActionResult> UpdateAsync(string customerId, [FromBody] CustomerDto customersDto)
        {
            var customerDto = _customerApplication.Get(customerId);
            if (customerDto.Data == null)
                return NotFound(customerDto.Message);
            if (customerDto == null)
                return BadRequest();

            var response = await _customerApplication.UpdateAsync(customersDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpDelete("DeleteAsync/{customerId}")]
        public async Task<IActionResult> DeleteAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = await _customerApplication.DeleteAsync(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAsync/{customerId}")]
        public async Task<IActionResult> GetAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = await _customerApplication.GetAsync(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _customerApplication.GetAllAsync();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAllWithPaginationAsync")]
        public async Task<IActionResult> GetAllWithPaginationAsync([FromQuery] int pageNumber, int pageSize)
        {
            var response = await _customerApplication.GetAllWithPaginationAsync(pageNumber, pageSize);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }
        #endregion
    }
}
