using System;
using System.Diagnostics.CodeAnalysis;

namespace Codibly.EmailService.Api.Services.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ValidationException : Exception
    {
        #region Construction

        public ValidationException() : base("The data provided is not correct")
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        #endregion
    }
}
