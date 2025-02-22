using Avastrad.Vector2Extension;
using UnityEngine;

namespace App.Entities.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Vector2 AnimationVelocity { get; private set; }

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