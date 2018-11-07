using System;

namespace VKApi.Model
{
    [Serializable]
    public class Exception2FAutorization : Exception
    {
        public Exception2FAutorization() { }
        public Exception2FAutorization(string message) : base(message) { }
        public Exception2FAutorization(string message, Exception inner) : base(message, inner) { }
        protected Exception2FAutorization(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ExceptionCapthaAutorization : Exception
    {
        public ExceptionCapthaAutorization() { }
        public ExceptionCapthaAutorization(string message) : base(message) { }
        public ExceptionCapthaAutorization(string message, Exception inner) : base(message, inner) { }
        protected ExceptionCapthaAutorization(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

