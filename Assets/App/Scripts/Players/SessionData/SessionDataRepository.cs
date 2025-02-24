using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace App.Players.SessionData
{
    public abstract class SessionDataRepository<T> : IPlayersSessionDataRepository<T>
        where T : NetworkBehaviour
    {
        public IReadOnlyDictionary<PlayerRef, T> PlayersSessionData => _playerSessionDatas;
        
        private readonly Dictionary<PlayerRef, T> _playerSessionDatas = new(2);
        
        public event Action<PlayerRef, T> OnAdd;
        public event Action<PlayerRef> OnRemove;
        
        public bool ContainsKey(PlayerRef playerRef) 
            => _playerSessionDatas.ContainsKey(playerRef);

        public T GetData(PlayerRef playerRef) 
            => _playerSessionDatas[playerRef];

        public bool TryRegister(PlayerRef playerRef, T netPlayerSessionData)
        {
            if (_playerSessionDatas.ContainsKey(playerRef))
            {
                Debug.LogWarning($"You try register session data of the player that already register: {playerRef} | {playerRef.PlayerId}");
                return false;
            }
            
            _playerSessionDatas.Add(playerRef, netPlayerSessionData);
            OnAdd?.Invoke(playerRef, netPlayerSessionData);
            return true;
        }

        public bool TryRemove(PlayerRef playerRef)
        {
            if (_playerSessionDatas.ContainsKey(playerRef))
            {
                _playerSessionDatas.Remove(playerRef);
                OnRemove?.Invoke(playerRef);
                return true;
            }

            Debug.LogWarning($"Cant find this player: {playerRef} | {playerRef.PlayerId}");
            return false;
        }
    }
}