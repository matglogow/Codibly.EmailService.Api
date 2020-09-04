using System;

namespace Codibly.EmailService.Api.Services.Exceptions
{
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
