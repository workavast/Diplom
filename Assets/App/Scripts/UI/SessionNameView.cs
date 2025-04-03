using App.NetworkRunning;
using Fusion;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.UI
{
    public class SessionNameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionName;
        
        [Inject] private NetworkRunnerProvider _networkRunnerProvider;
        
        private void Awake()
        {
            if (_networkRunnerProvider.TryGetNetworkRunner(out var netRunner))
            {
                if (netRunner.GameMode == GameMode.Single) 
                    Hide();
                else
                    sessionName.text = netRunner.SessionInfo.Name;
            }
            else
            {
                Hide();
            }
        }

        private void Hide() 
            => gameObject.SetActive(false);
    }
}