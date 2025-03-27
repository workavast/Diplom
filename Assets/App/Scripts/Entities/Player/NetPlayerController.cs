using App.PlayerInput;
using App.Weapons;
using Fusion;
using UnityEngine;

namespace App.Entities.Player
{
    public class NetPlayerController : NetworkBehaviour
    {
        [SerializeField] private NetEntity playerEntity;
        [SerializeField] private NetWeapon netWeapon;
        
        public override void FixedUpdateNetwork()
        {
            var hasInput = GetInput(out PlayerInputData input);
            if (hasInput)
            {
                playerEntity.RotateByLookDirection(input.LookDirection);

                var isSprint = input.Buttons.IsSet(PlayerButtons.Sprint);
                playerEntity.CalculateVelocity(input.HorizontalInput, input.VerticalInput, isSprint);
                
                if ((HasStateAuthority || HasInputAuthority) && input.Buttons.IsSet(PlayerButtons.Fire))
                    playerEntity.TryShoot();

                var reloadRequest = input.Buttons.IsSet(PlayerButtons.Reload) || netWeapon.RequiredReload;
                if ((HasStateAuthority || HasInputAuthority) && reloadRequest) 
                    playerEntity.TryReload();
            }
            
#if UNITY_EDITOR
            if (HasStateAuthority && Input.GetKeyDown(KeyCode.Q)) 
                playerEntity.TakeDamage(999, playerEntity);
#endif
        }
    }
}