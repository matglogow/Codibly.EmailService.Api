using System;

namespace Codibly.EmailService.Api.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        #region Construction

        public NotFoundException() : base("Item with requested id was not found")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        #endregion
    }
}
