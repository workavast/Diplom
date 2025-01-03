namespace Avastrad.GameCycleFramework
{
    public interface IGameCycleController
    {
        public GameCycleState CurrentState { get; }
        
        public void AddListener(GameCycleState cycleState, IGameCycleUpdate iGameCycleUpdate);
        public void AddListener(GameCycleState cycleState, IGameCycleFixedUpdate iGameCycleFixedUpdate);
        public void AddListener(GameCycleState cycleState, IGameCycleEnter iGameCycleEnter);
        public void AddListener(GameCycleState cycleState, IGameCycleExit iGameCycleExit); 

        public void RemoveListener(GameCycleState cycleState, IGameCycleUpdate iGameCycleUpdate);
        public void RemoveListener(GameCycleState cycleState, IGameCycleFixedUpdate iGameCycleFixedUpdate);
        public void RemoveListener(GameCycleState cycleState, IGameCycleEnter iGameCycleEnter);
        public void RemoveListener(GameCycleState cycleState, IGameCycleExit iGameCycleExit); 
    }
}