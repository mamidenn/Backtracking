using System;
using System.Runtime.Serialization;

namespace Backtracking
{
    [Serializable]
    internal class BoardNotSolvableException : Exception
    {
        public BoardNotSolvableException()
        {
        }

        public BoardNotSolvableException(string message) : base(message)
        {
        }

        public BoardNotSolvableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoardNotSolvableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}