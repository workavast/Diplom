namespace Avastrad.EventBusFramework
{
    public interface IEventReceiver<T> : IEventReceiverBase
        where T : struct, IEvent
    {
        public EventBusReceiverIdentifier EventBusReceiverIdentifier { get; }
        
        public void OnEvent(T e);
    }
}