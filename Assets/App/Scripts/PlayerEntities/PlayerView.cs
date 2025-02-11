using App.Entities;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerView : MonoBehaviour
    {
        public Vector2 Velocity { get; private set; }

        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float moveSpeed, float gravity, float deltaTime, float sprintSpeed)
        {
            var gravityVelocity = gravity * deltaTime * Vector3.up;
            var unscaledVelocity = moveSpeed * direction;
            var moveVelocity = unscaledVelocity * deltaTime;
            _characterController.Move(gravityVelocity + moveVelocity);

            var animationVelocity = transform.InverseTransformDirection(unscaledVelocity.normalized);
            animationVelocity *= moveSpeed * direction.magnitude / sprintSpeed;
            Velocity = animationVelocity.XZ();
        }

        public void SetLookPoint(Vector3 newLookPoint) 
            => gameObject.transform.rotation = Quaternion.LookRotation(newLookPoint - transform.position);
    }
}