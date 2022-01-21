using System.Runtime.Serialization;

namespace Hulk.Shared.Exception
{
    public class HulkException : System.Exception
    {
        public HulkException(int code) : base(new LetterError(code).Message)
        {
        }

        public HulkException(int code, string objectValue)
            : base(new LetterError(code, null, null, objectValue).Message)
        {
        }

        public HulkException(int code, string objectValue, string extraValue)
            : base(new LetterError(code, null, null, objectValue, extraValue).Message)
        {
        }

        public HulkException(int code, string objectPath, string objectValue, string extraValue)
            : base(new LetterError(code, null, objectPath, objectValue, extraValue).Message)
        {
        }

        public HulkException()
        {
        }

        public HulkException(string message)
            : base(message)
        {
        }

        public HulkException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public HulkException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public HulkException(string format, System.Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected HulkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}