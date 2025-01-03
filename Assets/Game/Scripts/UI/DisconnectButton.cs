using BlackRed.Game.DisconnectProviding;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlackRed.Game.UI
{
    [RequireComponent(typeof(Button))]
    public class DisconnectButton : MonoBehaviour
    {
        [Inject] private IDisconnectInvoker _disconnectInvoker;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Disconnect);
        }

        private void Disconnect()
            => _disconnectInvoker.Disconnect();
    }
}