using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using ArangkadaAPI.Services;
using AutoMapper;
using Moq;
using Xunit;

namespace ArangkadaTests.ServicesTests
{
    public class IEmailServicesTest : Profile
    {
        private readonly IEmailService _emailService;
        private readonly Mock<IEmailService> _fakeEmailService;

        public IEmailServicesTest()
        {
            _fakeEmailService = new Mock<IEmailService>();
            _emailService = _fakeEmailService.Object;
        }

        [Fact]
        public async Task SendEmailVerification_ValidEmailAddress_SendsEmail()
        {
            var emailAddress = "test@example.com";
            var verificationCode = "123456";

            _fakeEmailService.Setup(x => x.SendEmailVerification(emailAddress, verificationCode))
                             .Returns(Task.CompletedTask);

            await _emailService.SendEmailVerification(emailAddress, verificationCode);

            Assert.True(true);
        }

        [Fact]
        public async Task SendEmailVerification_InvalidEmailAddress_ThrowsException()
        {
            var emailAddress = "invalid@example.com";

            _fakeEmailService.Setup(x => x.SendEmailVerification(emailAddress, It.IsAny<string>()))
                             .Throws(new Exception("Simulated exception"));

            await Assert.ThrowsAsync<Exception>(async () => await _emailService.SendEmailVerification(emailAddress, "123456"));
        }
    }
}
