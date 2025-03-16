namespace App.FSM
{
    public abstract class State<T>
    {
        public abstract T Id { get; }
        
        public virtual void OnEnter(){} 
        public virtual void OnExit(){} 
        public virtual void OnUpdate(){} 
        public virtual void OnFixedUpdate(){} 
        public virtual void OnLateUpdate(){} 
    }
}