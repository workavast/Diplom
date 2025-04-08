using App.Entities;
using Fusion.Addons.FSM;

namespace App.Ai.FSMs.Weapon
{
    public class WeaponState : State<WeaponState>
    {
        protected readonly NetEntity Entity;

        protected WeaponState(NetEntity entity)
        {
            Entity = entity;
        }
    }
}