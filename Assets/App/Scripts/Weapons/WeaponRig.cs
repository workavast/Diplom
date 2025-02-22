using System.Collections;
using UnityEngine;

namespace App.Weapons
{
    public class WeaponRig : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Default()
        {
            animator.speed = 1;
            animator.Play("Default");
        }

        /// <param name="initialTime">Value between 0 and 1</param>
        public void Reloading(float duration, float initialTime)
        {
            animator.Play("Reloading", -1, initialTime);
            StartCoroutine(AdjustSpeed(duration));
        }

        private IEnumerator AdjustSpeed(float duration)
        {
            yield return null;

            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var length = stateInfo.length;
    
            animator.speed = length / duration;
        }
    }
}