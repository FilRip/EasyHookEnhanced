using System;
using System.Runtime.Serialization;

namespace EasyHook
{
    [Serializable()]
    public class EasyHookException : Exception
    {
        public EasyHookException() : base() { }

        public EasyHookException(string message) : base(message) { }

        public EasyHookException(string message, Exception innerException) : base(message, innerException) { }

        protected EasyHookException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
