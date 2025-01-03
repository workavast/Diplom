using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace App.Players.SessionDatas
{
    public class PlayerSessionDatasRepository : IPlayerSessionDatasRepository
    {
        private readonly Dictionary<PlayerRef, NetPlayerSessionData> _datas = new();
        public IReadOnlyDictionary<PlayerRef, NetPlayerSessionData> Datas => _datas;

        public event Action<PlayerRef, NetPlayerSessionData> OnPlayerAdd;
        public event Action<PlayerRef> OnPlayerRemove;
        
        public void Add(PlayerRef playerRef, NetPlayerSessionData playerView)
        {
            if (_datas.TryGetValue(playerRef, out var view))
            {
                if (view == null)
                {
                    _datas[playerRef] = playerView;
                }
                else
                {
                    Debug.LogError($"Duplicate exception: {playerRef} | {playerView}");
                    return;
                }
            }
            else
            {
                _datas.Add(playerRef, playerView);
            }
            
            OnPlayerAdd?.Invoke(playerRef, playerView);
        }

        public void Remove(PlayerRef playerRef)
        {
            _datas.Remove(playerRef);
            OnPlayerRemove?.Invoke(playerRef);
        }
    }
}