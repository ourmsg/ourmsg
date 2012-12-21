namespace Helper.ThreadingNET35
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class LockRecursionException : Exception
    {
        public LockRecursionException()
        {
        }

        public LockRecursionException(string message) : base(message)
        {
        }

        protected LockRecursionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LockRecursionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

