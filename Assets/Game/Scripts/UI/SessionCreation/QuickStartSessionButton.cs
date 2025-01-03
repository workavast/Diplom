using BlackRed.Game.Session;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlackRed.Game.UI.SessionCreation
{
    [RequireComponent(typeof(Button))]
    public class QuickStartSessionButton : MonoBehaviour
    {
        [Inject] private SessionCreator _sessionCreator;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(QuickStart);
        }

        private void QuickStart() 
            => _sessionCreator.QuickStart();
    }
}