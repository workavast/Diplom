using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Avastrad.EventBusFramework
{
    public class EventBus : IEventBus
    {
        /// <summary>
        /// key - eventType hash <br/>
        /// value(key) - receiver Id <br/>
        /// value(value) - receiver
        /// </summary>
        private readonly Dictionary<int, Dictionary<int, WeakReference<IEventReceiverBase>>> _receivers = new();
        
        /// <summary>
        /// list of refs that was un subscribed, in the same time with "Invoke()"
        /// </summary>
        private readonly List<NullRefData> _unSubscribers = new();

        //addList dont need cus "List.Add" add value to tail of list, in "Invoke()" we go from tail to head, so new refs dont be invoked anyway
        // private readonly Dictionary<int, object> _addList = new();
        private Type _currentInvokeType = null;

        public void Subscribe<T>(IEventReceiver<T> receiver)
            where T : struct, IEvent
        {
            var eventHashCode = typeof(T).Name.GetHashCode();
            var receiverId = receiver.EventBusReceiverIdentifier.Id;
            
            if (_receivers.TryGetValue(eventHashCode, out var receiversById))
            {
                if (receiversById.ContainsKey(receiverId))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"You try subscribe [{receiver}] that already subscribe to the event [{typeof(T).Name}]");
#endif
                    return;
                }
            }
            else
            {
                _receivers.Add(eventHashCode, new Dictionary<int, WeakReference<IEventReceiverBase>>());
            }
            
            var weakReference = new WeakReference<IEventReceiverBase>(receiver);
            _receivers[eventHashCode].Add(receiverId, weakReference);
        }

        public void UnSubscribe<T>(IEventReceiver<T> receiver)
            where T : struct, IEvent
        {
            var eventHashCode = typeof(T).Name.GetHashCode();
            var receiverId = receiver.EventBusReceiverIdentifier.Id;

            if (_receivers.TryGetValue(eventHashCode, out var receiversById)
                && receiversById.TryGetValue(receiverId, out var foundedReceiver))
            {
                if (typeof(T) == _currentInvokeType)
                {
                    foundedReceiver.SetTarget(null);
                    _unSubscribers.Add(new NullRefData(eventHashCode, receiverId));
                }
                else
                {
                    receiversById.Remove(receiverId);
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"You try unsub of event [{typeof(T).Name}] " +
                                 $"for that you was not subscribed [{receiver}]");
            }
#endif
        }

        public void Invoke<T>(T @event)
            where T : struct, IEvent
        {
            _currentInvokeType = typeof(T);
            var eventHashCode = _currentInvokeType.Name.GetHashCode();

            if (!_receivers.TryGetValue(eventHashCode, out var receiverPairs))
                return;

            var receivers = receiverPairs.ToList().AsReadOnly();
            
            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i].Value.TryGetTarget(out var target))
                {
                    var action = target as IEventReceiver<T>;
                    action?.OnEvent(@event);
                }
#if UNITY_EDITOR
                else
                {
                    if (_unSubscribers.Count <= 0 || !_unSubscribers.Any(nullRefData => 
                            nullRefData.EventHash == eventHashCode && nullRefData.ReceiverId == receivers[i].Key))
                        Debug.LogError($"ATTENTION: you are delete some reference," +
                                       $" but not unsubscribe him from event [{_currentInvokeType.Name}]");
                }
#endif
            }
            
            _currentInvokeType = null;
            DeleteNullRefs();
        }

        private void DeleteNullRefs()
        {
            foreach (var nullRef in _unSubscribers)
                _receivers[nullRef.EventHash].Remove(nullRef.ReceiverId);
            
            _unSubscribers.Clear();
        }
        
        private struct NullRefData
        {
            public int EventHash { get; }

            public int ReceiverId { get; }
            
            public NullRefData(int eventHash, int receiverId)
            {
                EventHash = eventHash;
                ReceiverId = receiverId;
            }
        }
    }
}