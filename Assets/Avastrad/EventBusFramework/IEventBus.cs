namespace Avastrad.EventBusFramework
{
    public interface IEventBus
    {
        public void Subscribe<T>(IEventReceiver<T> receiver)
            where T : struct, IEvent;

        public void UnSubscribe<T>(IEventReceiver<T> receiver)
            where T : struct, IEvent;

        public void Invoke<T>(T @event)
            where T : struct, IEvent;
    }
}