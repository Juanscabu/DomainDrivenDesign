using AutoMapper;
using EcommerceProject.Application.DTO;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Application.Interface.Infrastructure;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Application.Validator;
using EcommerceProject.Domain.Entities;
using EcommerceProject.Domain.Events;
using EcommerceProject.Transversal.Common;
using System.Collections.Generic;

namespace EcommerceProject.Application.Feature.Discounts
{
    public class DiscountsApplication : IDiscountsApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        private readonly DiscountDtoValidator _discountDtoValidator;

        public DiscountsApplication(IUnitOfWork unitOfWork, IMapper mapper, DiscountDtoValidator discountDtoValidator, IEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _discountDtoValidator = discountDtoValidator;
            _eventBus = eventBus;
        }

        public async Task<Response<bool>> Create(DiscountDto discountDto, CancellationToken cancellationToken = default)
        {
            var response = new Response<bool>();
            try
            {
                var validator = await _discountDtoValidator.ValidateAsync(discountDto,cancellationToken);
                if (!validator.IsValid) 
                {
                    response.Message = "Validation Errors";
                    response.Errors = validator.Errors;
                    return response;

                }
                var discount = _mapper.Map<Discount>(discountDto);
                await _unitOfWork.Discounts.InsertAsync(discount);
                response.Data = await _unitOfWork.Save(cancellationToken) > 0;
                if (response.Data) 
                {
                    response.IsSuccess = true;
                    response.Message = "Succesfull Registry";

                    //Event Publisher
                    var discountCreatedEvent = _mapper.Map<DiscountCreatedEvent>(discount);
                    _eventBus.Publish(discountCreatedEvent);
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<Response<bool>> Update(DiscountDto discountDto, CancellationToken cancellationToken = default)
        {
            var response = new Response<bool>();
            try
            {
                var validator = await _discountDtoValidator.ValidateAsync(discountDto, cancellationToken);
                if (!validator.IsValid)
                {
                    response.Message = "Validation Errors";
                    response.Errors = validator.Errors;
                    return response;

                }
                var discount = _mapper.Map<Discount>(discountDto);
                await _unitOfWork.Discounts.UpdateAsync(discount);
                response.Data = await _unitOfWork.Save(cancellationToken) > 0;
                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = "Succesfull Update";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<Response<bool>> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = new Response<bool>();
            try
            {
                await _unitOfWork.Discounts.DeleteAsync(id.ToString());
                response.Data = await _unitOfWork.Save(cancellationToken) > 0;
                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = "Succesfull Delete";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<Response<DiscountDto>> Get(int id, CancellationToken cancellationToken = default)
        {     
            var response = new Response<DiscountDto>();
            try
            {
                var discount = await _unitOfWork.Discounts.GetAsync(id.ToString());
                if (discount is null) 
                { 
                    response.IsSuccess = true;
                    response.Message = "There is no discount for that Id";
                    return response;
                }
                
                response.Data = _mapper.Map<DiscountDto>(discount);
                response.IsSuccess = true;
                response.Message = "Succesfull Query";
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<Response<IEnumerable<DiscountDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var response = new Response<IEnumerable<DiscountDto>>();
            try
            {
                var discounts = await _unitOfWork.Discounts.GetAllAsync(cancellationToken);
                response.Data = _mapper.Map<List<DiscountDto>>(discounts);
                if (response.Data != null)
                { 
                response.IsSuccess = true;
                response.Message = "Succesfull Query";
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
            }

            return response;
        }

    }
}
