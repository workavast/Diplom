using BlackRed.Game.Session;
using BugStrategy.ScenesLoading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlackRed.Game.UI.SessionCreation
{
    [RequireComponent(typeof(Button))]
    public class CreateSessionHandler : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_InputField serverNameInput;
        
        [Inject] private SessionCreator _sessionCreator;
        
        private void Awake() 
            => button.onClick.AddListener(CreateSession);

        private void CreateSession() 
            => _sessionCreator.CreateSession(serverNameInput.text, ScenesConfig.GameplaySceneIndex);
    }
}