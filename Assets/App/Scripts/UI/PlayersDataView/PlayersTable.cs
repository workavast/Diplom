using System.Linq;
using App.Players.Repository;
using App.Players.SessionData;
using App.Players.SessionData.Global;
using App.Pvp.Gameplay;
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

        [Inject] private IPlayersSessionDataRepository<NetGlobalSessionData> _globalSessionDataRep;
        [Inject] private IPlayersSessionDataRepository<NetGameplaySessionData> _gameplaySessionDataRep;
        [Inject] private IReadOnlyPlayersRepository _playersRepository;
        
        private Pool<TableRow> _pool;

        public void Awake()
        {
            _pool = new Pool<TableRow>(Instantiate);
        }
        
        private void OnEnable()
        {
            _playersRepository.OnPlayerJoined += OnPlayerAdded;
            CheckNewPlayers();
        }

        private void OnDisable()
        {
            _playersRepository.OnPlayerJoined -= OnPlayerAdded;
        }

        private void CheckNewPlayers()
        {
            var playersRefs = _playersRepository.Players;

            foreach (var playerRef in playersRefs)
            {
                if (_pool.BusyElements.All(row => row.PlayerRef != playerRef))
                    OnPlayerAdded(playerRef);
            }
        }
        
        private void OnPlayerAdded(PlayerRef playerRef)
        {
            var globalSessionData = _globalSessionDataRep.GetData(playerRef);
            var gameplaySessionData = _gameplaySessionDataRep.GetData(playerRef);
            
            _pool.ExtractElement(out var row);
            row.Initialize(playerRef, globalSessionData, gameplaySessionData);
        }

        private TableRow Instantiate() 
            => Instantiate(tableRowPrefab, rowsHolder);
    }
}