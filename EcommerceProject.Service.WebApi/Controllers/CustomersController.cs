using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Service.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ICustomersApplication _customerApplication;

        public CustomersController(ICustomersApplication customerApplication)
        {
            _customerApplication = customerApplication;
        }

        #region sync methods

        [HttpPost]
        public IActionResult Insert([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest(); 

            var response = _customerApplication.Insert(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest();

            var response = _customerApplication.Update(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpDelete("{customerId}")]
        public IActionResult Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = _customerApplication.Delete(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("{customerId}")]
        public IActionResult Get(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = _customerApplication.Get(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _customerApplication.GetAll();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        #endregion

        /*
        #region async methods
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest();

            var response = await _customerApplication.InsertAsync(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest();

            var response = await _customerApplication.UpdateAsync(customerDto);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = await _customerApplication.DeleteAsync(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                return BadRequest();

            var response = await _customerApplication.GetAsync(customerId);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _customerApplication.GetAllAsync();
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response.Message);
        }
        #endregion
        */
    }
}
