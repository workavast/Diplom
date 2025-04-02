using Fusion;
using Fusion.Addons.FSM;

namespace App.Weapons.FSM
{
    public abstract class WeaponState : State<WeaponState>
    {
        protected readonly NetWeaponModel NetWeaponModel;
        protected NetworkRunner Runner => NetWeaponModel.Runner;
        protected WeaponConfig WeaponConfig => NetWeaponModel.WeaponConfig; 
        
        protected WeaponState(NetWeaponModel netWeaponModel)
        {
            NetWeaponModel = netWeaponModel;
        }
        
        public abstract bool TryShot();
    }
}