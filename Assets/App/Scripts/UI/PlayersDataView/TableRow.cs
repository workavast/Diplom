using System;
using App.Players.SessionDatas;
using Avastrad.PoolSystem;
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

        private bool _isInitialized;
        private NetPlayerSessionData _sessionData;
        
        public event Action<TableRow> ReturnElementEvent;
        public event Action<TableRow> DestroyElementEvent;
        
        public void Initialize(NetPlayerSessionData sessionData)
        {
            if (_isInitialized)
            {
                _sessionData.OnDespawned -= ReturnElementInPool;
                _sessionData.OnNickNameChanged -= UpdateRow;
                _sessionData.OnPointsChanged -= UpdateRow;
                _sessionData.OnKillsChanged -= UpdateRow;
                _sessionData.OnDeathsChanged -= UpdateRow;
            }
            
            _isInitialized = true;
            _sessionData = sessionData;
            _sessionData.OnDespawned += ReturnElementInPool;
            
            if (gameObject.activeInHierarchy)
            {
                _sessionData.OnNickNameChanged += UpdateRow;
                _sessionData.OnPointsChanged += UpdateRow;
                _sessionData.OnKillsChanged += UpdateRow;
                _sessionData.OnDeathsChanged += UpdateRow;
            
                UpdateRow();    
            }
        }

        private void UpdateRow()
        {
            nickName.text = _sessionData.NickName.Value;
            points.text = _sessionData.Points.ToString();
            kills.text = _sessionData.Kills.ToString();
            deaths.text = _sessionData.Deaths.ToString();
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
            
            _sessionData.OnDespawned -= ReturnElementInPool;
          
            _sessionData.OnNickNameChanged -= UpdateRow;
            _sessionData.OnPointsChanged -= UpdateRow;
            _sessionData.OnKillsChanged -= UpdateRow;
            _sessionData.OnDeathsChanged -= UpdateRow;
            
            _sessionData = null;

            if (gameObject != null)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!_isInitialized)
                return;
            
            _sessionData.OnNickNameChanged += UpdateRow;
            _sessionData.OnPointsChanged += UpdateRow;
            _sessionData.OnKillsChanged += UpdateRow;
            _sessionData.OnDeathsChanged += UpdateRow;
            
            UpdateRow();
        }

        private void OnDisable()
        {
            if (!_isInitialized)
                return;
            
            _sessionData.OnNickNameChanged -= UpdateRow;
            _sessionData.OnPointsChanged -= UpdateRow;
            _sessionData.OnKillsChanged -= UpdateRow;
            _sessionData.OnDeathsChanged -= UpdateRow;
        }

        private void OnDestroy()
        {
            DestroyElementEvent?.Invoke(this);
        }
    }
}