using UnityEngine;

namespace BlackRed.Game.PlayerEntities
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float gravity = -9.8f;

        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            var gravityVelocity = gravity * deltaTime * Vector3.up;
            var moveVelocity = moveSpeed * deltaTime * direction.normalized;
            _characterController.Move(gravityVelocity + moveVelocity);
        }

        public void SetLookPoint(Vector3 newLookPoint) 
            => gameObject.transform.rotation = Quaternion.LookRotation(newLookPoint - transform.position);
    }
}