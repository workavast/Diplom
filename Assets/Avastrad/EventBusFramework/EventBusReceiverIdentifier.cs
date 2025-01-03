namespace Avastrad.EventBusFramework
{
    public class EventBusReceiverIdentifier
    {
        private static int _index;

        private int? _id;
        
        public int Id => _id ??= _index++;
    }
}