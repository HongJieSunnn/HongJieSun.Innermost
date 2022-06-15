[Serializable]
public class DomainEventHandleFailedException : Exception
{
    public DomainEventHandleFailedException() { }
    public DomainEventHandleFailedException(string message) : base(message) { }
    public DomainEventHandleFailedException(string message, Exception inner) : base(message, inner) { }
    protected DomainEventHandleFailedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}