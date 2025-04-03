using App.ScenesLoading;
using App.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Zenject;

namespace App.UI.SessionCreation
{
    [RequireComponent(typeof(Button))]
    public class ConnectToSessionHandler : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_InputField serverNameInput;
        
        [Inject] private SessionCreator _sessionCreator;
        
        private void Awake()
        {
            button.onClick.AddListener(ConnectToSession);
        }

        private void ConnectToSession()
        {
            if (serverNameInput.text.IsNullOrEmpty())
                return;
            
            _sessionCreator.ConnectToSession(serverNameInput.text, ScenesConfig.LobbySceneIndex);
        }
    }
}