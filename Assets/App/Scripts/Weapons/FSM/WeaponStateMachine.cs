using Fusion.Addons.FSM;

namespace App.Weapons.FSM
{
    public class WeaponStateMachine : StateMachine<WeaponState>
    {
        public WeaponStateMachine(string name, params WeaponState[] states)
            : base(name, states) { }
    }
}