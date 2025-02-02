using App.Session;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.SessionCreation
{
    [RequireComponent(typeof(Button))]
    public class SinglePlayerSessionButton : MonoBehaviour
    {
        [Inject] private SessionCreator _sessionCreator;
        
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(StartSinglePlayerSession);
        }

        private void StartSinglePlayerSession() 
            => _sessionCreator.CreateSinglePlayer(ScenesConfig.GameplaySceneIndex);
    }
}