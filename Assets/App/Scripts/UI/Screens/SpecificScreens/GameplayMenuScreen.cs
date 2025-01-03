using System;
using App.UI.PlayersDataView;
using Avastrad.UI.UiSystem;
using UnityEngine;

namespace App.UI.Screens
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