using AutoMapper;
using EcommerceProject.Application.Interface;
using EcommerceProject.Application.Main;
using EcommerceProject.Application.Validator;
using EcommerceProject.Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EcommerceProject.Application.Test
{
    public class UsersApplicationTest
    {

        private static Mock<IConfiguration> _configuration;
        private static Mock<IUsersDomain> _usersDomain;
        private static Mock<IMapper> _mapper;
        private static UsersDtoValidator _usersDtoValidator;
        private static IServiceScopeFactory _scopeFactory;

        public UsersApplicationTest()
        {
            _configuration = new Mock<IConfiguration>();
            _usersDomain = new Mock<IUsersDomain>();
            _mapper = new Mock<IMapper>();
            
        }

        [Fact]
        public void Authenticate_NoParams_ReturnErrorValidation()
        {
            //Arrange
            var userName = string.Empty;
            var password = string.Empty;
            var expected = "Validation errors";

            _usersDtoValidator = new UsersDtoValidator();

            //Act 
            IUsersApplication usersApplicationTest = new UsersApplication(_usersDomain.Object, _mapper.Object, _usersDtoValidator);
            var result = usersApplicationTest.Authenticate(userName, password);
            var actual = result.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}