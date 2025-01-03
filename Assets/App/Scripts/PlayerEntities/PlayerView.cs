using UnityEngine;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerView : MonoBehaviour
    {
        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float moveSpeed, float gravity, float deltaTime)
        {
            var gravityVelocity = gravity * deltaTime * Vector3.up;
            var moveVelocity = moveSpeed * deltaTime * direction.normalized;
            _characterController.Move(gravityVelocity + moveVelocity);
        }

        public void SetLookPoint(Vector3 newLookPoint) 
            => gameObject.transform.rotation = Quaternion.LookRotation(newLookPoint - transform.position);
    }
}