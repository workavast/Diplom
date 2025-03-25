using System;
using App.Players.SessionData.Global;
using App.Pvp.Gameplay;
using Avastrad.PoolSystem;
using Fusion;
using TMPro;
using UnityEngine;

namespace App.UI.PlayersDataView
{
    public class TableRow : MonoBehaviour, IPoolable<TableRow>
    {
        [SerializeField] private TMP_Text nickName;
        [SerializeField] private TMP_Text kills;
        [SerializeField] private TMP_Text deaths;
        [SerializeField] private TMP_Text points;

        public PlayerRef PlayerRef { get; private set; }

        private bool _isInitialized;
        private NetGameplaySessionData _gameplaySessionData;
        private NetGlobalSessionData _globalSessionData;
        
        public event Action<TableRow> ReturnElementEvent;
        public event Action<TableRow> DestroyElementEvent;
        
        public void Initialize(PlayerRef playerRef, NetGlobalSessionData globalSessionData, 
            NetGameplaySessionData gameplaySessionData)
        {
            if (_isInitialized)
            {
                _globalSessionData.OnDespawned -= ReturnElementInPool;
                _gameplaySessionData.OnDespawned -= ReturnElementInPool;
                
                _globalSessionData.OnNickNameChanged -= UpdateRow;
                
                _gameplaySessionData.OnPointsChanged -= UpdateRow;
                _gameplaySessionData.OnKillsChanged -= UpdateRow;
                _gameplaySessionData.OnDeathsChanged -= UpdateRow;
            }
            
            _isInitialized = true;
            _gameplaySessionData = gameplaySessionData;
            _globalSessionData = globalSessionData;

            PlayerRef = playerRef;
            
            _globalSessionData.OnDespawned += ReturnElementInPool;
            _gameplaySessionData.OnDespawned += ReturnElementInPool;
            
            if (gameObject.activeInHierarchy)
            {
                _globalSessionData.OnNickNameChanged += UpdateRow;
                
                _gameplaySessionData.OnPointsChanged += UpdateRow;
                _gameplaySessionData.OnKillsChanged += UpdateRow;
                _gameplaySessionData.OnDeathsChanged += UpdateRow;
            
                UpdateRow();    
            }
        }

        private void UpdateRow()
        {
            nickName.text = _globalSessionData.NickName.ToString();
            
            points.text = _gameplaySessionData.Points.ToString();
            kills.text = _gameplaySessionData.Kills.ToString();
            deaths.text = _gameplaySessionData.Deaths.ToString();
        }

        private void ReturnElementInPool() 
            => ReturnElementEvent?.Invoke(this);
        
        public void OnElementExtractFromPool()
        {
            gameObject.SetActive(true);
        }

        public void OnElementReturnInPool()
        {
            _isInitialized = false;
            
            _gameplaySessionData.OnDespawned -= ReturnElementInPool;
            _globalSessionData.OnDespawned -= ReturnElementInPool;
            
            _globalSessionData.OnNickNameChanged -= UpdateRow;
          
            _gameplaySessionData.OnPointsChanged -= UpdateRow;
            _gameplaySessionData.OnKillsChanged -= UpdateRow;
            _gameplaySessionData.OnDeathsChanged -= UpdateRow;
            
            _gameplaySessionData = null;

            if (gameObject != null)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!_isInitialized)
                return;
            
            _globalSessionData.OnNickNameChanged += UpdateRow;

            _gameplaySessionData.OnPointsChanged += UpdateRow;
            _gameplaySessionData.OnKillsChanged += UpdateRow;
            _gameplaySessionData.OnDeathsChanged += UpdateRow;
            
            UpdateRow();
        }

        private void OnDisable()
        {
            if (!_isInitialized)
                return;
            
            _globalSessionData.OnNickNameChanged -= UpdateRow;
            
            _gameplaySessionData.OnPointsChanged -= UpdateRow;
            _gameplaySessionData.OnKillsChanged -= UpdateRow;
            _gameplaySessionData.OnDeathsChanged -= UpdateRow;
        }

        public void OnDestroy()
        {
            DestroyElementEvent?.Invoke(this);
        }
    }
}