using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PictureGallery.CustomeExceptions
{
    [Serializable]
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException()
        {
        }

        public DuplicateItemException(string message) : base(message)
        {
        }

        public DuplicateItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
