using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Models;
using LibraryApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Services
{
    public class RequestErrorToStatusCode : IErrorToStatusCodeConverter
    {
        private readonly ILogger _logger;

        public RequestErrorToStatusCode(ILogger logger)
        {
            _logger = logger;
        }

        public int Convert(RequestError error)
        {
            switch (error)
            {
                case RequestError.ApplicationException:
                    return StatusCodes.Status500InternalServerError;

                case RequestError.AlreadyExists:
                    return StatusCodes.Status409Conflict;

                case RequestError.NotFound:
                    return StatusCodes.Status404NotFound;

                case RequestError.ValidationError:
                    return StatusCodes.Status400BadRequest;

                default:
                    _logger.LogWarning("No StatusCode for {errorType} found");
                    return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
