using App.ScenesLoading;
using Avastrad.ScenesLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI
{
    public class LoadSceneBtn : MonoBehaviour
    {
        [SerializeField] private SceneType scene;
        
        [Inject] private readonly ISceneLoader _sceneLoader;
        
        private void Awake() 
            => GetComponent<Button>().onClick.AddListener(LoadScene);

        private void LoadScene() 
            => _sceneLoader.LoadScene(scene.GetIndex());
    }
}