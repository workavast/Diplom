using App.PlayerInput;
using UnityEngine;

namespace App.Entities.FSM.SpecificStates
{
    public class Alive : EntityState
    {
        public Alive(NetEntity netEntity) 
            : base(netEntity) { }

        protected override void OnFixedUpdate()
        {
            if (HealthPoints <= 0)
            {
                // NetController.TryActivateState<Dead>();
                return;
            }

            var hasInput = OwnerBehaviour.GetInput(out PlayerInputData input);
            if (hasInput)
            {
                NetEntity.RotateByLookDirection(input.LookDirection);

                var isSprint = input.Buttons.IsSet(PlayerButtons.Sprint);
                NetEntity.CalculateVelocity(input.HorizontalInput, input.VerticalInput, isSprint);
                
                if ((HasStateAuthority || HasInputAuthority) && input.Buttons.IsSet(PlayerButtons.Fire))
                    NetEntity.TryShoot();

                var reloadRequest = input.Buttons.IsSet(PlayerButtons.Reload) || NetEntity.RequiredReload;
                if ((HasStateAuthority || HasInputAuthority) && reloadRequest) 
                    NetEntity.TryReload();
            }
            
#if UNITY_EDITOR
            if (HasStateAuthority && Input.GetKeyDown(KeyCode.Q)) 
                NetEntity.TakeDamage(999, NetEntity);
#endif
        }
    }
}