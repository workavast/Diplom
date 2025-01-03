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
        
        public void Initialize()
        {
            _pool = new Pool<TableRow>(Instantiate);
            _playerSessionDatasRepository.OnPlayerAdd += OnPlayerAdded;
        }
        
        private void OnDestroy() 
            => _playerSessionDatasRepository.OnPlayerAdd -= OnPlayerAdded;

        private void OnPlayerAdded(PlayerRef _, NetPlayerSessionData netPlayerSessionData)
        {
            _pool.ExtractElement(out var row);
            row.Initialize(netPlayerSessionData);
        }
        
        private TableRow Instantiate() 
            => Instantiate(tableRowPrefab, rowsHolder);
    }
}