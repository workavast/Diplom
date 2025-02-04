using System;
using System.Linq;
using App.Players.SessionDatas;
using Avastrad.PoolSystem;
using Fusion;
using UnityEngine;
using Zenject;

namespace App.UI.PlayersDataView
{
    public class PlayersTable : MonoBehaviour
    {
        [SerializeField] private TableRow tableRowPrefab;
        [SerializeField] private Transform rowsHolder;

        private PlayerSessionDatasRepository _playerSessionDatasRepository;
        private Pool<TableRow> _pool;

        [Inject]
        public void Construct(PlayerSessionDatasRepository playerSessionDatasRepository)
        {
            _playerSessionDatasRepository = playerSessionDatasRepository;
        }
        
        public void Awake()
        {
            _pool = new Pool<TableRow>(Instantiate);
        }
        
        private void OnEnable()
        {
            _playerSessionDatasRepository.OnPlayerAdd += OnPlayerAdded;
            CheckNewPlayers();
        }

        private void OnDisable()
        {
            _playerSessionDatasRepository.OnPlayerAdd -= OnPlayerAdded;
        }

        private void CheckNewPlayers()
        {
            var playersRefs = _playerSessionDatasRepository.Datas.Keys;

            foreach (var playerRef in playersRefs)
            {
                if (_pool.BusyElements.All(row => row.PlayerRef != playerRef))
                    OnPlayerAdded(playerRef, _playerSessionDatasRepository.Datas[playerRef]);
            }
        }
        
        private void OnPlayerAdded(PlayerRef _, NetPlayerSessionData netPlayerSessionData)
        {
            _pool.ExtractElement(out var row);
            row.Initialize(netPlayerSessionData);
        }

        private TableRow Instantiate() 
            => Instantiate(tableRowPrefab, rowsHolder);
    }
}