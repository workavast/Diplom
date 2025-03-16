using System;
using App.Players.Nicknames;
using App.Players.SessionData.Global;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.UI
{
    public class NickNameView : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text nickNameView;
        
        private NicknamesProvider _nicknamesProvider;
        private NetGlobalSessionData _globalSessionData;

        public void SetSessionData(NicknamesProvider nicknamesProvider, NetGlobalSessionData globalSessionData)
        {
            _nicknamesProvider = nicknamesProvider;
            
            if (_globalSessionData != null)
            {
                _globalSessionData.OnNickNameChanged -= UpdateNickNameView;
                _globalSessionData.OnDespawned -= Dispose;
            }

            _globalSessionData = globalSessionData;
            _globalSessionData.OnNickNameChanged += UpdateNickNameView;
            _globalSessionData.OnDespawned += Dispose;

            UpdateNickNameView();
        }
        
        public void Dispose()
        {
            if (_globalSessionData != null)
            {
                _globalSessionData.OnNickNameChanged -= UpdateNickNameView;
                _globalSessionData.OnDespawned -= Dispose;
            }
        }
        
        private void UpdateNickNameView()
        {
            nickNameView.text = _nicknamesProvider.GetNickName(_globalSessionData.PlayerRef);
        }
    }
}