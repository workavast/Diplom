using System;
using System.Collections;
using UnityEngine;

namespace Avastrad.ScenesLoading
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float hideFadeTime = 1;
        [SerializeField] private float showFadeTime = 1;
        
        public bool IsShow { get; private set; }
        
        public event Action OnPreShow;
        public event Action OnHided;

        public void Initialize() 
            => IsShow = gameObject.activeSelf;

        public void Show(bool instantly, Action onShowedCallback)
        {
            OnPreShow?.Invoke();
            if (instantly)
                ShowInstantly(onShowedCallback);
            else
                ShowWithFade(onShowedCallback);
        }

        public void Hide(bool instantly)
        {
            if (instantly)
                HideInstantly();
            else
                HideWithFade();
        }

        private void ShowInstantly(Action onShowedCallback)
        {
            StopAllCoroutines();
            canvasGroup.alpha = 1;
            gameObject.SetActive(true);
            IsShow = true;
            onShowedCallback?.Invoke();
        }
        
        private void ShowWithFade(Action onShowedCallback)
        {
            StopAllCoroutines();
            IsShow = true;
            gameObject.SetActive(true);
            StartCoroutine(ShowFade(onShowedCallback));
        }
        
        private void HideInstantly()
        {
            StopAllCoroutines();
            IsShow = false;
            gameObject.SetActive(false);
            OnHided?.Invoke();
        }
        
        private void HideWithFade()
        {
            StopAllCoroutines();
            StartCoroutine(HideFade());
        }
        
        private IEnumerator ShowFade(Action onShowedCallback)
        {
            float timer = 0;

            while (timer < showFadeTime)
            {
                yield return new WaitForEndOfFrame();
                canvasGroup.alpha = timer/showFadeTime;
                timer += Time.unscaledDeltaTime;
            }
            
            canvasGroup.alpha = 1;
            onShowedCallback?.Invoke();
        }

        private IEnumerator HideFade()
        {
            float timer = 0;

            while (timer < hideFadeTime)
            {
                yield return new WaitForEndOfFrame();
                canvasGroup.alpha = 1 - timer/hideFadeTime;
                timer += Time.unscaledDeltaTime;
            }
            
            IsShow = false;
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            OnHided?.Invoke();
        }
    }
}