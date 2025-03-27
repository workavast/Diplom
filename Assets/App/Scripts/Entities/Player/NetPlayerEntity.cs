using App.Armor;
using App.Players.Nicknames;
using Avastrad.EventBusFramework;
using Fusion;
using Zenject;

namespace App.Entities.Player
{
    public class NetPlayerEntity : NetEntity
    {
        public PlayerRef PlayerRef => Object.InputAuthority;
        public override EntityType EntityType => EntityType.Player;
        
        protected override IEventBus EventBus { get; set; }
        protected override ArmorsConfig ArmorsConfig { get; set; }

        private PlayersEntitiesRepository _playersEntitiesRepository;
        private NicknamesProvider _nicknamesProvider;
        
        [Inject]
        public void Construct(PlayersEntitiesRepository playersEntitiesRepository, NicknamesProvider nicknamesProvider, 
            IEventBus eventBus, ArmorsConfig armorsConfig)
        {
            _playersEntitiesRepository = playersEntitiesRepository;
            _nicknamesProvider = nicknamesProvider;
            EventBus = eventBus;
            ArmorsConfig = armorsConfig;
        }

        public override void Spawned()
        {
            base.Spawned();
            _playersEntitiesRepository.Add(Object.InputAuthority, this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            _playersEntitiesRepository.Remove(this);
        }

        public override string GetName() 
            => _nicknamesProvider.GetNickName(PlayerRef);
    }
}

