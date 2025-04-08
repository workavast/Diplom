using App.Entities;
using UnityEngine;

namespace App.Ai.FSMs.Weapon
{
    public class Reload : WeaponState
    {
        public Reload(NetEntity entity) 
            : base(entity) { }

        protected override void OnEnterState()
        {
            Entity.TryReload();
        }

        protected override void OnFixedUpdate()
        {
            if (Entity.CurrentAmmo == Entity.MaxAmmo)
            {
                TryActivateState<Pause>();
                return;
            }
        }
    }
}