using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BlackRed.Game.PlayerEntities
{
    public class PlayersRepository
    {
        private readonly Dictionary<PlayerRef, PlayerView> _playerViews = new();

        public event Action<PlayerRef, PlayerView> OnPlayerAdd;
        public event Action<PlayerRef> OnPlayerRemove;
        
        public void Add(PlayerRef playerRef, PlayerView playerView)
        {
            if (_playerViews.TryGetValue(playerRef, out var view))
            {
                if (view == null)
                {
                    _playerViews[playerRef] = playerView;
                }
                else
                {
                    Debug.LogError($"Duplicate exception: {playerRef} | {playerView}");
                    return;
                }
            }
            else
            {
                _playerViews.Add(playerRef, playerView);
            }
            
            OnPlayerAdd?.Invoke(playerRef, playerView);
        }

        public void Remove(PlayerRef playerRef)
        {
            _playerViews.Remove(playerRef);
            OnPlayerRemove?.Invoke(playerRef);
        }
    }
}