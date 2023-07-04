//using AutoMapper;
//using EcommerceProject.Application.Feature.Users;
//using EcommerceProject.Application.Interface.Features;
//using EcommerceProject.Application.Validator;
//using EcommerceProject.Domain.Entity;
//using Moq;
//using System;
//using Xunit;

//namespace EcommerceProject.Application.Test
//{
//    public class UsersApplicationTest
//    {

//        private static Mock<User> _usersDomain;
//        private static Mock<IMapper> _mapper;
//        private static UsersDtoValidator _usersDtoValidator;

//        public UsersApplicationTest()
//        {
//            _usersDomain = new Mock<User>();
//            _mapper = new Mock<IMapper>();

//        }

//        [Fact]
//        public void Authenticate_NoParams_ReturnErrorMessage()
//        {
//            //Arrange
//            var userName = string.Empty;
//            var password = string.Empty;
//            var expected = "Parameters can not be null";

//            _usersDtoValidator = new UsersDtoValidator();

//            //Act 
//            IUsersApplication usersApplicationTest = new UsersApplication(_usersDomain.Object, _mapper.Object, _usersDtoValidator);
//            var result = usersApplicationTest.Authenticate(userName, password);
//            var actual = result.Message;

//            //Assert
//            Assert.Equal(expected, actual);
//        }

//        [Fact]
//        public void Authenticate_CorrectParams_ReturnSuccessfullMessage()
//        {
//            //Arrange
//            var userName = "Juanscabu";
//            var password = "123";
//            var expected = "Successfull Authentication";
//            var user = new User();

//            _usersDtoValidator = new UsersDtoValidator();
//            _usersDomain.Setup(map => map.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
//                .Returns(user);

//            //Act 
//            IUsersApplication usersApplicationTest = new UsersApplication(_usersDomain.Object, _mapper.Object, _usersDtoValidator);
//            var result = usersApplicationTest.Authenticate(userName, password);
//            var actual = result.Message;

//            //Assert
//            Assert.Equal(expected, actual);
//        }

//        [Fact]
//        public void Authenticate_IncorrectParams_ReturnUserDoesNotExistMessage()
//        {
//            //Arrange
//            var userName = "Invalid User";
//            var password = "Invalid Password";
//            var expected = "Invalid Username or Password";

//            _usersDtoValidator = new UsersDtoValidator();
//            _usersDomain.Setup(map => map.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
//                .Throws<InvalidOperationException>();

//            //Act 
//            IUsersApplication usersApplicationTest = new UsersApplication(_usersDomain.Object, _mapper.Object, _usersDtoValidator);
//            var result = usersApplicationTest.Authenticate(userName, password);
//            var actual = result.Message;

//            //Assert
//            Assert.Equal(expected, actual);
//        }
//    }
//}