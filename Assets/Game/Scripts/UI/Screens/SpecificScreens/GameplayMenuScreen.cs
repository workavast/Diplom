using System;
using Avastrad.UI.UiSystem;
using BlackRed.Game.UI.PlayersDataView;
using UnityEngine;

namespace BlackRed.Game.UI
{
    public class GameplayMenuScreen : ScreenBase
    {
        [SerializeField] private PlayersTable playersTable;

        public override void Initialize()
        {
            base.Initialize();

            if (playersTable == null)
            {
                Debug.LogWarning($"You forgot serialize {nameof(playersTable)}");
                playersTable = GetComponentInChildren<PlayersTable>();
                
                if (playersTable == null)
                    throw new NullReferenceException($"Cant find {nameof(playersTable)}");
            }
            
            playersTable.Initialize();
        }
    }
}