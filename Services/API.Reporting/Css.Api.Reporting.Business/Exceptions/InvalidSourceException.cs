using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.Exceptions
{
    public class InvalidSourceException : Exception
    {
        public InvalidSourceException(string message) : base(message)
        {

        }
    }
}
