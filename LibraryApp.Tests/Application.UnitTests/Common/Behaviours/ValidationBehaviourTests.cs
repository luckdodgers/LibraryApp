using FluentValidation;
using FluentValidation.Results;
using LibraryApp.Application.Application.Common.Models;
using LibraryApp.Application.Common.Behaviours;
using LibraryApp.Application.Common.Models;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApp.Tests.Application.UnitTests
{
    public class ValidationBehaviourTests
    {
        private readonly Mock<AbstractValidator<IRequest<IRequestResult>>> _validator = new Mock<AbstractValidator<IRequest<IRequestResult>>>();
        private readonly Mock<IRequest<IRequestResult>> _command = new Mock<IRequest<IRequestResult>>();
        private readonly Mock<IRequestResult> _okRequestResult = new Mock<IRequestResult>();
        private readonly CancellationToken _cancelToken = new CancellationToken();

        public ValidationBehaviourTests()
        {
            _okRequestResult.SetupGet(ok => ok.Succeeded).Returns(true);
        }

        [Fact]
        public async Task SendInvalidData_ShouldReturnFailRequestResult()
        {
            _validator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<IRequest<IRequestResult>>>()))
                .Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure(string.Empty, string.Empty) } ));
            var _validatorsList = new List<IValidator>() { _validator.Object };
            var behaviour = new ValidationBehaviour<IRequest<IRequestResult>, IRequestResult>(_validatorsList);
            RequestHandlerDelegate<IRequestResult> nextDelegate = () => Task.FromResult(_okRequestResult.Object);

            var result = await behaviour.Handle(_command.Object, _cancelToken, nextDelegate);

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task SendValidData_ShouldReturnSuccessRequestResult()
        {
            _validator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<IRequest<IRequestResult>>>()))
                .Returns(new ValidationResult());
            var _validatorsList = new List<IValidator>() { _validator.Object };
            var behaviour = new ValidationBehaviour<IRequest<IRequestResult>, IRequestResult>(_validatorsList);
            RequestHandlerDelegate<IRequestResult> nextDelegate = () => Task.FromResult(_okRequestResult.Object);

            var result = await behaviour.Handle(_command.Object, _cancelToken, nextDelegate);

            Assert.True(result.Succeeded);
        }
    }
}
