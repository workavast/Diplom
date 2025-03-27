using UnityEngine;

namespace App.Entities
{
    public class AnimController : MonoBehaviour
    {
        [SerializeField] private SolderView solderView;
        [SerializeField] private Animator animator;
        
        [Tooltip("Debug")]
        [SerializeField] private bool useDebug;
        [SerializeField, Range(-1, 1)] private float velocityX;
        [SerializeField, Range(-1, 1)] private float velocityY;

        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityY = Animator.StringToHash("VelocityY");

        private void LateUpdate()
        {
            if (useDebug)
            {
                animator.SetFloat(VelocityX, velocityX);
                animator.SetFloat(VelocityY, velocityY);   
            }
            else
            {
                animator.SetFloat(VelocityX, solderView.AnimationVelocity.x);
                animator.SetFloat(VelocityY, solderView.AnimationVelocity.y);
            }
        }
    }
}