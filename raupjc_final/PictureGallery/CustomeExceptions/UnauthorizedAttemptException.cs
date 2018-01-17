using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PictureGallery.CustomeExceptions
{
    public class UnauthorizedAttemptException : Exception
    {
        public UnauthorizedAttemptException()
        {
        }

        public UnauthorizedAttemptException(string message) : base(message)
        {
        }

        public UnauthorizedAttemptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnauthorizedAttemptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
