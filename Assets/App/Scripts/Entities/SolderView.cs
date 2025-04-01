using System;
using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Entities
{
    public class SolderView : MonoBehaviour
    {
        [field:SerializeField] public bool IsAlive { get; private set; }
        public Vector2 AnimationVelocity { get; private set; }

        public event Action<bool> OnAliveStateChanged; 
        
        public void SetAliveState(bool isAlive)
        {
            if (IsAlive == isAlive)
                return;

            IsAlive = isAlive;
            OnAliveStateChanged?.Invoke(IsAlive);
        }

        public void MoveView(Vector3 unscaledVelocity, float sprintSpeed)
        {
            var animationVelocity = transform.InverseTransformDirection(unscaledVelocity.normalized);
            animationVelocity *= unscaledVelocity.magnitude / sprintSpeed;
            AnimationVelocity = animationVelocity.XZ();
        }

        public void SetLookPoint(Vector3 newLookPoint) 
            => gameObject.transform.rotation = Quaternion.LookRotation(newLookPoint - transform.position);
    }
}