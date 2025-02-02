using System.Collections.Generic;
using Avastrad.ScenesLoading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace App.ScenesLoading
{
    public class SceneLoadingBackgroundChanger : MonoBehaviour
    {
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private Image backgroundHolder;
        [SerializeField] private List<Sprite> backgrounds;
        
        private void Awake()
        {
            loadingScreen.OnPreShow += UpdateBackground;
        }

        private void OnDestroy()
        {
            loadingScreen.OnPreShow -= UpdateBackground;
        }

        private void UpdateBackground()
        {
            var spriteIndex = Random.Range(0, backgrounds.Count);
            backgroundHolder.sprite = backgrounds[spriteIndex];
        }
    }
}