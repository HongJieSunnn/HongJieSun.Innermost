namespace Innermost.Architecture.Exceptions
{

    [Serializable]
    public class CommandHandleFailedException : Exception
    {
        public CommandHandleFailedException() { }
        public CommandHandleFailedException(string message) : base(message) { }
        public CommandHandleFailedException(string message, Exception inner) : base(message, inner) { }
        protected CommandHandleFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
