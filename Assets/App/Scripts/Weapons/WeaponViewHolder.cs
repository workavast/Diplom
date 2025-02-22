using System.Collections;
using App.Audio.Sources;
using UnityEngine;
using Zenject;

namespace App.Weapons
{
    public class WeaponViewHolder : MonoBehaviour
    {
        [SerializeField] private WeaponView weaponView;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSourceHolderPoolable reloadStartSfxPrefab;
        [SerializeField] private AudioSourceHolderPoolable reloadEndSfxPrefab;

        [Inject] private readonly AudioFactory _audioFactory;
        
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

        public void ReloadStartSfx() 
            => _audioFactory.Create(reloadStartSfxPrefab, transform.position);
        
        public void ReloadEndSfx() 
            => _audioFactory.Create(reloadEndSfxPrefab, transform.position);

        public void ShotVfx() 
            => weaponView.ShotVfx();

        public void ShotSfx()
            => weaponView.ShotSfx();
    }
}