using UnityEngine;

namespace App.PlayerEntities
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private PlayerEntityConfig config;

        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            var gravityVelocity = config.Gravity * deltaTime * Vector3.up;
            var moveVelocity = config.MoveSpeed * deltaTime * direction.normalized;
            _characterController.Move(gravityVelocity + moveVelocity);
        }

        public void SetLookPoint(Vector3 newLookPoint) 
            => gameObject.transform.rotation = Quaternion.LookRotation(newLookPoint - transform.position);
    }
}